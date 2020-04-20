// <copyright file="MonitoredModView.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using ConflictSolver.UI;
using UnityEngine;

namespace ConflictSolver.Views
{
    /// <summary>
    /// A helper class for displaying the <see cref="MonitoredModViewModel"/>.
    /// </summary>
    internal static class MonitoredModView
    {
        private const int MaxItemsInStringList = 100;

        /// <summary>
        /// Draws the content of the specified <see cref="MonitoredModViewModel"/> instance
        /// in the current area.
        /// </summary>
        /// <param name="mod">A <see cref="MonitoredModViewModel"/> instance to display.</param>
        /// <param name="monotypeLabelStyle">The style of the items that have to be rendered with monotype font.</param>
        public static void DrawModView(MonitoredModViewModel mod, GUIStyle monotypeLabelStyle)
            => mod.IsExpanded = DrawTools.DrawExpander(
                () => DrawItemHeader(mod.ModName, mod.Description),
                mod.IsExpanded,
                _ => DrawModItemContent(mod, Appearance.LargeMargin, monotypeLabelStyle),
                0f);

        private static void DrawModItemContent(MonitoredModViewModel mod, float indent, GUIStyle monotypeLabelStyle)
        {
            indent += Appearance.LargeMargin;

            mod.IsReflectionListExpanded = DrawTools.DrawExpander(
                () => DrawItemHeader(Strings.ReflectionQueries, mod.QueriesDescription),
                mod.IsReflectionListExpanded,
                i => DrawStringList(mod.QueriedMembers, i, monotypeLabelStyle),
                indent);

            string conflictsCaption = mod.AnyConflicts
                ? Strings.PossibleModConflicts
                : Strings.PossibleModConflicts + ": " + Strings.None;

            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            GUI.contentColor = Colors.Text;
            GUILayout.Label(conflictsCaption);
            GUILayout.EndHorizontal();

            GUILayout.Space(Appearance.SmallMargin);

            if (mod.AnyConflicts)
            {
                DrawConflictedMods(mod.Conflicts, indent, monotypeLabelStyle);
            }
        }

        private static void DrawConflictedMods(IEnumerable<ConflictInfoViewModel> conflicts, float indent, GUIStyle monotypeLabelStyle)
        {
            foreach (var conflict in conflicts)
            {
                conflict.IsExpanded = DrawTools.DrawExpander(
                    () => DrawItemHeader(conflict.ModName, conflict.Description),
                    conflict.IsExpanded,
                    i => DrawStringList(conflict.MemberNames, i, monotypeLabelStyle),
                    indent);
            }
        }

        private static void DrawItemHeader(string caption, string description)
        {
            GUI.contentColor = Colors.Text;
            GUILayout.Label(caption);

            GUI.contentColor = Colors.ShadedText;
            GUILayout.Label(description);
        }

        private static void DrawStringList(IEnumerable<string> items, float indent, GUIStyle monotypeLabelStyle)
        {
            GUI.contentColor = Colors.Text;

            int currentItem = 0;
            foreach (string type in items.Take(MaxItemsInStringList + 1))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(indent);
                if (currentItem++ < MaxItemsInStringList)
                {
                    GUILayout.Label(type, monotypeLabelStyle);
                }
                else
                {
                    GUILayout.Label("...");
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
    }
}
