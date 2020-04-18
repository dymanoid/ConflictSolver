// <copyright file="AccessInfo.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// A description of a single access action caused by a particular <see cref="Type"/>.
    /// </summary>
    internal readonly struct AccessInfo : IEquatable<AccessInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessInfo"/> struct.
        /// </summary>
        /// <param name="accessor">The <see cref="Type"/> that cause the access.</param>
        /// <param name="accessTypes">The types of the access action.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="accessor"/> is null.</exception>
        public AccessInfo(Type accessor, AccessTypes accessTypes)
        {
            Accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            AccessTypes = accessTypes;
        }

        /// <summary>
        /// Gets the object that caused this access action.
        /// </summary>
        public Type Accessor { get; }

        /// <summary>
        /// Gets the access types of this access action.
        /// </summary>
        public AccessTypes AccessTypes { get; }

        /// <inheritdoc/>
        public bool Equals(AccessInfo other) => other.Accessor == Accessor && other.AccessTypes == AccessTypes;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is AccessInfo accessAction && Equals(accessAction);

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            // Overflow is fine, just wrap
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Accessor.GetHashCode();
                hash = (hash * 16777619) ^ AccessTypes.GetHashCode();
                return hash;
            }
        }
    }
}
