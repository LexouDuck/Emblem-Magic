using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    /// <summary>
    /// This class allows for creation of battle animations from files
    /// </summary>
    public class BattleAnimMaker
    {
        // state enum for the anim code parser
        enum CompileMode
        {
            Usual,
            Frame,
            Extra
        }



        /// <summary>
        /// The animation code translated from the text file's code
        /// </summary>
        public List<String>[] AnimCode;
        /// <summary>
        /// The OAM data for the sprites of each frame
        /// </summary>
        public List<OAM_Array> Frames;
        /// <summary>
        /// The tileset index and OAM offset for each frame (tileset index will be made into a pointer on insert)
        /// </summary>
        public List<Tuple<UInt32, UInt32>> FrameData;
        /// <summary>
        /// The (usually) 4 palettes for this battle animation
        /// </summary>
        public Palette[] Palettes;
        /// <summary>
        /// The tilesheets used to construct this battle animation with OAM
        /// </summary>
        public List<TileSheet> Graphics;



        /// <summary>
        /// Processes a battle animation from a txt file, reading all referenced image files
        /// </summary>
        public BattleAnimMaker(String folder, String[] file)
        {
            FormLoading loading = new FormLoading();
            loading.SetMessage("Processing animation...");
            loading.Show();

            this.AnimCode = new List<String>[12];
            this.Graphics = new List<TileSheet>();
            for (Int32 i = 0; i < this.AnimCode.Length; i++)
            {
                this.AnimCode[i] = new List<String>();
            }
            this.AddTileSheet();

            this.Palettes = Palette.Split(GetPaletteFromFile(folder), 4);



            this.Frames = new List<OAM_Array>();
            this.FrameData = new List<Tuple<UInt32, UInt32>>();
            List<String> filenames = new List<String>();
            List<Tuple<String, Point, Size>> affines = new List<Tuple<String, Point, Size>>();
            CompileMode compile = CompileMode.Usual;
            Boolean new_frame = true;
            Int32 mode = 0; // the current animation mode being processed
            Int32 frame = 0; // the current frame number
            Int32 affine = 0; // the current affine sprite number
            Int32 duration = 0; // the current frame's duration
            Decimal[] arguments;
            for (Int32 line = 0; line < file.Length; line++)
            {
                loading.SetPercent(100 * ((Single)line / (Single)file.Length));
                loading.SetMessage("Processing Anim Mode" +
                    ((mode == 0 || mode == 2 || mode == 8) ?
                    "s " + (mode + 1) + " and " + (mode + 2) :
                    " " + (mode + 1)) + "...");

                if (file[line] == null)
                    continue;
                else if (file[line] == "")
                    continue;
                else //try
                {
                    for (Int32 i = 0; i < file[line].Length; i++)
                    {
                        // Comments
                        if (file[line][i] == '#') break;
                        if (file[line][i] == '/' && file[line][i + 1] == '/') break;

                        // Syntax
                        switch (file[line][i])
                        {
                            case ' ': continue;
                            
                            case '\t': continue;

                            case 'c':
                            case 'C':
                                if (compile == CompileMode.Extra)
                                    compile = CompileMode.Usual;
                                if (compile == CompileMode.Usual)
                                {
                                    this.AnimCode[mode].Add("c" + file[line].Substring(i + 1, 2));
                                    if (mode == 0 || mode == 2)
                                    {
                                        this.AnimCode[mode + 1].Add("c" + file[line].Substring(i + 1, 2));
                                    }
                                    i += 2;
                                    continue;
                                }
                                else throw new Exception("Unexpected 'c' command read.");

                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                if (compile == CompileMode.Extra)
                                    compile = CompileMode.Usual;
                                if (compile == CompileMode.Usual)
                                {
                                    Int32 length = 0;
                                    while (i + length < file[line].Length)
                                    {
                                        if (file[line][i + length] == ' ' ||
                                            file[line][i + length] == '\t') break;
                                        else length++;
                                    }
                                    try
                                    {
                                        duration = Int32.Parse(file[line].Substring(i, length));
                                    }
                                    catch
                                    {
                                        throw new Exception("Invalid duration number read:" + file[line].Substring(i, length));
                                    }
                                    compile = CompileMode.Frame;
                                    i += (length - 1);
                                    continue;
                                }
                                else throw new Exception("Unexpected frame duration number read.");

                            case 'f':
                            case 'F':
                            case 'b':
                            case 'B':
                                Boolean bg = (file[line][i] == 'b' || file[line][i] == 'B');
                                if ((bg && compile == CompileMode.Extra) || compile == CompileMode.Frame)
                                {
                                    i++;
                                    String filename = this.ReadArgument_FileName(ref file, ref line, ref i);
                                    frame = -1;
                                    new_frame = true;
                                    for (Int32 f = 0; f < filenames.Count; f++)
                                    {
                                        if (filename.Equals(filenames[f]))
                                        {
                                            frame = f;
                                            new_frame = false;
                                            break;
                                        }
                                    }
                                    if (frame == -1)
                                    {
                                        frame = filenames.Count;
                                        filenames.Add(filename);
                                        this.AddFrame(bg, new GBA.Image(folder + filename, this.Palettes[0]), filename);
                                    }   // add the new frame's OAM if it hasn't yet been used
                                    
                                    if (bg)
                                    {
                                        if (mode == 0 || mode == 2 || mode == 8)
                                            this.AnimCode[mode + 1][this.AnimCode[mode + 1].Count - 1] = duration + " f" + Util.ByteToHex((Byte)frame);
                                        else throw new Exception("'b' background layer frame commands can only be used in animation modes 1, 3 and 9.");
                                    }
                                    else
                                    {
                                        this.AnimCode[mode].Add(duration + " f" + Util.ByteToHex((Byte)frame));
                                        if (mode == 0 || mode == 2 || mode == 8) this.AnimCode[mode + 1].Add(duration + " fFF");
                                    }

                                    compile = CompileMode.Extra;
                                    continue;
                                }
                                else throw new Exception("Unexpected '" + (bg ? 'b' : 'f') + "' frame command read.");
                            
                            case 'a':
                            case 'A':
                            case 'd':
                            case 'D':
                                Boolean big = (file[line][i] == 'd' || file[line][i] == 'D');
                                if (compile == CompileMode.Extra)
                                {
                                    i++;
                                    String affinefile = this.ReadArgument_FileName(ref file, ref line, ref i);
                                    i++;
                                    arguments = this.ReadArgument_Numbers(ref file, ref line, ref i);
                                    if (arguments.Length != 2) throw new Exception(
                                        "Expected affine sprite X and Y screen coordinates.");
                                    Point coords = new Point((Int32)arguments[0], (Int32)arguments[1]);

                                    arguments = this.ReadArgument_Numbers(ref file, ref line, ref i);
                                    if (arguments.Length != 1 && arguments.Length != 4) throw new Exception(
                                        "Expected affine sprite (angle) argument, or (Ux, Vx, Uy, and Vy) vector arguments.");
                                    Double[] vectors;
                                    if (arguments.Length == 1) // convert angle into (Ux Vx Uy Vy)
                                    {
                                        Double cos = Math.Cos((Double)arguments[0]);
                                        Double sin = Math.Sin((Double)arguments[1]);
                                        vectors = new Double[4] { cos, sin, -sin, cos };
                                    }
                                    else vectors = new Double[4]
                                    {
                                        (Double)arguments[0],
                                        (Double)arguments[1],
                                        (Double)arguments[2],
                                        (Double)arguments[3]
                                    };
                                    if (new_frame == false)
                                    {   // if there's an affine on a preexisting frame, duplicate the OAM data
                                        if (this.AddDuplicateFrameWithAffines(mode, ref frame, ref filenames, coords, vectors))
                                        {   // if it returned true, that means an identical frame with identical affine exists
                                            continue;
                                        }
                                    }
                                    affine = -1;
                                    for (Int32 a = 0; a < affines.Count; a++)
                                    {
                                        if (affinefile.Equals(affines[a].Item1))
                                        {
                                            affine = a;
                                            break;
                                        }
                                    }
                                    if (affine == -1)
                                    {
                                        affine = affines.Count;
                                        Tuple<Point, Size> sheet = this.AddAffineToTilesheet(
                                            frame, new GBA.Image(folder + affinefile, this.Palettes[0]));
                                        affines.Add(Tuple.Create(affinefile, sheet.Item1, sheet.Item2));
                                    }
                                    this.AddAffine(big, frame, coords, vectors,
                                        affines[affine].Item2, affines[affine].Item3);
                                    continue;
                                }
                                else throw new Exception("Unexpected '" + (big ? 'd' : 'a') + "' affine command read.");

                            case 'e':
                            case 'E':
                                if (compile == CompileMode.Extra)
                                    compile = CompileMode.Usual;
                                if (compile == CompileMode.Usual)
                                {
                                    if ((file[line][i + 1] == 'n' || file[line][i + 1] == 'N') &&
                                        (file[line][i + 2] == 'd' || file[line][i + 2] == 'D'))
                                    {
                                        this.AnimCode[mode].Add("end");
                                        if (mode == 0 || mode == 2 || mode == 8)
                                        {
                                            this.AnimCode[mode + 1].Add("end");
                                            mode += 2;
                                        }
                                        else mode++;
                                        i += 2;
                                        continue;
                                    }
                                    else throw new Exception("Invalid terminator read:" + file[line].Substring(i, 3));
                                }
                                else throw new Exception("Unexpected terminator read.");

                            default: throw new Exception("Unexpected character: " + file[line][i]);
                        }
                    }
                }/*
                catch (Exception ex)
                {
                    throw new Exception("At line " + line + ":\r\n'" + file[line] + "'\r\n" + ex.Message);
                }*/
            }
            UInt32 emptyFrame = (UInt32)(this.Frames[0].Sprites.Count * OAM.LENGTH);
            // this uint is just the offset to the 1st terminator, so it produces an empty frame
            while (this.FrameData.Count < 256)
            {
                this.FrameData.Add(Tuple.Create((UInt32)0x00000000, emptyFrame));
            }   // fill framedata with empty frames so 'fFF' commands or such are proeperly compiled
            Int32 oam_total = 0;
            for (Int32 i = 0; i < this.Frames.Count; i++)
            {
                oam_total +=
                    this.Frames[i].Affines.Count * OAM.LENGTH +
                    this.Frames[i].Sprites.Count * OAM.LENGTH +
                    OAM.LENGTH;
            }
            if (oam_total > BattleAnimation.MAXIMUM_OAM_LENGTH)
                UI.ShowWarning("The final OAM block is too large: " + oam_total +
                    "\nIt should weigh " + BattleAnimation.MAXIMUM_OAM_LENGTH + " bytes or less.");
        }
        /// <summary>
        /// Advances the 'line' index and 'i' char index through all the whitespace and line-returns
        /// </summary>
        void ReadWhitespace(ref String[] file, ref Int32 line, ref Int32 i)
        {
            while (file[line][i] == ' ' || file[line][i] == '\t')
            {
                i++;
                if (i >= file[line].Length ||
                    file[line][i] == '#' || 
                    file[line][i] == '/')
                {
                    line++;
                    i = 0;
                }
                if (line >= file.Length)
                    throw new Exception("Reached unexpected end of file.");
            }
        }
        /// <summary>
        /// Returns the contents of brackets in the string line given, incrementing the index as necessary
        /// </summary>
        String ReadArgument_FileName(ref String[] file, ref Int32 line, ref Int32 i)
        {
            this.ReadWhitespace(ref file, ref line, ref i);
            if (file[line][i] == '[') i++;
            else throw new Exception("Expected bracket arguments on this line.");
            Int32 length = 0;
            while (file[line][i + length] != ']')
            {
                length++;
                if (i >= file[line].Length)
                    throw new Exception("No closing bracket found. Brackets must be on one line.");
            }
            String result = file[line].Substring(i, length).Trim(' ');
            i += length;
            if (result.EndsWith(".png") || result.EndsWith(".PNG") ||
                result.EndsWith(".bmp") || result.EndsWith(".BMP") ||
                result.EndsWith(".gif") || result.EndsWith(".GIF"))
            {
                return result;
            }
            else throw new Exception("Invalid file extension (must be PNG, BMP or GIF): " + result);
        }
        /// <summary>
        /// Returns the contents of parentheses in the string line given, incrementing index as necessary
        /// </summary>
        Decimal[] ReadArgument_Numbers(ref String[] file, ref Int32 line, ref Int32 i)
        {
            this.ReadWhitespace(ref file, ref line, ref i);
            if (file[line][i] == '(') i++;
            else throw new Exception("Expected parenthese arguments, found: " + file[line][i]);
            this.ReadWhitespace(ref file, ref line, ref i);
            Int32 length;
            List<Decimal> result = new List<Decimal>();
            while (file[line][i] != ')')
            {
                length = 0;
                while (file[line][i + length] != ' '
                    && file[line][i + length] != ','
                    && file[line][i + length] != ')'
                    && file[line][i + length] != '\t') length++;
                try
                {
                    result.Add(Decimal.Parse(file[line].Substring(i, length),
                        System.Globalization.NumberStyles.AllowDecimalPoint |
                        System.Globalization.NumberStyles.AllowLeadingSign,
                        System.Globalization.CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception("Could not parse number: " + file[line].Substring(i, length) + " | " + result.Count);
                }
                i += length;
                this.ReadWhitespace(ref file, ref line, ref i);
                if (file[line][i] == ',') i++;
                else if (file[line][i] == ')') { i++; break; }
                else throw new Exception("Unexpected number argument separator: " + file[line][i]);
                this.ReadWhitespace(ref file, ref line, ref i);
            }
            return result.ToArray();
        }



        /// <summary>
        /// Adds a GBA.TileSheet to the 'Graphics' list, and returns its index
        /// </summary>
        Int32 AddTileSheet()
        {
            Int32 index = this.Graphics.Count;
            this.Graphics.Add(new TileSheet(32, 8));
            return index;
        }
        /// <summary>
        /// Adds the image at 'filepath' as a new frame, creating the OAM data for said frame.
        /// Returns the index of the tilesheet for this frame
        /// </summary>
        Tuple<String, OAM_Array> AddFrame(Boolean background, GBA.Image frame, String filename)
        {
            OAM_Maker oam;
            if (frame.Width == 240 && frame.Height == 160)
            {
                try
                {
                    oam = new OAM_Maker(ref this.Graphics, frame,
                        BattleAnimation.SCREEN_OFFSET_X_R,
                        BattleAnimation.SCREEN_OFFSET_Y);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while creating OAM for the image:\n" + filename + "\n\n" + ex.Message);
                }
                this.FrameData.Add(Tuple.Create((UInt32)oam.TilesetIndex, GetCurrentOAMOffset(this.Frames)));
                this.Frames.Add(oam.SpriteData);
            }
            else throw new Exception("Frame image must be 240x160 pixels. Invalid image given:\n" + filename);
            return Tuple.Create(filename, oam.SpriteData);
        }
        /// <summary>
        /// Adds an affine sprite to the given frame, positioned at 'screen' coordinates, transformed by 'vectors'
        /// </summary>
        void AddAffine(Boolean bigAffine, Int32 frame, Point screen, Double[] vectors, Point sheet, Size size)
        {
            var shapesize = OAM.GetShapeSize(size);

            this.Frames[frame].Sprites.Add(new OAM(
                shapesize.Item1, shapesize.Item2,
                (Int16)(screen.X - BattleAnimation.SCREEN_OFFSET_X_R),
                (Int16)(screen.Y - BattleAnimation.SCREEN_OFFSET_Y),
                0x00, 0x00,
                OAM.GFXMode.Normal,
                (bigAffine) ? OAM.OBJMode.BigAffine : OAM.OBJMode.Affine,
                false,
                false,
                (Byte)sheet.X, (Byte)sheet.Y,
                (Byte)this.Frames[frame].Affines.Count));

            this.Frames[frame].Affines.Add(new OAM.Affine(
                vectors[0],
                vectors[1],
                vectors[2],
                vectors[3]));
        }
        /// <summary>
        /// Adds an affine sprite to the given tilesheet, and returns its position on said tilesheet
        /// </summary>
        Tuple<Point, Size> AddAffineToTilesheet(Int32 frame, GBA.Image sprite)
        {
            Int32 index = (Int32)this.FrameData[frame].Item1;
            Size size = new Size(sprite.Width / Tile.SIZE, sprite.Height / Tile.SIZE);
            Point sheet = this.Graphics[index].CheckIfFits(size);
            if (sheet == new Point(-1, -1))
            {
                sheet = new Point(32 - size.Width, 8 - size.Height);
                //throw new Exception("Affine sprite doesn't fit on the current tilesheet.");
            }
            for (Int32 y = 0; y < size.Height; y++)
            for (Int32 x = 0; x < size.Width; x++)
            {
                    this.Graphics[index][sheet.X + x, sheet.Y + y] = sprite.GetTile(x * Tile.SIZE, y * Tile.SIZE);
            }
            return Tuple.Create(sheet, size);
        }
        /// <summary>
        /// Duplicates the OAM data for frames that are the same but use different affine sprites.
        /// Checks if the given frame has identical affine sprites to any other duplicate frame, so as to save up on OAM data
        /// </summary>
        Boolean AddDuplicateFrameWithAffines(Int32 mode, ref Int32 frame, ref List<String> filenames, Point coords, Double[] vectors)
        {
            String frame_old = "f" + Util.ByteToHex((Byte)frame);
            String frame_new;
            Boolean duplicate_found = false;
            for (Int32 f = frame + 1; f < this.Frames.Count; f++)
            {
                Boolean equal = true;
                for (Int32 s = 0; s < this.Frames[frame].Sprites.Count; s++)
                {
                    if (s >= this.Frames[f].Sprites.Count)
                        break;
                    if (!this.Frames[f].Sprites[s].Equals(this.Frames[frame].Sprites[s]))
                    {
                        equal = false;
                        break;
                    }
                    if (this.Frames[f].Sprites[s].IsAffineSprite())
                    {
                        if (this.Frames[f].Sprites[s].ScreenX + BattleAnimation.SCREEN_OFFSET_X_R == coords.X &&
                            this.Frames[f].Sprites[s].ScreenY + BattleAnimation.SCREEN_OFFSET_Y == coords.Y &&
                            this.Frames[f].Affines[this.Frames[f].Sprites[s].AffineIndex].Ux == vectors[0] &&
                            this.Frames[f].Affines[this.Frames[f].Sprites[s].AffineIndex].Uy == vectors[1] &&
                            this.Frames[f].Affines[this.Frames[f].Sprites[s].AffineIndex].Vx == vectors[2] &&
                            this.Frames[f].Affines[this.Frames[f].Sprites[s].AffineIndex].Vy == vectors[3])
                        {
                            continue;
                        }
                        equal = false;
                        break;
                    }
                }
                if (equal)
                {
                    frame = f;
                    duplicate_found = true;
                    break;
                }
            }
            if (duplicate_found)
                frame_new = "f" + Util.ByteToHex((Byte)frame);
            else
            {
                frame_new = "f" + Util.ByteToHex((Byte)this.Frames.Count);
                filenames.Add(filenames[frame]);
                this.FrameData.Add(Tuple.Create(this.FrameData[frame].Item1, GetCurrentOAMOffset(this.Frames)));
                this.Frames.Add(new OAM_Array(new List<OAM>(this.Frames[frame].Sprites)));
                frame = this.Frames.Count - 1;
            }
            Int32 line_number = this.AnimCode[mode].Count - 1;
            Int32 result = this.AnimCode[mode][line_number].IndexOf(frame_old);
            if (result != -1)
            {
                //UI.ShowDebug("old:" + frame_old + ", new:" + frame_new + "\r\n" + AnimCode[mode][line_number]);
                this.AnimCode[mode][line_number] =
                this.AnimCode[mode][line_number].Replace(frame_old, frame_new);
            }
            else
            {
                line_number = this.AnimCode[mode + 1].Count - 1;
                result = this.AnimCode[mode + 1][line_number].IndexOf(frame_old);
                if (result != -1)
                {
                    this.AnimCode[mode + 1][line_number] =
                    this.AnimCode[mode + 1][line_number].Replace(frame_old, frame_new);
                }
                else throw new Exception("Could not link affine sprite to its frame.");
            }
            return false;
        }
        
        /// <summary>
        /// Returns the current offset inside the big OAM block being constructed in the 'Frames' array
        /// </summary>
        static UInt32 GetCurrentOAMOffset(List<OAM_Array> frames)
        {
            UInt32 result = 0;
            foreach (OAM_Array frame in frames)
            {
                result += (UInt32)(frame.Affines.Count * OAM.LENGTH);
                result += (UInt32)(frame.Sprites.Count * OAM.LENGTH);
                result += OAM.LENGTH; // terminator
            }
            return result;
        }

        /// <summary>
        /// Returns the palette read from any valid file in the folder of the battle anim
        /// </summary>
        static Palette GetPaletteFromFile(String folder)
        {
            Int32 length = Palette.MAX * 4;
            Palette result = null;
                 if (File.Exists(folder + "palette.pal")) result = new Palette(folder + "palette.pal", length);
            else if (File.Exists(folder + "palette.PAL")) result = new Palette(folder + "palette.PAL", length);
            else if (File.Exists(folder + "palette.png")) result = new Palette(folder + "palette.png", length);
            else if (File.Exists(folder + "palette.PNG")) result = new Palette(folder + "palette.PNG", length);
            else if (File.Exists(folder + "palette.bmp")) result = new Palette(folder + "palette.bmp", length);
            else if (File.Exists(folder + "palette.BMP")) result = new Palette(folder + "palette.BMP", length);
            else if (File.Exists(folder + "palette.gif")) result = new Palette(folder + "palette.gif", length);
            else if (File.Exists(folder + "palette.GIF")) result = new Palette(folder + "palette.GIF", length);
            else throw new Exception("Palette file could not be found.\r\n" +
                "It must be named 'palette' and it can have any of the following filetypes:\r\n" +
                "PAL (128 bytes) or PNG, BMP, GIF (must be a 16x4 image of the palette).");
            return result;
        }
    }
}
