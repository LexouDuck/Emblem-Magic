using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GBA;
using KirbyMagic.Kirby;
using Magic;
using Magic.Editors;

namespace KirbyMagic.Editors
{
    public partial class TitleScreenEditor : Editor
    {
        private GBA.Bitmap _current;
        public GBA.Bitmap Current
        {
            get
            {
                return this._current;
            }
            set
            {
                this._current = value;
                this.Test_ImageBox.Load(value);
            }
        }
        private GBA.Palette _currentPalette;
        public GBA.Palette CurrentPalette
        {
            get
            {
                return this._currentPalette;
            }
            set
            {
                this._currentPalette = value;
                this.Test_PaletteBox.Load(value);
            }
        }



        public TitleScreenEditor()
        {
            this.InitializeComponent();
        }

        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            this.Core_LoadTitleScreen();
        }

        void Core_SaveImage(String filepath)
        {
            try
            {
                Core.SaveImage(filepath,
                    this.Current.Width,
                    this.Current.Height,
                    GBA.Palette.Split(this.CurrentPalette, 16),
                    delegate (Int32 x, Int32 y)
                    {
                        return (Byte)this.Current[x, y];
                    });
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save title screen image.", ex);
            }
        }
        void Core_SaveData(String filepath)
        {
            try
            {
                String path = Path.GetDirectoryName(filepath) + '\\';
                String file = Path.GetFileNameWithoutExtension(filepath);

                if (Core.App.Game is KND)
                {
                    String name_mg_palette = "Title Screen MG/FG Palette";
                    String name_mg_tileset = "Title Screen MG/FG Tileset";
                    String name_fg_tileset = "Title Screen FG Tileset";
                    String name_mg_tsa     = "Title Screen MG TSA";
                    String name_bg_tileset = "Title Screen BG Tileset";
                    String name_bg_palette = "Title Screen BG Palette";

                    Pointer address_mg_palette = Core.GetPointer(name_mg_palette);
                    Pointer address_mg_tileset = Core.GetPointer(name_mg_tileset);
                    Pointer address_fg_tileset = Core.GetPointer(name_fg_tileset);
                    Pointer address_mg_tsa     = Core.GetPointer(name_mg_tsa);
                    Pointer address_bg_tileset = Core.GetPointer(name_bg_tileset);
                    Pointer address_bg_palette = Core.GetPointer(name_bg_palette);

                    File.WriteAllBytes(path + name_mg_palette + ".pal", Core.ReadPalette(address_mg_palette, Palette.LENGTH * 8).ToBytes(false));
                    File.WriteAllBytes(path + name_mg_tileset + ".chr", new Tileset(Core.ReadData(address_mg_tileset, 0)).ToBytes(false));
                    File.WriteAllBytes(path + name_fg_tileset + ".chr", new Tileset(Core.ReadData(address_fg_tileset, 0)).ToBytes(false));
                    File.WriteAllBytes(path + name_mg_tsa     + ".tsa", Core.ReadTSA(address_mg_tsa, 32, 20, true, false).ToBytes(false, false));
                    File.WriteAllBytes(path + name_bg_tileset + ".pal", Core.ReadPalette(address_bg_palette, Palette.LENGTH).ToBytes(false));
                    File.WriteAllBytes(path + name_bg_palette + ".png", new Tileset(Core.ReadData(address_bg_tileset, 0)).ToBytes(false));
                }
                if (Core.App.Game is KAM)
                {
                    String name_bg_palette = "Title Screen BG Palette";
                    String name_bg_tileset = "Title Screen BG Tileset";
                    String name_mg_palette = "Title Screen MG Palette";
                    String name_mg_tileset = "Title Screen MG Tileset";
                    String name_mg_tsa     = "Title Screen MG TSA";
                    String name_fg_palette = "Title Screen FG Palette";
                    String name_fg_tileset = "Title Screen FG Tileset";

                    Pointer address_bg_palette = Core.GetPointer(name_bg_palette);
                    Pointer address_bg_tileset = Core.GetPointer(name_bg_tileset);
                    Pointer address_mg_palette = Core.GetPointer(name_mg_palette);
                    Pointer address_mg_tileset = Core.GetPointer(name_mg_tileset);
                    Pointer address_mg_tsa     = Core.GetPointer(name_mg_tsa);
                    Pointer address_fg_palette = Core.GetPointer(name_fg_palette);
                    Pointer address_fg_tileset = Core.GetPointer(name_fg_tileset);

                    Boolean tsa = (Core.App.Game.Region != GameRegion.JAP);

                    File.WriteAllBytes(path + name_bg_palette + ".pal", Core.ReadPalette(address_bg_palette, Palette.LENGTH).ToBytes(false));
                    File.WriteAllBytes(path + name_bg_tileset + ".chr", new Tileset(Core.ReadData(address_bg_tileset, 0)).ToBytes(false));
                    File.WriteAllBytes(path + name_mg_palette + ".pal", Core.ReadPalette(address_mg_palette, Palette.LENGTH).ToBytes(false));
                    File.WriteAllBytes(path + name_mg_tileset + ".chr", new Tileset(Core.ReadData(address_mg_tileset, 0)).ToBytes(false));
                    File.WriteAllBytes(path + name_mg_tsa     + ".tsa", Core.ReadTSA(address_mg_tsa, GBA.Screen.W_TILES, GBA.Screen.H_TILES, tsa, true).ToBytes(false, false));
                    File.WriteAllBytes(path + name_fg_palette + ".pal", Core.ReadPalette(address_fg_palette, Palette.LENGTH * 5).ToBytes(false));
                    File.WriteAllBytes(path + name_fg_tileset + ".chr", new Tileset(Core.ReadData(address_fg_tileset, 0)).ToBytes(false));
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not save title screen image data.", ex);
            }
        }

        void Core_LoadTitleScreen()
        {
            try
            {
                if (Core.App.Game is KND)
                {
                    Pointer address_mg_palette = Core.GetPointer("Title Screen MG/FG Palette");
                    Pointer address_mg_tileset = Core.GetPointer("Title Screen MG/FG Tileset");
                    Pointer address_fg_tileset = Core.GetPointer("Title Screen FG Tileset");
                    Pointer address_mg_tsa = Core.GetPointer("Title Screen MG TSA");
                    Pointer address_bg_tileset = Core.GetPointer("Title Screen BG Tileset");
                    Pointer address_bg_palette = Core.GetPointer("Title Screen BG Palette");

                    this.Core_LoadTitleScreen_FE6(
                        Core.ReadPalette(address_mg_palette, Palette.LENGTH * 8),
                        new Tileset(Core.ReadData(address_mg_tileset, 0)),
                        new Tileset(Core.ReadData(address_fg_tileset, 0)),
                        Core.ReadTSA(address_mg_tsa, 32, 20, true, false),
                        Core.ReadPalette(address_bg_palette, Palette.LENGTH),
                        new Tileset(Core.ReadData(address_bg_tileset, 0)));
                }
                if (Core.App.Game is KAM)
                {
                    Pointer address_bg_palette = Core.GetPointer("Title Screen BG Palette");
                    Pointer address_bg_tileset = Core.GetPointer("Title Screen BG Tileset");
                    Pointer address_mg_palette = Core.GetPointer("Title Screen MG Palette");
                    Pointer address_mg_tileset = Core.GetPointer("Title Screen MG Tileset");
                    Pointer address_mg_tsa = Core.GetPointer("Title Screen MG TSA");
                    Pointer address_fg_palette = Core.GetPointer("Title Screen FG Palette");
                    Pointer address_fg_tileset = Core.GetPointer("Title Screen FG Tileset");

                    Boolean tsa = (Core.App.Game.Region != GameRegion.JAP);
                    this.Core_LoadTitleScreen_FE7(
                        Core.ReadPalette(address_bg_palette, Palette.LENGTH),
                        new Tileset(Core.ReadData(address_bg_tileset, 0)),
                        Core.ReadPalette(address_mg_palette, Palette.LENGTH),
                        new Tileset(Core.ReadData(address_mg_tileset, 0)),
                        Core.ReadTSA(address_mg_tsa, GBA.Screen.W_TILES, GBA.Screen.H_TILES, tsa, true),
                        Core.ReadPalette(address_fg_palette, Palette.LENGTH * 5),
                        new Tileset(Core.ReadData(address_fg_tileset, 0)));
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to load the title screen.", ex);
            }
        }
        void Core_LoadTitleScreen_FE6(
            Palette mg_palette, Tileset mg_tileset,
            Tileset fg_tileset, TSA_Array mg_tsa,
            Palette bg_palette, Tileset bg_tileset)
        {
            GBA.Bitmap result;
            Palette palette = Palette.Empty(256);
            for (Int32 i = 0; i < mg_palette.Count; i++)
            {
                palette.Set(i, mg_palette[i]);
            }
            for (Int32 i = 0; i < bg_palette.Count; i++)
            {
                bg_palette[i] = bg_palette[i].SetAlpha(false);
                palette.Set(GBA.Palette.MAX * 15 + i, bg_palette[i]);
            }
            result = new GBA.Bitmap(GBA.Screen.WIDTH, GBA.Screen.HEIGHT);
            result.Colors = palette;

            if (this.BG_CheckBox.Checked)
            {
                GBA.Image bg = bg_tileset.ToImage(GBA.Screen.W_TILES + 2, GBA.Screen.H_TILES, bg_palette.ToBytes(false));
                for (Int32 y = 0; y < GBA.Screen.HEIGHT; y++)
                for (Int32 x = 0; x < GBA.Screen.WIDTH; x++)
                {
                    result[x, y] = GBA.Palette.MAX * 15 + bg[x, y];
                }
            }
            if (this.MG_CheckBox.Checked)
            {
                TSA_Image mg = new TSA_Image(mg_palette, mg_tileset, mg_tsa);
                for (Int32 y = 0; y < GBA.Screen.HEIGHT; y++)
                for (Int32 x = 0; x < GBA.Screen.WIDTH; x++)
                {
                    if (mg.GetColor(x, y).Value != 0)
                        result[x, y] = mg[x, y];
                }
            }
            if (this.FG_CheckBox.Checked)
            {
                Palette[] palettes = Palette.Split(mg_palette, 8);
                GBA.Image fg;
                // large japanese 'FIRE EMBLEM' title
                fg = mg_tileset.ToImage(32, 25, palettes[4].ToBytes(false));
                this.Core_DrawLayer(result, fg, new Rectangle(0, 152, GBA.Screen.WIDTH, 48), 0, 48);
                this.Core_DrawLayer(result, fg, new Rectangle(0, 104, GBA.Screen.WIDTH, 48), 0, 40);
                // small english 'FIRE EMBLEM'
                fg = fg_tileset.ToImage(32, 32, palettes[2].ToBytes(false));
                this.Core_DrawLayer(result, fg, new Rectangle(0, 0, 128, 16), 99, 27);
                // Nintendo & IS copyrights
                this.Core_DrawLayer(result, fg, new Rectangle(0, 48, 208, 8), 16, 152);
                // japanese subtitle scroll thingy
                fg.Colors = palettes[3];
                this.Core_DrawLayer(result, fg, new Rectangle(128, 16, 120, 32), 64, 85);
                // 'Press Start'
                fg.Colors = palettes[1];
                this.Core_DrawLayer(result, fg, new Rectangle(128, 0, 80, 16), 80, 120);
            }
            this.Current = result;
            this.CurrentPalette = palette;
        }
        void Core_LoadTitleScreen_FE7(
            Palette bg_palette, Tileset bg_tileset,
            Palette mg_palette, Tileset mg_tileset, TSA_Array   mg_tsa,
            Palette fg_palette, Tileset fg_tileset)
        {
            GBA.Bitmap result;
            Palette palette = Palette.Empty(256);
            for (Int32 i = 0; i < bg_palette.Count; i++)
            {
                palette.Set(GBA.Palette.MAX * 15 + i, bg_palette[i]);
            }
            for (Int32 i = 0; i < mg_palette.Count; i++)
            {
                palette.Set(GBA.Palette.MAX * 14 + i, mg_palette[i]);
            }
            for (Int32 i = 0; i < fg_palette.Count; i++)
            {
                palette.Set(i, fg_palette[i]);
            }
            result = new GBA.Bitmap(GBA.Screen.WIDTH, GBA.Screen.HEIGHT);
            result.Colors = palette;

            if (this.BG_CheckBox.Checked)
            {
                GBA.Image bg = bg_tileset.ToImage(GBA.Screen.W_TILES, GBA.Screen.H_TILES + 1, bg_palette.ToBytes(false));
                for (Int32 y = 0; y < GBA.Screen.HEIGHT; y++)
                for (Int32 x = 0; x < GBA.Screen.WIDTH; x++)
                {
                    if (x < 8 && y < 8)
                         result[x, y] = GBA.Palette.MAX * 15 + bg[x, GBA.Screen.HEIGHT + y];
                    else result[x, y] = GBA.Palette.MAX * 15 + bg[x, y];
                }
            }
            if (this.MG_CheckBox.Checked)
            {
                TSA_Image mg = new TSA_Image(mg_palette, mg_tileset, mg_tsa);
                for (Int32 y = 0; y < mg.Height; y++)
                for (Int32 x = 0; x < mg.Width; x++)
                {
                    if (mg[x, y] != 0)
                        result[x, 8 + y] = GBA.Palette.MAX * 14 + mg[x, y];
                }
            }
            if (this.FG_CheckBox.Checked)
            {
                Boolean jap = (Core.App.Game.Region == GameRegion.JAP);
                Boolean eur = (Core.App.Game.Region == GameRegion.EUR);
                Palette[] palettes = Palette.Split(fg_palette, 8);
                GBA.Image fg;
                // durandal sword
                fg = fg_tileset.ToImage(32, 32, palettes[0].ToBytes(false));
                this.Core_DrawLayer(result, fg, new Rectangle(0, 0, 192, 112), 32, 16);
                // large 'FIRE EMBLEM' title
                fg.Colors = palettes[4];
                this.Core_DrawLayer(result, fg, new Rectangle(0, GBA.Screen.HEIGHT,      GBA.Screen.WIDTH, 48), 2, jap ? 52 : 54);
                this.Core_DrawLayer(result, fg, new Rectangle(0, GBA.Screen.HEIGHT - 48, GBA.Screen.WIDTH, 48), 0, jap ? 48 : 52);
                // Nintendo & IS copyrights
                fg.Colors = palettes[2];
                this.Core_DrawLayer(result, fg, new Rectangle(0, GBA.Screen.WIDTH - 16, GBA.Screen.HEIGHT - 16, 8), eur ?   8 : 16,                GBA.Screen.HEIGHT - 16);
                this.Core_DrawLayer(result, fg, new Rectangle(0, GBA.Screen.WIDTH - 8,  GBA.Screen.HEIGHT - 64, 8), eur ? 136 : GBA.Screen.HEIGHT, GBA.Screen.HEIGHT - 16);
                // 'Press Start'
                fg.Colors = palettes[1];
                this.Core_DrawLayer(result, fg, new Rectangle(128, 208, 96, 16), jap ? 80 : 72, 120);
                if (jap)
                {   // japanese subtitle
                    fg.Colors = palettes[3];
                    this.Core_DrawLayer(result, fg, new Rectangle(144, 224, 112, 32), 64, 96);
                    // japanese 'FIRE EMBLEM' overhead
                    fg.Colors = palettes[2];
                    this.Core_DrawLayer(result, fg, new Rectangle(0, 208, 128, 16), 56, 40);
                }
            }
            this.Current = result;
            this.CurrentPalette = palette;
        }
        void Core_LoadTitleScreen_FE8(
            Palette  bg_palette, Tileset  bg_tileset, TSA_Array    bg_tsa,
            Palette  mg_palette, Tileset  mg_tileset, TSA_Array    mg_tsa,
            Palette  fg_palette, Tileset fg_tileset)
        {
            GBA.Bitmap result;
            Palette palette = Palette.Empty(256);
            for (Int32 i = 0; i < bg_palette.Count; i++)
            {
                bg_palette[i] = bg_palette[i].SetAlpha(false);
                palette.Set(GBA.Palette.MAX * 15 + i, bg_palette[i]);
            }
            for (Int32 i = 0; i < mg_palette.Count; i++)
            {
                palette.Set(GBA.Palette.MAX * 14 + i, mg_palette[i]);
            }
            for (Int32 i = 0; i < fg_palette.Count; i++)
            {
                palette.Set(i, fg_palette[i]);
            }
            result = new GBA.Bitmap(GBA.Screen.WIDTH, GBA.Screen.HEIGHT);
            result.Colors = palette;

            if (this.BG_CheckBox.Checked)
            {
                TSA_Image bg = new TSA_Image(bg_palette, bg_tileset, bg_tsa);
                for (Int32 y = 0; y < bg.Height; y++)
                for (Int32 x = 0; x < bg.Width; x++)
                {
                    if (bg[x, y] != 0)
                        result[x, y] = GBA.Palette.MAX * 15 + bg[x, y];
                }
            }
            if (this.MG_CheckBox.Checked)
            {
                TSA_Image mg = new TSA_Image(mg_palette, mg_tileset, mg_tsa);
                for (Int32 y = 0; y < mg.Height; y++)
                for (Int32 x = 0; x < mg.Width; x++)
                {
                    if (mg[x, y] != 0)
                        result[x, y] = GBA.Palette.MAX * 14 + mg[x, y];
                }
            }
            if (this.FG_CheckBox.Checked)
            {
                Palette[] palettes = Palette.Split(fg_palette, 8);
                GBA.Image fg;
                if (Core.App.Game.Region == GameRegion.JAP)
                {
                    // large 'FIRE EMBLEM' title logo
                    fg = fg_tileset.ToImage(32, 32, palettes[2].ToBytes(false));
                    this.Core_DrawLayer(result, fg, new Rectangle(0, 48, 240, 48), 0, 44); // shadow
                    this.Core_DrawLayer(result, fg, new Rectangle(0,  0, 240, 48), 0, 39); // logo
                    this.Core_DrawLayer(result, fg, new Rectangle(240, 0, 16, 16), 216, 39); // TM
                    // small 'FIRE EMBLEM' overhead
                    fg.Colors = palettes[1];
                    this.Core_DrawLayer(result, fg, new Rectangle(128, 104, 120, 16), 60, 26);
                    // 'THE SACRED STONES' subtitle/scroll thingy
                    fg.Colors = palettes[3];
                    this.Core_DrawLayer(result, fg, new Rectangle(0, 104, 128, 32), 56, 87);
                    // 'Press START'
                    fg.Colors = palettes[0];
                    this.Core_DrawLayer(result, fg, new Rectangle(128, 120, 80, 16), 80, 124);
                    // Nintendo & IS copyrights
                    fg.Colors = palettes[1];
                    this.Core_DrawLayer(result, fg, new Rectangle(0, 96, 240, 8), 16, 148);
                }
                else
                {
                    // large 'FIRE EMBLEM' title logo
                    fg = fg_tileset.ToImage(32, 32, palettes[2].ToBytes(false));
                    this.Core_DrawLayer(result, fg, new Rectangle(0, 32, 240, 32), 4, 53); // shadow
                    this.Core_DrawLayer(result, fg, new Rectangle(0,  0, 240, 32), 4, 48); // logo
                    this.Core_DrawLayer(result, fg, new Rectangle(240, 0, 16, 16), 220, 41); // TM
                    // 'THE SACRED STONES' subtitle/scroll thingy
                    fg.Colors = palettes[3];
                    this.Core_DrawLayer(result, fg, new Rectangle(0,  72, 208, 32), 16, 85);
                    // 'Press START'
                    fg.Colors = palettes[0];
                    this.Core_DrawLayer(result, fg, new Rectangle(208, 72, 48, 16), 72, 124);
                    this.Core_DrawLayer(result, fg, new Rectangle(208, 88, 48, 16), 120, 124);
                    // Nintendo & IS copyrights
                    fg.Colors = palettes[1];
                    this.Core_DrawLayer(result, fg, new Rectangle(0, 64, 240, 8), 4, 148);
                }
            }
            this.Current = result;
            this.CurrentPalette = palette;
        }

        void Core_DrawLayer(GBA.Bitmap result, GBA.Image image, Rectangle region, Int32 offsetX, Int32 offsetY)
        {
            Int32 index;
            Int32 pixel;
            for (Int32 y = 0; y < region.Height; y++)
            for (Int32 x = 0; x < region.Width; x++)
            {
                index = ((region.X + x) / 2) + ((region.Y + y) * (image.Width / 2));
                pixel = (x % 2 == 0) ?
                    (image.Bytes[index] & 0x0F) :
                    (image.Bytes[index] & 0xF0) >> 4;
                if (pixel != 0)
                    result.SetColor(offsetX + x, offsetY + y, image.Colors[pixel]);
            }
        }



        private void File_Insert_Click(Object sender, EventArgs e)
        {
            // TODO
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



        private void CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }

        private void BG_MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();

            if (Core.App.Game is KND)
            {
                editor.Core_SetEntry(GBA.Screen.W_TILES + 2, GBA.Screen.H_TILES,
                    Core.GetPointer("Title Screen BG Palette"), false,
                    Core.GetPointer("Title Screen BG Tileset"), true);
            }
            if (Core.App.Game is KAM)
            {
                editor.Core_SetEntry(GBA.Screen.W_TILES, GBA.Screen.H_TILES + 1,
                    Core.GetPointer("Title Screen BG Palette"), false,
                    Core.GetPointer("Title Screen BG Tileset"), true);
            }

            this.App.Core_OpenEditor(editor);
        }
        private void MG_MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();

            if (Core.App.Game is KND)
            {
                editor.Core_SetEntry(GBA.Screen.W_TILES + 2, GBA.Screen.H_TILES,
                    Core.GetPointer("Title Screen MG/FG Palette"), false,
                    Core.GetPointer("Title Screen MG/FG Tileset"), true,
                    Core.GetPointer("Title Screen MG TSA"), true, false);
            }
            if (Core.App.Game is KAM)
            {
                Boolean tsa = (Core.App.Game.Region != GameRegion.JAP);
                editor.Core_SetEntry(GBA.Screen.W_TILES + 2, GBA.Screen.H_TILES,
                    Core.GetPointer("Title Screen MG Palette"), false,
                    Core.GetPointer("Title Screen MG Tileset"), true,
                    Core.GetPointer("Title Screen MG TSA"), tsa, true);
            }

            this.App.Core_OpenEditor(editor);
        }
        private void FG_MagicButton_Click(Object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();

            if (Core.App.Game is KND)
            {
                editor.Core_SetEntry(32, 25,
                    Core.GetPointer("Title Screen MG/FG Palette"), false,
                    Core.GetPointer("Title Screen FG Tileset"), true);
            }
            if (Core.App.Game is KAM)
            {
                editor.Core_SetEntry(32, 32,
                    Core.GetPointer("Title Screen FG Palette"), false,
                    Core.GetPointer("Title Screen FG Tileset"), true);
            }

            this.App.Core_OpenEditor(editor);
        }
    }
}
