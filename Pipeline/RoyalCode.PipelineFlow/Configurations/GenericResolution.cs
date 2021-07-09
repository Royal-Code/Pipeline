using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Configurations
{
    /// <summary>
    /// Contém a resolução dos tipos genéricos do input e output em relação ao serviço de um method handler.
    /// </summary>
    public class GenericResolution
    {
        // deve conter uma relação/mapeamento dos parametros genéricos do serviço em relação
        // aos parâmetros genéricos do input e em relação ao output ou parâmetros genéricos do output.
        private readonly List<GenericParameterBinding> bindings;

        private readonly bool isAsync;
        private readonly bool hasOutput;
        private readonly MethodInfo handlerMethod;

        /// <summary>
        /// Cria uma resolução de um serviço genérico a partir dos input e output que podem ser genéricos.
        /// </summary>
        /// <param name="inputType">O tipo de entrada, o prmeiro parâmetro do método.</param>
        /// <param name="outputType">O tipo de retorno do método.</param>
        /// <param name="isAsync">Se é assíncrono.</param>
        /// <param name="hasOutput">Se produz um resultado.</param>
        /// <param name="handlerMethod">Info do método.</param>
        /// <exception cref="InvalidOperationException">
        /// <para>
        ///     Caso haja genéricos no input e não for possível mapear com algum genérico do serviço.
        /// </para>
        /// <para>
        ///     Caso haja genéricos no output e não for possível mapear com algum genérico do serviço.
        /// </para>
        /// <para>
        ///     Caso algum tipo genérico do serviço não for mapeado para um genérico do input ou output.
        /// </para>
        /// </exception>
        public GenericResolution(Type inputType, Type outputType, 
            bool isAsync, bool hasOutput, MethodInfo handlerMethod)
        {
            if (inputType is null)
                throw new ArgumentNullException(nameof(inputType));
            if (outputType is null)
                throw new ArgumentNullException(nameof(outputType));

            this.isAsync = isAsync;
            this.hasOutput = hasOutput;
            this.handlerMethod = handlerMethod ?? throw new ArgumentNullException(nameof(handlerMethod));

            var serviceGenerics = handlerMethod.DeclaringType.GetGenericArguments();
            bindings = new List<GenericParameterBinding>();

            for (int i = 0; i < serviceGenerics.Length; i++)
            {
                var generic = serviceGenerics[i];
                bindings.Add(new GenericParameterBinding(i, generic));
            }

            if (inputType.IsGenericType)
            {
                var inputGenerics = inputType.GetGenericArguments();
                for (int i = 0; i < inputGenerics.Length; i++)
                {
                    var inputGeneric = inputGenerics[i];
                    var binding = bindings.FirstOrDefault(b => b.ServiceGenericType == inputGeneric);
                    if (binding != null)
                    {
                        binding.Match = BindingMatch.ToInputGeneric;
                        binding.OtherIndex = i;
                    }
                    else
                    {
                        throw new InvalidOperationException("TODO create exception for case");
                    }
                }
            }

            if (hasOutput)
            {
                var binding = bindings.FirstOrDefault(b => b.ServiceGenericType == outputType);
                if (binding != null)
                {
                    binding.Match = BindingMatch.ToOutputType;
                }
                else if (outputType.IsGenericType)
                {
                    var outputGenerics = outputType.GetGenericArguments();
                    for (int i = 0; i < outputGenerics.Length; i++)
                    {
                        var outputGeneric = outputGenerics[i];
                        binding = bindings.FirstOrDefault(b => b.ServiceGenericType == outputGeneric);
                        if (binding != null)
                        {
                            binding.Match = BindingMatch.ToOutputGeneric;
                            binding.OtherIndex = i;
                        }
                        else
                        {
                            throw new InvalidOperationException("TODO create exception for case");
                        }
                    }
                }
            }

            if (bindings.Any(b => b.Match == BindingMatch.None))
                throw new InvalidOperationException("TODO create exception for case");
        }

        private class GenericParameterBinding
        {
            public int Index { get; }

            public Type ServiceGenericType { get; }

            public BindingMatch Match { get; set; }

            public int OtherIndex { get; set; }

            public GenericParameterBinding(int index, Type serviceGenericType)
            {
                Index = index;
                ServiceGenericType = serviceGenericType ?? throw new ArgumentNullException(nameof(serviceGenericType));
            }
        }

        private enum BindingMatch
        {
            None,
            ToInputGeneric,
            ToOutputType,
            ToOutputGeneric,
        }

        public Delegate CreateDelegate(Type inputType, Type outputType)
        {
            var genericTypes = new Type[bindings.Count];

            // resolver o typo do serviço a partir do tipo do input e output.
            for (int i = 0; i < bindings.Count; i++)
            {
                var binding = bindings[i];
                switch (binding.Match)
                {
                    case BindingMatch.ToInputGeneric:
                        genericTypes[i] = inputType.GetGenericArguments()[binding.OtherIndex];
                        break;
                    case BindingMatch.ToOutputType:
                        genericTypes[i] = outputType;
                        break;
                    case BindingMatch.ToOutputGeneric:
                        genericTypes[i] = outputType.GetGenericArguments()[binding.OtherIndex];
                        break;
                }
            }

            var serviceType = handlerMethod.DeclaringType.MakeGenericType(genericTypes);

            // pametros do método
            var parameters = handlerMethod.GetParameters();
            var parametersTypes = new Type[parameters.Length];
            parametersTypes[0] = inputType;
            if (parameters.Length > 1)
                for (int i = 1; i < parameters.Length; i++)
                {
                    parametersTypes[i] = parameters[i].ParameterType;
                }

            // resolver o método real.
            var executableMethod = serviceType.GetRuntimeMethod(handlerMethod.Name, parametersTypes);

            // criar o delegate.
            List<ParameterExpression> delegateParameters = new();

            var serviceParam = Expression.Parameter(serviceType, "service");
            delegateParameters.Add(serviceParam);

            foreach (var paramType in parametersTypes)
            {
                var paramExpression = Expression.Parameter(paramType);
                delegateParameters.Add(paramExpression);
            }
            
            // make the delegate type
            Type delegateType = MakeDelegateType(
                serviceType,
                parametersTypes, 
                (hasOutput || isAsync) ? executableMethod.ReturnType : null);

            // create the lambda for call the service method.
            var body = Expression.Call(serviceParam, executableMethod, delegateParameters.Skip(1));
            var lambda = Expression.Lambda(delegateType, body, delegateParameters);
            var methodDelegate = lambda.Compile();

            return methodDelegate;
        }

        private Type MakeDelegateType(Type serviceType, Type[] methodParametersTypes, Type? returnType = null)
        {
            var totalParameters = methodParametersTypes.Length + 1;

            if (returnType is null)
            {
                var parmsTypes = new Type[totalParameters];
                parmsTypes[0] = serviceType;
                Array.Copy(methodParametersTypes, 0, parmsTypes, 1, methodParametersTypes.Length);

                return totalParameters switch
                {
                    2 => typeof(Action<,>).MakeGenericType(parmsTypes),
                    3 => typeof(Action<,,>).MakeGenericType(parmsTypes),
                    _ => throw new InvalidOperationException(
                        $"Can not create the delegate for service '{serviceType}'. " +
                        $"Number of parameters: '{methodParametersTypes.Length}', and return type is void"),
                };
            }
            else
            {
                var parmsTypes = new Type[totalParameters + 1];
                parmsTypes[0] = serviceType;
                Array.Copy(methodParametersTypes, 1, parmsTypes, 1, methodParametersTypes.Length);
                parmsTypes[parmsTypes.Length - 1] = returnType;

                return totalParameters switch
                {
                    2 => typeof(Func<,,>).MakeGenericType(parmsTypes),
                    3 => typeof(Func<,,,>).MakeGenericType(parmsTypes),
                    4 => typeof(Func<,,,,>).MakeGenericType(parmsTypes),
                    _ => throw new InvalidOperationException(
                        $"Can not create the delegate for service '{serviceType}'. " +
                        $"Number of parameters: '{methodParametersTypes.Length}', and return type is {returnType}"),
                };
            }
        }
    }
}
