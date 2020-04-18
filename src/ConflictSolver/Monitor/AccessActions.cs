// <copyright file="AccessActions.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Reflection;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// A base class for a container that stores the member access actions
    /// performed by the sources described by the <typeparamref name="T"/> type.
    /// </summary>
    /// <typeparam name="T">The type of the objects that cause the access actions.</typeparam>
    internal abstract class AccessActions<T> : Container<MemberInfo, T, AccessTypes>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessActions{T}"/> class.
        /// </summary>
        /// <param name="accessedMember">A <see cref="MemberInfo"/> this container stores the info for.</param>
        protected AccessActions(MemberInfo accessedMember)
            : base(accessedMember)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessActions{T}"/> class
        /// and copies the data from the specified <paramref name="other"/> instance.
        /// </summary>
        /// <param name="other">The instance to copy the data from.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="other"/> is null.</exception>
        protected AccessActions(AccessActions<T> other)
            : base(other)
        {
        }

        /// <summary>
        /// Gets the <see cref="MemberInfo"/> this container stores the access information for.
        /// </summary>
        public MemberInfo AccessedMember => Key;

        /// <summary>
        /// Stores the access action for the specified access <paramref name="source"/>.
        /// </summary>
        /// <param name="source">A reference to an object that acts as the access action source.</param>
        /// <param name="accessTypes">The types of the access action.</param>
        public void StoreAccess(T source, AccessTypes accessTypes) => StoreData(source, v => v | accessTypes);
    }
}
