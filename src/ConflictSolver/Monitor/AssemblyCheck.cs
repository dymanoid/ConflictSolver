// <copyright file="AssemblyCheck.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Serialization;
using ColossalFramework.Plugins;
using Harmony;
using UnityEngine;

namespace ConflictSolver.Monitor
{
    /// <summary>
    /// A helper class for determining the assembly categories.
    /// </summary>
    internal sealed class AssemblyCheck : IAssemblyCheck
    {
        private static readonly string HarmonyAssemblyName = typeof(HarmonyInstance).Assembly.GetName().Name;

        // Using a list instead of hash set because we only have 6 entries, hash set will not be faster
        private readonly List<Assembly> _gameAssemblies = new List<Assembly>()
        {
            typeof(Type).Assembly,
            typeof(XmlSerializer).Assembly,
            typeof(MonoBehaviour).Assembly,
            typeof(PluginManager).Assembly,
            typeof(SimulationManager).Assembly,
        };

        private readonly List<Assembly> _systemAssemblies = new List<Assembly>()
        {
            typeof(Type).Assembly,
            typeof(XmlSerializer).Assembly,
        };

        /// <inheritdoc/>
        public bool IsHarmony(Assembly assembly) =>
            assembly.GetName().Name == HarmonyAssemblyName; // Checking via name because there could be multiple assembly instances

        /// <inheritdoc/>
        public bool IsUserModAssembly(Assembly assembly) => !IsHarmony(assembly) && !_gameAssemblies.Contains(assembly);

        /// <inheritdoc/>
        public bool IsGameAssembly(Assembly assembly) => _gameAssemblies.Contains(assembly);

        /// <inheritdoc/>
        public bool IsSystemAssembly(Assembly assembly) => _systemAssemblies.Contains(assembly);

        /// <inheritdoc/>
        public bool IsDynamic(Assembly assembly) => assembly.ManifestModule is ModuleBuilder builder && builder.IsTransient();
    }
}
