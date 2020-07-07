#!/bin/bash
OUTDIR=./output/
mkdir $(OUTDIR)
for f in ./*.{png,bmp}
do
	WIDTH=`magick identify -format %%w $f`
	FILENAME=`basename -s .png -s .bmp $f`
	printf "$(WIDTH) - $f"
	if [ $(WIDTH) = 480 || $(WIDTH) = 488 ]
	then
		magick $f -set filename:f ./$(OUTDIR)/$f -crop "240x160+0+0"   -format png "$(FILENAME)_f.png"
		magick $f -set filename:f ./$(OUTDIR)/$f -crop "240x160+240+0" -format png "$(FILENAME)_b.png"
	else
		magick $f -set filename:f ./$(OUTDIR)/$f -crop "240x160+0+0" -format png "$(FILENAME).png"
	fi
done
