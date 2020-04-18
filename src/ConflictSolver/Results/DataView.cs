// <copyright file="DataView.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConflictSolver.Game;
using ConflictSolver.Monitor;
using ConflictSolver.Tools;

namespace ConflictSolver.Results
{
    /// <summary>
    /// The data view for collected Reflection queries.
    /// </summary>
    internal sealed class DataView
    {
        private readonly Dictionary<MemberInfo, ModAccessActions> _usages = new Dictionary<MemberInfo, ModAccessActions>();
        private readonly Dictionary<string, MemberAccessActions> _queries = new Dictionary<string, MemberAccessActions>();

        private readonly IEnumerable<TypeAccessActions> _source;
        private readonly IModInfoProvider _modInfoProvider;
        private readonly IAssemblyCheck _assemblyCheck;
        private bool _isConsolidated;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataView"/> class.
        /// </summary>
        /// <param name="source">The data source to use for this data view.</param>
        /// <param name="modInfoProvider">A service that provides the mod information.</param>
        /// <param name="assemblyCheck">An <see cref="IAssemblyCheck"/> service implementation.</param>
        /// <exception cref="ArgumentNullException">Thrown when any argument is null.</exception>
        public DataView(IEnumerable<TypeAccessActions> source, IModInfoProvider modInfoProvider, IAssemblyCheck assemblyCheck)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _modInfoProvider = modInfoProvider ?? throw new ArgumentNullException(nameof(modInfoProvider));
            _assemblyCheck = assemblyCheck ?? throw new ArgumentNullException(nameof(assemblyCheck));
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

        private void Consolidate()
        {
            foreach (var accessActions in _source)
            {
                foreach (var accessAction in accessActions.GetAllActions())
                {
                    var accessedMember = accessActions.AccessedMember;
                    string modName = _modInfoProvider.GetModName(accessAction.Accessor);
                    var accessTypes = accessAction.AccessTypes;

                    var modAccessActions = _usages.GetOrAdd(
                        accessedMember, () => new ModAccessActions(accessedMember));

                    modAccessActions.StoreAccess(modName, accessTypes);

                    var memberAccessAction = _queries.GetOrAdd(
                        modName, () => new MemberAccessActions(modName));

                    var accessTarget = GetAccessTarget(accessedMember.DeclaringType.Assembly, accessAction.Accessor.Assembly);
                    memberAccessAction.StoreAccess(accessedMember, accessTarget, accessTypes);
                }
            }
        }

        private AccessTarget GetAccessTarget(Assembly accessedAssembly, Assembly accessorAssembly)
        {
            if (_assemblyCheck.IsGameAssembly(accessedAssembly))
            {
                return AccessTarget.Game;
            }

            if (_modInfoProvider.IsSameMod(accessorAssembly, accessedAssembly))
            {
                return AccessTarget.OwnMod;
            }

            if (_assemblyCheck.IsUserModAssembly(accessedAssembly))
            {
                return AccessTarget.ForeignMod;
            }

            return AccessTarget.Unknown;
        }

        private MonitoredMod CreateMonitoredMod(string modName)
        {
            var queriedMembers = GetQueriedMembers(modName);
            var modConflicts = GetConflicts(modName);
            return new MonitoredMod(modName, queriedMembers, modConflicts);
        }

        private IEnumerable<string> GetMonitoredModNames() => _queries.Keys.OrderBy(v => v);

        private IEnumerable<MemberAccessInfo> GetQueriedMembers(string modName)
            => _queries.TryGetValue(modName, out var queriedMembers)
                ? queriedMembers.GetMembers()
                : Enumerable.Empty<MemberAccessInfo>();

        private IEnumerable<ConflictInfo> GetConflicts(string modName)
        {
            if (!_queries.TryGetValue(modName, out var queriedMembers))
            {
                return Enumerable.Empty<ConflictInfo>();
            }

            var conflictingMods = new Dictionary<string, MemberAccessActions>();
            foreach (var member in queriedMembers.GetMembers())
            {
                if (_usages.TryGetValue(member.Member, out var accessingMods))
                {
                    foreach (string conflictingMod in accessingMods.GetModNames())
                    {
                        var memberAccessAction = conflictingMods.GetOrAdd(
                            conflictingMod, () => new MemberAccessActions(conflictingMod));

                        memberAccessAction.StoreAccess(member.Member, member.AccessTarget, member.AccessTypes);
                    }
                }
            }

            conflictingMods.Remove(modName);
            return conflictingMods.Select(v => new ConflictInfo(v.Key, v.Value.GetMembers()));
        }
    }
}
