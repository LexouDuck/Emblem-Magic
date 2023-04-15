using GBA;
using Library.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Magic.Editors
{
    public partial class PatchEditor : Editor
    {
        List<HackManager> Patches { get; set; }

        public SortableBindingList<Tuple<
            String,   // Patch Author
            String,   // Patch Name
            Boolean>> // Whether or not the patch is applied to the current ROM
            List { get; set; }



        public PatchEditor()
        {
            try
            {
                this.Patches = new List<HackManager>();
                this.List = new SortableBindingList<Tuple<
                String,
                String,
                Boolean>>();

                this.InitializeComponent();

                this.Patch_DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                this.Patch_DataGrid.AutoGenerateColumns = false;
                this.Patch_DataGrid.DataSource = this.List;

                this.Patch_Column_Author.DataPropertyName = "Item1";
                this.Patch_Column_PatchName.DataPropertyName = "Item2";
                this.Patch_Column_Applied.DataPropertyName = "Item3";
            }
            catch (Exception ex)
            {
                UI.ShowError("Could not properly open the " + this.Text, ex);

                this.Core_CloseEditor(this, null);
            }
        }
        
        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            try
            {
                this.List.Clear();
                this.Patches.Clear();
                String[] files = Directory.GetFiles(Core.Path_Patches);

                foreach (String file in files)
                {
                    if (file.EndsWith(".mhf", StringComparison.OrdinalIgnoreCase))
                    {
                        HackManager patch = new HackManager(this.App);
                        patch.OpenFile(file);
                        this.Patches.Add(patch);

                        this.List.Add(Tuple.Create(
                            patch.HackAuthor,
                            patch.HackName,
                            patch.IsApplied()));
                    }
                }
            }
            catch (Exception ex)
            {
                UI.ShowError("The patch list could not be loaded onscreen.", ex);
            }
        }

        public void Core_ApplyPatch(HackManager patch)
        {
            UI.SuspendUpdate();

            List<Write> repointables = new List<Write>();

            foreach (Write write in patch.Write.History)
            {
                if (write.Address == new Pointer())
                {
                    repointables.Add(write);
                }
                else
                {
                    Core.WriteData(this, write.Address, write.Data, write.Phrase);
                }
            }
            foreach (Repoint repoint in patch.Point.Repoints)
            {
                if (repoint.CurrentAddress == new Pointer())
                {
                    Write write = null;
                    for (Int32 i = 0; i < repointables.Count; i++)
                    {
                        if (repoint.AssetName == repointables[i].Phrase)
                        {
                            write = repointables[i];
                            repointables.RemoveAt(i);
                            break;
                        }
                    }
                    if (write == null) continue;
                    Pointer address = Prompt.ShowPointerDialog(
                        "Please enter a pointer for " + repoint.AssetName,
                        "Patch Repoint");

                    Core.WriteData(this, address, write.Data, write.Phrase);
                    Core.Repoint(this, repoint.DefaultAddress, address, repoint.AssetName + " repoint");
                }
                // else Core.Repoint(this, repoint.DefaultAddress, repoint.CurrentAddress, repoint.AssetName + " repoint");

                this.App.MHF.Point.Add(repoint);
            }

            UI.ResumeUpdate();
            UI.PerformUpdate();
        }


        private void PatchesDataGrid_Click(Object sender, EventArgs e)
        {
            if (this.Patch_DataGrid.SelectedRows.Count == 1)
            {
                String patchName = (String)this.Patch_DataGrid.SelectedRows[0].Cells[1].Value;
                foreach (HackManager patch in this.Patches)
                {
                    if (patchName.Equals(patch.HackName))
                    {
                        this.Patch_Description_Label.Text = patch.HackDescription; break;
                    }
                }
            }
            else
            {
                this.Patch_Description_Label.Text = "";
            }
        }

        private void RefreshButton_Click(Object sender, EventArgs e)
        {
            this.Core_Update();
        }
        private void ApplyButton_Click(Object sender, EventArgs e)
        {
            if (this.Patch_DataGrid.SelectedRows.Count == 0)
            {
                UI.ShowMessage("There is no patch currently selected.");
            }
            else if (this.Patch_DataGrid.SelectedRows.Count == 1)
            {
                String patchName = (String)this.Patch_DataGrid.SelectedRows[0].Cells[1].Value;

                foreach (HackManager patch in this.Patches)
                {
                    if (patchName.Equals(patch.HackName))
                    {
                        this.Core_ApplyPatch(patch);
                        break;
                    }
                }
            }
            else
            {
                String[] patchNames = new String[this.Patch_DataGrid.SelectedRows.Count];
                for (Int32 i = 0; i < patchNames.Length; i++)
                {
                    patchNames[i] = (String)this.Patch_DataGrid.SelectedRows[i].Cells[1].Value;
                }

                foreach (HackManager patch in this.Patches)
                {
                    for (Int32 i = 0; i < patchNames.Length; i++)
                    {
                        if (patchNames[i].Equals(patch.HackName))
                        {
                            this.Core_ApplyPatch(patch);
                            break;
                        }
                    }
                }
            }
        }
    }
}