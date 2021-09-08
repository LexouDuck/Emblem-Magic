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

echo "Updating executables and libraries in $DIST/..."
cp "./README_dist.txt"							$DIST/README.txt
cp "./Help/KirbyMagic.chm"						$DIST/
cp "./bin/Release/KirbyMagic.exe"				$DIST/
cp "./bin/Release/KirbyMagic.exe.config"		$DIST/
cp "./bin/Release/Utils/FastColoredTextBox.dll"	$DIST/Utils/

echo "Packaging .zip file..."
rm $DIST/KirbyMagic.zip
zip -r $DIST/KirbyMagic.zip $DIST/

echo "Done! The distributable .zip file is $DIST/KirbyMagic.zip"
