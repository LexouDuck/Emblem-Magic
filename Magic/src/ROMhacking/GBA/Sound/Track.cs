using System;
using System.Collections.Generic;
using GBA;
using Magic;

namespace GBA
{
    public class Track
    {
        public Pointer Address;
        public Byte[] Data;

        public Track(Pointer address)
        {
            List<Byte> data = new List<Byte>();
            Int32 index = 0;
            Byte buffer = 0x00;
            while (buffer != 0xB1)
            {
                buffer = Core.ReadByte(address + index);
                data.Add(buffer);
                index++;
            }
            this.Data = data.ToArray();
            this.Address = address;
        }

        String GetAddress(UInt32 address)
        {
            return Util.IntToHex(address - this.Address - Pointer.HardwareOffset);
        }
        Byte GetLength(Byte note, Boolean wait)
        {
            Int32 offset = wait ? 0x81 : 0xD0;

            if (note == 0x80)
                return 0;
            else if (note < offset)
                throw new Exception("Invalid timed note-on command byte.");
            else if (note < offset + 24)
                return (Byte)(note - offset);
            else
            {
                Byte result = 24;
                for (Int32 i = (offset + 24); i <= note; i++)
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
        public Int64 GetFrequency(Byte MIDI_note, Int64 base_key = -1)
        {
            Double magic = Math.Pow(2, (1d / 12d));
            Double result;
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
            return (Int64)(result * Math.Pow(magic, MIDI_note - 0x3C));
        }

        public String[][] GetTrackerString(ArrayFile notes)
        {
            const Int32 subcolumns = 5;
            const Int32 col_notes = 0;
            const Int32 col_vol = 1;
            const Int32 col_ins = 2;
            const Int32 col_pan = 3;
            const Int32 col_effects = 4;
            List<String[]> result = new List<String[]>();
            Byte length = 0; // used for all the time-tick relevant commands
            Byte repeat = 0; // stores the last repeatable command
            Byte note = 0; // stores the last note key played
            Int32 index = 0;
            result.Add(new String[subcolumns]);
            for (UInt32 i = 0; i < this.Data.Length; i++)
            {
                if (this.Data[i] < 0x80) // repeat command
                {
                    if (repeat >= 0xD0) // note-on command
                    {
                        if (result[index][col_notes] != null)
                            result[index][col_notes] += '\n';
                        length = this.GetLength(repeat, false);
                        result[index][col_notes] += notes[this.Data[i]];
                        note = this.Data[i];
                        if ((SByte)this.Data[i + 1] >= 0) result[index][col_notes] += ", " + Util.ByteToHex(this.Data[++i]);
                        if ((SByte)this.Data[i + 1] >= 0) length += this.Data[++i];
                        for (Int32 j = 0; j < length; j++)
                        {
                            result.Add(new String[subcolumns]);
                            index++;
                        }
                    }
                    else switch (repeat)
                        {
                            case 0xBD: // Set Instrument (1-byte arg)
                                result[index][col_ins] = Util.ByteToHex(this.Data[i]);
                                break;
                            case 0xBE: // Set Volume (1-byte arg)
                                result[index][col_vol] = Util.ByteToHex(this.Data[i]);
                                break;
                            case 0xBF: // Set Panning (1-byte arg)
                                result[index][col_pan] = Util.ByteToHex(this.Data[i]);
                                break;
                            case 0xC0: // Pitch bend value (1-byte arg)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                result[index][col_effects] += "pitch:" + (this.Data[i] - 0x40);
                                break;
                            case 0xC1: // Pitch bend semitones (1-byte arg)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                result[index][col_effects] += "slide:" + (this.Data[i] - 0x40);
                                break;
                            case 0xC4: // LFO Depth (1-byte arg)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                result[index][col_effects] += "lfo_depth:" + this.Data[i];
                                break;

                            case 0xC8: // Detune (1-byte arg)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                result[index][col_effects] += "tuning:" + (this.Data[i] - 0x40);
                                break;

                            case 0xCD: // Echo (two 1-byte args)
                                if (result[index][col_effects] != null)
                                    result[index][col_effects] += '\n';
                                switch (this.Data[i])
                                {
                                    case 0x08: result[index][col_effects] += "echo_vol:" + this.Data[++i]; break;
                                    case 0x09: result[index][col_effects] += "echo_len:" + this.Data[++i]; break;
                                    default:; break;
                                }
                                break;

                            case 0xCE: // Note Off (two optional args)
                                if (result[index][col_notes] != null)
                                    result[index][col_notes] += '\n';
                                result[index][col_notes] += "___";
                                if ((SByte)this.Data[i + 1] >= 0) { result[index][col_notes] += ", " + notes[this.Data[i]]; note = this.Data[i]; }
                                if ((SByte)this.Data[i + 1] >= 0) { result[index][col_notes] += ", " + Util.ByteToHex(this.Data[++i]); }
                                break;
                            case 0xCF: // Note On (two optional args)
                                if (result[index][col_notes] != null)
                                    result[index][col_notes] += '\n';
                                if ((SByte)this.Data[i + 1] >= 0) { result[index][col_notes] += notes[this.Data[i]]; note = this.Data[i]; }
                                else result[index][col_notes] += note;
                                if ((SByte)this.Data[i + 1] >= 0) { result[index][col_notes] += ", " + Util.ByteToHex(this.Data[++i]); }
                                break;

                            default: throw new Exception("No repeatable command to execute.");
                        }
                }
                else if (this.Data[i] <= 0xB0) // wait command
                {
                    length = this.GetLength(this.Data[i], true);
                    for (Int32 j = 0; j < length; j++)
                    {
                        result.Add(new String[subcolumns]);
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
                else if (this.Data[i] >= 0xD0) // note-on command
                {
                    repeat = this.Data[i];
                    if (result[index][col_notes] != null)
                        result[index][col_notes] += '\n';
                    length = this.GetLength(this.Data[i], false);
                    result[index][col_notes] += notes[this.Data[++i]];
                    note = this.Data[i];
                    if ((SByte)this.Data[i + 1] >= 0) result[index][col_notes] += ", " + Util.ByteToHex(this.Data[++i]);
                    if ((SByte)this.Data[i + 1] >= 0) length += this.Data[++i];
                    for (Int32 j = 0; j < length; j++)
                    {
                        result.Add(new String[subcolumns]);
                        index++;
                    }
                }
                else switch (this.Data[i])
                {
                    case 0xB1: // End of track
                        if (i != this.Data.Length - 1)
                            throw new Exception("There shouldn't be any data after an end of track command (0xB1)");
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "end";
                        break;
                    case 0xB2: // Jump to address (4-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        if (i + 4 >= this.Data.Length) result[index][col_effects] += "loop";
                        else result[index][col_effects] += "jump:" + this.GetAddress(this.Data.GetUInt32((uint)++i, true));
                        i += 3;
                        break;
                    case 0xB3: // Call subsection (4-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        if (i + 4 >= this.Data.Length) result[index][col_effects] += "call";
                        else result[index][col_effects] += "call:" + this.GetAddress(this.Data.GetUInt32((uint)++i, true));
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
                        result[index][col_effects] += "loop:" + this.Data[++i] + "*" + this.GetAddress(this.Data.GetUInt32((uint)++i, true));
                        i += 4;
                        break;

                    case 0xB9: // Conditional jump based on memory content (3 bytes..?)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "jump:" + this.Data[++i] + "*" + this.Data[++i] + "*" + this.Data[++i];
                        break;
                    case 0xBA: // Set track priority (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "priority:" + this.Data[++i];
                        break;
                    case 0xBB: // Set tempo (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "tempo:" + this.Data[++i];
                        break;
                    case 0xBC: // Transpose (1 signed byte)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "transpose:" + (SByte)this.Data[++i];
                        break;
                    case 0xBD: // Set Instrument (1-byte arg)
                        repeat = this.Data[i];
                        result[index][col_ins] = Util.ByteToHex(this.Data[++i]);
                        break;
                    case 0xBE: // Set Volume (1-byte arg)
                        repeat = this.Data[i];
                        result[index][col_vol] = Util.ByteToHex(this.Data[++i]);
                        break;
                    case 0xBF: // Set Panning (1-byte arg)
                        repeat = this.Data[i];
                        result[index][col_pan] = Util.ByteToHex(this.Data[++i]);
                        break;
                    case 0xC0: // Pitch bend value (1-byte arg)
                        repeat = this.Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "pitch:" + (this.Data[++i] - 0x40);
                        break;
                    case 0xC1: // Pitch bend semitones (1-byte arg)
                        repeat = this.Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "slide:" + (this.Data[++i] - 0x40);
                        break;
                    case 0xC2: // LFO Speed (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "lfo_speed:" + this.Data[++i];
                        break;
                    case 0xC3: // LFO Delay (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "lfo_delay:" + this.Data[++i];
                        break;
                    case 0xC4: // LFO Depth (1-byte arg)
                        repeat = this.Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "lfo_depth:" + this.Data[++i];
                        break;
                    case 0xC5: // LFO Type (1-byte arg)
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "lfo_type:" + this.Data[++i];
                        break;

                    case 0xC8: // Detune (1-byte arg)
                        repeat = this.Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        result[index][col_effects] += "detune:" + (this.Data[++i] - 0x40);
                        break;

                    case 0xCD: // Echo (two 1-byte args)
                        repeat = this.Data[i];
                        if (result[index][col_effects] != null)
                            result[index][col_effects] += '\n';
                        switch (this.Data[++i])
                        {
                            case 0x08: result[index][col_effects] += "echo_vol:" + this.Data[++i]; break;
                            case 0x09: result[index][col_effects] += "echo_len:" + this.Data[++i]; break;
                            default:; break;
                        }
                        break;

                    case 0xCE: // Note Off (two optional args)
                        repeat = this.Data[i];
                        if (result[index][col_notes] != null)
                            result[index][col_notes] += '\n';
                        result[index][col_notes] += "___";
                        if ((SByte)this.Data[i + 1] >= 0) { result[index][col_notes] += ", " + notes[this.Data[++i]]; note = this.Data[i]; }
                        if ((SByte)this.Data[i + 1] >= 0) { result[index][col_notes] += ", " + Util.ByteToHex(this.Data[++i]); }
                        break;
                    case 0xCF: // Note On (two optional args)
                        repeat = this.Data[i];
                        if (result[index][col_notes] != null)
                            result[index][col_notes] += '\n';
                        if ((SByte)this.Data[i + 1] >= 0) { result[index][col_notes] += notes[this.Data[++i]]; note = this.Data[i]; }
                        else result[index][col_notes] += note;
                        if ((SByte)this.Data[i + 1] >= 0) { result[index][col_notes] += ", " + Util.ByteToHex(this.Data[++i]); }
                        break;

                    default: throw new Exception("Unsupported command: " + Util.ByteToHex(this.Data[i]));
                }
            }
            return result.ToArray();
        }
    }
}
