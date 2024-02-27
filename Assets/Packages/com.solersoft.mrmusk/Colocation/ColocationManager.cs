using Newtonsoft.Json.Bson;
using SolerSoft.MRMUSK.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SolerSoft.MRMUSK.Colocation
{
    /// <summary>
    /// Localizes content in a multi-user experience.
    /// </summary>
    public class ColocationManager : MonoBehaviour
    {
        #region Constants

        /// <summary>
        /// The default buttons that should be held down to trigger colocation by controller.
        /// </summary>
        private static readonly OVRInput.Button[] s_ColocationButtons = { OVRInput.Button.One, OVRInput.Button.Two, OVRInput.Button.PrimaryIndexTrigger };

        #endregion Constants

        #region Private Fields

        private bool _isColocatingToController;

        #endregion Private Fields

        #region Unity Inspector Variables

        [Header("Camera Settings")]
        [SerializeField]
        [Tooltip("Whether to affect camera pitch during colocation. This is usually unchecked.")]
        private bool _affectPitch;

        [SerializeField]
        [Tooltip("Whether to affect camera roll during colocation. This is usually unchecked.")]
        private bool _affectRoll;

        [SerializeField]
        [Tooltip("The camera rig that should be moved when colocated. This must be assigned.")]
        private Transform _cameraRig;

        [Header("Controller Settings")]
        [SerializeField]
        [Tooltip("The buttons that should be held down to trigger colocation by controller.")]
        private OVRInput.Button[] _colocateButtons = s_ColocationButtons;

        [SerializeField]
        [Tooltip("The controller that should be used for controller-based colocation. This must be assigned.")]
        private OVRControllerHelper _controller;

        [Header("Offset Settings")]
        [SerializeField]
        [Tooltip("The positional offset of from the tracked device to consider the 'floor'. This offset might represent the height of a mount for the tracked device.")]
        private Vector3 _positionOffset;

        [SerializeField]
        [Tooltip("The rotational offset of from the tracked device to consider 'level'. This angle might represent a mount or holder for the tracked device.")]
        private Vector3 _rotationOffset;

        #endregion Unity Inspector Variables

        #region Private Methods

        /// <summary>
        /// Attempts to colocate to the controller.
        /// </summary>
        private bool TryColocateToController()
        {
            // If no controller defined, can't attempt to colocate to controller
            if (_controller == null)
            {
                Debug.LogError("Cannot colocate to controller because the controller has not been specified.");
                return false;
            }
            if ((_controller.m_controller != OVRInput.Controller.LTouch) && (_controller.m_controller != OVRInput.Controller.RTouch))
            {
                Debug.LogError("Cannot colocate to controller because only LTouch or RTouch are supported.");
                return false;
            }

            // Check to see if all the right buttons are held down on the target controller
            if (OVRInputHelper.GetAll(_controller.m_controller, _colocateButtons))
            {
                // Avoid re-entrance
                if (!_isColocatingToController)
                {
                    // We are now co-locating
                    _isColocatingToController = true;

                    // Colocate to world reference
                    ColocateTo(_controller.gameObject.transform);

                    // Success
                    return true;
                }
            }
            else
            {
                // OK to check again on next frame
                _isColocatingToController = false;
            }

            // Not colocated
            return false;
        }

        #endregion Private Methods

        #region Unity Message Handlers

        /// <inheritdoc />
        protected virtual void Start()
        {
            // Verify all dependencies are met
            if (_cameraRig == null)
            {
                Debug.LogError($"The player rig must be specified. {nameof(ColocationManager)} will be disabled.");
                enabled = false;
                return;
            }

            if (_controller == null)
            {
                Debug.LogError($"Controller helper must be specified. {nameof(ColocationManager)} will be disabled.");
                enabled = false;
                return;
            }
        }

        /// <inheritdoc />
        protected virtual void Update()
        {
            TryColocateToController();
        }

        #endregion Unity Message Handlers

        #region Public Methods

        /// <summary>
        /// Colocates to the specified transform.
        /// </summary>
        /// <param name="transform">
        /// The <see cref="Transform" /> to colocate to.
        /// </param>
        public void ColocateTo(Transform transform)
        {
            // Calculate the offset
            Quaternion rotationOffset = Quaternion.Euler(_rotationOffset);

            // The new rotation is the inverse of the target rotation multiplied by the current rotation
            _cameraRig.rotation = Quaternion.Inverse(transform.rotation * rotationOffset) * _cameraRig.rotation;

            // Now limit rotations to affected axis
            _cameraRig.rotation = Quaternion.Euler(_affectRoll ? _cameraRig.rotation.eulerAngles.x : 0, _cameraRig.rotation.eulerAngles.y, _affectPitch ? _cameraRig.rotation.eulerAngles.z : 0);

            // The new position is the old position offset by the target transforms NEGATIVE amount
            _cameraRig.transform.position = _cameraRig.transform.position + -(transform.position + _positionOffset);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets or sets whether to affect camera pitch during colocation. This is usually false.
        /// </summary>
        public bool AffectPitch { get => _affectPitch; set => _affectPitch = value; }

        /// <summary>
        /// Gets or sets whether to affect camera roll during colocation. This is usually false.
        /// </summary>
        public bool AffectRoll { get => _affectRoll; set => _affectRoll = value; }

        /// <summary>
        /// Gets or sets the camera rig that should be moved when colocated. This must be supplied.
        /// </summary>
        public Transform CameraRig { get => _cameraRig; set => _cameraRig = value; }

        /// <summary>
        /// Gets or sets the buttons that should be held down to trigger colocation by controller.
        /// </summary>
        public OVRInput.Button[] ColocateButtons { get => _colocateButtons; set => _colocateButtons = value; }

        /// <summary>
        /// The controller helper that represents the controller to be used for colocation. This
        /// must be supplied.
        /// </summary>
        public OVRControllerHelper ControllerHelper { get => _controller; set => _controller = value; }

        #endregion Public Properties
    }
}