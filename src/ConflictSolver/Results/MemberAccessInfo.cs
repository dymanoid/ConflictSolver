// <copyright file="MemberAccessInfo.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using ConflictSolver.Monitor;

namespace ConflictSolver.Results
{
    /// <summary>
    /// A description of a single member access action.
    /// </summary>
    internal readonly struct MemberAccessInfo : IEquatable<MemberAccessInfo>
    {
        private readonly string _string;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessInfo"/> struct.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/> that has been accessed.</param>
        /// <param name="accessTarget">The access target for this <paramref name="member"/>.</param>
        /// <param name="accessTypes">The types of performed access.</param>
        public MemberAccessInfo(MemberInfo member, AccessTarget accessTarget, AccessTypes accessTypes)
        {
            Member = member ?? throw new ArgumentNullException(nameof(member));
            MemberName = member.ToFullString();
            AccessTarget = accessTarget;
            AccessTypes = accessTypes;
            _string = $"[{accessTarget}] [{accessTypes}] {MemberName}";
        }

        /// <summary>
        /// Gets the member being accessed.
        /// </summary>
        public MemberInfo Member { get; }

        /// <summary>
        /// Gets the string representation of the member being accessed.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the access target of the <see cref="Member"/>.
        /// </summary>
        public AccessTarget AccessTarget { get; }

        /// <summary>
        /// Gets the access types of this access action.
        /// </summary>
        public AccessTypes AccessTypes { get; }

        /// <inheritdoc/>
        public bool Equals(MemberAccessInfo other)
            => other.Member == Member && other.AccessTarget == AccessTarget && other.AccessTypes == AccessTypes;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is MemberAccessInfo accessAction && Equals(accessAction);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Overflow is fine, just wrap
            unchecked
            {
                int hash = (int)2166136261;
                hash = hash * 16777619 ^ Member.GetHashCode();
                hash = hash * 16777619 ^ AccessTarget.GetHashCode();
                hash = hash * 16777619 ^ AccessTypes.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString() => _string;
    }
}
