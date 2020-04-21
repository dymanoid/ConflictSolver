// <copyright file="ConflictInfoViewModel.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using ConflictSolver.Results;
using ConflictSolver.Tools;

namespace ConflictSolver.Views
{
    /// <summary>
    /// A viewmodel for the <see cref="ConflictInfo"/> class.
    /// </summary>
    internal sealed class ConflictInfoViewModel
    {
        private readonly ConflictInfo _conflict;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictInfoViewModel"/> class.
        /// </summary>
        /// <param name="conflict">The conflict information to create a viewmodel for.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="conflict"/> is null.</exception>
        public ConflictInfoViewModel(ConflictInfo conflict)
        {
            _conflict = conflict ?? throw new ArgumentNullException(nameof(conflict));
            Description = TextTools.GetNumberDescriptionText(conflict.MemberCount, Strings.Member);
            var memberNames = _conflict.ConflictingMembers.Select(m => m.ToString()).ToList();
            MemberNames = memberNames;
            AnyMembers = memberNames.Count > 0;
        }

        /// <summary>
        /// Gets the mod name causing the conflict.
        /// </summary>
        public string ModName => _conflict.ModName;

        /// <summary>
        /// Gets a collection of member names causing the conflict.
        /// </summary>
        public IEnumerable<string> MemberNames { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="MemberNames"/> collection contains any values.
        /// </summary>
        public bool AnyMembers { get; }

        /// <summary>
        /// Gets the description of this conflict item.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is collapsed or expanded.
        /// </summary>
        public bool IsExpanded { get; set; }
    }
}
