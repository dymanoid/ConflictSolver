// <copyright file="MainWindow.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using UnityEngine;

namespace ConflictSolver.UI
{
    /// <summary>
    /// The mod's main window.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812", Justification = "Instantiated indirectly by Unity")]
    internal sealed class MainWindow : WindowBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
            : base(Strings.ModName, Appearance.DefaultWindowBoundaries, resizable: true)
        {
        }

        /// <inheritdoc/>
        public override void Update()
        {
            base.Update();

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F1))
            {
                IsVisible = true;
            }
        }

        /// <summary>
        /// Draws the GUI elements of this window.
        /// </summary>
        protected override void DrawWindow()
        {
            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Take snapshot", GUILayout.Height(Appearance.ButtonHeight), GUILayout.ExpandWidth(false)))
            {
            }

            GUILayout.FlexibleSpace();

            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
        }
    }
}
