// <copyright file="IGameObject.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace ConflictSolver
{
    /// <summary>
    /// An interface for the Unity game objects.
    /// </summary>
    internal interface IGameObject
    {
        /// <summary>
        /// Called by the Unity Engine to perform the user-defined processing of this object.
        /// </summary>
        void Update();
    }
}
