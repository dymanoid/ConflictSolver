// <copyright file="Engine.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace ConflictSolver
{
    /// <summary>
    /// The main conflict solver engine class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812", Justification = "Instantiated indirectly by Unity")]
    internal sealed class Engine : MonoBehaviour
    {
        private const string HarmonyId = "com.dymanoid.conflictsolver";

        private readonly HarmonyInstance _harmonyInstance = HarmonyInstance.Create(HarmonyId);
        private readonly MethodCatalog _methodCatalog = new MethodCatalog();

        private bool _isRunning;

        /// <summary>
        /// Runs the conflict solver engine by installing the required method patches.
        /// </summary>
        public void Run()
        {
            if (_isRunning)
            {
                Storage.Clear();
                return;
            }

            var storageType = typeof(Storage);

            var postfixMethod = storageType.GetMethod(Storage.MethodInfoCollectorName, BindingFlags.NonPublic | BindingFlags.Static);
            PatchWithPostfix(_methodCatalog.GetMethodsForMethodQuery(), postfixMethod);

            postfixMethod = storageType.GetMethod(Storage.FieldInfoCollectorName, BindingFlags.NonPublic | BindingFlags.Static);
            PatchWithPostfix(_methodCatalog.GetMethodsForFieldQuery(), postfixMethod);

            postfixMethod = storageType.GetMethod(Storage.PropertyInfoCollectorName, BindingFlags.NonPublic | BindingFlags.Static);
            PatchWithPostfix(_methodCatalog.GetMethodsForPropertyQuery(), postfixMethod);

            _isRunning = true;
            Debug.Log($"{Strings.DebugLogPrefix} initialized the monitoring patches");
        }

        /// <summary>
        /// Shuts down the conflict solver engine by removing the installed method patches.
        /// </summary>
        public void Shutdown()
        {
            UnPatch(_methodCatalog.GetAllMethods());
            Storage.Clear();
            _isRunning = false;
        }

        private void PatchWithPostfix(IEnumerable<MethodInfo> methodsToPatch, MethodInfo postfixMethod)
        {
            foreach (var method in methodsToPatch)
            {
                try
                {
                    _harmonyInstance.Patch(method, postfix: new HarmonyMethod(postfixMethod));
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"{Strings.DebugLogPrefix} could not patch the method {method}: {e}");
                }
            }
        }

        private void UnPatch(IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                try
                {
                    _harmonyInstance.Unpatch(method, HarmonyPatchType.All);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"{Strings.DebugLogPrefix} could not un-patch the method {method}: {e}");
                }
            }
        }
    }
}
