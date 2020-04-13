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
        public static Color WindowBackground { get; } = FromRgb(78, 78, 80, 245);

        /// <summary>
        /// Gets the color of the window border.
        /// </summary>
        public static Color WindowBorder { get; } = FromRgb(30, 30, 30);

        /// <summary>
        /// Gets the color of the window border in its the hovered state.
        /// </summary>
        public static Color WindowBorderHovered { get; } = FromRgb(120, 120, 124);

        /// <summary>
        /// Gets the controls text color.
        /// </summary>
        public static Color ControlText { get; } = FromRgb(255, 255, 255);

        /// <summary>
        /// Gets the basic text color.
        /// </summary>
        public static Color Text { get; } = FromRgb(200, 200, 200);

        /// <summary>
        /// Gets the shaded text color. It is somewhat shaded compared to the basic text color.
        /// </summary>
        public static Color ShadedText { get; } = FromRgb(128, 128, 128);

        /// <summary>
        /// Gets the color of the window's 'close' button.
        /// </summary>
        public static Color CloseButton { get; } = FromRgb(0, 122, 204);

        /// <summary>
        /// Gets the color of the window's 'close' button in its hovered state.
        /// </summary>
        public static Color CloseButtonHovered { get; } = FromRgb(28, 151, 234);

        private static Color FromRgb(int red, int green, int blue)
            => new Color(red / MaxByte, green / MaxByte, blue / MaxByte, 1f);

        private static Color FromRgb(int red, int green, int blue, int opacity)
            => new Color(red / MaxByte, green / MaxByte, blue / MaxByte, opacity / MaxByte);
    }
}
