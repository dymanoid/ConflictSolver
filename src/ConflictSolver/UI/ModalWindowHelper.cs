// <copyright file="ModalWindowHelper.cs" company="dymanoid">
// Copyright (c) dymanoid. All rights reserved.
// </copyright>

using ColossalFramework.UI;

namespace ConflictSolver.UI
{
    /// <summary>
    /// A special helper class for handling the modal UI. Prevents the mouse click-through for the mod's UI.
    /// </summary>
    internal sealed class ModalWindowHelper
    {
        private UIComponent _modalView;
        private bool _isModal;

        /// <summary>
        /// Updates the current modal state of the UI.
        /// </summary>
        /// <param name="mouseOverWindow">A value indicating whether the mouse cursor
        /// is currently over a mod's window.</param>
        public void UpdateModalState(bool mouseOverWindow)
        {
            if (_modalView == null)
            {
                _modalView = UIView.GetAView()?.AddUIComponent(typeof(UILabel));
                if (_modalView == null)
                {
                    return;
                }
            }

            if (InputController.MiddleMouseButtonState != MouseButtonState.None)
            {
                return;
            }

            if (mouseOverWindow)
            {
                if (!_isModal)
                {
                    _isModal = true;
                    UIView.PushModal(_modalView);
                }
            }
            else if (_isModal && UIView.GetModalComponent() == _modalView)
            {
                _isModal = false;
                UIView.PopModal();
            }
        }
    }
}
