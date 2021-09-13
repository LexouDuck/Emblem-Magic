using System;
using System.Windows.Forms;
using Compression;
using EmblemMagic.FireEmblem;
using GBA;
using Magic;
using Magic.Editors;

namespace EmblemMagic.Editors
{
    public partial class WorldMapEditor_FE6 : Editor
    {
        const Int32 SMALLMAP_TILESET = 0;
        const Int32 SMALLMAP_PALETTE = 1;
        const Int32 LARGEMAP_TILESET = 2;
        const Int32 LARGEMAP_PALETTE = 3;

        WorldMap_FE6_Small CurrentSmallMap { get; set; }
        WorldMap_FE6_Large CurrentLargeMap { get; set; }

        String CurrentEntry(Boolean small)
        {
            return (small ? "Small" : "Large") + " World Map - ";
        }



        public WorldMapEditor_FE6()
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
            Pointer[] pointers = new Pointer[4]
            {
                Core.GetPointer("Small World Map Tileset"),
                Core.GetPointer("Small World Map Palette"),
                Core.GetPointer("Large World Map Tileset"),
                Core.GetPointer("Large World Map Palette"),
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
                this.CurrentSmallMap = new WorldMap_FE6_Small(
                    pointers[SMALLMAP_PALETTE],
                    pointers[SMALLMAP_TILESET]);
                this.SmallMap_ImageBox.Load(this.CurrentSmallMap);
                this.SmallMap_PaletteBox.Load(this.CurrentSmallMap.Colors);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not load the small World Map.", ex);
                this.SmallMap_ImageBox.Reset();
                this.SmallMap_PaletteBox.Reset();
            }

            try
            {
                this.CurrentLargeMap = new WorldMap_FE6_Large(
                    pointers[LARGEMAP_PALETTE],
                    pointers[LARGEMAP_TILESET]);
                this.LargeMap_ImageBox.Load(this.CurrentLargeMap);
                this.LargeMap_PaletteBox.Load(this.CurrentLargeMap.Graphics[0].Colors);
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
            this.SmallMap_PalettePointerBox.ValueChanged -= this.SmallMap_PalettePointerBox_ValueChanged;
            this.SmallMap_GraphicsPointerBox.ValueChanged -= this.SmallMap_GraphicsPointerBox_ValueChanged;
            this.LargeMap_PalettePointerBox.ValueChanged -= this.LargeMap_TL_PalettePointerBox_ValueChanged;
            this.LargeMap_TL_GraphicsPointerBox.ValueChanged -= this.LargeMap_TL_GraphicsPointerBox_ValueChanged;
            this.LargeMap_TR_GraphicsPointerBox.ValueChanged -= this.LargeMap_TR_GraphicsPointerBox_ValueChanged;
            this.LargeMap_BL_GraphicsPointerBox.ValueChanged -= this.LargeMap_BL_GraphicsPointerBox_ValueChanged;
            this.LargeMap_BR_GraphicsPointerBox.ValueChanged -= this.LargeMap_BR_GraphicsPointerBox_ValueChanged;



            this.SmallMap_PalettePointerBox.Value = Core.ReadPointer(pointers[SMALLMAP_PALETTE]);
            this.SmallMap_GraphicsPointerBox.Value = Core.ReadPointer(pointers[SMALLMAP_TILESET]);

            Pointer[] address = WorldMap_FE6_Large.GetPointerArray(pointers[LARGEMAP_PALETTE]);
            this.LargeMap_PalettePointerBox.Value = address[0];
            address = WorldMap_FE6_Large.GetPointerArray(pointers[LARGEMAP_TILESET]);
            this.LargeMap_TL_GraphicsPointerBox.Value = address[0];
            this.LargeMap_TR_GraphicsPointerBox.Value = address[1];
            this.LargeMap_BL_GraphicsPointerBox.Value = address[2];
            this.LargeMap_BR_GraphicsPointerBox.Value = address[3];



            this.SmallMap_PalettePointerBox.ValueChanged += this.SmallMap_PalettePointerBox_ValueChanged;
            this.SmallMap_GraphicsPointerBox.ValueChanged += this.SmallMap_GraphicsPointerBox_ValueChanged;
            this.LargeMap_PalettePointerBox.ValueChanged += this.LargeMap_TL_PalettePointerBox_ValueChanged;
            this.LargeMap_TL_GraphicsPointerBox.ValueChanged += this.LargeMap_TL_GraphicsPointerBox_ValueChanged;
            this.LargeMap_TR_GraphicsPointerBox.ValueChanged += this.LargeMap_TR_GraphicsPointerBox_ValueChanged;
            this.LargeMap_BL_GraphicsPointerBox.ValueChanged += this.LargeMap_BL_GraphicsPointerBox_ValueChanged;
            this.LargeMap_BR_GraphicsPointerBox.ValueChanged += this.LargeMap_BR_GraphicsPointerBox_ValueChanged;
        }
        void Core_WriteSmallMap(String path)
        {
            try
            {
                this.CurrentSmallMap = new WorldMap_FE6_Small(path);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the image.", ex); return;
            }

            Byte[] data_palette = this.CurrentSmallMap.Colors.ToBytes(true);
            Byte[] data_tileset = LZ77.Compress(this.CurrentSmallMap.ToBytes());

            UI.SuspendUpdate();

            Boolean cancel = Prompt.ShowRepointDialog(this, "Repoint Small World Map",
                "The different parts of this image may need to be repointed upon insertion.",
                this.CurrentEntry(true), new Tuple<String, Pointer, Int32>[] {
                    Tuple.Create("Palette", Core.GetPointer("Small World Map Palette"), data_palette.Length),
                    Tuple.Create("Tileset", Core.GetPointer("Small World Map Tileset"), data_tileset.Length)});
            if (cancel) return;

            Core.WriteData(this,
                Core.GetPointer("Small World Map Palette"),
                data_palette,
                this.CurrentEntry(true) + "Palette changed");

            Core.WriteData(this,
                Core.GetPointer("Small World Map Tileset"),
                data_tileset,
                this.CurrentEntry(true) + "Tileset changed");

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }
        void Core_WriteLargeMap(String path)
        {
            try
            {
                this.CurrentLargeMap = new WorldMap_FE6_Large(new Bitmap(path));
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not insert the image.", ex); return;
            }

            UI.SuspendUpdate();

            Core.WriteData(this,
                this.LargeMap_PalettePointerBox.Value,
                this.CurrentLargeMap.Graphics[0].Colors.ToBytes(true),
                this.CurrentEntry(true) + "Palette changed");

            Core.WriteData(this,
                this.LargeMap_TL_GraphicsPointerBox.Value,
                LZ77.Compress(this.CurrentLargeMap.Graphics[0].ToBytes()),
                this.CurrentEntry(true) + "Graphics (TL) changed");
            Core.WriteData(this,
                this.LargeMap_TR_GraphicsPointerBox.Value,
                LZ77.Compress(this.CurrentLargeMap.Graphics[1].ToBytes()),
                this.CurrentEntry(true) + "Graphics (TR) changed");
            Core.WriteData(this,
                this.LargeMap_BL_GraphicsPointerBox.Value,
                LZ77.Compress(this.CurrentLargeMap.Graphics[2].ToBytes()),
                this.CurrentEntry(true) + "Graphics (BL) changed");
            Core.WriteData(this,
                this.LargeMap_BR_GraphicsPointerBox.Value,
                LZ77.Compress(this.CurrentLargeMap.Graphics[3].ToBytes()),
                this.CurrentEntry(true) + "Graphics (BR) changed");

            UI.ResumeUpdate();
            UI.PerformUpdate();
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
                this.Core_WriteSmallMap(openWindow.FileName);
            }
        }

        private void SmallMap_GraphicsPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Tileset"),
                this.SmallMap_GraphicsPointerBox.Value,
                this.CurrentEntry(true) + "Graphics repoint");
        }
        private void SmallMap_PalettePointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.Repoint(this,
                Core.GetPointer("Small World Map Palette"),
                this.SmallMap_PalettePointerBox.Value,
                this.CurrentEntry(true) + "Palette repoint");
        }

        private void SmallMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry(true),
                this.SmallMap_PalettePointerBox.Value, 0);
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
                this.Core_WriteLargeMap(openWindow.FileName);
            }
        }

        private void LargeMap_TL_GraphicsPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Large World Map Tileset"),
                this.LargeMap_TL_GraphicsPointerBox.Value,
                this.CurrentEntry(false) + "Graphics (TL) repoint");
        }
        private void LargeMap_TR_GraphicsPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Large World Map Tileset") + 8,
                this.LargeMap_TR_GraphicsPointerBox.Value,
                this.CurrentEntry(false) + "Graphics (TR) repoint");
        }
        private void LargeMap_BL_GraphicsPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Large World Map Tileset") + 16,
                this.LargeMap_BL_GraphicsPointerBox.Value,
                this.CurrentEntry(false) + "Graphics (BL) repoint");
        }
        private void LargeMap_BR_GraphicsPointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.GetPointer("Large World Map Tileset") + 24,
                this.LargeMap_BR_GraphicsPointerBox.Value,
                this.CurrentEntry(false) + "Graphics (BR) repoint");
        }

        private void LargeMap_TL_PalettePointerBox_ValueChanged(Object sender, EventArgs e)
        {
            Core.WritePointer(this,
                Core.App.Game.Addresses["Large World Map Palette"] + 0,
                this.LargeMap_PalettePointerBox.Value,
                this.CurrentEntry(false) + "Palette (TL) repoint");
            Core.WritePointer(this,
                Core.App.Game.Addresses["Large World Map Palette"] + 8,
                this.LargeMap_PalettePointerBox.Value,
                this.CurrentEntry(false) + "Palette (TR) repoint");
            Core.WritePointer(this,
                Core.App.Game.Addresses["Large World Map Palette"] + 16,
                this.LargeMap_PalettePointerBox.Value,
                this.CurrentEntry(false) + "Palette (BL) repoint");
            Core.WritePointer(this,
                Core.App.Game.Addresses["Large World Map Palette"] + 24,
                this.LargeMap_PalettePointerBox.Value,
                this.CurrentEntry(false) + "Palette (BR) repoint");
        }

        private void LargeMap_PaletteBox_Click(Object sender, EventArgs e)
        {
            UI.OpenPaletteEditor(this,
                this.CurrentEntry(false),
                this.LargeMap_PalettePointerBox.Value, 0);
        }
    }
}
