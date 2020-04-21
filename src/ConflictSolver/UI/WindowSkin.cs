// <copyright file="WindowSkin.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ConflictSolver.Tools;
using UnityEngine;
using static ConflictSolver.UI.Appearance;

namespace ConflictSolver.UI
{
    /// <summary>
    /// A skin for the mod's windows.
    /// </summary>
    internal sealed class WindowSkin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowSkin"/> class.
        /// </summary>
        public WindowSkin()
        {
            InitializeTextures();
            InitializeSkin();
        }

        /// <summary>
        /// Gets the texture of the window's background.
        /// </summary>
        public Texture2D BackgroundTexture { get; } = new Texture2D(1, 1);

        /// <summary>
        /// Gets the texture of the resize handle.
        /// </summary>
        public Texture2D ResizeHandleNormalTexture { get; } = new Texture2D(1, 1);

        /// <summary>
        /// Gets the texture of the resize handle in its hovered state.
        /// </summary>
        public Texture2D ResizeHandleHoverTexture { get; } = new Texture2D(1, 1);

        /// <summary>
        /// Gets the texture of the 'close' button.
        /// </summary>
        public Texture2D CloseButtonNormalTexture { get; } = new Texture2D(1, 1);

        /// <summary>
        /// Gets the texture of the 'close' button in its hovered state.
        /// </summary>
        public Texture2D CloseButtonHoverTexture { get; } = new Texture2D(1, 1);

        /// <summary>
        /// Gets the texture of the window's border.
        /// </summary>
        public Texture2D WindowBorderNormalTexture { get; } = new Texture2D(1, 1);

        /// <summary>
        /// Gets the texture of the window's border in their hovered state.
        /// </summary>
        public Texture2D WindowBorderHoverTexture { get; } = new Texture2D(1, 1);

        /// <summary>
        /// Gets the Unity Engine skin for the GUI system.
        /// </summary>
        public GUISkin UnitySkin { get; } = ScriptableObject.CreateInstance<GUISkin>();

        private void InitializeTextures()
        {
            BackgroundTexture.SetPixel(0, 0, Colors.WindowBackground);
            BackgroundTexture.Apply();

            ResizeHandleNormalTexture.SetPixel(0, 0, Colors.WindowBorder);
            ResizeHandleNormalTexture.Apply();

            ResizeHandleHoverTexture.SetPixel(0, 0, Colors.WindowBorderHovered);
            ResizeHandleHoverTexture.Apply();

            CloseButtonNormalTexture.SetPixel(0, 0, Colors.CloseButton);
            CloseButtonNormalTexture.Apply();

            CloseButtonHoverTexture.SetPixel(0, 0, Colors.CloseButtonHovered);
            CloseButtonHoverTexture.Apply();

            WindowBorderNormalTexture.SetPixel(0, 0, Colors.WindowBorder);
            WindowBorderNormalTexture.Apply();

            WindowBorderHoverTexture.SetPixel(0, 0, Colors.WindowBorderHovered);
            WindowBorderHoverTexture.Apply();
        }

        private void InitializeSkin()
        {
            ReflectionTools.CopyPropertyValues<GUISkin, GUIStyle>(UnitySkin, GUI.skin, s => new GUIStyle(s));
            ReflectionTools.CopyPropertyValues(UnitySkin.settings, GUI.skin.settings);

            UnitySkin.label.wordWrap = false;
            UnitySkin.label.alignment = TextAnchor.MiddleLeft;
            UnitySkin.label.stretchHeight = true;

            UnitySkin.toggle.alignment = TextAnchor.UpperLeft;
            UnitySkin.toggle.stretchHeight = true;

            UnitySkin.button.stretchWidth = false;
            UnitySkin.button.padding = new RectOffset(4, 4, 2, 2);

            UnitySkin.window.normal.background = BackgroundTexture;
            UnitySkin.window.onNormal.background = BackgroundTexture;

            UnitySkin.font = Font.CreateDynamicFontFromOSFont(FontNames, FontSize);
        }
    }
}
