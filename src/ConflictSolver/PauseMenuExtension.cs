// <copyright file="PauseMenuExtension.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using System;
using ColossalFramework.UI;

namespace ConflictSolver
{
    /// <summary>
    /// A game pause menu extension utility. Adds a button to the menu.
    /// </summary>
    internal sealed class PauseMenuExtension
    {
        private const string PauseMenuLibraryItem = "PauseMenu";
        private const string PauseMenuPanel = "Menu";
        private const string MenuItemTemplate = "PauseMenuButtonTemplate";
        private readonly string _buttonName;
        private readonly Action _buttonAction;
        private UIButton _button;

        /// <summary>
        /// Initializes a new instance of the <see cref="PauseMenuExtension"/> class.
        /// </summary>
        /// <param name="buttonName">The button caption to display.</param>
        /// <param name="buttonAction">A delegate that will be called when the button is clicked.</param>
        public PauseMenuExtension(string buttonName, Action buttonAction)
        {
            if (string.IsNullOrEmpty(buttonName))
            {
                throw new ArgumentException("message", nameof(buttonName));
            }

            _buttonName = buttonName;
            _buttonAction = buttonAction ?? throw new ArgumentNullException(nameof(buttonAction));
        }

        /// <summary>
        /// Adds the custom button in the pause menu if the button is not already added.
        /// </summary>
        public void Enable()
        {
            if (_button != null)
            {
                return;
            }

            var pauseMenu = UIView.library.Get<PauseMenu>(PauseMenuLibraryItem);
            var pauseMenuPanel = pauseMenu?.Find<UIPanel>(PauseMenuPanel);
            if (pauseMenuPanel is null)
            {
                return;
            }

            _button = CreateButton(MenuItemTemplate, _buttonName, pauseMenuPanel);

            var menuSize = pauseMenu.component.size;
            menuSize.y += _button.size.y + pauseMenuPanel.autoLayoutPadding.vertical;
            pauseMenu.component.size = menuSize;
        }

        /// <summary>
        /// Removes the custom button from the pause menu if the button exists there.
        /// </summary>
        public void Disable()
        {
            if (_button is null)
            {
                return;
            }

            var pauseMenu = UIView.library.Get<PauseMenu>(PauseMenuLibraryItem);
            var pauseMenuPanel = pauseMenu?.Find<UIPanel>(PauseMenuPanel);
            if (pauseMenuPanel is null)
            {
                return;
            }

            RemoveButton(_button, pauseMenuPanel);
            var menuSize = pauseMenu.component.size;
            menuSize.y -= _button.size.y + pauseMenuPanel.autoLayoutPadding.vertical;
            pauseMenu.component.size = menuSize;
            _button = null;
        }

        private UIButton CreateButton(string menuItemPrefab, string caption, UIComponent parent)
        {
            var gameObject = UITemplateManager.GetAsGameObject(menuItemPrefab);
            gameObject.name = caption;
            var button = (UIButton)parent.AttachUIComponent(gameObject);
            button.text = caption;
            button.eventClick += Click;
            return button;
        }

        private void RemoveButton(UIButton button, UIComponent parent)
        {
            button.eventClick -= Click;
            parent.RemoveUIComponent(button);
            UITemplateManager.RemoveInstance(button.text, parent);
        }

        private void Click(UIComponent component, UIMouseEventParameter eventParam) => _buttonAction();
    }
}
