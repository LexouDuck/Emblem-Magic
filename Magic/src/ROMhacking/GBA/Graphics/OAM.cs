using System;
using System.Drawing;
using System.Globalization;
using Magic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace GBA
{
    /// <summary>
    /// This struct is a wrapper to interpret 12-byte OAM sprite data blocks
    /// </summary>
    public class OAM
    {
        public static readonly OAM NULL = new OAM(new Byte[OAM.LENGTH]);

        /// <summary>
        /// The length of one block of OAM data
        /// </summary>
        public const Int32 LENGTH = 12;



        /// <summary>
        /// The entire struct is just a wrapper for this field.
        /// </summary>
        Byte[] Data;



        public OAM(Byte[] data)
        {
            if (data.Length != LENGTH)
                throw new Exception("OAM data block length given is invalid: " + data.Length);

            // Length is 12 bytes per sprite OAM
            // Fire Emblem stores OAM a bit differently, since it wants 16bit signed ints for the X and Y coordinates
            //Byte|_Length_|______________________________________Description_________________________________________|
            //__0_|_1 byte_|_ID, is 0x00 for normal OAM entry , or 0x01 for the terminator of the OAM array___________|
            //  1 |bits 0-1| OBJ_Mode - the kind of rendering, whether the sprite supports affine transforms or not   |
            //  1 |bits 2-3| GFX_Mode - whether or not the sprite supports alpha blending or alpha masking            |
            //  1 |  bit 4 | DrawMosaic Flag, blocks out select lines of pixels to draw a 'blocky' sprite             |
            //  1 |  bit 5 | FullColors Flag, 16 colors (4bpp) if cleared; 256 colors (8bpp) if set.                  |
            //__1_|bits 6-7|_OAM.Shape - this and OAM.Size determine the sprite's actual size_________________________|
            //__2_|_1 byte_|_Is always 0x00___________________________________________________________________________|
            //  3 |0     __|                                                                                          |
            //  3 |1    |  | ID is read if this is an affine OAM sprite - it's the transform data ID                  |
            //  3 |2    |  |                                                                                          |
            //  3 |3____|ID| fH is Flip Horizontal, read on regular non-affine OAM sprites                            |
            //  3 |4_fH_|  | fV is Flip Vertical, read on regular non-affine OAM sprites                              |
            //  3 |5_fV_|__|                                                                                          |
            //  3 |6  Size | Size is OAM.Size - determines the sprite's size, along with OAM.Shape (both are 2bits)   |
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

            this.Data = new Byte[LENGTH];
            Array.Copy(data, this.Data, LENGTH);
        }
        public OAM(
            OAM.Shape shape,
            OAM.Size size,
            Int16 screenX, Int16 screenY,
            Byte priority,
            Byte palette,
            OAM.GFXMode gfxMode,
            OAM.OBJMode objMode,
            Boolean fullColor,
            Boolean mosaic,
            Byte sheetX, Byte sheetY,
            Boolean flipH, Boolean flipV)
        {
            this.Data = new Byte[LENGTH];

            this.SpriteShape = shape;
            this.SpriteSize = size;

            this.ScreenX = screenX;
            this.ScreenY = screenY;

            this.ModeGFX = gfxMode;
            this.ModeOBJ = objMode;

            this.FullColors = fullColor;
            this.DrawMosaic = mosaic;

            this.SheetX = sheetX;
            this.SheetY = sheetY;

            this.Priority = priority;
            this.Palette = palette;

            this.FlipH = flipH;
            this.FlipV = flipV;
        }
        public OAM(
            OAM.Shape shape,
            OAM.Size size,
            Int16 screenX, Int16 screenY,
            Byte priority,
            Byte palette,
            OAM.GFXMode gfxMode,
            OAM.OBJMode objMode,
            Boolean fullColor,
            Boolean mosaic,
            Byte sheetX, Byte sheetY,
            Byte affineIndex)
        {
            this.Data = new Byte[LENGTH];

            this.SpriteShape = shape;
            this.SpriteSize = size;

            this.ScreenX = screenX;
            this.ScreenY = screenY;

            this.ModeGFX = gfxMode;
            this.ModeOBJ = objMode;

            this.FullColors = fullColor;
            this.DrawMosaic = mosaic;

            this.SheetX = sheetX;
            this.SheetY = sheetY;

            this.Priority = priority;
            this.Palette = palette;

            this.AffineIndex = affineIndex;
        }

        //================================= Normal OAM accessors ===================================

        /// <summary>
        /// The general shape of the OAM sprite - coupled with 'SpriteSize', determines the actual sprite dimensions
        /// </summary>
        public OAM.Shape SpriteShape
        {
            get
            {
                switch ((this.Data[1] >> 6) & 0x3)
                {
                    case (Byte)OAM.Shape.Square: return OAM.Shape.Square;
                    case (Byte)OAM.Shape.Rect_H: return OAM.Shape.Rect_H;
                    case (Byte)OAM.Shape.Rect_V: return OAM.Shape.Rect_V;
                    default: return OAM.Shape.Invalid;
                }
            }
            set
            {
                this.Data[1] &= (Byte)(0x3F);
                this.Data[1] |= (Byte)(((Byte)value & 0x3) << 6);
            }
        }
        /// <summary>
        /// The general size of the OAM sprite - coupled with 'SpriteShape', determines the actual sprite dimensions
        /// </summary>
        public OAM.Size SpriteSize
        {
            get
            {
                switch ((this.Data[3] >> 6) & 0x3)
                {
                    case (Byte)OAM.Size.Times1: return OAM.Size.Times1;
                    case (Byte)OAM.Size.Times2: return OAM.Size.Times2;
                    case (Byte)OAM.Size.Times4: return OAM.Size.Times4;
                    case (Byte)OAM.Size.Times8: return OAM.Size.Times8;
                    default: return 0;
                }
            }
            set
            {
                this.Data[3] &= (Byte)(0x3F);
                this.Data[3] |= (Byte)(((Byte)value & 0x3) << 6);
            }
        }

        /// <summary>
        /// What kind of rendering attributes this sprite has, regarding alpha channels
        /// </summary>
        public OAM.GFXMode ModeGFX
        {
            get
            {
                switch ((this.Data[1] >> 2) & 0x3)
                {
                    case (Byte)OAM.GFXMode.Normal: return OAM.GFXMode.Normal;
                    case (Byte)OAM.GFXMode.AlphaBlend: return OAM.GFXMode.AlphaBlend;
                    case (Byte)OAM.GFXMode.OBJ_Window: return OAM.GFXMode.OBJ_Window;
                    default: return OAM.GFXMode.Invalid;
                }
            }
            set
            {
                this.Data[1] &= (Byte)(0xF3);
                this.Data[1] |= (Byte)(((Byte)value & 0x3) << 2);
            }
        }
        /// <summary>
        /// What kind of rendering attributes this sprite has, regarding sprite transformations
        /// </summary>
        public OAM.OBJMode ModeOBJ
        {
            get
            {
                switch (this.Data[1] & 0x03)
                {
                    case (Byte)OAM.OBJMode.Normal: return OAM.OBJMode.Normal;
                    case (Byte)OAM.OBJMode.Affine: return OAM.OBJMode.Affine;
                    case (Byte)OAM.OBJMode.Hidden: return OAM.OBJMode.Hidden;
                    case (Byte)OAM.OBJMode.BigAffine: return OAM.OBJMode.BigAffine;
                    default: return 0;
                }
            }
            set
            {
                this.Data[1] &= (Byte)(0xFC);
                this.Data[1] |= (Byte)((Byte)value & 0x3);
            }
        }

        /// <summary>
        /// The X position at which the sprite is to be rendered
        /// </summary>
        public Int16 ScreenX
        {
            get
            {
                return Util.BytesToInt16(new Byte[2] { this.Data[6], this.Data[7] }, true);
            }
            set
            {
                Byte[] result = Util.Int16ToBytes(value, false);
                this.Data[6] = result[1]; this.Data[7] = result[0];
            }
        }
        /// <summary>
        /// The Y position at which the sprite is to be rendered
        /// </summary>
        public Int16 ScreenY
        {
            get
            {
                return Util.BytesToInt16(new Byte[2] { this.Data[8], this.Data[9] }, true);
            }
            set
            {
                Byte[] result = Util.Int16ToBytes(value, false);
                this.Data[8] = result[1]; this.Data[9] = result[0];
            }
        }

        /// <summary>
        /// The 0-3 priority at which this sprite should be drawn (if priority is the same, higher Y sprites are drawn first)
        /// </summary>
        public Byte Priority
        {
            get
            {
                return (Byte)((this.Data[5] >> 2) & 0x3);
            }
            set
            {
                this.Data[5] &= (Byte)(0xF3);
                this.Data[5] |= (Byte)((value & 0x3) << 2);
            }
        }
        /// <summary>
        /// The 0-15 index of the palette-bank this OAM should use (useless if FullColors==true)
        /// </summary>
        public Byte Palette
        {
            get
            {
                return (Byte)((this.Data[5] >> 4) & 0xF);
            }
            set
            {
                this.Data[5] &= (Byte)(0x0F);
                this.Data[5] |= (Byte)((value & 0xF) << 4);
            }
        }

        /// <summary>
        /// The X coordinate of the upper left tile of this sprite on the sheet
        /// </summary>
        public Byte SheetX
        {
            get
            {
                return (Byte)(this.Data[4] & 0x1F);
            }
            set
            {
                this.Data[4] = (Byte)((value & 0x1F) | ((this.SheetY << 5) & 0xE0));
            }
        }
        /// <summary>
        /// The Y coordinate of the upper left tile of this sprite on the sheet
        /// </summary>
        public Byte SheetY
        {
            get
            {
                return (Byte)((this.Data[4] & 0xE0) >> 5);
            }
            set
            {
                this.Data[4] = (Byte)((this.SheetX & 0x1F) | ((value << 5) & 0xE0));
            }
        }

        /// <summary>
        /// Whether or not this OAM sprite is to show up flipped horizontally - unused if affine OAM
        /// </summary>
        public Boolean FlipH
        {
            get
            {
                return (((this.Data[3] >> 4) & 0x1) == 1);
            }
            set
            {
                if (value) this.Data[3] |= (0x1 << 4);
                else this.Data[3] &= (Byte)(0xEF);
            }
        }
        /// <summary>
        /// Whether or not this OAM sprite is to show up flipped vertically - unused if affine OAM
        /// </summary>
        public Boolean FlipV
        {
            get
            {
                return (((this.Data[3] >> 5) & 0x1) == 1);
            }
            set
            {
                if (value) this.Data[3] |= (0x1 << 5);
                else this.Data[3] &= (Byte)(0xDF);
            }
        }

        /// <summary>
        /// if true, OAM sprite is in 256-color mode, otherwise it is in 16-color palettes mode
        /// </summary>
        public Boolean FullColors
        {
            get
            {
                return (((this.Data[1] >> 5) & 0x1) == 0x1);
            }
            set
            {
                if (value) this.Data[1] |= (0x1 << 5);
                else this.Data[1] &= (Byte)(0xDF);
            }
        }
        /// <summary>
        /// If true, the sprite renders as streched, only the first pixels of each row being drawn and repeated
        /// </summary>
        public Boolean DrawMosaic
        {
            get
            {
                return (((this.Data[1] >> 4) & 0x1) == 0x1);
            }
            set
            {
                if (value) this.Data[1] |= (0x1 << 4);
                else this.Data[1] &= (Byte)(0xEF);
            }
        }
        
        /// <summary>
        /// The index of the affine transform parameter block for this sprite, unused if GFX mode is not affine
        /// </summary>
        public Byte AffineIndex
        {
            get
            {
                return (Byte)((this.Data[3] & 0x3E) >> 1);
            }
            set
            {
                this.Data[3] &= (Byte)(0xC1);
                this.Data[3] |= (Byte)((value & 0x1F) << 1);
            }
        }



        /// <summary>
        /// Returns whether or not this OAM struct is simply an empty OAM block terminator
        /// </summary>
        public Boolean IsTerminator()
        {
            return (this.Data[0] == 0x01);
        }
        /// <summary>
        /// Returns whether or not this OAM sprite supports affine tranforms and rotation
        /// </summary>
        public Boolean IsAffineSprite()
        {
            return ((this.Data[1] & 0x1) == 0x1);
        }
        
        /// <summary>
        /// Returns a boolean indicating whether or not 'this' and 'oam' have the same value.
        /// </summary>
        public Boolean Equals(OAM oam)
        {
            for (Int32 i = 0; i < LENGTH; i++)
            {
                if (this.Data[i] != oam.Data[i])
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns a string describing this OAM struct
        /// </summary>
        public override String ToString()
        {
            System.Drawing.Size size = this.GetDimensions() * 8;
            return (
                "X:" + this.ScreenX.ToString().PadRight(6) +
                "Y:" + this.ScreenY.ToString().PadRight(6) +
                size.Width .ToString().PadLeft(2) + "x" +
                size.Height.ToString().PadLeft(2) + " ");
        }
        /// <summary>
        /// Returns a 12-length byte array of this OAM struct
        /// </summary>
        public Byte[] ToBytes()
        {
            return this.Data;
        }
        /// <summary>
        /// Returns the actual size of this OAM Sprite, in tiles- not pixels
        /// </summary>
        public System.Drawing.Size GetDimensions()
        {
            switch (this.SpriteShape)
            {
                case OAM.Shape.Square: switch (this.SpriteSize)
                    {
                        case OAM.Size.Times1: return new System.Drawing.Size(1, 1);
                        case OAM.Size.Times2: return new System.Drawing.Size(2, 2);
                        case OAM.Size.Times4: return new System.Drawing.Size(4, 4);
                        case OAM.Size.Times8: return new System.Drawing.Size(8, 8);
                        default: return new System.Drawing.Size();
                    }
                case OAM.Shape.Rect_H: switch (this.SpriteSize)
                    {
                        case OAM.Size.Times1: return new System.Drawing.Size(2, 1);
                        case OAM.Size.Times2: return new System.Drawing.Size(4, 1);
                        case OAM.Size.Times4: return new System.Drawing.Size(4, 2);
                        case OAM.Size.Times8: return new System.Drawing.Size(8, 4);
                        default: return new System.Drawing.Size();
                    }
                case OAM.Shape.Rect_V: switch (this.SpriteSize)
                    {
                        case OAM.Size.Times1: return new System.Drawing.Size(1, 2);
                        case OAM.Size.Times2: return new System.Drawing.Size(1, 4);
                        case OAM.Size.Times4: return new System.Drawing.Size(2, 4);
                        case OAM.Size.Times8: return new System.Drawing.Size(4, 8);
                        default: return new System.Drawing.Size();
                    }
                default: return new System.Drawing.Size();
            }
        }
        /// <summary>
        /// Returns a byte map of the tile indices for this OAM based on its SheetIndex
        /// </summary>
        public Int32?[,] GetTileMap(System.Drawing.Size size)
        {
            Int32?[,] result = new Int32?[size.Width, size.Height];
            Int32 index;
            for (Int32 y = 0; y < size.Height; y++)
            for (Int32 x = 0; x < size.Width; x++)
            {
                index = (this.SheetX + x) + (this.SheetY + y) * 32;
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
                return new OAM(new Byte[12]
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
        public static Tuple<OAM.Shape, OAM.Size> GetShapeSize(System.Drawing.Size dimensions)
        {
            switch (dimensions.Width)
            {
                case 1: switch (dimensions.Height)
                    {
                        case 1: return Tuple.Create(OAM.Shape.Square, OAM.Size.Times1);
                        case 2: return Tuple.Create(OAM.Shape.Rect_V, OAM.Size.Times1);
                        case 4: return Tuple.Create(OAM.Shape.Rect_V, OAM.Size.Times2);
                        default: return null;
                    }
                case 2: switch (dimensions.Height)
                    {
                        case 1: return Tuple.Create(OAM.Shape.Rect_H, OAM.Size.Times1);
                        case 2: return Tuple.Create(OAM.Shape.Square, OAM.Size.Times2);
                        case 4: return Tuple.Create(OAM.Shape.Rect_V, OAM.Size.Times4);
                        default: return null;
                    }
                case 4: switch (dimensions.Height)
                    {
                        case 1: return Tuple.Create(OAM.Shape.Rect_H, OAM.Size.Times2);
                        case 2: return Tuple.Create(OAM.Shape.Rect_H, OAM.Size.Times4);
                        case 4: return Tuple.Create(OAM.Shape.Square, OAM.Size.Times4);
                        case 8: return Tuple.Create(OAM.Shape.Rect_V, OAM.Size.Times8);
                        default: return null;
                    }
                case 8: switch (dimensions.Height)
                    {
                        case 4: return Tuple.Create(OAM.Shape.Rect_H, OAM.Size.Times8);
                        case 8: return Tuple.Create(OAM.Shape.Square, OAM.Size.Times8);
                        default: return null;
                    }
                default: return null;
            }
        }



        public enum Shape
        {
            Square = 0x0, // the sprite is square shaped:    8x8, 16x16, 32x32 or 64x64
            Rect_H = 0x1, // the sprite is rectangle shaped: 16x8, 32x8, 32x16 or 64x32
            Rect_V = 0x2, // the sprite is rectangle shaped: 8x16, 8x32, 16x32 or 32x64
            Invalid = 0x3,
        }
        public enum Size
        {
            Times1 = 0x0, // the sprite is small:   8x8,  16x8  or  8x16
            Times2 = 0x1, // the sprite is medium: 16x16, 32x8  or  8x32
            Times4 = 0x2, // the sprite is large:  32x32, 32x16 or 16x32
            Times8 = 0x3, // the sprite is huge:   64x64, 64x32 or 32x64
        }

        public enum GFXMode
        {
            Normal     = 0x0, // the sprite renders normally
            AlphaBlend = 0x1, // the sprite is rendered with semi-transparency, blended with graphics behind it
            OBJ_Window = 0x2, // the sprite is rendered as an alpha mask onto which another sprite/BG is displayed
            Invalid    = 0x3,
        }
        public enum OBJMode
        {
            Normal    = 0x0, // the sprite renders normally                                       [ Ux Vx ]
            Affine    = 0x1, // the sprite is an affine sprite, transformed along the matrix: A = [ Uy Vy ]
            Hidden    = 0x2, // the sprite is not rendered
            BigAffine = 0x3, // the sprite is an affine sprite with a 2x bigger draw surface, to accomodate rotations
        }



        /// <summary>
        /// This struct is a wrapper to interpret 12-byte OAM affine sprite transformation data blocks
        /// </summary>
        public class Affine
        {
            public static readonly Affine IDENTITY = new(
                1, 0,
                0, 1);

            const Int16 FIXEDPT_DENOMINATOR = 0x100;

            Int16 a;
            Int16 b;
            Int16 c;
            Int16 d;

            public Affine(Double u_x, Double u_y, Double v_x, Double v_y)
            {
                this.Ux = u_x;
                this.Uy = u_y;
                this.Vx = v_x;
                this.Vy = v_y;
            }
            public Affine(Byte[] data)
            {
                if (data[0] == 0x00) throw new Exception("Invalid OAM Affine data read. It should not start with 0x00.");
                // data format is 01 00 Cx Cy |U-x||V-x| |U-y||V-y|
                UInt32 index = 4;
                this.a = data.GetInt16(index, true); index += 2;
                this.b = data.GetInt16(index, true); index += 2;
                this.c = data.GetInt16(index, true); index += 2;
                this.d = data.GetInt16(index, true); index += 2;
            }

            public Double Ux { get { return (this.a / (Double)FIXEDPT_DENOMINATOR); } set { this.a = (Int16)(value * FIXEDPT_DENOMINATOR); } }
            public Double Uy { get { return (this.b / (Double)FIXEDPT_DENOMINATOR); } set { this.b = (Int16)(value * FIXEDPT_DENOMINATOR); } }
            public Double Vx { get { return (this.c / (Double)FIXEDPT_DENOMINATOR); } set { this.c = (Int16)(value * FIXEDPT_DENOMINATOR); } }
            public Double Vy { get { return (this.d / (Double)FIXEDPT_DENOMINATOR); } set { this.d = (Int16)(value * FIXEDPT_DENOMINATOR); } }

            /// <summary>
            /// Returns a boolean indicating whether or not 'this' and 'other' have the same value.
            /// </summary>
            public Boolean Equals(Affine other)
            {
                if (this.a != other.a) return false;
                if (this.b != other.b) return false;
                if (this.c != other.c) return false;
                if (this.d != other.d) return false;
                return true;
            }
            /// <summary>
            /// Returns a string describing this OAM struct
            /// </summary>
            public override String ToString()
            {
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                return ("{ " +
                    this.Ux.ToString("0.00", nfi).PadRight(4) + "," +
                    this.Vx.ToString("0.00", nfi).PadRight(4) + " | " +
                    this.Uy.ToString("0.00", nfi).PadRight(4) + "," +
                    this.Vy.ToString("0.00", nfi).PadRight(4) + " }");
            }
            /// <summary>
            /// Returns a 12-length byte array of this OAM affine transformation data
            /// </summary>
            public Byte[] ToBytes()
            {
                Byte[] result = new Byte[OAM.LENGTH];
                Int32 index = 0;
                result[index++] = 0x01;
                result[index++] = 0x00;
                result[index++] = 0xFF;
                result[index++] = 0xFF;
                Array.Copy(Util.Int16ToBytes(this.a, true), 0, result, index, 2); index += 2;
                Array.Copy(Util.Int16ToBytes(this.b, true), 0, result, index, 2); index += 2;
                Array.Copy(Util.Int16ToBytes(this.c, true), 0, result, index, 2); index += 2;
                Array.Copy(Util.Int16ToBytes(this.d, true), 0, result, index, 2); index += 2;
                return result;
            }



            /// <summary>
            /// This class expresses an object similar to the OAM.Affine 2x2 matrix, but with human-understandable decomposed singular values (SVD)
            /// </summary>
            public class SVD
            {
                public static readonly Affine.SVD IDENTITY = new(1, 1, 0, 0);

                /// <summary>
                /// horizontal scaling factor
                /// </summary>
                public Double ScaleX;
                /// <summary>
                /// vertical scaling factor
                /// </summary>
                public Double ScaleY;
                /// <summary>
                /// angle of rotation (in degrees)
                /// </summary>
                public Double Rotate;
                /// <summary>
                /// angle of shearing (in degrees)
                /// </summary>
                public Double Shear;

                /// <summary>
                /// holds the SVD matrices
                /// </summary>
                Svd<Double> svd;



                public SVD(Double scale_x, Double scale_y, Double rotate, Double shear)
                {
                    this.ScaleX = scale_x;
                    this.ScaleY = scale_y;
                    this.Rotate = rotate;
                    this.Shear = shear;
                }
                public SVD(Affine transform)
                {
                    Matrix<Double> matrix = CreateMatrix.Dense(2, 2,
                        new Double[4]
                        {
                            transform.Ux, transform.Vx,
                            transform.Uy, transform.Vy,
                        });
                    this.svd = matrix.Svd(true);
                    /*
                    MessageBox.Show("T: " + matrix.Transpose().ToString() +
                        "\nV*: " + svd.VT.ToString() +
                        "\nS: " + svd.S.ToString() +
                        "\nU: " + svd.U.ToString() +
                        "\n");
                    */
                    this.ScaleX = (1 / svd.S[0]);
                    this.ScaleY = (1 / svd.S[1]);

                    this.Rotate      = Math.Acos(svd.U[0, 0]);
                    if (this.Rotate != Math.Asin(svd.U[0, 1]))
                        this.Rotate *= -1;
                    this.Rotate *= (180 / Math.PI);
                    this.Rotate += 180;

                    this.Shear      = Math.Acos(svd.VT[0, 0]);
                    if (this.Shear != Math.Asin(svd.VT[0, 1]))
                        this.Shear *= -1;
                    this.Shear *= (180 / Math.PI);
                    this.Shear += 180;
                }



                public Affine ToAffine()
                {
                    Matrix<Double> sigma = CreateMatrix.Dense(2, 2,
                        new Double[4]
                        {
                            svd.S[0], 0,
                            0, svd.S[1],
                        });
                    Matrix<Double> matrix = (svd.VT + sigma + svd.U);
                    OAM.Affine result = new(
                        matrix[0, 0],
                        matrix[1, 0],
                        matrix[0, 1],
                        matrix[1, 1]);
                    return result;
                }
            }
        }
    }
}
