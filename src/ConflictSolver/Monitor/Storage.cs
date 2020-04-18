// <copyright file="Storage.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ConflictSolver.Tools;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// A storage class that holds the data collected by the <see cref="Engine"/>.
    /// The class must be static since it contains static methods that serve as method call postfixes.
    /// </summary>
    internal static class Storage
    {
        /// <summary>
        /// The name of the method that serves as the <see cref="MethodInfo"/> data collection sink.
        /// </summary>
        public const string MethodInfoCollectorName = nameof(StoreMethodInfo);

        /// <summary>
        /// The name of the method that serves as the <see cref="FieldInfo"/> data collection sink.
        /// </summary>
        public const string FieldInfoCollectorName = nameof(StoreFieldInfo);

        /// <summary>
        /// The name of the method that serves as the <see cref="PropertyInfo"/> data collection sink.
        /// </summary>
        public const string PropertyInfoCollectorName = nameof(StorePropertyInfo);

        /// <summary>
        /// The number of stack frames occupied by the patching helper methods of Harmony library itself.
        /// </summary>
        private const int HarmonyStackFramesCount = 3;

        private static readonly object SyncObject = new object();
        private static readonly Dictionary<MemberInfo, TypeAccessActions> Data = new Dictionary<MemberInfo, TypeAccessActions>();

        /// <summary>
        /// Gets the <see cref="IAssemblyCheck"/> service implementation.
        /// </summary>
        public static IAssemblyCheck AssemblyCheck { get; } = new AssemblyCheck();

        /// <summary>
        /// Gets the data currently contained in the storage. This method is thread safe.
        /// </summary>
        /// <returns>A copy of the data contained in this storage.</returns>
        public static IEnumerable<TypeAccessActions> GetData()
        {
            lock (SyncObject)
            {
                return Data.Values.Select(v => v.Clone()).ToList();
            }
        }

        /// <summary>
        /// Clears all data contained in the storage.
        /// </summary>
        public static void Clear()
        {
            lock (SyncObject)
            {
                Data.Clear();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StoreMethodInfo(MethodInfo __result)
        {
            if (__result != null)
            {
                Store(__result, AccessTypes.Query);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StoreFieldInfo(FieldInfo __result)
        {
            if (__result != null)
            {
                Store(__result, AccessTypes.Query);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StorePropertyInfo(PropertyInfo __result)
        {
            if (__result != null)
            {
                Store(__result, AccessTypes.Query);
            }
        }

        private static void Store(MemberInfo memberInfo, AccessTypes accessTypes)
        {
            var memberAssembly = memberInfo.DeclaringType.Assembly;

            // We do not track dynamic, Harmony and system types - hopefully no one patches those (except of special cases)
            if (AssemblyCheck.IsDynamic(memberAssembly) ||
                AssemblyCheck.IsHarmony(memberAssembly) ||
                AssemblyCheck.IsSystemAssembly(memberAssembly))
            {
                return;
            }

            var stackTrace = new StackTrace(HarmonyStackFramesCount, fNeedFileInfo: false);
            var callingMethod = stackTrace.GetFrame(0).GetMethod();
            var callingAssembly = callingMethod.DeclaringType.Assembly;

            // We do not track the queries from the game assemblies, because they are not caused by mods
            if (AssemblyCheck.IsGameAssembly(callingAssembly))
            {
                return;
            }

            var accessorType = GetModTypeFromStackTrace(stackTrace);
            if (accessorType == null)
            {
                return;
            }

            lock (SyncObject)
            {
                var accessActions = Data.GetOrAdd(memberInfo, () => new TypeAccessActions(memberInfo));
                accessActions.StoreAccess(accessorType, accessTypes);
            }
        }

        private static Type GetModTypeFromStackTrace(StackTrace stackTrace)
        {
            for (int i = stackTrace.FrameCount - 1; i >= 0; --i)
            {
                var frame = stackTrace.GetFrame(i);
                var frameCallerType = frame.GetMethod().DeclaringType;
                if (AssemblyCheck.IsUserModAssembly(frameCallerType.Assembly))
                {
                    return frameCallerType;
                }
            }

            return null;
        }
    }
}
