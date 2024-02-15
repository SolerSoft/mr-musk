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

        private IAnchorStore _anchorStore;

        #endregion Private Fields

        #region Unity Inspector Variables

        [SerializeField]
        [Tooltip("Whether multiple users are in the same room.")]
        private bool _inSameRoom = true;

        [SerializeField]
        [Tooltip("Whether to localize automatically on start.")]
        private bool _localizeOnStart = true;

        [SerializeField]
        [Tooltip("The UI to display log text.")]
        private TextMeshProUGUI _logText;

        [SerializeField]
        [Tooltip("The stage where colocated content is presented.")]
        private GameObject _stage;

        #endregion Unity Inspector Variables

        #region Unity Message Handlers

        /// <inheritdoc />
        private void Start()
        {
            // If no stage is provided, use current GameObject
            if (_stage == null) { _stage = gameObject; }

            // Get the anchor store
            _anchorStore = GetComponent<IAnchorStore>();

            // Ensure we got the anchor store
            if (_anchorStore == null)
            {
                LogError($"{nameof(ColocationManager)} requires a component that implements {nameof(IAnchorStore)} and will be disabled.");
                enabled = false;
                return;
            }

            // Localize?
            if (_localizeOnStart)
            {
                var t = LocalizeAsync();
            }
        }

        #endregion Unity Message Handlers

        #region Private Methods

        private void Log(string message)
        {
            Debug.Log(message);
            if (_logText != null) { _logText.text = message; }
        }

        private void LogError(string message)
        {
            Debug.Log(message);
            if (_logText != null) { _logText.text = message; }
        }

        private void LogWarning(string message)
        {
            Debug.Log(message);
            if (_logText != null) { _logText.text = message; }
        }

        private IEnumerator SaveAnchorToCloudRoutine(OVRSpatialAnchor osAnchor, TaskCompletionSource<bool> tcs)
        {
            // If in the unity editor this will never complete
            if (Application.isEditor)
            {
                Debug.LogWarning("Spatial anchors cannot be saved in the editor. Ignoring.");
                tcs.SetResult(true);
                yield break;
            }

            // Wait for anchor to e created and localized
            while (!osAnchor.Created && !osAnchor.Localized)
            {
                yield return new WaitForEndOfFrame(); //keep checking
            }

            try
            {
                // Save and use callback to complete the task
                osAnchor.Save((anchor, success) =>
                {
                    tcs.SetResult(success);
                });
            }
            catch (Exception e)
            {
                // Bad things happened, let the caller know
                tcs.SetException(e);
            }
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Binds the specified anchor to the stage.
        /// </summary>
        /// <param name="unboundAnchor">
        /// The anchor to bind.
        /// </param>
        /// <remarks>
        /// Note that the anchor must already be located. If it's not, use
        /// <see cref="TryLocalizeToAnchorAsync(OVRSpatialAnchor.UnboundAnchor)" /> instead.
        /// </remarks>
        public void BindToAnchor(OVRSpatialAnchor.UnboundAnchor unboundAnchor)
        {
            // Validate
            if (!unboundAnchor.Localized) { throw new InvalidOperationException($"{nameof(unboundAnchor)} has not been localized."); }

            // Log
            Log("Attempting to bind to anchor...");

            // Get the pose
            var pose = unboundAnchor.Pose;

            // Move the stage to the pose
            _stage.transform.position = pose.position;
            _stage.transform.rotation = pose.rotation;

            // Get or add the spatial anchor
            var _spatialAnchor = _stage.GetOrAddComponent<OVRSpatialAnchor>();

            // Bind the spatial anchor
            unboundAnchor.BindTo(_spatialAnchor);
        }

        /// <summary>
        /// Localizes the stage.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> that represents the operation.
        /// </returns>
        public async Task LocalizeAsync()
        {
            // Log
            Log("Attempting to localize...");

            // If we are not in the same room as other users
            if (!_inSameRoom)
            {
                // Just localize to floor
                await LocalizeToFloorAsync();

                // And we're done
                return;
            }

            // Try to localize to the stored anchor
            bool localizedToStore = await TryLocalizeToStoreAsync();

            // If not successful, we need to localize
            if (!localizedToStore)
            {
                // Log
                Log("Could not localize to store, localizing to floor instead.");

                // Localize to the floor
                await LocalizeToFloorAsync();

                // If we're in the same room we need to save to store and cloud
                if (_inSameRoom)
                {
                    // Log
                    Log("Since multiple users are in the same room, saving to cloud and store...");

                    // Save
                    await SaveToCloudAndStoreAsync();
                }
            }
        }

        /// <summary>
        /// Localizes the stage to the current room.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> that represents the operation.
        /// </returns>
        /// <remarks>
        /// This localizes the stage to the center of the floor.
        /// </remarks>
        public async Task LocalizeToFloorAsync()
        {
            // Log
            Log("NOT Attempting to localize to the floor...");

            // TODO:
        }

        /// <summary>
        /// Saves the current stage location to the store and cloud.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> that yields the result of the operation.
        /// </returns>
        public async Task SaveToCloudAndStoreAsync()
        {
            // Log
            Log("Saving anchor to cloud...");

            // If there is no spatial anchor, add it
            var anchor = _stage.GetOrAddComponent<OVRSpatialAnchor>();

            // Create a task completion source for the save
            TaskCompletionSource<bool> saveSource = new();

            // Start save process
            StartCoroutine(SaveAnchorToCloudRoutine(anchor, saveSource));

            // Wait for it to complete
            var success = await saveSource.Task;

            // If not successful, we can't save
            if (!success) { throw new InvalidOperationException("Could not save anchor to cloud."); }

            // Log
            Log("Saving anchor to store...");

            // Save to the store
            await _anchorStore.SaveAnchorIdAsync(anchor.Uuid);

            // Log
            Log("Anchor saved to cloud and store...");
        }

        /// <summary>
        /// Attempts to localize the stage to the specified anchor ID.
        /// </summary>
        /// <param name="anchorId">
        /// The ID of the anchor to localize to.
        /// </param>
        /// <returns>
        /// A <see cref="Task" /> that represents the operation.
        /// </returns>
        public async Task<bool> TryLocalizeToAnchorAsync(Guid anchorId)
        {
            // Log
            Log($"Attempting to load cloud anchor '{anchorId}'...");

            // Create the load options
            OVRSpatialAnchor.LoadOptions loadOptions = new OVRSpatialAnchor.LoadOptions
            {
                Timeout = 0,
                StorageLocation = OVRSpace.StorageLocation.Cloud,
                Uuids = new Guid[] { anchorId }
            };

            // Attempt to load anchors
            var loadedAnchors = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(loadOptions);

            // Did we get the unbound anchor?
            if (loadedAnchors.Length > 0)
            {
                // Bind
                BindToAnchor(loadedAnchors[0]);
                return true;
            }
            else
            {
                // Log
                LogWarning($"Cloud anchor '{anchorId}' could not be loaded...");
                return false;
            }
        }

        /// <summary>
        /// Attempts to localize the stage to the specified unbound anchor.
        /// </summary>
        /// <param name="unboundAnchor">
        /// The unbound anchor to localize to.
        /// </param>
        /// <returns>
        /// A <see cref="Task" /> that represents the operation.
        /// </returns>
        public async Task<bool> TryLocalizeToAnchorAsync(OVRSpatialAnchor.UnboundAnchor unboundAnchor)
        {
            // Already localized?
            if (unboundAnchor.Localized)
            {
                // Already localized, bind
                BindToAnchor(unboundAnchor);
                return true;
            }

            // Not already localized, localize
            var success = await unboundAnchor.LocalizeAsync();

            // If successful, bind
            if (success)
            {
                BindToAnchor(unboundAnchor);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to localize the stage to the stored anchor.
        /// </summary>
        /// <returns>
        /// A <see cref="Task" /> that yields the result of the operation.
        /// </returns>
        public async Task<bool> TryLocalizeToStoreAsync()
        {
            // Log
            Log("Trying to loading anchor ID from store...");

            // Attempt to load the anchor ID
            var anchorId = await _anchorStore.LoadAnchorIdAsync();

            // If the anchor is empty, nothing else to do
            if (anchorId == Guid.Empty)
            {
                // Log
                Log("No anchor ID found in the store...");
                return false;
            }

            // Try to localize by anchor ID
            return await TryLocalizeToAnchorAsync(anchorId);
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        /// Gets or sets whether multiple users are in the same room.
        /// </summary>
        public bool InSameRoom { get => _inSameRoom; set => _inSameRoom = value; }

        /// <summary>
        /// Gets or sets whether to localize automatically on start.
        /// </summary>
        public bool LocalizeOnStart { get => _localizeOnStart; set => _localizeOnStart = value; }

        /// <summary>
        /// Gets or sets the stage where colocated content is presented.
        /// </summary>
        public GameObject Stage { get => _stage; set => _stage = value; }

        #endregion Public Properties
    }
}