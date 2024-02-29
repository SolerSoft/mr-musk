# Mixed Reality Multi-User Starter Kit

## What is MR-MUSK?

Besides a silly name, it's also a Unity package for quickly prototyping shared Mixed Reality experiences on Meta Quest devices. It supports roughly 16 concurrent users, is compatible with Quest [Building Blocks](https://developer.oculus.com/documentation/unity/bb-overview/), and is especially well suited for hackathons and school courses.

## Doesn't Oculus already provide Multiplayer Samples?

YES! And if you fully intend to publish your app to [AppLab](https://developer.oculus.com/blog/introducing-app-lab-a-new-way-to-distribute-oculus-quest-apps/) or the [Quest Store](https://www.meta.com/experiences), you might want to consider starting with [Unity-LocalMultiplayerMR](https://github.com/oculus-samples/Unity-LocalMultiplayerMR) instead. However, keep in mind that project leverages [Shard Spatial Anchors](https://developer.oculus.com/blog/build-local-multiplayer-experiences-shared-spatial-anchors/) which in turn requires you to:

- Have a [verified](https://developer.oculus.com/policy/developer-verification/) Meta developer account (which needs a payment method or phone number).

- [Create an Organization](https://developer.oculus.com/resources/publish-account-management-intro/) for your company.

- [Create an App Record](https://developer.oculus.com/resources/publish-create-app/) in the [developer dashboard](https://developer.oculus.com/manage).

- Understand and complete a [Data Use Checkup](https://developer.oculus.com/resources/publish-data-use/) explaining how user data will be used.

- Enable “User ID” and “User Profile” in the Data Use Checkup.

- [Create Test User Accounts](https://developer.oculus.com/documentation/unity/unity-shared-spatial-anchors/?intern_source=devblog&intern_content=build-local-multiplayer-experiences-shared-spatial-anchors#create-test-users) for testing before publishing.

- [Add permissions](https://developer.oculus.com/documentation/unity/unity-shared-spatial-anchors/?intern_source=devblog&intern_content=build-local-multiplayer-experiences-shared-spatial-anchors#android-manifest) to the Android manifest (though the Oculus SDK helps with this).

- [Enable point cloud sharing](https://developer.oculus.com/documentation/unity/unity-shared-spatial-anchors/?intern_source=devblog&intern_content=build-local-multiplayer-experiences-shared-spatial-anchors#ensuring-share-point-cloud-data-is-enabled) on your device.

For hackathons and school settings, the above requirements may not be feasible for several reasons. This project allows MR colocation without any of the above requirements.
