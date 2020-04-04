// <copyright file="DataView.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConflictSolver.Game;

namespace ConflictSolver.Results
{
    /// <summary>
    /// The data view for collected Reflection queries.
    /// </summary>
    internal sealed class DataView
    {
        private readonly Dictionary<MemberInfo, HashSet<string>> _usages = new Dictionary<MemberInfo, HashSet<string>>();
        private readonly Dictionary<string, HashSet<MemberInfo>> _queries = new Dictionary<string, HashSet<MemberInfo>>();

        private readonly IDictionary<MemberInfo, HashSet<Type>> _source;
        private readonly IModInfoProvider _modInfoProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataView"/> class.
        /// </summary>
        /// <param name="source">The data source to use for this data view.</param>
        /// <param name="modInfoProvider">A service that provides the mod information.</param>
        public DataView(IDictionary<MemberInfo, HashSet<Type>> source, IModInfoProvider modInfoProvider)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _modInfoProvider = modInfoProvider ?? throw new ArgumentNullException(nameof(modInfoProvider));
        }

        /// <summary>
        /// Prepares this data view by consolidating the data from the source.
        /// </summary>
        public void Prepare()
        {
            foreach (var sourceItem in _source)
            {
                var queriedMember = sourceItem.Key;
                var requestingTypes = sourceItem.Value;

                foreach (var type in requestingTypes)
                {
                    string modName = _modInfoProvider.GetModName(type);
                    AddItem(_usages, queriedMember, modName);
                    AddItem(_queries, modName, queriedMember);
                }
            }
        }

        /// <summary>
        /// Gets the names of the mods that have been monitored in this session.
        /// </summary>
        /// <returns>A collection of strings representing the mod names.</returns>
        public IEnumerable<string> GetMonitoredModNames() => _queries.Keys.OrderBy(v => v);

        /// <summary>
        /// Gets the descriptive names of the members a mod with the specified <paramref name="modName"/> queried
        /// during the monitoring phase.
        /// </summary>
        /// <param name="modName">The name of the mod to get the queried members for.</param>
        /// <returns>A collection of strings describing the queried members.</returns>
        public IEnumerable<string> GetQueriedMembers(string modName)
        {
            if (string.IsNullOrEmpty(modName))
            {
                throw new ArgumentException("The mod name cannot be null or empty", nameof(modName));
            }

            return _queries.TryGetValue(modName, out var queriedMembers)
                ? queriedMembers.Select(m => m.DeclaringType.FullName + " -> " + m.ToString())
                : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets the conflicts information for the specified <paramref name="modName"/>.
        /// </summary>
        /// <param name="modName">The name of the mod to get the conflicts information for.</param>
        /// <returns>A collection of <see cref="ConflictInfo"/> items describing the conflicts.</returns>
        public IEnumerable<ConflictInfo> GetConflicts(string modName)
        {
            if (string.IsNullOrEmpty(modName))
            {
                throw new ArgumentException("The mod name cannot be null or empty", nameof(modName));
            }

            if (!_queries.TryGetValue(modName, out var queriedMembers))
            {
                return Enumerable.Empty<ConflictInfo>();
            }

            var conflicts = new Dictionary<string, HashSet<string>>();
            foreach (var member in queriedMembers)
            {
                if (_usages.TryGetValue(member, out var mods))
                {
                    foreach (string mod in mods.Where(m => m != modName))
                    {
                        AddItem(conflicts, mod, member.DeclaringType.FullName + " -> " + member.ToString());
                    }
                }
            }

            return conflicts.Select(c => new ConflictInfo(c.Key, c.Value));
        }

        private static void AddItem<TKey, TValue>(IDictionary<TKey, HashSet<TValue>> target, TKey key, TValue value)
        {
            if (!target.TryGetValue(key, out var usages))
            {
                usages = new HashSet<TValue>();
                target.Add(key, usages);
            }

            usages.Add(value);
        }
    }
}
