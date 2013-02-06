using System;

namespace Crowbar
{
    /// <summary>
    /// Helper methods for delegates.
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// Chains/Combines delegates.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="source">The delegate that should be invoked first.</param>
        /// <param name="then">The delegate that should be invoked after the source.</param>
        /// <returns>A chained/combined delegate.</returns>
        public static Action<T> Then<T>(this Delegate source, Action<T> then)
        {
            return (Action<T>)Delegate.Combine(source, then);
        }


        /// <summary>
        /// Chains/Combines delegates.
        /// </summary>
        /// <typeparam name="T1">The first parameter type.</typeparam>
        /// <typeparam name="T2">The second parameter type.</typeparam>
        /// <param name="source">The delegate that should be invoked first.</param>
        /// <param name="then">The delegate that should be invoked after the source.</param>
        /// <returns>A chained/combined delegate.</returns>
        public static Action<T1, T2> Then<T1, T2>(this Delegate source, Action<T1, T2> then)
        {
            return (Action<T1, T2>)Delegate.Combine(source, then);
        }

        /// <summary>
        /// Invokes the delegate if its not null.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="action">The delegate to be invoked.</param>
        /// <param name="arg">The argument that should be passed on the delegate.</param>
        public static void TryInvoke<T>(this Action<T> action, T arg)
        {
            if (action != null)
            {
                action(arg);
            }
        }

        /// <summary>
        /// Invokes the delegate if its not null.
        /// </summary>
        /// <typeparam name="T1">The first parameter type.</typeparam>
        /// <typeparam name="T2">The second parameter type.</typeparam>
        /// <param name="action">The delegate to be invoked.</param>
        /// <param name="arg1">The first argument that should be passed on the delegate.</param>
        /// <param name="arg2">The second argument that should be passed on the delegate.</param>
        public static void TryInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action != null)
            {
                action(arg1, arg2);
            }
        }
    }
}