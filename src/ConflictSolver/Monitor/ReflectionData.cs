// <copyright file="ReflectionData.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// The type of the reflection data to monitor.
    /// </summary>
    internal enum ReflectionData
    {
        /// <summary>
        /// No particular information.
        /// </summary>
        Unknown,

        /// <summary>
        /// Information about <see cref="System.Reflection.MethodInfo"/> queries.
        /// </summary>
        MethodInfo,

        /// <summary>
        /// Information about multiple <see cref="System.Reflection.MethodInfo"/> queries.
        /// </summary>
        MethodInfoMultiple,

        /// <summary>
        /// Information about <see cref="System.Reflection.FieldInfo"/> queries.
        /// </summary>
        FieldInfo,

        /// <summary>
        /// Information about multiple <see cref="System.Reflection.FieldInfo"/> queries.
        /// </summary>
        FieldInfoMultiple,

        /// <summary>
        /// Information about value  via <see cref="System.Reflection.FieldInfo"/>.
        /// </summary>
        FieldInfoValueRead,

        /// <summary>
        /// Information about value writes via <see cref="System.Reflection.FieldInfo"/>.
        /// </summary>
        FieldInfoValueWrite,

        /// <summary>
        /// Information about <see cref="System.Reflection.PropertyInfo"/> queries.
        /// </summary>
        PropertyInfo,

        /// <summary>
        /// Information about multiple <see cref="System.Reflection.PropertyInfo"/> array.
        /// </summary>
        PropertyInfoMultiple,

        /// <summary>
        /// Information about value reads via <see cref="System.Reflection.PropertyInfo"/>.
        /// </summary>
        PropertyInfoValueRead,

        /// <summary>
        /// Information about value writes via <see cref="System.Reflection.PropertyInfo"/>.
        /// </summary>
        PropertyInfoValueWrite,
    }
}
