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
    internal sealed class MonitoredModView
    {
        private const int MaxItemsInStringList = 100;
        private readonly GUIStyle _monotypeLabelStyle;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitoredModView"/> class.
        /// </summary>
        /// <param name="monotypeLabelStyle">The style of the items that have to be rendered with monotype font.</param>
        public MonitoredModView(GUIStyle monotypeLabelStyle)
        {
            _monotypeLabelStyle = monotypeLabelStyle ?? throw new System.ArgumentNullException(nameof(monotypeLabelStyle));
        }

        /// <summary>
        /// Draws the content of the specified <see cref="MonitoredModViewModel"/> instance
        /// in the current area.
        /// </summary>
        /// <param name="mod">A <see cref="MonitoredModViewModel"/> instance to display.</param>
        public void DrawModView(MonitoredModViewModel mod)
            => mod.IsExpanded = DrawTools.DrawExpander(
                () => DrawItemHeader(mod.ModName, mod.Description),
                enabled: true,
                expanded: mod.IsExpanded,
                _ => DrawModItemContent(mod, Appearance.LargeMargin),
                indent: 0f);

        private static void DrawItemHeader(string caption, string description)
        {
            GUI.contentColor = Colors.Text;
            GUILayout.Label(caption);

            GUI.contentColor = Colors.ShadedText;
            GUILayout.Label(description);
        }

        private void DrawModItemContent(MonitoredModViewModel mod, float indent)
        {
            indent += Appearance.LargeMargin;

            mod.IsReflectionListExpanded = DrawTools.DrawExpander(
                () => DrawItemHeader(Strings.ReflectionQueries, mod.QueriesDescription),
                enabled: mod.AnyQueries,
                expanded: mod.IsReflectionListExpanded,
                i => DrawStringList(mod.QueriedMembers, i),
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
                DrawConflictedMods(mod.Conflicts, indent);
            }
        }

        private void DrawConflictedMods(IEnumerable<ConflictInfoViewModel> conflicts, float indent)
        {
            foreach (var conflict in conflicts)
            {
                conflict.IsExpanded = DrawTools.DrawExpander(
                    () => DrawItemHeader(conflict.ModName, conflict.Description),
                    enabled: conflict.AnyMembers,
                    expanded: conflict.IsExpanded,
                    i => DrawStringList(conflict.MemberNames, i),
                    indent);
            }
        }

        private void DrawStringList(IEnumerable<MemberViewModel> items, float indent)
        {
            int currentItem = 0;
            foreach (var member in items.Take(MaxItemsInStringList + 1))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(indent);
                GUI.contentColor = Colors.Text;
                if (currentItem++ < MaxItemsInStringList)
                {
                    GUILayout.Label(member.AccessInfo, _monotypeLabelStyle);
                    GUI.contentColor = Colors.BlueText;
                    GUILayout.Label(member.Type, _monotypeLabelStyle);
                    GUI.contentColor = Colors.RedText;
                    GUILayout.Label(member.Assembly, _monotypeLabelStyle);
                    GUI.contentColor = Colors.GreenText;
                    GUILayout.Label(member.Class, _monotypeLabelStyle);
                    GUI.contentColor = Colors.YellowText;
                    GUILayout.Label(member.Name, _monotypeLabelStyle);
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
