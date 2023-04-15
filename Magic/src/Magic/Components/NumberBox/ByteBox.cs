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
            this.SuspendLayout();

            this.Minimum = 0;
            this.Maximum = 255;
            this.Name = "ByteBox";
            this.Size = new Size(40, 20);
            this.Hexadecimal = true;

            this.ResumeLayout(false);
        }
    }
}
