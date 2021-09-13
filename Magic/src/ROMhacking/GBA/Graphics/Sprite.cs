using Magic.Components;
using Magic;
using System;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// The GBA.Sprite is an image drawn by a GBA.TileMap mapping of 8x8 tiles - it can be flipped or rotated/transformed
    /// </summary>
    public class Sprite : IDisplayable
    {
        /// <summary>
        /// This indexer allows for fast access to pixel data in GBA.Color format - for IDisplayable
        /// </summary>
        public Int32 this[Int32 x, Int32 y]
        {
            get
            {
                if (Transform == null)
                {
                    if (x < 0 || x >= Width) throw new ArgumentException("X given is out of bounds: " + x);
                    if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                    if (FlipH) x = (Width - x - 1);
                    if (FlipV) y = (Height - y - 1);

                    Int32 tileX = x / Tile.SIZE;
                    Int32 tileY = y / Tile.SIZE;

                    Int32 index = Tiles[tileX, tileY];
                    if (index < 0 || index >= Sheet.Count)
                        return 0;

                    tileX = x % Tile.SIZE;
                    tileY = y % Tile.SIZE;

                    return Sheet[index][tileX, tileY];
                }
                else return Transform[x, y];
            }
        }
        public Color GetColor(Int32 x, Int32 y)
        {
            return (Colors[this[x, y]]);
        }
        
        /// <summary>
        /// Gets the width of this sprite, in pixels.
        /// </summary>
        public Int32 Width
        {
            get
            {
                return (Transform == null) ? Tiles.Width * 8 : Transform.Width;
            }
        }
        /// <summary>
        /// Gets the height of this sprite, in pixels.
        /// </summary>
        public Int32 Height
        {
            get
            {
                return (Transform == null) ? Tiles.Height * 8 : Transform.Height;
            }
        }



        /// <summary>
        /// Is null if the sprite is normally rendered - otherwise is a baked image of the transformed sprite
        /// </summary>
        public Bitmap Transform = null;

        /// <summary>
        /// Whether or not this Sprite is to show up flipped horizontally
        /// </summary>
        public Boolean FlipH { get; private set; }
        /// <summary>
        /// Whether or not this Sprite is to show up flipped vertically
        /// </summary>
        public Boolean FlipV { get; private set; }

        /// <summary>
        /// The Palette associated with this GBA Sprite
        /// </summary>
        public Palette Colors { get; private set; }
        /// <summary>
        /// The sheet of tiles that are used to make up this GBA.Sprite
        /// </summary>
        public Tileset Sheet { get; private set; }
        /// <summary>
        /// The tiling data for this sprite.
        /// </summary>
        public TileMap Tiles { get; private set; }



        /// <summary>
        /// Creates a GBA.Sprite from the given GBA.Palette, GBA.Tileset and GBA.Tilemap
        /// </summary>
        public Sprite(Palette palette, Tileset tileset, TileMap tilemap)
        {
            Load(palette, tileset, tilemap, false, false);
        }
        /// <summary>
        /// Creates a sprite from a block of OAM data
        /// </summary>
        public Sprite(Palette palette, Tileset tileset, OAM oam)
        {
            Load(palette, tileset, new TileMap(oam.GetTileMap(oam.GetDimensions())), oam.FlipH, oam.FlipV);
        }
        /// <summary>
        /// Creates an affine/transformed sprite from a block of OAM data and the affine data with it
        /// </summary>
        public Sprite(Palette palette, Tileset tileset, OAM oam, OAM_Affine transform)
        {
            Size size = oam.GetDimensions();
            Load(palette, tileset, new TileMap(oam.GetTileMap(size)), false, false);

            Int32 width  = 8 * size.Width;
            Int32 height = 8 * size.Height;
            if (oam.OBJMode == OAM_OBJMode.BigAffine)
            {
                width  *= 2;
                height *= 2;
            }
            Byte[] data = new Byte[width * height];
            Point tile = new();
            Point affine = new();
            Int32 halfW = width / 2;
            Int32 halfH = height / 2;
            for (Int32 y = -halfH; y < halfH; y++)
            for (Int32 x = -halfW; x < halfW; x++)
            {
                affine.X = (Int32)(transform.Ux * x + transform.Uy * y) + size.Width * 4;
                affine.Y = (Int32)(transform.Vx * x + transform.Vy * y) + size.Height * 4;
                if (affine.X < 0 || affine.X >= size.Width * 8) continue;
                if (affine.Y < 0 || affine.Y >= size.Height * 8) continue;

                tile.X = affine.X / Tile.SIZE;
                tile.Y = affine.Y / Tile.SIZE;
                    Int32 index = Tiles[tile.X, tile.Y];
                if (index < 0 || index >= Sheet.Count) continue;
                tile.X = affine.X % Tile.SIZE;
                tile.Y = affine.Y % Tile.SIZE;

                data[(halfW + x) + (halfH + y) * width] = (Byte)Sheet[index][tile.X, tile.Y];
            }
            Transform = new Bitmap(width, height, palette.ToBytes(false), data);
        }
        /// <summary>
        /// Creates a GBA.Sprite from a GBA.Image, making a GBA.Tilemap by checking with the given GBA.Tileset
        /// </summary>
        public Sprite(Image image, Boolean addAllTiles)
        {
            Int32 width = image.Width / Tile.SIZE;
            Int32 height = image.Height / Tile.SIZE;

            Tile tile;
            Tileset tileset = new Tileset();
            Byte[] buffer = new Byte[Tile.LENGTH];
            Int32?[,] map = new Int32?[width, height];
            Int32 index = 0;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                index = (x * 4) + y * (width * 4 * 8);

                for (Int32 i = 0; i < 8; i++)
                {
                    Array.Copy(image.Bytes, index + i * (width * 4), buffer, i * 4, 4);
                }
                tile = new Tile(buffer);

                index = tileset.Find(tile);

                if (addAllTiles || index == -1)
                {
                    index = tileset.Count;
                    tileset.Add(tile);
                }
                map[x, y] = index;
            }

            Load(new Palette(image.Colors), tileset, new TileMap(map), false, false);
        }
        /// <summary>
        /// Initializes the fields of this class
        /// </summary>
        void Load(Palette palette, Tileset tileset, TileMap tilemap, Boolean flipH, Boolean flipV)
        {
            Colors = palette;
            Sheet = tileset;
            Tiles = tilemap;
            FlipH = flipH;
            FlipV = flipV;
        }



        /// <summary>
        /// Returns an array of pixels of this sprite (so, without tiling information)
        /// </summary>
        public Color[,] GetPixels()
        {
            Color[,] result = new Color[Width, Height];
            Color[,] tile = new Color[Tile.SIZE, Tile.SIZE];

            for (Int32 iy = 0; iy < Height; iy += Tile.SIZE)
            for (Int32 ix = 0; ix < Width ; ix += Tile.SIZE)
            {
                tile = (Tiles[ix, iy] < 0) ?
                    Tile.Empty.GetPixels(Colors) :
                    Sheet[Tiles[ix, iy]].GetPixels(Colors);

                for (Int32 y = 0; y < Tile.SIZE; y++)
                for (Int32 x = 0; x < Tile.SIZE; x++)
                {
                    result[ix + x, iy + y] = tile[x, y];
                }
            }
            return result;
        }



        /// <summary>
        /// An empty Sprite
        /// </summary>
        public static Sprite Empty = new Sprite(new Palette(), new Tileset(), new TileMap(0, 0));
    }
}
