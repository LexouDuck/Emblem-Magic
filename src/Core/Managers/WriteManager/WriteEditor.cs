﻿using GBA;
using Library.Forms;
using System;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class WriteEditor : Editor
    {
        public SortableBindingList<Tuple<
            DateTime,
            Pointer,
            Int32,
            String,
            String,
            Boolean,
            Boolean>> List { get; set; }
        
        public WriteEditor() : base()
        {
            List = new SortableBindingList<Tuple<
            DateTime,
            Pointer,
            Int32,
            String,
            String,
            Boolean,
            Boolean>>();
            
            InitializeComponent();

            WriteDataHexBox.KeyDown += new KeyEventHandler(TextBox_SelectAll);
            WritePhraseTextBox.KeyDown += new KeyEventHandler(TextBox_SelectAll);

            WriteHistoryList.AutoGenerateColumns = false;
            WriteHistoryList.DataSource = this.List;

            List_TimeColumn.DataPropertyName    = "Item1";
            List_AddressColumn.DataPropertyName = "Item2";
            List_LengthColumn.DataPropertyName  = "Item3";
            List_AuthorColumn.DataPropertyName  = "Item4";
            List_PhraseColumn.DataPropertyName  = "Item5";
            List_IsSavedColumn.DataPropertyName = "Item6";
            List_PatchedColumn.DataPropertyName = "Item7";
        }



        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            Update_WriteList();

            Update_EditPanel(null);
        }



        void Update_WriteList()
        {
            try
            {
                List.Clear();

                foreach (var write in Program.Core.FEH.Write.History)
                {
                    List.Add(Tuple.Create(
                        write.Time,
                        write.Address,
                        write.Data.Length,//"0x" + Util.UInt32ToHex((uint)write.Data.Length).TrimStart('0'),
                        write.Author,
                        write.Phrase,
                        write.IsSaved,
                        write.Patched));
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("The write list could not be loaded onscreen.", ex);
            }
        }
        void Update_EditPanel(Write selection)
        {
            if (selection == null)
            {
                WriteDataHexBox.Value = new byte[0];
                WritePointerBox.Value = new Pointer();
                WriteLength.Text = "-";
                WriteEditorLabel.Text = "";
                WritePhraseTextBox.Text = "";
            }
            else
            {
                WriteDataHexBox.Value = selection.Data;
                WritePointerBox.Value = selection.Address;
                WriteLength.Text = "0x" + Util.UInt32ToHex((uint)selection.Data.Length).TrimStart('0');
                WriteEditorLabel.Text = selection.Author;
                WritePhraseTextBox.Text = selection.Phrase;
            }
        }



        /// <summary>
        /// Is read when the user clicks a cell or row on the DataGrid
        /// </summary>
        private void SelectWrite_Click(object sender, EventArgs e)
        {
            if (WriteHistoryList.SelectedRows.Count == 1)
            {
                Pointer address = (Pointer)WriteHistoryList.CurrentRow.Cells[1].Value;

                Update_EditPanel(Program.Core.FEH.Write.Find(address));
            }
            else
            {
                Update_EditPanel(null);
            }
        }
        /// <summary>
        /// Checks current selection, and applies the write (or prompts the user) accordingly.
        /// </summary>
        private void ApplyWrite_Click(object sender, EventArgs e)
        {
            if (WriteHistoryList.SelectedRows.Count == 0)
            {
                Program.ShowMessage("There is no write selected.");
            }
            else if (WriteHistoryList.SelectedRows.Count == 1)
            {
                Core.WriteData(this,
                    WritePointerBox.Value,
                    WriteDataHexBox.Value,
                    WritePhraseTextBox.Text);
            }
            else
            {
                if (WritePointerBox.Enabled)
                {
                    Program.ShowMessage("Cannot have several writes on one offset.");
                }
                else if (Prompt.ChangeSeveralWrites() == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in WriteHistoryList.SelectedRows)
                    {
                        Core.WriteData(this,
                            Util.StringToAddress((string)row.Cells[1].Value),
                            WriteDataHexBox.Value,
                            (string)row.Cells[4].Value);
                    }
                }
            }
        }
        /// <summary>
        /// Checks current selection, and deletes the write (or prompts the user) accordingly.
        /// </summary>
        private void DeleteWrite_Click(object sender, EventArgs e)
        {
            if (WriteHistoryList.SelectedRows.Count == 0)
            {
                Program.ShowMessage("There is no write selected.");
            }
            else if (WriteHistoryList.SelectedRows.Count == 1)
            {
                Core.RestoreData(WritePointerBox.Value, WriteDataHexBox.Value.Length);
            }
            else
            {
                if (Prompt.DeleteSeveralWrites() == DialogResult.Yes)
                {
                    Program.ShowMessage("Deleting several writes at once isn't supported yet, sorry...");
                }
            }
        }
        /// <summary>
        /// Merges all currently selected writes from the list, adding bytes in the interval to the write data
        /// </summary>
        private void MergeWrites_Click(object sender, EventArgs e)
        {
            if (WriteHistoryList.SelectedRows.Count == 0)
            {
                Program.ShowMessage("There is no write selected.");
            }
            else if (WriteHistoryList.SelectedRows.Count == 1)
            {
                Program.ShowMessage("Only one write is selected.");
            }
            else
            {
                Pointer address = new Pointer((uint)Core.CurrentROMSize);
                Pointer endbyte = new Pointer();
                foreach (DataGridViewRow row in WriteHistoryList.SelectedRows)
                {
                    if ((Pointer)row.Cells[1].Value < address)
                        address = (Pointer)row.Cells[1].Value;
                    if ((Pointer)row.Cells[1].Value + (int)row.Cells[2].Value > endbyte)
                        endbyte = (Pointer)row.Cells[1].Value + (int)row.Cells[2].Value;
                }
                Core.WriteData(this, address, Core.ReadData(address, endbyte - address), "Merged write");
                Core.PerformUpdate();
            }
        }

        private void MarkSpaceButton_Click(object sender, EventArgs e)
        {
            if (WriteHistoryList.SelectedRows.Count == 0)
            {
                Program.ShowMessage("There are no writes selected.");
            }
            else if (WriteHistoryList.SelectedRows.Count == 1)
            {
                Pointer address = (Pointer)WriteHistoryList.SelectedRows[0].Cells[1].Value;
                Program.Core.FEH.Space.MarkSpace((string)MarkSpaceBox.SelectedValue,
                    address, address + (int)WriteHistoryList.SelectedRows[0].Cells[2].Value);
                Core.PerformUpdate();
            }
            else
            {
                foreach (DataGridViewRow row in WriteHistoryList.SelectedRows)
                {
                    Pointer address = (Pointer)row.Cells[1].Value;
                    Program.Core.FEH.Space.MarkSpace((string)MarkSpaceBox.SelectedValue,
                        address, address + (int)row.Cells[2].Value);
                }
                Core.PerformUpdate();
            }
        }
    }
}
