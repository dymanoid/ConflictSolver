// <copyright file="IModInfoProvider.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Reflection;

namespace ConflictSolver.Game
{
    /// <summary>
    /// An interface for a service that can provide various information about the mods.
    /// </summary>
    internal interface IModInfoProvider
    {
        /// <summary>
        /// Gets the mod name for a specified <paramref name="anyType"/> that is located in an assembly
        /// that belongs to the mod package. If the type does not belong to a mod, returns <c>null</c>.
        /// </summary>
        /// <param name="anyType">A <see cref="Type"/> that is contained in any assembly of a mod.</param>
        /// <returns>A string describing the mod name, or <c>null</c>.</returns>
        string GetModName(Type anyType);

        /// <summary>
        /// Determines whether the specified <paramref name="otherAssembly"/> assemblies belong to the same mod
        /// as the <paramref name="modAssembly"/>.
        /// </summary>
        /// <param name="modAssembly">The assembly that belongs to a particular mod.</param>
        /// <param name="otherAssembly">The other assembly to check.</param>
        /// <returns><c>true</c> if both assemblies belong to the same mod; otherwise, <c>false</c>.</returns>
        bool IsSameMod(Assembly modAssembly, Assembly otherAssembly);
    }
}
