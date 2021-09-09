
Features:
- App: icons for the "open editor" buttons
- Spell anims yo
- Adding items to the item array ?
- Control with pretty map sprites for weapon effectiveness, in item editor
- Get european version default free space ranges for FE7 and FE8
- FE8 Large World Map: Road-editing, and a specific PaletteMap Editor
- Text editor: have text bubble previewing follow bytecode text commands
- Portrait Editor: CHR+PAL imagedata portrait saving
- Event Editor: unit loading/movement on map
- Event Editor: toggle to show AREA events on map (also villages, treasures, etc)
- Event Editor: a listbox timeline of all the TURN events for this chapter
- Battle Anim Editor: make an "export all" button
- Battle Anim editor: augment the duration of the "wait for HP" frame on GIF export

Bugfixes:
- When inserting a small world map into FE8, green covers the screen ingame (except when the camera is scrolled)..?
- Text editor, how can i show japanese shift-JIS fonts properly ?
- Map editor, doenst prompt to repoint on TMX or MAR insert
- Cutscene screen insertion problem with pointers ..?
- Battle Anim Editor, The in-editor CodeBox doesn't work all too well
- When opening a second ROM, moushover doc in the event editor doesn't work anymore

Keep in mind for release to check:
- File_RecentFiles.Enabled field being set to 'false' in Suite.Designer.cs, go delete that line
- MarkingComboBox "Datasource modified error" because of generated code in designer files, go delete that too
- The automatically defined "DEBUG" constant (not defined when compiling as 'Release' build in visual studio), will take care of:
    - Main Suite window (this file): Disabling the "open" buttons for WIP/unfinished Editor windows
    - ./src/Editors/EventEditor.cs, line 95 or so, absolute folderpath changes to appropriate function call
- Do a "Release" build (any CPU) in Visual studio
- Run the ./dist.sh shell script (if you're on Windows, use Cygwin), to prepare the "dist" folder for release.
