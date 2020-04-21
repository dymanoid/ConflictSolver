// <copyright file="MonitoredModViewModel.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using ConflictSolver.Monitor;
using ConflictSolver.Results;
using ConflictSolver.Tools;

namespace ConflictSolver.Views
{
    /// <summary>
    /// A viewmodel for an <see cref="MonitoredMod"/> class.
    /// </summary>
    internal sealed class MonitoredModViewModel
    {
        private readonly MonitoredMod _modInfo;
        private readonly List<MemberViewModel> _queriedMembers;
        private readonly List<MemberViewModel> _foreignQueriedMembers;
        private readonly string _queriedMembersCount;
        private readonly string _foreignQueriedMembersCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoredModViewModel"/> class.
        /// </summary>
        /// <param name="modInfo">The mod information item to create viewmodel for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="modInfo"/> is null.</exception>
        public MonitoredModViewModel(MonitoredMod modInfo)
        {
            _modInfo = modInfo ?? throw new ArgumentNullException(nameof(modInfo));
            Description = TextTools.GetNumberDescriptionText(modInfo.ConflictCount, Strings.PossibleConflict);
            var conflicts = modInfo.Conflicts.Select(c => new ConflictInfoViewModel(c)).ToList();
            AnyConflicts = conflicts.Count > 0;
            Conflicts = conflicts;
            _queriedMembers = _modInfo.QueriedMembers.Select(m => new MemberViewModel(in m)).ToList();
            _foreignQueriedMembers = _modInfo.QueriedMembers
                .Where(m => m.AccessTarget != AccessTarget.OwnMod)
                .Select(m => new MemberViewModel(in m)).ToList();
            _queriedMembersCount = "(" + _queriedMembers.Count + ")";
            _foreignQueriedMembersCount = "(" + _foreignQueriedMembers.Count + ")";
        }

        /// <summary>
        /// Gets the mod name.
        /// </summary>
        public string ModName => _modInfo.ModName;

        /// <summary>
        /// Gets a collection of member names that have been queried by this mod via Reflection.
        /// </summary>
        public IEnumerable<MemberViewModel> QueriedMembers => ShowOwnModQueries ? _queriedMembers : _foreignQueriedMembers;

        /// <summary>
        /// Gets the collection of the <see cref="ConflictInfoViewModel"/> objects that describe
        /// the mod conflicts this mod might cause.
        /// </summary>
        public IEnumerable<ConflictInfoViewModel> Conflicts { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Conflicts"/> collection contains any items.
        /// </summary>
        public bool AnyConflicts { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="QueriedMembers"/> collection contains any values.
        /// </summary>
        public bool AnyQueries => ShowOwnModQueries && _queriedMembers.Count > 0 || _foreignQueriedMembers.Count > 0;

        /// <summary>
        /// Gets or sets a value indicating whether this item is collapsed or expanded.
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="AccessTarget.OwnMod"/> items
        /// should be displayed.
        /// </summary>
        public bool ShowOwnModQueries { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="QueriedMembers"/> is collapsed or expanded.
        /// </summary>
        public bool IsReflectionListExpanded { get; set; }

        /// <summary>
        /// Gets the description of this item.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the description of the <see cref="QueriedMembers"/>.
        /// </summary>
        public string QueriesDescription => ShowOwnModQueries ? _queriedMembersCount : _foreignQueriedMembersCount;

        /// <summary>
        /// Expands all items for the current mod. The queried members sections will not
        /// change their state.
        /// </summary>
        public void ExpandAll() => ChangeExpandedState(expanded: true);

        /// <summary>
        /// Collapses all items and sub-items for the current mod, including the queried members sections.
        /// </summary>
        public void CollapseAll()
        {
            ChangeExpandedState(expanded: false);
            IsReflectionListExpanded = false;
        }

        private void ChangeExpandedState(bool expanded)
        {
            IsExpanded = expanded;
            foreach (var conflict in Conflicts)
            {
                conflict.IsExpanded = expanded;
            }
        }
    }
}
