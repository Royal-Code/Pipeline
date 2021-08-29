using System;
using System.Linq;
using System.Reflection;

namespace RoyalCode.PipelineFlow.Extensions
{
    public static class TypeExtensions
    {

        public static bool Implements(this Type type, Type other)
        {
            if (type == other)
                return true;

            if (other.IsGenericType && other.IsGenericTypeDefinition)
            {
                var closeGeneric = other.GetSubclassOfRawGeneric(type);

                if (closeGeneric is null)
                {
                    return false;
                }

                return true;
            }
            else
            {
                return other.IsAssignableFrom(type);
            }
        }

        /// <summary>
        /// TODO: docs em ingles.
        /// 
        /// Verifica se um tipo de dado é um determinado tipo genérico e retorna o tipo genérico concreto.
        /// Copiado do Stackoverflow.
        /// </summary>
        /// <param name="generic">Tipo genérico.</param>
        /// <param name="toCheck">Tipo a ser verificado.</param>
        /// <returns>O tipo genérico concreto, ou nulo caso não é um subtipo gerérico.</returns>
        public static Type? GetSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            // adicionado, se for uma interface, busca pelas interfaces do tipo.
            if (generic.IsInterface)
            {
                foreach (var interfaceToCheck in toCheck.GetInterfaces())
                {
                    var cur = interfaceToCheck.IsGenericType
                        ? interfaceToCheck.GetGenericTypeDefinition()
                        : interfaceToCheck;

                    if (generic == cur)
                    {
                        return interfaceToCheck;
                    }
                }
            }

            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return toCheck;
                }
                toCheck = toCheck.BaseType;
            }
            return null;
        }
    }
}
