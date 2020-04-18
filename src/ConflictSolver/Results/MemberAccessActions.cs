// <copyright file="MemberAccessActions.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConflictSolver.Monitor;

namespace ConflictSolver.Results
{
    /// <summary>
    /// A container for member access actions to various <see cref="MemberInfo"/>
    /// performed for a particular mod.
    /// </summary>
    internal class MemberAccessActions : Container<string, MemberInfo, AccessTypes>
    {
        private readonly Dictionary<MemberInfo, AccessTarget> _targets = new Dictionary<MemberInfo, AccessTarget>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessActions"/> class.
        /// </summary>
        /// <param name="modName">The name of the mod that performs the access actions.</param>
        public MemberAccessActions(string modName)
            : base(modName)
        {
        }

        /// <summary>
        /// Stores the access action for the specified access <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The type member being accessed.</param>
        /// <param name="accessTarget">The access target of the <paramref name="member"/>.</param>
        /// <param name="accessTypes">The types of the access action.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
        public void StoreAccess(MemberInfo member, AccessTarget accessTarget, AccessTypes accessTypes)
        {
            StoreData(member, a => a | accessTypes);
            _targets[member] = accessTarget;
        }

        /// <summary>
        /// Gets all member access actions for this mod.
        /// </summary>
        /// <returns>A collection of <see cref="MemberAccessInfo"/> objects describing the access actions.</returns>
        public IEnumerable<MemberAccessInfo> GetMembers()
            => GetData().Select(v => new MemberAccessInfo(v.Key, GetAccessTarget(v.Key), v.Value));

        private AccessTarget GetAccessTarget(MemberInfo member)
            => _targets.TryGetValue(member, out var result)
                ? result
                : AccessTarget.Unknown;
    }
}
