// <copyright file="WindowSkin.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using UnityEngine;

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
        /// Gets the texture of the window's border and title bar..
        /// </summary>
        public Texture2D WindowBorderNormalTexture { get; } = new Texture2D(1, 1);

        /// <summary>
        /// Gets the texture of the window's border and title bar in their hovered state.
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

            ResizeHandleNormalTexture.SetPixel(0, 0, Colors.ResizeHandle);
            ResizeHandleNormalTexture.Apply();

            ResizeHandleHoverTexture.SetPixel(0, 0, Colors.ResizeHandleHovered);
            ResizeHandleHoverTexture.Apply();

            CloseButtonNormalTexture.SetPixel(0, 0, Colors.CloseButton);
            CloseButtonNormalTexture.Apply();

            CloseButtonHoverTexture.SetPixel(0, 0, Colors.CloseButtonHovered);
            CloseButtonHoverTexture.Apply();

            WindowBorderNormalTexture.SetPixel(0, 0, Colors.TitleBarBackground);
            WindowBorderNormalTexture.Apply();

            WindowBorderHoverTexture.SetPixel(0, 0, Colors.TitleBarHoveredBackground);
            WindowBorderHoverTexture.Apply();
        }

        private void InitializeSkin()
        {
            UnitySkin.box = new GUIStyle(GUI.skin.box);
            UnitySkin.button = new GUIStyle(GUI.skin.button);
            UnitySkin.horizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
            UnitySkin.horizontalScrollbarLeftButton = new GUIStyle(GUI.skin.horizontalScrollbarLeftButton);
            UnitySkin.horizontalScrollbarRightButton = new GUIStyle(GUI.skin.horizontalScrollbarRightButton);
            UnitySkin.horizontalScrollbarThumb = new GUIStyle(GUI.skin.horizontalScrollbarThumb);
            UnitySkin.horizontalSlider = new GUIStyle(GUI.skin.horizontalSlider);
            UnitySkin.horizontalSliderThumb = new GUIStyle(GUI.skin.horizontalSliderThumb);
            UnitySkin.label = new GUIStyle(GUI.skin.label);
            UnitySkin.scrollView = new GUIStyle(GUI.skin.scrollView);
            UnitySkin.textArea = new GUIStyle(GUI.skin.textArea);
            UnitySkin.textField = new GUIStyle(GUI.skin.textField);
            UnitySkin.toggle = new GUIStyle(GUI.skin.toggle);
            UnitySkin.verticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
            UnitySkin.verticalScrollbarDownButton = new GUIStyle(GUI.skin.verticalScrollbarDownButton);
            UnitySkin.verticalScrollbarThumb = new GUIStyle(GUI.skin.verticalScrollbarThumb);
            UnitySkin.verticalScrollbarUpButton = new GUIStyle(GUI.skin.verticalScrollbarUpButton);
            UnitySkin.verticalSlider = new GUIStyle(GUI.skin.verticalSlider);
            UnitySkin.verticalSliderThumb = new GUIStyle(GUI.skin.verticalSliderThumb);
            UnitySkin.window = new GUIStyle(GUI.skin.window);
            UnitySkin.window.normal.background = BackgroundTexture;
            UnitySkin.window.onNormal.background = BackgroundTexture;

            UnitySkin.settings.cursorColor = GUI.skin.settings.cursorColor;
            UnitySkin.settings.cursorFlashSpeed = GUI.skin.settings.cursorFlashSpeed;
            UnitySkin.settings.doubleClickSelectsWord = GUI.skin.settings.doubleClickSelectsWord;
            UnitySkin.settings.selectionColor = GUI.skin.settings.selectionColor;
            UnitySkin.settings.tripleClickSelectsLine = GUI.skin.settings.tripleClickSelectsLine;

            UnitySkin.font = Font.CreateDynamicFontFromOSFont(Appearance.FontName, Appearance.FontSize);
        }
    }
}
