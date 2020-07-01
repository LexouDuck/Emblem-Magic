using GBA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class ASMEditor : Editor
    {
        ASM.Instruction[] CurrentASM;

        ASM.RegisterSet CurrentCPU;

        BindingList<string> CurrentStack;



        public ASMEditor()
        {
            InitializeComponent();

            Read_LengthBox.Maximum = Core.CurrentROMSize;

            CurrentCPU = new ASM.RegisterSet(new Pointer(), true);
            CurrentStack = new BindingList<string>();

            Test_Stack.DataSource = CurrentStack;
        }
        
        void Core_LoadValues()
        {
            Test_R0.Value = CurrentCPU.R0;
            Test_R1.Value = CurrentCPU.R1;
            Test_R2.Value = CurrentCPU.R2;
            Test_R3.Value = CurrentCPU.R3;
            Test_R4.Value = CurrentCPU.R4;
            Test_R5.Value = CurrentCPU.R5;
            Test_R6.Value = CurrentCPU.R6;
            Test_R7.Value = CurrentCPU.R7;
            Test_R8.Value = CurrentCPU.R8;
            Test_R9.Value = CurrentCPU.R9;
            Test_R10.Value = CurrentCPU.R10;
            Test_R11.Value = CurrentCPU.R11;
            Test_R12.Value = CurrentCPU.R12;
            Test_SP.Value = CurrentCPU.SP;
            Test_LR.Value = CurrentCPU.LR;
            Test_PC.Value = CurrentCPU.PC;

            Test_CPSR_N.Checked = CurrentCPU.N;
            Test_CPSR_Z.Checked = CurrentCPU.Z;
            Test_CPSR_C.Checked = CurrentCPU.C;
            Test_CPSR_V.Checked = CurrentCPU.V;

            Test_CPSR_I.Checked = CurrentCPU.I;
            Test_CPSR_F.Checked = CurrentCPU.F;
            Test_CPSR_T.Checked = CurrentCPU.T;
            Test_CPSR_Mode.Value = CurrentCPU.M;

            CurrentStack.Clear();
            foreach (uint item in CurrentCPU.Stack)
            {
                CurrentStack.Add(Util.UInt32ToHex(item).PadLeft(8, '0'));
            }
        }
        void Core_Dissassemble(Pointer address, int length, bool thumb)
        {
            if (address % 2 == 1)
            {
                address -= 1;
                thumb = true;
            }

            if (thumb && length % 2 != 0)
                throw new Exception("Thumb machine code is 2 bytes per instruction - length should be a multiple of 2");
            if (!thumb && length % 4 != 0)
                throw new Exception("ARM machine code is 4 bytes per instruction - length should be a multiple of 4");

            CurrentCPU.T = thumb;
            CurrentCPU.PC = address + Pointer.HardwareOffset;

            byte[] data = Core.ReadData(address, (int)length);

            CurrentASM = thumb ?
                GBA.ASM.Disassemble_Thumb(data, address) :
                GBA.ASM.Disassemble_ARM(data, address);

            CodeBox.Items.Clear();
            ListViewItem[] code = new ListViewItem[CurrentASM.Length];
            for (int i = 0; i < CurrentASM.Length; i++)
            {
                code[i] = new ListViewItem(new string[]
                {
                    CurrentASM[i].Address.ToString(),
                    Util.BytesToHex(CurrentASM[i].Data),
                    CurrentASM[i].Code
                });
            }
            CodeBox.Items.AddRange(code);
            CodeBox.Items[0].Selected = true;
            CodeBox.Select();
        }



        private void CodeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DissassembleButton_Click(object sender, EventArgs e)
        {
            try
            {
                Core_Dissassemble(
                    Read_AddressBox.Value,
                    (int)Read_LengthBox.Value,
                    Read_ThumbRadioButton.Checked);
                Core_LoadValues();
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error while attempting to disassemble.", ex);
                CodeBox.Items.Clear();
                return;
            }
        }



        private void Test_ResetCPUButton_Click(object sender, EventArgs e)
        {
            CurrentCPU = new ASM.RegisterSet(Read_AddressBox.Value, Read_ThumbRadioButton.Checked);

            Core_LoadValues();
        }
        private void Test_ReadLineButton_Click(object sender, EventArgs e)
        {
            int currentLine = CodeBox.SelectedIndices[0];

            try
            {
                CurrentCPU = CurrentASM[currentLine].Read(CurrentCPU);
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly read selected line.", ex); return;
            }

            Pointer address = new Pointer(CurrentCPU.PC, false, true);

            if (address - 2 == CurrentASM[currentLine].Address)
            {
                CodeBox.Items[currentLine + 1].Selected = true;
                CodeBox.Select();
            }
            else
            {
                Read_ThumbRadioButton.Checked = CurrentCPU.T;
                Read_AddressBox.Value = address;
                Try: try
                {
                    Core_Dissassemble(
                        Read_AddressBox.Value,
                        (int)Read_LengthBox.Value,
                        Read_ThumbRadioButton.Checked);
                    Core_LoadValues();
                }
                catch
                {
                    Read_LengthBox.Value += 2;
                    goto Try;
                }

                CodeBox.Items[0].Selected = true;
                CodeBox.Select();
            }
            Core_LoadValues();
        }

        private void IO_SaveButton_Click(Object sender, EventArgs e)
        {

        }
        private void IO_WriteButton_Click(Object sender, EventArgs e)
        {

        }
    }
}
