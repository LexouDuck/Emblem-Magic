using System;
using GBA;
using Magic;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    public class WorldMap_FE8_Mini : GBA.Sprite
    {
        const int WIDTH = 8;
        const int HEIGHT = 8;

        public WorldMap_FE8_Mini(
            Pointer palette,
            Pointer tileset)
            : base(
                Core.ReadPalette(palette, GBA.Palette.LENGTH),
                new Tileset(Core.ReadData(tileset, 0)),
                GetMap()) { }

        public WorldMap_FE8_Mini(string path, Palette palette = null)
            : base(new Image(path, palette), true) { }



        public static TileMap GetMap()
        {
            int?[,] map = new int?[8, 8];
            int index = 0;
            for (int y = 0; y < HEIGHT; y++)
            for (int x = 0; x < WIDTH; x++)
            {
                map[x, y] = index++;
            }
            return new TileMap(map);
        }
    }



    public class WorldMap_FE8_Small : TSA_Image
    {
        public const int WIDTH = 30;
        public const int HEIGHT = 20;

        public const int PALETTES = 4;



        public WorldMap_FE8_Small(
            Pointer palette,
            Pointer tileset,
            Pointer tsa) : base(
                Core.ReadData(palette, PALETTES * Palette.LENGTH),
                Core.ReadData(tileset, 0),
                Core.ReadTSA(tsa, WIDTH, HEIGHT, true, true)) { }

        public WorldMap_FE8_Small(string path, Palette palette = null) : base(WIDTH, HEIGHT)
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
        public int this[int x, int y]
        {
            get
            {
                int tileX = x / 8;
                int tileY = y / 8;
                int index = tileX + tileY * WIDTH;
                int palette = PaletteMap[tileX, tileY];
                tileX = x % Tile.SIZE;
                tileY = y % Tile.SIZE;
                return palette * Palette.MAX + Graphics[index][tileX, tileY];
            }
        }
        public Color GetColor(int x, int y)
        {
            int tileX = x / 8;
            int tileY = y / 8;
            int index = tileX + tileY * WIDTH;
            int palette = PaletteMap[tileX, tileY];
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

        public const int WIDTH = 60;
        public const int HEIGHT = 40;

        public const int PALETTES = 4;

        public Palette[] Palettes { get; set; }
        public Tileset Graphics { get; set; }
        public int[,] PaletteMap { get; set; }



        public WorldMap_FE8_Large(
            Pointer palette,
            Pointer tileset,
            Pointer tsa)
        {
            Palettes = new Palette[PALETTES];
            for (int i = 0; i < Palettes.Length; i++)
            {
                Palettes[i] = Core.ReadPalette(palette + i * 32, Palette.LENGTH);
            }
            
            Graphics = new Tileset(Core.ReadData(tileset, WIDTH * HEIGHT * Tile.LENGTH));

            byte[] map = Core.ReadData(tsa, 0);
            PaletteMap = new int[WIDTH, HEIGHT];
            int x = 0;
            int y = 0;
            for (int i = 0; i < map.Length; i++)
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

        public WorldMap_FE8_Large(string path, Palette palette = null)
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
            PaletteMap = new int[WIDTH, HEIGHT];
            for (int y = 0; y < HEIGHT; y++)
            for (int x = 0; x < WIDTH; x++)
            {
                PaletteMap[x, y] = image.Tiling[x, y].Palette;
            }
        }



        public Byte[] GetPaletteMap()
        {
            byte[] map = new byte[(WIDTH * HEIGHT) / 2];
            int x = 0;
            int y = 0;
            for (int i = 0; i < map.Length; i++)
            {
                map[i] = (byte)
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
