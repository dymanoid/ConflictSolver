// <copyright file="ConflictInfo.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace ConflictSolver.Results
{
    /// <summary>
    /// A mod conflict information item.
    /// </summary>
    internal sealed class ConflictInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictInfo"/> class.
        /// </summary>
        /// <param name="modName">The conflicting mod name. Cannot be null or empty.</param>
        /// <param name="memberNames">A collection of members causing the conflict. Cannot be null.</param>
        public ConflictInfo(string modName, IEnumerable<string> memberNames)
        {
            if (string.IsNullOrEmpty(modName))
            {
                throw new ArgumentException("The mod name cannot be null or empty", nameof(modName));
            }

            ModName = modName;
            MemberNames = memberNames ?? throw new ArgumentNullException(nameof(memberNames));
        }

        /// <summary>
        /// Gets the mod name causing the conflict.
        /// </summary>
        public string ModName { get; }

        /// <summary>
        /// Gets a collection of member names causing the conflict.
        /// </summary>
        public IEnumerable<string> MemberNames { get; }
    }
}
