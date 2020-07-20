using EmblemMagic.FireEmblem;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
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

        static int UNIT_argX = (Core.CurrentROM is FE8) ? 4 : 6;
        static int UNIT_argY = (Core.CurrentROM is FE8) ? 5 : 7;



        public EventEditor()
        {
            try
            {
                InitializeComponent();

                Entry_ArrayBox.Load("Chapter List.txt");
                Event_ArrayBox.Load("Map List.txt");

                Current = new StructFile("Chapter Struct.txt");

                Help_ToolTip_Timer = new Timer();
                Help_ToolTip_Timer.Tick += HoverTick;
                Help_ToolTip_Timer.Interval = 800;

                Chapter_MagicButton.EditorToOpen = "Module:Chapter Editor";
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        override public void Core_OnOpen()
        {
            Core_LoadEventSyntax();

            Core_Update();
        }
        override public void Core_Update()
        {
            Current.EntryIndex = Entry_ArrayBox.Value;
            Core_LoadValues();
            Core_LoadEventCode();
            MapChanges_ListBox.Items.Clear();
            Core_LoadMap();
        }

        void Core_LoadValues()
        {
            Event_ArrayBox.ValueChanged -= Event_ArrayBox_ValueChanged;
            Event_PointerBox.ValueChanged -= Event_PointerBox_ValueChanged;

            Event_ArrayBox.Value = Current["Events"];
            Event_PointerBox.Value = Core.ReadPointer(Core.GetPointer("Map Data Array") + 4 * Event_ArrayBox.Value);

            Event_ArrayBox.ValueChanged -= Event_ArrayBox_ValueChanged;
            Event_PointerBox.ValueChanged -= Event_PointerBox_ValueChanged;
        }
        
        void Core_LoadEventSyntax()
        {
            try
            {
                if (!EA.Program.CodesLoaded || LanguageProcessor == null)
                {
                    string folder =
#if (DEBUG)
"D:\\Lexou\\Projects\\EmblemMagic\\EventAssembler\\Event Assembler\\Event Assembler";
#else
Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(EA.Program)).Location);
#endif
                    LanguageProcessor = EA.Program.LoadCodes(folder + "\\Language Raws", ".txt", true, true);
                }
                Language = EA.Program.Languages[Core.CurrentROM.GetIdentifier().Substring(0, 3)];
                string keywords = "";
                IEnumerable<string> codes = Language.GetCodeNames();
                foreach (string code in codes)
                {
                    keywords += code;
                    keywords += "( |\r\n)|";
                }
                Event_CodeBox.AddSyntax("(//).*", SystemColors.ControlDark);
                Event_CodeBox.AddSyntax("#.* ", System.Drawing.Color.LimeGreen, FontStyle.Italic);
                Event_CodeBox.AddSyntax(keywords.TrimEnd('|'), SystemColors.Highlight, FontStyle.Bold);
                Event_CodeBox.AddSyntax(@"((\b[0-9]+)|((\b0x|\$)[0-9a-fA-F]+))\b", System.Drawing.Color.SlateBlue);
                Event_CodeBox.AddCollapse(":\r\n", "\r\n\r\n");
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while loading the EventAssembler language codes.", ex);
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
                    EA.Program.Disassemble(Language,
                        Program.Core.ROM.FileData,
                        "", writer, true,
                        EA.DisassemblyMode.Structure,
                        Event_PointerBox.Value,
                        EA.Code.Language.Priority.none,
                        0, log);
                    if (log.ErrorCount > 0 || log.MessageCount > 0)
                    {
                        log.PrintAll();
                    }
                }
                string result = stringbuilder.ToString().TrimStart('\r', '\n');
                EventList = Core_LoadEventCode_EventLabels(ref result);
                if (Tools_ManageSpace.Checked)
                    Core_LoadEventCode_ManageSpace(ref result);
                if (View_ArrayDefinitions.Checked)
                    EventAssemblerIO.LoadEventCode_ArrayDefinitions(ref result);
                if (View_HelperMacros.Checked)
                    EventAssemblerIO.LoadEventCode_HelperMacros(ref result);
                Core_LoadEventCode_MapUnitGroups();
                Event_CodeBox.Text = result;
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while loading the event code.", ex);
            }
        }
        List<Event> Core_LoadEventCode_EventLabels(ref string code)
        {
            List<Event> result = new List<Event>();
            string[] commands = new string[]
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
            string[] args;
            uint[] arguments;

            string label;
            int label_arg;
            int index;
            int new_index;
            int length;
            foreach (string command in commands)
            {
                index = 0;
                while ((new_index = code.IndexOf("\n" + command + " ", index)) > index)
                {
                    index = new_index + command.Length + 2;
                    length = code.IndexOf("\r\n", index) - index;
                    args = code.Substring(index, length).Split(
                        new char[] { ' ', ',', '[', ']', '\t' },
                        StringSplitOptions.RemoveEmptyEntries);
                    arguments = new uint[args.Length];
                    label = null;
                    label_arg = -1;
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i].Length >= 5 && args[i].Substring(0, 5) == "label")
                        {
                            bool label_suffix = false;
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
                                int name_end = code.LastIndexOf(':', index);
                                int name_start = code.LastIndexOf('\n', name_end) + 1;
                                label = code.Substring(name_start, name_end - name_start) + label;
                            }
                        }
                        else if (args[i].StartsWith("0x"))
                        {
                            arguments[i] = Util.HexToInt(args[i]);
                        }
                        else try
                        {
                            arguments[i] = uint.Parse(args[i]);
                        }
                        catch { continue; }
                    }
                    if (command == "UNIT")
                    {
                        int name_end = code.LastIndexOf(':', index);
                        int name_start = code.LastIndexOf('\n', name_end) + 1;
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
        void Core_LoadEventCode_ManageSpace(ref string code)
        {
            int index = 0;
            int new_index;
            int length;
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
            UnitEvents_ListBox.ItemCheck -= UnitEvents_ListBox_ItemCheck;
            UnitEvents_ListBox.Items.Clear();
            bool display_units;
            for (int i = 0; i < EventList.Count; i++)
            {
                if (EventList[i].Command == "UNIT" && EventList[i].Label != null)
                {
                    display_units = EventList[i].Label.StartsWith("Beginning");
                    if (Core.CurrentROM is FE7 &&
                        (EventList[i].Label.StartsWith("AllyUnits") ||
                        EventList[i].Label.StartsWith("EnemyUnits")))
                    {
                        display_units = (EventList[i].Label.EndsWith("ENM"));
                    }
                    if (!UnitEvents_ListBox.Items.Contains(EventList[i].Label))
                    {
                        UnitEvents_ListBox.Items.Add(EventList[i].Label, display_units);
                    }
                }
            }
            UnitEvents_ListBox.ItemCheck += UnitEvents_ListBox_ItemCheck;
        }

        void Core_LoadMap()
        {
            Pointer address = Core.GetPointer("Map Data Array");
            Pointer poin_palette    = Core.ReadPointer(address + 4 * (byte)Current["Palette"]);
            Pointer poin_tileset1   = Core.ReadPointer(address + 4 * (byte)Current["Tileset1"]);
            Pointer poin_tileset2   = Core.ReadPointer(address + 4 * (byte)Current["Tileset2"]);
            Pointer poin_tsa        = Core.ReadPointer(address + 4 * (byte)Current["TSA"]);
            Pointer poin_mapdata    = Core.ReadPointer(address + 4 * (byte)Current["Map"]);
            Pointer poin_mapchanges = Core.ReadPointer(address + 4 * (byte)Current["MapChanges"]);

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
                Program.ShowError("Could not load the the tileset for this chapter.", ex);
            }

            try
            {
                CurrentMap = new Map(map_tileset,
                    Core.ReadData(poin_mapdata, 0),
                    poin_mapchanges, false);
            }
            catch (Exception ex)
            {
                CurrentMap = null;
                Program.ShowError("Could not load the map for this chapter.", ex);
            }

            if (CurrentMap.Changes != null)
            {
                MapChanges_ListBox.ItemCheck -= MapChanges_ListBox_ItemCheck;
                if (MapChanges_ListBox.Items.Count > 0)
                {
                    for (int i = 0; i < CurrentMap.ShowChanges.Length; i++)
                    {
                        CurrentMap.ShowChanges[i] = MapChanges_ListBox.GetItemChecked(i);
                    }
                    MapChanges_ListBox.Items.Clear();
                }
                for (int i = 0; i < CurrentMap.Changes.Count; i++)
                {
                    MapChanges_ListBox.Items.Add(
                        "0x" + Util.ByteToHex(CurrentMap.Changes.GetNumber(i)),
                        CurrentMap.ShowChanges[i]);
                }
                MapChanges_ListBox.ItemCheck += MapChanges_ListBox_ItemCheck;
            }

            GBA.Bitmap result = new GBA.Bitmap(CurrentMap);

            if (View_Units.Checked)
            {
                Core_LoadMap_Units(result);
            }

            MapViewBox.Size = new Size(CurrentMap.Width, CurrentMap.Height);
            MapViewBox.Load(result);
        }
        void Core_LoadMap_Units(GBA.Bitmap result)
        {
            try
            {
                Pointer address_mapsprites = Core.GetPointer("Map Sprite Idle Array");
                Pointer address_classes = Core.GetPointer("Class Array");
                Pointer address;
                byte palette_index;
                byte palette_amount = 4;
                address = Core.GetPointer("Map Sprite Palettes");
                byte[][] palettes = new byte[palette_amount][];
                for (int i = 0; i < palette_amount; i++)
                {
                    palettes[i] = Core.ReadData(address + i * Palette.LENGTH, Palette.LENGTH);
                    result.Colors.Add(new Palette(palettes[i]));
                }
                int class_length = (Core.CurrentROM is FE6 ? 72 : 84);
                GBA.Image image;
                byte unitclass;
                byte mapsprite;
                byte size;
                Tileset tileset;
                int offsetX;
                int offsetY;
                int index;
                for (int i = 0; i < EventList.Count; i++)
                {
                    if (EventList[i].Command == "UNIT" &&
                        EventList[i].Label != null)
                    {
                        index = UnitEvents_ListBox.Items.IndexOf(EventList[i].Label);
                        if (index == -1)
                            continue;
                        else if (!UnitEvents_ListBox.GetItemChecked(index))
                            continue;
                        unitclass = (byte)EventList[i].Arguments[1];
                        mapsprite = Core.ReadByte(address_classes + 6 + unitclass * class_length);
                        address = address_mapsprites + mapsprite * 8;
                        size = Core.ReadByte(address + 2);
                        tileset = new Tileset(Core.ReadData(Core.ReadPointer(address + 4), 0));
                        palette_index = (byte)(EventList[i].Arguments[3] & 6);
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
                            offsetX = (int)(EventList[i].Arguments[UNIT_argX] * 16);
                            offsetY = (int)(EventList[i].Arguments[UNIT_argY] * 16);
                            offsetX -= (size == 0x02 ? 8 : 0);
                            offsetY -= (size == 0x00 ? 0 : 16);
                            int pixel;
                            for (int y = 0; y < image.Height; y++)
                            for (int x = 0; x < image.Width; x++)
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
                Program.ShowError("Could not load the units to display on the map.", ex);
            }
        }

        string Core_GetAllArrayDefines()
        {
            string result = "#define None 0\r\n";
            string define;
            ArrayFile definitions;
            string[] files = new string[]
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
            foreach (string file in files)
            {
                if (file.EndsWith("Commands.txt"))
                    continue;
                definitions = new ArrayFile(file);
                result += "\r\n";
                result += "// " + "file.Name\r\n";
                for (uint i = 0; i <= definitions.LastEntry; i++)
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

        

        private void File_Assemble_Click(object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Text file (*.event)|*.event|" +
                "All files (*.*)|*.*";

            string filepath = "";
            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                filepath = openWindow.FileName;
                if (filepath == null || !File.Exists(filepath))
                    throw new Exception("The given filepath is invalid.");
            }
            else return;

            Core.SuspendUpdate();

            EA_Log log = new EA_Log();
            ROM_Stream stream = new ROM_Stream(this);
            using (TextReader file = (TextReader)File.OpenText(filepath))
            using (BinaryWriter output = new BinaryWriter(stream))
            {
                stream.Description = "EventAssembler [" + Entry_ArrayBox.Text + "]";
                EA.Program.Assemble(Language, file, output, log);
                if (log.ErrorCount == 0)
                    stream.WriteToROM(Tools_ManageSpace.Checked);
                log.PrintAll();
            }

            Core.ResumeUpdate();
            Core.PerformUpdate();
        }
        private void File_Assemble_CurrentText_Click(object sender, EventArgs e)
        {
            Core.SuspendUpdate();

            string code = Core_GetAllArrayDefines() + Event_CodeBox.Text;
            EA_Log log = new EA_Log();
            ROM_Stream stream = new ROM_Stream(this);
            using (TextReader text = new StringReader(code))
            using (BinaryWriter output = new BinaryWriter(stream))
            {
                stream.Description = "EventAssembler [" + Entry_ArrayBox.Text + "]";
                EA.Program.Assemble(Language, text, output, log);
                if (log.ErrorCount == 0)
                    stream.WriteToROM(Tools_ManageSpace.Checked);
                log.PrintAll();
            }

            Core.ResumeUpdate();
            Core.PerformUpdate();
        }
        private void File_Save_Click(object sender, EventArgs e)
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
                    File.WriteAllText(saveWindow.FileName, Event_CodeBox.Text);
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save the event code text file.", ex);
            }
        }

        private void View_Grid_Click(object sender, EventArgs e)
        {
            MapViewBox.ShowGrid = View_Grid.Checked;
        }
        private void View_Units_Click(object sender, EventArgs e)
        {
            Core_LoadMap();
        }
        private void View_ArrayDefinitions_CheckedChanged(object sender, EventArgs e)
        {
            Core_LoadEventCode();
        }

        private void Tools_MakeEAtxt_Click(object sender, EventArgs e)
        {
            string[] file;
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
                    string number;
                    string define;
                    int start, end;
                    for (int i = 0; i < file.Length; i++)
                    {
                        start = 0;
                        end = file[i].IndexOfAny(new char[] { ' ', '\t' });
                        if (end < 0) continue;
                        number = file[i].Substring(start, end - start);
                        start = end + 1;
                        end = file[i].IndexOfAny(new char[] { '\r', '\n' }, start);
                        if (end < 0) end = file[i].Length;
                        define = EventAssemblerIO.ReplaceSpacesAndSpecialChars(file[i].Substring(start, end - start));
                        file[i] = ("#define " + define + "\t" + number);
                    }
                    File.WriteAllLines(saveWindow.FileName, file);
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not make EA definitions file from EM array text file.", ex);
            }
        }
        private void Tools_MakeEMtxt_Click(object sender, EventArgs e)
        {
            string[] file;
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
                    string number;
                    string define;
                    int start, end;
                    for (int i = 0; i < file.Length; i++)
                    {
                        start = file[i].IndexOfAny(new char[] { ' ', '\t' });
                        if (start < 0) continue;
                        while (start < file[i].Length && (file[i][start] == ' ' || file[i][start] == '\t')) ++start;
                        end = file[i].IndexOfAny(new char[] { ' ', '\t' }, start);
                        if (end < 0) continue;
                        define = file[i].Substring(start, end - start);
                        start = end;
                        while (start < file[i].Length && (file[i][start] == ' ' || file[i][start] == '\t')) ++start;
                        end = file[i].IndexOfAny(new char[] { ' ', '\t', '\r', '\n' }, start);
                        if (end < 0) end = file[i].Length;
                        number = file[i].Substring(start, end - start);
                        file[i] = (number + " " + define);
                    }
                    File.WriteAllLines(saveWindow.FileName, file);
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not make EM array text file from EA definitions file.", ex);
            }
        }
        private void Tools_ManageSpace_CheckedChanged(object sender, EventArgs e)
        {
            Core_LoadEventCode();
        }



        private void EntryArrayBox_ValueChanged(object sender, EventArgs e)
        {
            Core_Update();
            Chapter_MagicButton.EntryToSelect = Entry_ArrayBox.Value;
        }

        private void Event_ArrayBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WriteByte(this,
                Current.GetAddress(Current.EntryIndex, "Events"),
                Event_ArrayBox.Value,
                "Chapter Struct 0x" + Util.ByteToHex((byte)Entry_ArrayBox.Value) +
                " [" + Entry_ArrayBox.Text + "] - Events changed");
        }
        private void Event_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Map Data Array") + 4 * Event_ArrayBox.Value,
                Event_PointerBox.Value,
                "Map Data Array 0x" + Util.ByteToHex(Event_ArrayBox.Value) +
                " [" + Event_ArrayBox.Text + "] - repointed");
        }

        private Point MouseHoverLocation;
        private bool MouseHoverFinished;
        protected Timer Help_ToolTip_Timer;
        private void Event_CodeBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (Math.Abs(e.X - MouseHoverLocation.X) > 4 &&
                Math.Abs(e.Y - MouseHoverLocation.Y) > 4)
            { // Restart timer if the mouse moved too far
                if (MouseHoverFinished)
                    MouseHoverFinished = false;
                if (Help_ToolTip_Timer.Enabled)
                    Help_ToolTip_Timer.Enabled = false;
            }
            MouseHoverLocation = e.Location;
            Help_ToolTip_Timer.Enabled = true;
        }
        private void HoverTick(object sender, EventArgs e)
        {
            Help_ToolTip_Timer.Enabled = false;
            MouseHoverFinished = true;
            try
            {
                int code_index = Event_CodeBox.GetIndexFromPosition(MouseHoverLocation);
                string command = Event.GetWord(Event_CodeBox.Text, code_index);

                if (command.Length > 0)
                {
                    string caption = "";

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
                        caption = LanguageProcessor.GetDoc(command,
                            Core.CurrentROM.GetIdentifier().Substring(0, 3));
                        if (caption.StartsWith("command:"))
                        {
                            int length = caption.IndexOf('\n');
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
                        if (Help_ToolTip.Active && Help_ToolTip.ToolTipTitle == command)
                            return;
                        Help_ToolTip.Active = true;
                        Help_ToolTip.ToolTipTitle = command;
                        Help_ToolTip.Show(caption, this,
                            Event_CodeBox.Location.X + MouseHoverLocation.X,
                            Event_CodeBox.Location.Y + MouseHoverLocation.Y + 40);
                    }
                    else
                    {
                        Help_ToolTip.Hide(this);
                        Help_ToolTip.Active = false;
                    }
                }
                else
                {
                    Help_ToolTip.Hide(this);
                    Help_ToolTip.Active = false;
                }
            }
            catch (Exception ex)
            {
                //Program.ShowError("There has been an error while loading the mousehover tooltip documentation.", ex);
            }
        }

        private void MapViewBox_SelectionChanged(object sender, EventArgs e)
        {
            if (MapViewBox.SelectionIsSingle())
            {
                Point selection = MapViewBox.GetSelectionCoords();
                MapSelection_Label.Text = "X: " + selection.X + ", Y: " + selection.Y;
            }
            else MapSelection_Label.Text = "X: __, Y: __";

            int width = MapViewBox.Selection.GetLength(0);
            int height = MapViewBox.Selection.GetLength(1);
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                if (MapViewBox.Selection[x, y])
                {
                    for (int i = 0; i < EventList.Count; i++)
                    {
                        if (EventList[i].Command == "UNIT" &&
                            EventList[i].Arguments[UNIT_argX] == x &&
                            EventList[i].Arguments[UNIT_argY] == y)
                        {
                            int index = UnitEvents_ListBox.Items.IndexOf(EventList[i].Label);
                            if (index == -1) continue;
                            else if (UnitEvents_ListBox.GetItemChecked(index))
                            {
                                index = Event.GetCodeIndex(Event_CodeBox.Text, EventList[i].CodeLineNumber);
                                Event_CodeBox.SelectionStart = index;
                                Event_CodeBox.SelectionLength = 4;
                                Event_CodeBox.DoSelectionVisible();
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void UnitEvents_ListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(Core_LoadMap), null);
        }
        private void MapChanges_ListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(Core_LoadMap), null);
        }

        private void EventEditor_SizeChanged(object sender, EventArgs e)
        {
            AnchorStyles all, left, right;
            all = (AnchorStyles.Top | AnchorStyles.Bottom);
            left = (all | AnchorStyles.Left);
            right = (all | AnchorStyles.Right);
            all = (AnchorStyles)0xF;
            if (Event_CodeBox.Anchor == all)
            {
                if (Event_CodeBox.Width == Event_CodeBox.MinimumSize.Width)
                {
                    Event_CodeBox.Anchor = left;
                    Map_Panel.Anchor = all;
                }
                else Map_Panel.Width = this.Width - Event_CodeBox.Width - 47;
            }
            else
            {
                if (Map_Panel.Width == Map_Panel.MaximumSize.Width)
                {
                    Event_CodeBox.Anchor = all;
                    Map_Panel.Anchor = right;
                }
                else Event_CodeBox.Width = this.Width - Map_Panel.Width - 47;
            }
            if (Map_Panel.Location.X < 420)
                Map_Panel.Location = new Point(420, Map_Panel.Location.Y);
        }
    }
}