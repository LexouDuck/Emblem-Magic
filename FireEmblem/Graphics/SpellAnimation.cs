using GBA;
using System;
using System.Collections.Generic;

namespace EmblemMagic.FireEmblem
{
    public struct SpellAnimFrame
    {
        /// <summary>
        /// The length of one block of OAM data
        /// </summary>
        public const int LENGTH = 72;

        /// <summary>
        /// The entire struct is just a wrapper for this field.
        /// </summary>
        byte[] Data;



        byte FlagByte
        {
            get
            {
                return Data[0];
            }
            set
            {
                Data[0] = value;
            }
        }
        Int16 OffsetX
        {
            get
            {
                return Data.GetInt16(2, true);
            }
            set
            {
                byte[] data = Util.Int16ToBytes(value, true);
                Array.Copy(data, 0, Data, 2, 2);
            }
        }
        byte Duration
        {
            get
            {
                return Data[6];
            }
            set
            {
                Data[6] = value;
            }
        }
        UInt16 TSAwtf
        {
            get
            {
                return Data.GetUInt16(8, true);
            }
            set
            {
                byte[] data = Util.UInt16ToBytes(value, true);
                Array.Copy(data, 0, Data, 8, 2);
            }
        }
        byte Frame
        {
            get
            {
                return Data[19];
            }
            set
            {
                Data[19] = value;
            }
        }
        byte Command
        {
            get
            {
                return Data[21];
            }
            set
            {
                Data[21] = value;
            }
        }
        Pointer FramePointer
        {
            get
            {
                return new Pointer(Data.GetUInt32(32, true));
            }
            set
            {
                Array.Copy(value.ToBytes(), 0, Data, 32, 4);
            }
        }
        Pointer CommandPointer
        {
            get
            {
                return new Pointer(Data.GetUInt32(36, true));
            }
            set
            {
                Array.Copy(value.ToBytes(), 0, Data, 36, 4);
            }
        }
        Pointer OAM
        {
            get
            {
                return new Pointer(Data.GetUInt32(48, true));
            }
            set
            {
                Array.Copy(value.ToBytes(), 0, Data, 48, 4);
            }
        }
        Pointer Frame_Previous
        {
            get
            {
                return new Pointer(Data.GetUInt32(52, true));
            }
            set
            {
                Array.Copy(value.ToBytes(), 0, Data, 52, 4);
            }
        }
        Pointer Frame_Next
        {
            get
            {
                return new Pointer(Data.GetUInt32(56, true));
            }
            set
            {
                Array.Copy(value.ToBytes(), 0, Data, 56, 4);
            }
        }
        Pointer OAM_Pointer
{
            get
            {
                return new Pointer(Data.GetUInt32(60, true));
            }
            set
            {
                Array.Copy(value.ToBytes(), 0, Data, 60, 4);
            }
        }

        public SpellAnimFrame(byte[] data)
        {
            if (data.Length != LENGTH) throw new Exception("Invalid frame AIS length.");

            Data = new byte[LENGTH];
            Array.Copy(data, Data, LENGTH);
        }
    }

    public class SpellAnimation
    {
        public bool Looped { get; set; }
        public Pointer Name { get; set; }
        public Pointer Address_Constructor { get; }
        public Pointer Address_LoopRoutine { get; }
        public Pointer Address_AnimLoading { get; }
        public byte[] Constructor { get; }
        public byte[] LoopRoutine { get; }
        public byte[] AnimLoading { get; }

        public SpellAnimation(Pointer address)
        {
            Address_Constructor = address - 1;
            Constructor = ReadASM(Address_Constructor);

            Pointer callback = Address_Constructor + Constructor.Length;
            if (callback % 4 != 0) callback += 2;

            Address_LoopRoutine = ReadCallback(Core.ReadPointer(callback)) - 1;
            LoopRoutine = ReadASM(Address_LoopRoutine);

            Address_AnimLoading = Address_LoopRoutine + LoopRoutine.Length;
            while (Core.ReadByte(Address_AnimLoading) == 0x00 && Core.ReadByte(Address_AnimLoading + 1) == 0x00)
            {
                Address_AnimLoading += 2;
            }
            AnimLoading = ReadASM(Address_AnimLoading);
        }

        byte[] ReadASM(Pointer address)
        {
            const int iterate = 2000;
            int length = 0;
            while (length < iterate)
            {
                if (Core.ReadByte(address + length) == 0x00 && Core.ReadByte(address + length + 1) == 0x47 || // bx r0;
                    Core.ReadByte(address + length) == 0x70 && Core.ReadByte(address + length + 1) == 0x47)   // bx lr;
                {
                    break;
                }
                else length += 2;
            }
            if (length == iterate)
                throw new Exception("No 'bx r0'(0x4700) found in ASM code to end subroutine.");
            else length += 2;

            return Core.ReadData(address, length);
        }
        Pointer ReadCallback(Pointer address)
        {
            byte[] callback = Core.ReadData(address, 16);

            switch (callback.GetInt32(0, true))
            {
                case 0x19: break;
                case 0x01: Name = new Pointer(callback.GetUInt32(4, true), false, true); break;
                case 0x03: Looped = true; break;
                default: throw new Exception("Unusual 6C callback:\n" + Util.BytesToHex(callback));
            }
            return new Pointer(callback.GetUInt32(12, true), false, true);
        }




        public ASM.Instruction[] GetASM_Constructor()
        {
            return ASM.Disassemble_Thumb(Constructor, Address_Constructor);
        }
        public ASM.Instruction[] GetASM_LoopRoutine()
        {
            return ASM.Disassemble_Thumb(LoopRoutine, Address_LoopRoutine);
        }
        public ASM.Instruction[] GetASM_AnimLoading()
        {
            return ASM.Disassemble_Thumb(AnimLoading, Address_AnimLoading);
        }

        public string[] GetAnimCode(SpellCommands commands)
        {
            List<string> result = new List<string>();

            result.Add("Constructor");
            result.AddRange(GetAnimCode(GetASM_Constructor(), commands));
            result.Add("");
            result.Add("LoopRoutine");
            result.AddRange(GetAnimCode(GetASM_LoopRoutine(), commands));
            result.Add("");
            result.Add("AnimLoading");
            result.AddRange(GetAnimCode(GetASM_AnimLoading(), commands));

            return result.ToArray();
        }
        List<string> GetAnimCode(ASM.Instruction[] routine, SpellCommands commands)
        {
            routine = GetCleanASM(routine);

            int count = 1;
            int indent = 0;
            string command;
            List<string> result = new List<string>();
            List<Tuple<Pointer, string>> labels = new List<Tuple<Pointer, string>>();
            for (int i = 0; i < routine.Length; i++)
            {
                command = commands.Get(routine, ref i);
                while (labels.Count > 0 && routine[i].Address >= labels[0].Item1)
                {
                    result.Add(labels[0].Item2);
                    labels.RemoveAt(0);
                }
                if (command == "}") indent--;
                if (command != null)
                {
                    if (command.StartsWith("goto_"))
                    {
                        Pointer address = Util.StringToAddress(command.Substring(command.IndexOf('(') + 1, 8));
                        string label = (address == routine[routine.Length - 3].Address) ? "return" : "label_" + count;
                        int index = 0;
                        while (index < labels.Count && labels[index].Item1 <= address)
                            index++;
                        if (labels.Count == 0)
                        {
                            count++;
                            labels.Add(Tuple.Create(address, label));
                            command = command.Substring(5, command.IndexOf('(') - 5) + ' ' + label;
                        }
                        else if (index == 0)
                        {
                            count++;
                            labels.Insert(0, Tuple.Create(address, label));
                            command = command.Substring(5, command.IndexOf('(') - 5) + ' ' + label;
                        }
                        else if (labels[index - 1].Item1 == address)
                        {
                            command = command.Substring(5, command.IndexOf('(') - 5) + ' ' + labels[index - 1].Item2;
                        }
                        else
                        {
                            count++;
                            labels.Insert(index, Tuple.Create(address, label));
                            command = command.Substring(5, command.IndexOf('(') - 5) + ' ' + label;
                        }
                    }
                    result.Add(new string(' ', indent * 4) + command);
                }
                else result.Add(new string(' ', indent * 4) + routine[i].Code);
                if (command == "{") indent++;
            }
            return result;
        }
        /// <summary>
        /// This function removes garbage instructions (hardcoded pointers) from the given routine, as well as "nop"s
        /// </summary>
        ASM.Instruction[] GetCleanASM(ASM.Instruction[] routine)
        {
            for (int i = 0; i < routine.Length; i++)
            {
                for (int j = 0; j < routine[i].Code.Length; j++)
                {
                    if (routine[i].Code[j] == '[' &&
                        routine[i].Code[j + 1] == 'p' &&
                        routine[i].Code[j + 2] == 'c')
                    {
                        j += 8;
                        ushort offset = (routine[i].Code[j + 2] == ']') ?
                            Util.HexToByte(routine[i].Code.Substring(j, 2)) :
                            (UInt16)Util.HexToInt(routine[i].Code.Substring(j, 4));
                        j += (routine[i].Code[j + 7] == '=') ?  9 : 7;
                        uint address = Util.HexToInt(routine[i].Code.Substring(j, 8));
                        uint pc = (routine[i].Address & 0xFFFFFFFC) + 4;
                        byte n = 1;
                        while (routine[i + n].Address < pc + offset)
                        {
                            n++;
                            if (i + n >= routine.Length)
                                goto Continue;
                        }
                        if (i + n + 1 >= routine.Length)
                            goto Continue;
                        if (routine[i + n + 1].Data[0] == (byte)((address & 0xFF000000) >> 24) &&
                            routine[i + n + 1].Data[1] == (byte)((address & 0x00FF0000) >> 16) &&
                            routine[i + n + 0].Data[0] == (byte)((address & 0x0000FF00) >> 8) &&
                            routine[i + n + 0].Data[1] == (byte) (address & 0x000000FF))
                        {
                            /*
                            string code = routine[i].Code;
                            code = code.Substring(0, code.IndexOf('[')) + code.Substring(code.IndexOf('$'), 9);
                            routine[i].Code = code;
                            */
                            routine[i + n].Code = "nop";
                            routine[i + n + 1].Code = "nop";
                        }
                        break;
                    }
                }
                Continue: continue;
            }
            List<ASM.Instruction> result = new List<ASM.Instruction>();
            for (int i = 0; i < routine.Length; i++)
            {
                if (routine[i].Code == "nop")
                    continue;
                else result.Add(routine[i]);
            }
            return result.ToArray();
        }
    }
}