using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Magic.Components
{
    public class ShortArrayBox : ArrayBox<UInt16>
    {
        /// <summary>
        /// Gets or sets this ArrayBox's NumberBox value.
        /// </summary>
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        public override UInt16 Value
        {
            get
            {
                return (UInt16)this.EntryValueBox.Value;
            }
            set
            {
                this.EntryValueBox.Value = value;
            }
        }



        public ShortArrayBox()
        {
            this.EntryValueBox = new ShortBox();
            this.EntryComboBox = new ComboBox();
            base.InitializeComponent();

            this.EntryValueBox.ValueChanged += this.UpdateEntryComboBox;

            this.Name = "ShortArrayBox";
            this.Size = new System.Drawing.Size(128, 26);
            this.MinimumSize = new System.Drawing.Size(128, 26);
            this.EntryValueBox.Size = new System.Drawing.Size(55, 26);
            this.EntryComboBox.Location = new System.Drawing.Point(63, 2);
            this.EntryComboBox.Width = this.Width - this.EntryValueBox.Width - 10;
        }
    }
}