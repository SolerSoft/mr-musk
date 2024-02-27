using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolerSoft.MRMUSK.Input
{
    public static class OVRInputHelper
    {
        #region Public Methods

        /// <summary>
        /// Gets a value that indicates if all of the buttons are held.
        /// </summary>
        /// <param name="controller">
        /// The controller to test.
        /// </param>
        /// <param name="buttons">
        /// The buttons to test.
        /// </param>
        /// <returns>
        /// <c>true</c> if all specified buttons are held; otherwise <c>false</c>.
        /// </returns>
        public static bool GetAll(OVRInput.Controller controller, params OVRInput.Button[] buttons)
        {
            foreach (var button in buttons)
            {
                if (!OVRInput.Get(button, controller)) { return false; }
            }
            return true;
        }

        #endregion Public Methods
    }
}