// <copyright file="ModInfoProvider.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ColossalFramework.Plugins;
using ICities;

namespace ConflictSolver
{
    /// <summary>
    /// A service that can provide various information for the game mods.
    /// </summary>
    internal sealed class ModInfoProvider : IModInfoProvider
    {
        private readonly Dictionary<Assembly, PluginManager.PluginInfo> _mods = new Dictionary<Assembly, PluginManager.PluginInfo>();
        private readonly Dictionary<Type, PluginManager.PluginInfo> _cache = new Dictionary<Type, PluginManager.PluginInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModInfoProvider"/> class.
        /// </summary>
        public ModInfoProvider()
        {
            var pluginManager = PluginManager.instance;
            var mods = pluginManager.GetPluginsInfo()
                .Concat(pluginManager.GetCameraPluginInfos())
                .Select(p => new { Mod = p, Assemblies = p.GetAssemblies() });

            foreach (var mod in mods)
            {
                foreach (var assembly in mod.Assemblies)
                {
                    _mods.Add(assembly, mod.Mod);
                }
            }
        }

        /// <inheritdoc/>
        public string GetModName(Type anyType)
        {
            if (_cache.TryGetValue(anyType, out var modInfo))
            {
                return GetModName(modInfo);
            }

            if (!_mods.TryGetValue(anyType.Assembly, out modInfo))
            {
                throw new InvalidOperationException($"The assembly {anyType.Assembly.GetName().Name} could not be found in the mod registry");
            }

            _cache.Add(anyType, modInfo);
            return GetModName(modInfo);

            string GetModName(PluginManager.PluginInfo mod)
            {
                string result = mod.name;
                string name = ((IUserMod)mod.userModInstance)?.Name ?? mod.assembliesString;
                return result + " - " + name;
            }
        }
    }
}
