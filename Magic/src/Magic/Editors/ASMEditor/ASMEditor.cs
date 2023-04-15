using GBA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Editors
{
    public partial class ASMEditor : Editor
    {
        ASM.Instruction[] CurrentASM;

        ASM.RegisterSet CurrentCPU;

        BindingList<String> CurrentStack;



        public ASMEditor()
        {
            this.InitializeComponent();

            this.Read_LengthBox.Maximum = Core.CurrentROMSize;

            this.CurrentCPU = new ASM.RegisterSet(new Pointer(), true);
            this.CurrentStack = new BindingList<String>();

            this.Test_Stack.DataSource = this.CurrentStack;
        }
        
        void Core_LoadValues()
        {
            this.Test_R0.Value = this.CurrentCPU.R0;
            this.Test_R1.Value = this.CurrentCPU.R1;
            this.Test_R2.Value = this.CurrentCPU.R2;
            this.Test_R3.Value = this.CurrentCPU.R3;
            this.Test_R4.Value = this.CurrentCPU.R4;
            this.Test_R5.Value = this.CurrentCPU.R5;
            this.Test_R6.Value = this.CurrentCPU.R6;
            this.Test_R7.Value = this.CurrentCPU.R7;
            this.Test_R8.Value = this.CurrentCPU.R8;
            this.Test_R9.Value = this.CurrentCPU.R9;
            this.Test_R10.Value = this.CurrentCPU.R10;
            this.Test_R11.Value = this.CurrentCPU.R11;
            this.Test_R12.Value = this.CurrentCPU.R12;
            this.Test_SP.Value = this.CurrentCPU.SP;
            this.Test_LR.Value = this.CurrentCPU.LR;
            this.Test_PC.Value = this.CurrentCPU.PC;

            this.Test_CPSR_N.Checked = this.CurrentCPU.N;
            this.Test_CPSR_Z.Checked = this.CurrentCPU.Z;
            this.Test_CPSR_C.Checked = this.CurrentCPU.C;
            this.Test_CPSR_V.Checked = this.CurrentCPU.V;

            this.Test_CPSR_I.Checked = this.CurrentCPU.I;
            this.Test_CPSR_F.Checked = this.CurrentCPU.F;
            this.Test_CPSR_T.Checked = this.CurrentCPU.T;
            this.Test_CPSR_Mode.Value = this.CurrentCPU.M;

            this.CurrentStack.Clear();
            foreach (UInt32 item in this.CurrentCPU.Stack)
            {
                this.CurrentStack.Add(Util.UInt32ToHex(item).PadLeft(8, '0'));
            }
        }
        void Core_Dissassemble(Pointer address, Int32 length, Boolean thumb)
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

            this.CurrentCPU.T = thumb;
            this.CurrentCPU.PC = address + Pointer.HardwareOffset;

            Byte[] data = Core.ReadData(address, (Int32)length);

            this.CurrentASM = thumb ?
                GBA.ASM.Disassemble_Thumb(data, address) :
                GBA.ASM.Disassemble_ARM(data, address);

            this.CodeBox.Items.Clear();
            ListViewItem[] code = new ListViewItem[this.CurrentASM.Length];
            for (Int32 i = 0; i < this.CurrentASM.Length; i++)
            {
                code[i] = new ListViewItem(new String[]
                {
                    this.CurrentASM[i].Address.ToString(),
                    Util.BytesToHex(this.CurrentASM[i].Data),
                    this.CurrentASM[i].Code
                });
            }
            this.CodeBox.Items.AddRange(code);
            this.CodeBox.Items[0].Selected = true;
            this.CodeBox.Select();
        }



        private void CodeBox_SelectedIndexChanged(Object sender, EventArgs e)
        {

        }

        private void DissassembleButton_Click(Object sender, EventArgs e)
        {
            try
            {
                this.Core_Dissassemble(
                    this.Read_AddressBox.Value,
                    (Int32)this.Read_LengthBox.Value,
                    this.Read_ThumbRadioButton.Checked);
                this.Core_LoadValues();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while attempting to disassemble.", ex);
                this.CodeBox.Items.Clear();
                return;
            }
        }



        private void Test_ResetCPUButton_Click(Object sender, EventArgs e)
        {
            this.CurrentCPU = new ASM.RegisterSet(this.Read_AddressBox.Value, this.Read_ThumbRadioButton.Checked);

            this.Core_LoadValues();
        }
        private void Test_ReadLineButton_Click(Object sender, EventArgs e)
        {
            Int32 currentLine = this.CodeBox.SelectedIndices[0];

            try
            {
                this.CurrentCPU = this.CurrentASM[currentLine].Read(this.CurrentCPU);
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly read selected line.", ex); return;
            }

            Pointer address = new Pointer(this.CurrentCPU.PC, false, true);

            if (address - 2 == this.CurrentASM[currentLine].Address)
            {
                this.CodeBox.Items[currentLine + 1].Selected = true;
                this.CodeBox.Select();
            }
            else
            {
                this.Read_ThumbRadioButton.Checked = this.CurrentCPU.T;
                this.Read_AddressBox.Value = address;
                Try: try
                {
                    this.Core_Dissassemble(
                        this.Read_AddressBox.Value,
                        (Int32)this.Read_LengthBox.Value,
                        this.Read_ThumbRadioButton.Checked);
                    this.Core_LoadValues();
                }
                catch
                {
                    this.Read_LengthBox.Value += 2;
                    goto Try;
                }

                this.CodeBox.Items[0].Selected = true;
                this.CodeBox.Select();
            }
            this.Core_LoadValues();
        }

        private void IO_SaveButton_Click(Object sender, EventArgs e)
        {

        }
        private void IO_WriteButton_Click(Object sender, EventArgs e)
        {

        }
    }
}
