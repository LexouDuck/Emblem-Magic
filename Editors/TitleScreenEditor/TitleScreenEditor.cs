using EmblemMagic.FireEmblem;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class TitleScreenEditor : Editor
    {
        public TitleScreenEditor()
        {
            InitializeComponent();
        }

        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Core_LoadTitleScreen();
        }

        void Core_LoadTitleScreen()
        {
            try
            {
                if (Core.CurrentROM is FE6)
                {
                    Pointer address_mg_palette = Core.GetPointer("Title Screen MG/FG Palette");
                    Pointer address_mg_tileset = Core.GetPointer("Title Screen MG/FG Tileset");
                    Pointer address_fg_tileset = Core.GetPointer("Title Screen FG Tileset");
                    Pointer address_mg_tsa = Core.GetPointer("Title Screen MG TSA");
                    Pointer address_bg_tileset = Core.GetPointer("Title Screen BG Tileset");
                    Pointer address_bg_palette = Core.GetPointer("Title Screen BG Palette");

                    Core_LoadTitleScreen_FE6(
                        Core.ReadPalette(address_mg_palette, Palette.LENGTH * 8),
                        new Tileset(Core.ReadData(address_mg_tileset, 0)),
                        new Tileset(Core.ReadData(address_fg_tileset, 0)),
                        Core.ReadTSA(address_mg_tsa, 32, 20, true, false),
                        Core.ReadPalette(address_bg_palette, Palette.LENGTH),
                        new Tileset(Core.ReadData(address_bg_tileset, 0)));
                }
                if (Core.CurrentROM is FE7)
                {
                    Pointer address_bg_palette = Core.GetPointer("Title Screen BG Palette");
                    Pointer address_bg_tileset = Core.GetPointer("Title Screen BG Tileset");
                    Pointer address_mg_palette = Core.GetPointer("Title Screen MG Palette");
                    Pointer address_mg_tileset = Core.GetPointer("Title Screen MG Tileset");
                    Pointer address_mg_tsa = Core.GetPointer("Title Screen MG TSA");
                    Pointer address_fg_palette = Core.GetPointer("Title Screen FG Palette");
                    Pointer address_fg_tileset = Core.GetPointer("Title Screen FG Tileset");

                    bool tsa = (Core.CurrentROM.Version != GameVersion.JAP);
                    Core_LoadTitleScreen_FE7(
                        Core.ReadPalette(address_bg_palette, Palette.LENGTH),
                        new Tileset(Core.ReadData(address_bg_tileset, 0)),
                        Core.ReadPalette(address_mg_palette, Palette.LENGTH),
                        new Tileset(Core.ReadData(address_mg_tileset, 0)),
                        Core.ReadTSA(address_mg_tsa, 30, 20, tsa, true),
                        Core.ReadPalette(address_fg_palette, Palette.LENGTH * 5),
                        new Tileset(Core.ReadData(address_fg_tileset, 0)));
                }
                if (Core.CurrentROM is FE8)
                {
                    Pointer address_bg_palette = Core.GetPointer("Title Screen BG Palette");
                    Pointer address_bg_tileset1 = Core.GetPointer("Title Screen BG Tileset 1");
                    Pointer address_bg_tileset2 = Core.GetPointer("Title Screen BG Tileset 2");
                    Pointer address_bg_tsa = Core.GetPointer("Title Screen BG TSA");
                    Pointer address_mg_palette = Core.GetPointer("Title Screen MG Palette");
                    Pointer address_mg_tileset = Core.GetPointer("Title Screen MG Tileset");
                    Pointer address_mg_tsa = Core.GetPointer("Title Screen MG TSA");
                    Pointer address_fg_palette = Core.GetPointer("Title Screen FG Palette");
                    Pointer address_fg_tileset1 = Core.GetPointer("Title Screen FG Tileset 1");
                    Pointer address_fg_tileset2 = Core.GetPointer("Title Screen FG Tileset 2");

                    Tileset bg_tileset;
                    bg_tileset = new Tileset(Core.ReadData(address_bg_tileset1, 0));
                    bg_tileset.AddTileset(new Tileset(Core.ReadData(address_bg_tileset2, 0)));
                    Tileset fg_tileset;
                    fg_tileset = new Tileset(Core.ReadData(address_fg_tileset1, 0));
                    fg_tileset.AddTileset(new Tileset(Core.ReadData(address_fg_tileset2, 0)));

                    Core_LoadTitleScreen_FE8(
                        Core.ReadPalette(address_bg_palette, Palette.LENGTH), bg_tileset,
                        Core.ReadTSA(address_bg_tsa, 32, 32, true, false),
                        Core.ReadPalette(address_mg_palette, Palette.LENGTH),
                        new Tileset(Core.ReadData(address_mg_tileset, 0)),
                        Core.ReadTSA(address_mg_tsa, 32, 32, true, false),
                        Core.ReadPalette(address_fg_palette, Palette.LENGTH * 5), fg_tileset);
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while trying to load the title screen.", ex);
            }
        }

        void Core_LoadTitleScreen_FE6(
            Palette mg_palette, Tileset mg_tileset,
            Tileset fg_tileset, TSA_Array mg_tsa,
            Palette bg_palette, Tileset bg_tileset)
        {
            GBA.Bitmap result;
            Palette palette = Palette.Empty(256);
            for (int i = 0; i < mg_palette.Count; i++)
            {
                palette.Set(i, mg_palette[i]);
            }
            for (int i = 0; i < bg_palette.Count; i++)
            {
                bg_palette[i] = bg_palette[i].SetAlpha(false);
                palette.Set(240 + i, bg_palette[i]);
            }
            result = new GBA.Bitmap(GBA.Screen.WIDTH, GBA.Screen.HEIGHT);
            result.Colors = palette;

            if (BG_CheckBox.Checked)
            {
                GBA.Image bg = bg_tileset.ToImage(32, 20, bg_palette.ToBytes(false));
                for (int y = 0; y < GBA.Screen.HEIGHT; y++)
                for (int x = 0; x < GBA.Screen.WIDTH; x++)
                {
                    result[x, y] = bg[x, y];
                }
            }
            if (MG_CheckBox.Checked)
            {
                TSA_Image mg = new TSA_Image(mg_palette, mg_tileset, mg_tsa);
                for (int y = 0; y < GBA.Screen.HEIGHT; y++)
                for (int x = 0; x < GBA.Screen.WIDTH; x++)
                {
                    if (mg[x, y] != new GBA.Color())
                        result[x, y] = mg[x, y];
                }
            }
            if (FG_CheckBox.Checked)
            {
                Palette[] palettes = Palette.Split(mg_palette, 8);
                GBA.Image fg;
                // large japanese 'FIRE EMBLEM' title
                fg = mg_tileset.ToImage(32, 25, palettes[4].ToBytes(false));
                Core_DrawLayer(result, fg, new Rectangle(0, 152, 240, 48), 0, 48);
                Core_DrawLayer(result, fg, new Rectangle(0, 104, 240, 48), 0, 40);
                // small english 'FIRE EMBLEM'
                fg = fg_tileset.ToImage(32, 32, palettes[2].ToBytes(false));
                Core_DrawLayer(result, fg, new Rectangle(0, 0, 128, 16), 99, 27);
                // Nintendo & IS copyrights
                Core_DrawLayer(result, fg, new Rectangle(0, 48, 208, 8), 16, 152);
                // japanese subtitle scroll thingy
                fg.Colors = palettes[3];
                Core_DrawLayer(result, fg, new Rectangle(128, 16, 120, 32), 64, 85);
                // 'Press Start'
                fg.Colors = palettes[1];
                Core_DrawLayer(result, fg, new Rectangle(128, 0, 80, 16), 80, 120);
            }

            Test_ImageBox.Load(result);
            Test_PaletteBox.Load(palette);
        }
        void Core_LoadTitleScreen_FE7(
            Palette bg_palette, Tileset bg_tileset,
            Palette mg_palette, Tileset mg_tileset, TSA_Array   mg_tsa,
            Palette fg_palette, Tileset fg_tileset)
        {
            GBA.Bitmap result;
            Palette palette = Palette.Empty(256);
            for (int i = 0; i < bg_palette.Count; i++)
            {
                palette.Set(240 + i, bg_palette[i]);
            }
            for (int i = 0; i < mg_palette.Count; i++)
            {
                palette.Set(224 + i, mg_palette[i]);
            }
            for (int i = 0; i < fg_palette.Count; i++)
            {
                palette.Set(i, fg_palette[i]);
            }
            result = new GBA.Bitmap(GBA.Screen.WIDTH, GBA.Screen.HEIGHT);
            result.Colors = palette;

            if (BG_CheckBox.Checked)
            {
                GBA.Image bg = bg_tileset.ToImage(30, 21, bg_palette.ToBytes(false));
                for (int y = 0; y < GBA.Screen.HEIGHT; y++)
                for (int x = 0; x < GBA.Screen.WIDTH; x++)
                {
                    if (x < 8 && y < 8)
                         result[x, y] = bg[x, 160 + y];
                    else result[x, y] = bg[x, y];
                }
            }
            if (MG_CheckBox.Checked)
            {
                TSA_Image mg = new TSA_Image(mg_palette, mg_tileset, mg_tsa);
                for (int y = 0; y < mg.Height; y++)
                for (int x = 0; x < mg.Width; x++)
                {
                    if (mg[x, y] != mg.Palettes[0][0])
                        result[x, 8 + y] = mg[x, y];
                }
            }
            if (FG_CheckBox.Checked)
            {
                bool jap = (Core.CurrentROM.Version == GameVersion.JAP);
                bool eur = (Core.CurrentROM.Version == GameVersion.EUR);
                Palette[] palettes = Palette.Split(fg_palette, 8);
                GBA.Image fg;
                // durandal sword
                fg = fg_tileset.ToImage(32, 32, palettes[0].ToBytes(false));
                Core_DrawLayer(result, fg, new Rectangle(0, 0, 192, 112), 32, 16);
                // large 'FIRE EMBLEM' title
                fg.Colors = palettes[4];
                Core_DrawLayer(result, fg, new Rectangle(0, 160, 240, 48), 2, jap ? 52 : 54);
                Core_DrawLayer(result, fg, new Rectangle(0, 112, 240, 48), 0, jap ? 48 : 52);
                // Nintendo & IS copyrights
                fg.Colors = palettes[2];
                Core_DrawLayer(result, fg, new Rectangle(0, 224, 144, 8), eur ?   8 :  16, 144);
                Core_DrawLayer(result, fg, new Rectangle(0, 232,  96, 8), eur ? 136 : 160, 144);
                // 'Press Start'
                fg.Colors = palettes[1];
                Core_DrawLayer(result, fg, new Rectangle(128, 208, 96, 16), jap ? 80 : 72, 120);
                if (jap)
                {   // japanese subtitle
                    fg.Colors = palettes[3];
                    Core_DrawLayer(result, fg, new Rectangle(144, 224, 112, 32), 64, 96);
                    // japanese 'FIRE EMBLEM' overhead
                    fg.Colors = palettes[2];
                    Core_DrawLayer(result, fg, new Rectangle(0, 208, 128, 16), 56, 40);
                }
            }
            Test_ImageBox.Load(result);
            Test_PaletteBox.Load(palette);
        }
        void Core_LoadTitleScreen_FE8(
            Palette  bg_palette, Tileset  bg_tileset, TSA_Array    bg_tsa,
            Palette  mg_palette, Tileset  mg_tileset, TSA_Array    mg_tsa,
            Palette  fg_palette, Tileset fg_tileset)
        {
            GBA.Bitmap result;
            Palette palette = Palette.Empty(256);
            for (int i = 0; i < bg_palette.Count; i++)
            {
                bg_palette[i] = bg_palette[i].SetAlpha(false);
                palette.Set(240 + i, bg_palette[i]);
            }
            for (int i = 0; i < mg_palette.Count; i++)
            {
                palette.Set(224 + i, mg_palette[i]);
            }
            for (int i = 0; i < fg_palette.Count; i++)
            {
                palette.Set(i, fg_palette[i]);
            }
            result = new GBA.Bitmap(GBA.Screen.WIDTH, GBA.Screen.HEIGHT);
            result.Colors = palette;

            if (BG_CheckBox.Checked)
            {
                TSA_Image bg = new TSA_Image(bg_palette, bg_tileset, bg_tsa);
                for (int y = 0; y < bg.Height; y++)
                for (int x = 0; x < bg.Width; x++)
                {
                    if (bg[x, y] != bg.Palettes[0][0])
                        result[x, y] = bg[x, y];
                }
            }
            if (MG_CheckBox.Checked)
            {
                TSA_Image mg = new TSA_Image(mg_palette, mg_tileset, mg_tsa);
                for (int y = 0; y < mg.Height; y++)
                for (int x = 0; x < mg.Width; x++)
                {
                    if (mg[x, y] != mg.Palettes[0][0])
                        result[x, y] = mg[x, y];
                }
            }
            if (FG_CheckBox.Checked)
            {
                Palette[] palettes = Palette.Split(fg_palette, 8);
                GBA.Image fg;
                if (Core.CurrentROM.Version == GameVersion.JAP)
                {
                    // large 'FIRE EMBLEM' title logo
                    fg = fg_tileset.ToImage(32, 32, palettes[2].ToBytes(false));
                    Core_DrawLayer(result, fg, new Rectangle(0, 48, 240, 48), 0, 44); // shadow
                    Core_DrawLayer(result, fg, new Rectangle(0,  0, 240, 48), 0, 39); // logo
                    Core_DrawLayer(result, fg, new Rectangle(240, 0, 16, 16), 216, 39); // TM
                    // small 'FIRE EMBLEM' overhead
                    fg.Colors = palettes[1];
                    Core_DrawLayer(result, fg, new Rectangle(128, 104, 120, 16), 60, 26);
                    // 'THE SACRED STONES' subtitle/scroll thingy
                    fg.Colors = palettes[3];
                    Core_DrawLayer(result, fg, new Rectangle(0, 104, 128, 32), 56, 87);
                    // 'Press START'
                    fg.Colors = palettes[0];
                    Core_DrawLayer(result, fg, new Rectangle(128, 120, 80, 16), 80, 124);
                    // Nintendo & IS copyrights
                    fg.Colors = palettes[1];
                    Core_DrawLayer(result, fg, new Rectangle(0, 96, 240, 8), 16, 148);
                }
                else
                {
                    // large 'FIRE EMBLEM' title logo
                    fg = fg_tileset.ToImage(32, 32, palettes[2].ToBytes(false));
                    Core_DrawLayer(result, fg, new Rectangle(0, 32, 240, 32), 4, 53); // shadow
                    Core_DrawLayer(result, fg, new Rectangle(0,  0, 240, 32), 4, 48); // logo
                    Core_DrawLayer(result, fg, new Rectangle(240, 0, 16, 16), 220, 41); // TM
                    // 'THE SACRED STONES' subtitle/scroll thingy
                    fg.Colors = palettes[3];
                    Core_DrawLayer(result, fg, new Rectangle(0,  72, 208, 32), 16, 85);
                    // 'Press START'
                    fg.Colors = palettes[0];
                    Core_DrawLayer(result, fg, new Rectangle(208, 72, 48, 16), 72, 124);
                    Core_DrawLayer(result, fg, new Rectangle(208, 88, 48, 16), 120, 124);
                    // Nintendo & IS copyrights
                    fg.Colors = palettes[1];
                    Core_DrawLayer(result, fg, new Rectangle(0, 64, 240, 8), 4, 148);
                }
            }

            Test_ImageBox.Load(result);
            Test_PaletteBox.Load(palette);
        }

        void Core_DrawLayer(GBA.Bitmap result, GBA.Image image, Rectangle region, int offsetX, int offsetY)
        {
            int index;
            int pixel;
            for (int y = 0; y < region.Height; y++)
            for (int x = 0; x < region.Width; x++)
            {
                index = ((region.X + x) / 2) + ((region.Y + y) * (image.Width / 2));
                pixel = (x % 2 == 0) ?
                    (image.Bytes[index] & 0x0F) :
                    (image.Bytes[index] & 0xF0) >> 4;
                if (pixel != 0)
                    result[offsetX + x, offsetY + y] = image.Colors[pixel];
            }
        }



        private void CheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }

        private void BG_MagicButton_Click(object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();
            Program.Core.Core_OpenEditor(editor);

            if (Core.CurrentROM is FE6)
            {
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen BG Palette"), false,
                    Core.GetPointer("Title Screen BG Tileset"), true);
            }
            if (Core.CurrentROM is FE7)
            {
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen BG Palette"), false,
                    Core.GetPointer("Title Screen BG Tileset"), true);
            }
            if (Core.CurrentROM is FE8)
            {
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen BG Palette"), false,
                    Core.GetPointer("Title Screen BG Tileset 1"), true,
                    Core.GetPointer("Title Screen BG TSA"), true, false);
            }
        }
        private void MG_MagicButton_Click(object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();
            Program.Core.Core_OpenEditor(editor);

            if (Core.CurrentROM is FE6)
            {
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen MG/FG Palette"), false,
                    Core.GetPointer("Title Screen MG/FG Tileset"), true,
                    Core.GetPointer("Title Screen MG TSA"), true, false);
            }
            if (Core.CurrentROM is FE7)
            {
                bool tsa = (Core.CurrentROM.Version != GameVersion.JAP);
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen MG Palette"), false,
                    Core.GetPointer("Title Screen MG Tileset"), true,
                    Core.GetPointer("Title Screen MG TSA"), tsa, true);
            }
            if (Core.CurrentROM is FE8)
            {
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen MG Palette"), false,
                    Core.GetPointer("Title Screen MG Tileset"), true,
                    Core.GetPointer("Title Screen MG TSA"), true, false);
            }
        }
        private void FG_MagicButton_Click(object sender, EventArgs e)
        {
            GraphicsEditor editor = new GraphicsEditor();
            Program.Core.Core_OpenEditor(editor);

            if (Core.CurrentROM is FE6)
            {
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen MG/FG Palette"), false,
                    Core.GetPointer("Title Screen FG Tileset"), true);
            }
            if (Core.CurrentROM is FE7)
            {
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen FG Palette"), false,
                    Core.GetPointer("Title Screen FG Tileset"), true);
            }
            if (Core.CurrentROM is FE8)
            {
                editor.Core_SetEntry(
                    Core.GetPointer("Title Screen FG Palette"), false,
                    Core.GetPointer("Title Screen FG Tileset 1"), true);
            }
        }
    }
}
