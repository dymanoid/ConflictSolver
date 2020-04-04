// <copyright file="ConflictSolverMod.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Text;
using ConflictSolver.Results;
using ConflictSolver.UI;
using ICities;

namespace ConflictSolver
{
    /// <summary>
    /// The main Conflict Solver mod class.
    /// </summary>
    public sealed class ConflictSolverMod : LoadingExtensionBase, IUserMod
    {
        /// <summary>
        /// Gets the name of this mod.
        /// </summary>
        public string Name => Strings.ModName;

        /// <summary>
        /// Gets the description of this mod.
        /// </summary>
        public string Description => "Detects possible mod conflicts. Automatically!";

        /// <summary>
        /// This method will be called by the game when this mod has to be enabled.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Will be called by the game")]
        public void OnEnabled() => GameConnection.GetInstance<MainWindow>();

        /// <summary>
        /// This method will be called by the game when this mod has to be disabled.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822", Justification = "Will be called by the game")]
        public void OnDisabled()
        {
            GameConnection.DestroyInstance<Engine>(e => e.Shutdown());
            GameConnection.DestroyInstance<MainWindow>();
        }

        /// <summary>
        /// Called by the game when a map or a city is about to be unloaded.
        /// </summary>
        public override void OnLevelUnloading()
        {
            var dataSource = Storage.GetData();
            var modInfoProvider = new ModInfoProvider();
            var dataView = new DataView(dataSource, modInfoProvider);
            dataView.Prepare();

            var sb = new StringBuilder();

            sb.Append(Strings.DebugLogPrefix)
                .AppendLine(" dumps the conflicting mods report...");

            foreach (string mod in dataView.GetMonitoredModNames())
            {
                sb.Append("Monitored mod '")
                    .Append(mod)
                    .AppendLine("'");

                foreach (string queriedType in dataView.GetQueriedMembers(mod))
                {
                    sb.Append(" -> Queried: ")
                        .AppendLine(queriedType);
                }

                foreach (var conflict in dataView.GetConflicts(mod))
                {
                    sb.Append(" ** Possible conflict with mod '")
                        .Append(conflict.ModName)
                        .AppendLine("'");

                    foreach (string member in conflict.MemberNames)
                    {
                        sb.Append("    - ")
                            .AppendLine(member);
                    }
                }
            }

            sb.AppendLine("Report completed.");

            UnityEngine.Debug.Log(sb.ToString());
        }
    }
}
