#  Village Arsonist
To play the game, check it out on itch.io: https://kwoodhouse.itch.io/village-arsonist

Open-sourcing most of this project. The code is probably not great and I've no doubt done some unpleasant things in Unity through a lack of knowledge or staying up too late to work on it. Still preserving it here for future reference and in case anyone is interested in reusing any of it.

## Removed assets
I can't commit 3rd party assets to the public version of this repository (particularly paid ones). If you want to build the project, you'll need to import or replace the assets listed below.

### Unity Asset Store Packages
You'll need to import some files from the following Unity Asset Store packages:
* Pixel Art Icon Pack - RPG (free)
* Pixel Art Platformer - Village House (paid)
* Pixel Art Platformer - Village Props (free)
* Pixel Font - Tripfive (free)

> #### Unused imports?
> You won't need the whole packages but I don't have the patience to make a list of every file you do/don't need from them. If you want to trim the fat, in the Unity Editor, you can select all the scenes in `Assets/Scenes` > Right Click > `Export package...`. The window that pops up should include a list of all the assets that are actually referenced in the Scenes (as long as `Include Dependencies` at the bottom is checked). Compare that with the files you imported and delete whatever you don't need.

### Audio files
Also missing are SFX audio files:
* campfire-1.mp3
* ES_Fire Flame Crackle - SFX Producer.mp3
* ES_Fire Leaves Pile - SFX Producer.mp3
* ES_Night Insects 1 - SFX Producer.mp3
* fire-1.mp3
* fire-2.mp3

I'm not 100% sure of their provenance so I'm not including them. Replace them with audio files of your choosing.

And the following audio tracks by Roald Strauss (available with freeware licenses at indiegamemusic.com):
* A belated farewell.xm
* Coders Dream.ogg
* Slave16bit.xm
