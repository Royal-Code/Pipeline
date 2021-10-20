using RoyalCode.PipelineFlow.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Descriptors
{
    /// <summary>
    /// It contains the resolution of the generic input and output types in relation to the service of a method handler.
    /// </summary>
    public class GenericResolution
    {
        // deve conter uma relação/mapeamento dos parametros genéricos do serviço em relação
        // aos parâmetros genéricos do input e em relação ao output ou parâmetros genéricos do output.
        //
        // must contain a relationship/mapping of the generic service parameters
        // to the generic input parameters and to the output or generic output parameters.
        private readonly List<GenericParameterBinding> bindings;

        // se o método conter mais de um parâmetro, e algum destes outros parâmetros forem genéricos,
        // deverá ser criado uma resolução para os tipos genéricos deles
        // esta lista conterá a resolução.
        //
        // if the method contains more than one parameter, and some of these other parameters are generic,
        // a resolution must be created for their generic types this list will contain the resolution.
        private readonly List<GenericTypeArgumentsBinding>? methodGenericTypeParametersBindings;

        private readonly bool isAsync;
        private readonly bool hasOutput;
        private readonly MethodInfo handlerMethod;

        /// <summary>
        /// Creates a resolution of a generic service from the input and output that can be generic.
        /// </summary>
        /// <param name="inputType">The input type, the first parameter of the method.</param>
        /// <param name="outputType">The return type of the method.</param>
        /// <param name="isAsync">If it is asynchronous.</param>
        /// <param name="hasOutput">If it produces a result.</param>
        /// <param name="handlerMethod">Method Info.</param>
        /// <exception cref="InvalidOperationException">
        /// <para>
        ///     If there are generics in the input and it is not possible to map with any service generics.
        /// </para>
        /// <para>
        ///     If there are generics in the output and it is not possible to map with any service generics.
        /// </para>
        /// <para>
        ///     If any generic type of the service is not mapped to a generic of the input or output.
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

            var serviceGenerics = handlerMethod.DeclaringType.GetGenericArguments()
                .Where(arg => arg.IsGenericParameter)
                .ToArray();

            bindings = new List<GenericParameterBinding>();

            for (int i = 0; i < serviceGenerics.Length; i++)
            {
                var generic = serviceGenerics[i];
                bindings.Add(new GenericParameterBinding(i, generic));
            }

            var binding = bindings.FirstOrDefault(b => b.ServiceGenericType == inputType);
            if (binding is not null)
            {
                binding.Match = BindingMatch.ToInputType;
            }
            if (inputType.IsGenericType)
            {
                var inputGenerics = inputType.GetGenericArguments();
                for (int i = 0; i < inputGenerics.Length; i++)
                {
                    var inputGeneric = inputGenerics[i];
                    binding = bindings.FirstOrDefault(b => b.ServiceGenericType == inputGeneric);
                    if (binding is not null)
                    {
                        binding.Match = BindingMatch.ToInputGeneric;
                        binding.OtherIndex = i;
                    }
                    else
                    {
                        throw new NonResolvableInputTypeException(handlerMethod, inputType);
                    }
                }
            }

            if (hasOutput)
            {
                binding = bindings.FirstOrDefault(b => b.ServiceGenericType == outputType);
                if (binding is not null)
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
                        if (binding is not null)
                        {
                            binding.Match = BindingMatch.ToOutputGeneric;
                            binding.OtherIndex = i;
                        }
                        else if (outputGeneric.IsGenericParameter)
                        {
                            throw new NonResolvableOutputTypeException(handlerMethod, outputType);
                        }
                    }
                }
            }
            
            if (bindings.Any(b => b.Match == BindingMatch.None))
                throw new NonResolvableGenericParametersException(handlerMethod, inputType, outputType);

            var parameters = handlerMethod.GetParameters();
            for (int i = 1; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                if (parameter.ParameterType.IsGenericType)
                {
                    var gtab = new GenericTypeArgumentsBinding(parameter.ParameterType);
                    gtab.CheckGenericParameter(i, serviceGenerics);
                    methodGenericTypeParametersBindings ??= new();
                    methodGenericTypeParametersBindings.Add(gtab);
                }
            }
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

        private class GenericTypeArgumentsBinding
        {
            public int ParameterIndex { get; private set; }

            public Type GenericType { get; }

            public IEnumerable<GenericArgumentBinding> ArgumentBindings { get; }

            public GenericTypeArgumentsBinding(Type genericType)
            {
                GenericType = genericType ?? throw new ArgumentNullException(nameof(genericType));
                if (!genericType.IsGenericType)
                    throw new ArgumentException("A generic type is required", nameof(genericType));

                var argumentsBindigs = new List<GenericArgumentBinding>();
                var arguments = genericType.GetGenericArguments();
                for (int i = 0; i < arguments.Length; i++)
                {
                    var argument = arguments[i];
                    argumentsBindigs.Add(new GenericArgumentBinding(i, argument));
                }
                ArgumentBindings = argumentsBindigs;
            }

            public void CheckGenericParameter(int parameterIndex, Type[] serviceGenericArguments)
            {
                ParameterIndex = parameterIndex;
                foreach (var binding in ArgumentBindings)
                {
                    binding.CheckGenericParameter(serviceGenericArguments);
                }
            }

            public Type MakeParamterType(Type[] genericTypes)
            {
                var arguments = ArgumentBindings.Select(b => b.MakeArgumentType(genericTypes)).ToArray();
                var typeToMake = GenericType.IsConstructedGenericType ? GenericType.GetGenericTypeDefinition() : GenericType;
                return typeToMake.MakeGenericType(arguments);
            }
        }

        private class GenericArgumentBinding
        {
            public int Index { get; }

            public Type ArgumentType { get; }

            public int ServiceGenericParameterIndex { get; private set; } = -1;

            public bool IsGenericType => ArgumentType.IsGenericType;

            public bool IsGenericParameter => ArgumentType.IsGenericParameter;

            public GenericTypeArgumentsBinding? ArgumentsBinding { get; }

            public GenericArgumentBinding(int index, Type argumentType)
            {
                Index = index;
                ArgumentType = argumentType ?? throw new ArgumentNullException(nameof(argumentType));

                if (argumentType.IsGenericType)
                {
                    ArgumentsBinding = new GenericTypeArgumentsBinding(argumentType);
                }
            }

            public void CheckGenericParameter(Type[] serviceGenericArguments)
            {
                if (ArgumentsBinding is not null)
                {
                    ArgumentsBinding.CheckGenericParameter(-1, serviceGenericArguments);
                }
                else if (IsGenericParameter)
                {
                    for (int i = 0; i < serviceGenericArguments.Length; i++)
                    {
                        var argument = serviceGenericArguments[i];
                        if (argument == ArgumentType)
                        {
                            ServiceGenericParameterIndex = i;
                            break;
                        }
                    }
                }
            }

            public Type MakeArgumentType(Type[] genericTypes)
            {
                if (ArgumentsBinding is not null)
                    return ArgumentsBinding.MakeParamterType(genericTypes);
                if (IsGenericParameter)
                    return genericTypes[ServiceGenericParameterIndex];
                else
                    return ArgumentType;
            }
        }

        private enum BindingMatch
        {
            None,
            ToInputType,
            ToInputGeneric,
            ToOutputType,
            ToOutputGeneric,
        }

        /// <summary>
        /// Create the handler delegate, resolving the generic types from the atual input and output type.
        /// </summary>
        /// <param name="inputType">The atual input type.</param>
        /// <param name="outputType">The atual output type.</param>
        /// <returns></returns>
        public Delegate CreateDelegate(Type inputType, Type outputType)
        {
            var genericTypes = new Type[bindings.Count];

            // resolve the service type from input and output types.
            for (int i = 0; i < bindings.Count; i++)
            {
                var binding = bindings[i];
                switch (binding.Match)
                {
                    case BindingMatch.ToInputType:
                        genericTypes[i] = inputType;
                        break;
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

            var serviceType = genericTypes.Length > 0
                ? handlerMethod.DeclaringType.MakeGenericType(genericTypes)
                : handlerMethod.DeclaringType;

            // resolve real method parameters
            var parameters = handlerMethod.GetParameters();
            var parametersTypes = new Type[parameters.Length];
            parametersTypes[0] = inputType;
            if (parameters.Length > 1)
                for (int i = 1; i < parameters.Length; i++)
                {
                    var binding = methodGenericTypeParametersBindings?.FirstOrDefault(a => a.ParameterIndex == i);
                    parametersTypes[i] = binding is not null
                        ? binding.MakeParamterType(genericTypes)
                        : parameters[i].ParameterType;
                }

            // resolve the real method.
            var executableMethod = serviceType.GetRuntimeMethod(handlerMethod.Name, parametersTypes)!;

            // starting delegate creation
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
                hasOutput || isAsync ? executableMethod.ReturnType : null);

            // create the lambda for call the service method.
            var methodArgs = delegateParameters.Skip(1).ToArray();
            var body = Expression.Call(serviceParam, executableMethod, methodArgs);
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
                Array.Copy(methodParametersTypes, 0, parmsTypes, 1, methodParametersTypes.Length);
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
