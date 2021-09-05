using System;
using Compression;
using GBA;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    public class Map : IDisplayable
    {
        public Color GetColor(int x, int y)
        {
            if (x < 0 || x >= Width) throw new ArgumentException("X given is out of bounds: " + x);
            if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

            int tileX = x / 16;
            int tileY = y / 16;
            MapTile combo;
            int index = -1;
            for (int i = 0; i < ShowChanges.Length; i++)
            {
                if (ShowChanges[i] && Changes.Contains(i, tileX, tileY))
                    index = i;
            }
            if (index == -1)
            {
                combo = Tileset.Tiles[Layout[tileX, tileY]];
            }
            else
            {
                int tile_index = Changes.GetTile(index, tileX, tileY);
                if (tile_index == 0)
                {
                    combo = Tileset.Tiles[Layout[tileX, tileY]];
                }
                else
                {
                    combo = Tileset.Tiles[tile_index];
                    if (ShowChanges_TileBorders && (
                        x % 16 == 0 || x % 16 == 15 ||
                        y % 16 == 0 || y % 16 == 15))
                        return new Color(0x7FFF);
                }
            }
            tileX = x % 16;
            tileY = y % 16;
            int tileIndex;
            int palette;
            if (tileX < 8 && tileY < 8)
            {
                palette = combo.Palette_00;
                tileIndex = combo.ComboTile_00;
                if (combo.FlipH_00) tileX = 7 - tileX;
                if (combo.FlipV_00) tileY = 7 - tileY;
            }
            else if (tileX < 8)
            {
                palette = combo.Palette_01;
                tileIndex = combo.ComboTile_01;
                tileY -= 8;
                if (combo.FlipH_01) tileX = 7 - tileX;
                if (combo.FlipV_01) tileY = 7 - tileY;
            }
            else if (tileY < 8)
            {
                palette = combo.Palette_10;
                tileIndex = combo.ComboTile_10;
                tileX -= 8;
                if (combo.FlipH_10) tileX = 7 - tileX;
                if (combo.FlipV_10) tileY = 7 - tileY;
            }
            else
            {
                palette = combo.Palette_11;
                tileIndex = combo.ComboTile_11;
                tileX -= 8;
                tileY -= 8;
                if (combo.FlipH_11) tileX = 7 - tileX;
                if (combo.FlipV_11) tileY = 7 - tileY;
            }
            if (ShowFog) palette += 5;
            Tile tile = (Tileset.Tileset2 == null) ?
                Tileset.Tileset1[tileIndex] :
                (tileIndex < 512) ?
                Tileset.Tileset1[tileIndex] :
                Tileset.Tileset2[tileIndex - 512];
            return Tileset.Palettes[palette][tile[tileX, tileY]];
        }

        public const int PALETTES = 10;

        /// <summary>
        /// The Width of this map, in pixels
        /// </summary>
        public int Width
        {
            get
            {
                return WidthTiles * 16;
            }
        }
        /// <summary>
        /// The Height of this map, in pixels
        /// </summary>
        public int Height
        {
            get
            {
                return HeightTiles * 16;
            }
        }
        /// <summary>
        /// The Width of this map, in 16x16 tiles
        /// </summary>
        public byte WidthTiles
        {
            get
            {
                return (byte)Layout.GetLength(0);
            }
            set
            {
                int[,] tiles = Layout;
                Layout = new int[value, tiles.GetLength(1)];
                int width  = Layout.GetLength(0);
                int height = Layout.GetLength(1);
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (x >= tiles.GetLength(0))
                         Layout[x, y] = 0;
                    else Layout[x, y] = tiles[x, y];
                }
            }
        }
        /// <summary>
        /// The Height of this map, in 16x16 tiles
        /// </summary>
        public byte HeightTiles
        {
            get
            {
                return (byte)Layout.GetLength(1);
            }
            set
            {
                int[,] tiles = Layout;
                Layout = new int[tiles.GetLength(0), value];
                int width  = Layout.GetLength(0);
                int height = Layout.GetLength(1);
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (y >= tiles.GetLength(1))
                         Layout[x, y] = 0;
                    else Layout[x, y] = tiles[x, y];
                }
            }
        }

        /// <summary>
        /// The layout of tiles for this map
        /// </summary>
        public int[,] Layout { get; set; }
        /// <summary>
        /// The triggerable tile changes associated with this map
        /// </summary>
        public MapChanges Changes { get; set; }
        /// <summary>
        /// The set of 16x16 tiles that this map uses
        /// </summary>
        public MapTileset Tileset { get; set; }

        /// <summary>
        /// Whether or not to display this map with its secondary palette
        /// </summary>
        public bool ShowFog { get; set; }
        /// <summary>
        /// the number of the tile change to display - 0 is no tile change shown
        /// </summary>
        public bool[] ShowChanges { get; set; }
        /// <summary>
        /// If true, shows white borders around tiles that are a result of triggerable map changes
        /// </summary>
        public bool ShowChanges_TileBorders { get; set; }



        public Map(MapTileset tileset, byte[] map_data, Pointer map_changes, bool show_changeborders = true)
        {
            Layout = new int[map_data[0], map_data[1]];
            int x = 0;
            int y = 0;
            for (int i = 2; i < map_data.Length; i += 2)
            {
                Layout[x, y] = ((map_data[i] | (map_data[i + 1] << 8)) >> 2 & 0x03FF);
                x++;
                if (x % WidthTiles == 0)
                { x = 0; y++; }
            }
            Tileset = tileset;

            Changes = (map_changes == new Pointer()) ? null : new MapChanges(map_changes);
            ShowChanges = new bool[(Changes == null) ? 0 : Changes.Count];
            ShowChanges_TileBorders = show_changeborders;
        }



        public Byte[] ToBytes()
        {
            byte[] result = new byte[2 + WidthTiles * HeightTiles * 2];
            result[0] = WidthTiles;
            result[1] = HeightTiles;
            int x = 0;
            int y = 0;
            for (int i = 2; i < result.Length; i += 2)
            {
                result[i] = (byte)((Layout[x, y] << 2) & 0xFF);
                result[i + 1] = (byte)(Layout[x, y] >> 6);
                x++;
                if (x % WidthTiles == 0)
                { x = 0; y++; }
            }
            return LZ77.Compress(result);
        }
    }
}
