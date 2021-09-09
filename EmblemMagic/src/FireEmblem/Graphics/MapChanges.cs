using System;
using System.Collections.Generic;
using System.Drawing;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    /// <summary>
    /// Represents the set of triggerable tile changes that occur on the map throughout the chapter
    /// </summary>
    public class MapChanges
    {
        public Int32 Count
        {
            get
            {
                return Changes.Count;
            }
        }

        List<Tuple<Byte, Rectangle, Int32[,]>> Changes;



        public MapChanges(List<Tuple<Byte, Rectangle, Int32[,]>> changes)
        {
            Changes = changes;
        }
        public MapChanges(Pointer address)
        {
            Changes = new List<Tuple<Byte, Rectangle, Int32[,]>>();
            Rectangle area;
            Int32[,] tiles;

            Byte[] buffer;
            Pointer pointer;
            for (Int32 index = 0; index < 0xFF; index++)
            {
                buffer = Core.ReadData(address + index * 12, 12);

                if (buffer[0] == 0xFF)
                {
                    /* So apparently having only 0x00 bytes after the inital 0xFF byte of the terminator isn't necessary..?
                    for (int i = 1; i < 12; i++)
                    {
                        if (buffer[i] != 0x00)
                            UI.ShowError("Invalid terminator found at byte " + index * 12 + i + " of the map changes.");
                    }
                    */
                    break;
                }
                else
                {
                    area = new Rectangle(buffer[1], buffer[2], buffer[3], buffer[4]);
                    tiles = new Int32[area.Width, area.Height];

                    Byte[] bytes = new Byte[4];
                    Array.Copy(buffer, 8, bytes, 0, 4);
                    try { pointer = new Pointer(bytes); }
                    catch { throw new Exception("Invalid pointer found in the triggerable map changes."); }

                    bytes = Core.ReadData(pointer, tiles.Length * 2);
                    for (Int32 i = 0; i < tiles.Length; i++)
                    {
                        tiles[i % area.Width, i / area.Width] = ((bytes[i * 2] | (bytes[i * 2 + 1] << 8)) >> 2);
                    }

                    Changes.Add(Tuple.Create(buffer[0], area, tiles));
                }
            }
        }



        public Byte GetNumber(Int32 index)
        {
            return Changes[index].Item1;
        }
        public Rectangle GetArea(Int32 index)
        {
            return Changes[index].Item2;
        }
        public Int32[,] GetTiles(Int32 index)
        {
            return Changes[index].Item3;
        }
        public Int32 GetTile(Int32 index, Int32 x, Int32 y)
        {
            Rectangle area = Changes[index].Item2;
            if (area.Contains(x, y))
            {
                return Changes[index].Item3[x - area.X, y - area.Y];
            }
            else return 0;
        }

        public void SetNumber(Int32 index, Byte number)
        {
            Changes[index] = Tuple.Create(number, GetArea(index), GetTiles(index));
        }
        public void SetArea(Int32 index, Rectangle area)
        {
            Changes[index] = Tuple.Create(GetNumber(index), area, GetTiles(index));
        }
        public void SetTiles(Int32 index, Int32[,] tiles)
        {
            Changes[index] = Tuple.Create(GetNumber(index), GetArea(index), tiles);
        }
        public void SetTile(Int32 index, Int32 x, Int32 y, Int32 tile)
        {
            Rectangle area = GetArea(index);
            Int32[,] tiles = GetTiles(index);

            if (area.Contains(x, y))
            {
                tiles[x - area.X, y - area.Y] = tile;

                if (tile == 0)
                {
                    Int32 minX = area.Width;
                    Int32 minY = area.Height;
                    Int32 maxX = 0;
                    Int32 maxY = 0;
                    Boolean empty = true;
                    for (Int32 tileY = 0; tileY < area.Height; tileY++)
                    for (Int32 tileX = 0; tileX < area.Width; tileX++)
                    {
                        if (tiles[tileX, tileY] != 0)
                        {
                            empty = false;
                            if (tileX < minX) minX = tileX; if (tileY < minY) minY = tileY;
                            if (tileX > maxX) maxX = tileX; if (tileY > maxY) maxY = tileY;
                        }
                    }
                    if (empty)
                    {
                        Changes[index] = Tuple.Create(GetNumber(index), new Rectangle(), new Int32[0, 0]);
                    }
                    else
                    {
                        Rectangle new_area = new Rectangle(
                            area.X + minX,
                            area.Y + minY,
                            area.Width - maxX,
                            area.Height - maxY);

                        Int32[,] result = new Int32[new_area.Width, new_area.Height];
                        for (Int32 tileY = 0; tileY < new_area.Height; tileY++)
                        for (Int32 tileX = 0; tileX < new_area.Width; tileX++)
                        {
                            result[tileX, tileY] = tiles[minX + tileX, minY + tileY];
                        }

                        SetArea(index, new_area);
                        SetTiles(index, result);
                    }
                }

                SetTiles(index, tiles);
            }
            else if (tile != 0)
            {
                Boolean left  = (x < area.X);
                Boolean right = (x >= area.X + area.Width);
                Boolean up    = (y < area.Y);
                Boolean down  = (y >= area.Y + area.Height);

                Rectangle new_area = new Rectangle(
                    (left  ? x : area.X),
                    (up    ? y : area.Y),
                    (right ? 1 + x - area.X : left ? area.X - x + area.Width  : area.Width),
                    (down  ? 1 + y - area.Y : up   ? area.Y - y + area.Height : area.Height));

                Int32[,] result = new Int32[new_area.Width, new_area.Height];
                for (Int32 tileY = 0; tileY < area.Height; tileY++)
                for (Int32 tileX = 0; tileX < area.Width; tileX++)
                {
                    result[(area.X - new_area.X) + tileX,
                           (area.Y - new_area.Y) + tileY] = tiles[tileX, tileY];
                }
                result[x - new_area.X, y - new_area.Y] = tile;

                SetArea(index, new_area);
                SetTiles(index, result);
            }
        }
        


        /// <summary>
        /// Returns 'true' if the given point is contained within the area of the tile changes at 'index'
        /// </summary>
        public Boolean Contains(Int32 index, Int32 x, Int32 y)
        {
            return Changes[index].Item2.Contains(x, y);
        }
        
        public Byte[] ToBytes(Pointer address)
        {
            Byte[] changes = new Byte[Changes.Count * 12 + 12];

            for (Int32 i = 0; i < Changes.Count; i++)
            {
                changes[i * 12] = Changes[i].Item1;
                changes[i * 12 + 1] = (Byte)Changes[i].Item2.X;
                changes[i * 12 + 2] = (Byte)Changes[i].Item2.Y;
                changes[i * 12 + 3] = (Byte)Changes[i].Item2.Width;
                changes[i * 12 + 4] = (Byte)Changes[i].Item2.Height;
            }
            changes[changes.Length - 12] = 0xFF;

            List<Byte[]> tiles = new List<Byte[]>();
            Byte[] result;
            Pointer pointer;
            Int32 length = 0;
            Int32 index;
            for (Int32 i = 0; i < Changes.Count; i++)
            {
                pointer = address + changes.Length + length;
                Array.Copy(pointer.ToBytes(), 0, changes, i * 12 + 8, 4);

                result = new Byte[Changes[i].Item2.Width * Changes[i].Item2.Height * 2];
                index = 0;
                for (Int32 y = 0; y < Changes[i].Item2.Height; y++)
                for (Int32 x = 0; x < Changes[i].Item2.Width; x++)
                {
                    Array.Copy(Util.UInt16ToBytes((UInt16)(Changes[i].Item3[x, y] << 2), true), 0, result, index, 2);
                    index += 2;
                }
                tiles.Add(result);
                length += result.Length;
            }

            result = new Byte[changes.Length + length];
            Array.Copy(changes, result, changes.Length);
            length = changes.Length;
            for (Int32 i = 0; i < tiles.Count; i++)
            {
                Array.Copy(tiles[i], 0, result, length, tiles[i].Length);
                length += tiles[i].Length;
            }
            return result;
        }
    }
}
