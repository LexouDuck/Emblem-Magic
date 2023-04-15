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
        public Int32 CurrentLargeTSA { get { return (Int32)this.LargeMap_TSA_NumBox.Value; } }



        public WorldMapEditor_FE7()
        {
            try
            {
                this.InitializeComponent();
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }

        override public void Core_OnOpen()
        {
            this.Core_Update();
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
                this.Core_LoadValues(pointers);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly load world map pointers.", ex);
            }

            try
            {
                this.CurrentSmallMap = new WorldMap_FE7_Small(
                    pointers[SMALLMAP_PALETTE],
                    pointers[SMALLMAP_TILESET],
                    pointers[SMALLMAP_TSA]);
                this.SmallMap_ImageBox.Load(this.CurrentSmallMap);
                this.SmallMap_PaletteBox.Load(Palette.Merge(this.CurrentSmallMap.Palettes));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the small World Map.", ex);
                this.SmallMap_ImageBox.Reset();
                this.SmallMap_PaletteBox.Reset();
            }

            try
            {
                this.CurrentLargeMap = new WorldMap_FE7_Large(
                    pointers[LARGEMAP_PALETTE],
                    pointers[LARGEMAP_TILESET],
                    pointers[LARGEMAP_TSA]);
                this.LargeMap_ImageBox.Load(this.CurrentLargeMap);
                this.LargeMap_PaletteBox.Load(Palette.Merge(this.CurrentLargeMap.Palettes));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the large World Map.", ex);
                this.LargeMap_ImageBox.Reset();
                this.LargeMap_PaletteBox.Reset();
            }
        }

        void Core_LoadValues(Pointer[] pointers)
        {
            this.SmallMap_Palette_PointerBox.ValueChanged -= this.SmallMap_PalettePointerBox_ValueChanged;
            this.SmallMap_Tileset_PointerBox.ValueChanged -= this.SmallMap_TilesetPointerBox_ValueChanged;
            this.SmallMap_TSA_PointerBox.ValueChanged -= this.SmallMap_TSAPointerBox_ValueChanged;

            this.LargeMap_Palette_PointerBox.ValueChanged -= this.LargeMap_Palette_PointerBox_ValueChanged;
            this.LargeMap_Tileset_PointerBox.ValueChanged -= this.LargeMap_Tileset_PointerBox_ValueChanged;
            this.LargeMap_TSA_PointerBox.ValueChanged -= this.LargeMap_TSA_PointerBox_ValueChanged;



            this.SmallMap_Palette_PointerBox.Value = pointers[SMALLMAP_PALETTE];
            this.SmallMap_Tileset_PointerBox.Value = pointers[SMALLMAP_TILESET];
            this.SmallMap_TSA_PointerBox.Value     = pointers[SMALLMAP_TSA];
            this.LargeMap_Palette_PointerBox.Value = pointers[LARGEMAP_PALETTE];
            this.LargeMap_Tileset_PointerBox.Value = pointers[LARGEMAP_TILESET];
            this.LargeMap_TSA_PointerBox.Value     = pointers[LARGEMAP_TSA];



            this.SmallMap_Palette_PointerBox.ValueChanged += this.SmallMap_PalettePointerBox_ValueChanged;
            this.SmallMap_Tileset_PointerBox.ValueChanged += this.SmallMap_TilesetPointerBox_ValueChanged;
            this.SmallMap_TSA_PointerBox.ValueChanged += this.SmallMap_TSAPointerBox_ValueChanged;

            this.LargeMap_Palette_PointerBox.ValueChanged += this.LargeMap_Palette_PointerBox_ValueChanged;
            this.LargeMap_Tileset_PointerBox.ValueChanged += this.LargeMap_Tileset_PointerBox_ValueChanged;
            this.LargeMap_TSA_PointerBox.ValueChanged += this.LargeMap_TSA_PointerBox_ValueChanged;
        }
        void Core_InsertSmallMap(String filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                this.CurrentSmallMap = new WorldMap_FE7_Small(filepath, palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the Small World Map image.", ex); return;
            }

            Byte[] data_palette = Palette.Merge(this.CurrentSmallMap.Palettes).ToBytes(false);
            Byte[] data_tileset = this.CurrentSmallMap.Graphics.ToBytes(true);
            Byte[] data_tsa = this.CurrentSmallMap.Tiling.ToBytes(false, true);

            UI.SuspendUpdate();

            Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Small World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                this.CurrentEntry(true), new Tuple<String, Pointer, Int32>[] {
                    Tuple.Create("Palette", Core.GetPointer("Small World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Small World Map Tileset"), data_tileset.Length),
                    Tuple.Create("TSA", Core.GetPointer("Small World Map TSA"), data_tsa.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Small World Map Palette"),
                data_palette,
                this.CurrentEntry(true) + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map Tileset"),
                data_tileset,
                this.CurrentEntry(true) + "Tileset changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map TSA"),
                data_tsa,
                this.CurrentEntry(true) + "TSA changed");

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        void Core_InsertLargeMap(String filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                this.CurrentLargeMap = new WorldMap_FE7_Large(filepath, palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the Large World Map image.", ex); return;
            }

            UI.SuspendUpdate();

            for (Int32 i = 0; i < 12; i++)
            {
                Core.WriteData(this,
                    Core.ReadPointer(this.LargeMap_Tileset_PointerBox.Value + i * 4),
                    this.CurrentLargeMap.Graphics[i].ToBytes(false),
                    this.CurrentEntry(false) + "Graphics " + i + " changed");

                Core.WriteData(this,
                    Core.ReadPointer(this.LargeMap_TSA_PointerBox.Value + i * 4),
                    this.CurrentLargeMap.TSA_Sections[i].ToBytes(false, true),
                    this.CurrentEntry(false) + "TSA " + i + " changed");
            }

            Core.WriteData(this,
                this.LargeMap_Palette_PointerBox.Value,
                Palette.Merge(this.CurrentLargeMap.Palettes).ToBytes(false),
                this.CurrentEntry(false) + "Palette changed");

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
                this.Core_InsertSmallMap(openWindow.FileName);
            }
        }

        private void SmallMap_PalettePointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Palette"),
                this.SmallMap_Palette_PointerBox.Value,
                this.CurrentEntry(true) + "Palette repoint");
        }
        private void SmallMap_TilesetPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Tileset"),
                this.SmallMap_Tileset_PointerBox.Value,
                this.CurrentEntry(true) + "Tileset repoint");
        }
        private void SmallMap_TSAPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map TSA"),
                this.SmallMap_TSA_PointerBox.Value,
                this.CurrentEntry(true) + "TSA repoint");
        }

        private void SmallMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry(true),
                this.SmallMap_Palette_PointerBox.Value, 4);
        }
        private void SmallMap_TSA_Button_Click(Object sender, EventArgs e)
        {
            UI.OpenTSAEditor(this,
                this.CurrentEntry(true),
                this.SmallMap_Palette_PointerBox.Value, WorldMap_FE7_Small.PALETTES * Palette.LENGTH,
                this.SmallMap_Tileset_PointerBox.Value, 0,
                this.SmallMap_TSA_PointerBox.Value,
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
                this.Core_InsertLargeMap(openWindow.FileName);
            }
        }

        private void LargeMap_Palette_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map Palette"),
                this.LargeMap_Palette_PointerBox.Value,
                this.CurrentEntry(false) + "Palette repoint");
        }
        private void LargeMap_Tileset_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map Tileset"),
                this.LargeMap_Tileset_PointerBox.Value,
                this.CurrentEntry(false) + "Tileset repoint");
        }
        private void LargeMap_TSA_PointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map TSA"),
                this.LargeMap_TSA_PointerBox.Value,
                this.CurrentEntry(false) + "TSA repoint");
        }

        private void LargeMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry(false),
                this.LargeMap_Palette_PointerBox.Value, 4);
        }
        private void LargeMap_TSA_Button_Click(Object sender, EventArgs e)
        {
            UI.OpenTSAEditor(this,
                this.CurrentEntry(false),
                this.LargeMap_Palette_PointerBox.Value, WorldMap_FE7_Large.PALETTES * Palette.LENGTH,
                Core.ReadPointer(this.LargeMap_Tileset_PointerBox.Value + this.CurrentLargeTSA * 4), 32 * 32 * Tile.LENGTH,
                Core.ReadPointer(this.LargeMap_TSA_PointerBox.Value + this.CurrentLargeTSA * 4),
                32, 32,
                false, true);
        }
    }
}
