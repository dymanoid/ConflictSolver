// <copyright file="WindowArea.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using UnityEngine;

namespace ConflictSolver.UI
{
    /// <summary>
    /// A UI helper class that handles the layout of a window area.
    /// </summary>
    internal sealed class WindowArea
    {
        private readonly Vector2 _margin;
        private readonly IWindow _window;

        private Vector2 _absoluteOffset;
        private Vector2 _relativeOffset;

        private Vector2 _absoluteSize;
        private Vector2 _relativeSize = Vector2.one;

        private bool _drawingArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowArea"/> class without any margins.
        /// </summary>
        /// <param name="window">The <see cref="IWindow"/> this are belongs to.</param>
        public WindowArea(IWindow window)
            : this(window, 0f, 0f)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowArea"/> class with specified <paramref name="margin"/>.
        /// </summary>
        /// <param name="window">The <see cref="IWindow"/> this are belongs to.</param>
        /// <param name="margin">The vertical and horizontal margin of the area.</param>
        public WindowArea(IWindow window, float margin)
            : this(window, margin, margin)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowArea"/> class with specified horizontal
        /// and vertical margins.
        /// </summary>
        /// <param name="window">The <see cref="IWindow"/> this are belongs to.</param>
        /// <param name="horizontalMargin">The horizontal margin of the area.</param>
        /// <param name="verticalMargin">The vertical margin of the area.</param>
        public WindowArea(IWindow window, float horizontalMargin, float verticalMargin)
        {
            _window = window ?? throw new System.ArgumentNullException(nameof(window));
            _margin = new Vector2(horizontalMargin, verticalMargin);
        }

        /// <summary>
        /// Offsets this area in absolute coordinates by specified amount of units.
        /// </summary>
        /// <param name="horizontal">The absolute horizontal offset. If <c>null</c>, the current offset will not be changed.</param>
        /// <param name="vertical">The absolute vertical offset. If <c>null</c>, the current offset will not be changed.</param>
        /// <returns>This instance for easy method chaining.</returns>
        public WindowArea OffsetBy(float? horizontal = null, float? vertical = null)
        {
            _absoluteOffset = new Vector2(horizontal ?? _absoluteOffset.x, vertical ?? _absoluteOffset.y);
            return this;
        }

        /// <summary>
        /// Offsets this area relatively to the parent window.
        /// </summary>
        /// <param name="horizontal">The relative horizontal offset (-1 to 1). If <c>null</c>,
        /// the current offset will not be changed.</param>
        /// <param name="vertical">The relative vertical offset (-1 to 1). If <c>null</c>,
        /// the current offset will not be changed.</param>
        /// <returns>This instance for easy method chaining.</returns>
        public WindowArea OffsetRelative(float? horizontal = null, float? vertical = null)
        {
            _relativeOffset = new Vector2(
                Mathf.Clamp(horizontal ?? _relativeOffset.x, -1f, 1f),
                Mathf.Clamp(vertical ?? _relativeOffset.y, -1f, 1f));
            return this;
        }

        /// <summary>
        /// Changes the size of this area by specified absolute units.
        /// </summary>
        /// <param name="width">The absolute width change. If <c>null</c>, the current size will not be changed.</param>
        /// <param name="height">The absolute height change. If <c>null</c>, the current size will not be changed.</param>
        /// <returns>This instance for easy method chaining.</returns>
        public WindowArea ChangeSizeBy(float? width = null, float? height = null)
        {
            _absoluteSize = new Vector2(width ?? _absoluteSize.x, height ?? _absoluteSize.y);
            return this;
        }

        /// <summary>
        /// Changes the size of this area relatively to the parent window.
        /// </summary>
        /// <param name="width">The relative width value (-1 to 1). If <c>null</c>, the current size will not be changed.</param>
        /// <param name="height">The relative height value (-1 to 1). If <c>null</c>, the current size will not be changed.</param>
        /// <returns>This instance for easy method chaining.</returns>
        public WindowArea ChangeSizeRelative(float? width = null, float? height = null)
        {
            _relativeSize = new Vector2(
                Mathf.Clamp(width ?? _relativeSize.x, -1f, 1f),
                Mathf.Clamp(height ?? _relativeSize.y, -1f, 1f));
            return this;
        }

        /// <summary>
        /// Begins the drawing of this area.
        /// </summary>
        /// <returns><c>true</c> if the area can be laid out; otherwise, <c>false</c>.</returns>
        public bool Begin()
        {
            var bounds = GetBounds();
            if (bounds == Rect.zero)
            {
                return false;
            }

            GUILayout.BeginArea(bounds);
            _drawingArea = true;
            return true;
        }

        /// <summary>
        /// Ends the drawing of this area.
        /// </summary>
        public void End()
        {
            if (_drawingArea)
            {
                GUILayout.EndArea();
                _drawingArea = false;
            }
        }

        private Rect GetBounds()
        {
            var windowRect = _window.WindowBoundaries;

            return new Rect(
                _absoluteOffset.x + _relativeOffset.x * windowRect.width + _margin.x,
                _absoluteOffset.y + _relativeOffset.y * windowRect.height + _margin.y,
                _absoluteSize.x + _relativeSize.x * windowRect.width - _margin.x * 2,
                _absoluteSize.y + _relativeSize.y * windowRect.height - _margin.y * 2);
        }
    }
}
