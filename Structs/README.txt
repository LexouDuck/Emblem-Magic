This folder holds text files (called "struct files") which define how ingame objects are to be interpreted/read by Emblem Magic.
The syntax for a struct file is the following:

0x_____[Address of struct array] __[Amount of entries in this array]

0x__[Field offset within struct] _[Field length, in bytes] FieldName
0x__[Field offset within struct] _[Field length, in bytes] FieldName
0x__[Field offset within struct] _[Field length, in bytes] FieldName
...



That is the general way of looking at it, but it's often clearer with an example (This is the battle animation struct):

0xE00008 32

0x00 8 String Name
0x0C 4 Pointer Sections
0x10 4 Pointer AnimData
0x14 4 Pointer OAM_Right
0x18 4 Pointer OAM_Left
0x1C 4 Pointer Palettes

NB: The chosen field names are used by Emblem Magic, so they're not abitrary, they're specifically chosen.
What this means is that, if you change a field name in one of these files, the associated editor in Emblem Magic won't work.
As such, if you have some ASM hack which modifies an ingame struct, you need to modify the corresponding struct .txt file.
When you do this, you must only change the addresses/offsets, but conserve the existing field names, types and byte-lengths.