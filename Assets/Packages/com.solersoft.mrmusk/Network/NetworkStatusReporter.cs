using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;

namespace SolerSoft.MRMUSK.Network
{
    /// <summary>
    /// Updates a text control with status messages from the networking layer.
    /// </summary>
    public class NetworkStatusReporter : MonoBehaviour
    {
        #region Constants

        private const float UPDATE_RATE = 1.0f;

        #endregion Constants

        #region Private Fields

        private float _lastUpdateTime;

        #endregion Private Fields

        #region Unity Inspector Variables

        [SerializeField]
        [Tooltip("The Realtime instance to monitor for status changes.")]
        private Realtime _realtime;

        [SerializeField]
        [Tooltip("The Text block to update with status messages.")]
        private TextMeshPro _text;

        #endregion Unity Inspector Variables

        #region Private Methods

        private void TryUpdateStatus()
        {
            // Make sure we have the components we need
            if (_realtime == null || _text == null) { return; }

            // Update based on status
            if (_realtime.disconnected)
            {
                // Not connected. Configured?
                if ((_realtime.normcoreAppSettings != null) && (!string.IsNullOrEmpty(_realtime.normcoreAppSettings.normcoreAppKey)))
                {
                    _text.text = "Disconnected.";
                }
                else
                {
                    _text.text = "ERROR: Check Realtime Settings";
                }
            }
            else if (_realtime.connecting)
            {
                _text.text = "Connecting...";
            }
            else if (_realtime.connected)
            {
                // Are we in a room?
                if (_realtime.room != null)
                {
                    _text.text = $"Connected to '{_realtime.room.name}'.";
                }
                else
                {
                    // Are we trying to connect to a room?
                    if (!string.IsNullOrEmpty(_realtime.roomToJoinOnStart))
                    {
                        _text.text = $"Connected, joining '{_realtime.roomToJoinOnStart}'...";
                    }
                    else
                    {
                        _text.text = $"Connected to server but not to room.";
                    }
                }
            }
        }

        #endregion Private Methods

        #region Unity Events

        private void OnEnable()
        {
            TryUpdateStatus();
        }

        private void Update()
        {
            // Don't update on every frame
            if (Time.time >= (_lastUpdateTime + UPDATE_RATE))
            {
                _lastUpdateTime = Time.time;
                TryUpdateStatus();
            }
        }

        #endregion Unity Events

        #region Public Properties

        /// <summary>
        /// Gets or sets the Realtime instance to monitor for status changes.
        /// </summary>
        public Realtime Realtime { get => _realtime; set => _realtime = value; }

        /// <summary>
        /// Gets or sets the Text block to update with status messages.
        /// </summary>
        public TextMeshPro Text { get => _text; set => _text = value; }

        #endregion Public Properties
    }
}