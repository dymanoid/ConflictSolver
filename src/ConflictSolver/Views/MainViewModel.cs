// <copyright file="MainViewModel.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ColossalFramework;
using ConflictSolver.Game;
using ConflictSolver.Monitor;
using ConflictSolver.Results;

namespace ConflictSolver.Views
{
    /// <summary>
    /// The view-model of the main window.
    /// </summary>
    internal sealed class MainViewModel
    {
        private readonly object _lockObject = new object();

        private IEnumerable<MonitoredMod> _data;

        /// <summary>
        /// Gets a value indicating whether a data snapshot is already available.
        /// </summary>
        public bool IsSnapshotAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether a data snapshot is currently being processed.
        /// </summary>
        public bool IsProcessingSnapshot { get; private set; }

        /// <summary>
        /// Gets a collection of the mod information items representing a data snapshot
        /// obtained by user request.
        /// </summary>
        public IEnumerable<MonitoredModViewModel> Snapshot { get; private set; } = Enumerable.Empty<MonitoredModViewModel>();

        /// <summary>
        /// Takes the snapshot of the current data. The process can be monitored by polling the
        /// <see cref="IsSnapshotAvailable"/> and <see cref="IsProcessingSnapshot"/> properties.
        /// </summary>
        public void TakeSnapshot()
        {
            lock (_lockObject)
            {
                IsSnapshotAvailable = false;
                IsProcessingSnapshot = true;
            }

            ThreadPool.QueueUserWorkItem(DoTakeSnapshot);
        }

        /// <summary>
        /// Clears the current <see cref="Snapshot"/>.
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                IsSnapshotAvailable = false;
                Snapshot = Enumerable.Empty<MonitoredModViewModel>();
                _data = null;
            }
        }

        /// <summary>
        /// Expands all items in the current snapshot. The queried members sections will not
        /// change their state.
        /// </summary>
        public void ExpandAll()
        {
            lock (_lockObject)
            {
                if (!IsSnapshotAvailable)
                {
                    return;
                }
            }

            foreach (var item in Snapshot)
            {
                item.ExpandAll();
            }
        }

        /// <summary>
        /// Collapses all items and sub-items in the current snapshot, including the queried members sections.
        /// </summary>
        public void CollapseAll()
        {
            lock (_lockObject)
            {
                if (!IsSnapshotAvailable)
                {
                    return;
                }
            }

            foreach (var item in Snapshot)
            {
                item.CollapseAll();
            }
        }

        /// <summary>
        /// Copies the content of the current snapshot to the clipboard in a readable text format.
        /// </summary>
        public void CopyToClipboard()
        {
            lock (_lockObject)
            {
                if (!IsSnapshotAvailable)
                {
                    return;
                }
            }

            string content = ResultsTools.SnapshotToString(_data);
            Clipboard.text = content;
        }

        private void DoTakeSnapshot(object _)
        {
            var dataSource = Storage.GetData();
            var modInfoProvider = new ModInfoProvider();
            var dataView = new DataView(dataSource, modInfoProvider);
            var data = dataView.GetMonitoredMods().ToList();
            var snapshot = data.Select(m => new MonitoredModViewModel(m)).ToList();

            lock (_lockObject)
            {
                IsSnapshotAvailable = true;
                IsProcessingSnapshot = false;
                _data = data;
                Snapshot = snapshot;
            }
        }
    }
}
