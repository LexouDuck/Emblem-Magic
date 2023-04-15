using Magic.Components;
using Magic.Properties;
using System;
using System.Windows.Forms;

namespace Magic
{
    public partial class FormOptions : Form
    {
        IApp App;

        public FormOptions(IApp app)
        {
            this.App = app;

            this.InitializeComponent();

            this.LoadOptions();
        }
        void LoadOptions()
        {
            this.Paths_CustomArrays_CheckBox.Checked = Settings.Default.UseCustomPathArrays;
            this.Paths_CustomArrays_TextBox.Text = Settings.Default.PathCustomArrays;
            this.Paths_CustomStructs_CheckBox.Checked = Settings.Default.UseCustomPathStructs;
            this.Paths_CustomStructs_TextBox.Text = Settings.Default.PathCustomStructs;
            this.Paths_Arrays_TextBox.Text = Settings.Default.PathArrays;
            this.Paths_Structs_TextBox.Text = Settings.Default.PathStructs;
            this.Paths_CleanROMs_TextBox.Text = Settings.Default.PathCleanROMs;
            this.Paths_Modules_TextBox.Text = Settings.Default.PathModules;
            this.Paths_Patches_TextBox.Text = Settings.Default.PathPatches;

            this.Overwrite_TrackBar.Value = 0;
            if (Settings.Default.WarnOverwriteSeveral) this.Overwrite_TrackBar.Value++;
            if (Settings.Default.WarnOverwriteSizeDiffer) this.Overwrite_TrackBar.Value++;
            if (Settings.Default.WarnOverwrite) this.Overwrite_TrackBar.Value++;
            this.Overwrite_TrackBar_ValueChanged(this, null);

            this.Repoint_TrackBar.Value = 0;
            if (Settings.Default.PromptRepoints) this.Repoint_TrackBar.Value = 1;
            if (Settings.Default.WriteToFreeSpace) this.Repoint_TrackBar.Value = 2;
            this.Repoint_TrackBar_ValueChanged(this, null);

            this.RecentFiles_MaxNumBox.Maximum = RecentFileMenu.Maximum;
            this.RecentFiles_MaxNumBox.Value = Settings.Default.RecentFilesMax;

            this.UndoAndRedo_MaxNumBox.Maximum = 100;
            this.UndoAndRedo_MaxNumBox.Value = Settings.Default.UndoListMax;
        }



        void Options_ApplyButton_Click(Object sender, EventArgs e)
        {
            Settings.Default.UseCustomPathArrays = this.Paths_CustomArrays_CheckBox.Checked;
            Settings.Default.PathCustomArrays = this.Paths_CustomArrays_TextBox.Text;
            Settings.Default.UseCustomPathStructs = this.Paths_CustomStructs_CheckBox.Checked;
            Settings.Default.PathCustomStructs = this.Paths_CustomStructs_TextBox.Text;
            Settings.Default.PathArrays = this.Paths_Arrays_TextBox.Text;
            Settings.Default.PathStructs = this.Paths_Structs_TextBox.Text;
            Settings.Default.PathCleanROMs = this.Paths_CleanROMs_TextBox.Text;
            Settings.Default.PathModules = this.Paths_Modules_TextBox.Text;
            Settings.Default.PathPatches = this.Paths_Patches_TextBox.Text;

            switch (this.Overwrite_TrackBar.Value)
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
            switch (this.Repoint_TrackBar.Value)
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
            Settings.Default.RecentFilesMax = (Int32)this.RecentFiles_MaxNumBox.Value;
            Settings.Default.UndoListMax = (Int32)this.UndoAndRedo_MaxNumBox.Value;
            Settings.Default.PreferIndexedBMP = this.PreferIndexedBMP_CheckBox.Checked;

            Settings.Default.Save();
            this.DialogResult = DialogResult.Yes;
        }
        void Options_CancelButton_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }



        private void Paths_CustomArrays_BrowseButton_Click(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                this.Paths_CustomArrays_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_CustomStructs_BrowseButton_Click(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                this.Paths_CustomStructs_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_Arrays_BrowseButton_Click(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                this.Paths_Arrays_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_Structs_BrowseButton_Click(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                this.Paths_Structs_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_CleanROMs_BrowseButton_Click(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                this.Paths_CleanROMs_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_Modules_BrowseButton_Click(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                this.Paths_Modules_TextBox.Text = folderWindow.SelectedPath;
            }
        }
        private void Paths_Patches_BrowseButton_Click(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderWindow = new FolderBrowserDialog();
            if (folderWindow.ShowDialog() == DialogResult.OK)
            {
                this.Paths_Patches_TextBox.Text = folderWindow.SelectedPath;
            }
        }

        private void RecentFiles_ClearButton_Click(Object sender, EventArgs e)
        {
            this.App.File_RecentFiles.Clear();
        }

        private void Overwrite_TrackBar_ValueChanged(Object sender, EventArgs e)
        {
            switch (this.Overwrite_TrackBar.Value)
            {
                case 0:
                    this.Overwrite_Label.Text = "Never prompt on overwrite.";
                    break;
                case 1:
                    this.Overwrite_Label.Text = "Only prompt when several writes are affected.";
                    break;
                case 2:
                    this.Overwrite_Label.Text = "Prompt when overwrite has different size than the preexisting write.";
                    break;
                case 3:
                    this.Overwrite_Label.Text = "Always prompt when overwriting.";
                    break;
                default: this.Overwrite_Label.Text = ""; break;
            }
        }
        private void Repoint_TrackBar_ValueChanged(Object sender, EventArgs e)
        {
            switch (this.Repoint_TrackBar.Value)
            {
                case 0:
                    this.Repoint_Label.Text = "Never prompt to repoint data upon insertion.";
                    break;
                case 1:
                    this.Repoint_Label.Text = "Always show a repoint prompt when inserting data.";
                    break;
                case 2:
                    this.Repoint_Label.Text = "Never show repoint prompts, automatically repoint to free space.";
                    break;
                default: this.Repoint_Label.Text = ""; break;
            }
        }
    }
}
