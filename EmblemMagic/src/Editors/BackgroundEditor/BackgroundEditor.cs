using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Compression;
using Magic;
using Magic.Editors;
using EmblemMagic.FireEmblem;
using GBA;

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
                     if (this.DialogArray_RadioButton.Checked) return this.CurrentDialog;
                else if (this.BattleArray_RadioButton.Checked) return this.CurrentBattle;
                else if (this.ScreenArray_RadioButton.Checked) return this.CurrentScreen;
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
                if (this.DialogArray_RadioButton.Checked) return BackgroundType.Dialog;
                if (this.BattleArray_RadioButton.Checked) return BackgroundType.Battle;
                if (this.ScreenArray_RadioButton.Checked) return BackgroundType.Screen;
                return BackgroundType.None;
            }
        }
        /// <summary>
        /// Gets the full name of the current entry
        /// </summary>
        String CurrentEntry
        {
            get
            {
                String prefix = (this.CurrentType == BackgroundType.Screen) ? "Cutscene Screen 0x" : this.CurrentType.ToString() + " Background 0x";
                return prefix + Util.ByteToHex(this.EntryArrayBox.Value) + " [" + this.EntryArrayBox.Text + "] - ";
            }
        }



        public BackgroundEditor()
        {
            try
            {
                this.InitializeComponent();

                this.EntryArrayBox.Load("Dialog Background List.txt");

                this.CurrentDialog = new StructFile("Dialog Background Struct.txt");
                this.CurrentBattle = new StructFile("Battle Background Struct.txt");
                this.CurrentScreen = new StructFile("Cutscene Screen Struct.txt");
                this.CurrentDialog.Address = Core.GetPointer("Dialog Background Array");
                this.CurrentBattle.Address = Core.GetPointer("Battle Background Array");
                this.CurrentScreen.Address = Core.GetPointer("Cutscene Screen Array");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            this.Current.EntryIndex = this.EntryArrayBox.Value;

            this.Core_LoadBackground();
            this.Core_LoadValues();

            if (this.CurrentType == BackgroundType.Screen && Core.App.Game is FE6)
            {
                this.TSA_Label.Enabled = false;
                this.TSA_PointerBox.Enabled = false;
                this.Tool_OpenTSAEditor.Enabled = false;
            }
            else
            {
                this.TSA_Label.Enabled = true;
                this.TSA_PointerBox.Enabled = true;
                this.Tool_OpenTSAEditor.Enabled = true;
            }
        }

        void Core_LoadBackground()
        {
            try
            {
                Size bgsize = Background.GetBGSize(this.CurrentType);

                Byte[] palette = Core.ReadData((Pointer)this.Current["Palette"],
                    (this.CurrentType == BackgroundType.Battle) ? 0 : // is compressed
                    Background.GetPaletteAmount(this.CurrentType) * GBA.Palette.LENGTH);
                Byte[] tileset;
                TSA_Array tsa;

                switch (this.CurrentType)
                {
                    case BackgroundType.Dialog:
                        tileset = Core.ReadData((Pointer)this.Current["Tileset"], 0);
                        tsa = Core.ReadTSA((Pointer)this.Current["TSA"], bgsize.Width, bgsize.Height, false, true);
                        break;

                    case BackgroundType.Battle:
                        palette[0] = 0x00; palette[1] = 0x00;
                        // force 1st color black on battle BGs
                        tileset = Core.ReadData((Pointer)this.Current["Tileset"], 0);
                        tsa = Core.ReadTSA((Pointer)this.Current["TSA"], bgsize.Width, bgsize.Height, true, false);
                        break;

                    case BackgroundType.Screen:
                        if (Core.App.Game is FE6)
                        {
                            tileset = Core.ReadData((Pointer)this.Current["Tileset"], 0);
                            tsa = TSA_Array.GetBasicTSA(bgsize.Width, bgsize.Height);
                        }
                        else if (Core.App.Game is FE7 && Core.ReadByte(this.Current.GetAddress(this.Current.EntryIndex)) == 0x00)
                        {
                            tileset = Core.ReadData((Pointer)this.Current["Tileset"], 0);
                            tsa = Core.ReadTSA((Pointer)this.Current["TSA"], bgsize.Width, bgsize.Height, false, true);
                        }
                        else // its stored in 32x2 strips
                        {
                            tsa = Core.ReadTSA((Pointer)this.Current["TSA"], bgsize.Width, bgsize.Height, false, true);
                            Int32 amount = 10;
                            Int32 length = 32 * 2 * Tile.LENGTH;
                            tileset = new Byte[amount * length];
                            Pointer table = (Pointer)this.Current["Tileset"];
                            Pointer address;
                            Byte[] buffer;
                            for (Int32 i = 0; i < amount; i++)
                            {
                                address = Core.ReadPointer(table + i * 4);
                                buffer = Core.ReadData(address, 0);
                                Array.Copy(buffer, 0, tileset, i * length, length);
                            }   // assemble BGs that are stored in 32x2 strips
                        }
                        break;

                    default: throw new Exception("Invalid background type.");
                }
                this.CurrentBackground = new Background(palette, tileset, tsa);

                this.Background_ImageBox.Load(this.CurrentBackground);
                this.Background_PaletteBox.Load(Palette.Merge(this.CurrentBackground.Palettes));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the background image from the ROM.", ex);

                this.Background_ImageBox.Reset();
                this.Background_PaletteBox.Reset();
            }
        }
        void Core_LoadValues()
        {
            this.Palette_PointerBox.ValueChanged -= this.Palette_PointerBox_ValueChanged;
            this.Tileset_PointerBox.ValueChanged -= this.Tileset_PointerBox_ValueChanged;
            this.TSA_PointerBox.ValueChanged -= this.TSA_PointerBox_ValueChanged;

            try
            {
                this.Palette_PointerBox.Value = (Pointer)this.Current["Palette"];
                this.Tileset_PointerBox.Value = (Pointer)this.Current["Tileset"];
                if (!(Core.App.Game is FE6 && this.CurrentType == BackgroundType.Screen))
                    this.TSA_PointerBox.Value = (Pointer)this.Current["TSA"];
                else this.TSA_PointerBox.Value = new Pointer();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the values.", ex);

                this.Palette_PointerBox.Value = new Pointer();
                this.Tileset_PointerBox.Value = new Pointer();
                this.TSA_PointerBox.Value = new Pointer();
            }

            this.Palette_PointerBox.ValueChanged += this.Palette_PointerBox_ValueChanged;
            this.Tileset_PointerBox.ValueChanged += this.Tileset_PointerBox_ValueChanged;
            this.TSA_PointerBox.ValueChanged += this.TSA_PointerBox_ValueChanged;
        }

        void Core_Insert(Background insert)
        {
            UI.SuspendUpdate();
            try
            {
                Boolean compressed = (this.CurrentType == BackgroundType.Battle);

                Byte[] data_palette = Palette.Merge(insert.Palettes).ToBytes(compressed);
                Byte[] data_tileset = insert.Graphics.ToBytes(false); // is compressed below
                Byte[] data_tsa     = insert.Tiling.ToBytes(compressed, !compressed);
                
                var repoints = new List<Tuple<String, Pointer, Int32>>();
                var writepos = new List<Pointer>();

                List<Byte[]> strips = new List<Byte[]>();
                if (this.CurrentType == BackgroundType.Screen
                    && (Core.App.Game is FE8
                    || (Core.App.Game is FE7 &&
                        Core.App.Game.Region != GameRegion.JAP &&
                        (Byte)this.Current["Strips"] == 0x01)))
                {
                    Byte[] buffer = new Byte[32 * 2 * GBA.Tile.LENGTH];
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Array.Copy(data_tileset, i * buffer.Length, buffer, 0, Math.Min(data_tileset.Length - i * buffer.Length, buffer.Length));
                        strips.Add(LZ77.Compress(buffer));

                        repoints.Add(Tuple.Create("Tileset " + i, Core.ReadPointer((Pointer)this.Current["Tileset"] + i * 4), strips[i].Length));
                        writepos.Add(this.Current.GetAddress(this.Current.EntryIndex, "Tileset") + i * 4);
                    }
                }
                else
                {
                    data_tileset = LZ77.Compress(data_tileset);

                    repoints.Add(Tuple.Create("Tileset", (Pointer)this.Current["Tileset"], data_tileset.Length));
                    writepos.Add(this.Current.GetAddress(this.Current.EntryIndex, "Tileset"));
                }
                repoints.Add(Tuple.Create("Palette", (Pointer)this.Current["Palette"], data_palette.Length));
                writepos.Add(this.Current.GetAddress(this.Current.EntryIndex, "Palette"));

                repoints.Add(Tuple.Create("TSA", (Pointer)this.Current["TSA"], data_tsa.Length));
                writepos.Add(this.Current.GetAddress(this.Current.EntryIndex, "TSA"));

                Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Background",
                    "The background to insert may need some of its parts to be repointed.",
                    this.CurrentEntry, repoints.ToArray(), writepos.ToArray());
                if (cancel) return;

                if (this.CurrentType == BackgroundType.Screen && (Core.App.Game is FE8
                    || (Core.App.Game is FE7 && Core.ReadByte(this.Current.Address) == 0x01)))
                {
                    for (Int32 i = 0; i < 10; i++)
                    {
                        Core.WriteData(this,
                            Core.ReadPointer((Pointer)this.Current["Tileset"] + i * 4),
                            strips[i],
                            this.CurrentEntry + "Tileset " + i + " changed");
                    }
                }   // write graphics in 32x2 strips
                else
                {
                    Core.WriteData(this,
                        (Pointer)this.Current["Tileset"],
                        data_tileset,
                        this.CurrentEntry + "Tileset changed");
                }

                Core.WriteData(this,
                    (Pointer)this.Current["Palette"],
                    data_palette,
                    this.CurrentEntry + "Palette changed");

                Core.WriteData(this,
                    (Pointer)this.Current["TSA"],
                    data_tsa,
                    this.CurrentEntry + "TSA changed");
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the background.", ex);
            }
            UI.ResumeUpdate();
            UI.PerformUpdate();
        }

        void Core_InsertImage(String filepath)
        {
            Background background;
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                if (palette == null)
                {
                    background = new Background(
                        this.CurrentBackground.Tiling.Width,
                        this.CurrentBackground.Tiling.Height,
                        new GBA.Bitmap(filepath),
                        Background.GetPaletteAmount(this.CurrentType),
                        (this.CurrentType != BackgroundType.Screen));
                }
                else
                {
                    background = new Background(
                        this.CurrentBackground.Tiling.Width,
                        this.CurrentBackground.Tiling.Height,
                        new GBA.Bitmap(filepath),
                        palette,
                        Background.GetPaletteAmount(this.CurrentType),
                        (this.CurrentType != BackgroundType.Screen));
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load image.", ex);
                return;
            }
            this.Core_Insert(background);
        }
        void Core_InsertData(String filepath)
        {
            Background background;
            try
            {
                String path = Path.GetDirectoryName(filepath) + '\\';
                String file = Path.GetFileNameWithoutExtension(filepath);

                if (!File.Exists(path + file + ".pal"))
                    throw new Exception("Could not find Palette file:\n" + path + file + ".pal");
                if (!File.Exists(path + file + ".chr"))
                    throw new Exception("Could not find Tileset file:\n" + path + file + ".chr");
                if (!File.Exists(path + file + ".tsa"))
                    throw new Exception("Could not find TSA Array file:\n" + path + file + ".tsa");

                Byte[] palette = File.ReadAllBytes(path + file + ".pal");
                Byte[] tileset = File.ReadAllBytes(path + file + ".chr");
                TSA_Array tsa = new TSA_Array(
                        this.CurrentBackground.Tiling.Width,
                        this.CurrentBackground.Tiling.Height,
                        File.ReadAllBytes(path + file + ".tsa"));

                background = new Background(palette, tileset, tsa);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load image data.", ex);
                return;
            }
            this.Core_Insert(background);
        }
        void Core_SaveImage(String filepath)
        {
            try
            {
                Core.SaveImage(filepath,
                    this.CurrentBackground.Width,
                    this.CurrentBackground.Height,
                    this.CurrentBackground.Palettes,
                    delegate (Int32 x, Int32 y)
                    {
                        return (Byte)this.CurrentBackground[x, y];
                    });
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save background image.", ex);
            }
        }
        void Core_SaveData(String filepath)
        {
            try
            {
                String path = Path.GetDirectoryName(filepath) + '\\';
                String file = Path.GetFileNameWithoutExtension(filepath);

                Byte[] data_palette = Palette.Merge(this.CurrentBackground.Palettes).ToBytes(false);
                Byte[] data_tileset = this.CurrentBackground.Graphics.ToBytes(false);
                Byte[] data_tsa = this.CurrentBackground.Tiling.ToBytes(false, false);

                File.WriteAllBytes(path + file + ".pal", data_palette);
                File.WriteAllBytes(path + file + ".chr", data_tileset);
                File.WriteAllBytes(path + file + ".tsa", data_tsa);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save background image data.", ex);
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
                "TSA Image Data (.tsa + .pal + .chr)|*.tsa;*.pal;*.chr|" +
                "All files (*.*)|*.*";

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                if (openWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_InsertImage(openWindow.FileName);
                    return;
                }
                if (openWindow.FileName.EndsWith(".pal", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".chr", StringComparison.OrdinalIgnoreCase) ||
                    openWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_InsertData(openWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + openWindow.FileName);
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
                "TSA Image Data (.tsa + .pal + .chr)|*.tsa;*.pal;*.chr|" +
                "All files (*.*)|*.*";

            if (saveWindow.ShowDialog() == DialogResult.OK)
            {
                if (saveWindow.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_SaveImage(saveWindow.FileName.Remove(saveWindow.FileName.Length - 4));
                    return;
                }
                if (saveWindow.FileName.EndsWith(".tsa", StringComparison.OrdinalIgnoreCase))
                {
                    this.Core_SaveData(saveWindow.FileName);
                    return;
                }
                UI.ShowError("File chosen has invalid extension.\r\n" + saveWindow.FileName);
            }
        }

        private void Tool_OpenPaletteEditor_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry,
                (Pointer)this.Current["Palette"],
                (this.CurrentType == BackgroundType.Battle) ? 0 : Background.GetPaletteAmount(this.CurrentType));
        }
        private void Tool_OpenTSAEditor_Click(Object sender, EventArgs e)
        {
            Boolean compressed = (this.CurrentType == BackgroundType.Battle);

            UI.OpenTSAEditor(this,
                this.CurrentEntry,
                (Pointer)this.Current["Palette"], (this.CurrentType == BackgroundType.Battle) ?
                    0 : Background.GetPaletteAmount(this.CurrentType) * GBA.Palette.LENGTH,
                (Pointer)this.Current["Tileset"], (this.CurrentType == BackgroundType.Screen && (Core.App.Game is FE8 ||
                    (Core.App.Game is FE7 && Core.ReadByte(this.Current.GetAddress(this.Current.EntryIndex)) == 0x01))) ?
                    -1 : 0, // -1 if tileset is split into 32x2 strips
                (Pointer)this.Current["TSA"],
                this.CurrentBackground.Tiling.Width,
                this.CurrentBackground.Tiling.Height,
                compressed, !compressed);
        }



        private void EntryArrayBox_ValueChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void Array_RadioButton_CheckedChanged(Object sender, EventArgs e)
        {
            this.EntryArrayBox.ValueChanged -= this.EntryArrayBox_ValueChanged;
            this.EntryArrayBox.Value = 0;
            this.EntryArrayBox.ValueChanged += this.EntryArrayBox_ValueChanged;
            
            if (this.DialogArray_RadioButton.Checked) this.EntryArrayBox.Load("Dialog Background List.txt");
            if (this.BattleArray_RadioButton.Checked) this.EntryArrayBox.Load("Battle Background List.txt");
            if (this.ScreenArray_RadioButton.Checked) this.EntryArrayBox.Load("Cutscene Screen List.txt");

            this.Core_Update();
        }

        private void Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Palette"),
                this.Palette_PointerBox.Value,
                this.CurrentEntry + "palette repoint: " + this.Palette_PointerBox.Value);
        }
        private void Tileset_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "Tileset"),
                this.Tileset_PointerBox.Value,
                this.CurrentEntry + "image repoint: " + this.Tileset_PointerBox.Value);
        }
        private void TSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                this.Current.GetAddress(this.Current.EntryIndex, "TSA"),
                this.TSA_PointerBox.Value,
                this.CurrentEntry + "TSA repoint: " + this.TSA_PointerBox.Value);
        }

        private void MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();

            Size bgsize = Background.GetBGSize(this.CurrentType);
            switch (this.CurrentType)
            {
                case BackgroundType.Dialog:
                    editor.Core_SetEntry(bgsize.Width, bgsize.Height,
                        (Pointer)this.Current["Palette"], false,
                        (Pointer)this.Current["Tileset"], true,
                        (Pointer)this.Current["TSA"], false, true);
                    break;

                case BackgroundType.Battle:
                    editor.Core_SetEntry(bgsize.Width, bgsize.Height,
                        (Pointer)this.Current["Palette"], true,
                        (Pointer)this.Current["Tileset"], true,
                        (Pointer)this.Current["TSA"], true, false);
                    break;

                case BackgroundType.Screen:
                    if (Core.App.Game is FE6)
                    {
                        editor.Core_SetEntry(bgsize.Width, bgsize.Height,
                            (Pointer)this.Current["Palette"], false,
                            (Pointer)this.Current["Tileset"], true);
                    }
                    else if (Core.App.Game is FE7 && Core.ReadByte(this.Current.GetAddress(this.Current.EntryIndex)) == 0x00)
                    {
                        editor.Core_SetEntry(bgsize.Width, bgsize.Height,
                            (Pointer)this.Current["Palette"], false,
                            (Pointer)this.Current["Tileset"], true,
                            (Pointer)this.Current["TSA"], false, true);
                    }
                    else // its stored in 32x2 strips
                    {
                        editor.Core_SetEntry(bgsize.Width, bgsize.Height,
                            (Pointer)this.Current["Palette"], false,
                            (Pointer)this.Current["Tileset"], true,
                            (Pointer)this.Current["TSA"], false, true);
                    }
                    break;

                default: throw new Exception("Invalid background type.");
            }
            Program.Core.Core_OpenEditor(editor);
        }
    }
}
