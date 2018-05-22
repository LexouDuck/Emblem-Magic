using System;
using EmblemMagic.FireEmblem;
using GBA;
using System.Windows.Forms;
using Compression;

namespace EmblemMagic.Editors
{
    public partial class WorldMapEditor_FE8 : Editor
    {
        const int MINI_MAP_TILESET = 0;
        const int MINI_MAP_PALETTE = 1;
        const int SMALLMAP_TILESET = 2;
        const int SMALLMAP_PALETTE = 3;
        const int SMALLMAP_TSA     = 4;
        const int LARGEMAP_TILESET = 5;
        const int LARGEMAP_PALETTE = 6;
        const int LARGEMAP_TSA     = 7;

        WorldMap_FE8_Mini CurrentMiniMap { get; set; }
        public WorldMap_FE8_Small CurrentSmallMap { get; set; }
        public WorldMap_FE8_Large CurrentLargeMap { get; set; }

        string CurrentEntry(string prefix)
        {
            if (prefix.Equals("Mini") && Core.CurrentROM.Version == GameVersion.EUR)
                return prefix + " World Map " + Mini_Map_NumberBox.Value + " - ";
            else return prefix + " World Map - ";
        }



        public WorldMapEditor_FE8()
        {
            try
            {
                InitializeComponent();

                if (Core.CurrentROM.Version == GameVersion.EUR)
                    Mini_Map_NumberBox.Enabled = true;
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }

        override public void Core_OnOpen()
        {
            Core_Update();
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
                Core_LoadValues(pointers);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly load world map pointers.", ex);
            }

            try
            {
                CurrentMiniMap = new WorldMap_FE8_Mini(
                    pointers[MINI_MAP_PALETTE],
                    (Core.CurrentROM.Version == GameVersion.EUR) ?
                    Core.ReadPointer(pointers[MINI_MAP_TILESET] + (int)Mini_Map_NumberBox.Value * 4) :
                    pointers[MINI_MAP_TILESET]);
                Mini_Map_ImageBox.Load(CurrentMiniMap);
                Mini_Map_PaletteBox.Load(CurrentMiniMap.Colors);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the mini World Map.", ex);
                Mini_Map_ImageBox.Reset();
                Mini_Map_PaletteBox.Reset();
            }

            try
            {
                CurrentSmallMap = new WorldMap_FE8_Small(
                    pointers[SMALLMAP_PALETTE],
                    pointers[SMALLMAP_TILESET],
                    pointers[SMALLMAP_TSA]);
                SmallMap_ImageBox.Load(CurrentSmallMap);
                SmallMap_PaletteBox.Load(Palette.Merge(CurrentSmallMap.Palettes));
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the small World Map.", ex);
                SmallMap_ImageBox.Reset();
                SmallMap_PaletteBox.Reset();
            }

            try
            {
                CurrentLargeMap = new WorldMap_FE8_Large(
                    pointers[LARGEMAP_PALETTE],
                    pointers[LARGEMAP_TILESET],
                    pointers[LARGEMAP_TSA]);
                LargeMap_ImageBox.Load(CurrentLargeMap);
                LargeMap_PaletteBox.Load(Palette.Merge(CurrentLargeMap.Palettes));
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the large World Map.", ex);
                LargeMap_ImageBox.Reset();
                LargeMap_PaletteBox.Reset();
            }
        }

        void Core_LoadValues(Pointer[] pointers)
        {
            Mini_Map_PalettePointerBox.ValueChanged -= Mini_Map_PalettePointerBox_ValueChanged;
            Mini_Map_TilesetPointerBox.ValueChanged -= Mini_Map_TilesetPointerBox_ValueChanged;
            SmallMap_PalettePointerBox.ValueChanged -= SmallMap_PalettePointerBox_ValueChanged;
            SmallMap_TilesetPointerBox.ValueChanged -= SmallMap_TilesetPointerBox_ValueChanged;
            SmallMap_TSAPointerBox.ValueChanged     -= SmallMap_TSAPointerBox_ValueChanged;
            LargeMap_PalettePointerBox.ValueChanged -= LargeMap_PalettePointerBox_ValueChanged;
            LargeMap_TilesetPointerBox.ValueChanged -= LargeMap_TilesetPointerBox_ValueChanged;
            LargeMap_TSAPointerBox.ValueChanged     -= LargeMap_TSAPointerBox_ValueChanged;



            Mini_Map_PalettePointerBox.Value = pointers[MINI_MAP_PALETTE];
            Mini_Map_TilesetPointerBox.Value = pointers[MINI_MAP_TILESET];
            SmallMap_PalettePointerBox.Value = pointers[SMALLMAP_PALETTE];
            SmallMap_TilesetPointerBox.Value = pointers[SMALLMAP_TILESET];
            SmallMap_TSAPointerBox.Value     = pointers[SMALLMAP_TSA];
            LargeMap_PalettePointerBox.Value = pointers[LARGEMAP_PALETTE];
            LargeMap_TilesetPointerBox.Value = pointers[LARGEMAP_TILESET];
            LargeMap_TSAPointerBox.Value     = pointers[LARGEMAP_TSA];



            Mini_Map_PalettePointerBox.ValueChanged += Mini_Map_PalettePointerBox_ValueChanged;
            Mini_Map_TilesetPointerBox.ValueChanged += Mini_Map_TilesetPointerBox_ValueChanged;
            SmallMap_PalettePointerBox.ValueChanged += SmallMap_PalettePointerBox_ValueChanged;
            SmallMap_TilesetPointerBox.ValueChanged += SmallMap_TilesetPointerBox_ValueChanged;
            SmallMap_TSAPointerBox.ValueChanged     += SmallMap_TSAPointerBox_ValueChanged;
            LargeMap_PalettePointerBox.ValueChanged += LargeMap_PalettePointerBox_ValueChanged;
            LargeMap_TilesetPointerBox.ValueChanged += LargeMap_TilesetPointerBox_ValueChanged;
            LargeMap_TSAPointerBox.ValueChanged     += LargeMap_TSAPointerBox_ValueChanged;
        }

        void Core_InsertMiniMap(string filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                CurrentMiniMap = new WorldMap_FE8_Mini(filepath, palette);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the mini world map image.", ex); return;
            }

            byte[] data_palette = CurrentMiniMap.Colors.ToBytes(false);
            byte[] data_tileset = CurrentMiniMap.Sheet.ToBytes(true);

            Core.SuspendUpdate();

            bool cancel = Prompt.ShowRepointDialog(this, "Repoint Mini World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                CurrentEntry("Mini"), new Tuple<string, Pointer, int>[] {
                    Tuple.Create("Palette", Core.GetPointer("Mini World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Mini World Map Tileset"), data_tileset.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Mini World Map Palette"),
                data_palette,
                CurrentEntry("Mini") + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Mini World Map Tileset"),
                data_tileset,
                CurrentEntry("Mini") + "Graphics changed");

            Core.ResumeUpdate();
            Core.PerformUpdate();
        }
        void Core_InsertSmallMap(string filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                CurrentSmallMap = new WorldMap_FE8_Small(filepath, palette);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the small world map image.", ex); return;
            }

            byte[] data_palette = Palette.Merge(CurrentSmallMap.Palettes).ToBytes(false);
            byte[] data_tileset = CurrentSmallMap.Graphics.ToBytes(true);
            byte[] data_tsa = CurrentSmallMap.Tiling.ToBytes(true, true);

            Core.SuspendUpdate();

            bool cancel = Prompt.ShowRepointDialog(this, "Repoint Small World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                CurrentEntry("Small"), new Tuple<string, Pointer, int>[] {
                    Tuple.Create("Palette", Core.GetPointer("Small World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Small World Map Tileset"), data_tileset.Length),
                    Tuple.Create("TSA", Core.GetPointer("Small World Map TSA"), data_tsa.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Small World Map Palette"),
                data_palette,
                CurrentEntry("Small") + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map Tileset"),
                data_tileset,
                CurrentEntry("Small") + "Graphics changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map TSA"),
                data_tsa,
                CurrentEntry("Small") + "TSA changed");

            Core.ResumeUpdate();
            Core.PerformUpdate();
        }
        void Core_InsertLargeMap(string filepath)
        {
            try
            {
                Palette palette = Core.FindPaletteFile(filepath);

                CurrentLargeMap = new WorldMap_FE8_Large(filepath, palette);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the large world map image.", ex); return;
            }

            byte[] data_palette = Palette.Merge(CurrentLargeMap.Palettes).ToBytes(false);
            byte[] data_tileset = CurrentLargeMap.Graphics.ToBytes(false);
            byte[] data_tsa = LZ77.Compress(CurrentLargeMap.GetPaletteMap());

            Core.SuspendUpdate();

            bool cancel = Prompt.ShowRepointDialog(this, "Repoint Large World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                CurrentEntry("Large"), new Tuple<string, Pointer, int>[] {
                    Tuple.Create("Palette", Core.GetPointer("Large World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Large World Map Tileset"), data_tileset.Length),
                    Tuple.Create("TSA",     Core.GetPointer("Large World Map TSA"), data_tsa.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Large World Map Palette"),
                data_palette,
                CurrentEntry("Large") + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Large World Map Tileset"),
                data_tileset,
                CurrentEntry("Large") + "Graphics changed");

            Core.WriteData(this,
                Core.GetPointer("Large World Map TSA"),
                data_tsa,
                CurrentEntry("Large") + "TSA changed");

            Core.ResumeUpdate();
            Core.PerformUpdate();
        }



        private void Mini_Map_NumberBox_ValueChanged(Object sender, EventArgs e)
        {
            Pointer[] pointers = Core.CurrentROM.Address_WorldMap();

            try
            {
                CurrentMiniMap = new WorldMap_FE8_Mini(
                    pointers[MINI_MAP_PALETTE],
                    (Core.CurrentROM.Version == GameVersion.EUR) ?
                    Core.ReadPointer(pointers[MINI_MAP_TILESET] + (int)Mini_Map_NumberBox.Value * 4) :
                    pointers[MINI_MAP_TILESET]);
                Mini_Map_ImageBox.Load(CurrentMiniMap);
                Mini_Map_PaletteBox.Load(CurrentMiniMap.Colors);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the mini World Map.", ex);
                Mini_Map_ImageBox.Reset();
                Mini_Map_PaletteBox.Reset();
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
                Core_InsertMiniMap(openWindow.FileName);
            }
        }

        private void Mini_Map_PalettePointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Mini World Map Palette"),
                Mini_Map_PalettePointerBox.Value,
                CurrentEntry("Mini") + "Palette repoint");
        }
        private void Mini_Map_TilesetPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Mini World Map Tileset"),
                Mini_Map_TilesetPointerBox.Value,
                CurrentEntry("Mini") + "Tileset repoint");
        }

        private void Mini_Map_PaletteBox_Click(Object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                CurrentEntry("Mini"),
                Mini_Map_PalettePointerBox.Value, 1);
        }



        private void SmallMap_InsertButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter =
                "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "TSA Image Data (.tsa + .pal + .dmp)|*.tsa|" +
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
                SmallMap_PalettePointerBox.Value,
                CurrentEntry("Small") + "Palette repoint");
        }
        private void SmallMap_TilesetPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Tileset"),
                SmallMap_TilesetPointerBox.Value,
                CurrentEntry("Small") + "Tileset repoint");
        }
        private void SmallMap_TSAPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map TSA"),
                SmallMap_TSAPointerBox.Value,
                CurrentEntry("Small") + "TSA repoint");
        }

        private void SmallMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                CurrentEntry("Small"),
                SmallMap_PalettePointerBox.Value, 4);
        }
        private void SmallMap_TSA_Button_Click(Object sender, EventArgs e)
        {
            Core.OpenTSAEditor(this,
                CurrentEntry("Small"),
                SmallMap_PalettePointerBox.Value, WorldMap_FE8_Small.PALETTES * Palette.LENGTH,
                SmallMap_TilesetPointerBox.Value, 0, // compressed
                SmallMap_TSAPointerBox.Value,
                WorldMap_FE8_Small.WIDTH,
                WorldMap_FE8_Small.HEIGHT,
                true, true); // TSA is row-flipped AND compressed, strangely enough
        }



        private void LargeMap_InsertButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter =
                "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|" +
                "TSA Image Data (.tsa + .pal + .dmp)|*.tsa|" +
                "All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                Core_InsertLargeMap(openWindow.FileName);
            }
        }

        private void LargeMap_PalettePointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map Palette"),
                LargeMap_PalettePointerBox.Value,
                CurrentEntry("Large") + "Palette repoint");
        }
        private void LargeMap_TilesetPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map Tileset"),
                LargeMap_TilesetPointerBox.Value,
                CurrentEntry("Large") + "Tileset repoint");
        }
        private void LargeMap_TSAPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Large World Map TSA"),
                LargeMap_TSAPointerBox.Value,
                CurrentEntry("Large") + "TSA repoint");
        }

        private void LargeMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                CurrentEntry("Large"),
                LargeMap_PalettePointerBox.Value, 4);
        }
        private void LargeMap_TSA_Button_Click(Object sender, EventArgs e)
        {
            // This isn't TSA but actually a PaletteMap
            /*
            Core.OpenTSAEditor(this,
                CurrentEntry("Large"),
                LargeMap_TSAPointerBox.Value,
                WorldMap_FE8_Large.WIDTH,
                WorldMap_FE8_Large.HEIGHT,
                false, false); 
            */
        }
    }
}
