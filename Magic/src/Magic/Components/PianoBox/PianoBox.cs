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
            Octaves = 10;
            Selection = new Boolean[Octaves * 12];
            ToggleSelection = false;

            InitializeComponent();
            
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }



        public event EventHandler SelectionChanged;



        void CheckKeyboard(Boolean select)
        {
            Selection = new Boolean[Octaves * 12];
            for (Int32 i = 0; i < keys.Count; i++)
            {
                switch (keys[i])
                {
                    case Keys.Z:    Selection[12 * 4 + 0] = true; break; // C
                    case Keys.S:    Selection[12 * 4 + 1] = true; break; // C#
                    case Keys.X:    Selection[12 * 4 + 2] = true; break; // D
                    case Keys.D:    Selection[12 * 4 + 3] = true; break; // Eb
                    case Keys.C:    Selection[12 * 4 + 4] = true; break; // E 
                    case Keys.V:    Selection[12 * 4 + 5] = true; break; // F
                    case Keys.G:    Selection[12 * 4 + 6] = true; break; // F#
                    case Keys.B:    Selection[12 * 4 + 7] = true; break; // G
                    case Keys.H:    Selection[12 * 4 + 8] = true; break; // G#
                    case Keys.N:    Selection[12 * 4 + 9] = true; break; // A
                    case Keys.J:   Selection[12 * 4 + 10] = true; break; // Bb
                    case Keys.M:   Selection[12 * 4 + 11] = true; break; // B
                    case Keys.Oemcomma: Selection[12 * 5] = true; break;

                    default: keys.RemoveAt(i); i--; break;
                }
            }
            this.SelectionChanged(this, null);
            this.Invalidate();
        }
        void CheckMouseCollisions(MouseEventArgs e)
        {
            Single increment = Width / Octaves / 7; // the width of a white note
            Single width = (increment / 3) * 2;
            Single height = this.Height * 0.6f;
            Single x = width;
            if (e.Y <= height)
            {
                for (Int32 i = 0; i < Octaves * 12; i++)
                {
                    switch (i % 12)
                    {
                        case 1: case 3: case 6: case 8: case 10: // black notes
                            if (e.X >= x && e.X <= x + width)
                            {
                                if (ToggleSelection)
                                    Selection[i] = !Selection[i];
                                else Selection[i] = true;
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
            for (Int32 i = 0; i < Octaves * 12; i++)
            {
                switch (i % 12)
                {
                    case 0: case 2: case 4: case 5: case 7: case 9: case 11: // white notes
                        if (e.X >= x && e.X < x + increment)
                        {
                            if (ToggleSelection)
                                Selection[i] = !Selection[i];
                            else Selection[i] = true;
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

            if (!ToggleSelection && mouse)
            {
                Selection = new Boolean[Octaves * 12];
                CheckMouseCollisions(e);
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouse = false;

            if (!ToggleSelection)
            {
                Selection = new Boolean[Octaves * 12];
                this.SelectionChanged(this, null);
                this.Invalidate();
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouse = true;

            this.Focus();
            CheckMouseCollisions(e);
        }

        List<Keys> keys = new List<Keys>();
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (keys.Contains(e.KeyCode)) return;
            else keys.Add(e.KeyCode);

            CheckKeyboard(true);
            e.Handled = true;
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            keys.Remove(e.KeyCode);

            CheckKeyboard(false);
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

            Single increment = Width / Octaves / 7; // the width of a white note

            using (Brush white = new SolidBrush(this.ForeColor))
            {
                e.Graphics.FillRectangle(white, 0, 0, Width, Height);
            }

            using (Brush selected = new SolidBrush(SystemColors.Highlight))
            {
                Single width = (increment / 3) * 2;
                Single height = (this.Height * 0.6f);
                Single x = 0;
                using (Pen lines = new Pen(SystemColors.AppWorkspace))
                {
                    e.Graphics.DrawRectangle(lines, 0, 0, Width - 1, Height - 1);
                    
                    for (Int32 i = 0; i < Octaves * 12; i++)
                    {
                        switch (i % 12)
                        {
                            case 0: case 2: case 4: case 5: case 7: case 9: case 11: // white notes
                                e.Graphics.DrawLine(lines, x, 0, x, Height - 1);
                                if (Selection[i])
                                {
                                    e.Graphics.FillRectangle(selected, x + 1, 1, increment, Height - 1);
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
                    for (Int32 i = 0; i < Octaves * 12; i++)
                    {
                        switch (i % 12)
                        {
                            case 1: case 3: case 6: case 8: case 10: // black notes
                                e.Graphics.FillRectangle(black, x, 1, width, height);
                                if (Selection[i])
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
