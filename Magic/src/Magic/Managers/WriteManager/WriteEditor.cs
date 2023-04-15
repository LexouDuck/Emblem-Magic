using System;
using System.Windows.Forms;
using GBA;
using Library.Forms;

namespace Magic.Editors
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
        
        public WriteEditor()
        {
            this.List = new SortableBindingList<Tuple<
            DateTime,
            Pointer,
            Int32,
            String,
            String,
            Boolean,
            Boolean>>();

            this.InitializeComponent();

            this.WriteDataHexBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);
            this.WritePhraseTextBox.KeyDown += new KeyEventHandler(this.TextBox_SelectAll);

            this.WriteHistoryList.AutoGenerateColumns = false;
            this.WriteHistoryList.DataSource = this.List;

            this.List_TimeColumn.DataPropertyName    = "Item1";
            this.List_AddressColumn.DataPropertyName = "Item2";
            this.List_LengthColumn.DataPropertyName  = "Item3";
            this.List_AuthorColumn.DataPropertyName  = "Item4";
            this.List_PhraseColumn.DataPropertyName  = "Item5";
            this.List_IsSavedColumn.DataPropertyName = "Item6";
            this.List_PatchedColumn.DataPropertyName = "Item7";
        }



        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            this.Update_WriteList();

            this.Update_EditPanel(null);
        }



        void Update_WriteList()
        {
            try
            {
                this.List.Clear();

                foreach (var write in this.App.MHF.Write.History)
                {
                    this.List.Add(Tuple.Create(
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
                UI.ShowError("The write list could not be loaded onscreen.", ex);
            }
        }
        void Update_EditPanel(Write selection)
        {
            if (selection == null)
            {
                this.WriteDataHexBox.Value = new Byte[0];
                this.WritePointerBox.Value = new Pointer();
                this.WriteLength.Text = "-";
                this.WriteEditorLabel.Text = "";
                this.WritePhraseTextBox.Text = "";
            }
            else
            {
                this.WriteDataHexBox.Value = selection.Data;
                this.WritePointerBox.Value = selection.Address;
                this.WriteLength.Text = "0x" + Util.UInt32ToHex((UInt32)selection.Data.Length).TrimStart('0');
                this.WriteEditorLabel.Text = selection.Author;
                this.WritePhraseTextBox.Text = selection.Phrase;
            }
        }



        /// <summary>
        /// Is read when the user clicks a cell or row on the DataGrid
        /// </summary>
        private void SelectWrite_Click(Object sender, EventArgs e)
        {
            if (this.WriteHistoryList.SelectedRows.Count == 1)
            {
                Pointer address = (Pointer)this.WriteHistoryList.CurrentRow.Cells[1].Value;

                this.Update_EditPanel(this.App.MHF.Write.Find(address));
            }
            else
            {
                this.Update_EditPanel(null);
            }
        }
        /// <summary>
        /// Checks current selection, and applies the write (or prompts the user) accordingly.
        /// </summary>
        private void ApplyWrite_Click(Object sender, EventArgs e)
        {
            if (this.WriteHistoryList.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no write selected.");
            }
            else if (this.WriteHistoryList.SelectedRows.Count == 1)
            {
                Core.WriteData(this,
                    this.WritePointerBox.Value,
                    this.WriteDataHexBox.Value,
                    this.WritePhraseTextBox.Text);
            }
            else
            {
                if (this.WritePointerBox.Enabled)
                {
                    UI.ShowMessage("Cannot have several writes on one offset.");
                }
                else if (Prompt.ChangeSeveralWrites() == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in this.WriteHistoryList.SelectedRows)
                    {
                        Core.WriteData(this,
                            Util.StringToAddress((String)row.Cells[1].Value),
                            this.WriteDataHexBox.Value,
                            (String)row.Cells[4].Value);
                    }
                }
            }
        }
        /// <summary>
        /// Checks current selection, and deletes the write (or prompts the user) accordingly.
        /// </summary>
        private void DeleteWrite_Click(Object sender, EventArgs e)
        {
            if (this.WriteHistoryList.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no write selected.");
            }
            else if (this.WriteHistoryList.SelectedRows.Count == 1)
            {
                Core.RestoreData(this.WritePointerBox.Value, this.WriteDataHexBox.Value.Length);
            }
            else
            {
                if (Prompt.DeleteSeveralWrites() == DialogResult.Yes)
                {
                    UI.ShowMessage("Deleting several writes at once isn't supported yet, sorry...");
                }
            }
        }
        /// <summary>
        /// Merges all currently selected writes from the list, adding bytes in the interval to the write data
        /// </summary>
        private void MergeWrites_Click(Object sender, EventArgs e)
        {
            if (this.WriteHistoryList.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no write selected.");
            }
            else if (this.WriteHistoryList.SelectedRows.Count == 1)
            {
                UI.ShowMessage("Only one write is selected.");
            }
            else
            {
                Pointer address = new Pointer((UInt32)Core.CurrentROMSize);
                Pointer endbyte = new Pointer();
                foreach (DataGridViewRow row in this.WriteHistoryList.SelectedRows)
                {
                    if ((Pointer)row.Cells[1].Value < address)
                        address = (Pointer)row.Cells[1].Value;
                    if ((Pointer)row.Cells[1].Value + (Int32)row.Cells[2].Value > endbyte)
                        endbyte = (Pointer)row.Cells[1].Value + (Int32)row.Cells[2].Value;
                }
                Core.WriteData(this, address, Core.ReadData(address, endbyte - address), "Merged write");
                UI.PerformUpdate();
            }
        }

        private void MarkSpaceButton_Click(Object sender, EventArgs e)
        {
            if (this.WriteHistoryList.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There are no writes selected.");
            }
            else if (this.WriteHistoryList.SelectedRows.Count == 1)
            {
                Pointer address = (Pointer)this.WriteHistoryList.SelectedRows[0].Cells[1].Value;
                this.App.MHF.Space.MarkSpace((String)this.MarkSpaceBox.SelectedValue,
                    address, address + (Int32)this.WriteHistoryList.SelectedRows[0].Cells[2].Value);
                UI.PerformUpdate();
            }
            else
            {
                foreach (DataGridViewRow row in this.WriteHistoryList.SelectedRows)
                {
                    Pointer address = (Pointer)row.Cells[1].Value;
                    this.App.MHF.Space.MarkSpace((String)this.MarkSpaceBox.SelectedValue,
                        address, address + (Int32)row.Cells[2].Value);
                }
                UI.PerformUpdate();
            }
        }
    }
}
