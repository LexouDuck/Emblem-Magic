using GBA;
using Library.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Editors
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
                Patches = new List<HackManager>();
                List = new SortableBindingList<Tuple<
                String,
                String,
                Boolean>>();

                InitializeComponent();

                Patch_DataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                Patch_DataGrid.AutoGenerateColumns = false;
                Patch_DataGrid.DataSource = this.List;

                Patch_Column_Author.DataPropertyName = "Item1";
                Patch_Column_PatchName.DataPropertyName = "Item2";
                Patch_Column_Applied.DataPropertyName = "Item3";
            }
            catch (Exception ex)
            {
                Program.ShowError("Could not properly open the " + this.Text, ex);

                Core_CloseEditor(this, null);
            }
        }
        
        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            try
            {
                List.Clear();
                Patches.Clear();
                string[] files = Directory.GetFiles(Core.Path_Patches);

                foreach (string file in files)
                {
                    if (file.EndsWith(".feh", StringComparison.OrdinalIgnoreCase))
                    {
                        HackManager patch = new HackManager();
                        patch.OpenFile(file);
                        Patches.Add(patch);

                        List.Add(Tuple.Create(
                            patch.HackAuthor,
                            patch.HackName,
                            patch.IsApplied()));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.ShowError("The patch list could not be loaded onscreen.", ex);
            }
        }

        public void Core_ApplyPatch(HackManager patch)
        {
            Core.SuspendUpdate();

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
                    for (int i = 0; i < repointables.Count; i++)
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

                Program.Core.FEH.Point.Add(repoint);
            }

            Core.ResumeUpdate();
            Core.PerformUpdate();
        }


        private void PatchesDataGrid_Click(object sender, EventArgs e)
        {
            if (Patch_DataGrid.SelectedRows.Count == 1)
            {
                string patchName = (string)Patch_DataGrid.SelectedRows[0].Cells[1].Value;
                foreach (HackManager patch in Patches)
                {
                    if (patchName.Equals(patch.HackName))
                    {
                        Patch_Description_Label.Text = patch.HackDescription; break;
                    }
                }
            }
            else
            {
                Patch_Description_Label.Text = "";
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Core_Update();
        }
        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (Patch_DataGrid.SelectedRows.Count == 0)
            {
                Program.ShowMessage("There is no patch currently selected.");
            }
            else if (Patch_DataGrid.SelectedRows.Count == 1)
            {
                string patchName = (string)Patch_DataGrid.SelectedRows[0].Cells[1].Value;

                foreach (HackManager patch in Patches)
                {
                    if (patchName.Equals(patch.HackName))
                    {
                        Core_ApplyPatch(patch);
                        break;
                    }
                }
            }
            else
            {
                string[] patchNames = new string[Patch_DataGrid.SelectedRows.Count];
                for (int i = 0; i < patchNames.Length; i++)
                {
                    patchNames[i] = (string)Patch_DataGrid.SelectedRows[i].Cells[1].Value;
                }

                foreach (HackManager patch in Patches)
                {
                    for (int i = 0; i < patchNames.Length; i++)
                    {
                        if (patchNames[i].Equals(patch.HackName))
                        {
                            Core_ApplyPatch(patch);
                            break;
                        }
                    }
                }
            }
        }
    }
}