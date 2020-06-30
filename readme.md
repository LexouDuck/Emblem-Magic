Emblem Magic
------

Emblem Magic is an all-in-one ROMhacking/editing tool for the GBA Fire Emblem games.
I wanted it to be more complete than it is now before releasing it, but i'm going to be extremely busy, and I had set a deadline for myself beforehand.
As such, there isn't much in the way of guides as to how to use the program, so I hope that it won't be too confusing for users.
In any case, you can think of this of more of a public beta than an actual release: so bug reports are rather welcome, and so are feature suggestions.



So, Emblem Magic includes a set of different editors, as well as some core functionalities to better allow the user to manage the changes made to the ROM.
These core tools include:
- a history of writes done to the ROM (thanks to which the user can use Ctrl+Z/Ctrl+Y to undo/redo changes)
- an editor to manage which areas of the ROM are marked (by default the markings "FREE" and "USED" exist, but the user can create their own marking types)
- tools for easier repointing (the user can choose whether the program should prompt them to repoint upon insertion or not, for instance)
All this information can be stored by the user in an FEH file (FEH stands for Fire Emblem Hack); also note that with this file, the ROMhack can be recreated from an approriate clean ROM.
Also, Emblem Magic reads external text files to associate names to array entries in the ROM (kinda like the dropdowns from Nightmare, and the user can have their own set of text files, seperate from 

the ones for the vanilla ROMs).

The Editors range from some very fundamental ROMhacking tools to more specific ones intended for the GBA Fire Emblem games:
_Note_: The editors in the following which have an "X" instead of a "-" are the more incomplete and WIP ones (I say this, but in fact almost all of them are incomplete in some way).

- **Basic ROM Editor** (simple interface to read & write bytes, with LZ77 compression)
![](https://i.imgur.com/zXa7QhP.png)

- **Hex Editor** (like any hex editor, really)
![](https://i.imgur.com/c12seK8.png)

- **Patch Editor** (for applying various patches to the ROM through FEH files located in the "Patches" folder)
![](https://i.imgur.com/Iu0RGxa.png)

- **ASM Editor** (dissassemble/assemble ASM code, and can somewhat "test run" code line-per-line)
![](https://i.imgur.com/gvm4P8k.png)

X **Event Editor** (calls EA from a bundled dll file, the code editor has syntax coloring, mousehover documentation and some more cool functions)
![](https://i.imgur.com/78eDICB.png)

- **Module Editor** (basically like Nightmare, uses .emm text files to load GUI to edit arrays or other data in the ROM - create your own struct editors this way)
![](https://i.imgur.com/gvm4P8k.png)

- **Text Editor** (to change the text in the game, as well as the fonts used to display text)
![](https://i.imgur.com/L48DeB2.png)

- **Item Editor** (to edit anything pertaining to items, including their stat bonuses or the classes they're effective against)
![](https://i.imgur.com/ykHkL72.png)

X **Map Tileset Editor** (to change the tilesets of 16x16 tiles used for maps, their animations, their terrain associations)
![](https://i.imgur.com/JHtS290.png)

- **Map Editor** (to edit the maps for each chapter of the game, as well as the triggerable tile changes for these maps)
![](https://i.imgur.com/BIYSiKS.png)

- **Graphics Editor** (basically GBAGE, to view and edit images in the ROM)
![](https://i.imgur.com/dEuiZ0U.png)

- **Portrait Editor** (to view and edit the different portraits in the game)
![](https://i.imgur.com/WbAj6zP.png)

- **Map Sprite Editor** (to view and change the sprites of units on the map)
![](https://i.imgur.com/xdISHav.png)

- **Battle Screen Editor** (to change the screen frame during battle, or the platforms under the characters' feet)
![](https://i.imgur.com/bMfjteM.png)

- **Battle Animation** Editor (to view and change the various battle animations in the game, and the palettes for them)
![](https://i.imgur.com/EkwftEq.png)

X **Spell Animation** Editor
![No Screenshot](https://i.png)

- **Backgrounds Editor** (to change dialogue scene backgrounds, battle backgrounds, and cutscene CG screens)
![](https://i.imgur.com/4LvUHEp.png)

X Title Screen Editor (very unfinished, really just has shortcuts to the Graphics Editor for the 3 sub-images that make up the title screen)
![](https://i.imgur.com/wfljIvf.png)

X **Music Editor** (very unfinished, to change the music and sound-effect tracks used in the game)
![](https://i.imgur.com/sTQ7JmE.png)

- **World Map Editor** (pretty simple editor for image-inserting, for the large/small world maps for the game)
![](https://i.imgur.com/G7E5h5Y.png)

Some other more general editors are accessible from within the other editors, under the 'Tools' menu:

- **Palette Editor** (you can also open this by clicking on any palette in any editor)
![](https://i.imgur.com/kxvL0CC.png)

- **TSA Editor** (to change TSA arrays, these indicate tile layout and associate 16-color palettes to each tile, for more complex images)
![](https://i.imgur.com/vV78kzr.png)

- **OAM Editor** (accessed from the Battle Animation Editor, to change the sprite layout information for a given frame of animation)
![](https://i.imgur.com/Ptb0IdR.png)

Download the program here: (also i suggest reading the readme.txt before doing anything)
http://www.mediafire.com/file/8lv5xfzst1055cd/Emblem+Magic+BETA.zip

UPDATE: Second version of the beta is here, adding quite a few new features


- And plenty of other stuff (many bugfixes of course), including pretty much all the things said in this thread (like having more shortcuts) and some other things, like the ability to modify batte anim structs within the battle anim editor
http://www.mediafire.com/file/al2kt7s7ckn2932/Emblem+Magic+BETA+2.zip
