// <copyright file="ConflictSolverMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ConflictSolver.Game;
using ConflictSolver.Monitor;
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
        public string Name => Strings.ModName;

        /// <summary>
        /// Gets the description of this mod.
        /// </summary>
        public string Description => "Detects possible mod conflicts. Automatically!";

        /// <summary>
        /// This method will be called by the game when this mod has to be enabled.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Will be called by the game")]
        public void OnEnabled()
        {
            UnityEngine.Debug.Log($"{Strings.DebugLogPrefix} is now enabled");

            var mainWindow = GameConnection.GetInstance<MainWindow>();
            if (mainWindow == null)
            {
                UnityEngine.Debug.LogWarning($"{Strings.DebugLogPrefix} could not create its main window");
            }
            else
            {
                mainWindow.DataContext = new MainViewModel();
            }
        }

        /// <summary>
        /// This method will be called by the game when this mod has to be disabled.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Will be called by the game")]
        public void OnDisabled()
        {
            GameConnection.DestroyInstance<Engine>(e => e.Shutdown());
            GameConnection.DestroyInstance<MainWindow>();
        }
    }
}
