# FPS - MonoBehaviour to DOTS

This repository contains two Unity projects. `FPS_MonoBehaviour` which
is a version only using natives Unity components. And `FPS_DOTS` which
is a version using the DOTS stack.

Both projects are based on the same game design: a simple FPS where the 
player must kill a large amount of enemies. But the game is not the goal
of this repo, it exists to try DOTS and how "easy" it is to get an equivalent
to Mono.

## How to use

Both folders are an entire Unity project. Clone the repo and open the folder
you want in your Unity Hub.

In the project, you will find a scene to open in folder `_Scenes`. 
To tweak values for Mono, you can go on the `GameManager` GameObject and change
the values you need.
To tweak values for DOTS, you can go in the `EnemySubscene` and click the `Config`
GameObject Authoring which contains values to modify.

## Optimizations

Currently, there are no optimizations in both projects. The goal is to stress
test games to see where are the real improvements. 
Spoiler: there are not really so impressive. 

## Credits

- [Sci-Fi Modular Gun Pack](https://quaternius.com/packs/scifimodularguns.html)
- [Ultimate Stylized Nature Pack](https://quaternius.com/packs/ultimatestylizednature.html)
- [Ultimate Monsters](https://quaternius.com/packs/ultimatemonsters.html)