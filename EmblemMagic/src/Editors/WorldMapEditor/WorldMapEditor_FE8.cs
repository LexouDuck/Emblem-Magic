using System;
using EmblemMagic.FireEmblem;
using GBA;
using System.Windows.Forms;
using Compression;
using Magic;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class WorldMapEditor_FE8 : Editor
    {
        const Int32 MINI_MAP_TILESET = 0;
        const Int32 MINI_MAP_PALETTE = 1;
        const Int32 SMALLMAP_TILESET = 2;
        const Int32 SMALLMAP_PALETTE = 3;
        const Int32 SMALLMAP_TSA     = 4;
        const Int32 LARGEMAP_TILESET = 5;
        const Int32 LARGEMAP_PALETTE = 6;
        const Int32 LARGEMAP_TSA     = 7;

        WorldMap_FE8_Mini CurrentMiniMap { get; set; }
        public WorldMap_FE8_Small CurrentSmallMap { get; set; }
        public WorldMap_FE8_Large CurrentLargeMap { get; set; }

        String CurrentEntry(String prefix)
        {
            if (prefix.Equals("Mini") && Core.App.Game.Region == GameRegion.EUR)
                return prefix + " World Map " + this.Mini_Map_NumberBox.Value + " - ";
            else return prefix + " World Map - ";
        }



        public WorldMapEditor_FE8()
        {
            try
            {
                this.InitializeComponent();

                if (Core.App.Game.Region == GameRegion.EUR)
                    this.Mini_Map_NumberBox.Enabled = true;
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
            Pointer[] pointers = new Pointer[8]
            {
                Core.GetPointer("Mini World Map Tileset"),
                Core.GetPointer("Mini World Map Palette"),
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
                this.CurrentMiniMap = new WorldMap_FE8_Mini(
                    pointers[MINI_MAP_PALETTE],
                    (Core.App.Game.Region == GameRegion.EUR) ?
                    Core.ReadPointer(pointers[MINI_MAP_TILESET] + (Int32)this.Mini_Map_NumberBox.Value * 4) :
                    pointers[MINI_MAP_TILESET]);
                this.Mini_Map_ImageBox.Load(this.CurrentMiniMap);
                this.Mini_Map_PaletteBox.Load(this.CurrentMiniMap.Colors);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the mini World Map.", ex);
                this.Mini_Map_ImageBox.Reset();
                this.Mini_Map_PaletteBox.Reset();
            }

            try
            {
                this.CurrentSmallMap = new WorldMap_FE8_Small(
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
                this.CurrentLargeMap = new WorldMap_FE8_Large(
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
            this.Mini_Map_PalettePointerBox.ValueChanged -= this.Mini_Map_PalettePointerBox_ValueChanged;
            this.Mini_Map_TilesetPointerBox.ValueChanged -= this.Mini_Map_TilesetPointerBox_ValueChanged;
            this.SmallMap_PalettePointerBox.ValueChanged -= this.SmallMap_PalettePointerBox_ValueChanged;
            this.SmallMap_TilesetPointerBox.ValueChanged -= this.SmallMap_TilesetPointerBox_ValueChanged;
            this.SmallMap_TSAPointerBox.ValueChanged     -= this.SmallMap_TSAPointerBox_ValueChanged;
            this.LargeMap_PalettePointerBox.ValueChanged -= this.LargeMap_PalettePointerBox_ValueChanged;
            this.LargeMap_TilesetPointerBox.ValueChanged -= this.LargeMap_TilesetPointerBox_ValueChanged;
            this.LargeMap_TSAPointerBox.ValueChanged     -= this.LargeMap_TSAPointerBox_ValueChanged;



            this.Mini_Map_PalettePointerBox.Value = pointers[MINI_MAP_PALETTE];
            this.Mini_Map_TilesetPointerBox.Value = pointers[MINI_MAP_TILESET];
            this.SmallMap_PalettePointerBox.Value = pointers[SMALLMAP_PALETTE];
            this.SmallMap_TilesetPointerBox.Value = pointers[SMALLMAP_TILESET];
            this.SmallMap_TSAPointerBox.Value     = pointers[SMALLMAP_TSA];
            this.LargeMap_PalettePointerBox.Value = pointers[LARGEMAP_PALETTE];
            this.LargeMap_TilesetPointerBox.Value = pointers[LARGEMAP_TILESET];
            this.LargeMap_TSAPointerBox.Value     = pointers[LARGEMAP_TSA];



            this.Mini_Map_PalettePointerBox.ValueChanged += this.Mini_Map_PalettePointerBox_ValueChanged;
            this.Mini_Map_TilesetPointerBox.ValueChanged += this.Mini_Map_TilesetPointerBox_ValueChanged;
            this.SmallMap_PalettePointerBox.ValueChanged += this.SmallMap_PalettePointerBox_ValueChanged;
            this.SmallMap_TilesetPointerBox.ValueChanged += this.SmallMap_TilesetPointerBox_ValueChanged;
            this.SmallMap_TSAPointerBox.ValueChanged     += this.SmallMap_TSAPointerBox_ValueChanged;
            this.LargeMap_PalettePointerBox.ValueChanged += this.LargeMap_PalettePointerBox_ValueChanged;
            this.LargeMap_TilesetPointerBox.ValueChanged += this.LargeMap_TilesetPointerBox_ValueChanged;
            this.LargeMap_TSAPointerBox.ValueChanged     += this.LargeMap_TSAPointerBox_ValueChanged;
        }

        void Core_InsertMiniMap(String filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                this.CurrentMiniMap = new WorldMap_FE8_Mini(filepath, palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the mini world map image.", ex); return;
            }

            Byte[] data_palette = this.CurrentMiniMap.Colors.ToBytes(false);
            Byte[] data_tileset = this.CurrentMiniMap.Sheet.ToBytes(true);

            UI.SuspendUpdate();

            Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Mini World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                this.CurrentEntry("Mini"), new Tuple<String, Pointer, Int32>[] {
                    Tuple.Create("Palette", Core.GetPointer("Mini World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Mini World Map Tileset"), data_tileset.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Mini World Map Palette"),
                data_palette,
                this.CurrentEntry("Mini") + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Mini World Map Tileset"),
                data_tileset,
                this.CurrentEntry("Mini") + "Graphics changed");

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        void Core_InsertSmallMap(String filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                this.CurrentSmallMap = new WorldMap_FE8_Small(filepath, palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the small world map image.", ex); return;
            }

            Byte[] data_palette = Palette.Merge(this.CurrentSmallMap.Palettes).ToBytes(false);
            Byte[] data_tileset = this.CurrentSmallMap.Graphics.ToBytes(true);
            Byte[] data_tsa = this.CurrentSmallMap.Tiling.ToBytes(true, true);

            UI.SuspendUpdate();

            Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Small World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                this.CurrentEntry("Small"), new Tuple<String, Pointer, Int32>[] {
                    Tuple.Create("Palette", Core.GetPointer("Small World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Small World Map Tileset"), data_tileset.Length),
                    Tuple.Create("TSA", Core.GetPointer("Small World Map TSA"), data_tsa.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Small World Map Palette"),
                data_palette,
                this.CurrentEntry("Small") + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map Tileset"),
                data_tileset,
                this.CurrentEntry("Small") + "Graphics changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map TSA"),
                data_tsa,
                this.CurrentEntry("Small") + "TSA changed");

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        void Core_InsertLargeMap(String filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                this.CurrentLargeMap = new WorldMap_FE8_Large(filepath, palette);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the large world map image.", ex); return;
            }

            Byte[] data_palette = Palette.Merge(this.CurrentLargeMap.Palettes).ToBytes(false);
            Byte[] data_tileset = this.CurrentLargeMap.Graphics.ToBytes(false);
            Byte[] data_tsa = LZ77.Compress(this.CurrentLargeMap.GetPaletteMap());

            UI.SuspendUpdate();

            Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Large World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                this.CurrentEntry("Large"), new Tuple<String, Pointer, Int32>[] {
                    Tuple.Create("Palette", Core.GetPointer("Large World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Large World Map Tileset"), data_tileset.Length),
                    Tuple.Create("TSA",     Core.GetPointer("Large World Map TSA"), data_tsa.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Large World Map Palette"),
                data_palette,
                this.CurrentEntry("Large") + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Large World Map Tileset"),
                data_tileset,
                this.CurrentEntry("Large") + "Graphics changed");

            Core.WriteData(this,
                Core.GetPointer("Large World Map TSA"),
                data_tsa,
                this.CurrentEntry("Large") + "TSA changed");

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }



        private void Mini_Map_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            try
            {
                this.CurrentMiniMap = new WorldMap_FE8_Mini(
                    Core.GetPointer("Mini World Map Palette"),
                    (Core.App.Game.Region == GameRegion.EUR) ?
                    Core.ReadPointer(Core.GetPointer("Mini World Map Tileset") + (Int32)this.Mini_Map_NumberBox.Value * 4) :
                    Core.GetPointer("Mini World Map Tileset"));
                this.Mini_Map_ImageBox.Load(this.CurrentMiniMap);
                this.Mini_Map_PaletteBox.Load(this.CurrentMiniMap.Colors);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the mini World Map.", ex);
                this.Mini_Map_ImageBox.Reset();
                this.Mini_Map_PaletteBox.Reset();
            }
        }

        private void Mini_Map_InsertButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                this.Core_InsertMiniMap(openWindow.FileName);
            }
        }

        private void Mini_Map_PalettePointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Mini World Map Palette"),
                this.Mini_Map_PalettePointerBox.Value,
                this.CurrentEntry("Mini") + "Palette repoint");
        }
        private void Mini_Map_TilesetPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Mini World Map Tileset"),
                this.Mini_Map_TilesetPointerBox.Value,
                this.CurrentEntry("Mini") + "Tileset repoint");
        }

        private void Mini_Map_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry("Mini"),
                this.Mini_Map_PalettePointerBox.Value, 1);
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
                this.SmallMap_PalettePointerBox.Value,
                this.CurrentEntry("Small") + "Palette repoint");
        }
        private void SmallMap_TilesetPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Tileset"),
                this.SmallMap_TilesetPointerBox.Value,
                this.CurrentEntry("Small") + "Tileset repoint");
        }
        private void SmallMap_TSAPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map TSA"),
                this.SmallMap_TSAPointerBox.Value,
                this.CurrentEntry("Small") + "TSA repoint");
        }

        private void SmallMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry("Small"),
                this.SmallMap_PalettePointerBox.Value, 4);
        }
        private void SmallMap_TSA_Button_Click(Object sender, EventArgs e)
        {
            UI.OpenTSAEditor(this,
                this.CurrentEntry("Small"),
                this.SmallMap_PalettePointerBox.Value, WorldMap_FE8_Small.PALETTES * Palette.LENGTH,
                this.SmallMap_TilesetPointerBox.Value, 0, // compressed
                this.SmallMap_TSAPointerBox.Value,
                WorldMap_FE8_Small.WIDTH,
                WorldMap_FE8_Small.HEIGHT,
                true, true); // TSA is row-flipped AND compressed, strangely enough
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

        private void LargeMap_PalettePointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map Palette"),
                this.LargeMap_PalettePointerBox.Value,
                this.CurrentEntry("Large") + "Palette repoint");
        }
        private void LargeMap_TilesetPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map Tileset"),
                this.LargeMap_TilesetPointerBox.Value,
                this.CurrentEntry("Large") + "Tileset repoint");
        }
        private void LargeMap_TSAPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map TSA"),
                this.LargeMap_TSAPointerBox.Value,
                this.CurrentEntry("Large") + "TSA repoint");
        }

        private void LargeMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry("Large"),
                this.LargeMap_PalettePointerBox.Value, 4);
        }
        private void LargeMap_TSA_Button_Click(Object sender, EventArgs e)
        {
            // This isn't TSA but actually a PaletteMap
            /*
            UI.OpenTSAEditor(this,
                CurrentEntry("Large"),
                LargeMap_TSAPointerBox.Value,
                WorldMap_FE8_Large.WIDTH,
                WorldMap_FE8_Large.HEIGHT,
                false, false); 
            */
        }
    }
}
