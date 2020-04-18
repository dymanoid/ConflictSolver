// <copyright file="TypeAccessActions.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// A container for member access actions to a particular <see cref="MemberInfo"/>
    /// performed by a various <see cref="Type"/> accessors.
    /// </summary>
    internal sealed class TypeAccessActions : AccessActions<Type>, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeAccessActions"/> class.
        /// </summary>
        /// <param name="accessedMember">A <see cref="MemberInfo"/> this container stores the info for.</param>
        public TypeAccessActions(MemberInfo accessedMember)
            : base(accessedMember)
        {
        }

        private TypeAccessActions(TypeAccessActions other)
            : base(other)
        {
        }

        /// <summary>
        /// Gets all access actions this containers holds.
        /// </summary>
        /// <returns>A collection of <see cref="AccessDescriptor{T}"/> objects.</returns>
        public IEnumerable<AccessInfo> GetAllActions()
            => GetData().Select(item => new AccessInfo(item.Key, item.Value));

        /// <summary>
        /// Creates a new object that is a deep copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="TypeAccessActions"/> instance containing a copy of all data
        /// this instance holds.</returns>
        public TypeAccessActions Clone() => new TypeAccessActions(this);

        /// <inheritdoc/>
        object ICloneable.Clone() => Clone();
    }
}
