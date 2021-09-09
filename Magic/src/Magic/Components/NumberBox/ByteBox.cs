using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    public class ByteBox : NumericUpDown
    {
        public new Byte Value
        {
            get
            {
                return (Byte)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        public ByteBox() : base()
        {
            SuspendLayout();

            Minimum = 0;
            Maximum = 255;
            Name = "ByteBox";
            Size = new Size(40, 20);
            Hexadecimal = true;

            ResumeLayout(false);
        }
    }
}
