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
            InitializeComponent();

            this.Text = caption;
            TextLabel.Text = message;

            this.Height = 170 + pointers.Length * 30;
            BoxLayout.Height = Math.Min(this.Height - 170, pointers.Length * 28);
            BoxLayout.RowCount = pointers.Length;

            Lengths = new UInt32[pointers.Length];
            Boxes = new PointerBox[pointers.Length];
            Label label;
            for (Int32 i = 0; i < pointers.Length; i++)
            {
                Lengths[i] = (UInt32)pointers[i].Item3;

                label = new Label();
                label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                label.Text = pointers[i].Item1;
                label.TextAlign = ContentAlignment.MiddleRight;
                BoxLayout.Controls.Add(label, 0, i);

                Boxes[i] = new PointerBox();
                Boxes[i].Value = pointers[i].Item2;
                BoxLayout.Controls.Add(Boxes[i], 1, i);

                label = new Label();
                label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                label.Text = "0x" + Util.UInt32ToHex(Lengths[i]) + " /";
                label.TextAlign = ContentAlignment.MiddleRight;
                BoxLayout.Controls.Add(label, 2, i);

                label = new Label();
                label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                label.Text = Lengths[i] + " bytes";
                label.TextAlign = ContentAlignment.MiddleLeft;
                BoxLayout.Controls.Add(label, 3, i);
            }
            if (forceWidth > 0)
            {
                AutoSequence.Visible = false;
                this.MaximumSize = new Size(forceWidth, this.MaximumSize.Height);
                this.MinimumSize = new Size(forceWidth, this.MinimumSize.Height);
                this.Width = forceWidth;
                BoxLayout.Location = new Point(BoxLayout.Location.X, BoxLayout.Location.Y + 30);
                BoxLayout.ColumnStyles[0].SizeType = SizeType.Percent;
                BoxLayout.ColumnStyles[1].SizeType = SizeType.Percent;
                BoxLayout.ColumnStyles[2].SizeType = SizeType.Percent;
                BoxLayout.ColumnStyles[3].SizeType = SizeType.Percent;
                BoxLayout.ColumnStyles[0].Width = 50;
                BoxLayout.ColumnStyles[1].Width = 25;
                BoxLayout.ColumnStyles[2].Width = 25;
                BoxLayout.ColumnStyles[3].Width = 0;
            }
            Confirm.DialogResult = DialogResult.OK;
            Cancel.DialogResult = DialogResult.Cancel;
        }

        private void AutoSequence_Click(Object sender, EventArgs e)
        {
            for (Int32 i = 1; i < Boxes.Length; i++)
            {
                Boxes[i].Value = Boxes[i - 1].Value + (Int32)Lengths[i - 1];

                Boxes[i].Value += (4 - Boxes[i].Value % 4) % 4;
            }
        }
    }
}
