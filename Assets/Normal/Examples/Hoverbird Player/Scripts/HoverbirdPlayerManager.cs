#if NORMCORE

using UnityEngine;
using UnityEngine.Animations;

namespace Normal.Realtime.Examples {
    public class HoverbirdPlayerManager : MonoBehaviour {
        [SerializeField] private GameObject _camera = default;
        [SerializeField] private GameObject _prefab;

        private Realtime _realtime;

        private void Awake() {
            // Get the Realtime component on this game object
            _realtime = GetComponent<Realtime>();

            // Notify us when Realtime successfully connects to the room
            _realtime.didConnectToRoom += DidConnectToRoom;
        }

        private void DidConnectToRoom(Realtime realtime) {
            // Instantiate the Player for this client once we've successfully connected to the room
            var options = new Realtime.InstantiateOptions {
                ownedByClient            = true,    // Make sure the RealtimeView on this prefab is owned by this client.
                preventOwnershipTakeover = true,    // Prevent other clients from calling RequestOwnership() on the root RealtimeView.
                useInstance              = realtime // Use the instance of Realtime that fired the didConnectToRoom event.
            };
            GameObject playerGameObject = Realtime.Instantiate(_prefab.name, options);

            // Get a reference to the player
            HoverbirdPlayer player = playerGameObject.GetComponent<HoverbirdPlayer>();

            // Get the constraint used to position the camera behind the player
            ParentConstraint cameraConstraint = _camera.GetComponent<ParentConstraint>();

            // Add the camera target so the camera follows it
            ConstraintSource constraintSource = new ConstraintSource { sourceTransform = player.cameraTarget, weight = 1.0f };
            int constraintIndex = cameraConstraint.AddSource(constraintSource);

            // Set the camera offset so it acts like a third-person camera.
            cameraConstraint.SetTranslationOffset(constraintIndex, new Vector3( 0.0f,  1.0f, -4.0f));
            cameraConstraint.SetRotationOffset   (constraintIndex, new Vector3(15.0f,  0.0f,  0.0f));
        }
    }
}

#endif
