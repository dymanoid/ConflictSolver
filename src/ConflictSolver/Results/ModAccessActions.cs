// <copyright file="ModAccessActions.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Reflection;
using ConflictSolver.Monitor;

namespace ConflictSolver.Results
{
    /// <summary>
    /// A container for member access actions performed by a particular <see cref="Type"/>.
    /// </summary>
    internal sealed class ModAccessActions : AccessActions<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModAccessActions"/> class.
        /// </summary>
        /// <param name="accessedMember">A <see cref="MemberInfo"/> this container stores the info for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="accessedMember"/> is null.</exception>
        public ModAccessActions(MemberInfo accessedMember)
            : base(accessedMember)
        {
        }

        /// <summary>
        /// Gets the names of all mods that caused any access actions to the member this container
        /// stores the info for.
        /// </summary>
        /// <returns>A collection of strings representing the mod names.</returns>
        public IEnumerable<string> GetModNames() => GetValues();
    }
}
