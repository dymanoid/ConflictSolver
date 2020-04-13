// <copyright file="ReflectionTools.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Linq;

namespace ConflictSolver.Tools
{
    /// <summary>
    /// A helper class for managing objects via Reflection.
    /// </summary>
    internal static class ReflectionTools
    {
        /// <summary>
        /// Copies the values of all public get-set properties of type <typeparamref name="TProperty"/>
        /// from the specified instance <paramref name="source"/> and sets those values on the <paramref name="target"/>
        /// object. The value copying will be performed using the <paramref name="valueFactory"/> delegate
        /// that will get the current property value from the <<paramref name="source"/> instance.
        /// </summary>
        /// <typeparam name="TClass">The type of the <paramref name="source"/> and <paramref name="target"/> instances.</typeparam>
        /// <typeparam name="TProperty">The type of the properties to consider.</typeparam>
        /// <param name="target">The target instance that will get the copied values.</param>
        /// <param name="source">The source instance that will provide the values for copying.</param>
        /// <param name="valueFactory">A delegate that will be used to create a copy of a property value.</param>
        /// <exception cref="ArgumentNullException">Thrown when any argument is null.</exception>
        public static void CopyPropertyValues<TClass, TProperty>(
            TClass target, TClass source, Func<TProperty, TProperty> valueFactory)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (valueFactory == null)
            {
                throw new ArgumentNullException(nameof(valueFactory));
            }

            var properties = typeof(TClass).GetProperties()
                .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(TProperty));

            foreach (var property in properties)
            {
                var sourceValue = (TProperty)property.GetValue(source, index: null);
                var targetValue = valueFactory(sourceValue);
                property.SetValue(target, targetValue, index: null);
            }
        }

        /// <summary>
        /// Copies the values of all public get-set properties from the specified instance <paramref name="source"/>
        /// and sets those values on the <paramref name="target"/> object. The values will be copied
        /// by simple assignment; thus, the properties of reference types of the <paramref name="target"/> instance
        /// will reference the same objects as on the <paramref name="source"/> instance. The value type values
        /// will be copied as usual.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="source"/> and <paramref name="target"/> instances.</typeparam>
        /// <param name="target">The target instance that will get the copied values.</param>
        /// <param name="source">The source instance that will provide the values for copying.</param>
        /// <exception cref="ArgumentNullException">Thrown when any argument is null.</exception>
        public static void CopyPropertyValues<T>(T target, T source)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var properties = typeof(T).GetProperties().Where(p => p.CanRead && p.CanWrite);
            foreach (var property in properties)
            {
                object value = property.GetValue(source, index: null);
                property.SetValue(target, value, index: null);
            }
        }
    }
}
