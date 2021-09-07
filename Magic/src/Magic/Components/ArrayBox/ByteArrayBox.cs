using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Magic.Components
{
    public class ByteArrayBox : ArrayBox<Byte>
    {
        /// <summary>
        /// Gets or sets this ArrayBox's NumberBox value.
        /// </summary>
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        public override Byte Value
        {
            get
            {
                return (Byte)EntryValueBox.Value;
            }
            set
            {
                EntryValueBox.Value = value;
            }
        }



        public ByteArrayBox()
        {
            this.EntryValueBox = new ByteBox();
            this.EntryComboBox = new ComboBox();
            base.InitializeComponent();

            EntryValueBox.ValueChanged += UpdateEntryComboBox;

            this.Name = "ByteArrayBox";
            this.Size = new System.Drawing.Size(128, 26);
            this.MinimumSize = new System.Drawing.Size(128, 26);
            this.EntryValueBox.Size = new System.Drawing.Size(40, 26);
            this.EntryComboBox.Location = new System.Drawing.Point(48, 2);
            EntryComboBox.Width = this.Width - EntryValueBox.Width - 10;
        }
    }
}
