using EmblemMagic.Editors;
using GBA;
using Library.Forms;
using System;
using System.Windows.Forms;

namespace EmblemMagic
{
    public partial class PointEditor : Editor
    {
        public SortableBindingList<Tuple<
            String,
            Boolean,
            Pointer,
            int>> List { get; set; }

        public PointEditor()
        {
            List = new SortableBindingList<Tuple<
                String,
                Boolean,
                Pointer,
                int>>();

            InitializeComponent();

            PointerDataGrid.AutoGenerateColumns = false;
            PointerDataGrid.DataSource = this.List;

            AssetColumn.DataPropertyName      = "Item1";
            RepointedColumn.DataPropertyName  = "Item2";
            AddressColumn.DataPropertyName    = "Item3";
            ReferencesColumn.DataPropertyName = "Item4";
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

                if (ShowHideCheckBox.Checked)
                {
                    foreach (var repoint in Program.Core.FEH.Point.Repoints)
                    {
                        if (repoint.DefaultAddress != repoint.CurrentAddress)
                        {
                            List.Add(Tuple.Create(
                                repoint.AssetName,
                                true,
                                repoint.CurrentAddress,
                                repoint.References.Length));
                        }
                    }
                }
                else
                {
                    foreach (var repoint in Program.Core.FEH.Point.Repoints)
                    {
                        List.Add(Tuple.Create(
                            repoint.AssetName,
                            repoint.DefaultAddress != repoint.CurrentAddress,
                            repoint.CurrentAddress,
                            repoint.References.Length));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("The pointer list could not be loaded onscreen.", ex);
            }
        }
        void Update_EditPanel(Repoint selection)
        {
            if (selection == null)
            {
                ReferenceListBox.DataSource = null;
                Repoint_NameTextBox.Text = "";
                Repoint_DefaultPointerBox.Value = new Pointer();
                Repoint_CurrentPointerBox.Value = new Pointer();
            }
            else
            {
                ReferenceListBox.DataSource = selection.References;
                Repoint_NameTextBox.Text = selection.AssetName;
                Repoint_DefaultPointerBox.Value = selection.DefaultAddress;
                Repoint_CurrentPointerBox.Value = selection.CurrentAddress;
            }
        }



        private void SelectPointer_Click(object sender, EventArgs e)
        {
            if (PointerDataGrid.SelectedRows.Count == 0)
            {
                Program.ShowMessage("There is no pointer selected.");
            }
            else if (PointerDataGrid.SelectedRows.Count == 1)
            {
                String asset = (string)PointerDataGrid.CurrentRow.Cells[0].Value;

                Update_EditPanel(Program.Core.FEH.Point.Get(asset));
            }
            else
            {
                Update_EditPanel(null);
            }
        }

        private void RepointButton_Click(object sender, EventArgs e)
        {
            if (PointerDataGrid.SelectedRows.Count == 0)
            {
                Program.ShowMessage("There is no pointer selected.");
            }
            else if (PointerDataGrid.SelectedRows.Count == 1)
            {
                Repoint asset = Program.Core.FEH.Point.Get((string)PointerDataGrid.CurrentRow.Cells[0].Value);
                asset.AssetName = Repoint_NameTextBox.Text;
                Core.Repoint(this, asset.CurrentAddress, Repoint_CurrentPointerBox.Value, asset.AssetName + " repointed");
            }
            else
            {
                Program.ShowMessage("You cannot modify several asset pointers at once.");
            }
            Core_Update();
        }
        private void CreateButton_Click(object sender, EventArgs e)
        {
            Repoint repoint = Prompt.ShowRepointCreateDialog();

            Program.Core.FEH.Point.Add(repoint);

            if (repoint.CurrentAddress != repoint.DefaultAddress)
            {
                Core.Repoint(this, repoint.CurrentAddress, repoint.CurrentAddress, repoint.AssetName + " repoint");
            }
            Core_Update();
        }
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (PointerDataGrid.SelectedRows.Count == 0)
            {
                Program.ShowMessage("There is no pointer selected.");
            }
            else if (PointerDataGrid.SelectedRows.Count == 1)
            {
                Program.Core.FEH.Point.Remove((string)PointerDataGrid.CurrentRow.Cells[0].Value);
            }
            else foreach (DataGridViewRow row in PointerDataGrid.SelectedRows)
            {
                Program.Core.FEH.Point.Remove((string)row.Cells[0].Value);
            }
            Core_Update();
        }

        private void ShowHideCheckBox_CheckedChanged(Object sender, EventArgs e)
        {
            Core_Update();
        }
    }
}
