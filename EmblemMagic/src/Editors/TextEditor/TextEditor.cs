using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Compression;
using EmblemMagic.FireEmblem;
using EmblemMagic.Properties;
using Magic;
using Magic.Editors;
using GBA;

namespace EmblemMagic.Editors
{
    public partial class TextEditor : Editor
    {
        ArrayFile TextCommands1;
        ArrayFile TextCommands2;
        ArrayFile PortraitList;

        TextFind FindWindow;
        TextReplace ReplaceWindow;

        public FireEmblem.TextPreview CurrentTextPreview { get; set; }
        public FireEmblem.Font CurrentFont { get; set; }

        public UInt16 CurrentIndex
        {
            get
            {
                return EntryNumBox.Value;
            }
        }
        public String CurrentEntry
        {
            get
            {
                return "Text Entry 0x" + Util.UInt16ToHex(EntryNumBox.Value) + " - ";
            }
        }
        public Magic.Range CurrentSelection
        {
            get
            {
                return new Magic.Range(Text_CodeBox.SelectionStart, Text_CodeBox.SelectionStart + Text_CodeBox.SelectionLength);
            }
            set
            {
                Text_CodeBox.SelectionStart = (Int32)value.Start;
                Text_CodeBox.SelectionLength = (Int32)value.Length;
                this.Select();
                //Text_CodeBox.ScrollToCaret();
            }
        }



        public TextEditor(IApp app) : base(app)
        {
            try
            {
                InitializeComponent();
                TextCommands1 = new ArrayFile("Text Commands.txt");
                TextCommands2 = new ArrayFile("Text Commands 2.txt");
                PortraitList = new ArrayFile("Portrait List.txt");

                Text_CodeBox.KeyDown += new KeyEventHandler(TextBox_SelectAll);
                Text_CodeBox.AddSyntax(@"\[(.*?)\]", System.Drawing.SystemColors.Highlight);

                Font_ComboBox.DataSource = new String[]
                {
                    "Menu Font",
                    "Text Bubble Font"
                };
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        public override void Core_SetEntry(UInt32 entry)
        {
            EntryNumBox.Value = (UInt16)entry;
        }
        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Core_LoadFont();
            Core_LoadText();
            Core_LoadPreview();
        }

        void Core_LoadFont()
        {
            try
            {
                CurrentFont = new Font(Core.GetPointer((String)Font_ComboBox.SelectedValue));

                Font_GridBox.Load(CurrentFont);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the font.", ex);
            }
        }
        void Core_LoadFontValues()
        {
            Glyph_Address_PointerBox.ValueChanged -= Glyph_Address_PointerBox_ValueChanged;
            Glyph_Pointer_PointerBox.ValueChanged -= Glyph_Pointer_PointerBox_ValueChanged;
            Glyph_Shift_ByteBox.ValueChanged -= Glyph_Shift_ByteBox_ValueChanged;
            Glyph_Width_ByteBox.ValueChanged -= Glyph_Width_ByteBox_ValueChanged;

            if (Font_GridBox.SelectionIsEmpty())
            {
                Glyph_Address_PointerBox.Value = new Pointer();
                Glyph_Pointer_PointerBox.Value = new Pointer();
                Glyph_Shift_ByteBox.Value = 0;
                Glyph_Width_ByteBox.Value = 0;

                Glyph_Address_PointerBox.Text = Glyph_Address_PointerBox.Value.ToString();
                Glyph_Pointer_PointerBox.Text = Glyph_Pointer_PointerBox.Value.ToString();
                Glyph_Shift_ByteBox.Text = Glyph_Shift_ByteBox.Value.ToString();
                Glyph_Width_ByteBox.Text = Glyph_Width_ByteBox.Value.ToString();
            }
            else if (Font_GridBox.SelectionIsSingle())
            {
                System.Drawing.Point selection = Font_GridBox.GetSelectionCoords();
                Int32 index = selection.X + selection.Y * 16;
                if (CurrentFont.Glyphs[index] == null)
                {
                    Glyph_Address_PointerBox.Value = new Pointer();
                    Glyph_Pointer_PointerBox.Value = new Pointer();
                    Glyph_Shift_ByteBox.Value = 0;
                    Glyph_Width_ByteBox.Value = 0;
                }
                else
                {
                    Glyph_Address_PointerBox.Value = CurrentFont.Glyphs[index].Address;
                    Glyph_Pointer_PointerBox.Value = CurrentFont.Glyphs[index].LinkedAddress;
                    Glyph_Shift_ByteBox.Value = CurrentFont.Glyphs[index].ShiftJIS;
                    Glyph_Width_ByteBox.Value = CurrentFont.Glyphs[index].TextWidth;
                }
                Glyph_Address_PointerBox.Text = Glyph_Address_PointerBox.Value.ToString();
                Glyph_Pointer_PointerBox.Text = Glyph_Pointer_PointerBox.Value.ToString();
                Glyph_Shift_ByteBox.Text = Glyph_Shift_ByteBox.Value.ToString();
                Glyph_Width_ByteBox.Text = Glyph_Width_ByteBox.Value.ToString();
            }
            else
            {
                Glyph_Address_PointerBox.Text = "";
                Glyph_Pointer_PointerBox.Text = "";
                Glyph_Shift_ByteBox.Text = "";
                Glyph_Width_ByteBox.Text = "";
            }

            Glyph_Address_PointerBox.ValueChanged += Glyph_Address_PointerBox_ValueChanged;
            Glyph_Pointer_PointerBox.ValueChanged += Glyph_Pointer_PointerBox_ValueChanged;
            Glyph_Shift_ByteBox.ValueChanged += Glyph_Shift_ByteBox_ValueChanged;
            Glyph_Width_ByteBox.ValueChanged += Glyph_Width_ByteBox_ValueChanged;
        }
        void Core_LoadText()
        {
            try
            {
                Pointer address = Core.ReadPointer(Core.GetPointer("Text Array") + EntryNumBox.Value * 4);

                Text_PointerBox.Value = new Pointer((UInt32)(address & 0x7FFFFFFF));
                Text_ASCII_CheckBox.CheckedChanged -= Text_ASCII_CheckBox_CheckedChanged;
                Text_ASCII_CheckBox.Checked = ((address & 0x80000000) != 0);
                Text_ASCII_CheckBox.CheckedChanged += Text_ASCII_CheckBox_CheckedChanged;

                Text_CodeBox.Text = Core_GetText(EntryNumBox.Value);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the text.", ex);
            }
        }
        void Core_LoadPreview()
        {
            try
            {
                String[] text = FireEmblem.Text.RemoveBracketCodes(Text_CodeBox.Text.Split('\r', '\n'));
                Boolean bubble = ((String)Font_ComboBox.SelectedValue).Equals("Text Bubble Font");

                CurrentTextPreview = new TextPreview(CurrentFont, bubble, text, Text_Line_NumBox.Value);

                Text_Line_NumBox.Maximum = Math.Max(0, text.Length - (bubble ? 2 : 4));
                Text_Preview_ImageBox.Load(CurrentTextPreview);
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while loading the text preview image.", ex);
            }
        }

        public String Core_GetText(UInt16 entry)
        {
            Pointer address = Core.ReadPointer(Core.GetPointer("Text Array") + entry * 4);

            if ((address & 0x80000000) == 0)
            {
                if (address > Core.CurrentROMSize)
                    throw new Exception("Invalid pointer read in the Text Array: " + address);

                return FireEmblem.Text.BytesToText(
                    Huffman.Decompress(address,
                        Core.GetPointer("Huffman Tree"),
                        Core.ReadPointer(Core.GetPointer("Huffman Tree Root"))),
                    View_Bytecodes.Checked,
                    TextCommands1,
                    TextCommands2,
                    PortraitList);
            }
            else
            {
                address = new Pointer((UInt32)(address & 0x7FFFFFFF));
                if (address > Core.CurrentROMSize)
                    throw new Exception("Invalid pointer read in the Text Array: " + address);

                List<Byte> data = new List<Byte>();
                Byte buffer = 0xFF;
                Int32 index = 0;
                while (buffer != 0x00)
                {
                    buffer = Core.ReadByte(address + index);
                    data.Add(buffer);
                    index++;
                }
                return FireEmblem.Text.BytesToText(
                    data.ToArray(),
                    View_Bytecodes.Checked,
                    TextCommands1,
                    TextCommands2,
                    PortraitList);
            }
        }
        public void Core_WriteText(String text, UInt16 entry)
        {
            UI.SuspendUpdate();
            try
            {
                Pointer address;
                Byte[] data = FireEmblem.Text.TextToBytes(
                    text,
                    TextCommands1,
                    TextCommands2,
                    PortraitList);

                if (Settings.Default.WriteToFreeSpace)
                {
                    address = Core.GetFreeSpace(data.Length);
                }
                else if (Settings.Default.PromptRepoints)
                {
                    address = Prompt.ShowPointerDialog(
                        "Please enter the address at which to insert the text.",
                        "Text Repoint");

                    if (address == new Pointer()) return;
                }
                else address = Text_PointerBox.Value;

                Core.WritePointer(this,
                    Core.GetPointer("Text Array") + entry * 4,
                    new Pointer(address + 0x80000000),
                    CurrentEntry + "repoint");

                Core.WriteData(this,
                    address,
                    data,
                    CurrentEntry + "changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while while trying to write the text.", ex);
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        public void Core_WriteFont(String path)
        {
            UI.SuspendUpdate();

            try
            {
                GBA.Bitmap bitmap = new Bitmap(path);

                if (bitmap.Colors.Count > 4)
                    throw new Exception("Image is invalid: it cannot have more than 4 colors.");
                if (bitmap.Width != 256 || bitmap.Height != 256)
                    throw new Exception("Image is invalid: it must be 256x256 pixels.");
                
                FireEmblem.Font font = new FireEmblem.Font(bitmap);

                List<Tuple<String, Pointer, Int32>> repoints = new List<Tuple<String, Pointer, Int32>>();
                List<Pointer> addresses = new List<Pointer>();
                for (Int32 i = 0; i < 256; i++)
                {
                    if (font.Glyphs[i] != null && CurrentFont.Glyphs[i] == null)
                    {
                        repoints.Add(Tuple.Create("Glyph 0x" + Util.ByteToHex((Byte)i), new Pointer(), 0x48));
                        addresses.Add(CurrentFont.Address + i * 4);
                    }
                }

                if (addresses.Count != 0)
                {
                    Boolean cancel = Prompt.ShowRepointDialog(this,
                        "Repoint Null Glyphs",
                        "Some glyphs in the font are currently set as null in the ROM.\n" +
                        "As such, a non-null pointer must be given to these glyphs.",
                        Font_ComboBox.SelectedValue + " -",
                        repoints.ToArray(),
                        addresses.ToArray());

                    if (cancel) { UI.ResumeUpdate(); return; }
                }

                CurrentFont = new FireEmblem.Font(Core.GetPointer((String)Font_ComboBox.SelectedValue));

                for (Int32 i = 0; i < 256; i++)
                {
                    if (font.Glyphs[i] != null && CurrentFont.Glyphs[i].Address != new Pointer())
                    {
                        Core.WriteData(this,
                            CurrentFont.Glyphs[i].Address,
                            font.Glyphs[i].ToBytes(),
                            Font_ComboBox.SelectedValue + " - Glyph 0x" + Util.ByteToHex((Byte)i) + " changed");
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the font image file:\n" + path, ex);
            }

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }



        private void File_SaveEntry_Click(Object sender, EventArgs e)
        {
            using (SaveFileDialog saveWindow = new SaveFileDialog())
            {
                saveWindow.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                saveWindow.FilterIndex = 1;
                saveWindow.RestoreDirectory = true;
                saveWindow.CreatePrompt = true;
                saveWindow.OverwritePrompt = true;

                if (saveWindow.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveWindow.FileName, Text_CodeBox.Text);
                }
            }
        }
        private void File_SaveScript_Click(Object sender, EventArgs e)
        {
            using (SaveFileDialog saveWindow = new SaveFileDialog())
            {
                saveWindow.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                saveWindow.FilterIndex = 1;
                saveWindow.RestoreDirectory = true;
                saveWindow.CreatePrompt = true;
                saveWindow.OverwritePrompt = true;

                if (saveWindow.ShowDialog() == DialogResult.OK)
                {
                    using (FormLoading loading = new FormLoading())
                    {
                        loading.Show();
                        loading.SetLoading("Dumping script...", 0);
                        UInt16 entry = 0;
                        String file = "";
                        while (++entry < Int16.MaxValue)
                        {
                            loading.SetPercent(100 * ((Single)entry / (Single)0x5000));
                            try
                            {
                                file += Core_GetText(entry) + "\r\n";
                            }
                            catch { break; }
                        }
                        File.WriteAllText(saveWindow.FileName, file);
                    }
                }
            }
        }
        private void File_SaveFolder_Click(Object sender, EventArgs e)
        {
            using (var folderWindow = new FolderBrowserDialog())
            {
                folderWindow.ShowNewFolderButton = true;

                if (folderWindow.ShowDialog() == DialogResult.OK &&
                    !String.IsNullOrWhiteSpace(folderWindow.SelectedPath))
                {
                    using (FormLoading loading = new FormLoading())
                    {
                        loading.Show();
                        loading.SetLoading("Dumping script...", 0);
                        Int32 amount = 0;
                        UInt16 entry = 0;
                        String name;
                        while (++entry < Int16.MaxValue)
                        {
                            loading.SetPercent(30 + 70 * ((Single)entry / (Single)Int16.MaxValue));
                            try
                            {
                                name = folderWindow.SelectedPath + "\\0x" + Util.UInt16ToHex((UInt16)entry) + ".txt";

                                File.WriteAllText(name, Core_GetText(entry));
                                amount++;
                            }
                            catch { break; }
                        }
                        UI.ShowMessage(amount + " text entry files have been created.");
                    }
                }
            }
        }
        private void File_InsertEntry_Click(Object sender, EventArgs e)
        {
            using (OpenFileDialog openWindow = new OpenFileDialog())
            {
                openWindow.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                openWindow.FilterIndex = 1;
                openWindow.RestoreDirectory = true;
                openWindow.Multiselect = false;

                if (openWindow.ShowDialog() == DialogResult.OK)
                {
                    Core_WriteText(File.ReadAllText(openWindow.FileName), CurrentIndex);
                }
            }
        }
        private void File_InsertScript_Click(Object sender, EventArgs e)
        {
            using (OpenFileDialog openWindow = new OpenFileDialog())
            {
                openWindow.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                openWindow.FilterIndex = 1;
                openWindow.RestoreDirectory = true;
                openWindow.Multiselect = false;

                if (openWindow.ShowDialog() == DialogResult.OK)
                {
                    String end_command = TextCommands1[0x00];
                    String file = File.ReadAllText(openWindow.FileName);
                    UInt16 entry = 0x0000;
                    Int32 parse = 0;
                    Int32 length = 0;
                    while (parse < file.Length)
                    {
                        if (file[parse] == '[')
                        {
                            parse++;
                            length++;
                            if (file.Substring(parse, end_command.Length) == end_command)
                            {
                                parse += end_command.Length + 1;
                                length += end_command.Length + 1;
                                Core_WriteText(file.Substring(parse - length, length), entry);
                                length = 0;
                            }
                            else if (file.Substring(parse, 4) == "0x00")
                            {
                                parse += 4 + 1;
                                length += 4 + 1;
                                Core_WriteText(file.Substring(parse - length, length), entry);
                                length = 0;
                            }
                        }
                        parse++;
                        length++;
                    }
                }
            }
        }
        private void File_InsertFolder_Click(Object sender, EventArgs e)
        {
            using (var folderWindow = new FolderBrowserDialog())
            {
                if (folderWindow.ShowDialog() == DialogResult.OK &&
                    !String.IsNullOrWhiteSpace(folderWindow.SelectedPath))
                {
                    String[] files = Directory.GetFiles(folderWindow.SelectedPath);
                    String file;
                    Int32 amount = 0;
                    UInt16 index = 0x0000;
                    foreach (String filepath in files)
                    {
                        file = Path.GetFileName(filepath);
                        if (file.StartsWith("0x"))
                        {
                            index = (UInt16)((Util.HexToByte(file.Substring(2, 2)) << 8) + Util.HexToByte(file.Substring(4, 2)));

                            Core_WriteText(File.ReadAllText(filepath), index);
                            amount++;
                        }
                    }
                    UI.ShowMessage(amount + " text entries have been written to the ROM.");
                }
            }
        }

        private void Tool_Find_Click(Object sender, EventArgs e)
        {
            if (FindWindow == null || FindWindow.IsDisposed)
            {
                FindWindow = new TextFind(this);
                FindWindow.Show();
            }
            FindWindow.Select();
        }
        private void Tool_Replace_Click(Object sender, EventArgs e)
        {
            if (ReplaceWindow == null || ReplaceWindow.IsDisposed)
            {
                ReplaceWindow = new TextReplace(this);
                ReplaceWindow.Show();
            }
            ReplaceWindow.Select();
        }

        private void View_Bytecodes_Click(Object sender, EventArgs e)
        {
            Core_Update();
        }



        private void EntryNumBox_ValueChanged(Object sender, EventArgs e)
        {
            Text_Line_NumBox.ValueChanged -= Text_Line_NumBox_ValueChanged;
            Text_Line_NumBox.Value = 0;
            Text_Line_NumBox.ValueChanged += Text_Line_NumBox_ValueChanged;

            Core_LoadText();
            Core_LoadPreview();
        }

        private void Text_ASCII_CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            if (Text_ASCII_CheckBox.Checked)
            {
                Core_WriteText(Text_CodeBox.Text, CurrentIndex);
            }
            else
            {
                Text_ASCII_CheckBox.CheckedChanged -= Text_ASCII_CheckBox_CheckedChanged;
                Text_ASCII_CheckBox.Checked = true;
                Text_ASCII_CheckBox.CheckedChanged += Text_ASCII_CheckBox_CheckedChanged;

                UI.ShowMessage("Sorry, text cannot be changed back into huffman encoded text data.");
            }
        }
        private void Text_Apply_Button_Click(Object sender, EventArgs e)
        {
            Core_WriteText(Text_CodeBox.Text, CurrentIndex);
        }
        private void Text_Cancel_Button_Click(Object sender, EventArgs e)
        {
            Core_LoadText();
            Core_LoadPreview();
        }
        private void Text_Line_NumBox_ValueChanged(Object sender, EventArgs e)
        {
            Core_LoadPreview();
        }



        private void Font_ComboBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            Core_LoadFont();
            Core_LoadPreview();
        }
        private void Font_GridBox_SelectionChanged(Object sender, EventArgs e)
        {
            Core_LoadFontValues();
        }

        private void Font_InsertButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                Core_WriteFont(openWindow.FileName);
            }
        }
        
        private void Glyph_Address_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Boolean[,] selection = Font_GridBox.Selection;
            Int32 width  = selection.GetLength(0);
            Int32 height = selection.GetLength(1);
            Byte index;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (selection[x, y])
                {
                    index = (Byte)(x + y * 16);
                    Core.WritePointer(this,
                        CurrentFont.Address + index * 4,
                        Glyph_Address_PointerBox.Value,
                        (String)Font_ComboBox.SelectedValue + " - Glyph 0x" + Util.ByteToHex(index) + " repointed");
                }
            }
            Font_GridBox.Selection = selection;
        }
        private void Glyph_Pointer_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Boolean[,] selection = Font_GridBox.Selection;
            Int32 width  = selection.GetLength(0);
            Int32 height = selection.GetLength(1);
            Byte index;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (selection[x, y])
                {
                    index = (Byte)(x + y * 16);
                    Core.WritePointer(this,
                        CurrentFont.Glyphs[index].Address,
                        Glyph_Address_PointerBox.Value,
                        (String)Font_ComboBox.SelectedValue + " - Glyph 0x" + Util.ByteToHex(index) + " Local Pointer changed");
                }
            }
            Font_GridBox.Selection = selection;
        }
        private void Glyph_Shift_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Boolean[,] selection = Font_GridBox.Selection;
            Int32 width  = selection.GetLength(0);
            Int32 height = selection.GetLength(1);
            Byte index;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (selection[x, y])
                {
                    index = (Byte)(x + y * 16);
                    Core.WriteByte(this,
                        CurrentFont.Glyphs[index].Address + 4,
                        Glyph_Shift_ByteBox.Value,
                        (String)Font_ComboBox.SelectedValue + " - Glyph 0x" + Util.ByteToHex(index) + " Unknown byte changed");
                }
            }
            Font_GridBox.Selection = selection;
        }
        private void Glyph_Width_ByteBox_ValueChanged(Object sender, EventArgs e)
        {
            Boolean[,] selection = Font_GridBox.Selection;
            Int32 width  = selection.GetLength(0);
            Int32 height = selection.GetLength(1);
            Byte index;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                if (selection[x, y])
                {
                    index = (Byte)(x + y * 16);
                    Core.WriteByte(this,
                        CurrentFont.Glyphs[index].Address + 5,
                        Glyph_Width_ByteBox.Value,
                        (String)Font_ComboBox.SelectedValue + " - Glyph 0x" + Util.ByteToHex(index) + " Width changed");
                }
            }
            Font_GridBox.Selection = selection;
        }
    }
}
