using EmblemMagic.Components;
using EmblemMagic.Properties;
using System;
using System.Windows.Forms;

namespace EmblemMagic
{
    public partial class FormOptions : Form
    {
        public FormOptions()
        {
            InitializeComponent();

            LoadOptions();
        }
        void LoadOptions()
        {
            Paths_CustomArrays_CheckBox.Checked = Settings.Default.UseCustomPathArrays;
            Paths_CustomArrays_TextBox.Text = Settings.Default.PathCustomArrays;
            Paths_CustomStructs_CheckBox.Checked = Settings.Default.UseCustomPathStructs;
            Paths_CustomStructs_TextBox.Text = Settings.Default.PathCustomStructs;
            Paths_Arrays_TextBox.Text = Settings.Default.PathArrays;
            Paths_Structs_TextBox.Text = Settings.Default.PathStructs;
            Paths_CleanROMs_TextBox.Text = Settings.Default.PathCleanROMs;
            Paths_Modules_TextBox.Text = Settings.Default.PathModules;
            Paths_Patches_TextBox.Text = Settings.Default.PathPatches;

            Overwrite_TrackBar.Value = 0;
            if (Settings.Default.WarnOverwriteSeveral)    Overwrite_TrackBar.Value++;
            if (Settings.Default.WarnOverwriteSizeDiffer) Overwrite_TrackBar.Value++;
            if (Settings.Default.WarnOverwrite)           Overwrite_TrackBar.Value++;
            Overwrite_TrackBar_ValueChanged(this, null);

            Repoint_TrackBar.Value = 0;
            if (Settings.Default.PromptRepoints)   Repoint_TrackBar.Value = 1;
            if (Settings.Default.WriteToFreeSpace) Repoint_TrackBar.Value = 2;
            Repoint_TrackBar_ValueChanged(this, null);

            RecentFiles_MaxNumBox.Maximum = RecentFileMenu.Maximum;
            RecentFiles_MaxNumBox.Value = Settings.Default.RecentFilesMax;

            UndoAndRedo_MaxNumBox.Maximum = 100;
            UndoAndRedo_MaxNumBox.Value = Settings.Default.UndoListMax;
        }



        void Options_ApplyButton_Click(object sender, EventArgs e)
        {
            Settings.Default.UseCustomPathArrays = Paths_CustomArrays_CheckBox.Checked;
            Settings.Default.PathCustomArrays = Paths_CustomArrays_TextBox.Text;
            Settings.Default.UseCustomPathStructs = Paths_CustomStructs_CheckBox.Checked;
            Settings.Default.PathCustomStructs = Paths_CustomStructs_TextBox.Text;
            Settings.Default.PathArrays = Paths_Arrays_TextBox.Text;
            Settings.Default.PathStructs = Paths_Structs_TextBox.Text;
            Settings.Default.PathCleanROMs = Paths_CleanROMs_TextBox.Text;
            Settings.Default.PathModules = Paths_Modules_TextBox.Text;
            Settings.Default.PathPatches = Paths_Patches_TextBox.Text;

            switch (Overwrite_TrackBar.Value)
            {
                case 0:
                    Settings.Default.WarnOverwriteSeveral    = false;
                    Settings.Default.WarnOverwriteSizeDiffer = false;
                    Settings.Default.WarnOverwrite           = false;
                    break;
                case 1:
                    Settings.Default.WarnOverwriteSeveral    = true;
                    Settings.Default.WarnOverwriteSizeDiffer = false;
                    Settings.Default.WarnOverwrite           = false;
                    break;
                case 2:
                    Settings.Default.WarnOverwriteSeveral    = true;
                    Settings.Default.WarnOverwriteSizeDiffer = true;
                    Settings.Default.WarnOverwrite           = false;
                    break;
                case 3:
                    Settings.Default.WarnOverwriteSeveral    = true;
                    Settings.Default.WarnOverwriteSizeDiffer = true;
                    Settings.Default.WarnOverwrite           = true;
                    break;
                default: break;
            }
            switch (Repoint_TrackBar.Value)
            {
                case 0:
                    Settings.Default.PromptRepoints   = false;
                    Settings.Default.WriteToFreeSpace = false;
                    break;
                case 1:
                    Settings.Default.PromptRepoints   = true;
                    Settings.Default.WriteToFreeSpace = false;
                    break;
                case 2:
                    Settings.Default.PromptRepoints   = false;
                    Settings.Default.WriteToFreeSpace = true;
                    break;
                default: break;
            }
            Settings.Default.RecentFilesMax = (int)RecentFiles_MaxNumBox.Value;
            Settings.Default.UndoListMax = (int)UndoAndRedo_MaxNumBox.Value;
            Settings.Default.PreferIndexedBMP = PreferIndexedBMP_CheckBox.Checked;

            Settings.Default.Save();
            this.DialogResult = DialogResult.Yes;
        }
        void Options_CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }



        private void Paths_CustomArrays_BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                Paths_CustomArrays_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_CustomStructs_BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                Paths_CustomStructs_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_Arrays_BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                Paths_Arrays_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_Structs_BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                Paths_Structs_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_CleanROMs_BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                Paths_CleanROMs_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_Modules_BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                Paths_Modules_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_Patches_BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                Paths_Patches_TextBox.Text = folderWindow.SelectedPath;
            }
        }

        private void RecentFiles_ClearButton_Click(object sender, EventArgs e)
        {
            Program.Core.File_RecentFiles.Clear();
        }

        private void Overwrite_TrackBar_ValueChanged(Object sender, EventArgs e)
        {
            switch (Overwrite_TrackBar.Value)
            {
                case 0:
                    Overwrite_Label.Text = "Never prompt on overwrite.";
                    break;
                case 1:
                    Overwrite_Label.Text = "Only prompt when several writes are affected.";
                    break;
                case 2:
                    Overwrite_Label.Text = "Prompt when overwrite has different size than the preexisting write.";
                    break;
                case 3:
                    Overwrite_Label.Text = "Always prompt when overwriting.";
                    break;
                default: Overwrite_Label.Text = ""; break;
            }
        }
        private void Repoint_TrackBar_ValueChanged(Object sender, EventArgs e)
        {
            switch (Repoint_TrackBar.Value)
            {
                case 0:
                    Repoint_Label.Text = "Never prompt to repoint data upon insertion.";
                    break;
                case 1:
                    Repoint_Label.Text = "Always show a repoint prompt when inserting data.";
                    break;
                case 2:
                    Repoint_Label.Text = "Never show repoint prompts, automatically repoint to free space.";
                    break;
                default: Repoint_Label.Text = ""; break;
            }
        }
    }
}
