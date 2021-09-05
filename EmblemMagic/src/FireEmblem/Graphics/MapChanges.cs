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
        public int Count
        {
            get
            {
                return Changes.Count;
            }
        }

        List<Tuple<byte, Rectangle, int[,]>> Changes;



        public MapChanges(List<Tuple<byte, Rectangle, int[,]>> changes)
        {
            Changes = changes;
        }
        public MapChanges(Pointer address)
        {
            Changes = new List<Tuple<byte, Rectangle, int[,]>>();
            Rectangle area;
            int[,] tiles;

            byte[] buffer;
            Pointer pointer;
            for (int index = 0; index < 0xFF; index++)
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
                    tiles = new int[area.Width, area.Height];

                    byte[] bytes = new byte[4];
                    Array.Copy(buffer, 8, bytes, 0, 4);
                    try { pointer = new Pointer(bytes); }
                    catch { throw new Exception("Invalid pointer found in the triggerable map changes."); }

                    bytes = Core.ReadData(pointer, tiles.Length * 2);
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        tiles[i % area.Width, i / area.Width] = ((bytes[i * 2] | (bytes[i * 2 + 1] << 8)) >> 2);
                    }

                    Changes.Add(Tuple.Create(buffer[0], area, tiles));
                }
            }
        }



        public Byte GetNumber(int index)
        {
            return Changes[index].Item1;
        }
        public Rectangle GetArea(int index)
        {
            return Changes[index].Item2;
        }
        public int[,] GetTiles(int index)
        {
            return Changes[index].Item3;
        }
        public int GetTile(int index, int x, int y)
        {
            Rectangle area = Changes[index].Item2;
            if (area.Contains(x, y))
            {
                return Changes[index].Item3[x - area.X, y - area.Y];
            }
            else return 0;
        }

        public void SetNumber(int index, byte number)
        {
            Changes[index] = Tuple.Create(number, GetArea(index), GetTiles(index));
        }
        public void SetArea(int index, Rectangle area)
        {
            Changes[index] = Tuple.Create(GetNumber(index), area, GetTiles(index));
        }
        public void SetTiles(int index, int[,] tiles)
        {
            Changes[index] = Tuple.Create(GetNumber(index), GetArea(index), tiles);
        }
        public void SetTile(int index, int x, int y, int tile)
        {
            Rectangle area = GetArea(index);
            int[,] tiles = GetTiles(index);

            if (area.Contains(x, y))
            {
                tiles[x - area.X, y - area.Y] = tile;

                if (tile == 0)
                {
                    int minX = area.Width;
                    int minY = area.Height;
                    int maxX = 0;
                    int maxY = 0;
                    bool empty = true;
                    for (int tileY = 0; tileY < area.Height; tileY++)
                    for (int tileX = 0; tileX < area.Width; tileX++)
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
                        Changes[index] = Tuple.Create(GetNumber(index), new Rectangle(), new int[0, 0]);
                    }
                    else
                    {
                        Rectangle new_area = new Rectangle(
                            area.X + minX,
                            area.Y + minY,
                            area.Width - maxX,
                            area.Height - maxY);

                        int[,] result = new int[new_area.Width, new_area.Height];
                        for (int tileY = 0; tileY < new_area.Height; tileY++)
                        for (int tileX = 0; tileX < new_area.Width; tileX++)
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
                bool left  = (x < area.X);
                bool right = (x >= area.X + area.Width);
                bool up    = (y < area.Y);
                bool down  = (y >= area.Y + area.Height);

                Rectangle new_area = new Rectangle(
                    (left  ? x : area.X),
                    (up    ? y : area.Y),
                    (right ? 1 + x - area.X : left ? area.X - x + area.Width  : area.Width),
                    (down  ? 1 + y - area.Y : up   ? area.Y - y + area.Height : area.Height));

                int[,] result = new int[new_area.Width, new_area.Height];
                for (int tileY = 0; tileY < area.Height; tileY++)
                for (int tileX = 0; tileX < area.Width; tileX++)
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
        public bool Contains(int index, int x, int y)
        {
            return Changes[index].Item2.Contains(x, y);
        }
        
        public byte[] ToBytes(Pointer address)
        {
            byte[] changes = new byte[Changes.Count * 12 + 12];

            for (int i = 0; i < Changes.Count; i++)
            {
                changes[i * 12] = Changes[i].Item1;
                changes[i * 12 + 1] = (byte)Changes[i].Item2.X;
                changes[i * 12 + 2] = (byte)Changes[i].Item2.Y;
                changes[i * 12 + 3] = (byte)Changes[i].Item2.Width;
                changes[i * 12 + 4] = (byte)Changes[i].Item2.Height;
            }
            changes[changes.Length - 12] = 0xFF;

            List<byte[]> tiles = new List<byte[]>();
            byte[] result;
            Pointer pointer;
            int length = 0;
            int index;
            for (int i = 0; i < Changes.Count; i++)
            {
                pointer = address + changes.Length + length;
                Array.Copy(pointer.ToBytes(), 0, changes, i * 12 + 8, 4);

                result = new byte[Changes[i].Item2.Width * Changes[i].Item2.Height * 2];
                index = 0;
                for (int y = 0; y < Changes[i].Item2.Height; y++)
                for (int x = 0; x < Changes[i].Item2.Width; x++)
                {
                    Array.Copy(Util.UInt16ToBytes((UInt16)(Changes[i].Item3[x, y] << 2), true), 0, result, index, 2);
                    index += 2;
                }
                tiles.Add(result);
                length += result.Length;
            }

            result = new byte[changes.Length + length];
            Array.Copy(changes, result, changes.Length);
            length = changes.Length;
            for (int i = 0; i < tiles.Count; i++)
            {
                Array.Copy(tiles[i], 0, result, length, tiles[i].Length);
                length += tiles[i].Length;
            }
            return result;
        }
    }
}
