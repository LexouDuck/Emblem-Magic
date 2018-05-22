using EmblemMagic.Components;
using GBA;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace EmblemMagic
{
    public partial class FormRepoint : Form
    {
        public PointerBox[] Boxes { get; }

        public uint[] Lengths { get; }

        public FormRepoint(string caption, string message, Tuple<string, Pointer, int>[] pointers)
        {
            InitializeComponent();

            this.Text = caption;
            TextLabel.Text = message;

            this.Height = 170 + pointers.Length * 30;
            BoxLayout.Height = Math.Min(this.Height - 170, pointers.Length * 28);
            BoxLayout.RowCount = pointers.Length;

            Lengths = new uint[pointers.Length];
            Boxes = new PointerBox[pointers.Length];
            Label label;
            for (int i = 0; i < pointers.Length; i++)
            {
                Lengths[i] = (uint)pointers[i].Item3;

                label = new Label();
                label.Text = pointers[i].Item1;
                label.TextAlign = ContentAlignment.MiddleRight;
                BoxLayout.Controls.Add(label, 0, i);

                Boxes[i] = new PointerBox();
                Boxes[i].Value = pointers[i].Item2;
                BoxLayout.Controls.Add(Boxes[i], 1, i);

                label = new Label();
                label.Text = "0x" + Util.UInt32ToHex(Lengths[i]) + " /";
                label.TextAlign = ContentAlignment.MiddleRight;
                BoxLayout.Controls.Add(label, 2, i);

                label = new Label();
                label.Text = Lengths[i] + " bytes";
                label.TextAlign = ContentAlignment.MiddleLeft;
                BoxLayout.Controls.Add(label, 3, i);
            }
            
            Confirm.DialogResult = DialogResult.OK;
            Cancel.DialogResult = DialogResult.Cancel;
        }

        private void AutoSequence_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < Boxes.Length; i++)
            {
                Boxes[i].Value = Boxes[i - 1].Value + (int)Lengths[i - 1];

                Boxes[i].Value += (4 - Boxes[i].Value % 4) % 4;
            }
        }
    }
}
