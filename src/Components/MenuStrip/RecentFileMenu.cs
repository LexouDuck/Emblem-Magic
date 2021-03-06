﻿using EmblemMagic.Properties;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace EmblemMagic.Components
{
    public sealed class RecentFileMenuItem : ToolStripMenuItem
    {
        /// <summary>
        /// The full path + filename and extension the file in question.
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Gets the text to display on the MenuItem
        /// </summary>
        public override string Text
        {
            get
            {
                ToolStripMenuItem parent = (ToolStripMenuItem)this.OwnerItem;
                int index = parent.DropDownItems.IndexOf(this);
                return (index + 1) + " - " + FileName;
            }
        }

        public RecentFileMenuItem(string fileName)
        {
            FileName = fileName;
        }
    }



    public sealed class RecentFileMenu : ToolStripMenuItem
    {
        public const int Maximum = 25;
        
        public RecentFileMenu()
        {
            InitializeComponent();

            Settings.Default.PropertyChanged += new PropertyChangedEventHandler(Settings_Changed);

            ReloadItems();
        }
        
        /// <summary>
        /// clears and repopulates the list of items for this RecentFileMenu
        /// </summary>
        public void ReloadItems()
        {
            if (Settings.Default.RecentFiles == null)
                Settings.Default.RecentFiles = new StringCollection();

            Enabled = (Settings.Default.RecentFiles.Count > 0);

            DropDownItems.Clear();

            int count = Math.Min(Settings.Default.RecentFilesMax, Settings.Default.RecentFiles.Count);
            for(int i = 0; i < count; i++)
            {
                DropDownItems.Add(new RecentFileMenuItem(Settings.Default.RecentFiles[i]));
            }
        }
        /// <summary>
        /// Clears this RecentFileMenu
        /// </summary>
        public void Clear()
        {
            Settings.Default.RecentFiles.Clear();
            ReloadItems();
        }
        
        /// <summary>
        /// Adds a file to this RecentFileMenu
        /// </summary>
        public void AddFile(string fileName)
        {
            // check if the file is already in the collection
            int alreadyIn = GetIndex(fileName);
            if (alreadyIn > 0) // remove it
            {
                Settings.Default.RecentFiles.RemoveAt(alreadyIn);
                if (DropDownItems.Count > alreadyIn)
                    DropDownItems.RemoveAt(alreadyIn);
            }
            else if (alreadyIn == 0) // it´s the latest file so return
            {
                return;
            }

            if (!this.Enabled)
                this.Enabled = true;

            // insert the file on top of the list
            Settings.Default.RecentFiles.Insert(0, fileName);
            DropDownItems.Insert(0, new RecentFileMenuItem(fileName));

            // remove the last one, if max size is reached
            if (Settings.Default.RecentFiles.Count > Maximum)
                Settings.Default.RecentFiles.RemoveAt(Maximum);
            if (Settings.Default.RecentFiles.Count > Settings.Default.RecentFilesMax &&
                DropDownItems.Count > Settings.Default.RecentFilesMax)
                DropDownItems.RemoveAt(Settings.Default.RecentFilesMax);

            // save the changes
            Settings.Default.Save();
        }
        /// <summary>
        /// Gets the index of the filename in this RecentFileMenu, or -1 if not found
        /// </summary>
        int GetIndex(string fileName)
        {
            for (int i = 0; i < Settings.Default.RecentFiles.Count; i++)
            {
                string currentFile = Settings.Default.RecentFiles[i];
                if (string.Equals(currentFile, fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }
        
        void Settings_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RecentFilesMax")
            {
                ReloadItems();
            }
        }
        
        #region Component Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}
