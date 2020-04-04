// <copyright file="IGuiObject.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace ConflictSolver.UI
{
    /// <summary>
    /// An interface for the GUI objects of Unity.
    /// </summary>
    public interface IGuiObject
    {
        /// <summary>
        /// Called by Unity to draw the GUI elements of this object.
        /// </summary>
        void OnGUI();
    }
}
