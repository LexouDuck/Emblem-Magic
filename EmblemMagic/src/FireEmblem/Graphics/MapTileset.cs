using System;
using System.Collections.Generic;
using Compression;
using GBA;
using Magic;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    /// <summary>
    /// Represents a 16x16 combo tile for maps
    /// </summary>
    public struct MapTile
    {
        public int ComboTile_00; public int Palette_00; public bool FlipH_00; public bool FlipV_00;
        public int ComboTile_01; public int Palette_01; public bool FlipH_01; public bool FlipV_01;
        public int ComboTile_10; public int Palette_10; public bool FlipH_10; public bool FlipV_10;
        public int ComboTile_11; public int Palette_11; public bool FlipH_11; public bool FlipV_11;

        public MapTile(TSA tsa_00, TSA tsa_01, TSA tsa_10, TSA tsa_11)
        {
            ComboTile_00 = (tsa_00.Value & TSA.BITS_TILE);
            ComboTile_01 = (tsa_01.Value & TSA.BITS_TILE);
            ComboTile_10 = (tsa_10.Value & TSA.BITS_TILE);
            ComboTile_11 = (tsa_11.Value & TSA.BITS_TILE);

            Palette_00 = (tsa_00.Value & TSA.BITS_PALETTE) >> 12;
            Palette_01 = (tsa_01.Value & TSA.BITS_PALETTE) >> 12;
            Palette_10 = (tsa_10.Value & TSA.BITS_PALETTE) >> 12;
            Palette_11 = (tsa_11.Value & TSA.BITS_PALETTE) >> 12;

            FlipH_00 = (tsa_00.Value & TSA.BITS_FLIPH) != 0;
            FlipH_01 = (tsa_01.Value & TSA.BITS_FLIPH) != 0;
            FlipH_10 = (tsa_10.Value & TSA.BITS_FLIPH) != 0;
            FlipH_11 = (tsa_11.Value & TSA.BITS_FLIPH) != 0;

            FlipV_00 = (tsa_00.Value & TSA.BITS_FLIPV) != 0;
            FlipV_01 = (tsa_01.Value & TSA.BITS_FLIPV) != 0;
            FlipV_10 = (tsa_10.Value & TSA.BITS_FLIPV) != 0;
            FlipV_11 = (tsa_11.Value & TSA.BITS_FLIPV) != 0;
        }

        public byte[] ToBytes()
        {
            byte[] result = new byte[8];

            result[0] = (byte)(ComboTile_00 & 0xFF);
            result[1] = (byte)(ComboTile_00 >> 8);
            result[1] |= (byte)((FlipH_00 ? 1 : 0) << 2);
            result[1] |= (byte)((FlipV_00 ? 1 : 0) << 3);
            result[1] |= (byte)(Palette_00 << 4);

            result[2] = (byte)(ComboTile_01 & 0xFF);
            result[3] = (byte)(ComboTile_01 >> 8);
            result[3] |= (byte)((FlipH_01 ? 1 : 0) << 2);
            result[3] |= (byte)((FlipV_01 ? 1 : 0) << 3);
            result[3] |= (byte)(Palette_01 << 4);

            result[4] = (byte)(ComboTile_10 & 0xFF);
            result[5] = (byte)(ComboTile_10 >> 8);
            result[5] |= (byte)((FlipH_10 ? 1 : 0) << 2);
            result[5] |= (byte)((FlipV_10 ? 1 : 0) << 3);
            result[5] |= (byte)(Palette_10 << 4);

            result[6] = (byte)(ComboTile_11 & 0xFF);
            result[7] = (byte)(ComboTile_11 >> 8);
            result[7] |= (byte)((FlipH_11 ? 1 : 0) << 2);
            result[7] |= (byte)((FlipV_11 ? 1 : 0) << 3);
            result[7] |= (byte)(Palette_11 << 4);

            return result;
        }
    }

    /// <summary>
    /// A tileset of 16x16 tiles made from two sub-tilesets and TSA
    /// </summary>
    public class MapTileset : IDisplayable
    {
        public int this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                int tileX = x / 16;
                int tileY = y / 16;
                MapTile combo = Tiles[tileX + tileY * 32];

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
                Tile tile = (Tileset2 == null) ?
                    Tileset1[tileIndex] :
                    (tileIndex < 512) ?
                    Tileset1[tileIndex] :
                    Tileset2[tileIndex - 512];
                return palette * Palette.MAX + tile[tileX, tileY];
            }
        }
        public Color GetColor(int x, int y)
        {
            int color = this[x, y];
            return Palettes[color / Palette.MAX][color % Palette.MAX];
        }

        public const int WIDTH = 2;
        public const int HEIGHT = 2048;

        /// <summary>
        /// The Width of this map tileset, in pixels (is always 512)
        /// </summary>
        public int Width
        {
            get
            {
                return 512;
            }
        }
        /// <summary>
        /// The Height of this map tileset, in pixels (is always 512)
        /// </summary>
        public int Height
        {
            get
            {
                return 512;
            }
        }

        /// <summary>
        /// The list of 16-color Palettes used for this map/tileset
        /// </summary>
        public Palette[] Palettes { get; }
        /// <summary>
        /// The main 32x32-tile sub-tileset whose 8x8 tiles are used to make up 16x16 MapTiles
        /// </summary>
        public Tileset Tileset1 { get; }
        /// <summary>
        /// The secondary sub-tileset (only used in FE6 as far as i know..?)
        /// </summary>
        public Tileset Tileset2 { get; }
        /// <summary>
        /// The list of 16x16 combo tiles this map tileset holds
        /// </summary>
        public List<MapTile> Tiles { get; }
        /// <summary>
        /// The array detailing which tile is to be interpreted as which terrain type
        /// </summary>
        public byte[] Terrain { get; }



        public MapTileset(byte[] palettes, byte[] tileset1, byte[] tileset2, byte[] tsa_terrain)
        {
            Palettes = new Palette[palettes.Length / Palette.LENGTH];
            byte[] buffer = new byte[Palette.LENGTH];
            for (int p = 0; p < Palettes.Length; p++)
            {
                Array.Copy(palettes, p * Palette.LENGTH, buffer, 0, Palette.LENGTH);
                Palettes[p] = new Palette(buffer);
            }
            if (tileset2 == null || tileset2.Length == 0)
            {
                Tileset1 = new Tileset(tileset1, 1024);
                Tileset2 = null;
            }
            else
            {
                Tileset1 = new Tileset(tileset1, 512);
                Tileset2 = new Tileset(tileset2, 512);
            }
            Terrain = tsa_terrain.GetBytes(WIDTH * HEIGHT * 2);
            Tiles = new List<MapTile>();
            TSA_Array tsa = new TSA_Array(WIDTH, HEIGHT, tsa_terrain);
            int length = 32 * 32;
            for (int i = 0; i < length; i++)
            {
                Tiles.Add(new MapTile(
                    tsa[0, i * 2],
                    tsa[0, i * 2 + 1],
                    tsa[1, i * 2],
                    tsa[1, i * 2 + 1]));
            }
        }
        public MapTileset(Palette palette, Bitmap tileset, Bitmap tileset1, Bitmap tileset2)
        {
            TSA_Image maptileset;
            if (palette == null)
            {
                maptileset = new TSA_Image(64, 64, tileset, 4, true);
                Palettes = new Palette[8];
                for (int i = 0; i < 4; i++)
                {
                    Palettes[i] = maptileset.Palettes[i];
                }
                for (int i = 0; i < 4; i++)
                {
                    Palettes[4 + i] = new Palette();
                    Color color;
                    for (int j = 0; j < Palette.MAX; j++)
                    {
                        color = maptileset.Palettes[i][j];
                        Palettes[4 + i].Add(new GBA.Color(0,
                            (byte)(color.GetValueR() + 32),
                            (byte)(color.GetValueG() + 32),
                            (byte)(color.GetValueB() + 32)));
                    }
                }
            }
            else
            {
                maptileset = new TSA_Image(64, 64, tileset, palette, 8, true);
                Palettes = Palette.Split(palette, 8);
            }

            if (tileset1 == null) Tileset1 = maptileset.Graphics;
            else
            {
                
            }
            if (tileset2 == null)
            {

            }
            else
            {

            }
        }

        public byte[] GetTSAandTerrain(bool compressed)
        {
            List<byte> tsa = new List<byte>();
            foreach (MapTile tile in Tiles)
            {
                tsa.AddRange(tile.ToBytes());
            }
            byte[] result = new byte[tsa.Count + Terrain.Length];
            Array.Copy(tsa.ToArray(), result, tsa.Count);
            Array.Copy(Terrain, 0, result, tsa.Count, Terrain.Length);
            return compressed ? LZ77.Compress(result) : result;
        }
    }
}
