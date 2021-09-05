using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace Magic.Components
{
    public class CodeBox : FastColoredTextBox
    {
        public List<Tuple<Regex, TextStyle>> SyntaxColors { get; }
        public List<Tuple<string, string>> Collapses { get; }

        Brush bg;

        public CodeBox() : base()
        {
            SyntaxColors = new List<Tuple<Regex, TextStyle>>();
            Collapses = new List<Tuple<string, string>>();

            bg = new SolidBrush(this.BackColor);
            this.SelectionColor = SystemColors.Highlight;

            WordWrap = false;
            Font = new Font("Consolas", 8F);

            TextChanged += RedrawChangedText;
        }

        /// <summary>
        /// Adds a new syntax coloring item to this CodeBox
        /// </summary>
        public void AddSyntax(string regex, Color color, FontStyle style = FontStyle.Regular)
        {
            SyntaxColors.Add(Tuple.Create(new Regex(regex),
                new TextStyle(new SolidBrush(color), bg, style)));
        }
        /// <summary>
        /// Adds a new collapsing region rule for this CodeBox
        /// </summary>
        public void AddCollapse(string start, string end)
        {
            Collapses.Add(Tuple.Create(start, end));
        }

        void RedrawChangedText(object sender, TextChangedEventArgs e)
        {
            foreach (Tuple<Regex, TextStyle> syntax in SyntaxColors)
            {
                e.ChangedRange.ClearStyle(syntax.Item2);
                e.ChangedRange.SetStyle(syntax.Item2, syntax.Item1);
            }
            e.ChangedRange.ClearFoldingMarkers();
            foreach (Tuple<string, string> collapse in Collapses)
            {
                e.ChangedRange.SetFoldingMarkers(collapse.Item1, collapse.Item2);
            }
        }

        public int GetIndexFromPosition(Point position)
        {
            return PointToPosition(position);
        }
    }

        /* Old version, slow as hell

        public class CodeBox : Panel
        {
            public List<Regex> KeyWords { get; }
            public List<Color> Coloring { get; }

            RichTextBox RTB;

            public CodeBox() : base()
            {
                KeyWords = new List<Regex>();
                Coloring = new List<Color>();

                this.BorderStyle = BorderStyle.FixedSingle;
                this.Padding = new Padding(2, 0, 0, 0);

                RTB = new RichTextBox()
                {
                    Text = "",
                    WordWrap = false,
                    Font = new Font("Consolas", 8F),
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.None,
                    ScrollBars = RichTextBoxScrollBars.Both,
                };
                Controls.Add(RTB);
                RTB.TextChanged += new EventHandler(UpdateSyntaxColoring);
                RTB.SizeChanged += new EventHandler(Redraw);
            }
            private void Redraw(object sender, EventArgs e)
            {
                RTB.Invalidate();
            }
            private void UpdateSyntaxColoring(object sender, EventArgs e)
            {
                if (KeyWords == null) return;

                ControlPainting.SuspendPainting(RTB);
                int selection = RTB.SelectionStart;
                int scrollpos = NativeMethods.GetScrollPos(RTB.Handle, 1);
                RTB.Select(0, RTB.Text.Length);
                RTB.SelectionColor = Color.Black;
                for (int i = 0; i < KeyWords.Count; i++)
                {
                    foreach (Match match in KeyWords[i].Matches(Text))
                    {
                        RTB.Select(match.Index, match.Length);
                        RTB.SelectionColor = Coloring[i];
                    }
                }
                RTB.Select(selection, 0);
                ScrollTo(scrollpos);
                ControlPainting.ResumePainting(RTB);
            }

            /// <summary>
            /// Adds a new syntax coloring item to this CodeBox. Items added last have higher priority
            /// </summary>
            public void AddSyntax(Regex regex, Color color)
            {
                KeyWords.Add(regex);
                Coloring.Add(color);
            }



            public override string Text
            {
                get { return RTB.Text; }
                set { RTB.Text = value; }
            }
            public override Font Font
            {
                get { return RTB.Font; }
                set { RTB.Font = value; }
            }
            public int SelectionStart
            {
                get { return RTB.SelectionStart; }
                set { RTB.SelectionStart = value; }
            }
            public int SelectionLength
            {
                get { return RTB.SelectionLength; }
                set { RTB.SelectionLength = value; }
            }
            public void ScrollToCaret()
            {
                RTB.ScrollToCaret();
                RTB.Select();
            }
            public int GetIndexFromPosition(Point position)
            {
                return RTB.GetCharIndexFromPosition(position);
            }

            public new event EventHandler TextChanged
            {
                add    { RTB.TextChanged += value; }
                remove { RTB.TextChanged -= value; }
            }
            public new event EventHandler MouseHover
            {
                add    { RTB.MouseHover += value; }
                remove { RTB.MouseHover -= value; }
            }
            public new event MouseEventHandler MouseMove
            {
                add    { RTB.MouseMove += value; }
                remove { RTB.MouseMove -= value; }
            }


            public void ScrollTo(int Position)
            {
                NativeMethods.SetScrollPos((IntPtr)RTB.Handle, 0x1, Position, false);
                NativeMethods.PostMessage((IntPtr)RTB.Handle, 0x115, 4 + 0x10000 * Position, 0);
            }
        }
        */
    }