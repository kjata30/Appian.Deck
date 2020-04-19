namespace Appian.ExtensionMethods
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides extension methods for <see cref="IList{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Swaps the position of two elements in an <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="list">The list containing the elements to swap.</param>
        /// <param name="i">The ith position of the array.</param>
        /// <param name="j">The jth position of the array.</param>
        /// <typeparam name="T">The type of object in the list.</typeparam>
        /// <returns>The array after the swap has occurred.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown when the index (i or j) provided is outside the range of the array.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown when the List is read-only.
        /// </exception>
        public static IList<T> Swap<T>(this IList<T> list, int i, int j)
        {
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
            return list;
        } 
    }
}