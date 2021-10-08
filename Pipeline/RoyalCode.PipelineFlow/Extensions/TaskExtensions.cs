using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    internal static class TaskExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetResultSynchronously<T>(this Task<T> task)
            => Task.Run(async () => await task.ConfigureAwait(false)).ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
