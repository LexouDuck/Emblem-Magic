using System;
using EmblemMagic.FireEmblem;
using GBA;
using System.Windows.Forms;
using Compression;
using Magic;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class WorldMapEditor_FE7 : Editor
    {
        const Int32 SMALLMAP_TILESET = 0;
        const Int32 SMALLMAP_PALETTE = 1;
        const Int32 SMALLMAP_TSA     = 2;
        const Int32 LARGEMAP_TILESET = 3;
        const Int32 LARGEMAP_PALETTE = 4;
        const Int32 LARGEMAP_TSA     = 5;

        public WorldMap_FE7_Small CurrentSmallMap { get; set; }
        public WorldMap_FE7_Large CurrentLargeMap { get; set; }

        String CurrentEntry(Boolean small)
        {
            return (small ? "Small" : "Large") + " World Map - ";
        }
        public Int32 CurrentLargeTSA { get { return (Int32)LargeMap_TSA_NumBox.Value; } }



        public WorldMapEditor_FE7(IApp app) : base(app)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        override public void Core_OnOpen()
        {
            Core_Update();
        }
        override public void Core_Update()
        {
            Pointer[] pointers = new Pointer[6]
            {
                Core.GetPointer("Small World Map Tileset"),
                Core.GetPointer("Small World Map Palette"),
                Core.GetPointer("Small World Map TSA"),
                Core.GetPointer("Large World Map Tileset"),
                Core.GetPointer("Large World Map Palette"),
                Core.GetPointer("Large World Map TSA"),
            };

            try
            {
                Core_LoadValues(pointers);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly load world map pointers.", ex);
            }

            try
            {
                CurrentSmallMap = new WorldMap_FE7_Small(
                    pointers[SMALLMAP_PALETTE],
                    pointers[SMALLMAP_TILESET],
                    pointers[SMALLMAP_TSA]);
                SmallMap_ImageBox.Load(CurrentSmallMap);
                SmallMap_PaletteBox.Load(Palette.Merge(CurrentSmallMap.Palettes));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the small World Map.", ex);
                SmallMap_ImageBox.Reset();
                SmallMap_PaletteBox.Reset();
            }

            try
            {
                CurrentLargeMap = new WorldMap_FE7_Large(
                    pointers[LARGEMAP_PALETTE],
                    pointers[LARGEMAP_TILESET],
                    pointers[LARGEMAP_TSA]);
                LargeMap_ImageBox.Load(CurrentLargeMap);
                LargeMap_PaletteBox.Load(Palette.Merge(CurrentLargeMap.Palettes));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the large World Map.", ex);
                LargeMap_ImageBox.Reset();
                LargeMap_PaletteBox.Reset();
            }
        }

        void Core_LoadValues(Pointer[] pointers)
        {
            SmallMap_Palette_PointerBox.ValueChanged -= SmallMap_PalettePointerBox_ValueChanged;
            SmallMap_Tileset_PointerBox.ValueChanged -= SmallMap_TilesetPointerBox_ValueChanged;
            SmallMap_TSA_PointerBox.ValueChanged -= SmallMap_TSAPointerBox_ValueChanged;

            LargeMap_Palette_PointerBox.ValueChanged -= LargeMap_Palette_PointerBox_ValueChanged;
            LargeMap_Tileset_PointerBox.ValueChanged -= LargeMap_Tileset_PointerBox_ValueChanged;
            LargeMap_TSA_PointerBox.ValueChanged -= LargeMap_TSA_PointerBox_ValueChanged;



            SmallMap_Palette_PointerBox.Value = pointers[SMALLMAP_PALETTE];
            SmallMap_Tileset_PointerBox.Value = pointers[SMALLMAP_TILESET];
            SmallMap_TSA_PointerBox.Value     = pointers[SMALLMAP_TSA];
            LargeMap_Palette_PointerBox.Value = pointers[LARGEMAP_PALETTE];
            LargeMap_Tileset_PointerBox.Value = pointers[LARGEMAP_TILESET];
            LargeMap_TSA_PointerBox.Value     = pointers[LARGEMAP_TSA];



            SmallMap_Palette_PointerBox.ValueChanged += SmallMap_PalettePointerBox_ValueChanged;
            SmallMap_Tileset_PointerBox.ValueChanged += SmallMap_TilesetPointerBox_ValueChanged;
            SmallMap_TSA_PointerBox.ValueChanged += SmallMap_TSAPointerBox_ValueChanged;

            LargeMap_Palette_PointerBox.ValueChanged += LargeMap_Palette_PointerBox_ValueChanged;
            LargeMap_Tileset_PointerBox.ValueChanged += LargeMap_Tileset_PointerBox_ValueChanged;
            LargeMap_TSA_PointerBox.ValueChanged += LargeMap_TSA_PointerBox_ValueChanged;
        }
        void Core_InsertSmallMap(String filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                CurrentSmallMap = new WorldMap_FE7_Small(filepath, palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the Small World Map image.", ex); return;
            }

            Byte[] data_palette = Palette.Merge(CurrentSmallMap.Palettes).ToBytes(false);
            Byte[] data_tileset = CurrentSmallMap.Graphics.ToBytes(true);
            Byte[] data_tsa = CurrentSmallMap.Tiling.ToBytes(false, true);

            UI.SuspendUpdate();

            Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Small World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                CurrentEntry(true), new Tuple<String, Pointer, Int32>[] {
                    Tuple.Create("Palette", Core.GetPointer("Small World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Small World Map Tileset"), data_tileset.Length),
                    Tuple.Create("TSA", Core.GetPointer("Small World Map TSA"), data_tsa.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Small World Map Palette"),
                data_palette,
                CurrentEntry(true) + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map Tileset"),
                data_tileset,
                CurrentEntry(true) + "Tileset changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map TSA"),
                data_tsa,
                CurrentEntry(true) + "TSA changed");

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        void Core_InsertLargeMap(String filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                CurrentLargeMap = new WorldMap_FE7_Large(filepath, palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the Large World Map image.", ex); return;
            }

            UI.SuspendUpdate();

            for (Int32 i = 0; i < 12; i++)
            {
                Core.WriteData(this,
                    Core.ReadPointer(LargeMap_Tileset_PointerBox.Value + i * 4),
                    CurrentLargeMap.Graphics[i].ToBytes(false),
                    CurrentEntry(false) + "Graphics " + i + " changed");

                Core.WriteData(this,
                    Core.ReadPointer(LargeMap_TSA_PointerBox.Value + i * 4),
                    CurrentLargeMap.TSA_Sections[i].ToBytes(false, true),
                    CurrentEntry(false) + "TSA " + i + " changed");
            }

            Core.WriteData(this,
                LargeMap_Palette_PointerBox.Value,
                Palette.Merge(CurrentLargeMap.Palettes).ToBytes(false),
                CurrentEntry(false) + "Palette changed");

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }



        private void SmallMap_InsertButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter =
                "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "TSA Image Data (.tsa + .pal + .chr)|*.tsa|" +
                "All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                Core_InsertSmallMap(openWindow.FileName);
            }
        }

        private void SmallMap_PalettePointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Palette"),
                SmallMap_Palette_PointerBox.Value,
                CurrentEntry(true) + "Palette repoint");
        }
        private void SmallMap_TilesetPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Tileset"),
                SmallMap_Tileset_PointerBox.Value,
                CurrentEntry(true) + "Tileset repoint");
        }
        private void SmallMap_TSAPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map TSA"),
                SmallMap_TSA_PointerBox.Value,
                CurrentEntry(true) + "TSA repoint");
        }

        private void SmallMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                CurrentEntry(true),
                SmallMap_Palette_PointerBox.Value, 4);
        }
        private void SmallMap_TSA_Button_Click(Object sender, EventArgs e)
        {
            UI.OpenTSAEditor(this,
                CurrentEntry(true),
                SmallMap_Palette_PointerBox.Value, WorldMap_FE7_Small.PALETTES * Palette.LENGTH,
                SmallMap_Tileset_PointerBox.Value, 0,
                SmallMap_TSA_PointerBox.Value,
                WorldMap_FE7_Small.WIDTH,
                WorldMap_FE7_Small.HEIGHT,
                false, true);
        }



        private void LargeMap_InsertButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter =
                "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "TSA Image Data (.tsa + .pal + .chr)|*.tsa|" +
                "All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                Core_InsertLargeMap(openWindow.FileName);
            }
        }

        private void LargeMap_Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map Palette"),
                LargeMap_Palette_PointerBox.Value,
                CurrentEntry(false) + "Palette repoint");
        }
        private void LargeMap_Tileset_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map Tileset"),
                LargeMap_Tileset_PointerBox.Value,
                CurrentEntry(false) + "Tileset repoint");
        }
        private void LargeMap_TSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map TSA"),
                LargeMap_TSA_PointerBox.Value,
                CurrentEntry(false) + "TSA repoint");
        }

        private void LargeMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                CurrentEntry(false),
                LargeMap_Palette_PointerBox.Value, 4);
        }
        private void LargeMap_TSA_Button_Click(Object sender, EventArgs e)
        {
            UI.OpenTSAEditor(this,
                CurrentEntry(false),
                LargeMap_Palette_PointerBox.Value, WorldMap_FE7_Large.PALETTES * Palette.LENGTH,
                Core.ReadPointer(LargeMap_Tileset_PointerBox.Value + CurrentLargeTSA * 4), 32 * 32 * Tile.LENGTH,
                Core.ReadPointer(LargeMap_TSA_PointerBox.Value + CurrentLargeTSA * 4),
                32, 32,
                false, true);
        }
    }
}
