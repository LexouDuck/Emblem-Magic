using Magic;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GBA
{
    /// <summary>
    /// This struct is a wrapper to interpret 12-byte OAM affine sprite transformation data blocks
    /// </summary>
    public struct OAM_Affine
    {
        Int16 a;
        Int16 b;
        Int16 c;
        Int16 d;

        public OAM_Affine(float ux, float vx, float uy, float vy)
        {
            a = 0;
            b = 0;
            c = 0;
            d = 0;

            Ux = ux;
            Vx = vx;
            Uy = uy;
            Vy = vy;
        }
        public OAM_Affine(byte[] data)
        {
            if (data[0] == 0x00) throw new Exception("Invalid OAM Affine data read. It should not start with 0x00.");
            // data format is 01 00 Cx Cy |U-x||V-x| |U-y||V-y|
            uint index = 4;
            a = data.GetInt16(index, true); index += 2;
            b = data.GetInt16(index, true); index += 2;
            c = data.GetInt16(index, true); index += 2;
            d = data.GetInt16(index, true); index += 2;
        }

        public float Ux { get { return GetFloat16(a); } set { a = (Int16)(value * 256); } }
        public float Vx { get { return GetFloat16(b); } set { b = (Int16)(value * 256); } }
        public float Uy { get { return GetFloat16(c); } set { c = (Int16)(value * 256); } }
        public float Vy { get { return GetFloat16(d); } set { d = (Int16)(value * 256); } }

        /// <summary>
        /// Returns a 12-length byte array of this OAM affine transformation data
        /// </summary>
        public byte[] ToBytes()
        {
            byte[] result = new byte[OAM.LENGTH];
            int index = 0;
            result[index++] = 0x01;
            result[index++] = 0x00;
            result[index++] = 0xFF;
            result[index++] = 0xFF;
            Array.Copy(Util.Int16ToBytes(a, true), 0, result, index, 2); index += 2;
            Array.Copy(Util.Int16ToBytes(b, true), 0, result, index, 2); index += 2;
            Array.Copy(Util.Int16ToBytes(c, true), 0, result, index, 2); index += 2;
            Array.Copy(Util.Int16ToBytes(d, true), 0, result, index, 2); index += 2;
            return result;
        }
        static float GetFloat16(int value)
        {
            return ((float)value / (float)256);
        }
    }



    /// <summary>
    /// This struct is a wrapper to interpret 12-byte OAM sprite data blocks
    /// </summary>
    public struct OAM
    {
        /// <summary>
        /// The length of one block of OAM data
        /// </summary>
        public const int LENGTH = 12;
        
        /// <summary>
        /// The entire struct is just a wrapper for this field.
        /// </summary>
        byte[] Data;



        public OAM(byte[] data)
        {
            if (data.Length != LENGTH)
                throw new Exception("OAM data block length given is invalid: " + data.Length);

            // Length is 12 bytes per sprite OAM
            // Fire Emblem stores OAM a bit differently, since it wants 16bit signed ints for the X and Y coordinates
            //Byte|_Length_|______________________________________Description_________________________________________|
            //__0_|_1 byte_|_ID, is 0x00 for normal OAMentry , or 0x01 for the terminator of the OAM array____________|
            //  1 |bits 0-1| OBJ_Mode - the kind of rendering, whether the sprite supports affine transforms or not   |
            //  1 |bits 2-3| GFX_Mode - whether or not the sprite supports alpha blending or alpha masking            |
            //  1 |  bit 4 | DrawMosaic Flag, blocks out select lines of pixels to draw a 'blocky' sprite             |
            //  1 |  bit 5 | FullColors Flag, 16 colors (4bpp) if cleared; 256 colors (8bpp) if set.                  |
            //__1_|bits 6-7|_OAM_Shape - this and OAM_Size determine the sprite's actual size_________________________|
            //__2_|_1 byte_|_Is always 0x00___________________________________________________________________________|
            //  3 |0     __|                                                                                          |
            //  3 |1    |  | ID is read if this is an affine OAM sprite - it's the transform data ID                  |
            //  3 |2    |  |                                                                                          |
            //  3 |3____|ID| fH is Flip Horizontal, read on regular non-affine OAM sprites                            |
            //  3 |4_fH_|  | fV is Flip Vertical, read on regular non-affine OAM sprites                              |
            //  3 |5_fV_|__|                                                                                          |
            //  3 |6  Size | Size is OAM_Size - determines the sprite's size, along with OAM_Shape (both are 2bits)   |
            //__3_|7_______|__________________________________________________________________________________________|
            //__4_|_1 byte_|_The index of the top-left corner of this OAM sprite on the tileset_______________________|
            //  5 |bits 0-1| The rest of the tile index Y-coord rows ? i don't think it's used in Fire Emblem         |
            //  5 |bits 2-3| Sprite priority - what layer it's drawn on (a 0 to 3 number)                             |
            //__5_|bits_4-7|_Palette-bank - the index of palette to use, is unused in 256-color palette mode__________|
            //_6-7|_2_bytes|_16-bit int of the X coordinate for the sprite- add 0x58 (Left side), 0x94 (Right side)___|
            //_8-9|_2_bytes|_16-bit int of the Y coordinate for the sprite- add 0x58 for the actual screen coordinate_|
            //_10_|_1_byte_|_Is always 0x00___________________________________________________________________________|
            //_11_|_1_byte_|_Is always 0x00___________________________________________________________________________|

            if (data[2] != 0x00) throw new Exception("OAM data block byte 2 should be 0x00");

            if (data[10] != 0x00) throw new Exception("OAM data block byte 10 should be 0x00");
            if (data[11] != 0x00) throw new Exception("OAM data block byte 11 should be 0x00");

            Data = new byte[LENGTH];
            Array.Copy(data, Data, LENGTH);
        }
        public OAM(
            OAM_Shape shape,
            OAM_Size size,
            Int16 screenX, Int16 screenY,
            byte priority,
            byte palette,
            OAM_GFXMode gfxMode,
            OAM_OBJMode objMode,
            bool fullColor,
            bool mosaic,
            byte sheetX, byte sheetY,
            bool flipH, bool flipV)
        {
            Data = new byte[LENGTH];

            SpriteShape = shape;
            SpriteSize = size;

            ScreenX = screenX;
            ScreenY = screenY;

            GFXMode = gfxMode;
            OBJMode = objMode;

            FullColors = fullColor;
            DrawMosaic = mosaic;

            SheetX = sheetX;
            SheetY = sheetY;

            Priority = priority;
            Palette = palette;

            FlipH = flipH;
            FlipV = flipV;
        }
        public OAM(
            OAM_Shape shape,
            OAM_Size size,
            Int16 screenX, Int16 screenY,
            byte priority,
            byte palette,
            OAM_GFXMode gfxMode,
            OAM_OBJMode objMode,
            bool fullColor,
            bool mosaic,
            byte sheetX, byte sheetY,
            byte affineIndex)
        {
            Data = new byte[LENGTH];

            SpriteShape = shape;
            SpriteSize = size;

            ScreenX = screenX;
            ScreenY = screenY;

            GFXMode = gfxMode;
            OBJMode = objMode;

            FullColors = fullColor;
            DrawMosaic = mosaic;

            SheetX = sheetX;
            SheetY = sheetY;

            Priority = priority;
            Palette = palette;

            AffineIndex = affineIndex;
        }

        //================================= Normal OAM accessors ===================================

        /// <summary>
        /// The general shape of the OAM sprite - coupled with 'SpriteSize', determines the actual sprite dimensions
        /// </summary>
        public OAM_Shape SpriteShape
        {
            get
            {
                switch ((Data[1] >> 6) & 0x3)
                {
                    case (byte)OAM_Shape.Square: return OAM_Shape.Square;
                    case (byte)OAM_Shape.Rect_H: return OAM_Shape.Rect_H;
                    case (byte)OAM_Shape.Rect_V: return OAM_Shape.Rect_V;
                    default: return OAM_Shape.Invalid;
                }
            }
            set
            {
                Data[1] &= (byte)(0x3F);
                Data[1] |= (byte)(((byte)value & 0x3) << 6);
            }
        }
        /// <summary>
        /// The general size of the OAM sprite - coupled with 'SpriteShape', determines the actual sprite dimensions
        /// </summary>
        public OAM_Size SpriteSize
        {
            get
            {
                switch ((Data[3] >> 6) & 0x3)
                {
                    case (byte)OAM_Size.Times1: return OAM_Size.Times1;
                    case (byte)OAM_Size.Times2: return OAM_Size.Times2;
                    case (byte)OAM_Size.Times4: return OAM_Size.Times4;
                    case (byte)OAM_Size.Times8: return OAM_Size.Times8;
                    default: return 0;
                }
            }
            set
            {
                Data[3] &= (byte)(0x3F);
                Data[3] |= (byte)(((byte)value & 0x3) << 6);
            }
        }

        /// <summary>
        /// What kind of rendering attributes this sprite has, regarding alpha channels
        /// </summary>
        public OAM_GFXMode GFXMode
        {
            get
            {
                switch ((Data[1] >> 2) & 0x3)
                {
                    case (byte)OAM_GFXMode.Normal: return OAM_GFXMode.Normal;
                    case (byte)OAM_GFXMode.AlphaBlend: return OAM_GFXMode.AlphaBlend;
                    case (byte)OAM_GFXMode.OBJ_Window: return OAM_GFXMode.OBJ_Window;
                    default: return OAM_GFXMode.Invalid;
                }
            }
            set
            {
                Data[1] &= (byte)(0xF3);
                Data[1] |= (byte)(((byte)value & 0x3) << 2);
            }
        }
        /// <summary>
        /// What kind of rendering attributes this sprite has, regarding sprite transformations
        /// </summary>
        public OAM_OBJMode OBJMode
        {
            get
            {
                switch (Data[1] & 0x03)
                {
                    case (byte)OAM_OBJMode.Normal: return OAM_OBJMode.Normal;
                    case (byte)OAM_OBJMode.Affine: return OAM_OBJMode.Affine;
                    case (byte)OAM_OBJMode.Hidden: return OAM_OBJMode.Hidden;
                    case (byte)OAM_OBJMode.BigAffine: return OAM_OBJMode.BigAffine;
                    default: return 0;
                }
            }
            set
            {
                Data[1] &= (byte)(0xFC);
                Data[1] |= (byte)((byte)value & 0x3);
            }
        }

        /// <summary>
        /// The X position at which the sprite is to be rendered
        /// </summary>
        public Int16 ScreenX
        {
            get
            {
                return Util.BytesToInt16(new byte[2] { Data[6], Data[7] }, true);
            }
            set
            {
                byte[] result = Util.Int16ToBytes(value, false);
                Data[6] = result[1]; Data[7] = result[0];
            }
        }
        /// <summary>
        /// The Y position at which the sprite is to be rendered
        /// </summary>
        public Int16 ScreenY
        {
            get
            {
                return Util.BytesToInt16(new byte[2] { Data[8], Data[9] }, true);
            }
            set
            {
                byte[] result = Util.Int16ToBytes(value, false);
                Data[8] = result[1]; Data[9] = result[0];
            }
        }

        /// <summary>
        /// The 0-3 priority at which this sprite should be drawn (if priority is the same, higher Y sprites are drawn first)
        /// </summary>
        public byte Priority
        {
            get
            {
                return (byte)((Data[5] >> 2) & 0x3);
            }
            set
            {
                Data[5] &= (byte)(0xF3);
                Data[5] |= (byte)((value & 0x3) << 2);
            }
        }
        /// <summary>
        /// The 0-15 index of the palette-bank this OAM should use (useless if FullColors==true)
        /// </summary>
        public byte Palette
        {
            get
            {
                return (byte)((Data[5] >> 4) & 0xF);
            }
            set
            {
                Data[5] &= (byte)(0x0F);
                Data[5] |= (byte)((value & 0xF) << 4);
            }
        }

        /// <summary>
        /// The X coordinate of the upper left tile of this sprite on the sheet
        /// </summary>
        public byte SheetX
        {
            get
            {
                return (byte)(Data[4] & 0x1F);
            }
            set
            {
                Data[4] = (byte)((value & 0x1F) | ((SheetY << 5) & 0xE0));
            }
        }
        /// <summary>
        /// The Y coordinate of the upper left tile of this sprite on the sheet
        /// </summary>
        public byte SheetY
        {
            get
            {
                return (byte)((Data[4] & 0xE0) >> 5);
            }
            set
            {
                Data[4] = (byte)((SheetX & 0x1F) | ((value << 5) & 0xE0));
            }
        }

        /// <summary>
        /// Whether or not this OAM sprite is to show up flipped horizontally - unused if affine OAM
        /// </summary>
        public bool FlipH
        {
            get
            {
                return (((Data[3] >> 4) & 0x1) == 1);
            }
            set
            {
                if (value) Data[3] |= (0x1 << 4);
                else Data[3] &= (byte)(0xEF);
            }
        }
        /// <summary>
        /// Whether or not this OAM sprite is to show up flipped vertically - unused if affine OAM
        /// </summary>
        public bool FlipV
        {
            get
            {
                return (((Data[3] >> 5) & 0x1) == 1);
            }
            set
            {
                if (value) Data[3] |= (0x1 << 5);
                else Data[3] &= (byte)(0xDF);
            }
        }

        /// <summary>
        /// if true, OAM sprite is in 256-color mode, otherwise it is in 16-color palettes mode
        /// </summary>
        public bool FullColors
        {
            get
            {
                return (((Data[1] >> 5) & 0x1) == 0x1);
            }
            set
            {
                if (value) Data[1] |= (0x1 << 5);
                else Data[1] &= (byte)(0xDF);
            }
        }
        /// <summary>
        /// If true, the sprite renders as streched, only the first pixels of each row being drawn and repeated
        /// </summary>
        public bool DrawMosaic
        {
            get
            {
                return (((Data[1] >> 4) & 0x1) == 0x1);
            }
            set
            {
                if (value) Data[1] |= (0x1 << 4);
                else Data[1] &= (byte)(0xEF);
            }
        }
        
        /// <summary>
        /// The index of the affine transform parameter block for this sprite, unused if GFX mode is not affine
        /// </summary>
        public byte AffineIndex
        {
            get
            {
                return (byte)((Data[3] & 0x3E) >> 1);
            }
            set
            {
                Data[3] &= (byte)(0xC1);
                Data[3] |= (byte)((value & 0x1F) << 1);
            }
        }



        /// <summary>
        /// Returns whether or not this OAM struct is simply an empty OAM block terminator
        /// </summary>
        public Boolean IsTerminator()
        {
            return (Data[0] == 0x01);
        }
        /// <summary>
        /// Returns whether or not this OAM sprite supports affine tranforms and rotation
        /// </summary>
        public Boolean IsAffineSprite()
        {
            return ((Data[1] & 0x1) == 0x1);
        }
        
        /// <summary>
        /// Returns a boolean indicating whether or not 'this' and 'oam' have the same value.
        /// </summary>
        public bool Equals(OAM oam)
        {
            for (int i = 0; i < LENGTH; i++)
            {
                if (Data[i] != oam.Data[i])
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Returns a 12-length byte array of this OAM struct
        /// </summary>
        public Byte[] ToBytes()
        {
            return Data;
        }
        /// <summary>
        /// Returns the actual size of this OAM Sprite, in tiles- not pixels
        /// </summary>
        public Size GetDimensions()
        {
            switch (SpriteShape)
            {
                case OAM_Shape.Square: switch (SpriteSize)
                    {
                        case OAM_Size.Times1: return new Size(1, 1);
                        case OAM_Size.Times2: return new Size(2, 2);
                        case OAM_Size.Times4: return new Size(4, 4);
                        case OAM_Size.Times8: return new Size(8, 8);
                        default: return new Size();
                    }
                case OAM_Shape.Rect_H: switch (SpriteSize)
                    {
                        case OAM_Size.Times1: return new Size(2, 1);
                        case OAM_Size.Times2: return new Size(4, 1);
                        case OAM_Size.Times4: return new Size(4, 2);
                        case OAM_Size.Times8: return new Size(8, 4);
                        default: return new Size();
                    }
                case OAM_Shape.Rect_V: switch (SpriteSize)
                    {
                        case OAM_Size.Times1: return new Size(1, 2);
                        case OAM_Size.Times2: return new Size(1, 4);
                        case OAM_Size.Times4: return new Size(2, 4);
                        case OAM_Size.Times8: return new Size(4, 8);
                        default: return new Size();
                    }
                default: return new Size();
            }
        }
        /// <summary>
        /// Returns a byte map of the tile indices for this OAM based on its SheetIndex
        /// </summary>
        public int?[,] GetTileMap(Size size)
        {
            int?[,] result = new int?[size.Width, size.Height];
            int index;
            for (int y = 0; y < size.Height; y++)
            for (int x = 0; x < size.Width; x++)
            {
                index = (SheetX + x) + (SheetY + y) * 32;
                // the tilesets are 32x8 tilesheets
                result[x, y] = index;
            }
            return result;
        }
        
        /// <summary>
        /// Returns an empty/terminator OAM struct
        /// </summary>
        public static OAM Terminator
        {
            get
            {
                return new OAM(new byte[12]
                {
                    0x01,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x00,
                    0x00
                });
            }
        }
        /// <summary>
        /// Returns the correct OAM shape and size for the dimensions given (in tiles), or null if argument is invalid
        /// </summary>
        public static Tuple<OAM_Shape, OAM_Size> GetShapeSize(Size dimensions)
        {
            switch (dimensions.Width)
            {
                case 1: switch (dimensions.Height)
                    {
                        case 1: return Tuple.Create(OAM_Shape.Square, OAM_Size.Times1);
                        case 2: return Tuple.Create(OAM_Shape.Rect_V, OAM_Size.Times1);
                        case 4: return Tuple.Create(OAM_Shape.Rect_V, OAM_Size.Times2);
                        default: return null;
                    }
                case 2: switch (dimensions.Height)
                    {
                        case 1: return Tuple.Create(OAM_Shape.Rect_H, OAM_Size.Times1);
                        case 2: return Tuple.Create(OAM_Shape.Square, OAM_Size.Times2);
                        case 4: return Tuple.Create(OAM_Shape.Rect_V, OAM_Size.Times4);
                        default: return null;
                    }
                case 4: switch (dimensions.Height)
                    {
                        case 1: return Tuple.Create(OAM_Shape.Rect_H, OAM_Size.Times2);
                        case 2: return Tuple.Create(OAM_Shape.Rect_H, OAM_Size.Times4);
                        case 4: return Tuple.Create(OAM_Shape.Square, OAM_Size.Times4);
                        case 8: return Tuple.Create(OAM_Shape.Rect_V, OAM_Size.Times8);
                        default: return null;
                    }
                case 8: switch (dimensions.Height)
                    {
                        case 4: return Tuple.Create(OAM_Shape.Rect_H, OAM_Size.Times8);
                        case 8: return Tuple.Create(OAM_Shape.Square, OAM_Size.Times8);
                        default: return null;
                    }
                default: return null;
            }
        }
    }



    public enum OAM_Shape
    {
        Square = 0x0, // the sprite is square shaped:    8x8, 16x16, 32x32 or 64x64
        Rect_H = 0x1, // the sprite is rectangle shaped: 16x8, 32x8, 32x16 or 64x32
        Rect_V = 0x2, // the sprite is rectangle shaped: 8x16, 8x32, 16x32 or 32x64
        Invalid = 0x3,
    }
    public enum OAM_Size
    {
        Times1 = 0x0, // the sprite is small:   8x8,  16x8  or  8x16
        Times2 = 0x1, // the sprite is medium: 16x16, 32x8  or  8x32
        Times4 = 0x2, // the sprite is large:  32x32, 32x16 or 16x32
        Times8 = 0x3, // the sprite is huge:   64x64, 64x32 or 32x64
    }

    public enum OAM_GFXMode
    {
        Normal     = 0x0, // the sprite renders normally
        AlphaBlend = 0x1, // the sprite is rendered with semi-transparency, blended with graphics behind it
        OBJ_Window = 0x2, // the sprite is rendered as an alpha mask onto which another sprite/BG is displayed
        Invalid    = 0x3,
    }
    public enum OAM_OBJMode
    {
        Normal    = 0x0, // the sprite renders normally                                       [ Ux Vx ]
        Affine    = 0x1, // the sprite is an affine sprite, transformed along the matrix: A = [ Uy Vy ]
        Hidden    = 0x2, // the sprite is not rendered
        BigAffine = 0x3, // the sprite is an affine sprite with a 2x bigger draw surface, to accomodate rotations
    }
}
