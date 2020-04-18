// <copyright file="DrawTools.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using UnityEngine;
using static ConflictSolver.UI.Appearance;

namespace ConflictSolver.UI
{
    /// <summary>
    /// Common tools for displaying the UI elements.
    /// </summary>
    internal static class DrawTools
    {
        /// <summary>
        /// Centers the UI content vertically and horizontally in the current drawing area.
        /// The content specified by the <paramref name="content"/> delegate must be laid out
        /// using the <see cref="GUILayout"/> tools.
        /// </summary>
        /// <param name="content">A delegate that lays out the content.</param>
        public static void CenterInArea(Action content)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            content();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a button with specified <paramref name="caption"/>.
        /// </summary>
        /// <param name="caption">The caption of the button.</param>
        /// <returns><c>true</c> if the button was pressed; otherwise, <c>false</c>.</returns>
        public static bool DrawButton(string caption)
        {
            GUI.contentColor = Colors.ControlText;
            return GUILayout.Button(caption ?? string.Empty, GUILayout.Height(ButtonHeight));
        }

        /// <summary>
        /// Draws a square button with specified <paramref name="caption"/>.
        /// The width and the height of the button is defined by the <see cref="ButtonHeight"/> value.
        /// </summary>
        /// <param name="caption">The caption of the button.</param>
        /// <returns><c>true</c> if the button was pressed; otherwise, <c>false</c>.</returns>
        public static bool DrawSquareButton(string caption)
        {
            GUI.contentColor = Colors.ControlText;
            return GUILayout.Button(caption ?? string.Empty, GUILayout.Height(ButtonHeight), GUILayout.Width(ButtonHeight));
        }

        /// <summary>
        /// Draws an expander with user-provided header and content.
        /// </summary>
        /// <param name="header">A delegate that draws the header of the expander.</param>
        /// <param name="expanded">A value indicating whether the expander is in expanded state.</param>
        /// <param name="content">A delegate that draws the body of the expander. Will be only called if expanded.</param>
        /// <param name="indent">The right margin value for the header and the content.</param>
        /// <returns>A value indicating the expander is currently expanded.</returns>
        public static bool DrawExpander(Action header, bool expanded, Action<float> content, float indent)
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandHeight(false));

            GUILayout.Space(indent);

            string expanderSymbol = expanded ? ExpanderExpandedSymbol : ExpanderCollapsedSymbol;
            bool toggleExpand = DrawSquareButton(expanderSymbol);
            if (toggleExpand)
            {
                expanded = !expanded;
            }

            header();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (expanded)
            {
                indent += LargeMargin;
                content(indent);
            }

            GUILayout.Space(SmallMargin);
            return expanded;
        }
    }
}
