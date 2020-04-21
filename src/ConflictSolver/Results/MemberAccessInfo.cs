// <copyright file="MemberAccessInfo.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using ConflictSolver.Monitor;
using ConflictSolver.Tools;

namespace ConflictSolver.Results
{
    /// <summary>
    /// A description of a single member access action.
    /// </summary>
    internal readonly struct MemberAccessInfo : IEquatable<MemberAccessInfo>, IComparable<MemberAccessInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberAccessInfo"/> struct.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/> that has been accessed.</param>
        /// <param name="accessTarget">The access target for this <paramref name="member"/>.</param>
        /// <param name="accessTypes">The types of performed access.</param>
        public MemberAccessInfo(MemberInfo member, AccessTarget accessTarget, AccessTypes accessTypes)
        {
            Member = member ?? throw new ArgumentNullException(nameof(member));
            AccessTarget = accessTarget;
            AccessTypes = accessTypes;
        }

        /// <summary>
        /// Gets the member being accessed.
        /// </summary>
        public MemberInfo Member { get; }

        /// <summary>
        /// Gets the access target of the <see cref="Member"/>.
        /// </summary>
        public AccessTarget AccessTarget { get; }

        /// <summary>
        /// Gets the access types of this access action.
        /// </summary>
        public AccessTypes AccessTypes { get; }

        /// <inheritdoc/>
        public int CompareTo(MemberAccessInfo other)
        {
            int result = Member.DeclaringType.Assembly.FullName.CompareTo(other.Member.DeclaringType.Assembly.FullName);
            if (result != 0)
            {
                return result;
            }

            result = Member.DeclaringType.Name.CompareTo(other.Member.DeclaringType.Name);
            if (result != 0)
            {
                return result;
            }

            result = Member.Name.CompareTo(other.Member.Name);
            return result != 0 ? result : AccessTypes.CompareTo(other.AccessTypes);
        }

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
        public override string ToString() => $"[{AccessTarget,10}] [{AccessTypes,18}] {Member.ToFullString()}";
    }
}
