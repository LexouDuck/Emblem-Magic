using System;
using System.Collections.Generic;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// This class is used to create OAM data from user-given images
    /// </summary>
    public class OAM_Maker
    {
        /// <summary>
        /// The maximum amount of OAM object allowed for any given frame on the GBA
        /// </summary>
        public const Int32 MAXIMUM = 128;

        /// <summary>
        /// The index of the tileset to use for this sprite set
        /// </summary>
        public Int32 TilesetIndex;
        /// <summary>
        /// The array of OAM sprites this class creates from an image
        /// </summary>
        public OAM_Array SpriteData;


        
        public OAM_Maker(ref List<TileSheet> Graphics, GBA.Image image, Int32 offsetX, Int32 offsetY)
        {
            Rectangle sprite = GetSpriteArea(image);

            if (sprite == new Rectangle())
            {
                TilesetIndex = 0;
                SpriteData = new OAM_Array();
                return;
            }

            var OAMs = GetSpriteOAMs(image, sprite);

            TilesetIndex = -1;
            Point[] sheet = new Point[OAMs.Count];
            for (Int32 i = 0; i < Graphics.Count; i++)
            {
                sheet = Graphics[i].CheckIfFits(OAMs);

                if (sheet == null) continue;
                else
                {
                    TilesetIndex = i; break;
                }
            }
            if (TilesetIndex == -1)
            {
                TilesetIndex = Graphics.Count;
                Graphics.Add(new TileSheet(32, 8));
                sheet = Graphics[TilesetIndex].CheckIfFits(OAMs);
                if (sheet == null) throw new Exception("Frame has more tiles than a 32x8 TileSheet allows.");
            }

            List<OAM> result = new List<OAM>();
            Point pos;
            Size size;
            for (Int32 i = 0; i < OAMs.Count; i++)
            {
                pos = OAMs[i].Item1;
                size = OAMs[i].Item2;

                for (Int32 y = 0; y < size.Height; y++)
                for (Int32 x = 0; x < size.Width; x++)
                {
                    Graphics[TilesetIndex][sheet[i].X + x, sheet[i].Y + y] = image.GetTile(
                        pos.X + x * Tile.SIZE, 
                        pos.Y + y * Tile.SIZE);
                }

                var shapesize = OAM.GetShapeSize(size);
                if (shapesize == null) throw new Exception("Invalid OAM Shape/Size created.");

                if (result.Count == MAXIMUM)
                    throw new Exception("There cannot be more than " + MAXIMUM + " OAM objects in one frame.");
                result.Add(new OAM(
                    shapesize.Item1,
                    shapesize.Item2,
                    (Int16)(pos.X - offsetX),
                    (Int16)(pos.Y - offsetY),
                    0x00,
                    0x00,
                    OAM_GFXMode.Normal,
                    OAM_OBJMode.Normal,
                    false,
                    false,
                    (Byte)sheet[i].X, (Byte)sheet[i].Y,
                    false, false));
                /*
                Magic.UI.ShowMessage(
                    "h: " + size.Width + ", w: " + size.Height +
                    '\n' + "shape: " + shapesize.Item1 + ", size: " + shapesize.Item2 +
                    '\n' + result[result.Count - 1].SpriteShape + ", " + result[result.Count - 1].SpriteSize +
                    '\n' + Magic.Util.BytesToSpacedHex(result[result.Count - 1].ToBytes())); */
            }

            SpriteData = new OAM_Array(result);
        }



        /// <summary>
        /// Returns the placements and sizes of OAM blocks for the sprite in the area of the image given
        /// </summary>
        static List<Tuple<Point, Size>> GetSpriteOAMs(Image image, Rectangle sprite)
        {
            Int32 width = sprite.Width / Tile.SIZE;
            Int32 height = sprite.Height / Tile.SIZE;
            Boolean[,] tilesToMap = new Boolean[width, height];
            Tile tile = null;
            for (Int32 y = 0; y < height; y++)
            for (Int32 x = 0; x < width; x++)
            {
                tile = image.GetTile(
                    sprite.X + x * Tile.SIZE,
                    sprite.Y + y * Tile.SIZE);

                if (!tile.IsEmpty())
                {
                    tilesToMap[x, y] = true;
                }
            }

            var result = new List<Tuple<Point, Size>>();

            Point pos;
            Size size;
            Int32 tileAmount = 1;
            Int32 index = 0;
            Int32 offsetX;
            Int32 offsetY;
            while (tileAmount > 0)
            {
                offsetX = index % width;
                offsetY = index / width;
                index++;
                index %= tilesToMap.Length;
                
                pos = new Point(sprite.X + offsetX * 8, sprite.Y + offsetY * 8);
                size = GetSpriteSize(tilesToMap, offsetX, offsetY);
                if (size == new Size()) continue;

                result.Add(Tuple.Create(pos, size));

                for (Int32 y = 0; y < size.Height; y++)
                for (Int32 x = 0; x < size.Width; x++)
                {
                    tilesToMap[x + offsetX, y + offsetY] = false;
                }
                tileAmount = 0;
                foreach (Boolean value in tilesToMap)
                {
                    if (value) tileAmount++;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a rectangle containing every non-bg-color pixel of the given image
        /// </summary>
        static Rectangle GetSpriteArea(Image image)
        {
            Int32 minX = image.Width;
            Int32 minY = image.Height;
            Int32 maxX = 0;
            Int32 maxY = 0;
            Boolean empty = true;
            for (Int32 y = 0; y < image.Height; y++)
            for (Int32 x = 0; x < image.Width; x++)
            {
                if (image[x, y] != 0)
                {
                    empty = false;
                    if (x < minX) minX = x; if (y < minY) minY = y;
                    if (x > maxX) maxX = x; if (y > maxY) maxY = y;
                }
            }
            minX %= 8; minY %= 8;
            maxX += 8; maxY += 8;
            if (maxX > image.Width) maxX = image.Width;
            if (maxX > image.Width) maxY = image.Width;
            if (empty) return new Rectangle();
            else return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
        /// <summary>
        /// Returns the optimal shape/size for OAM in the bool tilemap at the given coordinates, without any overlap
        /// </summary>
        static Size GetSpriteSize(Boolean[,] map, Int32 x = 0, Int32 y = 0)
        {
            if (!map[x, y]) return new Size();

            foreach (Size oam in possibleOAMs)
            {
                for (Int32 tileY = 0; tileY < oam.Height; tileY++)
                for (Int32 tileX = 0; tileX < oam.Width; tileX++)
                {
                    try
                    {
                        if (!map[x + tileX, y + tileY])
                            goto Continue;
                    }
                    catch { goto Continue; }
                }
                return oam;
                Continue: continue;
            }
            return new Size();
        }

        /// <summary>
        /// A list of possibles OAM sprite sizes (in tiles), listed from biggest to smallest
        /// </summary>
        static Size[] possibleOAMs = new Size[12]
        {
            new Size(8, 8), // OAM_Shape.Square, OAM_Size.Times8
            new Size(8, 4), // OAM_Shape.Rect_H, OAM_Size.Times8
            new Size(4, 8), // OAM_Shape.Rect_V, OAM_Size.Times8
            new Size(4, 4), // OAM_Shape.Square, OAM_Size.Times4
            new Size(4, 2), // OAM_Shape.Rect_H, OAM_Size.Times4
            new Size(2, 4), // OAM_Shape.Rect_V, OAM_Size.Times4
            new Size(4, 1), // OAM_Shape.Rect_H, OAM_Size.Times2
            new Size(1, 4), // OAM_Shape.Rect_V, OAM_Size.Times2
            new Size(2, 2), // OAM_Shape.Square, OAM_Size.Times2
            new Size(2, 1), // OAM_Shape.Rect_H, OAM_Size.Times1
            new Size(1, 2), // OAM_Shape.Rect_V, OAM_Size.Times1
            new Size(1, 1), // OAM_Shape.Square, OAM_Size.Times1
        };




        /// <summary>
        /// Unused and probably doesn't work, supposed to do tile redundancy checking
        /// </summary>
        void /*Tuple<OAM_Array, uint>*/ GetOAMfromFile(GBA.Image image)
        {
            /*
            List<List<OAM>> matches;
            matches = new List<List<OAM>>();
            foreach (Tileset sheet in Graphics)
            {
                matches.Add(new List<OAM>());
            }
            OAM match;
            GBA.Tile tile;
            for (int y = minY; y < maxY; y += 8)
            for (int x = minX; x < maxX; x += 8)
            {
                tile = image.GetTile(x, y);

                if (tile.IsEmpty()) continue;
                
                for (int i = 0; i < Graphics.Count; i++)
                {
                    match = GetMatch(image, x, y, i, Graphics[i].FindMatch(tile));

                    if (match == null) continue;
                    else
                    {
                        matches[i].Add(match);
                    }
                }
            }

            int tileset = 0;
            string debug = "";
            for (int i = 0; i < Graphics.Count; i++)
            {
                debug += i + " - " + matches[i].Count + "\n";
                if (matches[i].Count > matches[tileset].Count)
                    tileset = i;
            }   // select the tileset with the most matches
            UI.ShowMessage(debug);
            */



            /*
            bool[,] mapped = new bool[width, height];
            // create a bool matrix to check what pixels have yet to be put in the tileset
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                if (image[minX + x, minY + y] == image.Colors[0])
                {
                    mapped[x, y] = true;
                }
            }   // all bg-color pixels are marked as mapped

            Size size;
            Point pos;
            for (int i = 0; i < matches[tileset].Count; i++)
            {
                pos = matches[tileset][i].Position;
                size = matches[tileset][i].GetDimensions();
                int X, Y;
                for (int y = 0; y < size.Height; y++)
                for (int x = 0; x < size.Width; x++)
                {
                    for (int tileY = 0; tileY < 8; tileY++)
                    for (int tileX = 0; tileX < 8; tileX++)
                    {
                        X = (pos.X + 0x94 - minX) + (x * 8 + tileX);
                        Y = (pos.Y - minY) + (y * 8 + tileY);
                        mapped[X, Y] = true;
                    }
                }
            }
            */



            /*
            int tileset = Graphics.Count - 1;
            string debug = "\n tileset " + tileset + ": \n";
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    debug += (Graphics[tileset][1 + x + y * 32] != null) ? "O" : ",_,";
                }
                debug += "\n";
            }
            UI.ShowMessage(debug);
            */
            // (matches[tileset].Count == 0) ? -1 : tileset);
            //UI.ShowMessage("added oam: " + new_oam.Count);
        }
        /// <summary>
        /// Supposed to check other OAMs in the sheet to avoid redundancy - currently unused
        /// </summary>
        OAM GetMatch(GBA.Image image, Int32 imageX, Int32 imageY, Int32 tileset, Tuple<Point, Boolean, Boolean> match, ref List<TileSheet> Graphics)
        {
            if (match == null) return OAM.Terminator;

            Boolean[,] matched = new Boolean[8, 8];
            Point sheet = match.Item1;
            Boolean flipH = match.Item2;
            Boolean flipV = match.Item3;
            Tile tile;
            for (Int32 tileY = 0; flipV ? (tileY > -8) : (tileY < 8); tileY += flipV ? -1 : 1)
            for (Int32 tileX = 0; flipH ? (tileX > -8) : (tileX < 8); tileX += flipH ? -1 : 1)
            {
                if (tileX == 0 && tileY == 0) continue;
                // top-left corner tile of OAM obj has already been matched
                try
                {
                    if (Graphics[tileset][sheet.X + tileX, sheet.Y + tileY] == null) continue;

                    tile = image.GetTile(imageX + tileX * 8, imageY + tileY * 8);
                }
                catch { break; }

                if (flipH) tile = tile.FlipHorizontal();
                if (flipV) tile = tile.FlipVertical();

                if (tile.Equals(Graphics[tileset][sheet.X + tileX, sheet.Y + tileY]))
                {
                    matched[Math.Abs(tileX), Math.Abs(tileY)] = true;
                }
            }

            var shapesize = OAM.GetShapeSize(GetSpriteSize(matched));
            if (shapesize == null) return OAM.Terminator;

            return new OAM(
                shapesize.Item1,
                shapesize.Item2,
                (Int16)(imageX - 0x94), (Int16)(imageY),
                0x00,
                0x00,
                OAM_GFXMode.Normal,
                OAM_OBJMode.Normal,
                false,
                false,
                (Byte)sheet.X, (Byte)sheet.Y,
                flipH, flipV);
        }
    }
}
