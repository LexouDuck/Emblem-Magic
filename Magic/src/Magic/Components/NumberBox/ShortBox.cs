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
            SuspendLayout();

            Minimum = 0;
            Maximum = UInt16.MaxValue;
            Name = "ShortBox";
            Size = new Size(55, 20);
            Hexadecimal = true;

            ResumeLayout(false);
        }
    }
}
