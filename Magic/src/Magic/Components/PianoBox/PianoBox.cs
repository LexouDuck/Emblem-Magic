using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    /// <summary>
    /// A piano-based control for various use
    /// </summary>
    public partial class PianoBox : Control
    {
        /// <summary>
        /// The amount of octaves to display in this PianoBox
        /// </summary>
        public Int32 Octaves { get; set; }
        /// <summary>
        /// The currently highlighted notes on the piano
        /// </summary>
        public Boolean[] Selection { get; set; }
        /// <summary>
        /// Whether or not the selection of notes on this PianoBox is sustained
        /// </summary>
        public Boolean ToggleSelection { get; set; }



        public PianoBox()
        {
            this.Octaves = 10;
            this.Selection = new Boolean[this.Octaves * 12];
            this.ToggleSelection = false;

            this.InitializeComponent();

            this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }



        public event EventHandler SelectionChanged;



        void CheckKeyboard(Boolean select)
        {
            this.Selection = new Boolean[this.Octaves * 12];
            for (Int32 i = 0; i < this.keys.Count; i++)
            {
                switch (this.keys[i])
                {
                    case Keys.Z: this.Selection[12 * 4 + 0] = true; break; // C
                    case Keys.S: this.Selection[12 * 4 + 1] = true; break; // C#
                    case Keys.X: this.Selection[12 * 4 + 2] = true; break; // D
                    case Keys.D: this.Selection[12 * 4 + 3] = true; break; // Eb
                    case Keys.C: this.Selection[12 * 4 + 4] = true; break; // E 
                    case Keys.V: this.Selection[12 * 4 + 5] = true; break; // F
                    case Keys.G: this.Selection[12 * 4 + 6] = true; break; // F#
                    case Keys.B: this.Selection[12 * 4 + 7] = true; break; // G
                    case Keys.H: this.Selection[12 * 4 + 8] = true; break; // G#
                    case Keys.N: this.Selection[12 * 4 + 9] = true; break; // A
                    case Keys.J: this.Selection[12 * 4 + 10] = true; break; // Bb
                    case Keys.M: this.Selection[12 * 4 + 11] = true; break; // B
                    case Keys.Oemcomma: this.Selection[12 * 5] = true; break;

                    default: this.keys.RemoveAt(i); i--; break;
                }
            }
            this.SelectionChanged(this, null);
            this.Invalidate();
        }
        void CheckMouseCollisions(MouseEventArgs e)
        {
            Single increment = this.Width / this.Octaves / 7; // the width of a white note
            Single width = (increment / 3) * 2;
            Single height = this.Height * 0.6f;
            Single x = width;
            if (e.Y <= height)
            {
                for (Int32 i = 0; i < this.Octaves * 12; i++)
                {
                    switch (i % 12)
                    {
                        case 1: case 3: case 6: case 8: case 10: // black notes
                            if (e.X >= x && e.X <= x + width)
                            {
                                if (this.ToggleSelection)
                                    this.Selection[i] = !this.Selection[i];
                                else this.Selection[i] = true;
                                this.SelectionChanged(this, null);
                                this.Invalidate();
                                return;
                            }
                            x += increment;
                            if (i % 12 == 3 || i % 12 == 10)
                                x += increment;
                            break;
                        default: continue;
                    }
                }
            }
            x = 0;
            for (Int32 i = 0; i < this.Octaves * 12; i++)
            {
                switch (i % 12)
                {
                    case 0: case 2: case 4: case 5: case 7: case 9: case 11: // white notes
                        if (e.X >= x && e.X < x + increment)
                        {
                            if (this.ToggleSelection)
                                this.Selection[i] = !this.Selection[i];
                            else this.Selection[i] = true;
                            this.SelectionChanged(this, null);
                            this.Invalidate();
                            return;
                        }
                        x += increment;
                        break;
                    default: continue;
                }
            }
            this.Invalidate();
        }



        Boolean mouse = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!this.ToggleSelection && this.mouse)
            {
                this.Selection = new Boolean[this.Octaves * 12];
                this.CheckMouseCollisions(e);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.mouse = false;

            if (!this.ToggleSelection)
            {
                this.Selection = new Boolean[this.Octaves * 12];
                this.SelectionChanged(this, null);
                this.Invalidate();
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.mouse = true;

            this.Focus();
            this.CheckMouseCollisions(e);
        }

        List<Keys> keys = new List<Keys>();
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.keys.Contains(e.KeyCode)) return;
            else this.keys.Add(e.KeyCode);

            this.CheckKeyboard(true);
            e.Handled = true;
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.keys.Remove(e.KeyCode);

            this.CheckKeyboard(false);
            e.Handled = true;
        }
        protected override Boolean IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Z: // C
                case Keys.S: // C#
                case Keys.X: // D
                case Keys.D: // Eb
                case Keys.C: // E 
                case Keys.V: // F
                case Keys.G: // F#
                case Keys.B: // G
                case Keys.H: // G#
                case Keys.N: // A
                case Keys.J: // Bb
                case Keys.M: // B
                case Keys.Oemcomma: return true;
            }
            return base.IsInputKey(keyData);
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Single increment = this.Width / this.Octaves / 7; // the width of a white note

            using (Brush white = new SolidBrush(this.ForeColor))
            {
                e.Graphics.FillRectangle(white, 0, 0, this.Width, this.Height);
            }

            using (Brush selected = new SolidBrush(SystemColors.Highlight))
            {
                Single width = (increment / 3) * 2;
                Single height = (this.Height * 0.6f);
                Single x = 0;
                using (Pen lines = new Pen(SystemColors.AppWorkspace))
                {
                    e.Graphics.DrawRectangle(lines, 0, 0, this.Width - 1, this.Height - 1);
                    
                    for (Int32 i = 0; i < this.Octaves * 12; i++)
                    {
                        switch (i % 12)
                        {
                            case 0: case 2: case 4: case 5: case 7: case 9: case 11: // white notes
                                e.Graphics.DrawLine(lines, x, 0, x, this.Height - 1);
                                if (this.Selection[i])
                                {
                                    e.Graphics.FillRectangle(selected, x + 1, 1, increment, this.Height - 1);
                                }
                                x += increment;
                                break;
                            default: continue;
                        }
                    }
                }
                x = width;
                using (Brush black = new SolidBrush(this.BackColor))
                {
                    for (Int32 i = 0; i < this.Octaves * 12; i++)
                    {
                        switch (i % 12)
                        {
                            case 1: case 3: case 6: case 8: case 10: // black notes
                                e.Graphics.FillRectangle(black, x, 1, width, height);
                                if (this.Selection[i])
                                {
                                    e.Graphics.FillRectangle(selected, x + 1, 1, width - 2, height - 1);
                                }
                                x += increment;
                                if (i % 12 == 3 || i % 12 == 10)
                                    x += increment;
                                break;
                            default: continue;
                        }
                    }
                }
            }
        }

        #region Component Designer generated code

        /// <summary>
        /// An event handler that calls the 'OnPaint' method
        /// </summary>
        void Redraw(Object sender, EventArgs e)
        {
            this.Invalidate();
        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.BackColor = SystemColors.ControlDarkDark;
            this.ForeColor = SystemColors.ControlLightLight;
            this.Name = "SampleBox";
            this.Size = new Size(200, 40);
            this.SizeChanged += new System.EventHandler(this.Redraw);

            this.ResumeLayout(false);
        }

        #endregion
    }
}
