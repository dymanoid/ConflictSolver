// <copyright file="MainWindow.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ConflictSolver.UI;
using UnityEngine;

namespace ConflictSolver.Views
{
    /// <summary>
    /// The mod's main window.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812", Justification = "Instantiated indirectly by Unity")]
    internal sealed class MainWindow : WindowBase<MainViewModel>
    {
        private Vector2 _scrollPosition;

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
            if (DataContext == null)
            {
                return;
            }

            if (DataContext.IsProcessingSnapshot)
            {
                DrawTools.CenterInArea(DrawProcessingLabel);
                return;
            }

            if (!DataContext.IsSnapshotAvailable)
            {
                DrawTools.CenterInArea(DrawSnapshotButton);
                return;
            }

            GUILayout.BeginVertical();
            DrawHeader();
            DrawSnapshotResults();
            GUILayout.EndVertical();
        }

        private static void DrawProcessingLabel()
        {
            GUI.contentColor = Colors.Text;
            GUILayout.Label("Processing...");
        }

        private void DrawSnapshotButton()
        {
            if (DrawTools.DrawButton("Take Snapshot"))
            {
                DataContext.TakeSnapshot();
            }
        }

        private void DrawHeader()
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandHeight(false));

            bool actionRequested = DrawTools.DrawButton("Expand All");
            if (actionRequested)
            {
                DataContext.ExpandAll();
            }

            actionRequested = DrawTools.DrawButton("Collapse All");
            if (actionRequested)
            {
                DataContext.CollapseAll();
            }

            actionRequested = DrawTools.DrawButton("Copy to Clipboard");
            if (actionRequested)
            {
                DataContext.CopyToClipboard();
            }

            GUILayout.FlexibleSpace();

            actionRequested = DrawTools.DrawButton("Delete Snapshot");
            if (actionRequested)
            {
                DataContext.Clear();
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(10);
        }

        private void DrawSnapshotResults()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            foreach (var mod in DataContext.Snapshot)
            {
                MonitoredModView.DrawModView(mod);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();
        }
    }
}
