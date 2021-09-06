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
                if (EntryComboBox.SelectedItem == null) return "";
                KeyValuePair<UInt32, String> current = (KeyValuePair<UInt32, String>)EntryComboBox.SelectedItem;
                return current.Value;
            }
            set
            {
                EntryComboBox.Text = value;
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
                return EntryValueBox.Name;
            }
            set
            {
                EntryValueBox.Name = value;
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
                return (Int32)EntryValueBox.Maximum;
            }
            set
            {
                EntryValueBox.Maximum = value - 1;
            }
        }



        /// <summary>
        /// Loads the file at 'path' to display list entry names
        /// </summary>
        public void Load(String path)
        {
            Load(new ArrayFile(path));
        }
        public void Load(ArrayFile file)
        {
            File = file;

            Int32 longestString = 0;
            for (UInt32 i = 0; i <= File.LastEntry; i++)
            {
                if (File[i].Length > longestString)
                    longestString = File[i].Length;
            }

            if (AutoSize)
            {
                this.Size = new Size(EntryValueBox.Width + 30 + longestString * 5, 26);
                EntryComboBox.Width = this.Width - EntryValueBox.Width - 10;
                this.AutoSize = false;
            }

            //if (File.LastEntry != 0) EntryValueBox.Maximum = File.LastEntry;

            EntryValueBox.MouseWheel += delegate (Object sender, MouseEventArgs e)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            };
            EntryComboBox.DataSource = File.ToList();
            EntryComboBox.ValueMember = "Key";
            EntryComboBox.DisplayMember = "Value";
            EntryComboBox.SelectedValueChanged += UpdateEntryValueBox;
            EntryComboBox.TextUpdate += UpdateArrayFileText;
            EntryComboBox.MouseWheel += delegate (Object sender, MouseEventArgs e)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            };

            UpdateEntryComboBox(this, null);
        }

        public void SelectFirstItem()
        {
            this.EntryComboBox.SelectedIndex = 0;
            if (EntryValueBox.Value == 0 && EntryComboBox.Items.Count > 1)
            {
                this.EntryComboBox.SelectedIndex = 1;
            }
        }



        public event EventHandler ValueChanged
        {
            add
            {
                EntryValueBox.ValueChanged += value;
            }
            remove
            {
                EntryValueBox.ValueChanged -= value;
            }
        }



        protected void UpdateEntryValueBox(Object sender, EventArgs e)
        {
            EntryValueBox.ValueChanged -= UpdateEntryComboBox;

            try
            {
                UInt32 value = ((KeyValuePair<UInt32, String>)EntryComboBox.SelectedItem).Key;
                EntryValueBox.Value = value;
            }
            catch
            {
                EntryComboBox.SelectedItem = null;
            }

            EntryValueBox.ValueChanged += UpdateEntryComboBox;
        }
        protected void UpdateEntryComboBox(Object sender, EventArgs e)
        {
            EntryComboBox.SelectedValueChanged -= UpdateEntryValueBox;

            try
            {
                EntryComboBox.SelectedValue = (UInt32)EntryValueBox.Value;
            }
            catch
            {
                EntryComboBox.SelectedItem = null;
            }

            EntryComboBox.SelectedValueChanged += UpdateEntryValueBox;
        }

        protected void UpdateArrayFileText(Object sender, EventArgs e)
        {
            if (Prompt.UpdateArrayFile() == DialogResult.Yes)
            {
                File.RenameEntry((UInt32)EntryValueBox.Value, EntryComboBox.Text);

                EntryComboBox.DataSource = File.ToList();
            }
        }
    }
}
