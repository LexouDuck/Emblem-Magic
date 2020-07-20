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
cp -r ./Patches			./dist/
cp -r ./Utils			./dist/
cp -r ./Modules			./dist/
rm -rf ./dist/Modules/old
rm -rf ./dist/Modules/tmp
cp -r ./ROMs			./dist/
rm -rf ./dist/ROMs/*.gba
rm -rf ./dist/ROMs/*.GBA

EA_FOLDER=./EventAssembler/Event\ Assembler/Event\ Assembler/
mkdir ./dist/EventAssembler/
cp		"./README*.txt"								./dist/EventAssembler/
cp -r	$FOLDER"Language Raws"						./dist/EventAssembler/
cp -r	$FOLDER"EA Standard Library"				./dist/EventAssembler/
cp -r	$FOLDER"Extensions"							./dist/EventAssembler/
cp -r	$FOLDER"Scripts"							./dist/EventAssembler/
cp -r	$FOLDER"Syntax"								./dist/EventAssembler/
cp		$FOLDER"*.event"							./dist/EventAssembler/
cp		$FOLDER"*.txt"								./dist/EventAssembler/
cp		$FOLDER"bin/Release/Core.exe"				./dist/EventAssembler/
cp		$FOLDER"bin/Release/Event Assembler.exe"	./dist/EventAssembler/
cp		$FOLDER"bin/Release/Nintenlord.dll"			./dist/EventAssembler/
cp		$FOLDER"bin/Release/Nintenlord.Forms.dll"	./dist/EventAssembler/


echo "Updating executables and libraries in ./dist/..."
cp "./Help/Emblem Magic.chm"				./dist/
cp "./bin/Release/Emblem Magic.exe"			./dist/
cp "./bin/Release/Emblem Magic.exe.config"	./dist/
cp "./bin/Release/FastColoredTextBox.dll"	./dist/Utils/

echo "Packaging .zip file..."
rm ./dist/EmblemMagic.zip
zip -r ./dist/EmblemMagic.zip ./dist/

echo "Done! The distributable .zip file is ./dist/EmblemMagic.zip"
