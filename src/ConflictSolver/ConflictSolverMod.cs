// <copyright file="ConflictSolverMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ConflictSolver.Game;
using ConflictSolver.Monitor;
using ConflictSolver.Views;
using ICities;

namespace ConflictSolver
{
    /// <summary>
    /// The main Conflict Solver mod class.
    /// </summary>
    public sealed class ConflictSolverMod : LoadingExtensionBase, IUserMod
    {
        private readonly PauseMenuExtension _pauseMenuExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictSolverMod"/> class.
        /// </summary>
        public ConflictSolverMod()
        {
            _pauseMenuExtension = new PauseMenuExtension(Name, ShowMainWindow);
        }

        /// <summary>
        /// Gets the name of this mod.
        /// </summary>
        public string Name => Strings.ModName;

        /// <summary>
        /// Gets the description of this mod.
        /// </summary>
        public string Description => Strings.ModDescription;

        /// <summary>
        /// This method will be called by the game when this mod has to be enabled.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Will be called by the game")]
        public void OnEnabled()
        {
            UnityEngine.Debug.Log($"{Strings.DebugLogPrefix} is now enabled");

            GameConnection.GetInstance<Engine>().Run();
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

        /// <summary>
        /// Called by the game after a city or a map has been loaded.
        /// </summary>
        /// <param name="mode">The game mode the city or map is loaded in.</param>
        public override void OnLevelLoaded(LoadMode mode) => _pauseMenuExtension.Enable();

        /// <summary>
        /// Called by the game just before a city or a map is unloaded.
        /// </summary>
        public override void OnLevelUnloading() => _pauseMenuExtension.Disable();

        private static void ShowMainWindow()
        {
            var mainWindow = GameConnection.GetInstance<MainWindow>();
            if (mainWindow != null)
            {
                mainWindow.IsVisible = true;
            }
        }
    }
}
