using System;
using System.Collections.Generic;

namespace GBA
{
    /// <summary>
    /// This class is simply a wrapper for a 16-bit unsigned int from which TSA information is read
    /// </summary>
    public struct TSA
    {
        /// <summary>
        /// The maximum amount of tiles for a TSA array (the number goes from 0x0000 to 0x03FF)
        /// </summary>
        public const Int32 MAX_TILES = 1024;

        // TSA info format is 'PPPP VHTT TTTT TTTT', so:
        // - 4bits for palette index
        // - 1bit for vertical flip
        // - 1bit for horizontal flip
        // - 10bits for tile index
        public const UInt16 BITS_PALETTE = 0xF000;
        public const UInt16 BITS_FLIPH   = 0x0400;
        public const UInt16 BITS_FLIPV   = 0x0800;
        public const UInt16 BITS_TILE    = 0x03FF;

        public const UInt16 MASK_PALETTE = 0x0FFF;
        public const UInt16 MASK_FLIPH   = 0xFBFF;
        public const UInt16 MASK_FLIPV   = 0xF7FF;
        public const UInt16 MASK_TILE    = 0xFC00;



        public TSA(UInt16 value)
        {
            Value = value;
        }
        public TSA(Int32 tile, Byte palette, Boolean flipH, Boolean flipV)
        {
            Value = (UInt16)((palette << 12) |
                (flipV ? BITS_FLIPV : 0) |
                (flipH ? BITS_FLIPH : 0) |
                (tile & 0x3FF));
        }



        /// <summary>
        /// The TSA is basically just a wrapper for this field - the actual value of the TSA item in the array
        /// </summary>
        public UInt16 Value;



        /// <summary>
        /// The index of the tile to appear (goes from 0x0000 to 0x03FF)
        /// </summary>
        public UInt16 TileIndex
        {
            get
            {
                return (UInt16)(Value & BITS_TILE);
            }
            set
            {
                if (value > BITS_TILE) throw new Exception("Invalid TSA tile index given: " + value);

                Value = (UInt16)((Value & MASK_TILE) | value);
            }
        }
        /// <summary>
        /// The 0-15 index of the palette for the tile of this TSA item
        /// </summary>
        public Byte Palette
        {
            get
            {
                return (Byte)((Value & BITS_PALETTE) >> 12);
            }
            set
            {
                if (value >= 16) throw new Exception("TSA cannot have more than 16 palettes.");

                Value = (UInt16)((Value & MASK_PALETTE) | (value << 12));
            }
        }
        /// <summary>
        /// The horizontal flip flag for the tile of this TSA item
        /// </summary>
        public Boolean FlipH
        {
            get
            {
                return ((Value & BITS_FLIPH) == BITS_FLIPH);
            }
            set
            {
                Value = (value) ?
                    (UInt16)(Value | BITS_FLIPH) :
                    (UInt16)(Value & MASK_FLIPH);
            }
        }
        /// <summary>
        /// The vertical flip flag for the tile of this TSA item
        /// </summary>
        public Boolean FlipV
        {
            get
            {
                return ((Value & BITS_FLIPV) == BITS_FLIPV);
            }
            set
            {
                Value = (value) ?
                    (UInt16)(Value | BITS_FLIPV) :
                    (UInt16)(Value & MASK_FLIPV);
            }
        }
    }
}
