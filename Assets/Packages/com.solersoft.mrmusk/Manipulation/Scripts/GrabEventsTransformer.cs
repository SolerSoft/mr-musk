using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SolerSoft.MRMUSK.Manipulation
{
    /// <summary>
    /// A grab transformer that raises events for when an object is grabbed, moved and released.
    /// </summary>
    /// <remarks>
    /// Thank you to Fahruz for this
    /// <see cref="https://communityforums.atmeta.com/t5/Unity-VR-Development/Check-if-an-object-is-grabbed/m-p/1042725/highlight/true#M21880">sample implementation</see>.
    /// </remarks>
    public class GrabEventsTransformer : OneGrabFreeTransformer, ITransformer
    {
        #region Public Methods

        public new void BeginTransform()
        {
            base.BeginTransform();
            Grabbed?.Invoke(gameObject, EventArgs.Empty);
        }

        public new void EndTransform()
        {
            base.EndTransform();
            Released?.Invoke(gameObject, EventArgs.Empty);
        }

        public new void Initialize(IGrabbable grabbable)
        {
            base.Initialize(grabbable);
        }

        public new void UpdateTransform()
        {
            base.UpdateTransform();
            Moved?.Invoke(gameObject, EventArgs.Empty);
        }

        #endregion Public Methods

        #region Public Events

        public event EventHandler Grabbed;

        public event EventHandler Moved;

        public event EventHandler Released;

        #endregion Public Events
    }
}