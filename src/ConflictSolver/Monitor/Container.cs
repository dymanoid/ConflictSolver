// <copyright file="Container.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// A container that can hold a <typeparamref name="TKey"/> as a key value
    /// and a collection of <typeparamref name="TValue"/> objects
    /// mapped to their metadata of type <typeparamref name="TValueMetadata"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key value.</typeparam>
    /// <typeparam name="TValue">The type of the value objects.</typeparam>
    /// <typeparam name="TValueMetadata">The type of the value's metadata.</typeparam>
    internal abstract class Container<TKey, TValue, TValueMetadata>
        where TKey : class
        where TValue : class
    {
        private readonly Dictionary<TValue, TValueMetadata> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="Container{TKey, TValue, TValueMetadata}"/> class.
        /// </summary>
        /// <param name="key">A key value for this extended container.</param>
        protected Container(TKey key)
        {
            Key = key;
            _data = new Dictionary<TValue, TValueMetadata>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Container{TKey, TValue, TValueMetadata}"/> class
        /// and copies the data from the specified <paramref name="other"/> instance.
        /// </summary>
        /// <param name="other">A <see cref="Container{TKey, TValue, TValueMetadata}"/>
        /// to copy the data from.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="other"/> is null.</exception>
        protected Container(Container<TKey, TValue, TValueMetadata> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            _data = new Dictionary<TValue, TValueMetadata>(other._data);
            Key = other.Key;
        }

        /// <summary>
        /// Gets the key object of this extended container.
        /// </summary>
        protected TKey Key { get; }

        /// <summary>
        /// Stores the data in this container for the specified <paramref name="value"/>
        /// using the <paramref name="processor"/> delegate for additional data manipulation.
        /// The <paramref name="processor"/> will get either the already existing <typeparamref name="TValueMetadata"/>
        /// value for the specified <paramref name="value"/>, or the default <typeparamref name="TValueMetadata"/> value
        /// if no data is found.
        /// </summary>
        /// <param name="value">The value to store in this container.</param>
        /// <param name="processor">A delegate for additional data processing.</param>
        protected void StoreData(TValue value, Func<TValueMetadata, TValueMetadata> processor)
        {
            _data.TryGetValue(value, out var storedAccess);
            _data[value] = processor(storedAccess);
        }

        /// <summary>
        /// Gets the data stored in this container.
        /// </summary>
        /// <returns>A collection of key-value pairs stored in this container.</returns>
        protected IEnumerable<KeyValuePair<TValue, TValueMetadata>> GetData() => _data;

        /// <summary>
        /// Gets all values this container holds.
        /// </summary>
        /// <returns>A collection of the <typeparamref name="TValue"/> items.</returns>
        protected IEnumerable<TValue> GetValues() => _data.Keys;
    }
}
