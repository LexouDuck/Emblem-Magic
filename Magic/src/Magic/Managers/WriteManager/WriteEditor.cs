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
        
        public WriteEditor(IApp app) : base(app)
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

                foreach (var write in App.MHF.Write.History)
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
                UI.ShowError("The write list could not be loaded onscreen.", ex);
            }
        }
        void Update_EditPanel(Write selection)
        {
            if (selection == null)
            {
                WriteDataHexBox.Value = new Byte[0];
                WritePointerBox.Value = new Pointer();
                WriteLength.Text = "-";
                WriteEditorLabel.Text = "";
                WritePhraseTextBox.Text = "";
            }
            else
            {
                WriteDataHexBox.Value = selection.Data;
                WritePointerBox.Value = selection.Address;
                WriteLength.Text = "0x" + Util.UInt32ToHex((UInt32)selection.Data.Length).TrimStart('0');
                WriteEditorLabel.Text = selection.Author;
                WritePhraseTextBox.Text = selection.Phrase;
            }
        }



        /// <summary>
        /// Is read when the user clicks a cell or row on the DataGrid
        /// </summary>
        private void SelectWrite_Click(Object sender, EventArgs e)
        {
            if (WriteHistoryList.SelectedRows.Count == 1)
            {
                Pointer address = (Pointer)WriteHistoryList.CurrentRow.Cells[1].Value;

                Update_EditPanel(App.MHF.Write.Find(address));
            }
            else
            {
                Update_EditPanel(null);
            }
        }
        /// <summary>
        /// Checks current selection, and applies the write (or prompts the user) accordingly.
        /// </summary>
        private void ApplyWrite_Click(Object sender, EventArgs e)
        {
            if (WriteHistoryList.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no write selected.");
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
                    UI.ShowMessage("Cannot have several writes on one offset.");
                }
                else if (Prompt.ChangeSeveralWrites() == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in WriteHistoryList.SelectedRows)
                    {
                        Core.WriteData(this,
                            Util.StringToAddress((String)row.Cells[1].Value),
                            WriteDataHexBox.Value,
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
            if (WriteHistoryList.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no write selected.");
            }
            else if (WriteHistoryList.SelectedRows.Count == 1)
            {
                Core.RestoreData(WritePointerBox.Value, WriteDataHexBox.Value.Length);
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
            if (WriteHistoryList.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no write selected.");
            }
            else if (WriteHistoryList.SelectedRows.Count == 1)
            {
                UI.ShowMessage("Only one write is selected.");
            }
            else
            {
                Pointer address = new Pointer((UInt32)Core.CurrentROMSize);
                Pointer endbyte = new Pointer();
                foreach (DataGridViewRow row in WriteHistoryList.SelectedRows)
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
            if (WriteHistoryList.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There are no writes selected.");
            }
            else if (WriteHistoryList.SelectedRows.Count == 1)
            {
                Pointer address = (Pointer)WriteHistoryList.SelectedRows[0].Cells[1].Value;
                App.MHF.Space.MarkSpace((String)MarkSpaceBox.SelectedValue,
                    address, address + (Int32)WriteHistoryList.SelectedRows[0].Cells[2].Value);
                UI.PerformUpdate();
            }
            else
            {
                foreach (DataGridViewRow row in WriteHistoryList.SelectedRows)
                {
                    Pointer address = (Pointer)row.Cells[1].Value;
                    App.MHF.Space.MarkSpace((String)MarkSpaceBox.SelectedValue,
                        address, address + (Int32)row.Cells[2].Value);
                }
                UI.PerformUpdate();
            }
        }
    }
}
