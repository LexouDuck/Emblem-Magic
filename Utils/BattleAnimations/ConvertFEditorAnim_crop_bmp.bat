@echo off
setlocal enabledelayedexpansion
md output
for %%I in ( *.bmp ) do (
	for /f %%A in ('magick identify -format %%w %%I') do set "width=%%~nA"
	echo !width! - %%I
	set "flag="
	if !width!==480 set flag=1
	if !width!==488 set flag=1
	if defined flag (
		magick "%%I" -set filename:f "output/%%~nI" -crop "240x160+0+0"   -format png "%%[filename:f]_f.png"
		magick "%%I" -set filename:f "output/%%~nI" -crop "240x160+240+0" -format png "%%[filename:f]_b.png"
	) else (
		magick "%%I" -set filename:f "output/%%~nI" -crop "240x160+0+0" -format png "%%[filename:f].png"
	)
)