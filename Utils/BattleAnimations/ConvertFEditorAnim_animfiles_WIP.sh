#!/bin/bash
IFS=$'\n'

  COLOR_RESET="\x1b[0m"
    COLOR_RED="\x1b[31m"
  COLOR_GREEN="\x1b[32m"
 COLOR_YELLOW="\x1b[33m"
   COLOR_BLUE="\x1b[34m"
COLOR_MAGENTA="\x1b[35m"
   COLOR_CYAN="\x1b[36m"

OUTDIR=./output
rm -rf $OUTDIR
FOLDERS=`find ./ -depth -type d`
mkdir -p $OUTDIR
mkdir -p `echo "$FOLDERS" | sed -e "s|\./|$OUTDIR/|g"`

function ConvertFEditorAnim_tilesets ()
{
	for f in $*
	do
		printf $COLOR_RESET"CONVERT image: "$f"\t- "$COLOR_RED
		if [ `ls $f | echo $?` == 2 ]
		then
			printf $COLOR_RED"ERROR"$COLOR_RESET": file not found ("$f")\n"
			return
		fi
		WIDTH=`magick identify -format %w "$f"`
		HEIGHT=`magick identify -format %h "$f"`
		printf $COLOR_RESET"width is "$WIDTH"px -> "
		OUTPUT="$OUTDIR/$f.png"
		if [ $HEIGHT != 64 ]
		then
			printf $COLOR_RED"ERROR"$COLOR_RESET": image file is invalid -> height should be 160 pixels.\n"
			continue
		elif [ $WIDTH == 256 ]
		then
			magick "$f" -set filename:f "$f" -crop "256x64+0+0" -format png $OUTPUT
			printf $COLOR_GREEN"SUCCESS"$COLOR_RESET": (already valid) created "$OUTPUT"\n"
		elif [ $WIDTH != 264 ]
		then
			printf $COLOR_RED"ERROR"$COLOR_RESET": image file is invalid -> height should be 160 pixels.\n"
			continue
		else
			magick "$f" -set filename:f "$f" -crop "256x64+0+0" -format png $OUTPUT
			printf $COLOR_GREEN"SUCCESS"$COLOR_RESET": created "$OUTPUT"\n"
		fi
		COLORS=`magick identify -verbose "$f" | grep "Colors:" | sed -r "s_  Colors: __g"`
		COLORS=`printf -v int "%d" $COLORS 2>/dev/null ; printf "%d" $int`
		if [ $COLORS -gt 16 ]
		then
			printf " -> "$COLOR_YELLOW"WARNING"$COLOR_RESET": image file has more than 16 colors total!"
		fi
	done
}

function ConvertFEditorAnim_animdata ()
{
	for f in $*
	do
		printf $COLOR_RESET"CONVERT animdata: $f -> "
		FILENAME=`echo "$f" | rev | cut -d '.' -f2- | rev`
		OUTPUT=`echo "$OUTDIR/$FILENAME.fea"`
		cp "$f" $OUTPUT
		printf $COLOR_GREEN"SUCCESS"$COLOR_RESET": output file is "$OUTPUT"\n"
	done
}

function ConvertFEditorAnim_palette ()
{
	printf $COLOR_RESET"CONVERT palette: from "$1" -> "$COLOR_RED
	magick convert $1 -colors 16 -unique-colors -scale 100% $OUTDIR"/palette.png" && \
	printf $COLOR_GREEN"SUCCESS"$COLOR_RESET": output file is "$OUTDIR"/palette.png\n"
}

ConvertFEditorAnim_tilesets	"`find ./ -type f -name "* Sheet *.png"`"
ConvertFEditorAnim_animdata	"`find ./ -type f -name "*.dmp"`"
ConvertFEditorAnim_palette	"`find $OUTDIR/ -type f -name "*.png" | head -n 1`"
