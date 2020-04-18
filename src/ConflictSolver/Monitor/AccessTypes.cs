// <copyright file="AccessTypes.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// The type of the access action performed with a member.
    /// </summary>
    [Flags]
    internal enum AccessTypes
    {
        /// <summary>
        /// The member was not accessed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The member information was queried via Reflection.
        /// </summary>
        Query = 1,

        /// <summary>
        /// The member value was read.
        /// </summary>
        Read = 2,

        /// <summary>
        /// The member value was written.
        /// </summary>
        Write = 4,
    }
}
