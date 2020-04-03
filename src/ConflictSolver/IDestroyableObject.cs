// <copyright file="IDestroyableObject.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace ConflictSolver
{
    /// <summary>
    /// An interface for the Unity game objects that can be destroyed.
    /// </summary>
    internal interface IDestroyableObject
    {
        /// <summary>
        /// Destroys this object.
        /// </summary>
        void OnDestroy();
    }
}
