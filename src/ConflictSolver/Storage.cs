// <copyright file="Storage.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace ConflictSolver
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
        private static readonly IAssemblyCheck AssemblyCheck = new AssemblyCheck();
        private static readonly Dictionary<MemberInfo, HashSet<Type>> Data = new Dictionary<MemberInfo, HashSet<Type>>();

        /// <summary>
        /// Gets the data currently contained in the storage. This method is thread safe.
        /// </summary>
        /// <returns>A copy of the data contained in this storage.</returns>
        public static IDictionary<MemberInfo, HashSet<Type>> GetData()
        {
            lock (SyncObject)
            {
                return new Dictionary<MemberInfo, HashSet<Type>>(Data);
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
                Store(__result);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StoreFieldInfo(FieldInfo __result)
        {
            if (__result != null)
            {
                Store(__result);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StorePropertyInfo(PropertyInfo __result)
        {
            if (__result != null)
            {
                Store(__result);
            }
        }

        private static void Store(MemberInfo memberInfo)
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

            var callerType = GetModTypeFromStackTrace(stackTrace);
            if (callerType == null)
            {
                return;
            }

            lock (SyncObject)
            {
                if (!Data.TryGetValue(memberInfo, out var requestingTypes))
                {
                    requestingTypes = new HashSet<Type>();
                    Data.Add(memberInfo, requestingTypes);
                }

                requestingTypes.Add(callerType);
            }
        }

        private static Type GetModTypeFromStackTrace(StackTrace stackTrace)
        {
            Type callerType = null;
            for (int i = 1; i < stackTrace.FrameCount; ++i)
            {
                var frame = stackTrace.GetFrame(i);
                var frameCallerType = frame.GetMethod().DeclaringType;
                if (AssemblyCheck.IsUserModAssembly(frameCallerType.Assembly))
                {
                    callerType = frameCallerType;
                }
                else if (callerType != null)
                {
                    break;
                }
            }

            return callerType;
        }
    }
}
