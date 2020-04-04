// <copyright file="ModuleInitializer.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ConflictSolver.Game;
using ConflictSolver.Monitor;

namespace ConflictSolver
{
    /// <summary>
    /// A static class that holds a static method which will be called by the module initializer.
    /// </summary>
    internal static class ModuleInitializer
    {
        /// <summary>
        /// Initializes the conflict solver engine.
        /// </summary>
        public static void Initialize() => GameConnection.GetInstance<Engine>().Run();
    }
}
