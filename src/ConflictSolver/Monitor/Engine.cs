// <copyright file="Engine.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// The main conflict solver engine class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812", Justification = "Instantiated indirectly by Unity")]
    internal sealed class Engine : MonoBehaviour
    {
        private const string HarmonyId = Strings.ModHarmonyId;

        private readonly HarmonyInstance _harmonyInstance = HarmonyInstance.Create(HarmonyId);
        private readonly IMethodCatalog _methodCatalog = new MethodCatalog();

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

            foreach (var reflectionData in Enum.GetValues(typeof(ReflectionData)).Cast<ReflectionData>().Skip(1))
            {
                var methods = _methodCatalog.GetMethods(reflectionData);
                var postfixMethod = Storage.GetMethod(reflectionData);
                PatchWithPostfix(methods, postfixMethod);
            }

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
