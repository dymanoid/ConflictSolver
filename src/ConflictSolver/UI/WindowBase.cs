// <copyright file="WindowBase.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ConflictSolver.Game;
using UnityEngine;
using static ConflictSolver.UI.Appearance;

namespace ConflictSolver.UI
{
    /// <summary>
    /// A base class for the mod's windows.
    /// </summary>
    /// <typeparam name="T">The type of the data context to be used as view-model.</typeparam>
    internal abstract class WindowBase<T> : MonoBehaviour, IGameObject, IDestroyableObject, IGuiObject, IWindow
        where T : class
    {
        private static int LastUsedWindowId = 4095;

        private readonly string _title;
        private readonly int _windowId;
        private readonly bool _resizable;
        private readonly ModalWindowHelper _modalUI = new ModalWindowHelper();

        private readonly WindowArea _titleBarArea;
        private readonly WindowArea _contentArea;

        private Vector2 _resizeDragHandle;
        private Vector2 _moveDragHandle;

        // The skin can only be initialized in OnGUI call, thus this field is not read-only
        private WindowSkin _skin;

        private Rect _windowBoundaries;
        private bool _isVisible;
        private bool _isMoving;
        private bool _isResizing;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowBase{T}"/> class.
        /// </summary>
        /// <param name="title">The title of the window.</param>
        /// <param name="initialBoundaries">The initial window's boundaries.</param>
        /// <param name="resizable">A value indicating whether the window should be resizable.</param>
        protected WindowBase(string title, Rect initialBoundaries, bool resizable)
        {
            _windowId = ++LastUsedWindowId;
            _title = title;
            _windowBoundaries = initialBoundaries;
            _resizable = resizable;

            _titleBarArea = new WindowArea(this, SmallMargin, 0f)
                .ChangeSizeRelative(height: 0)
                .ChangeSizeBy(height: TitleBarHeight);

            _contentArea = new WindowArea(this, SmallMargin)
                .ChangeSizeBy(height: -TitleBarHeight)
                .OffsetBy(vertical: TitleBarHeight);
        }

        /// <inheritdoc/>
        public Rect WindowBoundaries => _windowBoundaries;

        /// <inheritdoc/>
        public bool IsVisible
        {
            get => _isVisible;

            set
            {
                bool wasVisible = _isVisible;
                _isVisible = value;
                if (_isVisible && !wasVisible)
                {
                    GUI.BringWindowToFront(_windowId);
                    ProcessWindowOpened();
                }
                else if (!_isVisible && wasVisible)
                {
                    ProcessWindowClosed();
                }
            }
        }

        /// <inheritdoc/>
        public bool IsMouseOverWindow => _isVisible && _windowBoundaries.Contains(GetScreenMousePosition());

        /// <summary>
        /// Gets or sets the data context of this window.
        /// </summary>
        public T DataContext { get; set; }

        /// <inheritdoc/>
        public virtual void Update() => _modalUI.UpdateModalState(IsMouseOverWindow);

        /// <inheritdoc/>
        public virtual void OnDestroy()
        {
        }

        /// <inheritdoc/>
        public virtual void OnGUI()
        {
            if (_skin == null)
            {
                _skin = new WindowSkin();
            }

            if (!IsVisible)
            {
                return;
            }

            var originalSkin = GUI.skin;

            try
            {
                GUI.skin = _skin.UnitySkin;
                _windowBoundaries = GUI.Window(_windowId, _windowBoundaries, WindowFunction, string.Empty);
            }
            finally
            {
                GUI.skin = originalSkin;
            }
        }

        /// <summary>
        /// When overridden in derived classes, performs the drawing of the window's content.
        /// </summary>
        protected abstract void DrawWindow();

        /// <summary>
        /// When overridden in derived classes, performs actions after the window was completely drawn.
        /// </summary>
        protected virtual void ProcessWindowDrawn()
        {
        }

        /// <summary>
        /// When overridden in derived classes, performs actions after the window was opened.
        /// </summary>
        protected virtual void ProcessWindowOpened()
        {
        }

        /// <summary>
        /// When overridden in derived classes, performs actions after the window was closed.
        /// </summary>
        protected virtual void ProcessWindowClosed()
        {
        }

        /// <summary>
        /// When overridden in derived classes, performs actions after the window was resized.
        /// </summary>
        protected virtual void ProcessWindowResized()
        {
        }

        /// <summary>
        /// When overridden in derived classes, performs actions after the window was moved.
        /// </summary>
        protected virtual void ProcessWindowMoved()
        {
        }

        private static Vector2 GetScreenMousePosition()
        {
            var result = Input.mousePosition;
            result.y = Screen.height - result.y;
            return result;
        }

        private void WindowFunction(int windowId)
        {
            GUILayout.Space(SmallMargin);

            try
            {
                _contentArea.Begin();
                DrawWindow();
                _contentArea.End();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"{Strings.DebugLogPrefix} has an error drawing the window: {e}");
            }

            GUILayout.Space(LargeMargin);
            DrawBorder();

            var mousePosition = GetScreenMousePosition();

            DrawCloseButton(mousePosition);
            DrawTitlebar(mousePosition);

            if (_resizable)
            {
                DrawResizeHandle(mousePosition);
            }

            ProcessWindowDrawn();
        }

        private void DrawBorder()
        {
            var left = new Rect(0f, 0f, WindowBorderWidth, _windowBoundaries.height);
            var right = new Rect(_windowBoundaries.width - WindowBorderWidth, 0f, WindowBorderWidth, _windowBoundaries.height);
            var bottom = new Rect(0f, _windowBoundaries.height - WindowBorderWidth, _windowBoundaries.width, WindowBorderWidth);

            GUI.DrawTexture(left, _skin.WindowBorderNormalTexture);
            GUI.DrawTexture(right, _skin.WindowBorderNormalTexture);
            GUI.DrawTexture(bottom, _skin.WindowBorderNormalTexture);
        }

        private void DrawTitlebar(Vector2 mousePosition)
        {
            var titlebarBoundaries = _windowBoundaries;
            titlebarBoundaries.height = TitleBarHeight;

            var texture = ProcessMoving(titlebarBoundaries, mousePosition)
                ? _skin.WindowBorderHoverTexture
                : _skin.WindowBorderNormalTexture;

            GUI.DrawTexture(new Rect(0f, 0f, _windowBoundaries.width - TitleBarHeight, TitleBarHeight), texture, ScaleMode.StretchToFill);

            _titleBarArea.Begin();
            GUI.contentColor = Colors.TitleBarText;
            GUILayout.Label(_title);
            _titleBarArea.End();
        }

        private void DrawCloseButton(Vector2 mousePosition)
        {
            var closeButtonBoundaries = new Rect(
                _windowBoundaries.xMax - TitleBarHeight,
                _windowBoundaries.y,
                TitleBarHeight,
                TitleBarHeight);

            var texture = ProcessClosing(closeButtonBoundaries, mousePosition)
                ? _skin.CloseButtonHoverTexture
                : _skin.CloseButtonNormalTexture;

            GUI.DrawTexture(
                new Rect(_windowBoundaries.width - TitleBarHeight, 0f, TitleBarHeight, TitleBarHeight),
                texture,
                ScaleMode.StretchToFill);
        }

        private void DrawResizeHandle(Vector2 mousePosition)
        {
            var resizeHandleBoundaries = new Rect(
                _windowBoundaries.xMax - LargeMargin,
                _windowBoundaries.yMax - SmallMargin,
                LargeMargin,
                SmallMargin);

            var texture = ProcessResizing(resizeHandleBoundaries, mousePosition)
                ? _skin.ResizeHandleHoverTexture
                : _skin.ResizeHandleNormalTexture;

            GUI.DrawTexture(
                new Rect(_windowBoundaries.width - LargeMargin, _windowBoundaries.height - SmallMargin, LargeMargin, SmallMargin),
                texture,
                ScaleMode.StretchToFill);
        }

        private bool ProcessClosing(Rect closeButtonBoundaries, Vector2 mousePosition)
        {
            if (GUIUtility.hasModalWindow)
            {
                return false;
            }

            if (closeButtonBoundaries.Contains(mousePosition))
            {
                bool isWindowIdle = !_isMoving && !_isResizing;
                if (InputController.IsLeftMouseButtonPressed && isWindowIdle)
                {
                    IsVisible = false;
                }

                return isWindowIdle;
            }

            return false;
        }

        private bool ProcessMoving(Rect titleBarBoundaries, Vector2 mousePosition)
        {
            if (GUIUtility.hasModalWindow)
            {
                return false;
            }

            if (_isMoving)
            {
                if (InputController.IsLeftMouseButtonPressed)
                {
                    var newBoundaries = _windowBoundaries;
                    newBoundaries.position = mousePosition + _moveDragHandle;

                    if (newBoundaries.x < 0f)
                    {
                        newBoundaries.x = 0f;
                    }

                    if (newBoundaries.y < 0f)
                    {
                        newBoundaries.y = 0f;
                    }

                    if (newBoundaries.xMax > Screen.width)
                    {
                        newBoundaries.x = Screen.width - newBoundaries.width;
                    }

                    if (newBoundaries.yMax > Screen.height)
                    {
                        newBoundaries.y = Screen.height - newBoundaries.height;
                    }

                    _windowBoundaries = newBoundaries;
                }
                else
                {
                    _isMoving = false;
                    ProcessWindowMoved();
                }

                return true;
            }

            if (titleBarBoundaries.Contains(mousePosition))
            {
                if (InputController.IsLeftMouseButtonPressed && !_isResizing)
                {
                    _isMoving = true;
                    _moveDragHandle = _windowBoundaries.position - mousePosition;
                }

                return !_isResizing;
            }

            return false;
        }

        private bool ProcessResizing(Rect resizeHandleBoundaries, Vector2 mousePosition)
        {
            if (GUIUtility.hasModalWindow)
            {
                return false;
            }

            if (_isResizing)
            {
                if (InputController.IsLeftMouseButtonPressed)
                {
                    var newBoundaries = _windowBoundaries;
                    var newSize = mousePosition - _windowBoundaries.position + _resizeDragHandle;

                    if (newSize.x < MinimumWindowWidth)
                    {
                        newSize.x = MinimumWindowWidth;
                    }

                    if (newSize.y < MinimumWindowHeight)
                    {
                        newSize.y = MinimumWindowHeight;
                    }

                    newBoundaries.size = newSize;

                    if (newBoundaries.xMax >= Screen.width)
                    {
                        newBoundaries.width = Screen.width - newBoundaries.x;
                    }

                    if (newBoundaries.yMax >= Screen.height)
                    {
                        newBoundaries.height = Screen.height - newBoundaries.y;
                    }

                    _windowBoundaries = newBoundaries;
                }
                else
                {
                    _isResizing = false;
                    ProcessWindowResized();
                }

                return true;
            }

            if (resizeHandleBoundaries.Contains(mousePosition))
            {
                if (InputController.IsLeftMouseButtonPressed && !_isMoving)
                {
                    _isResizing = true;
                    _resizeDragHandle = _windowBoundaries.position + _windowBoundaries.size - mousePosition;
                }

                return !_isMoving;
            }

            return false;
        }
    }
}
