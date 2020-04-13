// <copyright file="MonitoredMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace ConflictSolver.Results
{
    /// <summary>
    /// A data item class representing a mod that has been monitored.
    /// </summary>
    internal sealed class MonitoredMod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoredMod"/> class.
        /// </summary>
        /// <param name="modName">The mod name. Cannot be null or empty.</param>
        /// <param name="queriedMembers">A collection of the member names this mod has queried via Reflection.</param>
        /// <param name="conflicts">A collection of conflict information items for this mod.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="modName"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="queriedMembers"/>
        /// or <paramref name="conflicts"/> is null.</exception>
        public MonitoredMod(string modName, IEnumerable<string> queriedMembers, IEnumerable<ConflictInfo> conflicts)
        {
            if (string.IsNullOrEmpty(modName))
            {
                throw new ArgumentException("The mod name cannot be null or empty", nameof(modName));
            }

            if (queriedMembers is null)
            {
                throw new ArgumentNullException(nameof(queriedMembers));
            }

            if (conflicts is null)
            {
                throw new ArgumentNullException(nameof(conflicts));
            }

            ModName = modName;
            var conflictList = conflicts.ToList();
            Conflicts = conflictList;
            ConflictCount = conflictList.Count;

            var queriesList = queriedMembers.ToList();
            QueriedMembers = queriesList;
            QueriesCount = queriesList.Count;
        }

        /// <summary>
        /// Gets the mod name.
        /// </summary>
        public string ModName { get; }

        /// <summary>
        /// Gets a collection of <see cref="ConflictInfo"/> items describing the possible
        /// conflicts this mod might cause with other mods.
        /// </summary>
        public IEnumerable<ConflictInfo> Conflicts { get; }

        /// <summary>
        /// Gets the number ob items in the <see cref="Conflicts"/> collection.
        /// </summary>
        public int ConflictCount { get; }

        /// <summary>
        /// Gets a collection of member names that have been queried by this mod via Reflection.
        /// </summary>
        public IEnumerable<string> QueriedMembers { get; }

        /// <summary>
        /// Gets the number of items in the <see cref="QueriedMembers"/> collection.
        /// </summary>
        public int QueriesCount { get; }
    }
}
