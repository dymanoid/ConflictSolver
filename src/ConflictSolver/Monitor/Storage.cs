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
        /// Gets the <see cref="MethodInfo"/> descriptor of a method that can be used for storing
        /// the information about the specified <paramref name="reflectionData"/>.
        /// </summary>
        /// <param name="reflectionData">The reflection data type of interest.</param>
        /// <returns>A <see cref="MethodInfo"/> descriptor of a method of this class.</returns>
        public static MethodInfo GetMethod(ReflectionData reflectionData)
        {
            string methodName;
            switch (reflectionData)
            {
                case ReflectionData.MethodInfo:
                    methodName = nameof(StoreMethodInfo);
                    break;

                case ReflectionData.MethodInfoMultiple:
                    methodName = nameof(StoreMethodInfos);
                    break;

                case ReflectionData.FieldInfo:
                    methodName = nameof(StoreFieldInfo);
                    break;

                case ReflectionData.FieldInfoMultiple:
                    methodName = nameof(StoreFieldInfos);
                    break;

                case ReflectionData.FieldInfoValueRead:
                    methodName = nameof(StoreFieldInfoValueRead);
                    break;

                case ReflectionData.FieldInfoValueWrite:
                    methodName = nameof(StoreFieldInfoValueWrite);
                    break;

                case ReflectionData.PropertyInfo:
                    methodName = nameof(StorePropertyInfo);
                    break;

                case ReflectionData.PropertyInfoMultiple:
                    methodName = nameof(StorePropertyInfos);
                    break;

                case ReflectionData.PropertyInfoValueRead:
                    methodName = nameof(StorePropertyInfoValueRead);
                    break;

                case ReflectionData.PropertyInfoValueWrite:
                    methodName = nameof(StorePropertyInfoValueWrite);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(reflectionData), "Unsupported reflection data type");
            }

            return typeof(Storage).GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
        }

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
        private static void StoreMethodInfos(MethodInfo[] __result)
        {
            if (__result?.Length > 0)
            {
                foreach (var method in __result)
                {
                    Store(method, AccessTypes.Query);
                }
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
        private static void StoreFieldInfos(FieldInfo[] __result)
        {
            if (__result?.Length > 0)
            {
                foreach (var field in __result)
                {
                    Store(field, AccessTypes.Query);
                }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StorePropertyInfos(PropertyInfo[] __result)
        {
            if (__result?.Length > 0)
            {
                foreach (var property in __result)
                {
                    Store(property, AccessTypes.Query);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StorePropertyInfoValueRead(PropertyInfo __instance) => Store(__instance, AccessTypes.Read);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StorePropertyInfoValueWrite(PropertyInfo __instance) => Store(__instance, AccessTypes.Write);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StoreFieldInfoValueRead(FieldInfo __instance) => Store(__instance, AccessTypes.Read);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006", Justification = "Harmony patch")]
        private static void StoreFieldInfoValueWrite(FieldInfo __instance) => Store(__instance, AccessTypes.Write);

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
