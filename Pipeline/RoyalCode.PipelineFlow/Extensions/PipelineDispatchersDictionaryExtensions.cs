using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RoyalCode.PipelineFlow.Extensions
{
    internal static class PipelineDispatchersDictionaryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object GetOrCreate(this Dictionary<Type, object> dispatchers, Type requestType, MethodInfo method)
        {
            if (dispatchers.ContainsKey(requestType))
                return dispatchers[requestType];

            var dispatcher = method.MakeGenericMethod(requestType).Invoke(null, null)!;
            dispatchers[requestType] = dispatcher;
            return dispatcher;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static object GetOrCreate(
            this Dictionary<Type, object> dispatchers,
            Type requestType, Type outputType, MethodInfo method)
        {
            if (dispatchers.ContainsKey(requestType))
                return dispatchers[requestType];

            var dispatcher = method.MakeGenericMethod(requestType, outputType).Invoke(null, null)!;
            dispatchers[requestType] = dispatcher;
            return dispatcher;
        }
    }
}
