// <copyright file="IWindow.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using UnityEngine;

namespace ConflictSolver.UI
{
    /// <summary>
    /// An interface for a mod's window.
    /// </summary>
    internal interface IWindow
    {
        /// <summary>
        /// Gets a value indicating whether the mouse cursor is currently over this window.
        /// </summary>
        bool IsMouseOverWindow { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the window is currently visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets the current window's boundaries.
        /// </summary>
        Rect WindowBoundaries { get; }
    }
}
