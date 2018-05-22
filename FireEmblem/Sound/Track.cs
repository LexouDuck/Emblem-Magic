using GBA;
using System;
using System.Collections.Generic;

namespace EmblemMagic.FireEmblem
{
    public class Track
    {
        public Pointer Address;
        public byte[] Data;

        public Track(Pointer address)
        {
            List<byte> data = new List<byte>();
            int index = 0;
            byte buffer = 0x00;
            while (buffer != 0xB1)
            {
                buffer = Core.ReadByte(address + index);
                data.Add(buffer);
                index++;
            }
            Data = data.ToArray();
            Address = address;
        }

        string GetAddress(uint address)
        {
            return Util.IntToHex(address - this.Address - Pointer.HardwareOffset);
        }
        byte GetLength(byte note, bool wait)
        {
            int offset = wait ? 0x81 : 0xD0;

            if (note == 0x80)
                return 0;
            else if (note < offset)
                throw new Exception("Invalid timed note-on command byte.");
            else if (note < offset + 24)
                return (byte)(note - offset);
            else
            {
                byte result = 24;
                for (int i = (offset + 24); i <= note; i++)
                {
                    switch (i % 4)
                    {
                        case 0: result += 4; break;
                        case 1: result += 2; break;
                        case 2: result += 2; break;
                        case 3: result += 4; break;
                    }
                }
                return result;
            }
        }
        public long GetFrequency(byte MIDI_note, long base_key = -1)
        {
            double magic = Math.Pow(2, (1d / 12d));
            double result;
            if (base_key == -1)
            {
                result = 7040 * Math.Pow(magic, 3);
            }
            else if (base_key == -2)
            {
                result = (7040 / 2) * Math.Pow(magic, 3);
            }
            else
            {
                result = base_key;
            }
            return (long)(result * Math.Pow(magic, MIDI_note - 0x3C));
        }

        public string[][] GetTrackerString(ArrayFile notes)
        {
            const int subcolumns = 5;
            const int col_notes = 0;
            const int col_vol = 1;
            const int col_ins = 2;
            const int col_pan = 3;
            const int col_effects = 4;
            List<string[]> result = new List<string[]>();
            byte length = 0; // used for all the time-tick relevant commands
            byte repeat = 0; // stores the last repeatable command
            byte note = 0; // stores the last note key played
            int index = 0;
            result.Add(new string[subcolumns]);
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i] < 0x80) // repeat command
                {
                    if (repeat >= 0xD0) // note-on command
                    {
                        if (result[index][col_notes] != null)
                            result[index][col_notes] += '\n';
                        length = GetLength(repeat, false);
                        result[index][col_notes] += notes[Data[i]];
                        note = Data[i];
                        if ((sbyte)Data[i + 1] >= 0) result[index][col_notes] += ", " + Util.ByteToHex(Data[++i]);
                        if ((sbyte)Data[i + 1] >= 0) length += Data[++i];
                        for (int j = 0; j < length; j++)
                        {
                            result.Add(new string[subcolumns]);
                            index++;
                        }
                    }
                    else switch (repeat)
                        {
                            case 0xBD: // Set Instrument (1-byte arg)
                                result[index][col_ins] = Util.ByteToHex(Data[i]);
                                break;
                            case 0xBE: // Set Volume (1-byte arg)
                                result[index][col_vol] = Util.ByteToHex(Data[i]);
                                break;
                            case 0xBF: // Set Panning (1-byte arg)
                                result[index][col_pan] = Util.ByteToHex(Data[i]);
                                break;
                            case 0xC0: // Pitch bend value (1-byte arg)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                result[index][col_effects] += "pitch:" + (Data[i] - 0x40);
                                break;
                            case 0xC1: // Pitch bend semitones (1-byte arg)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                result[index][col_effects] += "slide:" + (Data[i] - 0x40);
                                break;
                            case 0xC4: // LFO Depth (1-byte arg)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                result[index][col_effects] += "lfo_depth:" + Data[i];
                                break;

                            case 0xC8: // Detune (1-byte arg)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                result[index][col_effects] += "tuning:" + (Data[i] - 0x40);
                                break;

                            case 0xCD: // Echo (two 1-byte args)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                switch (Data[i])
                                {
                                    case 0x08: result[index][col_effects] += "echo_vol:" + Data[++i]; break;
                                    case 0x09: result[index][col_effects] += "echo_len:" + Data[++i]; break;
                                    default:; break;
                                }
                                break;

                            case 0xCE: // Note Off (two optional args)
                                if (result[index][col_notes] != null)
                                    result[index][col_notes] += '\n';
                                result[index][col_notes] += "___";
                                if ((sbyte)Data[i + 1] >= 0) { result[index][col_notes] += ", " + notes[Data[i]]; note = Data[i]; }
                                if ((sbyte)Data[i + 1] >= 0) { result[index][col_notes] += ", " + Util.ByteToHex(Data[++i]); }
                                break;
                            case 0xCF: // Note On (two optional args)
                                if (result[index][col_notes] != null)
                                    result[index][col_notes] += '\n';
                                if ((sbyte)Data[i + 1] >= 0) { result[index][col_notes] += notes[Data[i]]; note = Data[i]; }
                                else result[index][col_notes] += note;
                                if ((sbyte)Data[i + 1] >= 0) { result[index][col_notes] += ", " + Util.ByteToHex(Data[++i]); }
                                break;

                            default: throw new Exception("No repeatable command to execute.");
                        }
                }
                else if (Data[i] <= 0xB0) // wait command
                {
                    length = GetLength(Data[i], true);
                    for (int j = 0; j < length; j++)
                    {
                        result.Add(new string[subcolumns]);
                        index++;
                    }
                    /*
                    if (length % ticks != 0)
                    {
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "timing:" + (length % ticks);
                    }*/
                }
                else if (Data[i] >= 0xD0) // note-on command
                {
                    repeat = Data[i];
                    if (result[index][col_notes] != null)
                        result[index][col_notes] += '\n';
                    length = GetLength(Data[i], false);
                    result[index][col_notes] += notes[Data[++i]];
                    note = Data[i];
                    if ((sbyte)Data[i + 1] >= 0) result[index][col_notes] += ", " + Util.ByteToHex(Data[++i]);
                    if ((sbyte)Data[i + 1] >= 0) length += Data[++i];
                    for (int j = 0; j < length; j++)
                    {
                        result.Add(new string[subcolumns]);
                        index++;
                    }
                }
                else switch (Data[i])
                {
                    case 0xB1: // End of track
                        if (i != Data.Length - 1)
                            throw new Exception("There shouldn't be any data after an end of track command (0xB1)");
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "end";
                        break;
                    case 0xB2: // Jump to address (4-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        if (i + 4 >= Data.Length) result[index][col_effects] += "loop";
                        else result[index][col_effects] += "jump:" + GetAddress(Data.GetUInt32((uint)++i, true));
                        i += 3;
                        break;
                    case 0xB3: // Call subsection (4-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        if (i + 4 >= Data.Length) result[index][col_effects] += "call";
                        else result[index][col_effects] += "call:" + GetAddress(Data.GetUInt32((uint)++i, true));
                        i += 3;
                        break;
                    case 0xB4: // End subsection
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "done";
                        break;
                    case 0xB5: // Call and repeat subsection. (1-byte and 4-byte args)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "loop:" + Data[++i] + "*" + GetAddress(Data.GetUInt32((uint)++i, true));
                        i += 4;
                        break;

                    case 0xB9: // Conditional jump based on memory content (3 bytes..?)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "jump:" + Data[++i] + "*" + Data[++i] + "*" + Data[++i];
                        break;
                    case 0xBA: // Set track priority (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "priority:" + Data[++i];
                        break;
                    case 0xBB: // Set tempo (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "tempo:" + Data[++i];
                        break;
                    case 0xBC: // Transpose (1 signed byte)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "transpose:" + (sbyte)Data[++i];
                        break;
                    case 0xBD: // Set Instrument (1-byte arg)
                        repeat = Data[i];
                        result[index][col_ins] = Util.ByteToHex(Data[++i]);
                        break;
                    case 0xBE: // Set Volume (1-byte arg)
                        repeat = Data[i];
                        result[index][col_vol] = Util.ByteToHex(Data[++i]);
                        break;
                    case 0xBF: // Set Panning (1-byte arg)
                        repeat = Data[i];
                        result[index][col_pan] = Util.ByteToHex(Data[++i]);
                        break;
                    case 0xC0: // Pitch bend value (1-byte arg)
                        repeat = Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "pitch:" + (Data[++i] - 0x40);
                        break;
                    case 0xC1: // Pitch bend semitones (1-byte arg)
                        repeat = Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "slide:" + (Data[++i] - 0x40);
                        break;
                    case 0xC2: // LFO Speed (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "lfo_speed:" + Data[++i];
                        break;
                    case 0xC3: // LFO Delay (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "lfo_delay:" + Data[++i];
                        break;
                    case 0xC4: // LFO Depth (1-byte arg)
                        repeat = Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "lfo_depth:" + Data[++i];
                        break;
                    case 0xC5: // LFO Type (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "lfo_type:" + Data[++i];
                        break;

                    case 0xC8: // Detune (1-byte arg)
                        repeat = Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "detune:" + (Data[++i] - 0x40);
                        break;

                    case 0xCD: // Echo (two 1-byte args)
                        repeat = Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        switch (Data[++i])
                        {
                            case 0x08: result[index][col_effects] += "echo_vol:" + Data[++i]; break;
                            case 0x09: result[index][col_effects] += "echo_len:" + Data[++i]; break;
                            default:; break;
                        }
                        break;

                    case 0xCE: // Note Off (two optional args)
                        repeat = Data[i];
                        if (result[index][col_notes] != null)
                            result[index][col_notes] += '\n';
                        result[index][col_notes] += "___";
                        if ((sbyte)Data[i + 1] >= 0) { result[index][col_notes] += ", " + notes[Data[++i]]; note = Data[i]; }
                        if ((sbyte)Data[i + 1] >= 0) { result[index][col_notes] += ", " + Util.ByteToHex(Data[++i]); }
                        break;
                    case 0xCF: // Note On (two optional args)
                        repeat = Data[i];
                        if (result[index][col_notes] != null)
                            result[index][col_notes] += '\n';
                        if ((sbyte)Data[i + 1] >= 0) { result[index][col_notes] += notes[Data[++i]]; note = Data[i]; }
                        else result[index][col_notes] += note;
                        if ((sbyte)Data[i + 1] >= 0) { result[index][col_notes] += ", " + Util.ByteToHex(Data[++i]); }
                        break;

                    default: throw new Exception("Unsupported command: " + Util.ByteToHex(Data[i]));
                }
            }
            return result.ToArray();
        }
    }
}
