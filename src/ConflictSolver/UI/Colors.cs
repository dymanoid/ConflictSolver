// <copyright file="Colors.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using UnityEngine;

namespace ConflictSolver.UI
{
    /// <summary>
    /// A static class that defines the common colors for the mod UI.
    /// </summary>
    internal static class Colors
    {
        private const float MaxByte = 255f;

        /// <summary>
        /// Gets the window background color.
        /// </summary>
        public static Color WindowBackground { get; } = FromRgb(100, 100, 110, 235);

        /// <summary>
        /// Gets the window title bar background color.
        /// </summary>
        public static Color TitleBarBackground { get; } = FromRgb(20, 20, 20);

        /// <summary>
        /// Gets the window title bar background color in its the hovered state.
        /// </summary>
        public static Color TitleBarHoveredBackground { get; } = FromRgb(70, 70, 70);

        /// <summary>
        /// Gets window the title bar text color.
        /// </summary>
        public static Color TitleBarText { get; } = Color.white;

        /// <summary>
        /// Gets the color of the window's resize handle.
        /// </summary>
        public static Color ResizeHandle { get; } = FromRgb(160, 160, 160);

        /// <summary>
        /// Gets the color of the window's resize handle in its the hovered state.
        /// </summary>
        public static Color ResizeHandleHovered { get; } = FromRgb(190, 190, 190);

        /// <summary>
        /// Gets the color of the window's 'close' button.
        /// </summary>
        public static Color CloseButton { get; } = FromRgb(190, 20, 20);

        /// <summary>
        /// Gets the color of the window's 'close' button in its hovered state.
        /// </summary>
        public static Color CloseButtonHovered { get; } = FromRgb(240, 40, 40);

        private static Color FromRgb(int red, int green, int blue)
            => new Color(red / MaxByte, green / MaxByte, blue / MaxByte, 1f);

        private static Color FromRgb(int red, int green, int blue, int opacity)
            => new Color(red / MaxByte, green / MaxByte, blue / MaxByte, opacity / MaxByte);
    }
}
