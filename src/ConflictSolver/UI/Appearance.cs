// <copyright file="Appearance.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using UnityEngine;

namespace ConflictSolver.UI
{
    /// <summary>
    /// A static class that defines the appearance of the mod's UI.
    /// </summary>
    internal static class Appearance
    {
        /// <summary>
        /// The height of the window title bar.
        /// </summary>
        public const float TitleBarHeight = 24f;

        /// <summary>
        /// The window's border thickness.
        /// </summary>
        public const float WindowBorderWidth = 2f;

        /// <summary>
        /// A default small margin value.
        /// </summary>
        public const float SmallMargin = 8f;

        /// <summary>
        /// A default large margin value.
        /// </summary>
        public const float LargeMargin = 16f;

        /// <summary>
        /// The minimum width of the window.
        /// </summary>
        public const float MinimumWindowWidth = 320f;

        /// <summary>
        /// The minimum height of the window.
        /// </summary>
        public const float MinimumWindowHeight = 240f;

        /// <summary>
        /// The default button height.
        /// </summary>
        public const float ButtonHeight = 28f;

        /// <summary>
        /// The font name for the UI.
        /// </summary>
        public const string FontName = "Arial";

        /// <summary>
        /// The font size for the UI.
        /// </summary>
        public const int FontSize = 14;

        /// <summary>
        /// Gets default boundaries of the mod's window.
        /// </summary>
        public static Rect DefaultWindowBoundaries { get; } = new Rect(200f, 200f, 640f, 480f);
    }
}
