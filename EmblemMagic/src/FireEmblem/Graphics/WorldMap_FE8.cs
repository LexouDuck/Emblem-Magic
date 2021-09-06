using System;
using GBA;
using Magic;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    public class WorldMap_FE8_Mini : GBA.Sprite
    {
        const Int32 WIDTH = 8;
        const Int32 HEIGHT = 8;

        public WorldMap_FE8_Mini(
            Pointer palette,
            Pointer tileset)
            : base(
                Core.ReadPalette(palette, GBA.Palette.LENGTH),
                new Tileset(Core.ReadData(tileset, 0)),
                GetMap()) { }

        public WorldMap_FE8_Mini(String path, Palette palette = null)
            : base(new Image(path, palette), true) { }



        public static TileMap GetMap()
        {
            Int32?[,] map = new Int32?[8, 8];
            Int32 index = 0;
            for (Int32 y = 0; y < HEIGHT; y++)
            for (Int32 x = 0; x < WIDTH; x++)
            {
                map[x, y] = index++;
            }
            return new TileMap(map);
        }
    }



    public class WorldMap_FE8_Small : TSA_Image
    {
        public const Int32 WIDTH = 30;
        public const Int32 HEIGHT = 20;

        public const Int32 PALETTES = 4;



        public WorldMap_FE8_Small(
            Pointer palette,
            Pointer tileset,
            Pointer tsa) : base(
                Core.ReadData(palette, PALETTES * Palette.LENGTH),
                Core.ReadData(tileset, 0),
                Core.ReadTSA(tsa, WIDTH, HEIGHT, true, true)) { }

        public WorldMap_FE8_Small(String path, Palette palette = null) : base(WIDTH, HEIGHT)
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
            Graphics = image.Graphics;
            Palettes = image.Palettes;
            Tiling = image.Tiling;
        }
    }



    public class WorldMap_FE8_Large : IDisplayable
    {
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                Int32 tileX = x / 8;
                Int32 tileY = y / 8;
                Int32 index = tileX + tileY * WIDTH;
                Int32 palette = PaletteMap[tileX, tileY];
                tileX = x % Tile.SIZE;
                tileY = y % Tile.SIZE;
                return palette * Palette.MAX + Graphics[index][tileX, tileY];
            }
        }
        public Color GetColor(Int32 x, Int32 y)
        {
            Int32 tileX = x / 8;
            Int32 tileY = y / 8;
            Int32 index = tileX + tileY * WIDTH;
            Int32 palette = PaletteMap[tileX, tileY];
            tileX = x % Tile.SIZE;
            tileY = y % Tile.SIZE;
            return Palettes[palette][Graphics[index][tileX, tileY]];
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

        public const Int32 WIDTH = 60;
        public const Int32 HEIGHT = 40;

        public const Int32 PALETTES = 4;

        public Palette[] Palettes { get; set; }
        public Tileset Graphics { get; set; }
        public Int32[,] PaletteMap { get; set; }



        public WorldMap_FE8_Large(
            Pointer palette,
            Pointer tileset,
            Pointer tsa)
        {
            Palettes = new Palette[PALETTES];
            for (Int32 i = 0; i < Palettes.Length; i++)
            {
                Palettes[i] = Core.ReadPalette(palette + i * 32, Palette.LENGTH);
            }
            
            Graphics = new Tileset(Core.ReadData(tileset, WIDTH * HEIGHT * Tile.LENGTH));

            Byte[] map = Core.ReadData(tsa, 0);
            PaletteMap = new Int32[WIDTH, HEIGHT];
            Int32 x = 0;
            Int32 y = 0;
            for (Int32 i = 0; i < map.Length; i++)
            {
                PaletteMap[x++, y] = (map[i] & 0x0F);
                PaletteMap[x++, y] = (map[i] & 0xF0) >> 4;
                if (x % WIDTH == 0)
                {
                    i += 2;
                    x = 0;
                    y++;
                }
            }
        }

        public WorldMap_FE8_Large(String path, Palette palette = null)
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
            Graphics = image.Graphics;
            Palettes = image.Palettes;
            PaletteMap = new Int32[WIDTH, HEIGHT];
            for (Int32 y = 0; y < HEIGHT; y++)
            for (Int32 x = 0; x < WIDTH; x++)
            {
                PaletteMap[x, y] = image.Tiling[x, y].Palette;
            }
        }



        public Byte[] GetPaletteMap()
        {
            Byte[] map = new Byte[(WIDTH * HEIGHT) / 2];
            Int32 x = 0;
            Int32 y = 0;
            for (Int32 i = 0; i < map.Length; i++)
            {
                map[i] = (Byte)
                    ((PaletteMap[x++, y] & 0x0F) &
                    ((PaletteMap[x++, y] << 4) & 0xF0));

                if (x % WIDTH == 0)
                {
                    i += 2;
                    x = 0;
                    y++;
                }
            }
            return map;
        }
    }
}
