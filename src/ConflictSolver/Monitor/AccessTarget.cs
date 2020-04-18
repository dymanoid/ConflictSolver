// <copyright file="AccessTarget.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// Describes the categories of the access targets.
    /// </summary>
    internal enum AccessTarget
    {
        /// <summary>
        /// The category of the access target is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The access target is a type or member from an internal game assembly.
        /// </summary>
        Game,

        /// <summary>
        /// The access target is a type or member from a foreign mod assembly.
        /// </summary>
        ForeignMod,

        /// <summary>
        /// The access target is a type or member from own mod's assembly.
        /// </summary>
        OwnMod,
    }
}
