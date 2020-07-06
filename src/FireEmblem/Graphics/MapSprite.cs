using Compression;
using GBA;
using System;
using System.Drawing;

namespace EmblemMagic.FireEmblem
{
    public class MapSprite : GBA.SpriteSheet
    {
        public const int W_TILES = 20;
        public const int H_TILES = 16;

        public const int WIDTH  = W_TILES * Tile.SIZE;
        public const int HEIGHT = H_TILES * Tile.SIZE;

        public const int IDLE = 0;
        public const int WALK = 1;

        public const int IDLE_SIZE_16x16 = 0x00;
        public const int IDLE_SIZE_16x32 = 0x01;
        public const int IDLE_SIZE_32x32 = 0x02;

        // can either 0x00 for 16x16, 0x01 for 16x32, or 0x02 for 32x32
        public byte IdleSize;



        /// <summary>
        /// Creates a map sprite from the given data
        /// </summary>
        public MapSprite(Palette palette, byte[] idle, byte[] move, byte size)
            : base(W_TILES * Tile.SIZE, H_TILES * Tile.SIZE)
        {
            if (palette == null) throw new Exception("Map Sprite palette is null.");
            if (idle == null) throw new Exception("Map Sprite idle sheet is null.");
            if (move == null) throw new Exception("Map sprite move sheet is null.");
            if (size > IDLE_SIZE_32x32) throw new Exception("Map Sprite size byte is invalid: " + size);

            AddSprite(new Sprite(palette, new Tileset(idle), new TileMap(Map_Idle(size))), 0, 32);
            AddSprite(new Sprite(palette, new Tileset(move), new TileMap(Map_Move())),     32, 0);
            IdleSize = size;
        }
        /// <summary>
        /// Creates a Map Sprite from the given image
        /// </summary>
        public MapSprite(GBA.Image image)
            : base(WIDTH, HEIGHT)
        {
            if (image.Width != Width || image.Height != Height) throw new Exception(
                "Image given has invalid dimensions: it should be " + Width + "x" + Height + " pixels");

            Tileset idleTiles = new GBA.Tileset();
            Tileset moveTiles = new GBA.Tileset();
            byte size = 0x02;
            if (image.IsRegionEmpty(new Rectangle( 0, 32, Tile.SIZE, HEIGHT - 32)) &&
                image.IsRegionEmpty(new Rectangle(24, 32, Tile.SIZE, HEIGHT - 32)))
            {
                size = 0x01;
            }
            else if (
                image.IsRegionEmpty(new Rectangle(Tile.SIZE,  0, Tile.SIZE * 2, Tile.SIZE * 2)) &&
                image.IsRegionEmpty(new Rectangle(Tile.SIZE, 32, Tile.SIZE * 2, Tile.SIZE * 2)) &&
                image.IsRegionEmpty(new Rectangle(Tile.SIZE, 64, Tile.SIZE * 2, Tile.SIZE * 2)))
            {
                size = 0x00;
            }

            idleTiles.Parse(image, TileMap.Place(Map_Idle(size), 0, 4, W_TILES, H_TILES));
            moveTiles.Parse(image, TileMap.Place(Map_Move(),     4, 0, W_TILES, H_TILES));
                
            AddSprite(new Sprite(image.Colors, idleTiles, new TileMap(Map_Idle(size))), 0 * 8, 4 * 8);
            AddSprite(new Sprite(image.Colors, moveTiles, new TileMap(Map_Move())),     4 * 8, 0 * 8);
            IdleSize = size;
        }

        public static int?[,] Map_Idle(byte size)
        {
            switch (size)
            {
                case 0x00: return TileMap.Convert(new int?[12, 4]
                {
                    { null, null, null, null },
                    { null, null, null, null },
                    { null, 0x00, 0x01, null },
                    { null, 0x02, 0x03, null },
                    { null, null, null, null },
                    { null, null, null, null },
                    { null, 0x04, 0x05, null },
                    { null, 0x06, 0x07, null },
                    { null, null, null, null },
                    { null, null, null, null },
                    { null, 0x08, 0x09, null },
                    { null, 0x0A, 0x0B, null },
                }, 0);

                case 0x01: return TileMap.Convert(new int?[12, 4]
                {
                    { null, 0x00, 0x01, null },
                    { null, 0x02, 0x03, null },
                    { null, 0x04, 0x05, null },
                    { null, 0x06, 0x07, null },
                    { null, 0x08, 0x09, null },
                    { null, 0x0A, 0x0B, null },
                    { null, 0x0C, 0x0D, null },
                    { null, 0x0E, 0x0F, null },
                    { null, 0x10, 0x11, null },
                    { null, 0x12, 0x13, null },
                    { null, 0x14, 0x15, null },
                    { null, 0x16, 0x17, null },
                }, 0);

                case 0x02: return TileMap.Convert(new int?[12, 4]
                {
                    { 0x00, 0x01, 0x02, 0x03 },
                    { 0x04, 0x05, 0x06, 0x07 },
                    { 0x08, 0x09, 0x0A, 0x0B },
                    { 0x0C, 0x0D, 0x0E, 0x0F },
                    { 0x10, 0x11, 0x12, 0x13 },
                    { 0x14, 0x15, 0x16, 0x17 },
                    { 0x18, 0x19, 0x1A, 0x1B },
                    { 0x1C, 0x1D, 0x1E, 0x1F },
                    { 0x20, 0x21, 0x22, 0x23 },
                    { 0x24, 0x25, 0x26, 0x27 },
                    { 0x28, 0x29, 0x2A, 0x2B },
                    { 0x2C, 0x2D, 0x2E, 0x2F },
                }, 0);

                default: return new int?[0, 0];
            }
        }
        public static int?[,] Map_Move()
        {
            return TileMap.Convert(new int?[16, 16]
            {
                { 0x00, 0x01, 0x02, 0x03, 0x40, 0x41, 0x42, 0x43, 0x80, 0x81, 0x82, 0x83, null, null, null, null },
                { 0x04, 0x05, 0x06, 0x07, 0x44, 0x45, 0x46, 0x47, 0x84, 0x85, 0x86, 0x87, null, null, null, null },
                { 0x08, 0x09, 0x0A, 0x0B, 0x48, 0x49, 0x4A, 0x4B, 0x88, 0x89, 0x8A, 0x8B, null, null, null, null },
                { 0x0C, 0x0D, 0x0E, 0x0F, 0x4C, 0x4D, 0x4E, 0x4F, 0x8C, 0x8D, 0x8E, 0x8F, null, null, null, null },
                { 0x10, 0x11, 0x12, 0x13, 0x50, 0x51, 0x52, 0x53, 0x90, 0x91, 0x92, 0x93, 0xC0, 0xC1, 0xC2, 0xC3 },
                { 0x14, 0x15, 0x16, 0x17, 0x54, 0x55, 0x56, 0x57, 0x94, 0x95, 0x96, 0x97, 0xC4, 0xC5, 0xC6, 0xC7 },
                { 0x18, 0x19, 0x1A, 0x1B, 0x58, 0x59, 0x5A, 0x5B, 0x98, 0x99, 0x9A, 0x9B, 0xC8, 0xC9, 0xCA, 0xCB },
                { 0x1C, 0x1D, 0x1E, 0x1F, 0x5C, 0x5D, 0x5E, 0x5F, 0x9C, 0x9D, 0x9E, 0x9F, 0xCC, 0xCD, 0xCE, 0xCF },
                { 0x20, 0x21, 0x22, 0x23, 0x60, 0x61, 0x62, 0x63, 0xA0, 0xA1, 0xA2, 0xA3, 0xD0, 0xD1, 0xD2, 0xD3 },
                { 0x24, 0x25, 0x26, 0x27, 0x64, 0x65, 0x66, 0x67, 0xA4, 0xA5, 0xA6, 0xA7, 0xD4, 0xD5, 0xD6, 0xD7 },
                { 0x28, 0x29, 0x2A, 0x2B, 0x68, 0x69, 0x6A, 0x6B, 0xA8, 0xA9, 0xAA, 0xAB, 0xD8, 0xD9, 0xDA, 0xDB },
                { 0x2C, 0x2D, 0x2E, 0x2F, 0x6C, 0x6D, 0x6E, 0x6F, 0xAC, 0xAD, 0xAE, 0xAF, 0xDC, 0xDD, 0xDE, 0xDF },
                { 0x30, 0x31, 0x32, 0x33, 0x70, 0x71, 0x72, 0x73, 0xB0, 0xB1, 0xB2, 0xB3, 0xE0, 0xE1, 0xE2, 0xE3 },
                { 0x34, 0x35, 0x36, 0x37, 0x74, 0x75, 0x76, 0x77, 0xB4, 0xB5, 0xB6, 0xB7, 0xE4, 0xE5, 0xE6, 0xE7 },
                { 0x38, 0x39, 0x3A, 0x3B, 0x78, 0x79, 0x7A, 0x7B, 0xB8, 0xB9, 0xBA, 0xBB, 0xE8, 0xE9, 0xEA, 0xEB },
                { 0x3C, 0x3D, 0x3E, 0x3F, 0x7C, 0x7D, 0x7E, 0x7F, 0xBC, 0xBD, 0xBE, 0xBF, 0xEC, 0xED, 0xEE, 0xEF },
            }, 0);
        }
        public static int?[,] Map_Test(int offset, byte? size = null)
        {
            if (size == null)
            {
                return TileMap.Convert(new int?[4, 4]
                {
                    { 0x00, 0x01, 0x02, 0x03 },
                    { 0x04, 0x05, 0x06, 0x07 },
                    { 0x08, 0x09, 0x0A, 0x0B },
                    { 0x0C, 0x0D, 0x0E, 0x0F }
                }, offset - 1);
            }
            else
            {
                switch (size)
                {
                    case 0x00: offset /= 4;
                    return TileMap.Convert(new int?[4, 4]
                    {
                        { null, null, null, null },
                        { null, null, null, null },
                        { null, 0x00, 0x01, null },
                        { null, 0x02, 0x03, null },
                    }, offset);

                    case 0x01: offset /= 2;
                    return TileMap.Convert(new int?[4, 4]
                    {
                        { null, 0x00, 0x01, null },
                        { null, 0x02, 0x03, null },
                        { null, 0x04, 0x05, null },
                        { null, 0x06, 0x07, null },
                    }, offset);

                    case 0x02:
                    return TileMap.Convert(new int?[4, 4]
                    {
                        { 0x00, 0x01, 0x02, 0x03 },
                        { 0x04, 0x05, 0x06, 0x07 },
                        { 0x08, 0x09, 0x0A, 0x0B },
                        { 0x0C, 0x0D, 0x0E, 0x0F },
                    }, offset);

                    default: return new int?[0, 0];
                }
            }
        }
    }
}
