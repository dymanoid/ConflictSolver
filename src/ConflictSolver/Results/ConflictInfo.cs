// <copyright file="ConflictInfo.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="members">A collection of members causing the conflict. Cannot be null.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="modName"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="members"/> is null.</exception>
        public ConflictInfo(string modName, IEnumerable<MemberAccessInfo> members)
        {
            if (string.IsNullOrEmpty(modName))
            {
                throw new ArgumentException("The mod name cannot be null or empty", nameof(modName));
            }

            if (members is null)
            {
                throw new ArgumentNullException(nameof(members));
            }

            ModName = modName;

            var memberList = members.ToList();
            memberList.Sort();
            ConflictingMembers = memberList;
            MemberCount = memberList.Count;
        }

        /// <summary>
        /// Gets the mod name causing the conflict.
        /// </summary>
        public string ModName { get; }

        /// <summary>
        /// Gets a collection of member names causing the conflict.
        /// </summary>
        public IEnumerable<MemberAccessInfo> ConflictingMembers { get; }

        /// <summary>
        /// Gets the number of items in the <see cref="ConflictingMembers"/> collection.
        /// </summary>
        public int MemberCount { get; }
    }
}
