using Magic.Components;
using GBA;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace Magic
{
    public partial class FormRepoint : Form
    {
        public PointerBox[] Boxes { get; }

        public UInt32[] Lengths { get; }

        public FormRepoint(String caption, String message, Tuple<String, Pointer, Int32>[] pointers, Int32 forceWidth = 0)
        {
            this.InitializeComponent();

            this.Text = caption;
            this.TextLabel.Text = message;

            this.Height = 170 + pointers.Length * 30;
            this.BoxLayout.Height = Math.Min(this.Height - 170, pointers.Length * 28);
            this.BoxLayout.RowCount = pointers.Length;

            this.Lengths = new UInt32[pointers.Length];
            this.Boxes = new PointerBox[pointers.Length];
            Label label;
            for (Int32 i = 0; i < pointers.Length; i++)
            {
                this.Lengths[i] = (UInt32)pointers[i].Item3;

                label = new Label();
                label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                label.Text = pointers[i].Item1;
                label.TextAlign = ContentAlignment.MiddleRight;
                this.BoxLayout.Controls.Add(label, 0, i);

                this.Boxes[i] = new PointerBox();
                this.Boxes[i].Value = pointers[i].Item2;
                this.BoxLayout.Controls.Add(this.Boxes[i], 1, i);

                label = new Label();
                label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                label.Text = "0x" + Util.UInt32ToHex(this.Lengths[i]) + " /";
                label.TextAlign = ContentAlignment.MiddleRight;
                this.BoxLayout.Controls.Add(label, 2, i);

                label = new Label();
                label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                label.Text = this.Lengths[i] + " bytes";
                label.TextAlign = ContentAlignment.MiddleLeft;
                this.BoxLayout.Controls.Add(label, 3, i);
            }
            if (forceWidth > 0)
            {
                this.AutoSequence.Visible = false;
                this.MaximumSize = new Size(forceWidth, this.MaximumSize.Height);
                this.MinimumSize = new Size(forceWidth, this.MinimumSize.Height);
                this.Width = forceWidth;
                this.BoxLayout.Location = new Point(this.BoxLayout.Location.X, this.BoxLayout.Location.Y + 30);
                this.BoxLayout.ColumnStyles[0].SizeType = SizeType.Percent;
                this.BoxLayout.ColumnStyles[1].SizeType = SizeType.Percent;
                this.BoxLayout.ColumnStyles[2].SizeType = SizeType.Percent;
                this.BoxLayout.ColumnStyles[3].SizeType = SizeType.Percent;
                this.BoxLayout.ColumnStyles[0].Width = 50;
                this.BoxLayout.ColumnStyles[1].Width = 25;
                this.BoxLayout.ColumnStyles[2].Width = 25;
                this.BoxLayout.ColumnStyles[3].Width = 0;
            }
            this.Confirm.DialogResult = DialogResult.OK;
            this.Cancel.DialogResult = DialogResult.Cancel;
        }

        private void AutoSequence_Click(Object sender, EventArgs e)
        {
            for (Int32 i = 1; i < this.Boxes.Length; i++)
            {
                this.Boxes[i].Value = this.Boxes[i - 1].Value + (Int32)this.Lengths[i - 1];

                this.Boxes[i].Value += (4 - this.Boxes[i].Value % 4) % 4;
            }
        }
    }
}
