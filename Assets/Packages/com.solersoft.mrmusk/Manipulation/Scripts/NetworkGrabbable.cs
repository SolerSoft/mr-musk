using Normal.Realtime;
using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SolerSoft.MRMUSK.Manipulation
{
    /// <summary>
    /// Makes a Meta Quest grabbable object network grabbable.
    /// </summary>
    public class NetworkGrabbable : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        [Tooltip(
            "The Grab Events Transformer that is used to detect when the object is grabbed or released. " +
            "If this is not supplied, the applied object will be searched for a Grabbable and attempt to create one.")]
        private GrabEventsTransformer _grabEventsTransformer;

        [SerializeField]
        [Tooltip(
            "The Realtime Transform that will be used to control ownership. " +
            "If this is not supplied, the applied object will be searched for one.")]
        private RealtimeTransform _realtimeTransform;

        #endregion Private Fields

        #region Private Methods

        private void GrabEvents_Grabbed(object sender, EventArgs e)
        {
            _realtimeTransform.RequestOwnership();
        }

        private void GrabEvents_Released(object sender, EventArgs e)
        {
            _realtimeTransform.ClearOwnership();
        }

        /// <summary>
        /// Attempts to get or create the <see cref="GrabEventsTransformer" />.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the events transformer was obtained; otherwise <c>false</c>.
        /// </returns>
        private bool TryGetOrCreateGrabEvents()
        {
            // Do we already have it?
            if (_grabEventsTransformer != null) { return true; }

            // Try and get the Grabbable
            Grabbable grabbable = gameObject.GetComponent<Grabbable>();
            if (grabbable == null)
            {
                Debug.LogError($"{nameof(NetworkGrabbable)} could not obtain a {nameof(Grabbable)} and will be disabled.");
                return false;
            }

            // Create the transformer
            _grabEventsTransformer = gameObject.AddComponent<GrabEventsTransformer>();

            // Set it on the Grabbable
            grabbable.InjectOptionalOneGrabTransformer(_grabEventsTransformer);

            // Success
            return true;
        }

        /// <summary>
        /// Attempts to get the <see cref="RealtimeView" />.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the view was obtained; otherwise <c>false</c>.
        /// </returns>
        private bool TryGetRealtimeView()
        {
            // Passed in?
            if (_realtimeTransform != null) { return true; }

            // Attempt to locate
            _realtimeTransform = gameObject.GetComponent<RealtimeTransform>();
            if (_realtimeTransform == null)
            {
                Debug.LogError($"{nameof(NetworkGrabbable)} could not obtain a {nameof(RealtimeTransform)} and will be disabled.");
                return false;
            }

            // Success
            return true;
        }

        #endregion Private Methods

        #region Unity Message Handlers

        /// <inheritdoc />
        protected virtual void Start()
        {
            // Make sure we have all dependencies
            if (!TryGetOrCreateGrabEvents() || !TryGetRealtimeView())
            {
                enabled = false;
                return;
            }

            // Subscribe to the events
            _grabEventsTransformer.Grabbed += GrabEvents_Grabbed;
            _grabEventsTransformer.Released += GrabEvents_Released;
        }

        #endregion Unity Message Handlers
    }
}