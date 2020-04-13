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

        private bool _isConsolidated;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataView"/> class.
        /// </summary>
        /// <param name="source">The data source to use for this data view.</param>
        /// <param name="modInfoProvider">A service that provides the mod information.</param>
        /// <exception cref="ArgumentNullException">Thrown when any argument is null.</exception>
        public DataView(IDictionary<MemberInfo, HashSet<Type>> source, IModInfoProvider modInfoProvider)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _modInfoProvider = modInfoProvider ?? throw new ArgumentNullException(nameof(modInfoProvider));
        }

        /// <summary>
        /// Gets the monitored mods this data view stores information about.
        /// </summary>
        /// <returns>A collection of <see cref="MonitoredMod"/> objects describing the monitored mods.</returns>
        public IEnumerable<MonitoredMod> GetMonitoredMods()
        {
            if (!_isConsolidated)
            {
                Consolidate();
                _isConsolidated = true;
            }

            return GetMonitoredModNames().Select(CreateMonitoredMod).ToList();
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

        private void Consolidate()
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

        private MonitoredMod CreateMonitoredMod(string modName)
        {
            var modQueriedTypes = GetQueriedMembers(modName);
            var modConflicts = GetConflicts(modName);
            return new MonitoredMod(modName, modQueriedTypes, modConflicts);
        }

        private IEnumerable<string> GetMonitoredModNames() => _queries.Keys.OrderBy(v => v);

        private IEnumerable<string> GetQueriedMembers(string modName)
            => _queries.TryGetValue(modName, out var queriedMembers)
                ? queriedMembers.Select(m => m.ToFullString())
                : Enumerable.Empty<string>();

        private IEnumerable<ConflictInfo> GetConflicts(string modName)
        {
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
                        AddItem(conflicts, mod, member.ToFullString());
                    }
                }
            }

            return conflicts.Select(c => new ConflictInfo(c.Key, c.Value));
        }
    }
}
