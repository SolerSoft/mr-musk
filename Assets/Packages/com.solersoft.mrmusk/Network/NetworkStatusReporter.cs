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
        #region Private Fields

        [SerializeField]
        [Tooltip("The Realtime instance to monitor for status changes.")]
        private Realtime _realtime;

        [SerializeField]
        [Tooltip("The Text block to update with status messages.")]
        private TextMeshPro _text;

        #endregion Private Fields

        #region Unity Events

        private void OnEnable()
        {
            // _realtime.
        }

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
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