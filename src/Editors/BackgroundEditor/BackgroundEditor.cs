using Compression;
using EmblemMagic.FireEmblem;
using EmblemMagic.Properties;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class BackgroundEditor : Editor
    {
        /// <summary>
        /// The background currently displayed in the editor
        /// </summary>
        Background CurrentBackground;
        /// <summary>
        /// The array entry that is currently being viewed
        /// </summary>
        StructFile Current
        {
            get
            {
                     if (DialogArray_RadioButton.Checked) return CurrentDialog;
                else if (BattleArray_RadioButton.Checked) return CurrentBattle;
                else if (ScreenArray_RadioButton.Checked) return CurrentScreen;
                else return null;
            }
        }
        public StructFile CurrentDialog;
        public StructFile CurrentBattle;
        public StructFile CurrentScreen;
        /// <summary>
        /// The current type of background to be edited: scene/dialog BGs, battle BGs, or cutscene screens
        /// </summary>
        BackgroundType CurrentType
        {
            get
            {
                if (DialogArray_RadioButton.Checked) return BackgroundType.Dialog;
                if (BattleArray_RadioButton.Checked) return BackgroundType.Battle;
                if (ScreenArray_RadioButton.Checked) return BackgroundType.Screen;
                return BackgroundType.None;
            }
        }
        /// <summary>
        /// Gets the full name of the current entry
        /// </summary>
        string CurrentEntry
        {
            get
            {
                string prefix = (CurrentType == BackgroundType.Screen) ? "Cutscene Screen 0x" : CurrentType.ToString() + " Background 0x";
                return prefix + Util.ByteToHex(EntryArrayBox.Value) + " [" + EntryArrayBox.Text + "] - ";
            }
        }



        public BackgroundEditor()
        {
            try
            {
                InitializeComponent();
                
                EntryArrayBox.Load("Dialog Background List.txt");

                CurrentDialog = new StructFile("Dialog Background Struct.txt");
                CurrentBattle = new StructFile("Battle Background Struct.txt");
                CurrentScreen = new StructFile("Cutscene Screen Struct.txt");
                CurrentDialog.Address = Core.GetPointer("Dialog Background Array");
                CurrentBattle.Address = Core.GetPointer("Battle Background Array");
                CurrentScreen.Address = Core.GetPointer("Cutscene Screen Array");
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Current.EntryIndex = EntryArrayBox.Value;

            Core_LoadBackground();
            Core_LoadValues();

            if (CurrentType == BackgroundType.Screen && Core.CurrentROM is FE6)
            {
                TSA_Label.Enabled = false;
                TSA_PointerBox.Enabled = false;
                Tool_OpenTSAEditor.Enabled = false;
            }
            else
            {
                TSA_Label.Enabled = true;
                TSA_PointerBox.Enabled = true;
                Tool_OpenTSAEditor.Enabled = true;
            }
        }

        void Core_LoadBackground()
        {
            try
            {
                Size bgsize = Background.GetBGSize(CurrentType);

                byte[] palette = Core.ReadData((Pointer)Current["Palette"],
                    (CurrentType == BackgroundType.Battle) ? 0 : // is compressed
                    Background.GetPaletteAmount(CurrentType) * GBA.Palette.LENGTH);
                byte[] tileset;
                TSA_Array tsa;

                switch (CurrentType)
                {
                    case BackgroundType.Dialog:
                        tileset = Core.ReadData((Pointer)Current["Tileset"], 0);
                        tsa = Core.ReadTSA((Pointer)Current["TSA"], bgsize.Width, bgsize.Height, false, true);
                        break;

                    case BackgroundType.Battle:
                        palette[0] = 0x00; palette[1] = 0x00;
                        // force 1st color black on battle BGs
                        tileset = Core.ReadData((Pointer)Current["Tileset"], 0);
                        tsa = Core.ReadTSA((Pointer)Current["TSA"], bgsize.Width, bgsize.Height, true, false);
                        break;

                    case BackgroundType.Screen:
                        if (Core.CurrentROM is FE6)
                        {
                            tileset = Core.ReadData((Pointer)Current["Tileset"], 0);
                            tsa = TSA_Array.GetBasicTSA(bgsize.Width, bgsize.Height);
                        }
                        else if (Core.CurrentROM is FE7 && Core.ReadByte(Current.GetAddress(Current.EntryIndex)) == 0x00)
                        {
                            tileset = Core.ReadData((Pointer)Current["Tileset"], 0);
                            tsa = Core.ReadTSA((Pointer)Current["TSA"], bgsize.Width, bgsize.Height, false, true);
                        }
                        else // its stored in 32x2 strips
                        {
                            tsa = Core.ReadTSA((Pointer)Current["TSA"], bgsize.Width, bgsize.Height, false, true);
                            int amount = 10;
                            int length = 32 * 2 * Tile.LENGTH;
                            tileset = new byte[amount * length];
                            Pointer table = (Pointer)Current["Tileset"];
                            Pointer address;
                            byte[] buffer;
                            for (int i = 0; i < amount; i++)
                            {
                                address = Core.ReadPointer(table + i * 4);
                                buffer = Core.ReadData(address, 0);
                                Array.Copy(buffer, 0, tileset, i * length, length);
                            }   // assemble BGs that are stored in 32x2 strips
                        }
                        break;

                    default: throw new Exception("Invalid background type.");
                }
                CurrentBackground = new Background(palette, tileset, tsa);

                Background_ImageBox.Load(CurrentBackground);
                Background_PaletteBox.Load(Palette.Merge(CurrentBackground.Palettes));
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the background image from the ROM.", ex);

                Background_ImageBox.Reset();
                Background_PaletteBox.Reset();
            }
        }
        void Core_LoadValues()
        {
            Palette_PointerBox.ValueChanged -= Palette_PointerBox_ValueChanged;
            Tileset_PointerBox.ValueChanged -= Tileset_PointerBox_ValueChanged;
            TSA_PointerBox.ValueChanged -= TSA_PointerBox_ValueChanged;

            try
            {
                Palette_PointerBox.Value = (Pointer)Current["Palette"];
                Tileset_PointerBox.Value = (Pointer)Current["Tileset"];
                if (!(Core.CurrentROM is FE6 && CurrentType == BackgroundType.Screen))
                    TSA_PointerBox.Value = (Pointer)Current["TSA"];
                else TSA_PointerBox.Value = new Pointer();
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the values.", ex);

                Palette_PointerBox.Value = new Pointer();
                Tileset_PointerBox.Value = new Pointer();
                TSA_PointerBox.Value = new Pointer();
            }

            Palette_PointerBox.ValueChanged += Palette_PointerBox_ValueChanged;
            Tileset_PointerBox.ValueChanged += Tileset_PointerBox_ValueChanged;
            TSA_PointerBox.ValueChanged += TSA_PointerBox_ValueChanged;
        }

        void Core_Insert(Background insert)
        {
            Core.SuspendUpdate();
            try
            {
                bool compressed = (CurrentType == BackgroundType.Battle);

                byte[] data_palette = Palette.Merge(insert.Palettes).ToBytes(compressed);
                byte[] data_tileset = insert.Graphics.ToBytes(false); // is compressed below
                byte[] data_tsa     = insert.Tiling.ToBytes(compressed, !compressed);
                
                var repoints = new List<Tuple<string, Pointer, int>>();
                var writepos = new List<Pointer>();

                List<byte[]> strips = new List<byte[]>();
                if (CurrentType == BackgroundType.Screen
                    && (Core.CurrentROM is FE8
                    || (Core.CurrentROM is FE7 &&
                        Core.CurrentROM.Version != GameVersion.JAP &&
                        (byte)Current["Strips"] == 0x01)))
                {
                    byte[] buffer = new byte[32 * 2 * GBA.Tile.LENGTH];
                    for (int i = 0; i < 10; i++)
                    {
                        Array.Copy(data_tileset, i * buffer.Length, buffer, 0, Math.Min(data_tileset.Length - i * buffer.Length, buffer.Length));
                        strips.Add(LZ77.Compress(buffer));

                        repoints.Add(Tuple.Create("Tileset " + i, Core.ReadPointer((Pointer)Current["Tileset"] + i * 4), strips[i].Length));
                        writepos.Add(Current.GetAddress(Current.EntryIndex, "Tileset") + i * 4);
                    }
                }
                else
                {
                    data_tileset = LZ77.Compress(data_tileset);

                    repoints.Add(Tuple.Create("Tileset", (Pointer)Current["Tileset"], data_tileset.Length));
                    writepos.Add(Current.GetAddress(Current.EntryIndex, "Tileset"));
                }
                repoints.Add(Tuple.Create("Palette", (Pointer)Current["Palette"], data_palette.Length));
                writepos.Add(Current.GetAddress(Current.EntryIndex, "Palette"));

                repoints.Add(Tuple.Create("TSA", (Pointer)Current["TSA"], data_tsa.Length));
                writepos.Add(Current.GetAddress(Current.EntryIndex, "TSA"));

                bool cancel = Prompt.ShowRepointDialog(this, "Repoint Background",
                    "The background to insert may need some of its parts to be repointed.",
                    CurrentEntry, repoints.ToArray(), writepos.ToArray());
                if (cancel) return;

                if (CurrentType == BackgroundType.Screen && (Core.CurrentROM is FE8
                    || (Core.CurrentROM is FE7 && Core.ReadByte(Current.Address) == 0x01)))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Core.WriteData(this,
                            Core.ReadPointer((Pointer)Current["Tileset"] + i * 4),
                            strips[i],
                            CurrentEntry + "Tileset " + i + " changed");
                    }
                }   // write graphics in 32x2 strips
                else
                {
                    Core.WriteData(this,
                        (Pointer)Current["Tileset"],
                        data_tileset,
                        CurrentEntry + "Tileset changed");
                }

                Core.WriteData(this,
                    (Pointer)Current["Palette"],
                    data_palette,
                    CurrentEntry + "Palette changed");

                Core.WriteData(this,
                    (Pointer)Current["TSA"],
                    data_tsa,
                    CurrentEntry + "TSA changed");
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the background.", ex);
            }
            Core.ResumeUpdate();
            Core.PerformUpdate();
        }

        void Core_InsertImage(string filepath)
        {
            Background background;
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                if (palette == null)
                {
                    background = new Background(
                        CurrentBackground.Tiling.Width,
                        CurrentBackground.Tiling.Height,
                        new GBA.Bitmap(filepath),
                        Background.GetPaletteAmount(CurrentType),
                        (CurrentType != BackgroundType.Screen));
                }
                else
                {
                    background = new Background(
                        CurrentBackground.Tiling.Width,
                        CurrentBackground.Tiling.Height,
                        new GBA.Bitmap(filepath),
                        palette,
                        Background.GetPaletteAmount(CurrentType),
                        (CurrentType != BackgroundType.Screen));
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load image.", ex);
                return;
            }
            Core_Insert(background);
        }
        void Core_InsertData(string filepath)
        {
            Background background;
            try
            {
                string path = Path.GetDirectoryName(filepath) + '\\';
                string file = Path.GetFileNameWithoutExtension(filepath);

                if (!File.Exists(path + file + ".pal"))
                    throw new Exception("Could not find Palette file:\n" + path + file + ".pal");
                if (!File.Exists(path + file + ".dmp"))
                    throw new Exception("Could not find Tileset file:\n" + path + file + ".dmp");
                if (!File.Exists(path + file + ".tsa"))
                    throw new Exception("Could not find TSA Array file:\n" + path + file + ".tsa");

                byte[] palette = File.ReadAllBytes(path + file + ".pal");
                byte[] tileset = File.ReadAllBytes(path + file + ".dmp");
                TSA_Array tsa = new TSA_Array(
                        CurrentBackground.Tiling.Width,
                        CurrentBackground.Tiling.Height,
                        File.ReadAllBytes(path + file + ".tsa"));

                background = new Background(palette, tileset, tsa);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load image data.", ex);
                return;
            }
            Core_Insert(background);
        }
        void Core_SaveImage(string filepath)
        {
            try
            {
                Core.SaveImage(filepath,
                    CurrentBackground.Width,
                    CurrentBackground.Height,
                    CurrentBackground.Palettes,
                    delegate (int x, int y)
                    {
                        return (byte)CurrentBackground[x, y];
                    });
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save background image.", ex);
            }
        }
        void Core_SaveData(string filepath)
        {
            try
            {
                string path = Path.GetDirectoryName(filepath) + '\\';
                string file = Path.GetFileNameWithoutExtension(filepath);

                byte[] data_palette = Palette.Merge(CurrentBackground.Palettes).ToBytes(false);
                byte[] data_tileset = CurrentBackground.Graphics.ToBytes(false);
                byte[] data_tsa = CurrentBackground.Tiling.ToBytes(false, false);

                File.WriteAllBytes(path + file + ".pal", data_palette);
                File.WriteAllBytes(path + file + ".dmp", data_tileset);
                File.WriteAllBytes(path + file + ".tsa", data_tsa);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not save background image data.", ex);
            }
        }



        private void File_Insert_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;
            openWindow.FilterIndex = 1;
            openWindow.Filter =
                "Image file (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "TSA Image Data (.tsa + .pal + .dmp)|*.tsa;*.pal;*.dmp|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    Core_InsertImage(openWindow.FileName);
                    return;
                }
                if (openWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".dmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    Core_InsertData(openWindow.FileName);
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
            }
        }
        private void File_Save_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.RestoreDirectory = true;
            saveWindow.OverwritePrompt = true;
            saveWindow.CreatePrompt = false;
            saveWindow.FilterIndex = 1;
            saveWindow.Filter =
                "Image file (*.png)|*.png|" +
                "TSA Image Data (.tsa + .pal + .dmp)|*.tsa;*.pal;*.dmp|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    Core_SaveData(saveWindow.FileName);
                    return;
                }
                Program.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }

        private void Tool_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                CurrentEntry,
                (Pointer)Current["Palette"],
                (CurrentType == BackgroundType.Battle) ? 0 : Background.GetPaletteAmount(CurrentType));
        }
        private void Tool_OpenTSAEditor_Click(Object sender, EventArgs e)
        {
            bool compressed = (CurrentType == BackgroundType.Battle);

            Core.OpenTSAEditor(this,
                CurrentEntry,
                (Pointer)Current["Palette"], (CurrentType == BackgroundType.Battle) ?
                    0 : Background.GetPaletteAmount(CurrentType) * GBA.Palette.LENGTH,
                (Pointer)Current["Tileset"], (CurrentType == BackgroundType.Screen && (Core.CurrentROM is FE8 ||
                    (Core.CurrentROM is FE7 && Core.ReadByte(Current.GetAddress(Current.EntryIndex)) == 0x01))) ?
                    -1 : 0, // -1 if tileset is split into 32x2 strips
                (Pointer)Current["TSA"],
                CurrentBackground.Tiling.Width,
                CurrentBackground.Tiling.Height,
                compressed, !compressed);
        }



        private void EntryArrayBox_ValueChanged(object sender, EventArgs e)
        {
            Core_Update();
        }

        private void Array_RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            EntryArrayBox.ValueChanged -= EntryArrayBox_ValueChanged;
            EntryArrayBox.Value = 0;
            EntryArrayBox.ValueChanged += EntryArrayBox_ValueChanged;
            
            if (DialogArray_RadioButton.Checked) EntryArrayBox.Load("Dialog Background List.txt");
            if (BattleArray_RadioButton.Checked) EntryArrayBox.Load("Battle Background List.txt");
            if (ScreenArray_RadioButton.Checked) EntryArrayBox.Load("Cutscene Screen List.txt");

            Core_Update();
        }

        private void Palette_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Palette"),
                Palette_PointerBox.Value,
                CurrentEntry + "palette repoint: " + Palette_PointerBox.Value);
        }
        private void Tileset_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "Tileset"),
                Tileset_PointerBox.Value,
                CurrentEntry + "image repoint: " + Tileset_PointerBox.Value);
        }
        private void TSA_PointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Current.GetAddress(Current.EntryIndex, "TSA"),
                TSA_PointerBox.Value,
                CurrentEntry + "TSA repoint: " + TSA_PointerBox.Value);
        }
    }
}
