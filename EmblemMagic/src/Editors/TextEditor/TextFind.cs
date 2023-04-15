using System;
using System.Windows.Forms;
using GBA;
using Magic;

namespace EmblemMagic.Editors
{
    public partial class TextFind : Form
    {
        new TextEditor Owner;

        const UInt16 MAXIMUM = 0x8000;



        public TextFind(TextEditor owner)
        {
            base.Owner = owner;
            this.Owner = owner;

            this.InitializeComponent();
        }

        void Core_Find(String text, Boolean forward)
        {
            if (text == null || text == "")
            {
                UI.ShowMessage("There is no input text to find."); return;
            }
            String current = this.Owner.Core_GetText(this.Owner.CurrentIndex);

            this.Find_ProgressBar.Maximum = MAXIMUM;

            for (Int32 i = forward ?
                    (Int32)(this.Owner.CurrentSelection.Start + this.Owner.CurrentSelection.Length) :
                    (Int32)(this.Owner.CurrentSelection.Start - 1);
                forward ?
                    (i < current.Length) :
                    (i >= 0);
                i += forward ? 1 : -1)
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
                        this.Find_ProgressBar.Value = MAXIMUM;
                        this.Owner.CurrentSelection = new Magic.Range(i, i + text.Length);
                        return;
                    }
                }
            }
            this.Find_ProgressBar.Value = 0;

            if (this.Find_Complete_Button.Checked)
            {
                this.Find_ProgressBar.Maximum = MAXIMUM;

                for (UInt16 entry = forward ?
                        (UInt16)(this.Owner.CurrentIndex + 1):
                        (UInt16)(this.Owner.CurrentIndex - 1);
                    forward ? 
                        (entry < MAXIMUM) :
                        (entry >= 0);
                    entry += (UInt16)(forward ? 1 : -1))
                {
                    this.Find_ProgressBar.Value = entry;
                    try
                    {
                        current = this.Owner.Core_GetText(entry);

                        for (Int32 i = forward ? 0 : current.Length - 1;
                            forward ?
                                (i < current.Length) :
                                (i >= 0);
                            i += forward ? 1 : -1)
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
                                    this.Find_ProgressBar.Value = MAXIMUM;
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

        private void Find_Prev_Button_Click(Object sender, EventArgs e)
        {
            this.Core_Find(this.Find_TextBox.Text, false);
        }
        private void Find_Next_Button_Click(Object sender, EventArgs e)
        {
            this.Core_Find(this.Find_TextBox.Text, true);
        }
    }
}
