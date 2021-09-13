using System;
using System.Collections.Generic;
using System.IO;
using GBA;
using Magic;

namespace EmblemMagic.FireEmblem
{
    public class BattleAnimFrame
    {
        /// <summary>
        /// The array of Left-to-Right OAM data for this frame of a battle anim
        /// </summary>
        public OAM_Array OAM_Data_L;
        /// <summary>
        /// The array of Right-to-Left OAM data for this frame of a battle anim
        /// </summary>
        public OAM_Array OAM_Data_R;
        /// <summary>
        /// The index of the tileset to use for this frame
        /// </summary>
        public UInt32 TilesetIndex;
        /// <summary>
        /// The offset of this OAM data in the larger array of combined OAM framedata
        /// </summary>
        public UInt32 OAM_Offset;
        /// <summary>
        /// The duration (in 60ths/second) that this frame is to show up onscreen
        /// </summary>
        public UInt32 Duration;

        public BattleAnimFrame(
            OAM_Array oamData_L,
            OAM_Array oamData_R,
            UInt32 tileset,
            UInt32 offset,
            UInt32 duration)
        {
            this.OAM_Data_L = oamData_L;
            this.OAM_Data_R = oamData_R;
            this.TilesetIndex = tileset;
            this.OAM_Offset = offset;
            this.Duration = duration;
        }
    }



    /// <summary>
    /// This class allows for reading and editing of battle animations
    /// </summary>
    public class BattleAnimation : GBA.SpriteSheet
    {
        public const Byte SCREEN_OFFSET_X_L = 0x5C;
        public const Byte SCREEN_OFFSET_X_R = 0x94;
        public const Byte SCREEN_OFFSET_Y = 0x58;
        
        public const Int32 MAXIMUM_ANIMDATA_LENGTH = 0x2A00;
        public const Int32 MAXIMUM_OAM_LENGTH = 0x5800;
        /// <summary>
        /// The amount of different modes for any battle animation
        /// </summary>
        public const Int32 MODES = 12;
        public static String[] Modes = new String[12]
        {
            "Normal Attack (layer1)",
            "Normal Attack (layer2)",
            "Critical Attack (layer1)",
            "Critical Attack (layer2)",
            "Normal Attack (ranged)",
            "Critical Attack (ranged)",
            "Avoid",
            "Avoid (ranged)",
            "Idle (layer1)",
            "Idle (layer2)",
            "Idle (ranged)",
            "Missed Attack"
        };
        public static String[] Modes_Layered = new String[9]
        {
            "Normal Attack",
            "Critical Attack",
            "Normal Attack (ranged)",
            "Critical Attack (ranged)",
            "Avoid",
            "Avoid (ranged)",
            "Idle",
            "Idle (ranged)",
            "Missed Attack"
        };



        /// <summary>
        /// The animation code for this class animation - in a jagged array for the 12 sections of anim code
        /// </summary>
        public String[][] AnimCode;
        /// <summary>
        /// The different tilesheets used for this battle animation
        /// </summary>
        public Tileset[] Tilesets;
        /// <summary>
        /// Stores the OAM data detailing how sprites are to be loaded from the tileset and shown onscreen
        /// </summary>
        public BattleAnimFrame[] Frames;



        public BattleAnimation(
            UInt32[] sections,
            Byte[] animdata,
            Byte[] OAMdataL,
            Byte[] OAMdataR) : base(240, 160)
        {
            Byte lastFrame = 0;
            for (Int32 i = 0; i < animdata.Length; i += 4)
            {
                if (animdata[i + 3] == 0x86)
                {
                    if (animdata[i + 2] > lastFrame) lastFrame = animdata[i + 2];
                }
            }
            // this tuple array holds 3 values - Tileset pointer, OAM data offset, and frame duration
            Tuple<Pointer, UInt32, Byte>[] frames = new Tuple<Pointer, UInt32, Byte>[lastFrame + 1];
            List<Tuple<Pointer, UInt32, Byte>> anomalies = new List<Tuple<Pointer, UInt32, Byte>>();
            for (UInt32 i = 0; i < animdata.Length; i += 4)
            {
                if (animdata[i + 3] == 0x86)
                {
                    Byte duration = animdata[i];
                    Byte frame = animdata[i + 2];
                    i += 4;
                    Pointer pointer = new Pointer(animdata.GetUInt32(i, false), true, true);
                    i += 4;
                    UInt32 offset = animdata.GetUInt32(i, true);

                    if (frames[frame] != null && (pointer != frames[frame].Item1 || offset != frames[frame].Item2))
                    //    throw new Exception("Inconsistent frames found in the animation.");
                    // this is thrown with FEditor animations, should make a prompt to re-number the frames in FEditor anims
                    {
                        Boolean duplicate = false;
                        foreach (var f in anomalies)
                        {
                            if (f.Item1 == pointer && f.Item2 == offset)
                                duplicate = true;
                        }
                        if (!duplicate)
                            anomalies.Add(Tuple.Create(pointer, offset, duration));
                    }
                    else frames[frame] = Tuple.Create(pointer, offset, duration);
                }
            }

            this.Frames = new BattleAnimFrame[frames.Length + anomalies.Count];
            List<Pointer> pointers = new List<Pointer>();
            for (Int32 i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null)
                {
                    if (frames[i].Item1 != new Pointer() && !pointers.Contains(frames[i].Item1))
                    {
                        pointers.Add(frames[i].Item1);
                    }
                    this.Frames[i] = new BattleAnimFrame(
                        new OAM_Array(OAMdataL, frames[i].Item2),
                        new OAM_Array(OAMdataR, frames[i].Item2),
                        frames[i].Item1.Address,
                        frames[i].Item2,
                        frames[i].Item3);
                }
            }
            for (Int32 i = 0; i < anomalies.Count; i++)
            {
                if (anomalies[i].Item1 != new Pointer() && !pointers.Contains(anomalies[i].Item1))
                {
                    pointers.Add(anomalies[i].Item1);
                }
                this.Frames[frames.Length + i] = new BattleAnimFrame(
                    new OAM_Array(OAMdataL, anomalies[i].Item2),
                    new OAM_Array(OAMdataR, anomalies[i].Item2),
                    anomalies[i].Item1.Address,
                    anomalies[i].Item2,
                    anomalies[i].Item3);
            }

            this.AnimCode = new String[sections.Length][];
            for (Int32 i = 0; i < sections.Length; i++)
            {
                this.AnimCode[i] = DecompileAnimCode(animdata, sections[i], this.Frames);
            }

            pointers.Sort(delegate (Pointer first, Pointer second)
            {
                return (Int32)(first.Address - second.Address);
            });

            this.Tilesets = new Tileset[pointers.Count];
            for (Int32 i = 0; i < this.Tilesets.Length; i++)
            {
                for (Int32 j = 0; j < this.Frames.Length; j++)
                {
                    if (this.Frames[j] == null) continue;
                    if (this.Frames[j].TilesetIndex == pointers[i])
                        this.Frames[j].TilesetIndex = (UInt32)i;
                }
                this.Tilesets[i] = new Tileset(Core.ReadData(pointers[i], 0));
            }
        }



        /// <summary>
        /// Makes the given frame into the current display state of this BattleAnimation
        /// </summary>
        public void ShowFrame(Palette palette, Byte frame, Boolean leftToRight)
        {
            this.Clear();

            if (frame >= this.Frames.Length || this.Frames[frame] == null)
                return;

            this.AddSprite(palette,
                this.Tilesets[this.Frames[frame].TilesetIndex],
                leftToRight ? this.Frames[frame].OAM_Data_L : this.Frames[frame].OAM_Data_R,
                leftToRight ? SCREEN_OFFSET_X_L : SCREEN_OFFSET_X_R,
                SCREEN_OFFSET_Y);
        }

        /// <summary>
        /// Returns the animation code of all 12 modes combined into one string array
        /// </summary>
        public String[] GetMergedAnimCode()
        {
            Int32 length = 0;
            for (Int32 i = 0; i < this.AnimCode.Length; i++)
            {
                length += this.AnimCode[i].Length;
            }
            String[] result = new String[length];
            length = 0;
            for (Int32 i = 0; i < this.AnimCode.Length; i++)
            {
                for (Int32 j = 0; j < this.AnimCode[i].Length; j++)
                {
                    result[length] = this.AnimCode[i][j];
                    length++;
                }
            }
            return result;
        }
        


        /// <summary>
        /// Returns the section offsets for the 12 modes of animation data
        /// </summary>
        public static Byte[] GetSections(Byte[] animdata)
        {
            Int32[] sections = new Int32[12];
            Int32 current = 0;
            for (Int32 i = 0; i < animdata.Length; i += 4)
            {
                if (animdata[i + 3] == 0x80)
                {
                    i += 4;
                    current++;
                    try { sections[current] = i; }
                    catch { break; }
                }
            }
            Byte[] result = new Byte[12 * 4];
            for (Int32 i = 0; i < 12; i++)
            {
                Array.Copy(Util.Int32ToBytes(sections[i], true), 0, result, i * 4, 4);
            }
            return result;
        }
        /// <summary>
        /// Returns all the differents pointers to tilesets found in the given byte array, sorted by address
        /// </summary>
        public static List<Pointer> GetTilesetPointers(Byte[] animdata)
        {
            List<Pointer> result = new List<Pointer>();
            Byte[] buffer = new Byte[4];
            Pointer pointer;
            for (Int32 i = 0; i < animdata.Length; i += 4)
            {
                if (animdata[i + 3] == 0x86)
                {
                    i += 4;
                    Array.Copy(animdata, i, buffer, 0, 4);
                    pointer = new Pointer(buffer);

                    if (!result.Contains(pointer))
                        result.Add(pointer);
                }
            }
            result.Sort(delegate (Pointer first, Pointer second)
            {
                return (first - second);
            });
            return result;
        }
        /// <summary>
        /// Flips the given OAM and returns both (left-side-facing-right and right-side-facing-left)
        /// </summary>
        public static Tuple<Byte[], Byte[]> GetFlippedOAM(Byte[] OAMdata)
        {
            Byte[] flipped = new Byte[OAMdata.Length];
            OAM block;
            Byte[] buffer = new Byte[12];
            for (Int32 i = 0; i < OAMdata.Length; i += 12)
            {
                Array.Copy(OAMdata, i, buffer, 0, 12);

                if (OAMdata[i] == 0x00)
                {
                    block = new OAM(buffer);

                    block.FlipH = !block.FlipH;

                    block.ScreenX = (Int16)((block.ScreenX + block.GetDimensions().Width * 8) * -1);

                    buffer = block.ToBytes();
                }
                Array.Copy(buffer, 0, flipped, i, 12);
            }

            return Tuple.Create(flipped, OAMdata);
        }


        
        /// <summary>
        /// Compiles one mode of animation code (that is, a block with a terminator at the end)
        /// </summary>
        public static Byte[] CompileAnimationCode(String[] animcode, Tuple<UInt32, UInt32>[] frames)
        {
            try
            {
                List<Byte> result = new List<Byte>();
                Byte duration = 1;
                Byte frame;
                Boolean frameCompile = false;
                for (Int32 line = 0; line < animcode.Length; line++)
                {
                    try
                    {
                        if (animcode[line] == null)
                            continue;
                        else if (animcode[line] == "")
                            continue;
                        else for (Int32 i = 0; (i < animcode[line].Length); i++)
                        {
                            if (animcode[line][i] == '#') break;

                            switch (animcode[line][i])
                            {
                                case ' ': continue;

                                case '\t': continue;

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
                                    if (frameCompile) throw new Exception("Unexpected number read.");
                                        Int32 length = 1;
                                    while (animcode[line][i + length] != ' ') length++;
                                    duration = (Byte)Int32.Parse(animcode[line].Substring(i, length));
                                    frameCompile = true;
                                    i += length;
                                    break;

                                case 'f':
                                case 'F':
                                    if (frameCompile)
                                    {
                                        frame = Util.HexToByte(animcode[line].Substring(i + 1, 2));
                                        if (frame >= frames.Length) throw new Exception(
                                            "Frame number " + Util.ByteToHex(frame) + " is invalid.");
                                        result.Add(duration);
                                        result.Add(0x00);
                                        result.Add(frame);
                                        result.Add(0x86);
                                        result.AddRange(Util.UInt32ToBytes(frames[frame].Item1, true));
                                        result.AddRange(Util.UInt32ToBytes(frames[frame].Item2, true));
                                        frameCompile = false;
                                        i += 2;
                                    }
                                    else throw new Exception("Frame command must have a duration number before it.");
                                    break;

                                case 'c':
                                case 'C':
                                    if (frameCompile) throw new Exception("Unexpected 'c' battle anim command read.");
                                    result.Add(Util.HexToByte(animcode[line].Substring(i + 1, 2)));
                                    result.Add(0x00);
                                    result.Add(0x00);
                                    result.Add(0x85);
                                    i += 2;
                                    break;

                                case 'e':
                                case 'E':
                                    if ((animcode[line][i + 1] == 'n' || animcode[line][i + 1] == 'N') &&
                                        (animcode[line][i + 2] == 'd' || animcode[line][i + 2] == 'D'))
                                    {
                                        if (frameCompile) throw new Exception("Unexpected terminator read.");

                                        result.AddRange(new Byte[4] { 0x00, 0x00, 0x00, 0x80 });

                                        return result.ToArray();
                                    }
                                    else throw new Exception("Invalid terminator read.");
                                    
                                default: throw new Exception("Unexpected character: " + animcode[line][i]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("At line " + (line + 1) + ":\r\n" + animcode[line] + "\r\n" + ex.Message);
                    }
                }
                throw new Exception("At end of code:\r\n" + "No 'end' terminator found.");
            }
            catch (Exception ex)
            {
                throw new Exception("There has been an error while trying to compile the animation code.\n\n" + ex.Message);
            }
        }
        /// <summary>
        /// Converts the given byte array of animation data into animation code, stopping at the first terminator met
        /// </summary>
        public static String[] DecompileAnimCode(Byte[] animdata, UInt32 offset, BattleAnimFrame[] frames)
        {
            List<String> result = new List<String>();
            Int32 index = 0;
            for (UInt32 i = offset; i < animdata.Length; i += 4)
            {
                if (animdata[i + 3] == 0x86)
                {
                    Byte duration = animdata[i];
                    Int32 frame = -1;
                    Pointer pointer = new Pointer(animdata.GetUInt32(i + 4, false), true, true);
                    UInt32 OAM_offset = animdata.GetUInt32(i + 8, true);

                    for (Int32 f = 0; f < frames.Length; f++)
                    {
                        if (frames[f] != null &&
                            frames[f].TilesetIndex == pointer &&
                            frames[f].OAM_Offset == OAM_offset)
                        { frame = f; break; }
                    }
                    //if (frame == -1) frame = animdata[i + 2];
                    result.Add(duration + " f" + Util.ByteToHex((Byte)frame)); // + " " + pointer + " " + OAM_offset);
                    i += 8;
                }
                else if (animdata[i + 3] == 0x85)
                {
                    result.Add("c" + Util.ByteToHex(animdata[i]));
                    index = result.Count - 1;
                    try
                    {
                        result[index] += " #" + File.ReadAllLines(Core.Path_Arrays + "Battle Animation Commands.txt")[animdata[i]].Substring(4);
                    }
                    catch { continue; }
                }
                else if (animdata[i + 3] == 0x80)
                {
                    if (animdata[i] == 0x00 && animdata[i + 1] == 0x00 && animdata[i + 2] == 0x00)
                    {
                        result.Add("end");
                        return result.ToArray();
                    }
                    else throw new Exception("Invalid terminator at offset " + i);
                }
                else throw new Exception("Invalid battle anim command: 0x" + Util.ByteToHex(animdata[i + 3]));
            }
            throw new Exception("No terminator found.");
        }

        /// <summary>
        /// Returns the two seperate anim codes from a 2-layered mode's string array
        /// </summary>
        public static Tuple<String[], String[]> SplitLayeredAnimationCode(String[] animcode)
        {
            try
            {
                String[] result1 = new String[animcode.Length];
                String[] result2 = new String[animcode.Length];
                String duration = null;
                String command;
                Int32 compile = 0; // 0 is normal, 1 = expecting f command, 2 = expecting b command
                for (Int32 line = 0; line < animcode.Length; line++)
                {
                    try
                    {
                        if (animcode[line] == null)
                            continue;
                        if (animcode[line] == "")
                            continue;
                        for (Int32 i = 0; (i < animcode[line].Length); i++)
                        {
                            if (animcode[line][i] == '#') break;

                            switch (animcode[line][i])
                            {
                                case ' ': continue;

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
                                    if (compile > 0) throw new Exception("Unexpected number encountered.");
                                    Int32 length = 1;
                                    while (animcode[line][i + length] != ' ') length++;
                                    duration = animcode[line].Substring(i, length + 1);
                                    result1[line] += duration;
                                    result2[line] += duration;
                                    compile = 1;
                                    i += length;
                                    break;

                                case 'f':
                                case 'F':
                                    if (compile == 1)
                                    {
                                        result1[line] += animcode[line].Substring(i, 3);
                                        compile = 2;
                                        i += 2;
                                    }
                                    else throw new Exception("Unexpected 'f' frame command encountered.");
                                    break;

                                case 'b':
                                case 'B':
                                    if (compile == 2)
                                    {
                                        result2[line] += "f" + animcode[line].Substring(i + 1, 2);
                                        compile = 0;
                                        i += 2;
                                    }
                                    else throw new Exception("Unexpected 'b' back-layer frame command encountered.");
                                    break;

                                case 'a':
                                case 'A':
                                    if (compile == 0 && duration != null)
                                    {
                                        duration = null;
                                        command = animcode[line].Substring(i, 3);
                                        result1[line] += command;
                                        result2[line] += command;
                                        i += 2;
                                    }
                                    else throw new Exception("Unexpected 'a' affine sprite command encountered.");
                                    break;

                                case 'c':
                                case 'C':
                                    if (compile > 0) throw new Exception("Unexpected 'c' battle anim command encountered.");
                                    duration = null;
                                    command = animcode[line].Substring(i, 3);
                                    result1[line] += command;
                                    result2[line] += command;
                                    i += 2;
                                    break;

                                case 'e':
                                case 'E':
                                    if ((animcode[line][i + 1] == 'n' || animcode[line][i + 1] == 'N') &&
                                        (animcode[line][i + 2] == 'd' || animcode[line][i + 2] == 'D'))
                                    {
                                        if (compile > 0) throw new Exception("Unexpected terminator encountered.");
                                        result1[line] += "end";
                                        result2[line] += "end";
                                        return Tuple.Create(result1, result2);
                                    }
                                    else throw new Exception("Invalid terminator read.");

                                default: throw new Exception("Unexpected character: " + animcode[line][i]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("At line " + (line + 1) + ":\r\n" + animcode[line] + "\r\n" + ex.Message);
                    }
                }
                throw new Exception("At end of code:\r\n" + "No 'end' terminator found.");
            }
            catch (Exception ex)
            {
                throw new Exception("There has been an error while trying to split the 2-layered anim code.\n\n" + ex.Message);
            }
        }

        /// <summary>
        /// Makes one line-returned string from a string array of anim code
        /// </summary>
        public static String GetAnimationCode(String[][] animcode,
            Int32 mode, Boolean layered, Boolean every_mode,
            ref List<Tuple<UInt32, OAM>> affines, ref Int32[] duplicates,
            String file = null, BattleAnimFrame[] frames = null)
        {
            String result = "";
            Int32 i = (every_mode) ? 0 : mode;
            Int32 f, f1, f2;
            Int32 frame_index;
            String frame;
            String frame1;
            String frame2;
            while (every_mode ? i < BattleAnimation.MODES : i == mode)
            {
                if (layered && (i == 0 || i == 2 || i == 8)) // two-layer anim modes
                {
                    result += "# Modes " + (i + 1) + " & " + (i + 2) + ": ";
                    result += BattleAnimation.Modes_Layered[(i == 8) ? 6 : i / 2];
                    String[] layer1 = animcode[i]; Int32 l1 = 0; // front layer
                    String[] layer2 = animcode[i + 1]; Int32 l2 = 0; // back layer
                    while (l1 < layer1.Length || l2 < layer2.Length)
                    {
                        result += "\r\n";
                        if (i != BattleAnimation.MODES - 1 && layer1[l1] == "end")
                        {
                            result += "end";
                            if (every_mode)
                                result += "\r\n\r\n";
                            else return result;
                        }
                        else if (l1 >= layer1.Length || l2 >= layer2.Length || layer1[l1][0] != layer2[l2][0])
                        {   // sometimes there are 'c' commands in layer2 that aren't in layer1's anim code, and vice-versa
                            // go see the lyn blade lord critical anim for an example, or brigand handaxe attack
                            if (l2 < layer2.Length && layer2[l2][0] == 'c')
                            {
                                for (Int32 j = 0; j < layer2[l2].Length; j++)
                                {
                                    result += layer2[l2][j];
                                }
                                l1--;
                            }
                            else if (l1 < layer1.Length && layer1[l1][0] == 'c')
                            {
                                for (Int32 j = 0; j < layer1[l1].Length; j++)
                                {
                                    result += layer1[l1][j];
                                }
                                l2--;
                            }
                        }
                        else for (Int32 j = 0; j < layer1[l1].Length; j++)
                        {
                            if (layer1[l1][j] == 'f')
                            {
                                j++;
                                frame1 = layer1[l1].Substring(j, 2);
                                frame2 = layer2[l2].Substring(j, 2);
                                if (file != null)
                                {
                                    f1 = Util.HexToByte(frame1);
                                    frame_index = GetAnimationCode_CheckDuplicateFrames(f1, frames, ref duplicates);
                                    frame1 = frame_index.ToString();
                                    result += (  "f [" + file + "_" + frame1 + ".png]");
                                    result += GetAnimationCode_AddAffinesIfAny(file,
                                        frames[f1].TilesetIndex, frames[f1].OAM_Data_R, ref affines);
                                    f2 = Util.HexToByte(frame2);
                                    frame_index = GetAnimationCode_CheckDuplicateFrames(f2, frames, ref duplicates);
                                    frame2 = frame_index.ToString();
                                    result += ("  b [" + file + "_" + frame2 + ".png]");
                                    result += GetAnimationCode_AddAffinesIfAny(file,
                                        frames[f2].TilesetIndex, frames[f2].OAM_Data_R, ref affines);
                                }
                                else result += ("f" + frame1 + " b" + frame2);
                                j++;
                            }
                            else if (layer1[l1][j] == '#')
                            {
                                result += layer1[l1].Substring(j); break;
                            }
                            else result += layer1[l1][j];
                        }
                        l1++;
                        l2++;
                    }
                    i++;
                }
                else
                {
                    result += "# Mode " + (i + 1) + ": ";
                    result += BattleAnimation.Modes[i];
                    foreach (String line in animcode[i])
                    {
                        result += "\r\n";
                        if (i != BattleAnimation.MODES - 1 && line == "end")
                        {
                            result += "end";
                            if (every_mode)
                                result += "\r\n\r\n";
                            else return result;
                        }
                        else for (Int32 j = 0; j < line.Length; j++)
                        {
                            if (line[j] == 'f')
                            {
                                j++;
                                frame = line.Substring(j, 2);
                                if (file != null)
                                {
                                    f = Util.HexToByte(frame);
                                    frame_index = GetAnimationCode_CheckDuplicateFrames(f, frames, ref duplicates);
                                    frame = frame_index.ToString();
                                    result += ("f [" + file + "_" + frame + ".png]");
                                    result += GetAnimationCode_AddAffinesIfAny(file,
                                        frames[f].TilesetIndex, frames[f].OAM_Data_R, ref affines);
                                }
                                else result += ("f" + frame);
                                j++;
                            }
                            else if (line[j] == '#')
                            {
                                result += line.Substring(j); break;
                            }
                            else result += line[j];
                        }
                    }
                }
                i++;
            }
            return result;
        }
        private static Int32 GetAnimationCode_CheckDuplicateFrames(
            Int32 frame, BattleAnimFrame[] frames, ref Int32[] duplicates)
        {
            List<OAM> oam = frames[frame].OAM_Data_R.Sprites;
            List<OAM> check_oam;
            Boolean equal;
            for (Int32 i = 0; i < frame; i++)
            {
                if (frames[i] == null || i == frame)
                    continue;
                check_oam = frames[i].OAM_Data_R.Sprites;
                equal = true;
                for (Int32 j = 0; j < oam.Count; j++)
                {
                    if (j >= check_oam.Count)
                    {
                        if (!oam[j].IsAffineSprite())
                        {
                            equal = false;
                            break;
                        }
                        else continue;
                    }
                    if (oam[j].IsAffineSprite() || check_oam[j].IsAffineSprite())
                        continue;
                    if (!check_oam[j].Equals(oam[j]))
                    {
                        equal = false;
                        break;
                    }
                }
                if (equal)
                {
                    duplicates[frame] = i;
                    return i;
                }
            }
            return frame;
        }
        private static String GetAnimationCode_AddAffinesIfAny(String file,
            UInt32 tileset, OAM_Array oam, ref List<Tuple<UInt32, OAM>> affines)
        {
            String result = "";
            Int32 s = 0;
            while (s < oam.Sprites.Count)
            {
                if (oam.Sprites[s].IsAffineSprite())
                {
                    Int32 affine_index = -1;
                    for (Int32 i = 0; i < affines.Count; i++)
                    {
                        if (tileset == affines[i].Item1 &&
                            oam.Sprites[s].SheetX == affines[i].Item2.SheetX &&
                            oam.Sprites[s].SheetY == affines[i].Item2.SheetY &&
                            oam.Sprites[s].SpriteShape == affines[i].Item2.SpriteShape &&
                            oam.Sprites[s].SpriteSize == affines[i].Item2.SpriteSize)
                            affine_index = i;
                    }
                    if (affine_index == -1)
                    {
                        affine_index = affines.Count;
                        affines.Add(Tuple.Create(tileset, oam.Sprites[s]));
                    }
                    if (result.Length > 0)
                        result += "\r\n\t";
                    Int32 index = oam.Sprites[s].AffineIndex;
                    Boolean big = oam.Sprites[s].OBJMode == OAM_OBJMode.BigAffine;
                    result += (big ? " d" : " a");
                    result += " [" + file + "_affine_" + affine_index;
                    result += ".png]";
                    result += " (" + (oam.Sprites[s].ScreenX + SCREEN_OFFSET_X_R);
                    result += ", " + (oam.Sprites[s].ScreenY + SCREEN_OFFSET_Y);
                    result += ") ("+ oam.Affines[index].Ux.ToString("0.00000").Replace(',', '.');
                    result += ", " + oam.Affines[index].Uy.ToString("0.00000").Replace(',', '.');
                    result += ", " + oam.Affines[index].Vx.ToString("0.00000").Replace(',', '.');
                    result += ", " + oam.Affines[index].Vy.ToString("0.00000").Replace(',', '.');
                    result += ")";
                }
                s++;
            }
            return result;
        }

        /// <summary>
        /// Replaces the tileset indices after 0x86 commands with the pointers in the given list
        /// </summary>
        public static Byte[] PrepareAnimationData(Byte[] animdata, List<Pointer> pointers)
        {
            for (UInt32 i = 0; i < animdata.Length; i += 4)
            {
                if (animdata[i + 3] == 0x86)
                {
                    i += 4;
                    Int32 tileset = animdata.GetInt32(i, true);
                    Array.Copy(pointers[tileset].ToBytes(), 0, animdata, i, 4);
                }
            }
            return animdata;
        }
        /// <summary>
        /// Replaces the tileset pointers after 0x86 commands with simple tileset indices
        /// </summary>
        public static Byte[] ExportAnimationData(Byte[] animdata, List<Pointer> pointers)
        {
            Byte[] buffer = new Byte[4];
            for (Int32 i = 0; i < animdata.Length; i += 4)
            {
                if (animdata[i + 3] == 0x86)
                {
                    i += 4;
                    Array.Copy(animdata, i, buffer, 0, 4);
                    Pointer pointer = new Pointer(buffer);

                    Int32 tileset = pointers.FindIndex(delegate (Pointer p) {
                        return (p == pointer);
                    });
                    Array.Copy(Util.Int32ToBytes(tileset, true), 0, animdata, i, 4);
                }
            }
            return animdata;
        }
    }
}
