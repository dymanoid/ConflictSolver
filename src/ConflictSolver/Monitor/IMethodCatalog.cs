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
        /// Gets the <see cref="MethodInfo"/> descriptors of the <see cref="Type.GetField"/> overloads.
        /// </summary>
        /// <returns>A collection of the <see cref="MethodInfo"/> objects.</returns>
        IEnumerable<MethodInfo> GetMethodsForFieldQuery();

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> descriptors of the <see cref="Type.GetMethod"/> overloads.
        /// </summary>
        /// <returns>A collection of the <see cref="MethodInfo"/> objects.</returns>
        IEnumerable<MethodInfo> GetMethodsForMethodQuery();

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> descriptors of the <see cref="Type.GetProperty"/> overloads.
        /// </summary>
        /// <returns>A collection of the <see cref="MethodInfo"/> objects.</returns>
        IEnumerable<MethodInfo> GetMethodsForPropertyQuery();
    }
}
