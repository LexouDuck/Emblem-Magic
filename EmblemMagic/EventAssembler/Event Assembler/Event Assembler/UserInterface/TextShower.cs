using System;
using System.Collections.Generic;

using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nintenlord.Event_Assembler.UserInterface
{
    public partial class TextShower : Form
    {
        string text;
        public TextShower(string text)
        {
            InitializeComponent();
            this.text = text;

            var g = this.richTextBox1.CreateGraphics();
            var size = g.MeasureString(text, richTextBox1.Font);

            richTextBox1.Text = text;
            richTextBox1.KeyUp += new KeyEventHandler(AllKeyUp);
            this.Width = (int)size.Width + 10;


            this.KeyUp += new KeyEventHandler(AllKeyUp);
        }

        void AllKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
