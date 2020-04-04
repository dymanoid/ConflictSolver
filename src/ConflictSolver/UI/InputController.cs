// <copyright file="InputController.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using UnityEngine;

namespace ConflictSolver.UI
{
    /// <summary>
    /// The user input controller helper.
    /// </summary>
    internal static class InputController
    {
        /// <summary>
        /// Gets the current state of the mouse middle button.
        /// </summary>
        public static MouseButtonState MiddleMouseButtonState
        {
            get
            {
                if (Input.GetMouseButtonDown(2))
                {
                    return MouseButtonState.Pressed;
                }
                else if (Input.GetMouseButtonUp(2))
                {
                    return MouseButtonState.Released;
                }
                else if (Input.GetMouseButton(2))
                {
                    return MouseButtonState.Held;
                }

                return MouseButtonState.None;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the left mouse button was or is pressed.
        /// </summary>
        public static bool IsLeftMouseButtonPressed => Input.GetMouseButton(0);
    }
}
