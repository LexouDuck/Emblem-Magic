using GBA;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    public class PointerBox : NumericUpDown
    {
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        public new Pointer Value
        {
            get
            {
                return new Pointer((UInt32)base.Value);
            }
            set
            {
                base.Value = value.Address;
            }
        }

        public PointerBox() : base()
        {
            this.SuspendLayout();

            this.Minimum = 0;
            this.Maximum = DataManager.ROM_MAX_SIZE;
            this.Name = "PointerBox";
            this.Size = new Size(70, 20);
            this.Hexadecimal = true;
            ValueChanged += new EventHandler(this.UpdateMaximum);

            this.ResumeLayout(false);
        }

        void UpdateMaximum(Object sender, EventArgs e)
        {
            try
            {
                this.Maximum = Core.CurrentROMSize;
            }
            catch
            {
                this.Maximum = 0;
            }
        }
    }
}
