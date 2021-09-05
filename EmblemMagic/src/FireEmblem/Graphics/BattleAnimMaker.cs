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
        /// <summary>
        /// The animation code translated from the text file's code
        /// </summary>
        public List<string>[] AnimCode;
        /// <summary>
        /// The OAM data for the sprites of each frame
        /// </summary>
        public List<OAM_Array> Frames;
        /// <summary>
        /// The tileset index and OAM offset for each frame (tileset index will be made into a pointer on insert)
        /// </summary>
        public List<Tuple<uint, uint>> FrameData;
        /// <summary>
        /// The (usually) 4 palettes for this battle animation
        /// </summary>
        public Palette[] Palettes;
        /// <summary>
        /// The tilesheets used to construct this battle animation with OAM
        /// </summary>
        public List<TileSheet> Graphics;

        // state enum for the anim code parser
        enum CompileMode
        {
            Usual,
            Frame,
            Extra
        }


        /// <summary>
        /// Processes a battle animation from a txt file, reading all referenced image files
        /// </summary>
        public BattleAnimMaker(string folder, string[] file)
        {
            FormLoading loading = new FormLoading();
            loading.SetMessage("Processing animation...");
            loading.Show();
            
            AnimCode = new List<string>[12];
            Graphics = new List<TileSheet>();
            for (int i = 0; i < AnimCode.Length; i++)
            {
                AnimCode[i] = new List<string>();
            }
            AddTileSheet();
            
            Palettes = Palette.Split(GetPaletteFromFile(folder), 4);



            Frames = new List<OAM_Array>();
            FrameData = new List<Tuple<uint, uint>>();
            List<string> filenames = new List<string>();
            List<Tuple<string, Point, Size>> affines = new List<Tuple<string, Point, Size>>();
            CompileMode compile = CompileMode.Usual;
            bool new_frame = true;
            int mode = 0; // the current animation mode being processed
            int frame = 0; // the current frame number
            int affine = 0; // the current affine sprite number
            int duration = 0; // the current frame's duration
            decimal[] arguments;
            for (int line = 0; line < file.Length; line++)
            {
                loading.SetPercent(100 * ((float)line / (float)file.Length));
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
                    for (int i = 0; i < file[line].Length; i++)
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
                                    AnimCode[mode].Add("c" + file[line].Substring(i + 1, 2));
                                    if (mode == 0 || mode == 2)
                                    {
                                        AnimCode[mode + 1].Add("c" + file[line].Substring(i + 1, 2));
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
                                    int length = 0;
                                    while (i + length < file[line].Length)
                                    {
                                        if (file[line][i + length] == ' ' ||
                                            file[line][i + length] == '\t') break;
                                        else length++;
                                    }
                                    try
                                    {
                                        duration = int.Parse(file[line].Substring(i, length));
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
                                bool bg = (file[line][i] == 'b' || file[line][i] == 'B');
                                if ((bg && compile == CompileMode.Extra) || compile == CompileMode.Frame)
                                {
                                    i++;
                                    string filename = ReadArgument_FileName(ref file, ref line, ref i);
                                    frame = -1;
                                    new_frame = true;
                                    for (int f = 0; f < filenames.Count; f++)
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
                                        AddFrame(bg, new GBA.Image(folder + filename, Palettes[0]), filename);
                                    }   // add the new frame's OAM if it hasn't yet been used
                                    
                                    if (bg)
                                    {
                                        if (mode == 0 || mode == 2 || mode == 8)
                                            AnimCode[mode + 1][AnimCode[mode + 1].Count - 1] = duration + " f" + Util.ByteToHex((byte)frame);
                                        else throw new Exception("'b' background layer frame commands can only be used in animation modes 1, 3 and 9.");
                                    }
                                    else
                                    {
                                        AnimCode[mode].Add(duration + " f" + Util.ByteToHex((byte)frame));
                                        if (mode == 0 || mode == 2 || mode == 8) AnimCode[mode + 1].Add(duration + " fFF");
                                    }

                                    compile = CompileMode.Extra;
                                    continue;
                                }
                                else throw new Exception("Unexpected '" + (bg ? 'b' : 'f') + "' frame command read.");
                            
                            case 'a':
                            case 'A':
                            case 'd':
                            case 'D':
                                bool big = (file[line][i] == 'd' || file[line][i] == 'D');
                                if (compile == CompileMode.Extra)
                                {
                                    i++;
                                    string affinefile = ReadArgument_FileName(ref file, ref line, ref i);
                                    i++;
                                    arguments = ReadArgument_Numbers(ref file, ref line, ref i);
                                    if (arguments.Length != 2) throw new Exception(
                                        "Expected affine sprite X and Y screen coordinates.");
                                    Point coords = new Point((int)arguments[0], (int)arguments[1]);

                                    arguments = ReadArgument_Numbers(ref file, ref line, ref i);
                                    if (arguments.Length != 1 && arguments.Length != 4) throw new Exception(
                                        "Expected affine sprite (angle) argument, or (Ux, Vx, Uy, and Vy) vector arguments.");
                                    float[] vectors;
                                    if (arguments.Length == 1) // convert angle into (Ux Vx Uy Vy)
                                    {
                                        float cos = (float)Math.Cos((double)arguments[0]);
                                        float sin = (float)Math.Sin((double)arguments[1]);
                                        vectors = new float[4] { cos, sin, -sin, cos };
                                    }
                                    else vectors = new float[4]
                                    {
                                        (float)arguments[0],
                                        (float)arguments[1],
                                        (float)arguments[2],
                                        (float)arguments[3]
                                    };
                                    if (new_frame == false)
                                    {   // if there's an affine on a preexisting frame, duplicate the OAM data
                                        if (AddDuplicateFrameWithAffines(mode, ref frame, ref filenames, coords, vectors))
                                        {   // if it returned true, that means an identical frame with identical affine exists
                                            continue;
                                        }
                                    }
                                    affine = -1;
                                    for (int a = 0; a < affines.Count; a++)
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
                                        Tuple<Point, Size> sheet = AddAffineToTilesheet(
                                            frame, new GBA.Image(folder + affinefile, Palettes[0]));
                                        affines.Add(Tuple.Create(affinefile, sheet.Item1, sheet.Item2));
                                    }
                                    AddAffine(big, frame, coords, vectors,
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
                                        AnimCode[mode].Add("end");
                                        if (mode == 0 || mode == 2 || mode == 8)
                                        {
                                            AnimCode[mode + 1].Add("end");
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
            uint emptyFrame = (uint)(Frames[0].Sprites.Count * OAM.LENGTH);
            // this uint is just the offset to the 1st terminator, so it produces an empty frame
            while (FrameData.Count < 256)
            {
                FrameData.Add(Tuple.Create((uint)0x00000000, emptyFrame));
            }   // fill framedata with empty frames so 'fFF' commands or such are proeperly compiled
            int oam_total = 0;
            for (int i = 0; i < Frames.Count; i++)
            {
                oam_total +=
                    Frames[i].Affines.Count * OAM.LENGTH +
                    Frames[i].Sprites.Count * OAM.LENGTH +
                    OAM.LENGTH;
            }
            if (oam_total > BattleAnimation.MAXIMUM_OAM_LENGTH)
                UI.ShowWarning("The final OAM block is too large: " + oam_total +
                    "\nIt should weigh " + BattleAnimation.MAXIMUM_OAM_LENGTH + " bytes or less.");
        }
        /// <summary>
        /// Advances the 'line' index and 'i' char index through all the whitespace and line-returns
        /// </summary>
        void ReadWhitespace(ref string[] file, ref int line, ref int i)
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
        string ReadArgument_FileName(ref string[] file, ref int line, ref int i)
        {
            ReadWhitespace(ref file, ref line, ref i);
            if (file[line][i] == '[') i++;
            else throw new Exception("Expected bracket arguments on this line.");
            int length = 0;
            while (file[line][i + length] != ']')
            {
                length++;
                if (i >= file[line].Length)
                    throw new Exception("No closing bracket found. Brackets must be on one line.");
            }
            string result = file[line].Substring(i, length).Trim(' ');
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
        decimal[] ReadArgument_Numbers(ref string[] file, ref int line, ref int i)
        {
            ReadWhitespace(ref file, ref line, ref i);
            if (file[line][i] == '(') i++;
            else throw new Exception("Expected parenthese arguments, found: " + file[line][i]);
            ReadWhitespace(ref file, ref line, ref i);
            int length;
            List<decimal> result = new List<decimal>();
            while (file[line][i] != ')')
            {
                length = 0;
                while (file[line][i + length] != ' '
                    && file[line][i + length] != ','
                    && file[line][i + length] != ')'
                    && file[line][i + length] != '\t') length++;
                try
                {
                    result.Add(decimal.Parse(file[line].Substring(i, length),
                        System.Globalization.NumberStyles.AllowDecimalPoint |
                        System.Globalization.NumberStyles.AllowLeadingSign,
                        System.Globalization.CultureInfo.InvariantCulture));
                }
                catch
                {
                    throw new Exception("Could not parse number: " + file[line].Substring(i, length) + " | " + result.Count);
                }
                i += length;
                ReadWhitespace(ref file, ref line, ref i);
                if (file[line][i] == ',') i++;
                else if (file[line][i] == ')') { i++; break; }
                else throw new Exception("Unexpected number argument separator: " + file[line][i]);
                ReadWhitespace(ref file, ref line, ref i);
            }
            return result.ToArray();
        }



        /// <summary>
        /// Adds a GBA.TileSheet to the 'Graphics' list, and returns its index
        /// </summary>
        int AddTileSheet()
        {
            int index = Graphics.Count;
            Graphics.Add(new TileSheet(32, 8));
            return index;
        }
        /// <summary>
        /// Adds the image at 'filepath' as a new frame, creating the OAM data for said frame.
        /// Returns the index of the tilesheet for this frame
        /// </summary>
        Tuple<string, OAM_Array> AddFrame(bool background, GBA.Image frame, string filename)
        {
            OAM_Maker oam;
            if (frame.Width == 240 && frame.Height == 160)
            {
                try
                {
                    oam = new OAM_Maker(ref Graphics, frame,
                        BattleAnimation.SCREEN_OFFSET_X_R,
                        BattleAnimation.SCREEN_OFFSET_Y);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while creating OAM for the image:\n" + filename + "\n\n" + ex.Message);
                }
                FrameData.Add(Tuple.Create((uint)oam.TilesetIndex, GetCurrentOAMOffset(Frames)));
                Frames.Add(oam.SpriteData);
            }
            else throw new Exception("Frame image must be 240x160 pixels. Invalid image given:\n" + filename);
            return Tuple.Create(filename, oam.SpriteData);
        }
        /// <summary>
        /// Adds an affine sprite to the given frame, positioned at 'screen' coordinates, transformed by 'vectors'
        /// </summary>
        void AddAffine(bool bigAffine, int frame, Point screen, float[] vectors, Point sheet, Size size)
        {
            var shapesize = OAM.GetShapeSize(size);

            Frames[frame].Sprites.Add(new OAM(
                shapesize.Item1, shapesize.Item2,
                (Int16)(screen.X - BattleAnimation.SCREEN_OFFSET_X_R),
                (Int16)(screen.Y - BattleAnimation.SCREEN_OFFSET_Y),
                0x00, 0x00,
                OAM_GFXMode.Normal,
                (bigAffine) ? OAM_OBJMode.BigAffine : OAM_OBJMode.Affine,
                false,
                false,
                (byte)sheet.X, (byte)sheet.Y,
                (byte)Frames[frame].Affines.Count));

            Frames[frame].Affines.Add(new OAM_Affine(
                vectors[0],
                vectors[1],
                vectors[2],
                vectors[3]));
        }
        /// <summary>
        /// Adds an affine sprite to the given tilesheet, and returns its position on said tilesheet
        /// </summary>
        Tuple<Point, Size> AddAffineToTilesheet(int frame, GBA.Image sprite)
        {
            int index = (int)FrameData[frame].Item1;
            Size size = new Size(sprite.Width / Tile.SIZE, sprite.Height / Tile.SIZE);
            Point sheet = Graphics[index].CheckIfFits(size);
            if (sheet == new Point(-1, -1))
            {
                sheet = new Point(32 - size.Width, 8 - size.Height);
                //throw new Exception("Affine sprite doesn't fit on the current tilesheet.");
            }
            for (int y = 0; y < size.Height; y++)
            for (int x = 0; x < size.Width; x++)
            {
                Graphics[index][sheet.X + x, sheet.Y + y] = sprite.GetTile(x * Tile.SIZE, y * Tile.SIZE);
            }
            return Tuple.Create(sheet, size);
        }
        /// <summary>
        /// Duplicates the OAM data for frames that are the same but use different affine sprites.
        /// Checks if the given frame has identical affine sprites to any other duplicate frame, so as to save up on OAM data
        /// </summary>
        bool AddDuplicateFrameWithAffines(int mode, ref int frame, ref List<string> filenames, Point coords, float[] vectors)
        {
            string frame_old = "f" + Util.ByteToHex((byte)frame);
            string frame_new;
            bool duplicate_found = false;
            for (int f = frame + 1; f < Frames.Count; f++)
            {
                bool equal = true;
                for (int s = 0; s < Frames[frame].Sprites.Count; s++)
                {
                    if (s >= Frames[f].Sprites.Count)
                        break;
                    if (!Frames[f].Sprites[s].Equals(Frames[frame].Sprites[s]))
                    {
                        equal = false;
                        break;
                    }
                    if (Frames[f].Sprites[s].IsAffineSprite())
                    {
                        if (Frames[f].Sprites[s].ScreenX + BattleAnimation.SCREEN_OFFSET_X_R == coords.X &&
                            Frames[f].Sprites[s].ScreenY + BattleAnimation.SCREEN_OFFSET_Y == coords.Y &&
                            Frames[f].Affines[Frames[f].Sprites[s].AffineIndex].Ux == vectors[0] &&
                            Frames[f].Affines[Frames[f].Sprites[s].AffineIndex].Vx == vectors[1] &&
                            Frames[f].Affines[Frames[f].Sprites[s].AffineIndex].Uy == vectors[2] &&
                            Frames[f].Affines[Frames[f].Sprites[s].AffineIndex].Vy == vectors[3])
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
                frame_new = "f" + Util.ByteToHex((byte)frame);
            else
            {
                frame_new = "f" + Util.ByteToHex((byte)Frames.Count);
                filenames.Add(filenames[frame]);
                FrameData.Add(Tuple.Create(FrameData[frame].Item1, GetCurrentOAMOffset(Frames)));
                Frames.Add(new OAM_Array(new List<OAM>(Frames[frame].Sprites)));
                frame = Frames.Count - 1;
            }
            int line_number = AnimCode[mode].Count - 1;
            int result = AnimCode[mode][line_number].IndexOf(frame_old);
            if (result != -1)
            {
                //UI.ShowDebug("old:" + frame_old + ", new:" + frame_new + "\r\n" + AnimCode[mode][line_number]);
                AnimCode[mode][line_number] =
                AnimCode[mode][line_number].Replace(frame_old, frame_new);
            }
            else
            {
                line_number = AnimCode[mode + 1].Count - 1;
                result = AnimCode[mode + 1][line_number].IndexOf(frame_old);
                if (result != -1)
                {
                    AnimCode[mode + 1][line_number] =
                    AnimCode[mode + 1][line_number].Replace(frame_old, frame_new);
                }
                else throw new Exception("Could not link affine sprite to its frame.");
            }
            return false;
        }
        
        /// <summary>
        /// Returns the current offset inside the big OAM block being constructed in the 'Frames' array
        /// </summary>
        static uint GetCurrentOAMOffset(List<OAM_Array> frames)
        {
            uint result = 0;
            foreach (OAM_Array frame in frames)
            {
                result += (uint)(frame.Affines.Count * OAM.LENGTH);
                result += (uint)(frame.Sprites.Count * OAM.LENGTH);
                result += OAM.LENGTH; // terminator
            }
            return result;
        }

        /// <summary>
        /// Returns the palette read from any valid file in the folder of the battle anim
        /// </summary>
        static Palette GetPaletteFromFile(string folder)
        {
            int length = Palette.MAX * 4;
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
