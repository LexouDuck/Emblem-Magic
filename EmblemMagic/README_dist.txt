There are a couple first steps you need to take to ensure that Emblem Magic can function properly.

1) Install .NET framework
Emblem Magic is made with C# WinForms, and uses the .NET framework 4.5.1
If you don't have that, the software is not likely to work correctly.

2) ROM files
Get a clean ROM of the game you wish to edit, and place it in the 'ROMs' folder.
Emblem Magic supports all GBA Fire Emblem games in their different versions.
So that they can be recognised by the program, you must name your clean ROM correctly, like so:
FE6J.gba
FE7J.gba
FE7U.gba
FE7E.gba
FE8J.gba
FE8U.gba
FE8E.gba

3) Folder paths
Open up Emblem Magic, and open the Options window (under Edit -> Options).
Make sure the folder paths are correct (for the 'Arrays', 'Structs', 'Modules', 'Patches' and 'ROMs' folders)

- The 'Arrays' folder holds lists of entry names for arrays in the vanilla games (some are pointer lists)
You can choose to check the 'Use Custom Array Path' option to read array entry names from your own folder.

- The 'Structs' folder holds text files detailing how bytes are laid out for certain important arrays.

- The 'Modules' folder holds Emblem Magic Module (.mmf) text files, for use with the Module Editor.
  The Module Editor is equivalent to Nightmare (with some changes, mostly to allow categories).

- The 'Patches' folder holds .mhf files, these are the files used by Emblem Magic to save hack information.

- The .mhf files in this folder are used by the Patch Editor, for quick and easy functionality hacking.

- Lastly, the 'ROMs' folder holds clean ROMs to use as reference to restore data, or find certain pointers.
