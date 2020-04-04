// <copyright file="IModInfoProvider.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;

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
    }
}
