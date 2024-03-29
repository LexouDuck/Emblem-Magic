using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EmblemMagic.FireEmblem;
using GBA;
using Magic;
using Magic.Editors;
using EA = Nintenlord.Event_Assembler.Core;

namespace EmblemMagic.Editors
{
    public partial class EventEditor : Editor
    {
        /// <summary>
        /// The current entry in the chapter array
        /// </summary>
        StructFile Current;
        /// <summary>
        /// The map of the current chapter.
        /// </summary>
        FireEmblem.Map CurrentMap;

        /// <summary>
        /// The event assembler language for this editor - this is also used for syntax coloring
        /// </summary>
        EA.Code.Language.EACodeLanguage Language;
        EA.Code.LanguageProcessor LanguageProcessor;

        List<Event> EventList = new List<Event>();

        static Int32 UNIT_argX = (Core.App.Game is FE8) ? 4 : 6;
        static Int32 UNIT_argY = (Core.App.Game is FE8) ? 5 : 7;



        public EventEditor()
        {
            try
            {
                this.InitializeComponent();

                this.Entry_ArrayBox.Load("Chapter List.txt");
                this.Event_ArrayBox.Load("Map List.txt");

                this.Current = new StructFile("Chapter Struct.txt");

                this.Help_ToolTip_Timer = new Timer();
                this.Help_ToolTip_Timer.Tick += this.HoverTick;
                this.Help_ToolTip_Timer.Interval = 800;

                this.Chapter_MagicButton.EditorToOpen = "Module:Chapter Editor";
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        override public void Core_OnOpen()
        {
            this.Core_LoadEventSyntax();

            this.Core_Update();
        }
        override public void Core_Update()
        {
            this.Current.EntryIndex = this.Entry_ArrayBox.Value;
            this.Core_LoadValues();
            this.Core_LoadEventCode();
            this.MapChanges_ListBox.Items.Clear();
            this.Core_LoadMap();
        }

        void Core_LoadValues()
        {
            this.Event_ArrayBox.ValueChanged -= this.Event_ArrayBox_ValueChanged;
            this.Event_PointerBox.ValueChanged -= this.Event_PointerBox_ValueChanged;

            this.Event_ArrayBox.Value = this.Current["Events"];
            this.Event_PointerBox.Value = Core.ReadPointer(Core.GetPointer("Map Data Array") + 4 * this.Event_ArrayBox.Value);

            this.Event_ArrayBox.ValueChanged -= this.Event_ArrayBox_ValueChanged;
            this.Event_PointerBox.ValueChanged -= this.Event_PointerBox_ValueChanged;
        }
        
        void Core_LoadEventSyntax()
        {
            try
            {
                if (!EA.Program.CodesLoaded || this.LanguageProcessor == null)
                {
                    String folder =
#if (DEBUG)
"D:\\Lexou\\Projects\\EmblemMagic\\EventAssembler\\Event Assembler\\Event Assembler";
#else
Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(EA.Program)).Location);
#endif
                    this.LanguageProcessor = EA.Program.LoadCodes(folder + "\\Language Raws", ".txt", true, true);
                }
                this.Language = EA.Program.Languages[Core.App.Game.Identifier.Substring(0, 3)];
                String keywords = "";
                IEnumerable<String> codes = this.Language.GetCodeNames();
                foreach (String code in codes)
                {
                    keywords += code;
                    keywords += "( |\r\n)|";
                }
                this.Event_CodeBox.AddSyntax("(//).*", SystemColors.ControlDark);
                this.Event_CodeBox.AddSyntax("#.* ", System.Drawing.Color.LimeGreen, FontStyle.Italic);
                this.Event_CodeBox.AddSyntax(keywords.TrimEnd('|'), SystemColors.Highlight, FontStyle.Bold);
                this.Event_CodeBox.AddSyntax(@"((\b[0-9]+)|((\b0x|\$)[0-9a-fA-F]+))\b", System.Drawing.Color.SlateBlue);
                this.Event_CodeBox.AddCollapse(":\r\n", "\r\n\r\n");
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the EventAssembler language codes.", ex);
            }
        }
        void Core_LoadEventCode()
        {
            try
            {
                StringBuilder stringbuilder = new StringBuilder();
                using (StringWriter writer = new StringWriter(stringbuilder))
                {
                    EA.IO.Logs.TextWriterMessageLog log = new EA.IO.Logs.TextWriterMessageLog(writer);
                    EA.Program.Disassemble(this.Language,
                        Program.Core.ROM.FileData,
                        "", writer, true,
                        EA.DisassemblyMode.Structure,
                        this.Event_PointerBox.Value,
                        EA.Code.Language.Priority.none,
                        0, log);
                    if (log.ErrorCount > 0 || log.MessageCount > 0)
                    {
                        log.PrintAll();
                    }
                }
                String result = stringbuilder.ToString().TrimStart('\r', '\n');
                this.EventList = this.Core_LoadEventCode_EventLabels(ref result);
                if (this.Tools_ManageSpace.Checked)
                    this.Core_LoadEventCode_ManageSpace(ref result);
                if (this.View_ArrayDefinitions.Checked)
                    EventAssemblerIO.LoadEventCode_ArrayDefinitions(ref result);
                if (this.View_HelperMacros.Checked)
                    EventAssemblerIO.LoadEventCode_HelperMacros(ref result);
                this.Core_LoadEventCode_MapUnitGroups();
                this.Event_CodeBox.Text = result;
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the event code.", ex);
            }
        }
        List<Event> Core_LoadEventCode_EventLabels(ref String code)
        {
            List<Event> result = new List<Event>();
            String[] commands = new String[]
            {
                "TURN",
                "AFEV",
                "AREA",
                "SHOP",
                "VILL",
                "LOCA",
                "CHAR",

                "CALL",
                "MOVE",
                "FIGH",

                "LOU1",
                "LOU2",
                "LOUMODE1",
                "LOUMODE2",
                "LOUFILTERED",
                "LOUFILTERED2",

                "UNIT"
            };
            String[] args;
            UInt32[] arguments;

            String label;
            Int32 label_arg;
            Int32 index;
            Int32 new_index;
            Int32 length;
            foreach (String command in commands)
            {
                index = 0;
                while ((new_index = code.IndexOf("\n" + command + " ", index)) > index)
                {
                    index = new_index + command.Length + 2;
                    length = code.IndexOf("\r\n", index) - index;
                    args = code.Substring(index, length).Split(
                        new Char[] { ' ', ',', '[', ']', '\t' },
                        StringSplitOptions.RemoveEmptyEntries);
                    arguments = new UInt32[args.Length];
                    label = null;
                    label_arg = -1;
                    for (Int32 i = 0; i < args.Length; i++)
                    {
                        if (args[i].Length >= 5 && args[i].Substring(0, 5) == "label")
                        {
                            Boolean label_suffix = false;
                            label_arg = i;
                            switch (command)
                            {
                                case "TURN": label = "TurnEvent"; break;
                                case "AFEV": label = "AfterEvent"; break;
                                case "AREA": label = "AreaEvent"; break;
                                case "SHOP": label = "ShopEvent"; break;
                                case "VILL": label = "VillageEvent"; break;
                                case "LOCA": label = "LocationEvent"; break;
                                case "CHAR": label = "CharacterEvent"; break;
                                case "CALL": label = "_Call";  label_suffix = true; break;
                                case "MOVE": label = "_Move";  label_suffix = true; break;
                                case "FIGH": label = "_Fight"; label_suffix = true; break;
                                case "LOU1": case "LOUMODE1": case "LOUFILTERED":
                                case "LOU2": case "LOUMODE2": case "LOUFILTERED2":
                                    label = "_Units"; label_suffix = true; break;
                                default: label = null; break;
                            }
                            if (label_suffix)
                            {
                                Int32 name_end = code.LastIndexOf(':', index);
                                Int32 name_start = code.LastIndexOf('\n', name_end) + 1;
                                label = code.Substring(name_start, name_end - name_start) + label;
                            }
                        }
                        else if (args[i].StartsWith("0x"))
                        {
                            arguments[i] = Util.HexToInt(args[i]);
                        }
                        else try
                        {
                            arguments[i] = UInt32.Parse(args[i]);
                        }
                        catch { continue; }
                    }
                    if (command == "UNIT")
                    {
                        Int32 name_end = code.LastIndexOf(':', index);
                        Int32 name_start = code.LastIndexOf('\n', name_end) + 1;
                        label = code.Substring(name_start, name_end - name_start);
                    }
                    if (label != null && label_arg != -1)
                    {
                        Event.ApplyLabel(ref code, result, args[label_arg], ref label);
                    }
                    if (args.Length > 0)
                    {
                        result.Add(new Event(command, label, Event.GetLineNumber(code, new_index + 1), arguments));
                    }
                }
            }
            return result;
        }
        void Core_LoadEventCode_ManageSpace(ref String code)
        {
            Int32 index = 0;
            Int32 new_index;
            Int32 length;
            if (code.StartsWith("ORG "))
            {
                length = code.IndexOf("\r\n") + 2;
                code = code.Remove(0, length);
            }
            while ((new_index = code.IndexOf("ORG ", index)) > index)
            {
                index = new_index;
                length = code.IndexOf("\r\n", new_index) + 2 - index;
                code = code.Remove(index, length);
                code = code.Insert(index, "ALIGN 4\r\n");
            }
            index = 0;
            while ((new_index = code.IndexOf("ASSERT ", index)) > index)
            {
                index = code.LastIndexOf("//The next", new_index);
                length = code.IndexOf("\r\n", new_index) + 2 - index;
                code = code.Remove(index, length);
            }
        }
        void Core_LoadEventCode_MapUnitGroups()
        {
            this.UnitEvents_ListBox.ItemCheck -= this.UnitEvents_ListBox_ItemCheck;
            this.UnitEvents_ListBox.Items.Clear();
            Boolean display_units;
            for (Int32 i = 0; i < this.EventList.Count; i++)
            {
                if (this.EventList[i].Command == "UNIT" && this.EventList[i].Label != null)
                {
                    display_units = this.EventList[i].Label.StartsWith("Beginning");
                    if (Core.App.Game is FE7 &&
                        (this.EventList[i].Label.StartsWith("AllyUnits") ||
                        this.EventList[i].Label.StartsWith("EnemyUnits")))
                    {
                        display_units = (this.EventList[i].Label.EndsWith("ENM"));
                    }
                    if (!this.UnitEvents_ListBox.Items.Contains(this.EventList[i].Label))
                    {
                        this.UnitEvents_ListBox.Items.Add(this.EventList[i].Label, display_units);
                    }
                }
            }
            this.UnitEvents_ListBox.ItemCheck += this.UnitEvents_ListBox_ItemCheck;
        }

        void Core_LoadMap()
        {
            Pointer address = Core.GetPointer("Map Data Array");
            Pointer poin_palette    = Core.ReadPointer(address + 4 * (Byte)this.Current["Palette"]);
            Pointer poin_tileset1   = Core.ReadPointer(address + 4 * (Byte)this.Current["Tileset1"]);
            Pointer poin_tileset2   = Core.ReadPointer(address + 4 * (Byte)this.Current["Tileset2"]);
            Pointer poin_tsa        = Core.ReadPointer(address + 4 * (Byte)this.Current["TSA"]);
            Pointer poin_mapdata    = Core.ReadPointer(address + 4 * (Byte)this.Current["Map"]);
            Pointer poin_mapchanges = Core.ReadPointer(address + 4 * (Byte)this.Current["MapChanges"]);

            MapTileset map_tileset;
            try
            {
                map_tileset = new MapTileset(
                    Core.ReadData(poin_palette, Map.PALETTES * Palette.LENGTH),
                    Core.ReadData(poin_tileset1, 0),
                    Core.ReadData(poin_tileset2, 0),
                    Core.ReadData(poin_tsa, 0));
            }
            catch (Exception ex)
            {
                map_tileset = null;
                UI.ShowError("Could not load the the tileset for this chapter.", ex);
            }

            try
            {
                this.CurrentMap = new Map(map_tileset,
                    Core.ReadData(poin_mapdata, 0),
                    poin_mapchanges, false);
            }
            catch (Exception ex)
            {
                this.CurrentMap = null;
                UI.ShowError("Could not load the map for this chapter.", ex);
            }

            if (this.CurrentMap.Changes != null)
            {
                this.MapChanges_ListBox.ItemCheck -= this.MapChanges_ListBox_ItemCheck;
                if (this.MapChanges_ListBox.Items.Count > 0)
                {
                    for (Int32 i = 0; i < this.CurrentMap.ShowChanges.Length; i++)
                    {
                        this.CurrentMap.ShowChanges[i] = this.MapChanges_ListBox.GetItemChecked(i);
                    }
                    this.MapChanges_ListBox.Items.Clear();
                }
                for (Int32 i = 0; i < this.CurrentMap.Changes.Count; i++)
                {
                    this.MapChanges_ListBox.Items.Add(
                        "0x" + Util.ByteToHex(this.CurrentMap.Changes.GetNumber(i)),
                        this.CurrentMap.ShowChanges[i]);
                }
                this.MapChanges_ListBox.ItemCheck += this.MapChanges_ListBox_ItemCheck;
            }

            GBA.Bitmap result = new GBA.Bitmap(this.CurrentMap);

            if (this.View_Units.Checked)
            {
                this.Core_LoadMap_Units(result);
            }

            this.MapViewBox.Size = new Size(this.CurrentMap.Width, this.CurrentMap.Height);
            this.MapViewBox.Load(result);
        }
        void Core_LoadMap_Units(GBA.Bitmap result)
        {
            try
            {
                Pointer address_mapsprites = Core.GetPointer("Map Sprite Idle Array");
                Pointer address_classes = Core.GetPointer("Class Array");
                Pointer address;
                Byte palette_index;
                Byte palette_amount = 4;
                address = Core.GetPointer("Map Sprite Palettes");
                Byte[][] palettes = new Byte[palette_amount][];
                for (Int32 i = 0; i < palette_amount; i++)
                {
                    palettes[i] = Core.ReadData(address + i * Palette.LENGTH, Palette.LENGTH);
                    result.Colors.Add(new Palette(palettes[i]));
                }
                Int32 class_length = (Core.App.Game is FE6 ? 72 : 84);
                GBA.Image image;
                Byte unitclass;
                Byte mapsprite;
                Byte size;
                Tileset tileset;
                Int32 offsetX;
                Int32 offsetY;
                Int32 index;
                for (Int32 i = 0; i < this.EventList.Count; i++)
                {
                    if (this.EventList[i].Command == "UNIT" &&
                        this.EventList[i].Label != null)
                    {
                        index = this.UnitEvents_ListBox.Items.IndexOf(this.EventList[i].Label);
                        if (index == -1)
                            continue;
                        else if (!this.UnitEvents_ListBox.GetItemChecked(index))
                            continue;
                        unitclass = (Byte)this.EventList[i].Arguments[1];
                        mapsprite = Core.ReadByte(address_classes + 6 + unitclass * class_length);
                        address = address_mapsprites + mapsprite * 8;
                        size = Core.ReadByte(address + 2);
                        tileset = new Tileset(Core.ReadData(Core.ReadPointer(address + 4), 0));
                        palette_index = (Byte)(this.EventList[i].Arguments[3] & 6);
                        if (palette_index > 2) palette_index -= 3;
                        switch (size)
                        {
                            case 0x00: image = tileset.ToImage(2, 2, palettes[palette_index]); break;
                            case 0x01: image = tileset.ToImage(2, 4, palettes[palette_index]); break;
                            case 0x02: image = tileset.ToImage(4, 4, palettes[palette_index]); break;
                            default: image = null; break;
                        }
                        if (image != null)
                        {
                            offsetX = (Int32)(this.EventList[i].Arguments[UNIT_argX] * 16);
                            offsetY = (Int32)(this.EventList[i].Arguments[UNIT_argY] * 16);
                            offsetX -= (size == 0x02 ? 8 : 0);
                            offsetY -= (size == 0x00 ? 0 : 16);
                            Int32 pixel;
                            for (Int32 y = 0; y < image.Height; y++)
                            for (Int32 x = 0; x < image.Width; x++)
                            {
                                index = (x / 2) + y * (image.Width / 2);
                                pixel = (x % 2 == 0) ?
                                    (image.Bytes[index] & 0x0F) :
                                    (image.Bytes[index] & 0xF0) >> 4;
                                if (pixel != 0 &&
                                    offsetX + x >= 0 && offsetX + x < result.Width &&
                                    offsetY + y >= 0 && offsetY + y < result.Height)
                                {
                                    result.SetColor(offsetX + x, offsetY + y, image.Colors[pixel]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the units to display on the map.", ex);
            }
        }

        String Core_GetAllArrayDefines()
        {
            String result = "#define None 0\r\n";
            String define;
            ArrayFile definitions;
            String[] files = new String[]
            {
                "Dialog Background List.txt",
                "Cutscene Screen List.txt",
                "Music List.txt",
                "Chapter List.txt",
                "Character List.txt",
                "Class List.txt",
                "Item List.txt",
            };
            //FileInfo[] files = new DirectoryInfo(Core.Path_Arrays).GetFiles("*.txt");
            //foreach (FileInfo file in files)
            foreach (String file in files)
            {
                if (file.EndsWith("Commands.txt"))
                    continue;
                definitions = new ArrayFile(file);
                result += "\r\n";
                result += "// " + "file.Name\r\n";
                for (UInt32 i = 0; i <= definitions.LastEntry; i++)
                {
                    define = EventAssemblerIO.ReplaceSpacesAndSpecialChars(definitions[i]);
                    if (define.Equals("None"))
                        continue;
                    if (define.Length > 1)
                        result += "#define " + define + "\t" + i + "\r\n";
                }
            }
            return result;
        }

        

        private void File_Assemble_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Text file (*.event)|*.event|" +
                "All files (*.*)|*.*";

            String filepath = "";
            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                filepath = openWindow.FileName;
                if (filepath == null || !File.Exists(filepath))
                    throw new Exception("The given filepath is invalid.");
            }
            else return;

            UI.SuspendUpdate();

            EA_Log log = new EA_Log();
            ROM_Stream stream = new ROM_Stream(this);
            using (TextReader file = (TextReader)File.OpenText(filepath))
            using (BinaryWriter output = new BinaryWriter(stream))
            {
                stream.Description = "EventAssembler [" + this.Entry_ArrayBox.Text + "]";
                EA.Program.Assemble(this.Language, file, output, log);
                if (log.ErrorCount == 0)
                    stream.WriteToROM(this.Tools_ManageSpace.Checked);
                log.PrintAll();
            }

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        private void File_Assemble_CurrentText_Click(Object sender, EventArgs e)
        {
            UI.SuspendUpdate();

            String code = this.Core_GetAllArrayDefines() + this.Event_CodeBox.Text;
            EA_Log log = new EA_Log();
            ROM_Stream stream = new ROM_Stream(this);
            using (TextReader text = new StringReader(code))
            using (BinaryWriter output = new BinaryWriter(stream))
            {
                stream.Description = "EventAssembler [" + this.Entry_ArrayBox.Text + "]";
                EA.Program.Assemble(this.Language, text, output, log);
                if (log.ErrorCount == 0)
                    stream.WriteToROM(this.Tools_ManageSpace.Checked);
                log.PrintAll();
            }

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        private void File_Save_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Text file (*.event)|*.event|" +
                "All files (*.*)|*.*";
            try
            {
                if (saveWindow.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveWindow.FileName, this.Event_CodeBox.Text);
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save the event code text file.", ex);
            }
        }

        private void View_Grid_Click(Object sender, EventArgs e)
        {
            this.MapViewBox.ShowGrid = this.View_Grid.Checked;
        }
        private void View_Units_Click(Object sender, EventArgs e)
        {
            this.Core_LoadMap();
        }
        private void View_ArrayDefinitions_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_LoadEventCode();
        }

        private void Tools_MakeEAtxt_Click(Object sender, EventArgs e)
        {
            String[] file;
            try
            {
                OpenFileDialog openWindow = new OpenFileDialog();
                openWindow.Title = "Open EM array text file";
                openWindow.RestoreDirectory = true;
                openWindow.Multiselect = false;
                openWindow.FilterIndex = 1;
                openWindow.Filter =
                    "Text file (*.txt)|*.txt|" +
                    "All files (*.*)|*.*";
                if (openWindow.ShowDialog() == DialogResult.OK)
                {
                    file = File.ReadAllLines(openWindow.FileName);
                }
                else return;
                SaveFileDialog saveWindow = new SaveFileDialog();
                openWindow.Title = "Choose destination for EA definitions file";
                saveWindow.RestoreDirectory = true;
                saveWindow.OverwritePrompt = true;
                saveWindow.CreatePrompt = false;
                saveWindow.FilterIndex = 1;
                saveWindow.Filter =
                    "Text file (*.event)|*.event|" +
                    "All files (*.*)|*.*";
                if (saveWindow.ShowDialog() == DialogResult.OK)
                {
                    String number;
                    String define;
                    Int32 start, end;
                    for (Int32 i = 0; i < file.Length; i++)
                    {
                        start = 0;
                        end = file[i].IndexOfAny(new Char[] { ' ', '\t' });
                        if (end < 0) continue;
                        number = file[i].Substring(start, end - start);
                        start = end + 1;
                        end = file[i].IndexOfAny(new Char[] { '\r', '\n' }, start);
                        if (end < 0) end = file[i].Length;
                        define = EventAssemblerIO.ReplaceSpacesAndSpecialChars(file[i].Substring(start, end - start));
                        file[i] = ("#define " + define + "\t" + number);
                    }
                    File.WriteAllLines(saveWindow.FileName, file);
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not make EA definitions file from EM array text file.", ex);
            }
        }
        private void Tools_MakeEMtxt_Click(Object sender, EventArgs e)
        {
            String[] file;
            try
            {
                OpenFileDialog openWindow = new OpenFileDialog();
                openWindow.Title = "Open EA definitions file";
                openWindow.RestoreDirectory = true;
                openWindow.Multiselect = false;
                openWindow.FilterIndex = 1;
                openWindow.Filter =
                    "Text file (*.event)|*.event|" +
                    "All files (*.*)|*.*";
                if (openWindow.ShowDialog() == DialogResult.OK)
                {
                    file = File.ReadAllLines(openWindow.FileName);
                }
                else return;
                SaveFileDialog saveWindow = new SaveFileDialog();
                openWindow.Title = "Choose destination for EM array text file";
                saveWindow.RestoreDirectory = true;
                saveWindow.OverwritePrompt = true;
                saveWindow.CreatePrompt = false;
                saveWindow.FilterIndex = 1;
                saveWindow.Filter =
                    "Text file (*.txt)|*.txt|" +
                    "All files (*.*)|*.*";
                if (saveWindow.ShowDialog() == DialogResult.OK)
                {
                    String number;
                    String define;
                    Int32 start, end;
                    for (Int32 i = 0; i < file.Length; i++)
                    {
                        start = file[i].IndexOfAny(new Char[] { ' ', '\t' });
                        if (start < 0) continue;
                        while (start < file[i].Length && (file[i][start] == ' ' || file[i][start] == '\t')) ++start;
                        end = file[i].IndexOfAny(new Char[] { ' ', '\t' }, start);
                        if (end < 0) continue;
                        define = file[i].Substring(start, end - start);
                        start = end;
                        while (start < file[i].Length && (file[i][start] == ' ' || file[i][start] == '\t')) ++start;
                        end = file[i].IndexOfAny(new Char[] { ' ', '\t', '\r', '\n' }, start);
                        if (end < 0) end = file[i].Length;
                        number = file[i].Substring(start, end - start);
                        file[i] = (number + " " + define);
                    }
                    File.WriteAllLines(saveWindow.FileName, file);
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not make EM array text file from EA definitions file.", ex);
            }
        }
        private void Tools_ManageSpace_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_LoadEventCode();
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
            this.Chapter_MagicButton.EntryToSelect = this.Entry_ArrayBox.Value;
        }

        private void Event_ArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WriteByte(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Events"),
                this.Event_ArrayBox.Value,
                "Chapter Struct 0x" + Util.ByteToHex((Byte)this.Entry_ArrayBox.Value) +
                " [" + this.Entry_ArrayBox.Text + "] - Events changed");
        }
        private void Event_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * this.Event_ArrayBox.Value,
                this.Event_PointerBox.Value,
                "Map Data Array 0x" + Util.ByteToHex(this.Event_ArrayBox.Value) +
                " [" + this.Event_ArrayBox.Text + "] - repointed");
        }

        private Point MouseHoverLocation;
        private Boolean MouseHoverFinished;
        protected Timer Help_ToolTip_Timer;
        private void Event_CodeBox_MouseMove(Object sender, MouseEventArgs e)
        {
            if (Math.Abs(e.X - this.MouseHoverLocation.X) > 4 &&
                Math.Abs(e.Y - this.MouseHoverLocation.Y) > 4)
            { // Restart timer if the mouse moved too far
                if (this.MouseHoverFinished)
                    this.MouseHoverFinished = false;
                if (this.Help_ToolTip_Timer.Enabled)
                    this.Help_ToolTip_Timer.Enabled = false;
            }
            this.MouseHoverLocation = e.Location;
            this.Help_ToolTip_Timer.Enabled = true;
        }
        private void HoverTick(Object sender, EventArgs e)
        {
            this.Help_ToolTip_Timer.Enabled = false;
            this.MouseHoverFinished = true;
            try
            {
                Int32 code_index = this.Event_CodeBox.GetIndexFromPosition(this.MouseHoverLocation);
                String command = Event.GetWord(this.Event_CodeBox.Text, code_index);

                if (command.Length > 0)
                {
                    String caption = "";

                    if (command.EndsWith(":"))
                    {
                        command = command.Substring(0, command.Length - 1);
                        caption = "Pointer label";
                    }
                    else if (command.StartsWith("#"))
                    {
                        caption = "C preprocessor directive";
                    }
                    else if (command.StartsWith("$"))
                    {
                        caption = "Pointer/Address number";
                    }
                    else if (command.StartsWith("0x"))
                    {
                        caption = "Hexadecimal number";
                    }
                    if (caption == "")
                    {
                        caption = this.LanguageProcessor.GetDoc(command,
                            Core.App.Game.Identifier.Substring(0, 3));
                        if (caption.StartsWith("command:"))
                        {
                            Int32 length = caption.IndexOf('\n');
                            command = command + " - " + caption.Substring(8, length - 8);
                            caption = caption.Substring(length + 1);
                        }
                    }
                    if (caption == "")
                    {
                        //caption = ;
                    }

                    if (caption.Length > 0)
                    {
                        if (this.Help_ToolTip.Active && this.Help_ToolTip.ToolTipTitle == command)
                            return;
                        this.Help_ToolTip.Active = true;
                        this.Help_ToolTip.ToolTipTitle = command;
                        this.Help_ToolTip.Show(caption, this,
                            this.Event_CodeBox.Location.X + this.MouseHoverLocation.X,
                            this.Event_CodeBox.Location.Y + this.MouseHoverLocation.Y + 40);
                    }
                    else
                    {
                        this.Help_ToolTip.Hide(this);
                        this.Help_ToolTip.Active = false;
                    }
                }
                else
                {
                    this.Help_ToolTip.Hide(this);
                    this.Help_ToolTip.Active = false;
                }
            }
            catch (Exception ex)
            {
                //UI.ShowError("There has been an error while loading the mousehover tooltip documentation.", ex);
            }
        }

        private void MapViewBox_SelectionChanged(Object sender, EventArgs e)
        {
            if (this.MapViewBox.SelectionIsSingle())
            {
                Point selection = this.MapViewBox.GetSelectionCoords();
                this.MapSelection_Label.Text = "X: " + selection.X + ", Y: " + selection.Y;
            }
            else this.MapSelection_Label.Text = "X: __, Y: __";

            Int32 width = this.MapViewBox.Selection.GetLength(0);
            Int32 height = this.MapViewBox.Selection.GetLength(1);
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (this.MapViewBox.Selection[x, y])
                {
                    for (Int32 i = 0; i < this.EventList.Count; i++)
                    {
                        if (this.EventList[i].Command == "UNIT" &&
                            this.EventList[i].Arguments[UNIT_argX] == x &&
                            this.EventList[i].Arguments[UNIT_argY] == y)
                        {
                                Int32 index = this.UnitEvents_ListBox.Items.IndexOf(this.EventList[i].Label);
                            if (index == -1) continue;
                            else if (this.UnitEvents_ListBox.GetItemChecked(index))
                            {
                                index = Event.GetCodeIndex(this.Event_CodeBox.Text, this.EventList[i].CodeLineNumber);
                                    this.Event_CodeBox.SelectionStart = index;
                                    this.Event_CodeBox.SelectionLength = 4;
                                    this.Event_CodeBox.DoSelectionVisible();
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void UnitEvents_ListBox_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(this.Core_LoadMap), null);
        }
        private void MapChanges_ListBox_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(this.Core_LoadMap), null);
        }

        private void EventEditor_SizeChanged(Object sender, EventArgs e)
        {
            AnchorStyles all, left, right;
            all = (AnchorStyles.Top | AnchorStyles.Bottom);
            left = (all | AnchorStyles.Left);
            right = (all | AnchorStyles.Right);
            all = (AnchorStyles)0xF;
            if (this.Event_CodeBox.Anchor == all)
            {
                if (this.Event_CodeBox.Width == this.Event_CodeBox.MinimumSize.Width)
                {
                    this.Event_CodeBox.Anchor = left;
                    this.Map_Panel.Anchor = all;
                }
                else this.Map_Panel.Width = this.Width - this.Event_CodeBox.Width - 47;
            }
            else
            {
                if (this.Map_Panel.Width == this.Map_Panel.MaximumSize.Width)
                {
                    this.Event_CodeBox.Anchor = all;
                    this.Map_Panel.Anchor = right;
                }
                else this.Event_CodeBox.Width = this.Width - this.Map_Panel.Width - 47;
            }
            if (this.Map_Panel.Location.X < 420)
                this.Map_Panel.Location = new Point(420, this.Map_Panel.Location.Y);
        }
    }
}