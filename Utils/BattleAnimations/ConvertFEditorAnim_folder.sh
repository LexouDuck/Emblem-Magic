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

SPLITFRAMES=""

function ConvertFEditorAnim_frames ()
{
	if [ $1 == "./*.png" ] || [ $1 == "./*.bmp" ]
	then
		return
	fi
	for f in $*
	do
		printf $COLOR_RESET"CONVERT image: "$f"\t- "$COLOR_RED
		WIDTH=`magick identify -format %w "$f"`
		HEIGHT=`magick identify -format %h "$f"`
		printf $COLOR_RESET"width is "$WIDTH"px -> "
		if [ $WIDTH != 240 ] && [ $WIDTH != 248 ] && [ $WIDTH != 480 ] && [ $WIDTH != 488 ]
		then
			printf $COLOR_RED"ERROR"$COLOR_RESET": image file is invalid -> width should be 240, 248, 480, or 488 pixels.\n"
		elif [ $HEIGHT != 160 ]
		then
			printf $COLOR_RED"ERROR"$COLOR_RESET": image file is invalid -> height should be 160 pixels.\n"
		else
			FILENAME=`echo "$f" | rev | cut -d '.' -f2- | rev`
			if [ $WIDTH -eq 480 ] || [ $WIDTH -eq 488 ]
			then
				OUTPUT1=$OUTDIR"/"$FILENAME"_f.png" ; magick "$f" -set filename:f "$f" -crop "240x160+0+0"   -format png $OUTPUT1
				OUTPUT2=$OUTDIR"/"$FILENAME"_b.png" ; magick "$f" -set filename:f "$f" -crop "240x160+240+0" -format png $OUTPUT2
				printf $COLOR_GREEN"SUCCESS"$COLOR_RESET": created "$OUTPUT1" & "$OUTPUT2
				SPLITFRAMES=$SPLITFRAMES" \"$f\""
			else
				OUTPUT=$OUTDIR"/"$FILENAME".png"
				magick "$f" -set filename:f "$f" -crop "240x160+0+0" -format png $OUTPUT
				printf $COLOR_GREEN"SUCCESS"$COLOR_RESET": created "$OUTPUT
			fi
			COLORS=`magick identify -verbose "$f" | grep "Colors:" | sed -r "s_  Colors: __g"`
			COLORS=`printf -v int "%d" $COLORS 2>/dev/null ; printf "%d" $int`
			if [ $COLORS -gt 16 ]
			then
				printf " -> "$COLOR_YELLOW"WARNING"$COLOR_RESET": image file has more than 16 colors total!"
			fi
			printf "\n"
		fi
	done
}

function ConvertFEditorAnim_code ()
{
	for f in $*
	do
		printf $COLOR_RESET"CONVERT anim code: $f -> "
		OUTPUT=`printf "$OUTDIR/$f" | tr ' ' '_'`
		cp "$f" $OUTPUT
		unix2dos --quiet $OUTPUT
		sed -i -e "s|~~~|end\r\n|g"		$OUTPUT
		sed -i -e "s|\s\+p\- |\tf\[|g"	$OUTPUT
		sed -i -e "s|//|# |g"			$OUTPUT
		sed -i -e "s|.png|.png\]|g"		$OUTPUT
		sed -i -e "s|.bmp|.png\]|g"		$OUTPUT
		for i in $SPLITFRAMES
		do
			FILENAME=`echo "$i" | cut -d '.' -f2- | rev | cut -d '.' -f2- | rev | sed -e "s|/|\\\\\\\\\\\\\\\\|g"`
			SED_STR="s|"$FILENAME".png\]|"$FILENAME"_f.png\]\tb\["$FILENAME"_b.png\]|g"
			sed -i -e $SED_STR	$OUTPUT
		done
		printf $COLOR_GREEN"SUCCESS"$COLOR_RESET": ported code output file is "$OUTPUT"\n"
	done
}

function ConvertFEditorAnim_palette ()
{
	printf $COLOR_RESET"CONVERT palette: from "$1" -> "$COLOR_RED
	magick convert $1 -colors 16 -unique-colors -scale 100% $OUTDIR"/palette.png" && \
	printf $COLOR_GREEN"SUCCESS"$COLOR_RESET": output file is "$OUTDIR"/palette.png\n"
}

ConvertFEditorAnim_frames	"`find ./ -type f -name "*.png" -o -name "*.bmp"`"
ConvertFEditorAnim_code		"`find ./ -type f -name "*.txt"`"
ConvertFEditorAnim_palette	"`find $OUTDIR/ -type f -name "*.png" | head -n 1`"
