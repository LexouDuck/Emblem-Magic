
Features:
- ??? Title Screen Editor ???
- ??? Menu Editor ???
- ASM assembler
- Music Player
- CSV import/export for module editor
- A "reorder palette without changing image" tool for the background editor
- text-based tilemaps so users can change stuff
- Set correct tab index for every control in each editor
- Module Editor: drag-n-drop reordering of UI sections, see https://www.codeproject.com/Articles/48411/Using-the-FlowLayoutPanel-and-Reordering-with-Drag

Bugfixes:
- Make it so palettes ALWAYS have 16-byte writes, even if they're shorter than that
- Clicking 'Cancel' when opening a ROM should actually cancel the file-open
- For some people, the program suddenly stops working (the main window is permanently minimized) and they need to redownload
- There are 2 prompts when copying in HexEditor, and deciding not to proceed still copies anyways..?
- Hex Editor, copy-pasting takes actual bytes ??
- ASM Editor, registers getting reset to 0 on bl instruction ? test on routine at 0x2C7C in FE8U
- Text editor, how can i show japanese shift-JIS fonts properly ?
- When the error for not having the correct clean ROM is up, the program cant be closed
