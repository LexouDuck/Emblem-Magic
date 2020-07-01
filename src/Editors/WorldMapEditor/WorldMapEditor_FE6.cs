using System;
using EmblemMagic.FireEmblem;
using GBA;
using Compression;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class WorldMapEditor_FE6 : Editor
    {
        const int SMALLMAP_TILESET = 0;
        const int SMALLMAP_PALETTE = 1;
        const int LARGEMAP_TILESET = 2;
        const int LARGEMAP_PALETTE = 3;

        WorldMap_FE6_Small CurrentSmallMap { get; set; }
        WorldMap_FE6_Large CurrentLargeMap { get; set; }

        string CurrentEntry(bool small)
        {
            return (small ? "Small" : "Large") + " World Map - ";
        }



        public WorldMapEditor_FE6()
        {
            try
            {
                InitializeComponent();
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
            Pointer[] pointers = new Pointer[4]
            {
                Core.GetPointer("Small World Map Tileset"),
                Core.GetPointer("Small World Map Palette"),
                Core.GetPointer("Large World Map Tileset"),
                Core.GetPointer("Large World Map Palette"),
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
                CurrentSmallMap = new WorldMap_FE6_Small(
                    pointers[SMALLMAP_PALETTE],
                    pointers[SMALLMAP_TILESET]);
                SmallMap_ImageBox.Load(CurrentSmallMap);
                SmallMap_PaletteBox.Load(CurrentSmallMap.Colors);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not load the small World Map.", ex);
                SmallMap_ImageBox.Reset();
                SmallMap_PaletteBox.Reset();
            }

            try
            {
                CurrentLargeMap = new WorldMap_FE6_Large(
                    pointers[LARGEMAP_PALETTE],
                    pointers[LARGEMAP_TILESET]);
                LargeMap_ImageBox.Load(CurrentLargeMap);
                LargeMap_PaletteBox.Load(CurrentLargeMap.Graphics[0].Colors);
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
            SmallMap_PalettePointerBox.ValueChanged -= SmallMap_PalettePointerBox_ValueChanged;
            SmallMap_GraphicsPointerBox.ValueChanged -= SmallMap_GraphicsPointerBox_ValueChanged;
            LargeMap_PalettePointerBox.ValueChanged -= LargeMap_TL_PalettePointerBox_ValueChanged;
            LargeMap_TL_GraphicsPointerBox.ValueChanged -= LargeMap_TL_GraphicsPointerBox_ValueChanged;
            LargeMap_TR_GraphicsPointerBox.ValueChanged -= LargeMap_TR_GraphicsPointerBox_ValueChanged;
            LargeMap_BL_GraphicsPointerBox.ValueChanged -= LargeMap_BL_GraphicsPointerBox_ValueChanged;
            LargeMap_BR_GraphicsPointerBox.ValueChanged -= LargeMap_BR_GraphicsPointerBox_ValueChanged;



            SmallMap_PalettePointerBox.Value = Core.ReadPointer(pointers[SMALLMAP_PALETTE]);
            SmallMap_GraphicsPointerBox.Value = Core.ReadPointer(pointers[SMALLMAP_TILESET]);

            Pointer[] address = WorldMap_FE6_Large.GetPointerArray(pointers[LARGEMAP_PALETTE]);
            LargeMap_PalettePointerBox.Value = address[0];
            address = WorldMap_FE6_Large.GetPointerArray(pointers[LARGEMAP_TILESET]);
            LargeMap_TL_GraphicsPointerBox.Value = address[0];
            LargeMap_TR_GraphicsPointerBox.Value = address[1];
            LargeMap_BL_GraphicsPointerBox.Value = address[2];
            LargeMap_BR_GraphicsPointerBox.Value = address[3];



            SmallMap_PalettePointerBox.ValueChanged += SmallMap_PalettePointerBox_ValueChanged;
            SmallMap_GraphicsPointerBox.ValueChanged += SmallMap_GraphicsPointerBox_ValueChanged;
            LargeMap_PalettePointerBox.ValueChanged += LargeMap_TL_PalettePointerBox_ValueChanged;
            LargeMap_TL_GraphicsPointerBox.ValueChanged += LargeMap_TL_GraphicsPointerBox_ValueChanged;
            LargeMap_TR_GraphicsPointerBox.ValueChanged += LargeMap_TR_GraphicsPointerBox_ValueChanged;
            LargeMap_BL_GraphicsPointerBox.ValueChanged += LargeMap_BL_GraphicsPointerBox_ValueChanged;
            LargeMap_BR_GraphicsPointerBox.ValueChanged += LargeMap_BR_GraphicsPointerBox_ValueChanged;
        }
        void Core_WriteSmallMap(string path)
        {
            try
            {
                CurrentSmallMap = new WorldMap_FE6_Small(path);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the image.", ex); return;
            }

            byte[] data_palette = CurrentSmallMap.Colors.ToBytes(true);
            byte[] data_tileset = LZ77.Compress(CurrentSmallMap.ToBytes());

            Core.SuspendUpdate();

            bool cancel = Prompt.ShowRepointDialog(this, "Repoint Small World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                CurrentEntry(true), new Tuple<string, Pointer, int>[] {
                    Tuple.Create("Palette", Core.GetPointer("Small World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Small World Map Tileset"), data_tileset.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Small World Map Palette"),
                data_palette,
                CurrentEntry(true) + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map Tileset"),
                data_tileset,
                CurrentEntry(true) + "Tileset changed");

            Core.ResumeUpdate();
            Core.PerformUpdate();
        }
        void Core_WriteLargeMap(string path)
        {
            try
            {
                CurrentLargeMap = new WorldMap_FE6_Large(new Bitmap(path));
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not insert the image.", ex); return;
            }

            Core.SuspendUpdate();

            Core.WriteData(this,
                LargeMap_PalettePointerBox.Value,
                CurrentLargeMap.Graphics[0].Colors.ToBytes(true),
                CurrentEntry(true) + "Palette changed");

            Core.WriteData(this,
                LargeMap_TL_GraphicsPointerBox.Value,
                LZ77.Compress(CurrentLargeMap.Graphics[0].ToBytes()),
                CurrentEntry(true) + "Graphics (TL) changed");
            Core.WriteData(this,
                LargeMap_TR_GraphicsPointerBox.Value,
                LZ77.Compress(CurrentLargeMap.Graphics[1].ToBytes()),
                CurrentEntry(true) + "Graphics (TR) changed");
            Core.WriteData(this,
                LargeMap_BL_GraphicsPointerBox.Value,
                LZ77.Compress(CurrentLargeMap.Graphics[2].ToBytes()),
                CurrentEntry(true) + "Graphics (BL) changed");
            Core.WriteData(this,
                LargeMap_BR_GraphicsPointerBox.Value,
                LZ77.Compress(CurrentLargeMap.Graphics[3].ToBytes()),
                CurrentEntry(true) + "Graphics (BR) changed");

            Core.ResumeUpdate();
            Core.PerformUpdate();
        }



        private void SmallMap_InsertButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                Core_WriteSmallMap(openWindow.FileName);
            }
        }

        private void SmallMap_GraphicsPointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Tileset"),
                SmallMap_GraphicsPointerBox.Value,
                CurrentEntry(true) + "Graphics repoint");
        }
        private void SmallMap_PalettePointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Palette"),
                SmallMap_PalettePointerBox.Value,
                CurrentEntry(true) + "Palette repoint");
        }

        private void SmallMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                CurrentEntry(true),
                SmallMap_PalettePointerBox.Value, 0);
        }



        private void LargeMap_InsertButton_Click(Object sender, EventArgs e)
        {
            OpenFileDialog openWindow = new OpenFileDialog();
            openWindow.Filter = "Image files (*.png, *.bmp, *.gif)|*.png;*.bmp;*.gif|All files (*.*)|*.*";
            openWindow.FilterIndex = 1;
            openWindow.RestoreDirectory = true;
            openWindow.Multiselect = false;

            if (openWindow.ShowDialog() == DialogResult.OK)
            {
                Core_WriteLargeMap(openWindow.FileName);
            }
        }

        private void LargeMap_TL_GraphicsPointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Large World Map Tileset"),
                LargeMap_TL_GraphicsPointerBox.Value,
                CurrentEntry(false) + "Graphics (TL) repoint");
        }
        private void LargeMap_TR_GraphicsPointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Large World Map Tileset") + 8,
                LargeMap_TR_GraphicsPointerBox.Value,
                CurrentEntry(false) + "Graphics (TR) repoint");
        }
        private void LargeMap_BL_GraphicsPointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Large World Map Tileset") + 16,
                LargeMap_BL_GraphicsPointerBox.Value,
                CurrentEntry(false) + "Graphics (BL) repoint");
        }
        private void LargeMap_BR_GraphicsPointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Large World Map Tileset") + 24,
                LargeMap_BR_GraphicsPointerBox.Value,
                CurrentEntry(false) + "Graphics (BR) repoint");
        }

        private void LargeMap_TL_PalettePointerBox_ValueChanged(object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.CurrentROM.Address_WorldMap()[LARGEMAP_PALETTE],
                LargeMap_PalettePointerBox.Value,
                CurrentEntry(false) + "Palette (TL) repoint");
            Core.WritePointer(this,
                Core.CurrentROM.Address_WorldMap()[LARGEMAP_PALETTE] + 8,
                LargeMap_PalettePointerBox.Value,
                CurrentEntry(false) + "Palette (TR) repoint");
            Core.WritePointer(this,
                Core.CurrentROM.Address_WorldMap()[LARGEMAP_PALETTE] + 16,
                LargeMap_PalettePointerBox.Value,
                CurrentEntry(false) + "Palette (BL) repoint");
            Core.WritePointer(this,
                Core.CurrentROM.Address_WorldMap()[LARGEMAP_PALETTE] + 24,
                LargeMap_PalettePointerBox.Value,
                CurrentEntry(false) + "Palette (BR) repoint");
        }

        private void LargeMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            Core.OpenPaletteEditor(this,
                CurrentEntry(false),
                LargeMap_PalettePointerBox.Value, 0);
        }
    }
}
