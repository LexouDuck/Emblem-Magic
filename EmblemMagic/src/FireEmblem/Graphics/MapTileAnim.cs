using System;
using System.Collections.Generic;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public struct MapTileFrame
    {
        public const Int32 LENGTH = 8;

        Byte[] Data;

        public MapTileFrame(Byte[] data)
        {
            if (data.Length != LENGTH) throw new Exception("Map tile frame length is invalid: " + data.Length);

            this.Data = data;
        }



        /// <summary>
        /// The pointer to the data used for this frame (can be palette data or tileset data)
        /// </summary>
        public Pointer FrameData
        {
            get
            {
                if (this.IsPaletteAnimation())
                     return new Pointer(this.Data.GetUInt32(0, true), false, true);
                else return new Pointer(this.Data.GetUInt32(4, true), false, true);
            }
            set
            {
                if (this.IsPaletteAnimation())
                     Array.Copy(value.ToBytes(), 0, this.Data, 0, 4);
                else Array.Copy(value.ToBytes(), 0, this.Data, 4, 4);
            }
        }
        /// <summary>
        /// The duration of this frame of the map tile animation
        /// </summary>
        public Byte Duration
        {
            get
            {
                if (this.IsPaletteAnimation())
                     return this.Data[4];
                else return this.Data[0];
            }
            set
            {
                if (this.IsPaletteAnimation())
                    this.Data[4] = value;
                else this.Data[0] = value;
            }
        }
        /// <summary>
        /// The offset of the color to apply this animation to (only used if this is a palette anim)
        /// </summary>
        public Byte Offset
        {
            get
            {
                if (this.IsPaletteAnimation())
                    return this.Data[6];
                else return 0x00;
            }
            set
            {
                if (this.IsPaletteAnimation())
                    this.Data[6] = value;
            }
        }
        /// <summary>
        /// The length of data pointed to by this map tile animation
        /// </summary>
        public Byte Length
        {
            get
            {
                if (this.IsPaletteAnimation())
                     return this.Data[5]; // 1 per color, so 2 bytes
                else return this.Data[3]; // 1 per map tile, so 16x16 4bpp data
            }
            set
            {
                if (this.IsPaletteAnimation())
                    this.Data[5] = value;
                else this.Data[3] = value;
            }
        }



        /// <summary>
        /// Returns true if this MapTileFrame struct is a terminator - 8 bytes of 0x00
        /// </summary>
        public Boolean IsTerminator()
        {
            for (Int32 i = 0; i < LENGTH; i++)
            {
                if (this.Data[i] != 0x00) return false;
            }
            return true;
        }
        /// <summary>
        /// Returns true if this struct indicates a palette change frame (the pointer is at +0x04 and not +0x00)
        /// </summary>
        /// <returns></returns>
        public Boolean IsPaletteAnimation()
        {
            return (this.Data[7] == 0x00);
        }
        /// <summary>
        /// Returns the 8-length byte array for this MapTileFrame struct
        /// </summary>
        public Byte[] ToBytes()
        {
            return this.Data;
        }
    }



    public class MapTileAnim
    {
        public const Int32 LENGTH = 8;

        public Pointer Address;
        public List<MapTileFrame> Frames;
        
        public MapTileAnim(Pointer address)
        {
            this.Address = address;
            this.Frames = new List<MapTileFrame>();
            if (address == new Pointer())
            {
                return;
            }
            Byte[] frame;
            Int32 index = 0;
            do
            {
                frame = Core.ReadData(address + index * MapTileFrame.LENGTH, MapTileFrame.LENGTH);
                this.Frames.Add(new MapTileFrame(frame));
            }
            while (!this.Frames[index++].IsTerminator());
        }
        


        public Byte[] ToBytes()
        {
            Byte[] result = new Byte[this.Frames.Count * MapTileFrame.LENGTH];
            for (Int32 i = 0; i < this.Frames.Count; i++)
            {
                Array.Copy(this.Frames[i].ToBytes(), 0, result, i * MapTileFrame.LENGTH, MapTileFrame.LENGTH);
            }
            return result;
        }
    }
}
