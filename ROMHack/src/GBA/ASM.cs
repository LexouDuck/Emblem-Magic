using Magic;
using System;
using System.Collections.Generic;

namespace GBA
{
    /// <summary>
    /// Holds everything for dissassembling GBA machine code, and reading GBA ASM
    /// </summary>
    public static class ASM
    {
        /// <summary>
        /// Simulates the functioning of a GBA CPU.
        /// </summary>
        public class RegisterSet
        {
            public UInt32 this[Int32 index]
            {
                get
                {
                    switch (index)
                    {
                        case 0:  return R0;
                        case 1:  return R1;
                        case 2:  return R2;
                        case 3:  return R3;
                        case 4:  return R4;
                        case 5:  return R5;
                        case 6:  return R6;
                        case 7:  return R7;
                        case 8:  return R8;
                        case 9:  return R9;
                        case 10: return R10;
                        case 11: return R11;
                        case 12: return R12;
                        case 13: return SP;
                        case 14: return LR;
                        case 15: return PC;
                    }
                    throw new ArgumentException("invalid index given for register set: " + index);
                }
                set
                {
                    switch (index)
                    {
                        case 0:  R0 = value;  return;
                        case 1:  R1 = value;  return;
                        case 2:  R2 = value;  return;
                        case 3:  R3 = value;  return;
                        case 4:  R4 = value;  return;
                        case 5:  R5 = value;  return;
                        case 6:  R6 = value;  return;
                        case 7:  R7 = value;  return;
                        case 8:  R8 = value;  return;
                        case 9:  R9 = value;  return;
                        case 10: R10 = value; return;
                        case 11: R11 = value; return;
                        case 12: R12 = value; return;
                        case 13: SP = value;  return;
                        case 14: LR = value;  return;
                        case 15: PC = value;  return;
                    }
                    throw new ArgumentException("invalid index given for register set: " + index);
                }
            }

            public UInt32 R0;  // GPR (Lo) - General Purpose Registers
            public UInt32 R1;  // GPR (Lo)
            public UInt32 R2;  // GPR (Lo)
            public UInt32 R3;  // GPR (Lo)
            public UInt32 R4;  // GPR (Lo)
            public UInt32 R5;  // GPR (Lo)
            public UInt32 R6;  // GPR (Lo)
            public UInt32 R7;  // GPR (Lo)
            public UInt32 R8;  // GPR (Hi) - Hi registers can only be freely accessed in ARM state
            public UInt32 R9;  // GPR (Hi)
            public UInt32 R10; // GPR (Hi)
            public UInt32 R11; // GPR (Hi)
            public UInt32 R12; // GPR (Hi)
            public UInt32 SP;  // Stack Pointer: each exception handler must use its own stack (is a GPR in ARM state)
            public UInt32 LR;  // Link Register: stores the address to return to after executing a BL subroutine
            public UInt32 PC;  // Program Counter: the address of the code currently being read (+n, depending on instruction)
            
            public Boolean N; // N - Sign Flag(0=Not Signed, 1=Signed)
            public Boolean Z; // Z - Zero Flag(0=Not Zero, 1=Zero)
            public Boolean C; // C - Carry Flag(0=Borrow/No Carry, 1=Carry/No Borrow)
            public Boolean V; // V - Overflow Flag(0=No Overflow, 1=Overflow)
            
            public Boolean I; // I - IRQ disable(0=Enable, 1=Disable)
            public Boolean F; // F - FIQ disable(0=Enable, 1=Disable)
            public Boolean T; // T - State Bit(0=ARM, 1=THUMB)
            public Byte M; // M4-M0 - Mode Bits(See below)

            public List<UInt32> Stack;

            public RegisterSet(Pointer pc, Boolean thumb)
            {
                R0 = 0; R8  = 0;
                R1 = 0; R9  = 0;
                R2 = 0; R10 = 0;
                R3 = 0; R11 = 0;
                R4 = 0; R12 = 0;
                R5 = 0;  SP = 0;
                R6 = 0;  LR = 0;
                R7 = 0;  PC = pc.Address + Pointer.HardwareOffset;
                
                N = false; I = false;
                Z = false; F = false;
                C = false; T = thumb;
                V = false; M = 0x00;
                
                Stack = new List<UInt32>();
            }
        }

        public class Instruction
        {
            public Pointer Address;
            public Byte[] Data;
            public String Code;

            public Instruction(Pointer address, Byte[] data, String asmCode)
            {
                this.Address = address;
                this.Data = data;
                this.Code = asmCode;
            }

            override public String ToString()
            {
                return Address + " | " + Util.BytesToHex(Data) + (Data.Length == 2 ? "     | " : " | ") + Code;
            }

            public RegisterSet Read(RegisterSet cpu)
            {
                Int32 index = 0;
                String opcode = "";
                while (Code[index] != ' ')
                {
                    opcode += Code[index++];
                }
                
                cpu.PC += 2;

                if (opcode.Equals("B", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 i = 0;
                    while (Code[i] != '$') i++;
                    i++;
                    return B(cpu, new Pointer(Util.HexToInt(Code.Substring(i, 8))), "");
                }
                if (opcode.Equals("BL", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 i = 0;
                    while (Code[i] != '$') i++;
                    i++;
                    return BL(cpu, new Pointer(Util.HexToInt(Code.Substring(i, 8))));
                }
                if (opcode.Equals("BLH", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 i = 0;
                    while (Code[i] != '$') i++;
                    i++;
                    return BLH(cpu, new Pointer(Util.HexToInt(Code.Substring(i, 8))));
                }
                if (opcode.Equals("BX", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        Int32 i = 0;
                        while (Code[i] != '$') i++;
                        i++;
                        return BL(cpu, new Pointer(Util.HexToInt(Code.Substring(i, 8))));
                    }
                    catch
                    {
                        Int32 Rs = Read_Register(ref index);
                        return BX(cpu, Rs);
                    }
                }
                if (opcode.StartsWith("B", StringComparison.OrdinalIgnoreCase) && opcode.Length == 3)
                {
                    Int32 i = 0;
                    while (Code[i] != '$') i++;
                    i++;
                    return B(cpu, new Pointer(Util.HexToInt(Code.Substring(i, 8))), opcode.Substring(1, 2));
                }

                if (opcode.Equals("PUSH", StringComparison.OrdinalIgnoreCase))
                {
                    Int32[] Rlist = Read_RegisterList(ref index);
                    return PUSH(cpu, Rlist);
                }
                if (opcode.Equals("POP", StringComparison.OrdinalIgnoreCase))
                {
                    Int32[] Rlist = Read_RegisterList(ref index);
                    return POP(cpu, Rlist);
                }

                if (opcode.Equals("MOV", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Boolean immediate = false;
                    for (Int32 i = 0; i < Code.Length; i++)
                    {
                        if (Code[i] == '#') immediate = true;
                    }
                    if (immediate)
                    {
                        Byte Imm = (Byte)Read_Immediate(ref index);
                        return MOV(cpu, Rd, Imm);
                    }
                    else
                    {
                        Int32 Rs = Read_Register(ref index);
                        return MOV(cpu, Rd, Rs);
                    }
                }
                if (opcode.Equals("MVN", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return MVN(cpu, Rd, Rs);
                }
                if (opcode.Equals("AND", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return AND(cpu, Rd, Rs);
                }
                if (opcode.Equals("TST", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return TST(cpu, Rd, Rs);
                }
                if (opcode.Equals("BIC", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return BIC(cpu, Rd, Rs);
                }
                if (opcode.Equals("ORR", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return ORR(cpu, Rd, Rs);
                }
                if (opcode.Equals("EOR", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return EOR(cpu, Rd, Rs);
                }
                if (opcode.Equals("LSL", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    Boolean immediate = false;
                    for (Int32 i = 0; i < Code.Length; i++)
                    {
                        if (Code[i] == '#') immediate = true;
                    }
                    if (immediate)
                    {
                        Byte Imm = (Byte)Read_Immediate(ref index);
                        return LSL(cpu, Rd, Rs, Imm);
                    }
                    else return LSL(cpu, Rd, Rs);
                }
                if (opcode.Equals("LSR", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    Boolean immediate = false;
                    for (Int32 i = 0; i < Code.Length; i++)
                    {
                        if (Code[i] == '#') immediate = true;
                    }
                    if (immediate)
                    {
                        Byte Imm = (Byte)Read_Immediate(ref index);
                        return LSR(cpu, Rd, Rs, Imm);
                    }
                    else return LSR(cpu, Rd, Rs);
                }
                if (opcode.Equals("ASR", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    Boolean immediate = false;
                    for (Int32 i = 0; i < Code.Length; i++)
                    {
                        if (Code[i] == '#') immediate = true;
                    }
                    if (immediate)
                    {
                        Byte Imm = (Byte)Read_Immediate(ref index);
                        return ASR(cpu, Rd, Rs, Imm);
                    }
                    else return ASR(cpu, Rd, Rs);
                }
                if (opcode.Equals("ROR", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return ROR(cpu, Rd, Rs);
                }
                if (opcode.Equals("NOP", StringComparison.OrdinalIgnoreCase))
                {
                    return cpu;
                }


                if (opcode.Equals("ADC", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return ADC(cpu, Rd, Rs);
                }
                if (opcode.Equals("SBC", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    return SBC(cpu, Rd, Rs);
                }

                if (opcode.StartsWith("LDR", StringComparison.OrdinalIgnoreCase))
                {
                    Int32 Rd = Read_Register(ref index);
                    Int32 Rs = Read_Register(ref index);
                    Boolean immediate = false;
                    for (Int32 i = 0; i < Code.Length; i++)
                    {
                        if (Code[i] == '#') immediate = true;
                    }
                    if (immediate)
                    {
                        Byte Imm = (Byte)Read_Immediate(ref index);
                        return LDR(cpu, Rd, Rs, Imm);
                    }
                    else
                    {
                        Int32 Ro = Read_Register(ref index);
                        return LDR(cpu, Rd, Rs, Ro);
                    }
                }

                return cpu;
            }

            private Int32 Read_Register(ref Int32 index)
            {
                while (index < Code.Length)
                {
                    if (Code[index] == 'r' || Code[index] == 'R')
                    {
                        index++;
                        break;
                    }
                    else if (Code.Substring(index, 2).Equals("SP", StringComparison.OrdinalIgnoreCase)) { index += 2; return 13; }
                    else if (Code.Substring(index, 2).Equals("LR", StringComparison.OrdinalIgnoreCase)) { index += 2; return 14; }
                    else if (Code.Substring(index, 2).Equals("PC", StringComparison.OrdinalIgnoreCase)) { index += 2; return 15; }
                    else index++;
                }
                Int32 length = 1;
                while (index + length < Code.Length)
                {
                    if (Code[index + length] == ' '
                     || Code[index + length] == ','
                     || Code[index + length] == '-'
                     || Code[index + length] == '}'
                     || Code[index + length] == ']') break;
                    else length++;
                }
                String result = Code.Substring(index, length);
                index += length;
                return Int32.Parse(result);
            }
            private Int32 Read_Immediate(ref Int32 index)
            {
                while (index < Code.Length)
                {
                    if (Code[index] == '#')
                        break;
                    else index++;
                }
                index++;
                Int32 length = 0;
                while (index + length < Code.Length)
                {
                    if (Code[index + length] == ' '
                     || Code[index + length] == ','
                     || Code[index + length] == '-'
                     || Code[index + length] == '}'
                     || Code[index + length] == ']') break;
                    else length++;
                }
                String immediate = Code.Substring(index, length);
                index += length;
                if (immediate.StartsWith("0x"))
                {
                    return (Int32)Util.HexToInt(immediate.Substring(2));
                }
                return Int32.Parse(immediate);
            }
            private Int32[] Read_RegisterList(ref Int32 index)
            {
                List<Int32> result = new List<Int32>();
                while (Code[index] != '{') index++;
                result.Add(Read_Register(ref index));
                while (Code[index] == ' ') index++;

                if (Code[index] == '}') goto End;
                else
                {
                    if (Code[index] == '-')
                    {
                        Int32 list_end = Read_Register(ref index);
                        for (Int32 i = result[0]; i <= list_end; i++)
                        {
                            result.Add(i);
                        }
                    }
                    else if (Code[index] == ',')
                    {
                        result.Add(Read_Register(ref index));
                    }
                    
                    while (Code[index] == ' ') index++;

                    if (Code[index] == '}') goto End;
                    if (Code[index] == ',')
                    {
                        result.Add(Read_Register(ref index));
                    }
                }
                End: return result.ToArray();
            }

            RegisterSet MOV(RegisterSet cpu, Int32 Rd, Byte Imm)
            {
                cpu[Rd] = Imm;

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet MOV(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                cpu[Rd] = cpu[Rs];

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                cpu.C = false;
                cpu.V = false;

                return cpu;
            }
            RegisterSet MVN(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                cpu[Rd] = ~cpu[Rs];

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet AND(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                cpu[Rd] &= cpu[Rs];

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet TST(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                UInt32 test = (cpu[Rd] & cpu[Rs]);

                cpu.N = (test < 0);
                cpu.Z = (test == 0);

                return cpu;
            }
            RegisterSet BIC(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                cpu[Rd] &= ~cpu[Rs];

                cpu.Z = (cpu[Rd] == 0);
                cpu.N = (cpu[Rd] < 0);

                return cpu;
            }
            RegisterSet ORR(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                cpu[Rd] |= cpu[Rs];

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet EOR(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                cpu[Rd] ^= cpu[Rs];

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet LSL(RegisterSet cpu, Int32 Rd, Int32 Rs, Byte Imm)
            {
                cpu[Rd] = cpu[Rs] << Imm;

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                if (Imm != 0)
                    cpu.C = (cpu[Rs] & (0x1 << (32 - Imm))) != 0;

                return cpu;
            }
            RegisterSet LSL(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                Int32 shift = (Int32)(cpu[Rs] & 0xFF);
                cpu[Rd] = (UInt32)((Int32)cpu[Rd] << shift);

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                if (shift != 0)
                    cpu.C = (cpu[Rs] & (0x1 << (32 - shift))) != 0;

                return cpu;
            }
            RegisterSet LSR(RegisterSet cpu, Int32 Rd, Int32 Rs, Byte Imm)
            {
                cpu[Rd] = cpu[Rs] >> Imm;

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                if (Imm != 0)
                    cpu.C = (cpu[Rs] & (0x1 << (Imm - 1))) != 0;

                return cpu;
            }
            RegisterSet LSR(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                Int32 shift = (Int32)(cpu[Rs] & 0xFF);
                cpu[Rd] = cpu[Rd] >> shift;

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                if (shift != 0)
                    cpu.C = (cpu[Rs] & (0x1 << (shift - 1))) != 0;

                return cpu;
            }
            RegisterSet ASR(RegisterSet cpu, Int32 Rd, Int32 Rs, Byte Imm)
            {
                cpu[Rd] = (UInt32)((Int32)cpu[Rs] >> Imm);

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                if (Imm != 0)
                    cpu.C = (cpu[Rs] & (0x1 << (Imm - 1))) != 0;

                return cpu;
            }
            RegisterSet ASR(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                Int32 shift = (Int32)(cpu[Rs] & 0xFF);
                cpu[Rd] = (UInt32)((Int32)cpu[Rd] >> shift);

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                if (shift != 0)
                    cpu.C = (cpu[Rs] & (0x1 << (shift - 1))) != 0;

                return cpu;
            }
            RegisterSet ROR(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                Int32 rotate = (Int32)(cpu[Rs] & 0xFF);
                cpu[Rd] = (cpu[Rd] >> rotate) | (cpu[Rd] << (32 - rotate));

                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                if (rotate != 0)
                    cpu.C = (cpu[Rs] & (0x1 << (rotate - 1))) != 0;

                return cpu;
            }

            RegisterSet ADD(RegisterSet cpu, Int32 Rd, Int32 Rs, Byte Imm)
            {
                try
                {
                    cpu[Rd] = checked(cpu[Rs] + Imm);

                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);
                
                return cpu;
            }
            RegisterSet ADD(RegisterSet cpu, Int32 Rd, Byte Imm)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] += Imm;
                    }
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet ADD(RegisterSet cpu, Int32 Rd, Int32 Rs, Int32 Rn)
            {
                try
                {
                    cpu[Rd] = checked(cpu[Rs] + cpu[Rn]);

                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet ADD(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] += cpu[Rs];
                    }
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet ADC(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] += cpu[Rs];
                        if (cpu.C) cpu[Rd]++;
                    }
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet SUB(RegisterSet cpu, Int32 Rd, Int32 Rs, Byte Imm)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] = cpu[Rs] - Imm;
                    }
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet SUB(RegisterSet cpu, Int32 Rd, Byte Imm)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] -= Imm;
                    }
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet SUB(RegisterSet cpu, Int32 Rd, Int32 Rs, Int32 Rn)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] = cpu[Rs] - cpu[Rn];
                    }
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet SBC(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] -= cpu[Rs];
                        if (!cpu.C) cpu[Rd]--;
                    }
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet NEG(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] = 0 - cpu[Rs];
                    }
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }
            RegisterSet CMP(RegisterSet cpu, Int32 Rd, Byte Imm)
            {
                Int32 result;
                try
                {
                    result = checked((Int32)(cpu[Rd] - Imm));
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    result = (Int32)((Int64)cpu[Rd] - (Int64)Imm);
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (result < 0);
                cpu.Z = (result == 0);

                return cpu;
            }
            RegisterSet CMP(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                Int32 result;
                try
                {
                    result = checked ((Int32)(cpu[Rd] - cpu[Rs]));
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    result = (Int32)((Int64)cpu[Rd] - (Int64)cpu[Rs]);
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (result < 0);
                cpu.Z = (result == 0);

                return cpu;
            }
            RegisterSet CMN(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                Int32 result;
                try
                {
                    result = checked((Int32)(cpu[Rd] + cpu[Rs]));
                    cpu.C = false;
                    cpu.V = false;
                }
                catch
                {
                    result = (Int32)((Int64)cpu[Rd] + (Int64)cpu[Rs]);
                    cpu.C = true;
                    cpu.V = true;
                }
                cpu.N = (result < 0);
                cpu.Z = (result == 0);

                return cpu;
            }
            RegisterSet MUL(RegisterSet cpu, Int32 Rd, Int32 Rs)
            {
                try
                {
                    checked
                    {
                        cpu[Rd] *= cpu[Rs];
                    }
                    cpu.C = false;
                }
                catch
                {
                    cpu.C = true;
                }
                cpu.N = (cpu[Rd] < 0);
                cpu.Z = (cpu[Rd] == 0);

                return cpu;
            }

            RegisterSet B(RegisterSet cpu, Pointer address, String condition)
            {
                Boolean result;
                switch (condition)
                {
                    case "eq": result = (cpu.Z == true); break;
                    case "ne": result = (cpu.Z == false); break;
                    case "cs": result = (cpu.C == true); break;
                    case "cc": result = (cpu.C == false); break;
                    case "hs": result = (cpu.C == true); break;
                    case "lo": result = (cpu.C == false); break;
                    case "mi": result = (cpu.N == true); break;
                    case "pl": result = (cpu.N == false); break;
                    case "vs": result = (cpu.V == true); break;
                    case "vc": result = (cpu.V == false); break;
                    case "hi": result = (cpu.C == true && cpu.Z == false); break;
                    case "ls": result = (cpu.C == false || cpu.Z == true); break;
                    case "ge": result = (cpu.N == cpu.V); break;
                    case "lt": result = (cpu.N != cpu.V); break;
                    case "gt": result = (cpu.Z == false && cpu.N == cpu.V); break;
                    case "le": result = (cpu.Z == true || cpu.N != cpu.V); break;
                    case "al": result = true; break;
                    case "nv": result = false; break;
                    case "": result = true; break;
                    default: throw new Exception("Invalid branch condition read.");
                }
                if (result) cpu.PC = address.Address;

                return cpu;
            }
            RegisterSet BL(RegisterSet cpu, Pointer address)
            {
                if (Data.Length == 4)
                     cpu.LR = cpu.PC + 2;
                else cpu.LR = cpu.PC;
                cpu.PC = address.Address;

                return cpu;
            }
            RegisterSet BLH(RegisterSet cpu, Pointer address)
            {
                cpu.LR = cpu.PC;
                cpu.PC = address.Address;

                return cpu;
            }
            RegisterSet BX(RegisterSet cpu, Int32 Rs)
            {
                cpu.PC = cpu[Rs] & 0xFFFFFFFE;
                cpu.T = (cpu[Rs] & 0x1) != 0;

                return cpu;
            }

            RegisterSet PUSH(RegisterSet cpu, Int32[] Rlist)
            {
                for (Int32 i = 0; i < Rlist.Length; i++)
                {
                    cpu.Stack.Insert(0, cpu[Rlist[i]]);
                }
                return cpu;
            }
            RegisterSet POP(RegisterSet cpu, Int32[] Rlist)
            {
                for (Int32 i = 0; i < Rlist.Length; i++)
                {
                    cpu[Rlist[i]] = cpu.Stack[0];
                    cpu.Stack.RemoveAt(0);
                }
                return cpu;
            }

            RegisterSet LDR(RegisterSet cpu, Int32 Rd, Int32 Rb, Byte Imm)
            {
                Pointer address = new Pointer((Rb == 15) ?
                   (cpu[Rb] & 0xFFFFFFFC) + 4 + Imm : // PC-relative ldr has PC+4 rather than PC+2
                    cpu[Rb] + Imm,
                    false, true);
                cpu[Rd] = Util.BytesToUInt32(Core.ReadData(address, 4), true);

                return cpu;
            }
            RegisterSet LDR(RegisterSet cpu, Int32 Rd, Int32 Rb, Int32 Ro)
            {
                cpu[Rd] = Util.BytesToUInt32(Core.ReadData(new Pointer(cpu[Rb] + cpu[Ro], false, true), 4), true);

                return cpu;
            }
            RegisterSet LDRB(RegisterSet cpu, Int32 Rd, Int32 Rb, Byte Imm)
            {
                cpu[Rd] = Core.ReadByte(new Pointer(cpu[Rb] + Imm, false, true));

                return cpu;
            }
            RegisterSet LDRB(RegisterSet cpu, Int32 Rd, Int32 Rb, Int32 Ro)
            {
                cpu[Rd] = Core.ReadByte(new Pointer(cpu[Rb] + cpu[Ro], false, true));

                return cpu;
            }
            RegisterSet LDRH(RegisterSet cpu, Int32 Rd, Int32 Rb, Byte Imm)
            {
                cpu[Rd] = Util.BytesToUInt16(Core.ReadData(new Pointer(cpu[Rb] + (UInt32)(Imm << 1), false, true), 2), true);

                return cpu;
            }
            RegisterSet LDRH(RegisterSet cpu, Int32 Rd, Int32 Rb, Int32 Ro)
            {
                cpu[Rd] = Util.BytesToUInt16(Core.ReadData(new Pointer(cpu[Rb] + cpu[Ro], false, true), 2), true);

                return cpu;
            }
            RegisterSet LDSB(RegisterSet cpu, Int32 Rd, Int32 Rb, Int32 Ro)
            {
                cpu[Rd] = Core.ReadByte(new Pointer(cpu[Rb] + cpu[Ro], false, true));

                return cpu;
            }
            RegisterSet LDSH(RegisterSet cpu, Int32 Rd, Int32 Rb, Int32 Ro)
            {
                cpu[Rd] = Util.BytesToUInt16(Core.ReadData(new Pointer(cpu[Rb] + cpu[Ro], false, true), 2), true);

                return cpu;
            }
        }

        /// <summary>
        /// The 16 registers of the GBA processor
        /// </summary>
        static String[] Registers = new String[16]
        {
            "r0",  // GPR (Lo) - General Purpose Registers
            "r1",  // GPR (Lo)
            "r2",  // GPR (Lo)
            "r3",  // GPR (Lo)
            "r4",  // GPR (Lo)
            "r5",  // GPR (Lo)
            "r6",  // GPR (Lo)
            "r7",  // GPR (Lo)
            "r8",  // GPR (Hi) - Hi registers can only be freely accessed in ARM state
            "r9",  // GPR (Hi)
            "r10", // GPR (Hi)
            "r11", // GPR (Hi)
            "r12", // GPR (Hi)
            "sp",  // Stack Pointer: each exception handler must use its own stack (is a GPR in ARM state)
            "lr",  // Link Register: stores the address to return to after executing a BL subroutine
            "pc"   // Program Counter: the address of the code currently being read (+n, depending on instruction)
        };

        /// <summary>
        /// These conditions can be added as suffixes to certain instructions
        /// </summary>
        static String[] Conditions = new String[16]
        {
            "eq", // 0:   EQ    Z = 1           EQual (zero) (same)
            "ne", // 1:   NE    Z = 0           Not Equal (nonzero) (not same)
            "cs", // 2:   CS/HS C = 1           (Carry Set) unsigned higher or same
            "cc", // 3:   CC/LO C = 0           (Carry Cleared) unsigned lower
            "mi", // 4:   MI    N = 1           (MInus) negative
            "pl", // 5:   PL    N = 0           (PLus) positive or zero
            "vs", // 6:   VS    V = 1           (V Set) overflow
            "vc", // 7:   VC    V = 0           (V Cleared) no overflow
            "hi", // 8:   HI    C = 1 & Z = 0   unsigned HIgher
            "ls", // 9:   LS    C = 0 or Z = 1  unsigned Lower or Same
            "ge", // A:   GE    N = V           Greater or Equal
            "lt", // B:   LT    N<>V            Less Than
            "gt", // C:   GT    Z = 0 & N = V   Greater Than
            "le", // D:   LE    Z = 1 or N<>V   Less or Equal
            "",   // E:   AL      -             ALways (the "AL" suffix can be omitted)
            "nv"  // F:   NV      -             NeVer(ARMv1, v2 only) (Reserved ARMv3 and up)
        };

        static String[] ALU_Shifts = new String[5]
        {
            "lsl", // Logical Shift Left
            "lsr", // Logical Shift Right
            "asr", // Arithmetic Shift Right
            "ror", // ROtate Right
            "rrx"  // 
        };

        static String[] ARM_AddressingModes = new String[8]
        {
            // non-stack
            "da", // decrement after
            "ia", // increment after
            "db", // decrement before
            "ib", // increment before
            // stack transfers
            "ed", // empty stack, descending
            "ea", // empty stack, ascending
            "fd", // full stack, descending
            "fa"  // full stack, ascending
        };

        struct Opcode
        {
            public UInt32 BitMask;
            public UInt32 CodeValue;
            public String Format;

            public Opcode(UInt32 bitmask, UInt32 instruction, String formatstring)
            {
                this.BitMask = bitmask;
                this.CodeValue = instruction;
                this.Format = formatstring;
            }
        };



        /// <summary>
        /// Format codes :
        /// %c - indicates a condition suffix to an instruction
        /// %r - indicates a register, the following number being said register's bit index in the opcode
        /// %i - indicates an immediate value, the following numbers being the bit index, the bit length, and shift amount
        /// %o - indicates an immediate signed offset
        /// %q - indicates a SoftWareInterrupt or BreaKPoinT comment field
        /// %n - indicates the second operand for ALU operations
        /// %s - indicates an S flag - whether or not the instruction affects the CPSR
        /// %u - indicates a 'U' (unsigned) or 'S' (signed) instruction prefix
        /// %A - indicates an address for LDR or STR (accordingly Pre-indexed or Post-indexed)
        /// </summary>
        static Opcode[] Opcodes_ARM = new Opcode[]
        {
            // Undefined
            new Opcode(0x0E000010, 0x06000010, "[ undefined ]"),
            // Branch instructions
            new Opcode(0x0F000000, 0x0A000000, "b%c %o(0,24,2)"),
            new Opcode(0x0F000000, 0x0B000000, "bl%c %o(0,24,2)"),
            new Opcode(0x0FFFFFF0, 0x012FFF10, "bx%c %r(0)"),
            new Opcode(0x0FFFFFF0, 0x012FFF30, "blx%c %r(0)"),
            new Opcode(0x0F000000, 0x0F000000, "swi%c %q"),
            new Opcode(0xFF000000, 0xE1000000, "bkpt %q"),
            // Data processing (ALU)
            new Opcode(0x0DE00000, 0x00000000, "and%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x00200000, "eor%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x00400000, "sub%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x00600000, "rsb%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x00800000, "add%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x00A00000, "adc%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x00C00000, "sbc%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x00E00000, "rsc%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x01000000, "tst%c %r(16), %n"),
            new Opcode(0x0DE00000, 0x01200000, "teq%c %r(16), %n"),
            new Opcode(0x0DE00000, 0x01400000, "cmp%c %r(16), %n"),
            new Opcode(0x0DE00000, 0x01600000, "cmn%c %r(16), %n"),
            new Opcode(0x0DE00000, 0x01800000, "orr%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x01A00000, "mov%c%s %r(12), %n"),
            new Opcode(0x0DE00000, 0x01C00000, "bic%c%s %r(12), %r(16), %n"),
            new Opcode(0x0DE00000, 0x01E00000, "mvn%c%s %r(12), %n"),
            // Multiply and Multiply-Accumulate instructions
            new Opcode(0x0FE000F0, 0x00000090, "mul%c%s %r(16), %r(0), %r(8)"),
            new Opcode(0x0FE000F0, 0x00200090, "mla%c%s %r(16), %r(0), %r(8), %r(12)"),
            new Opcode(0x0FA000F0, 0x00800090, "%umull%c%s %r(12), %r(16), %r(0), %r(8)"),
            new Opcode(0x0FA000F0, 0x00A00090, "%umlal%c%s %r(12), %r(16), %r(0), %r(8)"),
            // PSR transfer
            new Opcode(0x0FBF0FFF, 0x010F0000, "mrs%c %r(12), %x"),
            new Opcode(0x0FB0F000, 0x0120F000, "msr%c %X, %r(0)"),
            new Opcode(0x0FB0FFF0, 0x0160F000, "msr%c %X, [%i(0,8,0), lsl %i(8,4,1)]"),
            // Load/Store instructions
            new Opcode(0x0FB00FF0, 0x01000090, "swp%c%b(22) %r(12), %r(0), [%r(16)]"),
            new Opcode(0x0FB000F0, 0x01000090, "[ ??? ]"),
            new Opcode(0x0C100000, 0x04000000, "str%c%b(22)%t %r(12), %a"),
            new Opcode(0x0C100000, 0x04100000, "ldr%c%b(22)%t %r(12), %a"),
            // Halfword, Doubleword, and signed load/store
            new Opcode(0x0E5000F0, 0x000000B0, "str%ch %r(12), %A"),
            new Opcode(0x0E5000F0, 0x000000D0, "ldr%cd %r(12), %A"),
            new Opcode(0x0E5000F0, 0x000000F0, "str%cd %r(12), %A"),
            new Opcode(0x0E5000F0, 0x001000B0, "ldr%ch %r(12), %A"),
            new Opcode(0x0E5000F0, 0x001000D0, "ldr%csb %r(12), %A"),
            new Opcode(0x0E5000F0, 0x001000F0, "ldr%csh %r(12), %A"),
            // Multiple load/store instructions
            new Opcode(0x0E100000, 0x08000000, "stm%c%m %r(16)%w, %l%U"),
            new Opcode(0x0E100000, 0x08100000, "ldm%c%m %r(16)%w, %l%U"),
            // Coprocessor instructions
            new Opcode(0x0F100010, 0x0E000010, "mcr%c %p, %O, %r(12), %C(16), %C(0)"),
            new Opcode(0x0F100010, 0x0E100010, "mrc%c %p, %O, %r(12), %C(16), %C(0)"),
            new Opcode(0x0F000010, 0x0E000000, "cdp%c %p, %O, %C(12), %C(16), %C(0)"),
            new Opcode(0x0E100000, 0x0C000000, "stc%c%L %p, %C(12), %o(0,8,2)"),
            new Opcode(0x0E100000, 0x0C000000, "stc%c%L %p, %C(12), %o(0,8,2)"),
            // Unknown
            new Opcode(0x00000000, 0x00000000, "[ ??? ]")
        };
        
        public static Instruction[] Disassemble_ARM(Byte[] data, Pointer pc)
        {
            List<Instruction> result = new List<Instruction>();

            String line;
            for (UInt32 i = 0; i < data.Length; i += 4)
            {
                line = "";
                UInt32 opcode = data.GetUInt32(i, true);

                Int32 index = 0;
                while ((Opcodes_ARM[index].BitMask & opcode) != Opcodes_ARM[index].CodeValue)
                    index++;

                Int32 j = 0;
                Int32[] args;
                while (j < Opcodes_ARM[index].Format.Length)
                {
                    if (Opcodes_ARM[index].Format[j] != '%')
                    {
                        line += Opcodes_ARM[index].Format[j++];
                    }
                    else
                    {
                        j++;

                        switch (Opcodes_ARM[index].Format[j])
                        {
                            case 'r': // register
                                j++; args = ReadFormatCodeArgs(Opcodes_ARM[index].Format, ref j);
                                {
                                    line += Registers[(opcode >> args[0]) & 0xF];
                                } break;

                            case 'C': // coprocessor register
                                j++; args = ReadFormatCodeArgs(Opcodes_ARM[index].Format, ref j);
                                {
                                    line += Registers[(opcode >> args[0]) & 0xF];
                                } break;

                            case 'p': // coprocessor number
                                j++;
                                {
                                    line += "p" + ((opcode >> 8) & 0xF);
                                }
                                break;

                            case 'i': // immediate value
                                j++; args = ReadFormatCodeArgs(Opcodes_ARM[index].Format, ref j);
                                {
                                    UInt32 val = (UInt32)(((opcode >> args[0]) & GetBitMask(args[1])) << args[2]);
                                    line += "#";
                                    if (val < 256) line += "0x" + Util.ByteToHex((Byte)val);
                                    else line += "0x" + Util.UInt16ToHex((UInt16)val);
                                }
                                break;

                            case 'o': // offset
                                j++; args = ReadFormatCodeArgs(Opcodes_ARM[index].Format, ref j);
                                {
                                    Pointer address;
                                    address = new Pointer((((pc + i) & 0xFFFFFFFC) + 4));
                                    address += (Int32)(((opcode >> args[0]) & GetBitMask(args[1])) << args[2]);
                                    Byte[] pointer = new Byte[4];
                                    pointer = Core.ReadData(address, 4);
                                    Array.Reverse(pointer);
                                    line += "(=$" + Util.BytesToHex(pointer) + ")";
                                }
                                break;



                            case 'c': // Condition
                                j++;
                                {
                                    line += Conditions[(opcode >> 28) & 0xF];
                                } break;

                            case 's': // Set CPSR flag
                                j++;
                                {
                                    line += 's';
                                } break;

                            case 't': // Force user mode flag
                                j++;
                                {
                                    if (((opcode & (0x1 << 24)) == 0) && ((opcode & (0x1 << 21)) != 0))
                                        line += 't';
                                } break;

                            case 'u': // unsigned/signed flag
                                j++;
                                {
                                    line += (((opcode >> 22) & 0x1) == 1) ? 's' : 'u';
                                } break;

                            case 'b': // Byte suffix flag
                                j++; args = ReadFormatCodeArgs(Opcodes_ARM[index].Format, ref j);
                                {
                                    if ((opcode & (0x1 << args[0])) != 0)
                                        line += "b";
                                } break;

                            case 'm': // Addressing mode
                                j++;
                                {
                                    line += ARM_AddressingModes[(opcode >> 23) & 0x3];
                                }
                                break;

                            case 'w': // Write-back flag
                                j++;
                                {
                                    if ((opcode & (0x1 << 21)) != 0)
                                        line += '!';
                                }
                                break;

                            case 'U': // PSR / User mode flag
                                j++;
                                {
                                    if ((opcode & (0x1 << 22)) != 0)
                                        line += '^';
                                }
                                break;

                            case 'O': // CoProcessor Opcode
                                j++;
                                {
                                    line += "???";
                                }
                                break;



                            case 'x': // Psr argument
                                j++;
                                {
                                    Boolean mode = ((opcode & (0x1 << 22)) != 0);
                                    line += (mode) ? "spsr" : "cpsr";
                                }
                                break;

                            case 'X': // Psr argument with field
                                j++;
                                {
                                    Boolean mode = ((opcode & (0x1 << 22)) != 0);
                                    line += (mode) ? "spsr" : "cpsr";
                                    if (((opcode >> 19) & 0x1) == 1) line += "_flg";
                                    if (((opcode >> 16) & 0x1) == 1) line += "_ctl";
                                }
                                break;

                            case 'L': // Long Transer bitflag for coprocessor
                                j++;
                                {
                                    if (((opcode >> 22) & 0x1) == 1) line += 'l';
                                }
                                break;

                            case 'q': // comment field for SWI or BKPT
                                j++;
                                {
                                    UInt32 comment = opcode & 0xFFFFFF;
                                    line +=  "#0x" + Util.UInt32ToHex(comment);
                                }
                                break;

                            case 'n': // second operand for ALU operations
                                j++;
                                {
                                    Int32 operand;
                                    Int32 shift;
                                    if (((opcode >> 25) & 0x1) == 1) // immediate value
                                    {
                                        operand = (Byte)(opcode & 0xFF);
                                        shift = (Int32)((opcode & 0xF00) >> 7);
                                        line += "#0x" + Util.UInt32ToHex((UInt32)(operand << shift));
                                    }
                                    else // register as 2nd operand
                                    {
                                        UInt32 reg = (opcode & 0xF);
                                        line += Registers[reg];

                                        shift = (((Int32)opcode >> 5) & 0x3);
                                        line += ',' + ALU_Shifts[shift] + ' ';

                                        if (((opcode >> 4) & 0x1) == 1) // shift by register
                                        {
                                            reg = (opcode >> 8) & 0xF;
                                            line += Registers[reg];
                                        }
                                        else
                                        {
                                            operand = (Byte)(opcode & 0xFF);
                                            shift = (Int32)((opcode & 0xF80) >> 7);
                                            line += "#0x" + Util.UInt32ToHex((UInt32)(operand << shift));
                                        }
                                    }
                                }
                                break;

                            case 'a': // Pre/Post-indexed address for LDR and STR
                                j++;
                                {
                                    Boolean add = (opcode & (0x1 << 23)) != 0;
                                    Boolean pre_indexed = (opcode & (0x1 << 24)) != 0;
                                    Boolean immediate = (opcode & (0x1 << 25)) != 0;
                                    UInt32 reg = ((opcode >> 16) & 0xF);
                                    if (immediate)
                                    {
                                        UInt16 immed = (UInt16)(opcode & 0xFFF);
                                        if (immed == 0)
                                        {
                                            line += '[' + Registers[reg] + ']';
                                        }
                                        else if (pre_indexed)
                                        {
                                            line += '[' + Registers[reg] + ", " + (add ? "#0x" : "#-0x") +
                                                Util.UInt16ToHex((UInt16)immed) + ']';
                                            if ((opcode & (0x1 << 24)) == 0 | (opcode & (0x1 << 21)) != 0)
                                                line += '!';
                                        }
                                        else
                                        {
                                            line += '[' + Registers[reg] + "], " + (add ? "#0x" : "#-0x") +
                                                Util.UInt16ToHex((UInt16)immed);
                                        }
                                    }
                                    else
                                    {
                                        Byte shift = (Byte)((opcode >> 7) & 0x1F);
                                        UInt32 shift_type = ((opcode >> 5) & 0x3);
                                        UInt32 offset_reg = (opcode & 0xF);
                                        if (pre_indexed)
                                        {
                                            line += '[' + Registers[reg] + ", " + (add ? "+" : "-") + Registers[offset_reg];
                                            if (shift != 0)
                                            {
                                                line += ", " + ALU_Shifts[shift_type] + " #0x" + Util.ByteToHex(shift);
                                            }
                                            line += ']';
                                        }
                                        else
                                        {
                                            line += '[' + Registers[reg] + "], " + (add ? "+" : "-") + Registers[offset_reg];
                                            if (shift != 0)
                                            {
                                                line += ", " + ALU_Shifts[shift_type] + " #0x" + Util.ByteToHex(shift);
                                            }
                                        }
                                        if ((opcode & (0x1 << 24)) == 0 | (opcode & (0x1 << 21)) != 0)
                                            line += '!';
                                    }
                                } break;

                            case 'A': // Pre/Post-indexed address for LDRH/SH/D and STRH/SH/D
                                j++;
                                {
                                    Boolean add = (opcode & (0x1 << 23)) != 0;
                                    Boolean pre_indexed = (opcode & (0x1 << 24)) != 0;
                                    Boolean immediate = (opcode & (0x1 << 25)) != 0;
                                    UInt32 reg = ((opcode >> 16) & 0xF);
                                    if (immediate)
                                    {
                                        UInt16 immed = (UInt16)((opcode & 0xF) | ((opcode >> 4) & 0xF0));
                                        if (immed == 0)
                                        {
                                            line += '[' + Registers[reg] + ']';
                                        }
                                        else if (pre_indexed)
                                        {
                                            line += '[' + Registers[reg] + ", " + (add ? "#0x" : "#-0x") +
                                                Util.UInt16ToHex((UInt16)immed) + ']';
                                            if ((opcode & (0x1 << 24)) == 0 | (opcode & (0x1 << 21)) != 0)
                                                line += '!';
                                        }
                                        else
                                        {
                                            line += '[' + Registers[reg] + "], " + (add ? "#0x" : "#-0x") +
                                                Util.UInt16ToHex((UInt16)immed);
                                        }
                                    }
                                    else
                                    {
                                        Byte shift = (Byte)((opcode >> 7) & 0x1F);
                                        UInt32 shift_type = ((opcode >> 5) & 0x3);
                                        UInt32 offset_reg = (opcode & 0xF);
                                        if (pre_indexed)
                                        {
                                            line += '[' + Registers[reg] + ", " + (add ? "+" : "-") + Registers[offset_reg];
                                            if (shift != 0)
                                            {
                                                line += ", " + ALU_Shifts[shift_type] + " #0x" + Util.ByteToHex(shift);
                                            }
                                            line += ']';
                                        }
                                        else
                                        {
                                            line += '[' + Registers[reg] + "], " + (add ? "+" : "-") + Registers[offset_reg];
                                            if (shift != 0)
                                            {
                                                line += ", " + ALU_Shifts[shift_type] + " #0x" + Util.ByteToHex(shift);
                                            }
                                        }
                                        if ((opcode & (0x1 << 24)) == 0 | (opcode & (0x1 << 21)) != 0)
                                            line += '!';
                                    }
                                }
                                break;



                            case 'l': // register list
                                j++;
                                {
                                    Int32 amount = 0;
                                    Int32 lastFirstReg = 0;
                                    Boolean hasRegs = false;
                                    for (Int32 u = 0; u < 8; u++)
                                    {
                                        if ((opcode & (1 << u)) != 0)
                                        {
                                            if (amount == 0)
                                                lastFirstReg = u;
                                            amount++;
                                        }
                                        else
                                        {
                                            if (hasRegs && amount > 0)
                                            {
                                                line += ",";
                                                hasRegs = false;
                                            }

                                            if (amount > 2)
                                            {
                                                line += Registers[lastFirstReg] + "-" + Registers[u - 1];
                                                hasRegs = true;
                                            }
                                            else if (amount == 2)
                                            {
                                                line += Registers[lastFirstReg] + "," + Registers[u - 1];
                                                hasRegs = true;
                                            }
                                            else if (amount == 1)
                                            {
                                                line += Registers[u - 1];
                                                hasRegs = true;
                                            }
                                            amount = 0;
                                        }
                                    }
                                    if (amount != 0)
                                    {
                                        if (hasRegs)
                                            line += ",";

                                        if (amount > 2)
                                            line += Registers[lastFirstReg] + "-" + Registers[7];
                                        else if (amount == 2)
                                            line += Registers[lastFirstReg] + "," + Registers[7];
                                        else if (amount == 1)
                                            line += Registers[7];
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
                result.Add(new Instruction(new Pointer(pc + i), Util.UInt32ToBytes(opcode, false), line));
            }
            return result.ToArray();
        }



        /// <summary>
        /// Format codes :
        /// %r - indicates a register, the following number being said register's bit index in the opcode
        /// %h - indicates a Hi register, the first number is register bits, second number is high bit
        /// %i - indicates an immediate value, the following numbers being the bit index, the bit length, and shift amount
        /// %c - indicates a condition suffix to an instruction
        /// %b - indicates a 'B' byte suffix to an instruction, the argument number being the bit index of the bit flag
        /// %p - indicates an immediate signed offset
        /// </summary>
        static Opcode[] Opcodes_Thumb = new Opcode[]
        {
            new Opcode(0xFFFF, 0x0000, "nop"),
            // Format 1 - move shifted register
            new Opcode(0xF800, 0x0800, "lsr %r(0), %r(3), %i(6,5,0)"),
            new Opcode(0xF800, 0x0000, "lsl %r(0), %r(3), %i(6,5,0)"),
            new Opcode(0xF800, 0x1000, "asr %r(0), %r(3), %i(6,5,0)"),
            // Format 2 - add/subtract
            new Opcode(0xFE00, 0x1800, "add %r(0), %r(3), %r(6)"),
            new Opcode(0xFE00, 0x1A00, "sub %r(0), %r(3), %r(6)"),
            new Opcode(0xFFC0, 0x1C00, "mov %r(0), %r(3)"),
            new Opcode(0xFE00, 0x1C00, "add %r(0), %r(3), %i(6,3,0)"),
            new Opcode(0xFE00, 0x1E00, "sub %r(0), %r(3), %i(6,3,0)"),
            // Format 3 - move/compare/add/subtract immediate
            new Opcode(0xF800, 0x2000, "mov %r(8), %i(0,8,0)"),
            new Opcode(0xF800, 0x2800, "cmp %r(8), %i(0,8,0)"),
            new Opcode(0xF800, 0x3000, "add %r(8), %i(0,8,0)"),
            new Opcode(0xF800, 0x3800, "sub %r(8), %i(0,8,0)"),
            // Format 4 - ALU operations
            new Opcode(0xFFC0, 0x4000, "and %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4040, "eor %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4080, "lsl %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x40C0, "lsr %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4100, "asr %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4140, "adc %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4180, "sbc %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x41C0, "ror %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4200, "tst %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4240, "neg %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4280, "cmp %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x42C0, "cmn %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4300, "orr %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4340, "mul %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x4380, "bic %r(0), %r(3)"),
            new Opcode(0xFFC0, 0x43C0, "mvn %r(0), %r(3)"),
            // Format 5 - Hi register operations / branch exchange
            new Opcode(0xFF80, 0x4700, "bx %h(3,6)"),
            new Opcode(0xFCC0, 0x4400, "?"),
            new Opcode(0xFF00, 0x4400, "add %h(0,7), %h(3,6)"),
            new Opcode(0xFF00, 0x4500, "cmp %h(0,7), %h(3,6)"),
            new Opcode(0xFF00, 0x4600, "mov %h(0,7), %h(3,6)"),
            // Format 6 - Load PC-relative immediate
            new Opcode(0xF800, 0x4800, "ldr %r(8), [pc, %i(0,8,2)] %e"),
            // Format 7 - load/store with register offset
            new Opcode(0xFA00, 0x5000, "str%b(10) %r(0), [%r(3), %r(6)]"),
            new Opcode(0xFA00, 0x5800, "ldr%b(10) %r(0), [%r(3), %r(6)]"),
            // Format 8 - load/store sign extended byte/halfword
            new Opcode(0xFE00, 0x5200, "strh %r(0), [%r(3), %r(6)]"),
            new Opcode(0xFE00, 0x5A00, "ldsb %r(0), [%r(3), %r(6)]"),
            new Opcode(0xFE00, 0x5600, "ldrh %r(0), [%r(3), %r(6)]"),
            new Opcode(0xFE00, 0x5E00, "ldsh %r(0), [%r(3), %r(6)]"),
            // Format 9 - load/store with immediate offset
            new Opcode(0xF800, 0x6000, "str %r(0), [%r(3), %i(6,5,2)]"),
            new Opcode(0xF800, 0x6800, "ldr %r(0), [%r(3), %i(6,5,2)]"),
            new Opcode(0xF800, 0x7000, "strb %r(0), [%r(3), %i(6,5,0)]"),
            new Opcode(0xF800, 0x7800, "ldrb %r(0), [%r(3), %i(6,5,0)]"),
            // Format 10 - load/store halfword
            new Opcode(0xF800, 0x8000, "strh %r(0), [%r(3), %i(6,5,1)]"),
            new Opcode(0xF800, 0x8800, "ldrh %r(0), [%r(3), %i(6,5,1)]"),
            // Format 11 - load/store SP-relative
            new Opcode(0xF800, 0x9000, "str %r(8), [sp, %i(0,8,2)]"),
            new Opcode(0xF800, 0x9800, "ldr %r(8), [sp, %i(0,8,2)]"),
            // Format 12 - get relative address
            new Opcode(0xF800, 0xA000, "add %r(8), pc, %i(0,8,2) %e"),
            new Opcode(0xF800, 0xA800, "add %r(8), sp, %i(0,8,2)"),
            // Format 13 - add offset to stack pointer
            new Opcode(0xFF80, 0xB000, "add sp, %i(0,7,2)"),
            new Opcode(0xFF80, 0xB080, "add sp, -%i(0,7,2)"),
            // Format 14 - push/pop registers
            new Opcode(0xFFFF, 0xB500, "push {lr}"),
            new Opcode(0xFF00, 0xB400, "push {%l}"),
            new Opcode(0xFF00, 0xB500, "push {%l,lr}"),
            new Opcode(0xFFFF, 0xBD00, "pop {pc}"),
            new Opcode(0xFF00, 0xBD00, "pop {%l,pc}"),
            new Opcode(0xFF00, 0xBC00, "pop {%l}"),
            // Format 15 - multiple load/store
            new Opcode(0xF800, 0xC000, "stmia %r(8)!, {%l}"),
            new Opcode(0xF800, 0xC800, "ldmia %r(8)!, {%l}"),
            // Format 16 - conditional branch
            new Opcode(0xF000, 0xD000, "b%c %o(0,8,1)"),
            // Format 18 - unconditional branch
            new Opcode(0xF800, 0xE000, "b %o(0,11,1)"),
            // Format 19 - long branch with link
            new Opcode(0xF800, 0xF000, "bl%p"),
            new Opcode(0xF800, 0xF800, "blh %P"),
            // Format 17 - software interrupt and breakpoint
            new Opcode(0xFF00, 0xDF00, "swi %i(0,8,0)"),
            new Opcode(0xFF00, 0xBE00, "bkpt %i(0,8,0)"),
            // Unknown
            new Opcode(0x0000, 0x0000, "???")
        };

        public static Instruction[] Disassemble_Thumb(Byte[] data, Pointer pc)
        {
            List<Instruction> result = new List<Instruction>();

            String line;
            for (UInt32 i = 0; i < data.Length; i += 2)
            {
                line = "";
                UInt16 opcode = data.GetUInt16(i, true);

                Int32 index = 0;
                while ((Opcodes_Thumb[index].BitMask & opcode) != Opcodes_Thumb[index].CodeValue)
                    index++;

                Int32 j = 0;
                Int32[] args;
                while (j < Opcodes_Thumb[index].Format.Length)
                {
                    if (Opcodes_Thumb[index].Format[j] != '%')
                    {
                        line += Opcodes_Thumb[index].Format[j++];
                    }
                    else
                    {
                        j++;

                        switch (Opcodes_Thumb[index].Format[j])
                        {
                            case 'r': // register
                                j++; args = ReadFormatCodeArgs(Opcodes_Thumb[index].Format, ref j);
                                {
                                    line += Registers[(opcode >> args[0]) & 0x7];
                                }
                                break;

                            case 'h': // hi register
                                j++; args = ReadFormatCodeArgs(Opcodes_Thumb[index].Format, ref j);
                                {
                                    Int32 reg = (opcode >> args[0]) & 7;
                                    if ((opcode & (0x1 << args[1])) != 0)
                                        reg += 8;
                                    line += Registers[reg];
                                }
                                break;



                            case 'c': // condition
                                j++;
                                {
                                    line += Conditions[(opcode >> 8) & 0xF];
                                }
                                break;

                            case 'b': // byte suffix flag
                                j++; args = ReadFormatCodeArgs(Opcodes_Thumb[index].Format, ref j);
                                {
                                    if ((opcode & (0x1 << args[0])) != 0)
                                        line += "b";
                                }
                                break;



                            case 'e': // PC-relative offset operation result
                                j++;
                                {
                                    Pointer address;
                                    address = new Pointer((((pc + i) & 0xFFFFFFFC) + 4));
                                    address += ((opcode & 0xFF) << 2);
                                    Byte[] pointer = new Byte[4];
                                    pointer = Core.ReadData(address, 4);
                                    Array.Reverse(pointer);
                                    line += "(=$" + Util.BytesToHex(pointer) + ")";
                                }
                                break;



                            case 'i': // immediate value
                                j++; args = ReadFormatCodeArgs(Opcodes_Thumb[index].Format, ref j);
                                {
                                    Int32 val = ((opcode >> args[0]) & GetBitMask(args[1])) << args[2];
                                    line += "#";
                                    if (val < 256) line += "0x" + Util.ByteToHex((Byte)val);
                                    else line += "0x" + Util.UInt16ToHex((UInt16)val);
                                }
                                break;



                            case 'o': // signed offset
                                j++; args = ReadFormatCodeArgs(Opcodes_Thumb[index].Format, ref j);
                                {
                                    Int32 offset = (opcode >> args[0]) & GetBitMask(args[1]);
                                    if ((offset & (0x1 << (args[1] - 1))) != 0)
                                        offset = (Int32)((Int64)offset - 0x80000000);
                                    offset <<= args[2];
                                    Pointer address = new Pointer((UInt32)(pc + i - (i & 1) + (offset + 4)));
                                    line += '$' + Util.BytesToHex(address.ToBytes(false));
                                }
                                break;

                            case 'p': // long branch instruction pointer
                                j++;
                                {
                                    i += 2;
                                    UInt16 opcode2;
                                    try { opcode2 = data.GetUInt16(i, true); }
                                    catch { opcode2 = Util.BytesToUInt16(Core.ReadData(pc + i, 2), true); }
                                    if ((opcode2 & 0xF800) == 0xE800) line += 'x'; // switch to ARM
                                    line += ' ';
                                    Int32 branch = opcode & 0x7FF;
                                    if ((branch & 0x400) != 0) // is negative
                                        branch |= 0xFFF800;
                                    branch = (branch << 12) | ((opcode2 & 0x7FF) << 1);
                                    Pointer address = new Pointer((UInt32)((pc + i) + 2 + branch));
                                    line += '$' + Util.BytesToHex(address.ToBytes(false));
                                }
                                break;

                            case 'P': // BLH relative pointer
                                j++;
                                {
                                    i += 2;
                                    UInt16 opcode2 = data.GetUInt16(i, true);
                                    Pointer address = new Pointer((UInt32)((pc + i) + 2 + ((opcode2 & 0x7FF) << 1)));
                                    line += '$' + Util.BytesToHex(address.ToBytes(false));
                                }
                                break;


                            case 'l': // register list
                                j++;
                                {
                                    Int32 amount = 0;
                                    Int32 lastFirstReg = 0;
                                    Boolean hasRegs = false;
                                    for (Int32 u = 0; u < 8; u++)
                                    {
                                        if ((opcode & (1 << u)) != 0)
                                        {
                                            if (amount == 0)
                                                lastFirstReg = u;
                                            amount++;
                                        }
                                        else
                                        {
                                            if (hasRegs && amount > 0)
                                            {
                                                line += ",";
                                                hasRegs = false;
                                            }

                                            if (amount > 2)
                                            {
                                                line += Registers[lastFirstReg] + "-" + Registers[u - 1];
                                                hasRegs = true;
                                            }
                                            else if (amount == 2)
                                            {
                                                line += Registers[lastFirstReg] + "," + Registers[u - 1];
                                                hasRegs = true;
                                            }
                                            else if (amount == 1)
                                            {
                                                line += Registers[u - 1];
                                                hasRegs = true;
                                            }
                                            amount = 0;
                                        }
                                    }
                                    if (amount != 0)
                                    {
                                        if (hasRegs)
                                            line += ",";

                                        if (amount > 2)
                                            line += Registers[lastFirstReg] + "-" + Registers[7];
                                        else if (amount == 2)
                                            line += Registers[lastFirstReg] + "," + Registers[7];
                                        else if (amount == 1)
                                            line += Registers[7];
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
                Pointer pc_address = new Pointer(pc + i);
                Byte[] asm_data;
                if (line.StartsWith("bl"))
                {
                    UInt32 long_opcode = (UInt32)((opcode << 16) | data.GetUInt16(i, true));
                    asm_data = Util.UInt32ToBytes(long_opcode, false);
                    pc_address -= 2;
                }
                else
                {
                    asm_data = Util.UInt16ToBytes(opcode, false);
                }
                result.Add(new Instruction(pc_address, asm_data, line));
            }
            return result.ToArray();
        }



        static Int32[] ReadFormatCodeArgs(String formatCode, ref Int32 index)
        {
            List<Int32> result = new List<Int32>();
            while (formatCode[index] != '(')
                index++;

            index++;
            Int32 length = 0; 
            while (index + length < formatCode.Length)
            {
                if (formatCode[index + length] == ')')
                {
                    result.Add(Convert.ToInt32(formatCode.Substring(index, length)));
                    index += length;
                    index++;
                    break;
                }
                else if (formatCode[index + length] == ',')
                {
                    result.Add(Convert.ToInt32(formatCode.Substring(index, length)));
                    index += length;
                    index++;
                    length = 0;
                }
                else length++;
            }
            return result.ToArray();
        }

        static Int32 GetBitMask(Int32 bitAmount)
        {
            Int32 result = 0;
            for (Int32 i = 0; i < bitAmount; i++)
            {
                result <<= 1;
                result += 1;
            }
            return result;
        }

        public static String GetInstructionsRegex_Thumb()
        {
            String result = null;
            String instruction;
            Int32 length;
            Int32 index;
            foreach (Opcode opcode in Opcodes_Thumb)
            {
                if (result == null)
                    result = "(";
                else result += "|";
                length = opcode.Format.IndexOf(' ');
                if (length < 0) length = opcode.Format.Length;
                instruction = opcode.Format.Substring(0, length);
                if (instruction.Contains("?"))
                    continue;
                index = instruction.IndexOf('%');
                if (index >= 0)
                {
                    Char format_char = instruction[index + 1];
                    instruction = instruction.Remove(index, 2);
                    switch (format_char)
                    {
                        case 'c':
                            foreach (String flag in Conditions)
                            {
                                result += instruction.Insert(index, flag) + "|";
                            }
                            result += instruction.Insert(index, "al");
                            continue;
                        case 'b': instruction = instruction.Remove(index);
                                    result += instruction + "|"; result += instruction.Insert(index, "b"); continue;
                        case 'p':   result += instruction + "|"; result += instruction.Insert(index, "x"); continue;
                    }
                }
                else result += instruction;
            }
            result += ") ";
            return result;
        }
    }
}