using System;
using GBA;
using EmblemMagic.Components;
using System.Drawing;

namespace EmblemMagic.FireEmblem
{
    public class WorldMap_FE7_Small : TSA_Image
    {
        public const int WIDTH = 30;
        public const int HEIGHT = 20;

        public const int PALETTES = 4;



        public WorldMap_FE7_Small(
            Pointer palette,
            Pointer tiles,
            Pointer tsa)
            : base(
                Core.ReadData(palette, PALETTES * Palette.LENGTH),
                Core.ReadData(tiles, 0),
                Core.ReadTSA(tsa, WIDTH, HEIGHT, false, true)) { }

        public WorldMap_FE7_Small(string path, Palette palette = null) : base(WIDTH, HEIGHT)
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
            Palettes = image.Palettes;
            Graphics = image.Graphics;
            Tiling = image.Tiling;
        }
    }



    public class WorldMap_FE7_Large : IDisplayable
    {
        public GBA.Color this[int x, int y]
        {
            get
            {
                int tileX = (x / 8) / 32;
                int tileY = (y / 8) / 32;
                int index = tileX + tileY * 4;
                tileX = (x / 8) % 32;
                tileY = (y / 8) % 32;
                int tile = TSA_Sections[index][tileX, tileY].TileIndex;
                int palette = TSA_Sections[index][tileX, tileY].Palette;
                tileX = (x % 32) % 8;
                tileY = (y % 32) % 8;
                return Palettes[palette][Graphics[index][tile][tileX, tileY]];
            }
            set
            {
                throw new NotImplementedException();
            }
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

        public const int WIDTH = 128;
        public const int HEIGHT = 86;

        public const int PALETTES = 4;
        public const int SECTIONS = 12;

        public Palette[] Palettes { get; set; }
        public Tileset[] Graphics { get; set; }
        public TSA_Array[] TSA_Sections { get; set; }



        public WorldMap_FE7_Large(
            Pointer palette,
            Pointer tiles,
            Pointer tsa)
        {
            Pointer[] pointers;
            Palettes = new Palette[PALETTES];
            for (int i = 0; i < Palettes.Length; i++)
            {
                Palettes[i] = Core.ReadPalette(palette + i * Palette.LENGTH, Palette.LENGTH);
            }

            pointers = GetPointerArray(tiles);
            Graphics = new Tileset[SECTIONS];
            for (int i = 0; i < Graphics.Length; i++)
            {
                Graphics[i] = new Tileset(Core.ReadData(pointers[i], 32 * 32 * Tile.LENGTH));
            }

            pointers = GetPointerArray(tsa);
            TSA_Sections = new TSA_Array[SECTIONS];
            for (int i = 0; i < TSA_Sections.Length; i++)
            {
                TSA_Sections[i] = Core.ReadTSA(pointers[i], 32, 32, false, true);
            }   // sections of 32x32 and 32x22 to make an image thats 128x86 tiles, 1024x688 pixels
        }

        public WorldMap_FE7_Large(string path, Palette palette = null)
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

            Palettes = image.Palettes;
            
            Graphics = new Tileset[SECTIONS];
            TSA_Sections = new TSA_Array[SECTIONS];
            int width = 32;
            int height = 32;
            for (int i = 0; i < SECTIONS; i++)
            {
                if (i == 8) height = 22;

                Graphics[i] = new Tileset(width * height);
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    Graphics[i].Add(image.Graphics[
                        (i % 4 * width + x) +
                        (i / 4 * width + y) * WIDTH]);
                }

                TSA_Sections[i] = TSA_Array.GetBasicTSA(width, height);
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    TSA_Sections[i].SetPalette(x, y, image.Tiling[
                        (i % 4 * width) + x,
                        (i / 4 * width) + y].Palette);
                }
            }
        }
        
        public static Pointer[] GetPointerArray(Pointer address)
        {
            Pointer[] result = new Pointer[SECTIONS];
            for (int i = 0; i < SECTIONS; i++)
            {
                result[i] = Core.ReadPointer(address + i * 4);
            }
            return result;
        }

        /// <summary>
        /// Returns a bitmap of the section number given
        /// </summary>
        public TSA_Image GetSection(int section)
        {
            return new TSA_Image(
                Palette.Merge(Palettes),
                Graphics[section],
                TSA_Sections[section]);
        }
    }
}
