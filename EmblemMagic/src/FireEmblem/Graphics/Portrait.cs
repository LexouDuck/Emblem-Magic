using System;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public enum PortraitType
    {
        None = 0,
        Portrait = 1,
        Generic = 2,
        Shop = 3
    }

    /// <summary>
    /// Represents a character portrait (is a GBA.SpriteSheet containing the several parts that make up a portrait)
    /// </summary>
    public class Portrait : GBA.SpriteSheet
    {
        public const Int32 MAIN = 0;
        public const Int32 CHIBI = 1;
        public const Int32 MOUTH = 2;

        public const Int32 WIDTH = 16;
        public const Int32 HEIGHT = 14;

        public static Int32 Face_Width  = (Core.App.Game is FE6) ? 8 : 32;
        public static Int32 Face_Height = (Core.App.Game is FE6) ? 17 : 4;
        public static Int32 Face_Length = Face_Width * Face_Height * Tile.LENGTH;
        public static Int32 Card_Width  = 10;
        public static Int32 Card_Height = 9;
        public static Int32 Card_Length = Card_Width * Card_Height * Tile.LENGTH;
        public static Int32 Chibi_Width  = 4;
        public static Int32 Chibi_Height = 4;
        public static Int32 Chibi_Length = Chibi_Width * Chibi_Height * Tile.LENGTH;
        public static Int32 Mouth_Width  = 12;
        public static Int32 Mouth_Height = 4;
        public static Int32 Mouth_Length = Mouth_Width * Mouth_Height * Tile.LENGTH;



        /// <summary>
        /// The type of portrait this is
        /// </summary>
        public PortraitType Type { get; set; }
        /// <summary>
        /// Gets the 16-color palette for this portrait
        /// </summary>
        public Palette Colors
        {
            get
            {
                return Sprites[MAIN].Colors;
            }
        }



        /// <summary>
        /// Creates a regular fire emblem portrait from the given data
        /// </summary>
        public Portrait(Palette palette, Byte[] face, Byte[] chibi, Byte[] mouth)
            : base(WIDTH * Tile.SIZE, HEIGHT * Tile.SIZE)
        {
            if (palette == null) throw new Exception("Palette given is null.");
            if (face == null) throw new Exception("Portrait main image is null.");
            
            Type = PortraitType.Portrait;
            AddSprite(new Sprite(palette,
                new Tileset(face),
                new TileMap(Map_Face())), 0, 0);

            if (chibi == null)
            {
                Type = (Core.App.Game is FE6) ?
                    PortraitType.Generic :
                    PortraitType.Shop;
                AddSprite(Sprite.Empty, 0, 0);
            }
            else
            {
                AddSprite(new Sprite(palette,
                    new Tileset(chibi),
                    new TileMap(Map_Chibi())), 12 * Tile.SIZE, 2 * Tile.SIZE);
            }

            if (mouth == null)
            {
                if (!(Core.App.Game is FE6))
                    Type = PortraitType.Generic;
                AddSprite(Sprite.Empty, 0, 0);
            }
            else
            {
                AddSprite(new Sprite(palette,
                    new Tileset(mouth),
                    new TileMap(Map_Mouth())), 0 * Tile.SIZE, 10 * Tile.SIZE);
            }
        }
        /// <summary>
        /// Creates a generic class card Portrait from the given palette and tile data
        /// </summary>
        public Portrait(Palette palette, Byte[] card)
            : base(Card_Width * GBA.Tile.SIZE, Card_Height * GBA.Tile.SIZE)
        {
            if (card == null) throw new Exception("Portrait main image is null");

            Type = PortraitType.Generic;
            AddSprite(new Sprite(palette, new Tileset(card), new TileMap(Map_Card())), 0, 0);
        }
        /// <summary>
        /// Creates a FireEmblem.Portrait from the given image.
        /// </summary>
        public Portrait(Image image, Boolean generic)
            : base((generic) ? Card_Width * Tile.SIZE : WIDTH * Tile.SIZE,
                  (generic) ? Card_Height * Tile.SIZE : HEIGHT * Tile.SIZE)
        {
            if ((image.Width  / Tile.SIZE) != (generic ? Card_Width : WIDTH) ||
                (image.Height / Tile.SIZE) != (generic ? Card_Height : HEIGHT))
                throw new Exception("Image given has invalid dimensions: should be " + (generic ?
                    Card_Width * Tile.SIZE + "x" + Card_Height * Tile.SIZE + " pixels" :
                    WIDTH * Tile.SIZE + "x" + HEIGHT * Tile.SIZE + " pixels"));

            Tileset tileset;
            if (generic)
            {
                tileset = new Tileset(); tileset.Parse(image, Map_Card());
                AddSprite(new Sprite(image.Colors, tileset, new TileMap(Map_Card())), 0, 0);
            }
            else
            {
                tileset = new Tileset(); tileset.Parse(image, Map_Face());
                AddSprite(new Sprite(image.Colors, tileset, new TileMap(Map_Face())), 0, 0);

                tileset = new Tileset(); tileset.Parse(image, TileMap.Place(Map_Chibi(), 12, 2, WIDTH, HEIGHT));
                AddSprite(new Sprite(image.Colors, tileset, new TileMap(Map_Chibi())), 12 * 8, 2 * 8);

                if (Core.App.Game is FE6)
                {
                    AddSprite(Sprite.Empty, 0, 0);
                }
                else
                {
                    tileset = new Tileset(); tileset.Parse(image, TileMap.Place(Map_Mouth(), 0, 10, WIDTH, HEIGHT));
                    AddSprite(new Sprite(image.Colors, tileset, new TileMap(Map_Mouth())), 0 * 8, 10 * 8);
                }
            }
        }



        public static Int32?[,] Map_Test(Boolean generic)
        {
            if (generic)
            {
                return TileMap.Convert(new Int32?[9, 10]
                {
                    { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 },
                    { 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13 },
                    { 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D },
                    { 0x1E, 0x1F, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27 },
                    { 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31 },
                    { 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B },
                    { 0x3C, 0x3D, 0x3E, 0x3F, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45 },
                    { 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                    { 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59 }
                }, 0);
            }
            else return TileMap.Convert(new Int32?[10, 12]
            {
                { null, null, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, null, null },
                { null, null, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, null, null },
                { null, null, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, null, null },
                { null, null, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, null, null },
                { null, null, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, null, null },
                { null, null, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, null, null },
                { 0x14, 0x15, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x16, 0x17 },
                { 0x34, 0x35, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x36, 0x37 },
                { 0x54, 0x55, 0x10, 0x11, 0x12, 0x13, 0x50, 0x51, 0x52, 0x53, 0x56, 0x57 },
                { 0x74, 0x75, 0x30, 0x31, 0x32, 0x33, 0x70, 0x71, 0x72, 0x73, 0x76, 0x77 },
            }, 0);
        }
        static Int32?[,] Map_Face()
        {
            if (Program.Core.Game is FE6)
            {
                return TileMap.Convert(new Int32?[14, 16]
                {
                    { null, null, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, null, null, null, null, null, null },
                    { null, null, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, null, null, null, null, null, null },
                    { null, null, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, null, null, null, null, null, null },
                    { null, null, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, null, null, null, null, null, null },
                    { null, null, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, null, null, null, null, null, null },
                    { null, null, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, null, null, null, null, null, null },
                    { 0x14, 0x15, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x16, 0x17, null, null, null, null },
                    { 0x34, 0x35, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x36, 0x37, null, null, null, null },
                    { 0x54, 0x55, 0x10, 0x11, 0x12, 0x13, 0x50, 0x51, 0x52, 0x53, 0x56, 0x57, null, null, null, null },
                    { 0x74, 0x75, 0x30, 0x31, 0x32, 0x33, 0x70, 0x71, 0x72, 0x73, 0x76, 0x77, null, null, null, null },
                    { 0x1C, 0x1D, 0x1E, 0x1F, 0x5C, 0x5D, 0x5E, 0x5F, null, null, null, null, 0x80, 0x81, 0x82, 0x83 },
                    { 0x3C, 0x3D, 0x3E, 0x3F, 0x7C, 0x7D, 0x7E, 0x7F, null, null, null, null, 0x84, 0x85, 0x86, 0x87 },
                    { 0x18, 0x19, 0x1A, 0x1B, 0x58, 0x59, 0x5A, 0x5B, null, null, null, null, null, null, null, null },
                    { 0x38, 0x39, 0x3A, 0x3B, 0x78, 0x79, 0x7A, 0x7B, null, null, null, null, null, null, null, null }
                }, 0);
            }
            else
            {
                return TileMap.Convert(new Int32?[14, 16]
                {
                    { null, null, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, null, null, null, null, null, null },
                    { null, null, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, null, null, null, null, null, null },
                    { null, null, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, null, null, null, null, null, null },
                    { null, null, 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, null, null, null, null, null, null },
                    { null, null, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, null, null, null, null, null, null },
                    { null, null, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, null, null, null, null, null, null },
                    { 0x14, 0x15, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B },
                    { 0x34, 0x35, 0x68, 0x69, 0x6A, 0x6B, 0x6C, 0x6D, 0x6E, 0x6F, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B },
                    { 0x54, 0x55, 0x10, 0x11, 0x12, 0x13, 0x50, 0x51, 0x52, 0x53, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B },
                    { 0x74, 0x75, 0x30, 0x31, 0x32, 0x33, 0x70, 0x71, 0x72, 0x73, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B },
                    { null, null, null, null, null, null, null, null, null, null, null, null, 0x1C, 0x1D, 0x1E, 0x1F },
                    { null, null, null, null, null, null, null, null, null, null, null, null, 0x3C, 0x3D, 0x3E, 0x3F },
                    { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
                    { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null }
                }, 0);
            }
        }
        static Int32?[,] Map_Chibi()
        {
            return TileMap.Convert(new Int32?[4, 4]
            {
                { 0x00, 0x01, 0x02, 0x03 },
                { 0x04, 0x05, 0x06, 0x07 },
                { 0x08, 0x09, 0x0A, 0x0B },
                { 0x0C, 0x0D, 0x0E, 0x0F },
            }, 0);
        }
        static Int32?[,] Map_Mouth()
        {
            return TileMap.Convert(new Int32?[4, 12] {
                { 0x00, 0x01, 0x02, 0x03, 0x08, 0x09, 0x0A, 0x0B, 0x10, 0x11, 0x12, 0x13 },
                { 0x04, 0x05, 0x06, 0x07, 0x0C, 0x0D, 0x0E, 0x0F, 0x14, 0x15, 0x16, 0x17 },
                { 0x18, 0x19, 0x1A, 0x1B, 0x20, 0x21, 0x22, 0x23, 0x28, 0x29, 0x2A, 0x2B },
                { 0x1C, 0x1D, 0x1E, 0x1F, 0x24, 0x25, 0x26, 0x27, 0x2C, 0x2D, 0x2E, 0x2F },
            }, 0);
        }
        static Int32?[,] Map_Card()
        {
            return TileMap.Convert(new Int32?[9, 10]
            {
                { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 },
                { 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13 },
                { 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D },
                { 0x1E, 0x1F, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27 },
                { 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F, 0x30, 0x31 },
                { 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B },
                { 0x3C, 0x3D, 0x3E, 0x3F, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45 },
                { 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F },
                { 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59 }
            }, 0);
        }
    }
}
