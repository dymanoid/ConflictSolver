// <copyright file="IAssemblyCheck.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Reflection;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// An interface for a helper service for determining the assembly categories.
    /// </summary>
    internal interface IAssemblyCheck
    {
        /// <summary>
        /// Determines whether the specified <paramref name="assembly"/> is an original game assembly.
        /// </summary>
        /// <param name="assembly">The assembly to check. Cannot be null.</param>
        /// <returns><c>true</c> if the specified <paramref name="assembly"/> is a game assembly;
        /// otherwise, <c>false</c>.</returns>
        bool IsGameAssembly(Assembly assembly);

        /// <summary>
        /// Determines whether the specified <paramref name="assembly"/> is a system assembly, like mscorlib.
        /// </summary>
        /// <param name="assembly">The assembly to check. Cannot be null.</param>
        /// <returns><c>true</c> if the specified <paramref name="assembly"/> is a system assembly;
        /// otherwise, false.</returns>
        bool IsSystemAssembly(Assembly assembly);

        /// <summary>
        /// Determines whether the specified <paramref name="assembly"/> is a Harmony Library assembly.
        /// </summary>
        /// <param name="assembly">The assembly to check. Cannot be null.</param>
        /// <returns><c>true</c> if the specified <paramref name="assembly"/> is a Harmony Library assembly;
        /// otherwise, <c>false</c>.</returns>
        bool IsHarmony(Assembly assembly);

        /// <summary>
        /// Determines whether the specified <paramref name="assembly"/> is a user mod assembly.
        /// </summary>
        /// <param name="assembly">The assembly to check. Cannot be null.</param>
        /// <returns><c>true</c> if the specified <paramref name="assembly"/> is a user mod assembly;
        /// <c>false</c> if the <paramref name="assembly"/> is a game assembly.</returns>
        bool IsUserModAssembly(Assembly assembly);

        /// <summary>
        /// Determines whether the specified <paramref name="assembly"/> is a dynamically generated assembly.
        /// </summary>
        /// <param name="assembly">The assembly to check. Cannot be null.</param>
        /// <returns><c>true</c> if the specified <paramref name="assembly"/> is a dynamically generated assembly;
        /// otherwise, <c>false</c>.</returns>
        bool IsDynamic(Assembly assembly);
    }
}
