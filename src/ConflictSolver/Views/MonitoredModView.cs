// <copyright file="MonitoredModView.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using ConflictSolver.UI;
using UnityEngine;

namespace ConflictSolver.Views
{
    /// <summary>
    /// A helper class for displaying the <see cref="MonitoredModViewModel"/>.
    /// </summary>
    internal static class MonitoredModView
    {
        /// <summary>
        /// Draws the content of the specified <see cref="MonitoredModViewModel"/> instance
        /// in the current area.
        /// </summary>
        /// <param name="mod">A <see cref="MonitoredModViewModel"/> instance to display.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="mod"/> is null.</exception>
        public static void DrawModView(MonitoredModViewModel mod)
        {
            if (mod is null)
            {
                throw new ArgumentNullException(nameof(mod));
            }

            mod.IsExpanded = DrawTools.DrawExpander(
                () => DrawItemHeader(mod.ModName, mod.Description),
                mod.IsExpanded,
                _ => DrawModItemContent(mod, Appearance.LargeMargin),
                0f);
        }

        private static void DrawModItemContent(MonitoredModViewModel mod, float indent)
        {
            indent += Appearance.LargeMargin;

            mod.IsReflectionListExpanded = DrawTools.DrawExpander(
                () => DrawItemHeader(Strings.ReflectionQueries, mod.QueriesDescription),
                mod.IsReflectionListExpanded,
                i => DrawStringList(mod.QueriedMembers, i),
                indent);

            if (!mod.AnyConflicts)
            {
                return;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            GUI.contentColor = Colors.Text;
            GUILayout.Label(Strings.PossibleModConflicts);
            GUILayout.EndHorizontal();

            DrawConflictedMods(mod.Conflicts, indent);
        }

        private static void DrawConflictedMods(IEnumerable<ConflictInfoViewModel> conflicts, float indent)
        {
            foreach (var conflict in conflicts)
            {
                conflict.IsExpanded = DrawTools.DrawExpander(
                    () => DrawItemHeader(conflict.ModName, conflict.Description),
                    conflict.IsExpanded,
                    i => DrawStringList(conflict.MemberNames, i),
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

        private static void DrawStringList(IEnumerable<string> items, float indent)
        {
            GUI.contentColor = Colors.Text;

            foreach (string type in items)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(indent);
                GUILayout.Label(type);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
    }
}
