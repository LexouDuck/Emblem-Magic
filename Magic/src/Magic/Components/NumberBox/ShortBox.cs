using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    public class ShortBox : NumericUpDown
    {
        public new UInt16 Value
        {
            get
            {
                return (UInt16)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        public ShortBox() : base()
        {
            this.SuspendLayout();

            this.Minimum = 0;
            this.Maximum = UInt16.MaxValue;
            this.Name = "ShortBox";
            this.Size = new Size(55, 20);
            this.Hexadecimal = true;

            this.ResumeLayout(false);
        }
    }
}
