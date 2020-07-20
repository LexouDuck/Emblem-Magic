#!/bin/bash
DIST="./dist"
echo "Deleting $DIST folder..."
rm -rf $DIST/
mkdir $DIST

echo "Updating files/folders in $DIST/..."
cp -r ./Arrays			$DIST/
cp -r ./Structs			$DIST/
cp -r ./Modules			$DIST/
cp -r ./Patches			$DIST/
cp -r ./Utils			$DIST/
cp -r ./ROMs			$DIST/
rm -rf $DIST/Modules/old
rm -rf $DIST/Modules/tmp
rm -rf $DIST/ROMs/*.gba
rm -rf $DIST/ROMs/*.GBA

mkdir $DIST/EventAssembler/
FOLDER="./EventAssembler/Event Assembler/Event Assembler"
find "./EventAssembler/" -maxdepth 1 -name "README*" | while read f ; do cp "$f" $DIST/EventAssembler/ ; done
find "$FOLDER"			 -maxdepth 1 -name "*.event" | while read f ; do cp "$f" $DIST/EventAssembler/ ; done
find "$FOLDER"			 -maxdepth 1 -name "*.txt"	 | while read f ; do cp "$f" $DIST/EventAssembler/ ; done
cp -r	"$FOLDER/Language Raws"						$DIST/EventAssembler/
cp -r	"$FOLDER/EA Standard Library"				$DIST/EventAssembler/
cp -r	"$FOLDER/Extensions"						$DIST/EventAssembler/
cp -r	"$FOLDER/Scripts"							$DIST/EventAssembler/
cp -r	"$FOLDER/Syntax"							$DIST/EventAssembler/
cp 		"$FOLDER/bin/Release/Core.exe"				$DIST/EventAssembler/
cp 		"$FOLDER/bin/Release/Event Assembler.exe"	$DIST/EventAssembler/
cp 		"$FOLDER/bin/Release/Nintenlord.dll"		$DIST/EventAssembler/
cp 		"$FOLDER/bin/Release/Nintenlord.Forms.dll"	$DIST/EventAssembler/


echo "Updating executables and libraries in $DIST/..."
cp "./README_dist.txt"							$DIST/README.txt
cp "./Help/Emblem Magic.chm"					$DIST/
cp "./bin/Release/Emblem Magic.exe"				$DIST/
cp "./bin/Release/Emblem Magic.exe.config"		$DIST/
cp "./bin/Release/Utils/FastColoredTextBox.dll"	$DIST/Utils/

echo "Packaging .zip file..."
rm $DIST/EmblemMagic.zip
zip -r $DIST/EmblemMagic.zip $DIST/

echo "Done! The distributable .zip file is $DIST/EmblemMagic.zip"
