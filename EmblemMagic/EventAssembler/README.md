
Event-Assembler
===============
Nintenlord's event code assembler tool for the Fire Emblem games on GBA (FE6, FE7, and FE8).



--------
Credits:
--------

##### Original README

- Nintenlord for making this.
- Kate/Klo/whatever for writing FE6 `CHAR` support
- Everyone who submitted event codes for this, especially Fire Blazer and flyingace24.
- markyjoe1990 for FE7 event template.
- Mariobro3828 for FE7 world map definition values and for making the EAstdlib Macro and Command List.txt.
- Arch for making code I can use to debug this app, for his tutorials and his FE6 template.
- Ryrumeli for telling me the ASM routine that handles the events in FE8.
- Omni for reporting errors with FE6 disassembly script.
- Camtech075/Cam/Kam for making FE8 template.

##### Additional Credits

- CrazyColorz5, for updating and maintaing EA after Nintenlord stopped working on it.
- Vennobennu/Venno, for identifying many FE8 codes, and  making many of the codes that deal with the world map.
- Gryz, for aiding a ton in all the AI research done on FE7, which was a boon in developing the new AI model.
- Circleseverywhere, for inspiring some of the design decisions in revamping EA. Also checking my work, and making several macros.
- Camdar/CT075 and Zahlman, for inspiring a `UNIX`-like design for #incext.
- LexouDuck, for adding documentation and aliases to many 'Language Raws' commands.

##### Special Thanks

- Everyone who has reported bugs and/or given feedback.
- Everyone who uses this program.



------------
Legal Notes:
------------

This program and everything it comes with, referred to as "product" from now on, is delivered "as is" and has no warranty whatsoever.

You may modify, add and distribute the product in its entirety as you wish, with the condition that the origin of the product remains clear.
In other words, any distribution of this product must clearly and explicitly state that it was originally created by Nintenlord.

Any money made with this product belongs to the original creator, Nintenlord.



-----------
How to use:
-----------

### GUI: "Event Assembler.exe"

For "input", select a text file containing event assembly code (typically, a `.txt` or `.event` file).
For "output", select a ROM file to write to (typically, a `.gba` file).
Click on the "Assemble" button: if all went well, you should see `Finished. No errors or warnings.` in the output window.
And that's it! You can then test/play your modified ROM.


If you wish to learn about event disassembling, you may read the tutorial, which is available in here:
http://www.bwdyeti.com/cafe/forum/index.php?topic=24.0
http://serenesforest.net/forums/index.php?showtopic=26206

If you wish to learn about making full custom chapters for FE, read this tutorial by Arch:
http://www.bwdyeti.com/cafe/forum/index.php?topic=55.0
http://serenesforest.net/forums/index.php?showtopic=21165



### Commandline: "Core.exe"

If you wish to use EA from your terminal/commandline, the format is the following:

##### Assembling code to ROM:

```sh
> Core A [language] -input:./path/to/rom.gba [FLAGS...]
```

##### Disassembling code from ROM:

```sh
> Core D [language] [mode] [address] [priority] [length] -output:./dissassembly.event [FLAGS...]
```

##### ARGS:

```
language =	"FE6", "FE7" or "FE8"
			The EA code language to use (which set of commands/macros)

mode	 =	"Block", "ToEnd" or "Structure"
			The kind of dissassembly mode to use

address	 =	Any valid number (between 0 and the given ROM filesize)
			The pointer at which to begin dissassembling EA code

priority =	(not used in Structure disassembly mode)
			The priority of the commands used in disassembly

length	 =	Any valid number, must be under (ROMsize - address)
			(only used in "Block" disassembly mode)
```

##### FLAGS:

```
-addEndGuards
```
Adds end guards to disassembly code output.

```
-raws:folderpath_or_filepath
```
The folder or file to load EA language raws from.
The default value for this parameter is "Language Raws".

```
-rawsExt:extension
```
The file extension to look for, when loading files that contain language raws.
This parameter flag is ignored when loading from a single raws file.
The default value for this parameter is ".txt".

```
-input:File
```
The file to give in as input (text file for assembly, `.gba` ROM for dissassembly).
This parameter is required when assembling code (`> Core A ...`).
For disassembling, the default is to read from standard input (stdin).

```
-output:File
```
The file to use as output (`.gba` ROM for assembly, text file for dissassembly).
This parameter is required when disassembling code (`> Core D ...`).
For assembling, the default is to write to standard output (stdout).

```
-error:File
```
File to write errors, warnings and messages shall be logged.
If not supplied, will output to standard error (stderr).



------------------
History/Changelog:
------------------

V 1.0:
- First public release.
- Entered the Programming contest on FEU.

V 1.1:
- Added codes: `ENUT`, `ENUF` and all condition codes.

V 2.0:
- Added stuff to make code maintaining easier. 
 Too many to list right now.

V 3.0
- Added FE8 support for some codes.
- Added support for World map codes.

V 4.0
- Made definitions and labels have a scope.
- Added support for definition files.

V 4.1
- Fixed a bug in `SHLI` code

V 4.2
- Fixed a bug in `REPA` code

V 5.0
- Added support for some FE6 codes

V 5.1
- Added support for some FE8 codes.

V 5.2
- Fixed glitch with local variables.
- Added `MNC2` for FE7 and FE8 and fixed `MNCH` for FE8.

V 6.0
- Rewrote most of the source, added bunch of codes and abilities. 
- Removed scope for definitions.

V 6.1
- Fixed glitch with FE8 version of `UNIT`.
- Fixed `MISC` related glitches.

V 6.2
- Added FE6 `CHAR` support thanks to Kate/Klo/whatever.

V 6.3
- Various codes added.
- Define file support re-added.

V 6.4
- Pointer error with offsets higher than 01000000 fixed.

V 6.5
- Fixed error handling space in quotes.

V 6.6
- Added `MSGE` code that sends messages to message box.
- Added `#ifdef`, `#ifndef`, `#else`, `#endif` and `#undef`.

V 6.7
- Fixed `LOCA` `DOOR` for FE7 and FE6.

V 6.8
- Improved Exception handling to not crash on most assembly code mistakes.

V 6.9
- Fixed error with FE8 `ENDA` code.
- Added markyjoe1990s FE7 template.

V 7.0
- Added macro support.
- Included the first version of EA standard library.
- Moved most codes to language raws.
- Several code changes, removals and additions.
- Remade error and warning systems completely.
- Added support for arithmetic calculations for defined words.
- Added more arithmetic operations such as &, | and %.

V 7.1
- Added disassembly.
- Added some FE8 world map codes.

V 7.2
- Added option for chapter-wide disassembly.
- Some minor code changes.
- Updated EA standard library.

V 7.3
- Readded `SHLI` and `MOMA` codes.
- Readded proper coordinate support for FE8 unit data.
- Made language raws support bit-accurate codes.
- Added `REDA` for FE8
- Added `IFET` and changed `IFEV` to `IFEF`.
- Added `ASME` code
- Updated EA standard library.

V 7.4
- Fixed a bug with `MOMA` and `SHLI`.
- Fixed a bug in `LWMC` and `TEX8` for FE8.
- Added `CMDS`, `CMDL`, `FIG1` and `FIG2` code for FE8.
- Added `FLDT` and `RMSP` codes.
- Added binary support for assembly.
- Added support for preferred base for disassembly.
- Made ending guardians in disassembly an option.
- Made ending guardians appear at the end of every string of code.

V 7.5
- Fixed a bug in FE8 unit data.
- Added preferred bases to most raw codes.
- Updated EA standard library.

V 7.6
- Fixed a bug that caused binary numbers to not be recognized.
- Changed bases in raw codes to correct.

V 7.7
- Fixed a bug related to uneven bit writing to the end of code.
- Added language raws specification. See file Language.raws in folder Language raws for details about the raws.
- Updated EA standard library.

V 7.8
- Added support for FE6 `AFEV` code.
- Updated template.
- Added `MESSAGEIF` code.
- Fixed FE7 `CHAI` coordinate version code.
- More error messages.
- Some other fixes.
- Updated EA standard library.

V 7.9
- Fixed errors in Event assembler language file.
- Added `WARP` code for FE7.
- Added `TEXTSTART`, `TEXTSHOW`, `TEXTCONT` and `TEXTEND` codes for FE8. 
- Added an extra parameter to `REDA` code.
- Fixed coordinate related problems in `REDA` and FE8 `UNIT`.
- Added `ALIGN` code.
- Updated template.
- Updated EA standard library.

V 8.0
- Fixed crash on loading language raw files with different line ending modes.
- Fixed a code in language raws randomly getting ignored. 
- Added support for negative numbers.
- Some UI changes.
- Fixed `MOVE` code for FE6.
- Fixed FE8 `FADI`/`FADU` confusion.
- Fixed `TEX3` code.
- Added new parameter for `MUS3`.
- Added new parameter for `RMSP`.
- Added parameter to `MACC`.
- Split `SHOWMAP` and `HIDEMAP` codes from `FADU`, `FADI`, `FAWI` and `FAWU`.
- Renamed `TEX3` as `TEXTIFEM`, `TEX4` as `MORETEXT`, `TEX7` as `TEXTWM`.
- Removed `UNIT` Empty code, though EAstdlib has backwards  compability code.
- Removed data from `GOTO` and `MNCH`.
- Added lot's of FE6 and FE7 codes. Every code used in those games is covered somehow.
- Updated EA standard library.

V 8.1
- Rewrote macro handling to be faster and more predictable.
- Added and changed some codes, including FE7 & 8 World map codes.
- Added some built-in macros.
- Added pool ability.
- Made Event Assembler Language.txt generate automatically from raws and updated it considerably.
- Updated EA standard library.

V 8.2
- Separated core functions to separate assembly, called Core. Core can only be run from command line.
- Included scripts to disassemble all chapters and world maps for all  3 GBA games.
- Added few FE6 and FE7 codes that I overlooked in previous release.
- Added description of doc codes to Language.raws file.
- Fixed last bits getting written wrong in FE8 `UNIT` code.
- Fixed a bug in `ITGM` for FE8.
- Fixed `MUSI` and `MUNO` FE8 confusion.
- Fixed FE7 `TEXTBOXTOBOTTOM` and `TEXTBOXTOTOP` mixup.
- Removed `CAM1` for FE8 for not working.
- Added some experimental FE8 codes.
- Added EAstdlib Macro and Command List.txt.
- Updated EA standard library.

V 8.3
- Added Assembly scripts.
- Added script for generating Event assembler language.txt.
- Made EA accept non-caps codes.
- Added `FIRE` and `GAST` for FE7.
- Fixed some FE7 codes.
- Fixed a problem with pool dumping.
- Fixed a problem with codes not incrementing offset properly.
- Updated EA standard library.

V 8.4
- Fixed FE7 `ENUT` and `ENUF` screw-up.
- Updated EA standard library.

V 8.5
- Made FE7 `IFET` and `IFEF` codes match `ENUT` and `ENUF`.
- Added THE_END and LYN_END codes for FE7.
- Added `NCONVOS` and `NEVENTS` for the control freaks out there.
- Updated EA standard library.

V 9.0
- The great FE8 update.
- Improved disassembly performance, especially for FE8.
- Disabled `REDA` disassembling for causing problems.
- Removed bunch of faulty codes for FE8.
- Added large bunch of codes FE8.
- Added `CGSTAL` code for FE7.
- Renamed `TEX5` to `TEXTCG` for FE7.
- Added `MORETEXTCG` for FE7.
- Added `FROMCGTOBG`, `FROMBGTOCG`, `FROMCGTOMAP` for FE7.
- Added new built-in macro, String.
- Fixed error reporting error when file isn't found  with #include and #incbin
- Updated EA standard library.

V 9.1
- Rewrote parsing.
- Added `<<` and `>>` bitshift operators.
- Removed `CODE` and `FILL` codes.
- Added `WORD`, `SHORT`, and `BYTE`.
- Improved error reporting.
- Updated EA standard library.

V 9.2
- Fixed ; handling repeating the first code.
- Added proper errors to some symbol related parameters  being undefined.
- Added proper errors to built-in codes getting a parameter of wrong type.
- Fixed Terminating string templates misreporting length  during disassembly.
- Updated FE7 Template.

V 9.3
- Fixed EA not giving the error on mis-aligned codes.
- Fixed crashing upon certain parsing errors.
- Fixed crash with 0x integer literals.
- Added FE6 and FE8 templates and updated FE7 one.

V 9.4
- Fixed line comments not getting ignored in block comments.
- Fixed error with custom error file when calling Core from command line.
- Fixed disassembly not handling mergeable codes lengths properly.
- Added offset of bad pointer to error message when handling the pointerlist of a chapter.
- Some performance improvements to both assembly and disassembly.
- Fixed crash when using read-only file as output file in assembly from command line.
- Updated EA standard library.

V 9.5
- Removed output showing window from taskbar.
- Made column numbering start from 1 instead of 0.
- Improved error when using vector of vectors as parameter.
- More minor performance improvements.

V 9.6
- Renamed `GOTO` to `CALL` and `_GOTO_HELL` to `_CALL_HELL`.
- Fixed a bug causing `CURRENTOFFSET` to not print properly.
- Fixed length disassembly adding end guards in wrong places with wrong offsets.
- Added Programmers Notepad syntax highlighting scheme. Only works with version 2.0.8.718 of PN.
- Added shaky backwards compatibility for `CODE`. Define `USING_CODE` to use it.
- Updated EA standard library.

V 9.7
- Fixed errors in FE6 disassembly script, thanks to Omni.
- Fixed a raw file not being included with releases, causing `NCONVOS` and `NEVENTS` codes not to work.
- Added backwards compatibility for experimental codes moved to full codes.
- Made GUI not freeze when completing a task.
- Renamed some language raws files.
- Fixed crash on having wrong amount of parameters with ORG or `ALIGN`.
- Updated EA standard library.

V 9.8
- Made EA abort writing any data to output if any errors are encountered.
- Updated EA standard library.

V 9.9
- Fixed `SHORT` and `BYTE` giving alignment errors on 2 and 1 aligned codes.
- Updated Templates to match changes.
- Added `END_MAIN` code.
- Updated EA standard library to 2.13.

V 9.10
- Fixed disassembly always crashing.
- Stopped `MESSAGE`, `ERROR` and `WARNING` codes from printing the name of the code.
- Updated EA standard library to 2.14.

V 9.11
- Removed `ROMH`ackingCore.dll due to not being used.
- Updated EA standard library to 2.14. `TODO`

V 9.11
- Crazycolorz5's Edits.
- Great AI update.
- Finally fixed the great FE7 `IFAT`/`IFAF` mixup.
- Integrated the many FE8 raws that Venno identified.
- Added many aliases for existing codes.
- Added a ton of macros. See EA standard library for full list.
- Changed name of "EA Standard library" to "EA Standard Library".
- Updated EA Standard Library to 3.0.

V 9.12
- Aliased `BYTE` as `CODE` for both backwards compatability and usage of #incbin
- Bundled Hack Installation and AI Assembly into Extensions folder.
- FE7, FE8: Added expanded templates for use in making hack buildfiles.
- Updated EA Standard Library to 3.1.

V 10.0
- FE6: Added THE_END code in raws, replacing `_0x3E` (credit to Onmi)
- FE8: Changed old FE8 `IFET` to `CHECK_EVENTID`
- Updated EA Standard Library to 9.2.
- Made capitalization in includes and folder structure more consistent.
- Fixed bug where resources inconsistently closed when included.
- Fixed #incbin to use `BYTE` instead of `CODE`.
- Fixed bug where `#ifndef` didn't properly recognize automatic definitions.
- Made `>>` always do 32-bit logical shifts; Added `>>>` to do 32-bit arithmetic shifts.
- Fixed operator precedence to match that of C's.
- Loading a raw code will also load `_0xXXXX` where `XXXX` is its ID; If the code is <= 0xFF, it also loads `_0xXX`.
- Redefinitions are now only warnings, rather than errors.
- Added #incext; Searches Tools folder for the name of the tool provided, then calls the tool by command line with the rest of the parameters as commandline parameters, then inlines with `BYTE` any output written to stdout. An error is logged if the first bytes of the output are "ERROR: ".
- Added the Tools ParseFolder, Png2Dmp, and PortraitFormatter.
- Added `PUSH`/POP to store and load `CURRENTOFFSET`.
- Added #easteregg. Try to find them all!

V 10.1
- Fixed bugs with #incext
- Added `#runext` -- like incext but doesn't pass the `--to-stdout` option and discards output.
- Added `#ifndef` guards around Tools/Tool Helpers.txt
- Allowed escaping in parsing -- `\` lets the next character to be parsed plain. So, able to pass things like `My\ File.txt` or have `"` inside a quote, etc. This still needs some work when it's being lexed/displayed.
- Updated toolkit.
- Added `MNTS` code for FE8 (returns to title screen)
- Fixed Fe8Code assembling, which was produced by the disassembler.

V10.1.1
- Added `POIN2`; it's just `POIN` but doesn't require word-alignment.
- Fixed Hack Installation/setPointerTableEntry
- Added #inctext for programs that output event code, rather than just binary.

V 11.0
- Changed #inctext to #inctevent. I recommnend the use of this name in the future because it's more intuitive.
- Fixed bug where Core would hang if it got an invalid input file.
- Added `PROTECT` (start) (end) to make a region write-protected.
- Made Event assembler.exe look for .event files by default (over .txt)
- Changed lexing/parsing. 
- String literals now work (with `MESSAGE` and such)
- Labels/Definitions come into scope for parameters properly.
- Added ability for use of definitions in preprocessor calls.
- Made disassembly work again; changed auto-adding of `_0xXX` codes to not mess up disassembly.
- Changed disassemblies to use `ASSERT` for stronger protection over just `MESSAGE`-ing the currentOffset.
- Changed scripts to take/output .event files by default.

V11.0.1
- Aliased `#inctevent` as `#inctext` for backwards compatability with V10.1.1



-------------
Future plans:
-------------

- http://feuniverse.us/t/ea-features-to-add/1608
