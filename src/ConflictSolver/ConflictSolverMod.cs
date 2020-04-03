// <copyright file="ConflictSolverMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ICities;

namespace ConflictSolver
{
    /// <summary>
    /// The main Conflict Solver mod class.
    /// </summary>
    public sealed class ConflictSolverMod : LoadingExtensionBase, IUserMod
    {
        /// <summary>
        /// Gets the name of this mod.
        /// </summary>
        public string Name => "Conflict Solver";

        /// <summary>
        /// Gets the description of this mod.
        /// </summary>
        public string Description => "Detects possible mod conflicts. Automatically!";

        /// <summary>
        /// This method will be called by the game when this mod has to be disabled.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Will be called by the game")]
        public void OnDisabled() => GameConnection.DestroyInstance<Engine>(e => e.Shutdown());
    }
}
