# VRStarViewer

This project was built over the course of a few days back in late 2020 after I discovered the [Hyg Database](http://www.astronexus.com/hyg) and thought it would be fun to make a VR experience to visualize and explore it.

## Getting Started

After pulling the project down you'll want to make sure you load up the "WebXR - Space" scene. If you have an Oculus CV1 set up and connected you should only need to hit play to see it working.

NOTE: Sol, our home system, will initialize at your feet so youll need to move "down" a bit to find it

## Controls

To move around the experience, hold out your hand in the direction you want to travel and squeeze the grip. You will move in the direction of your hand by a factor of how far your hand is away from your head.

To view information about a star, hold out your hand and touch it. The name should appear above it. If the star has no name in the database it will show either as NN with its id number from other collections, or as "Unnamed"

WARNING: This can be an intense VR experience and is *not* for those who get queasy in VR easily. On a long enough timeline I would likely look into more pleasant VR locomotion solutions. It's built this way because I knew I could handle it and I wanted to get it up and running as fast as possible.

## Code Sample

There is an amount of 3rd party code and assets in this prototype, the code I wrote can be found in the "Assets/HFN/Scripts/Runtime" directory. StarBuilder.cs is really the meat of the simulation but the rest of the scripts there provide valuable context and utilities.