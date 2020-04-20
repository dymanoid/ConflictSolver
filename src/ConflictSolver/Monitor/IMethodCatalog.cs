// <copyright file="IMethodCatalog.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Reflection;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// An interface for a catalog of <see cref="MethodInfo"/> instances that need to be patched to collect
    /// Reflection requests.
    /// </summary>
    internal interface IMethodCatalog
    {
        /// <summary>
        /// Gets all <see cref="MethodInfo"/> descriptors of this catalog.
        /// </summary>
        /// <returns>A collection of the <see cref="MethodInfo"/> objects.</returns>
        IEnumerable<MethodInfo> GetAllMethods();

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> descriptors of the methods that generate
        /// the monitored <paramref name="reflectionData"/>.
        /// </summary>
        /// <param name="reflectionData">The type of the reflection data of interest.</param>
        /// <returns>A collection of the <see cref="MethodInfo"/> objects.</returns>
        IEnumerable<MethodInfo> GetMethods(ReflectionData reflectionData);
    }
}
