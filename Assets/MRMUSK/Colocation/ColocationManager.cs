using Newtonsoft.Json.Bson;
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
        #region Private Fields
        private bool _isColocating;
        #endregion Private Fields

        #region Unity Inspector Variables

        [SerializeField]
        [Tooltip("The transform that represents the player rig.")]
        private Transform _playerRig;

        [SerializeField]
        [Tooltip("The transform that will be used as a reference for world center. This is often a controller but could be any tracked device.")]
        private Transform _worldCenterReference;

        #endregion Unity Inspector Variables

        #region Private Methods

        /// <summary>
        /// Checks if we should Colocate based on controller input.
        /// </summary>
        private void TryColocate()
        {
            // Check to see if all buttons are held
            if (OVRInput.Get(OVRInput.Button.One) && OVRInput.Get(OVRInput.Button.Two) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                // Avoid re-entrance
                if (!_isColocating)
                {
                    // We are now co-locating
                    //_isColocating = true;

                    // Colocate to world reference
                    Colocate();
                }
            }
            else
            {
                // OK to check again on next frame
                _isColocating = false;
            }
        }

        #endregion Private Methods

        #region Unity Message Handlers

        /// <inheritdoc />
        protected virtual void Start()
        {
            // Verify all dependencies are met
            if (_playerRig == null)
            {
                Debug.LogError($"The player rig must be specified. {nameof(ColocationManager)} will be disabled.");
                enabled = false;
                return;
            }

            if (_worldCenterReference == null)
            {
                Debug.LogError($"World center reference must be specified. {nameof(ColocationManager)} will be disabled.");
                enabled = false;
                return;
            }
        }

        /// <inheritdoc />
        protected virtual void Update()
        {
            TryColocate();
        }

        #endregion Unity Message Handlers

        #region Public Methods

        /// <summary>
        /// Colocates to <see cref="WorldCenterReference" />.
        /// </summary>
        public void Colocate()
        {
            ColocateTo(_worldCenterReference);
        }

        /*
        /// <summary>
        /// Colocates to the specified position and rotation.
        /// </summary>
        /// <param name="worldPosition">
        /// The world center to colocate to.
        /// </param>
        /// <param name="worldRotation">
        /// The world rotation to colocate to.
        /// </param>
        public void ColocateTo(Vector3 worldPosition, Quaternion worldRotation)
        {
            // Move player rig opposite of position, which will make the position now 0,0,0 _playerRig.transform.Translate(-worldCenter);

            // Rotate the player rig to the opposite Y angle of rotation
            // _playerRig.transform.Rotate(Vector3.up, worldRotation.eulerAngles.y * -1);
            // _playerRig.transform.localEulerAngles = new Vector3(0, -worldRotation.eulerAngles.y, 0);

            // Calculate the relative position of GOB from the perspective of GOA
            Vector3 relativePosition = _playerRig.InverseTransformPoint(worldPosition);

            // Calculate the relative rotation of GOB from the perspective of GOA
            Quaternion relativeRotation = Quaternion.Inverse(_playerRig.rotation) * worldRotation;

            // Move GOA to make GOB appear at position (0,0,0) and rotation (0,0,0) from its perspective
            _playerRig.position -= relativePosition;
            // _playerRig.rotation *= Quaternion.Euler(0, -relativeRotation.eulerAngles.y, 0);
        }
        */

        /// <summary>
        /// Colocates to the specified transform.
        /// </summary>
        /// <param name="transform">
        /// The <see cref="Transform" /> to colocate to.
        /// </param>
        public void ColocateTo(Transform transform)
        {
            // Get difference between player rig to transform
            Vector3 rigToTransformDistance = _playerRig.position - transform.position;

            // Get difference between player rig to transform rotation
            float rigToTransformAngle = _playerRig.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;

            /*
            // Translate
            _playerRig.Translate(-transform.position);

            // Rotate first
            _playerRig.Rotate(Vector3.up, -transform.rotation.eulerAngles.y, Space.World);
            // _playerRig.rotation = Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0);

            // _playerRig.Translate(transform.InverseTransformPoint(Vector3.zero));
            */

            // Calculate the translation and rotation needed to reset the child's world position and rotation
            Vector3 childPosition = transform.position;
            Quaternion childRotation = transform.rotation;

            // Reverse the child's position and rotation to get the offset needed for the parent
            Vector3 offsetPosition = -childPosition;
            Quaternion offsetRotation = Quaternion.Inverse(childRotation);

            // Apply the offset to the parent's position and rotation _playerRig.rotation =
            // Quaternion.Euler(0, offsetRotation.eulerAngles.y - rigToTransformAngle, 0);
            // _playerRig.Rotate(Vector3.up, -transform.rotation.eulerAngles.y +
            // rigToTransformAngle, Space.World);
            _playerRig.Rotate(Vector3.up, -transform.rotation.eulerAngles.y, Space.World);

            // _playerRig.position += offsetPosition; _playerRig.Translate(-rigToTransformDistance);
            // _playerRig.position = -(transform.position - rigToTransformDistance);
            _playerRig.Translate(-transform.position);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets or sets the transform that represents the player rig.
        /// </summary>
        public Transform PlayerRig { get => _playerRig; set => _playerRig = value; }

        /// <summary>
        /// Gets or sets the transform that will be used as a reference for world center.
        /// </summary>
        /// <remarks>
        /// This is often a controller but could be any tracked device.
        /// </remarks>
        public Transform WorldCenterReference { get => _worldCenterReference; set => _worldCenterReference = value; }

        #endregion Public Properties
    }
}