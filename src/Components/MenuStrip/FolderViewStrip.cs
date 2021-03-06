﻿using EmblemMagic.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace EmblemMagic.Components
{
    public class FolderViewMenuItem : ToolStripMenuItem
    {
        /// <summary>
        /// The path + filename and extension the file in question.
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Gets the text to display on the MenuItem
        /// </summary>
        public override string Text
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }

        public FolderViewMenuItem(string filepath)
        {
            FilePath = filepath;
        }
    }



    public class FolderViewMenu : ToolStripMenuItem
    {
        public FolderViewMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads the files located at the given path into this FolderViewMenu
        /// </summary>
        public void LoadFiles(string path, string extension = null)
        {
            string[] files = Directory.GetFiles(path);

            if (extension == null)
            {
                foreach (string file in files)
                {
                    DropDownItems.Add(new FolderViewMenuItem(file));
                }
            }
            else
            {
                foreach (string file in files)
                {
                    if (file.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                       DropDownItems.Add(new FolderViewMenuItem(file));
                }
            }

            this.Enabled = (DropDownItems.Count > 0);
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
