using EmblemMagic.Components;
using EmblemMagic;
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
        public Color this[int x, int y]
        {
            get
            {
                if (Transform == null)
                {
                    if (x < 0 || x >= Width)  throw new ArgumentException("X given is out of bounds: " + x);
                    if (y < 0 || y >= Height) throw new ArgumentException("Y given is out of bounds: " + y);

                    if (FlipH) x = (Width - x - 1);
                    if (FlipV) y = (Height - y - 1);

                    int tileX = x / Tile.SIZE;
                    int tileY = y / Tile.SIZE;

                    int index = Tiles[tileX, tileY];
                    if (index < 0 || index >= Sheet.Count)
                        return Colors[0];

                    tileX = x % Tile.SIZE;
                    tileY = y % Tile.SIZE;

                    return Colors[Sheet[index][tileX, tileY]];
                }
                else return Transform[x, y];
            }
        }
        
        /// <summary>
        /// Gets the width of this sprite, in pixels.
        /// </summary>
        public int Width
        {
            get
            {
                return (Transform == null) ? Tiles.Width * 8 : Transform.Width;
            }
        }
        /// <summary>
        /// Gets the height of this sprite, in pixels.
        /// </summary>
        public int Height
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
        public bool FlipH { get; private set; }
        /// <summary>
        /// Whether or not this Sprite is to show up flipped vertically
        /// </summary>
        public bool FlipV { get; private set; }

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

            int width = (oam.OBJMode == OAM_OBJMode.BigAffine) ? size.Width * 16 : size.Width * 8;
            int height = (oam.OBJMode == OAM_OBJMode.BigAffine) ? size.Height * 16: size.Height * 8;
            byte[] data = new byte[width * height];
            int tileX, tileY;
            int affX, affY;
            int halfW = width / 2;
            int halfH = height / 2;
            for (int y = halfH * -1; y < halfH; y++)
            for (int x = halfW * -1; x < halfW; x++)
            {
                affX = (int)(transform.Ux * x + transform.Vx * y) + size.Width * 4;
                affY = (int)(transform.Uy * x + transform.Vy * y) + size.Height * 4;
                if (affX < 0 || affX >= size.Width * 8) continue;
                if (affY < 0 || affY >= size.Height * 8) continue;

                tileX = affX / Tile.SIZE;
                tileY = affY / Tile.SIZE;
                int index = Tiles[tileX, tileY];
                if (index < 0 || index >= Sheet.Count) continue;
                tileX = affX % Tile.SIZE;
                tileY = affY % Tile.SIZE;

                data[(halfW + x) + (halfH + y) * width] = (byte)Sheet[index][tileX, tileY];
            }
            Transform = new Bitmap(width, height, palette.ToBytes(false), data);
        }
        /// <summary>
        /// Creates a GBA.Sprite from a GBA.Image, making a GBA.Tilemap by checking with the given GBA.Tileset
        /// </summary>
        public Sprite(Image image, bool addAllTiles)
        {
            int width = image.Width / Tile.SIZE;
            int height = image.Height / Tile.SIZE;

            Tile tile;
            Tileset tileset = new Tileset();
            byte[] buffer = new byte[Tile.LENGTH];
            int?[,] map = new int?[width, height];
            int index = 0;
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                index = (x * 4) + y * (width * 4 * 8);

                for (int i = 0; i < 8; i++)
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
        void Load(Palette palette, Tileset tileset, TileMap tilemap, bool flipH, bool flipV)
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

            for (int iy = 0; iy < Height; iy += Tile.SIZE)
            for (int ix = 0; ix < Width ; ix += Tile.SIZE)
            {
                tile = (Tiles[ix, iy] < 0) ?
                    Tile.Empty.GetPixels(Colors) :
                    Sheet[Tiles[ix, iy]].GetPixels(Colors);

                for (int y = 0; y < Tile.SIZE; y++)
                for (int x = 0; x < Tile.SIZE; x++)
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
