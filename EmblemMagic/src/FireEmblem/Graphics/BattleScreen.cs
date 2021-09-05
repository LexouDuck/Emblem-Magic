using System;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public class BattleScreen : TSA_Image
    {
        public const int WIDTH = 32;
        public const int HEIGHT = 13;
        public const int HALFWIDTH = 16;

        public static int NAME_WIDTH      { get { return Core.CurrentROM is FE8 ? 7 : 6; } } 
        public static int WEAPON_WIDTH    { get { return Core.CurrentROM is FE8 ? 8 : 7; } }
        public const int NAME_HEIGHT = 6;
        public const int WEAPON_HEIGHT = 7;

        public static int L_NAME_OFFSET   { get { return Core.CurrentROM is FE8 ? 68 : 32; } } 
        public static int L_WEAPON_OFFSET { get { return Core.CurrentROM is FE8 ? 82 : 44; } } 
        public static int R_NAME_OFFSET   { get { return Core.CurrentROM is FE8 ? 98 : 58; } } 
        public static int R_WEAPON_OFFSET { get { return Core.CurrentROM is FE8 ? 112 : 70; } } 

        public static int EMPTY_TILE     { get { return Core.CurrentROM is FE8 ? 0 : 31; } }
        public static int TILE_LIMIT     { get { return Core.CurrentROM is FE8 ? 68 : 32; } }
        public static int TILE_LIMIT_END { get { return Core.CurrentROM is FE8 ? 128 : 84; } }

        public Tileset L_Name
        {
            get
            {
                return Graphics.GetTiles(L_NAME_OFFSET, NAME_WIDTH * 2);
            }
        }
        public Tileset L_Weapon
        {
            get
            {
                return Graphics.GetTiles(L_WEAPON_OFFSET, WEAPON_WIDTH * 2);
            }
        }
        public Tileset R_Name
        {
            get
            {
                return Graphics.GetTiles(R_NAME_OFFSET, NAME_WIDTH * 2);
            }
        }
        public Tileset R_Weapon
        {
            get
            {
                return Graphics.GetTiles(R_WEAPON_OFFSET, WEAPON_WIDTH * 2);
            }
        }



        public BattleScreen(
            byte[] palettes, byte[] tileset, TSA_Array tsa,
            Tileset L_name, Tileset L_weapon,
            Tileset R_name, Tileset R_weapon)
            : base(palettes, tileset, tsa)
        {
            LoadSecondaryTileset(L_name,   L_NAME_OFFSET);
            LoadSecondaryTileset(L_weapon, L_WEAPON_OFFSET);
            LoadSecondaryTileset(R_name,   R_NAME_OFFSET);
            LoadSecondaryTileset(R_weapon, R_WEAPON_OFFSET);
        }
        void LoadSecondaryTileset(Tileset tileset, int offset)
        {
            while (Graphics.Count < offset + tileset.Count)
            {
                Graphics.Add(Tile.Empty);
            }
            for (int i = 0; i < tileset.Count; i++)
            {
                Graphics[offset + i] = tileset[i];
            }
        }

        public BattleScreen(Bitmap image, Palette[] palettes) : base(WIDTH, HEIGHT)
        {
            if (image.Width != Width || image.Height != Height)
                throw new Exception("Image given has invalid dimensions, should be " + Width + "x" + Height + " pixels.");

            Palettes = palettes;
            Graphics = new Tileset(WIDTH * HEIGHT);
            Tiling = new TSA_Array(WIDTH, HEIGHT);

            const int X_L_NAME = 1;
            const int X_L_WEAPON = 8;
            int X_R_NAME = Core.CurrentROM is FE8 ? 24 : 25;
            const int X_R_WEAPON = 18;
            const int Y_NAME = 1;
            const int Y_WEAPON = 8;
            
            Tile tile;
            byte paletteIndex;
            System.Drawing.Rectangle region;
            for (int y = 0; y < HEIGHT; y++)
            for (int x = 0; x < WIDTH; x++)
            {
                if (x >= HALFWIDTH)
                {
                    if ((x >= X_R_NAME   && x < X_R_NAME + NAME_WIDTH && y >= Y_NAME   && y < Y_NAME   + 2) ||
                        (x >= X_R_WEAPON && x < X_R_WEAPON + 6        && y >= Y_WEAPON && y < Y_WEAPON + 2))
                            continue;
                    paletteIndex = 0;
                }
                else
                {
                    if ((x >= X_L_NAME   && x <  X_L_NAME  +  NAME_WIDTH  && y >= Y_NAME   && y < Y_NAME   + 2) ||
                        (x >= X_L_WEAPON && x < X_L_WEAPON + WEAPON_WIDTH && y >= Y_WEAPON && y < Y_WEAPON + 2))
                            continue;
                    paletteIndex = 1;
                }
                region = new System.Drawing.Rectangle(x * Tile.SIZE, y * Tile.SIZE, Tile.SIZE, Tile.SIZE);
                tile = new Tile(new Image(image, region, palettes[paletteIndex]));
                    
                if (Graphics.Count == EMPTY_TILE)
                {
                    Graphics.Add(Tile.Empty);
                }
                if (Graphics.Count == TILE_LIMIT)
                {
                    for (int i = 0; TILE_LIMIT + i < TILE_LIMIT_END; i++)
                    {
                        Graphics.Add(Tile.Empty);
                    }
                }
                if (Graphics.Count >= 128)
                {
                    throw new Exception("Battle Screen Frame cannot have more than 128 tiles.");
                }
                if (tile.IsEmpty())
                {
                    Tiling[x, y] = new TSA(EMPTY_TILE, paletteIndex, false, false);
                }
                else
                {
                    var match = Graphics.FindMatch(tile);
                        
                    if (match == null)
                    {
                        Tiling[x, y] = new TSA(Graphics.Count, paletteIndex, false, false);
                        Graphics.Add(tile);
                    }
                    else
                    {
                        Tiling[x, y] = new TSA(match.Item1, paletteIndex, match.Item2, match.Item3);
                    }
                }
            }
            while (Graphics.Count < TILE_LIMIT_END) Graphics.Add(Tile.Empty);

            LoadSecondaryTileset(image, palettes, L_NAME_OFFSET,   1, X_L_NAME,   Y_NAME,   NAME_WIDTH,   2);
            LoadSecondaryTileset(image, palettes, L_WEAPON_OFFSET, 1, X_L_WEAPON, Y_WEAPON, WEAPON_WIDTH, 2);
            LoadSecondaryTileset(image, palettes, R_NAME_OFFSET,   0, X_R_NAME,   Y_NAME,   NAME_WIDTH,   2);
            LoadSecondaryTileset(image, palettes, R_WEAPON_OFFSET, 0, X_R_WEAPON, Y_WEAPON, WEAPON_WIDTH, 2);
        }
        void LoadSecondaryTileset(
            Bitmap image, Palette[] palettes,
            int index, byte paletteIndex,
            int tileX, int tileY, int width, int height)
        {
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Tiling[tileX + x, tileY + y] = new TSA(index, paletteIndex, false, false);
                Graphics[index++] = new Tile(new Image(image,
                    new System.Drawing.Rectangle(
                        (tileX + x) * Tile.SIZE,
                        (tileY + y) * Tile.SIZE,
                        Tile.SIZE, Tile.SIZE),
                    palettes[paletteIndex]));
            }
        }



        /// <summary>
        /// Makes the battle screen frame into a readible nicely formatted image
        /// </summary>
        public static TSA_Array GetReadibleTSA(TSA_Array tsa_array)
        {
            TSA_Array result = new TSA_Array(WIDTH, HEIGHT);

            tsa_array.Width = 15;
            // top left
            for (int y = 0; y < NAME_HEIGHT; y++)
            for (int x = 0; x < 15; x++)
            {
                result[1 + x, y] = tsa_array[x, y];
            }
            // bottom left
            for (int y = 0; y < WEAPON_HEIGHT; y++)
            for (int x = 0; x < 15; x++)
            {
                result[1 + x, NAME_HEIGHT + y] = tsa_array[x, NAME_HEIGHT * 2 + y];
            }
            // top right
            for (int y = 0; y < NAME_HEIGHT; y++)
            for (int x = 0; x < 15; x++)
            {
                result[HALFWIDTH + x, y] = tsa_array[x, NAME_HEIGHT + y];
            }
            // bottom right
            tsa_array.Width = 16;
            for (int y = 0; y < WEAPON_HEIGHT; y++)
            for (int x = 0; x < 2; x++)
            {
                result[HALFWIDTH + x, NAME_HEIGHT + y] = tsa_array[14 + x, 17 + y];
            }
            for (int y = 0; y < WEAPON_HEIGHT; y++)
            for (int x = 0; x < 14; x++)
            {
                result[HALFWIDTH + 2 + x, NAME_HEIGHT + y] = tsa_array[x, 18 + y];
            }
            if (Core.CurrentROM is FE8)
            {
                for (int i = 0; i < WEAPON_HEIGHT; i++)
                {
                    result[0, NAME_HEIGHT + i] = result[WIDTH - 1, NAME_HEIGHT + i];
                    result.SetPalette(0, NAME_HEIGHT + i, 1);
                    result.SetFlipH(0, NAME_HEIGHT + i, true);
                }
            }
            else
            {
                int offset = 398;
                for (int i = 0; i < NAME_HEIGHT; i++)
                {
                    result[0, i] = tsa_array.Tiles[offset + i * 2 + 1];
                    result[WIDTH - 1, i] = tsa_array.Tiles[offset + i * 2];
                }
                offset += (NAME_HEIGHT + 7) * 2;
                for (int i = 0; i < WEAPON_HEIGHT; i++)
                {
                    result[0, NAME_HEIGHT + i] = tsa_array.Tiles[offset + i * 2];
                    result.SetPalette(0, NAME_HEIGHT + i, 1);
                    result.SetFlipH(0, NAME_HEIGHT + i, false);
                }
            }
            return result;
        }
        /// <summary>
        /// Makes a readible battle frame TSA into an insertable version
        /// </summary>
        public static TSA_Array GetInsertableTSA(TSA_Array tsa_array)
        {
            TSA_Array result = (Core.CurrentROM is FE8) ? new TSA_Array(16, 32) : new TSA_Array(16, 28);

            result.Width = 15;
            // top left
            for (int y = 0; y < NAME_HEIGHT; y++)
            for (int x = 0; x < 15; x++)
            {
                result[x, y] = tsa_array[1 + x, y];
            }
            // bottom left
            for (int y = 0; y < WEAPON_HEIGHT; y++)
            for (int x = 0; x < 15; x++)
            {
                result[x, NAME_HEIGHT * 2 + y] = tsa_array[1 + x, NAME_HEIGHT + y];
            }
            // top right
            for (int y = 0; y < NAME_HEIGHT; y++)
            for (int x = 0; x < 15; x++)
            {
                result[x, NAME_HEIGHT + y] = tsa_array[HALFWIDTH + x, y];
            }
            // bottom right
            result.Width = 16;
            for (int y = 0; y < WEAPON_HEIGHT; y++)
            for (int x = 0; x < 2; x++)
            {
                result[14 + x, 17 + y] = tsa_array[HALFWIDTH + x, NAME_HEIGHT + y];
            }
            for (int y = 0; y < WEAPON_HEIGHT; y++)
            for (int x = 0; x < 14; x++)
            {
                result[x, 18 + y] = tsa_array[HALFWIDTH + 2 + x, NAME_HEIGHT + y];
            }
            // bottom right, set the rest of the TSA
            if (Core.CurrentROM is FE8)
            {   // (used for unit promotion)
                for (int y = 0; y < WEAPON_HEIGHT; y++)
                for (int x = 0; x < 2; x++)
                {
                    result[14 + x, 24 + y] = tsa_array[HALFWIDTH + x, NAME_HEIGHT + y];
                }
                for (int y = 0; y < WEAPON_HEIGHT; y++)
                for (int x = 0; x < 14; x++)
                {
                    result[x, 25 + y] = tsa_array[HALFWIDTH + 2 + x, NAME_HEIGHT + y];
                }
            }
            else
            {   // (used when screenshaking)
                int empty_height = 7;
                int offset = 398;
                TSA tsa;
                for (int i = 0; i < (HEIGHT + empty_height); i++)
                {
                    if (i < NAME_HEIGHT)
                    {
                        tsa = tsa_array[WIDTH - 1, i];
                        result.Tiles[offset + i * 2] = new TSA(tsa.TileIndex, 0, tsa.FlipH, tsa.FlipV);
                        tsa = tsa_array[0, i];
                        result.Tiles[offset + i * 2 + 1] = new TSA(tsa.TileIndex, 1, tsa.FlipH, tsa.FlipV);
                    }
                    else if (i < (NAME_HEIGHT + empty_height))
                    {
                        tsa = new TSA(EMPTY_TILE, 0, false, false);
                        result.Tiles[offset + i * 2] = tsa;
                        result.Tiles[offset + i * 2 + 1] = tsa;
                    }
                    else
                    {
                        tsa = tsa_array[WIDTH - 1, i - empty_height];
                        result.Tiles[offset + i * 2] = new TSA(tsa.TileIndex, 0, tsa.FlipH, tsa.FlipV);
                        tsa = tsa_array[0, i - empty_height];
                        result.Tiles[offset + i * 2 + 1] = new TSA(tsa.TileIndex, 1, tsa.FlipH, tsa.FlipV);
                    }
                }
            }
            return result;
        }
    }
}
