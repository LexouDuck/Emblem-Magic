using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    public interface IArrayBox
    {
        void Load(ArrayFile file);

        event EventHandler ValueChanged;
    }

    public abstract partial class ArrayBox<T> : Panel, IArrayBox
    {
        /// <summary>
        /// The list of entries to read from
        /// </summary>
        public ArrayFile File;


        
        /// <summary>
        /// Gets or sets this ArrayBox's NumberBox value.
        /// </summary>
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        public abstract T Value
        {
            get; set;
        }
        /// <summary>
        /// Gets or sets this ArrayBox's ComboBox text
        /// </summary>
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        override public String Text
        {
            get
            {
                if (this.EntryComboBox.SelectedItem == null) return "";
                KeyValuePair<UInt32, String> current = (KeyValuePair<UInt32, String>)this.EntryComboBox.SelectedItem;
                return current.Value;
            }
            set
            {
                this.EntryComboBox.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets this ArrayBox's NumberBox's name value
        /// </summary>
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        new public String Name
        {
            get
            {
                return this.EntryValueBox.Name;
            }
            set
            {
                this.EntryValueBox.Name = value;
            }
        }
        /// <summary>
        /// Gets or sets this ArrayBox's NumberBox's maximum value
        /// </summary>
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        public Int32 Maximum
        {
            get
            {
                return (Int32)this.EntryValueBox.Maximum;
            }
            set
            {
                this.EntryValueBox.Maximum = value - 1;
            }
        }



        /// <summary>
        /// Loads the file at 'path' to display list entry names
        /// </summary>
        public void Load(String path)
        {
            this.Load(new ArrayFile(path));
        }
        public void Load(ArrayFile file)
        {
            this.File = file;

            Int32 longestString = 0;
            for (UInt32 i = 0; i <= this.File.LastEntry; i++)
            {
                if (this.File[i].Length > longestString)
                    longestString = this.File[i].Length;
            }

            if (this.AutoSize)
            {
                this.Size = new Size(this.EntryValueBox.Width + 30 + longestString * 5, 26);
                this.EntryComboBox.Width = this.Width - this.EntryValueBox.Width - 10;
                this.AutoSize = false;
            }

            //if (File.LastEntry != 0) EntryValueBox.Maximum = File.LastEntry;

            this.EntryValueBox.MouseWheel += delegate (Object sender, MouseEventArgs e)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            };
            this.EntryComboBox.DataSource = this.File.ToList();
            this.EntryComboBox.ValueMember = "Key";
            this.EntryComboBox.DisplayMember = "Value";
            this.EntryComboBox.SelectedValueChanged += this.UpdateEntryValueBox;
            this.EntryComboBox.TextUpdate += this.UpdateArrayFileText;
            this.EntryComboBox.MouseWheel += delegate (Object sender, MouseEventArgs e)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            };

            this.UpdateEntryComboBox(this, null);
        }

        public void SelectFirstItem()
        {
            this.EntryComboBox.SelectedIndex = 0;
            if (this.EntryValueBox.Value == 0 && this.EntryComboBox.Items.Count > 1)
            {
                this.EntryComboBox.SelectedIndex = 1;
            }
        }



        public event EventHandler ValueChanged
        {
            add
            {
                this.EntryValueBox.ValueChanged += value;
            }
            remove
            {
                this.EntryValueBox.ValueChanged -= value;
            }
        }



        protected void UpdateEntryValueBox(Object sender, EventArgs e)
        {
            this.EntryValueBox.ValueChanged -= this.UpdateEntryComboBox;

            try
            {
                UInt32 value = ((KeyValuePair<UInt32, String>)this.EntryComboBox.SelectedItem).Key;
                this.EntryValueBox.Value = value;
            }
            catch
            {
                this.EntryComboBox.SelectedItem = null;
            }

            this.EntryValueBox.ValueChanged += this.UpdateEntryComboBox;
        }
        protected void UpdateEntryComboBox(Object sender, EventArgs e)
        {
            this.EntryComboBox.SelectedValueChanged -= this.UpdateEntryValueBox;

            try
            {
                this.EntryComboBox.SelectedValue = (UInt32)this.EntryValueBox.Value;
            }
            catch
            {
                this.EntryComboBox.SelectedItem = null;
            }

            this.EntryComboBox.SelectedValueChanged += this.UpdateEntryValueBox;
        }

        protected void UpdateArrayFileText(Object sender, EventArgs e)
        {
            if (Prompt.UpdateArrayFile() == DialogResult.Yes)
            {
                this.File.RenameEntry((UInt32)this.EntryValueBox.Value, this.EntryComboBox.Text);

                this.EntryComboBox.DataSource = this.File.ToList();
            }
        }
    }
}
