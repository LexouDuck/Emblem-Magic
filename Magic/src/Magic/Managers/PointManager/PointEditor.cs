using Magic.Editors;
using GBA;
using Library.Forms;
using System;
using System.Windows.Forms;

namespace Magic
{
    public partial class PointEditor : Editor
    {
        public SortableBindingList<Tuple<
            String,
            Boolean,
            Pointer,
            Int32>> List { get; set; }

        public PointEditor()
        {
            this.List = new SortableBindingList<Tuple<
                String,
                Boolean,
                Pointer,
                Int32>>();

            this.InitializeComponent();

            this.PointerDataGrid.AutoGenerateColumns = false;
            this.PointerDataGrid.DataSource = this.List;

            this.AssetColumn.DataPropertyName      = "Item1";
            this.RepointedColumn.DataPropertyName  = "Item2";
            this.AddressColumn.DataPropertyName    = "Item3";
            this.ReferencesColumn.DataPropertyName = "Item4";
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

                if (this.ShowHideCheckBox.Checked)
                {
                    foreach (var repoint in this.App.MHF.Point.Repoints)
                    {
                        if (repoint.DefaultAddress != repoint.CurrentAddress)
                        {
                            this.List.Add(Tuple.Create(
                                repoint.AssetName,
                                true,
                                repoint.CurrentAddress,
                                repoint.References.Length));
                        }
                    }
                }
                else
                {
                    foreach (var repoint in this.App.MHF.Point.Repoints)
                    {
                        this.List.Add(Tuple.Create(
                            repoint.AssetName,
                            repoint.DefaultAddress != repoint.CurrentAddress,
                            repoint.CurrentAddress,
                            repoint.References.Length));
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("The pointer list could not be loaded onscreen.", ex);
            }
        }
        void Update_EditPanel(Repoint selection)
        {
            if (selection == null)
            {
                this.ReferenceListBox.DataSource = null;
                this.Repoint_NameTextBox.Text = "";
                this.Repoint_DefaultPointerBox.Value = new Pointer();
                this.Repoint_CurrentPointerBox.Value = new Pointer();
            }
            else
            {
                this.ReferenceListBox.DataSource = selection.References;
                this.Repoint_NameTextBox.Text = selection.AssetName;
                this.Repoint_DefaultPointerBox.Value = selection.DefaultAddress;
                this.Repoint_CurrentPointerBox.Value = selection.CurrentAddress;
            }
        }



        private void SelectPointer_Click(Object sender, EventArgs e)
        {
            if (this.PointerDataGrid.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no pointer selected.");
            }
            else if (this.PointerDataGrid.SelectedRows.Count == 1)
            {
                String asset = (String)this.PointerDataGrid.CurrentRow.Cells[0].Value;

                this.Update_EditPanel(this.App.MHF.Point.Get(asset));
            }
            else
            {
                this.Update_EditPanel(null);
            }
        }

        private void RepointButton_Click(Object sender, EventArgs e)
        {
            if (this.PointerDataGrid.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no pointer selected.");
            }
            else if (this.PointerDataGrid.SelectedRows.Count == 1)
            {
                Repoint asset = this.App.MHF.Point.Get((String)this.PointerDataGrid.CurrentRow.Cells[0].Value);
                asset.AssetName = this.Repoint_NameTextBox.Text;
                Core.Repoint(this, asset.CurrentAddress, this.Repoint_CurrentPointerBox.Value, asset.AssetName + " repointed");
            }
            else
            {
                UI.ShowMessage("You cannot modify several asset pointers at once.");
            }
            this.Core_Update();
        }
        private void CreateButton_Click(Object sender, EventArgs e)
        {
            Repoint repoint = Prompt.ShowRepointCreateDialog();

            this.App.MHF.Point.Add(repoint);

            if (repoint.CurrentAddress != repoint.DefaultAddress)
            {
                Core.Repoint(this, repoint.CurrentAddress, repoint.CurrentAddress, repoint.AssetName + " repoint");
            }
            this.Core_Update();
        }
        private void DeleteButton_Click(Object sender, EventArgs e)
        {
            if (this.PointerDataGrid.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no pointer selected.");
            }
            else if (this.PointerDataGrid.SelectedRows.Count == 1)
            {
                this.App.MHF.Point.Remove((String)this.PointerDataGrid.CurrentRow.Cells[0].Value);
            }
            else foreach (DataGridViewRow row in this.PointerDataGrid.SelectedRows)
            {
                    this.App.MHF.Point.Remove((String)row.Cells[0].Value);
            }
            this.Core_Update();
        }

        private void ShowHideCheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
    }
}
