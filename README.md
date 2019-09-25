# Polaris
This repository contains the source code of the **Polaris** game, which you can download for free [here](https://m-ody.itch.io/polaris).

I worked as the solo programmer in this game, with a team working on level design, modelling and texturing.

The game was built in **Unity 2017.2.0f3**, but it will not build as it is in this repository because the lack of assets. This is only for reference.

# About the source
This code was developed from ground up to be performant, and if you play it without Unity's Post-Process effects, I think the goal was accomplished. With the post-process enabled, the game will suffer on the biggest rooms.
All of the systems and functions were written from scratch, except for the EZ Camera Shake plugin.
Some of the tricks used in the game are:
- pooling of decals and sounds;
- extensive use of collision layers;
- there's no instantiation or de-instantiation at runtime;
- everything that could be preloaded is preloaded;
- there's very little use of Find or GetComponent at runtime;

The game is also modular, so is not hard to add more guns, enemies or items.

# Additional Notes
Looking back, there's a lot of things which I would do differently today. However, I'm very proud of what this turned, despite the poor planning and lack of experience with such a big project.
