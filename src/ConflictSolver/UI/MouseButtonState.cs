// <copyright file="MouseButtonState.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace ConflictSolver.UI
{
    /// <summary>
    /// Possible mouse button states.
    /// </summary>
    internal enum MouseButtonState
    {
        /// <summary>
        /// No particular state (or unknown).
        /// </summary>
        None,

        /// <summary>
        /// The mouse button was just pressed.
        /// </summary>
        Pressed,

        /// <summary>
        /// The mouse button is currently being held.
        /// </summary>
        Held,

        /// <summary>
        /// The mouse button was just released.
        /// </summary>
        Released,
    }
}
