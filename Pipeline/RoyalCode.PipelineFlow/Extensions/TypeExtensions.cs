using System;

namespace RoyalCode.PipelineFlow.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks that one type implements another, working for generic types.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="other">The required implementation type.</param>
        /// <returns>
        /// <para>
        ///     True if <paramref name="type"/> implements <paramref name="other"/>, false otherwise.
        /// </para>
        /// <para>
        ///     This means that a variable of type '<paramref name="other"/>' can receive objects of type '<paramref name="type"/>'.
        /// </para>
        /// </returns>
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
        /// <para>
        ///     Checks if a data type is a certain generic type and returns the concrete generic type.
        /// </para>
        /// <para>
        ///     Adapted from Stackoverflow.
        /// </para>
        /// </summary>
        /// <param name="generic">The generic type.</param>
        /// <param name="toCheck">The to be checked.</param>
        /// <returns>The concrete generic type, or null case is not a generic subtype.</returns>
        public static Type? GetSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            // added, if it is an interface, it searches for interfaces of the type.
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
