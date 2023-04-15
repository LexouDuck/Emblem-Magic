using System;
using System.Windows.Forms;
using Magic;

namespace EmblemMagic.Editors
{
    public partial class TextReplace : Form
    {
        new TextEditor Owner;

        const UInt16 MAXIMUM = 0x8000;



        public TextReplace(TextEditor owner)
        {
            base.Owner = owner;
            this.Owner = owner;

            this.InitializeComponent();
        }

        void Core_Replace(String text, String replace)
        {
            if (text == null || text == "")
            {
                UI.ShowMessage("There is no input text to find."); return;
            }

            String current = this.Owner.Core_GetText(this.Owner.CurrentIndex);

            if (text == current.Substring((Int32)this.Owner.CurrentSelection.Start, (Int32)this.Owner.CurrentSelection.Length))
            {
                this.Owner.Core_WriteText(
                    current.Substring(0, (Int32)this.Owner.CurrentSelection.Start) + replace +
                    current.Substring((Int32)this.Owner.CurrentSelection.End), this.Owner.CurrentIndex);
            }

            this.Find_ProgressBar.Maximum = current.Length;

            for (Int32 i = (Int32)(this.Owner.CurrentSelection.Start + this.Owner.CurrentSelection.Length); i < current.Length; i++)
            {
                if (this.Find_Current_Button.Checked)
                    this.Find_ProgressBar.Value = i;

                if (current[i] == text[0])
                {
                    Boolean match = true;
                    for (Int32 j = 1; j < text.Length; j++)
                    {
                        if (current[i + j] == text[j]) continue;
                        else { match = false; break; }
                    }
                    if (match)
                    {
                        this.Find_ProgressBar.Value = this.Find_ProgressBar.Maximum - 1;
                        this.Owner.CurrentSelection = new Magic.Range(i, i + text.Length);
                        return;
                    }
                }
            }
            this.Find_ProgressBar.Value = 0;

            if (this.Find_Complete_Button.Checked)
            {
                this.Find_ProgressBar.Maximum = MAXIMUM;

                for (UInt16 entry = (UInt16)(this.Owner.CurrentIndex + 1); entry < MAXIMUM; entry++)
                {
                    this.Find_ProgressBar.Value = entry;
                    try
                    {
                        current = this.Owner.Core_GetText(entry);

                        for (Int32 i = 0; (i < current.Length); i++)
                        {
                            if (current[i] == text[0])
                            {
                                Boolean match = true;
                                for (Int32 j = 1; j < text.Length; j++)
                                {
                                    if (current[i + j] == text[j]) continue;
                                    else { match = false; break; }
                                }
                                if (match)
                                {
                                    this.Find_ProgressBar.Value = this.Find_ProgressBar.Maximum - 1;
                                    this.Owner.Core_SetEntry((UInt16)entry);
                                    this.Owner.CurrentSelection = new Magic.Range(i, i + text.Length);
                                    return;
                                }
                            }
                        }
                    }
                    catch { continue; }
                }
                this.Find_ProgressBar.Value = 0;
            }
        }
        void Core_ReplaceAll(String text, String replace)
        {
            if (text == null || text == "")
            {
                UI.ShowMessage("There is no input text to find."); return;
            }

            String current = this.Owner.Core_GetText(this.Owner.CurrentIndex);

            this.Find_ProgressBar.Maximum = MAXIMUM;

            for (UInt16 entry = 0; entry < MAXIMUM; entry++)
            {
                this.Find_ProgressBar.Value = entry;
                try
                {
                    current = this.Owner.Core_GetText(entry);

                    this.Owner.Core_WriteText(current.Replace(text, replace), this.Owner.CurrentIndex);
                }
                catch { continue; }
            }
            this.Find_ProgressBar.Value = 0;
        }

        private void ReplaceNext_Button_Click(Object sender, EventArgs e)
        {
            this.Core_Replace(this.Find_TextBox.Text, this.Replace_TextBox.Text);
        }
        private void ReplaceAll_Button_Click(Object sender, EventArgs e)
        {
            this.Core_ReplaceAll(this.Find_TextBox.Text, this.Replace_TextBox.Text);
        }
    }
}
