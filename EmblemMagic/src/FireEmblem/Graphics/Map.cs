using System;
using Compression;
using GBA;
using Magic.Components;

namespace EmblemMagic.FireEmblem
{
    public class Map : IDisplayable
    {
        public Color GetColor(Int32 x, Int32 y)
        {
            if (x < 0 || x >= this.Width) throw new ArgumentException("X given is out of bounds: " + x);
            if (y < 0 || y >= this.Height) throw new ArgumentException("Y given is out of bounds: " + y);

            Int32 tileX = x / 16;
            Int32 tileY = y / 16;
            MapTile combo;
            Int32 index = -1;
            for (Int32 i = 0; i < this.ShowChanges.Length; i++)
            {
                if (this.ShowChanges[i] && this.Changes.Contains(i, tileX, tileY))
                    index = i;
            }
            if (index == -1)
            {
                combo = this.Tileset.Tiles[this.Layout[tileX, tileY]];
            }
            else
            {
                Int32 tile_index = this.Changes.GetTile(index, tileX, tileY);
                if (tile_index == 0)
                {
                    combo = this.Tileset.Tiles[this.Layout[tileX, tileY]];
                }
                else
                {
                    combo = this.Tileset.Tiles[tile_index];
                    if (this.ShowChanges_TileBorders && (
                        x % 16 == 0 || x % 16 == 15 ||
                        y % 16 == 0 || y % 16 == 15))
                        return new Color(0x7FFF);
                }
            }
            tileX = x % 16;
            tileY = y % 16;
            Int32 tileIndex;
            Int32 palette;
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
            if (this.ShowFog) palette += 5;
            Tile tile = (this.Tileset.Tileset2 == null) ?
                this.Tileset.Tileset1[tileIndex] :
                (tileIndex < 512) ?
                this.Tileset.Tileset1[tileIndex] :
                this.Tileset.Tileset2[tileIndex - 512];
            return this.Tileset.Palettes[palette][tile[tileX, tileY]];
        }

        public const Int32 PALETTES = 10;

        /// <summary>
        /// The Width of this map, in pixels
        /// </summary>
        public Int32 Width
        {
            get
            {
                return this.WidthTiles * 16;
            }
        }
        /// <summary>
        /// The Height of this map, in pixels
        /// </summary>
        public Int32 Height
        {
            get
            {
                return this.HeightTiles * 16;
            }
        }
        /// <summary>
        /// The Width of this map, in 16x16 tiles
        /// </summary>
        public Byte WidthTiles
        {
            get
            {
                return (Byte)this.Layout.GetLength(0);
            }
            set
            {
                Int32[,] tiles = this.Layout;
                this.Layout = new Int32[value, tiles.GetLength(1)];
                Int32 width  = this.Layout.GetLength(0);
                Int32 height = this.Layout.GetLength(1);
                for (Int32 y = 0; y < height; y++)
                for (Int32 x = 0; x < width; x++)
                {
                    if (x >= tiles.GetLength(0))
                            this.Layout[x, y] = 0;
                    else this.Layout[x, y] = tiles[x, y];
                }
            }
        }
        /// <summary>
        /// The Height of this map, in 16x16 tiles
        /// </summary>
        public Byte HeightTiles
        {
            get
            {
                return (Byte)this.Layout.GetLength(1);
            }
            set
            {
                Int32[,] tiles = this.Layout;
                this.Layout = new Int32[tiles.GetLength(0), value];
                Int32 width  = this.Layout.GetLength(0);
                Int32 height = this.Layout.GetLength(1);
                for (Int32 y = 0; y < height; y++)
                for (Int32 x = 0; x < width; x++)
                {
                    if (y >= tiles.GetLength(1))
                            this.Layout[x, y] = 0;
                    else this.Layout[x, y] = tiles[x, y];
                }
            }
        }

        /// <summary>
        /// The layout of tiles for this map
        /// </summary>
        public Int32[,] Layout { get; set; }
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
        public Boolean ShowFog { get; set; }
        /// <summary>
        /// the number of the tile change to display - 0 is no tile change shown
        /// </summary>
        public Boolean[] ShowChanges { get; set; }
        /// <summary>
        /// If true, shows white borders around tiles that are a result of triggerable map changes
        /// </summary>
        public Boolean ShowChanges_TileBorders { get; set; }



        public Map(MapTileset tileset, Byte[] map_data, Pointer map_changes, Boolean show_changeborders = true)
        {
            this.Layout = new Int32[map_data[0], map_data[1]];
            Int32 x = 0;
            Int32 y = 0;
            for (Int32 i = 2; i < map_data.Length; i += 2)
            {
                this.Layout[x, y] = ((map_data[i] | (map_data[i + 1] << 8)) >> 2 & 0x03FF);
                x++;
                if (x % this.WidthTiles == 0)
                { x = 0; y++; }
            }
            this.Tileset = tileset;

            this.Changes = (map_changes == new Pointer()) ? null : new MapChanges(map_changes);
            this.ShowChanges = new Boolean[(this.Changes == null) ? 0 : this.Changes.Count];
            this.ShowChanges_TileBorders = show_changeborders;
        }



        public Byte[] ToBytes()
        {
            Byte[] result = new Byte[2 + this.WidthTiles * this.HeightTiles * 2];
            result[0] = this.WidthTiles;
            result[1] = this.HeightTiles;
            Int32 x = 0;
            Int32 y = 0;
            for (Int32 i = 2; i < result.Length; i += 2)
            {
                result[i] = (Byte)((this.Layout[x, y] << 2) & 0xFF);
                result[i + 1] = (Byte)(this.Layout[x, y] >> 6);
                x++;
                if (x % this.WidthTiles == 0)
                { x = 0; y++; }
            }
            return LZ77.Compress(result);
        }
    }
}
