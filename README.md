# Mixed Reality Multi-User Starter Kit

https://github.com/SolerSoft/mr-musk/assets/149444736/563eca7d-10d3-45e3-bf57-6556dd57630d

## What is MR-MUSK?

Besides a silly name, it's also a library for quickly prototyping shared Mixed Reality experiences on Meta Quest devices. It supports colocation of [up to 16 users](https://normcore.io/documentation/essentials/common-questions#how-many-players-can-i-fit-in-a-single-room), is compatible with Quest [Building Blocks](https://developer.oculus.com/documentation/unity/bb-overview/), and is especially well suited for hackathons and college courses.

## What is colocation?

asdf

## Getting Started

asdf

## Didn't Oculus already provide a Multiplayer Sample?

YES! And if you fully intend to publish to AppLab or the Quest Store you might want to start with [Unity-LocalMultiplayerMR](https://github.com/oculus-samples/Unity-LocalMultiplayerMR) instead. However, keep in mind that project uses Shared Spatial Anchors, and SSA's are incredibly difficult to use in hackathons and college settings. For more information see [The Problem with Shared Anchors](#the-problem-with-shared-anchors).

## Without Shared Anchors, how do you colocate?

Ideally we'd use a QR code or image target, but since 3rd party apps can't access the camera we need something else that's trackable. Luckily, we have something trackable in the form of controllers. We just need a way to get those controllers into a precise location, and for that we use a simple controller stand.

<img title="" src="Docs/Images/ControllersInStand.jpg" alt="ControllersInStand.jpg" width="400">

At app launch, each user places their controller into the stand and presses a button combination to colocate (**Button 1** + **Button 2** + **Trigger** by default). After that, the controller can be removed from the stand and the experience proceeds normally.

> **Advanced Note**: Since the camera transform itself is updated during colocation, Mixed Reality experiences can be prototyped quickly in Unity World Space rather than needing to think about dividing the word across anchors. This is quite helpful to developers who are just getting started with Mixed Reality, but learning how to design for spatial anchors is still highly valuable in a curriculum.

## Where can I get the controller stand?

We 3D printed [this stand](https://www.printables.com/model/618477-meta-quest-3-controller-stand) ([Prusa version](https://www.printables.com/model/706332-meta-quest-3-controller-stand)) but any stand will do. If you like this stand we recommend scaling it down to 95% before printing. This makes the controller fit nice and snug, which helps improve accuracy for colocating.

## What if I need to use a different stand or location?

No problem! The **Offset Settings** in [Colocation Manager](Assets/Packages/com.solersoft.mrmusk/Colocation/ColocationManager.cs) can handle this. 

<img src="Docs/Images/ColocationManager.png" title="" alt="ColocationManager.png" width="393">

Update **Rotation Offset** to match the angles of your stand, and optionally use **Position Offset** to specify a distance between the stand and the floor.

## The Problem with Shared Anchors

So what *is* the problem with [Shared Spatial Anchors](https://developer.oculus.com/blog/build-local-multiplayer-experiences-shared-spatial-anchors/) anyway, and why all this controller stuff?

It's actually less of a problem with Shared Spatial Anchors and more a problem with the setup required to use them. To use Shared Anchors, even in a prototype, Meta currently requires developers to:

- Have a [verified](https://developer.oculus.com/policy/developer-verification/) developer account (which in turn requires a payment method or phone number).

- [Create an Organization](https://developer.oculus.com/resources/publish-account-management-intro/) for their company.

- [Create an App Record](https://developer.oculus.com/resources/publish-create-app/) in the [developer dashboard](https://developer.oculus.com/manage).

- Understand and complete a [Data Use Checkup](https://developer.oculus.com/resources/publish-data-use/) explaining how user data will be used.

- Enable “User ID” and “User Profile” in the Data Use Checkup.

- [Create Test User Accounts](https://developer.oculus.com/documentation/unity/unity-shared-spatial-anchors/?intern_source=devblog&intern_content=build-local-multiplayer-experiences-shared-spatial-anchors#create-test-users) to use for testing before publishing.

- [Add permissions](https://developer.oculus.com/documentation/unity/unity-shared-spatial-anchors/?intern_source=devblog&intern_content=build-local-multiplayer-experiences-shared-spatial-anchors#android-manifest) to the Android manifest (though the SDK helps with this).

- [Enable point cloud sharing](https://developer.oculus.com/documentation/unity/unity-shared-spatial-anchors/?intern_source=devblog&intern_content=build-local-multiplayer-experiences-shared-spatial-anchors#ensuring-share-point-cloud-data-is-enabled) on their device.

For hackathons and college settings, the above requirements are often not feasible or require too much time. This library allows projects to start quickly without any of the above requirements.
