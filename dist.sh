#!/bin/bash
echo "Deleting folders in ./dist/..."
rm -rf ./dist/Arrays
rm -rf ./dist/Structs
rm -rf ./dist/Modules
rm -rf ./dist/Patches
rm -rf ./dist/ROMs
rm -rf ./dist/Utils
rm -rf ./dist/EventAssembler

echo "Updating folders in ./dist/..."
cp -r ./Arrays			./dist/
cp -r ./Structs			./dist/
cp -r ./Modules			./dist/
cp -r ./Patches			./dist/
cp -r ./ROMs			./dist/
cp -r ./Utils			./dist/
cp -r ./EventAssembler	./dist/
rm -rf ./dist/Modules/old
rm -rf ./dist/Modules/tmp
rm -rf ./dist/ROMs/*.gba
rm -rf ./dist/ROMs/*.GBA

echo "Updating executables and libraries in ./dist/..."
cp "./Help/Emblem Magic.chm"				./dist/
cp "./bin/Release/Emblem Magic.exe"			./dist/
cp "./bin/Release/Emblem Magic.exe.config"	./dist/
cp "./bin/Release/FastColoredTextBox.dll"	./dist/Utils/

echo "Packaging .zip file..."
rm ./dist/EmblemMagic.zip
zip -r ./dist/EmblemMagic.zip ./dist/

echo "Done! The distributable .zip file is ./dist/EmblemMagic.zip"
