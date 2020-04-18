// <copyright file="DictionaryExtensions.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace ConflictSolver.Tools
{
    /// <summary>
    /// Extension methods for a <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Gets a value from the <paramref name="dictionary"/> if it exists for the specified <paramref name="key"/>;
        /// otherwise, adds a new value to the <paramref name="dictionary"/> using the <paramref name="valueFactory"/>
        /// as the value source.
        /// </summary>
        /// <typeparam name="TKey">The type of dictionary's key.</typeparam>
        /// <typeparam name="TValue">The type of the values stored in the dictionary.</typeparam>
        /// <param name="dictionary">A dictionary to operate on.</param>
        /// <param name="key">The key to search for.</param>
        /// <param name="valueFactory">A delegate that will be called to produce a value if the <paramref name="key"/>
        /// is not found in the dictionary.</param>
        /// <returns>A value corresponding to the specified <paramref name="key"/>. Guaranteed to be stored
        /// in the dictionary.</returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            value = valueFactory();
            dictionary.Add(key, value);
            return value;
        }
    }
}
