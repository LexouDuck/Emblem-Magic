using System;
using System.Collections.Generic;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public struct MapTileFrame
    {
        public const int LENGTH = 8;

        byte[] Data;

        public MapTileFrame(byte[] data)
        {
            if (data.Length != LENGTH) throw new Exception("Map tile frame length is invalid: " + data.Length);

            Data = data;
        }



        /// <summary>
        /// The pointer to the data used for this frame (can be palette data or tileset data)
        /// </summary>
        public Pointer FrameData
        {
            get
            {
                if (IsPaletteAnimation())
                     return new Pointer(Data.GetUInt32(0, true), false, true);
                else return new Pointer(Data.GetUInt32(4, true), false, true);
            }
            set
            {
                if (IsPaletteAnimation())
                     Array.Copy(value.ToBytes(), 0, Data, 0, 4);
                else Array.Copy(value.ToBytes(), 0, Data, 4, 4);
            }
        }
        /// <summary>
        /// The duration of this frame of the map tile animation
        /// </summary>
        public byte Duration
        {
            get
            {
                if (IsPaletteAnimation())
                     return Data[4];
                else return Data[0];
            }
            set
            {
                if (IsPaletteAnimation())
                     Data[4] = value;
                else Data[0] = value;
            }
        }
        /// <summary>
        /// The offset of the color to apply this animation to (only used if this is a palette anim)
        /// </summary>
        public byte Offset
        {
            get
            {
                if (IsPaletteAnimation())
                    return Data[6];
                else return 0x00;
            }
            set
            {
                if (IsPaletteAnimation())
                    Data[6] = value;
            }
        }
        /// <summary>
        /// The length of data pointed to by this map tile animation
        /// </summary>
        public byte Length
        {
            get
            {
                if (IsPaletteAnimation())
                     return Data[5]; // 1 per color, so 2 bytes
                else return Data[3]; // 1 per map tile, so 16x16 4bpp data
            }
            set
            {
                if (IsPaletteAnimation())
                     Data[5] = value;
                else Data[3] = value;
            }
        }



        /// <summary>
        /// Returns true if this MapTileFrame struct is a terminator - 8 bytes of 0x00
        /// </summary>
        public bool IsTerminator()
        {
            for (int i = 0; i < LENGTH; i++)
            {
                if (Data[i] != 0x00) return false;
            }
            return true;
        }
        /// <summary>
        /// Returns true if this struct indicates a palette change frame (the pointer is at +0x04 and not +0x00)
        /// </summary>
        /// <returns></returns>
        public bool IsPaletteAnimation()
        {
            return (Data[7] == 0x00);
        }
        /// <summary>
        /// Returns the 8-length byte array for this MapTileFrame struct
        /// </summary>
        public byte[] ToBytes()
        {
            return Data;
        }
    }



    public class MapTileAnim
    {
        public const int LENGTH = 8;

        public Pointer Address;
        public List<MapTileFrame> Frames;
        
        public MapTileAnim(Pointer address)
        {
            Address = address;
            Frames = new List<MapTileFrame>();
            if (address == new Pointer())
            {
                return;
            }
            byte[] frame;
            int index = 0;
            do
            {
                frame = Core.ReadData(address + index * MapTileFrame.LENGTH, MapTileFrame.LENGTH);
                Frames.Add(new MapTileFrame(frame));
            }
            while (!Frames[index++].IsTerminator());
        }
        


        public byte[] ToBytes()
        {
            byte[] result = new byte[Frames.Count * MapTileFrame.LENGTH];
            for (int i = 0; i < Frames.Count; i++)
            {
                Array.Copy(Frames[i].ToBytes(), 0, result, i * MapTileFrame.LENGTH, MapTileFrame.LENGTH);
            }
            return result;
        }
    }
}
