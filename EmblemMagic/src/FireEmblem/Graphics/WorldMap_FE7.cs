using System;
using GBA;
using Magic;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    public class WorldMap_FE7_Small : TSA_Image
    {
        public const Int32 WIDTH = 30;
        public const Int32 HEIGHT = 20;

        public const Int32 PALETTES = 4;



        public WorldMap_FE7_Small(
            Pointer palette,
            Pointer tiles,
            Pointer tsa)
            : base(
                Core.ReadData(palette, PALETTES * Palette.LENGTH),
                Core.ReadData(tiles, 0),
                Core.ReadTSA(tsa, WIDTH, HEIGHT, false, true)) { }

        public WorldMap_FE7_Small(String path, Palette palette = null) : base(WIDTH, HEIGHT)
        {
            TSA_Image image;
            if (palette == null)
            {
                image = new TSA_Image(WIDTH, HEIGHT, new GBA.Bitmap(path), PALETTES, true);
            }
            else
            {
                image = new TSA_Image(WIDTH, HEIGHT, new GBA.Bitmap(path), palette, PALETTES, true);
            }
            this.Palettes = image.Palettes;
            this.Graphics = image.Graphics;
            this.Tiling = image.Tiling;
        }
    }



    public class WorldMap_FE7_Large : IDisplayable
    {
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                Int32 tileX = (x / 8) / 32;
                Int32 tileY = (y / 8) / 32;
                Int32 index = tileX + tileY * 4;
                tileX = (x / 8) % 32;
                tileY = (y / 8) % 32;
                Int32 tile = this.TSA_Sections[index][tileX, tileY].TileIndex;
                Int32 palette = this.TSA_Sections[index][tileX, tileY].Palette;
                tileX = (x % 32) % 8;
                tileY = (y % 32) % 8;
                return palette * Palette.MAX + this.Graphics[index][tile][tileX, tileY];
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public GBA.Color GetColor(Int32 x, Int32 y)
        {
            Int32 tileX = (x / 8) / 32;
            Int32 tileY = (y / 8) / 32;
            Int32 index = tileX + tileY * 4;
            tileX = (x / 8) % 32;
            tileY = (y / 8) % 32;
            Int32 tile = this.TSA_Sections[index][tileX, tileY].TileIndex;
            Int32 palette = this.TSA_Sections[index][tileX, tileY].Palette;
            tileX = (x % 32) % 8;
            tileY = (y % 32) % 8;
            return this.Palettes[palette][this.Graphics[index][tile][tileX, tileY]];
        }

        public Int32 Width
        {
            get
            {
                return WIDTH * 8;
            }
        }
        public Int32 Height
        {
            get
            {
                return HEIGHT * 8;
            }
        }

        public const Int32 WIDTH = 128;
        public const Int32 HEIGHT = 86;

        public const Int32 PALETTES = 4;
        public const Int32 SECTIONS = 12;

        public Palette[] Palettes { get; set; }
        public Tileset[] Graphics { get; set; }
        public TSA_Array[] TSA_Sections { get; set; }



        public WorldMap_FE7_Large(
            Pointer palette,
            Pointer tiles,
            Pointer tsa)
        {
            Pointer[] pointers;
            this.Palettes = new Palette[PALETTES];
            for (Int32 i = 0; i < this.Palettes.Length; i++)
            {
                this.Palettes[i] = Core.ReadPalette(palette + i * Palette.LENGTH, Palette.LENGTH);
            }

            pointers = GetPointerArray(tiles);
            this.Graphics = new Tileset[SECTIONS];
            for (Int32 i = 0; i < this.Graphics.Length; i++)
            {
                this.Graphics[i] = new Tileset(Core.ReadData(pointers[i], 32 * 32 * Tile.LENGTH));
            }

            pointers = GetPointerArray(tsa);
            this.TSA_Sections = new TSA_Array[SECTIONS];
            for (Int32 i = 0; i < this.TSA_Sections.Length; i++)
            {
                this.TSA_Sections[i] = Core.ReadTSA(pointers[i], 32, 32, false, true);
            }   // sections of 32x32 and 32x22 to make an image thats 128x86 tiles, 1024x688 pixels
        }

        public WorldMap_FE7_Large(String path, Palette palette = null)
        {
            TSA_Image image;
            if (palette == null)
            {
                image = new TSA_Image(WIDTH, HEIGHT, new GBA.Bitmap(path), PALETTES, false);
            }
            else
            {
                image = new TSA_Image(WIDTH, HEIGHT, new GBA.Bitmap(path), palette, PALETTES, false);
            }

            this.Palettes = image.Palettes;

            this.Graphics = new Tileset[SECTIONS];
            this.TSA_Sections = new TSA_Array[SECTIONS];
            Int32 width = 32;
            Int32 height = 32;
            for (Int32 i = 0; i < SECTIONS; i++)
            {
                if (i == 8) height = 22;

                this.Graphics[i] = new Tileset(width * height);
                for (Int32 y = 0; y < height; y++)
                for (Int32 x = 0; x < width; x++)
                {
                        this.Graphics[i].Add(image.Graphics[
                        (i % 4 * width + x) +
                        (i / 4 * width + y) * WIDTH]);
                }

                this.TSA_Sections[i] = TSA_Array.GetBasicTSA(width, height);
                for (Int32 y = 0; y < height; y++)
                for (Int32 x = 0; x < width; x++)
                {
                        this.TSA_Sections[i].SetPalette(x, y, image.Tiling[
                        (i % 4 * width) + x,
                        (i / 4 * width) + y].Palette);
                }
            }
        }
        
        public static Pointer[] GetPointerArray(Pointer address)
        {
            Pointer[] result = new Pointer[SECTIONS];
            for (Int32 i = 0; i < SECTIONS; i++)
            {
                result[i] = Core.ReadPointer(address + i * 4);
            }
            return result;
        }

        /// <summary>
        /// Returns a bitmap of the section number given
        /// </summary>
        public TSA_Image GetSection(Int32 section)
        {
            return new TSA_Image(
                Palette.Merge(this.Palettes),
                this.Graphics[section],
                this.TSA_Sections[section]);
        }
    }
}
