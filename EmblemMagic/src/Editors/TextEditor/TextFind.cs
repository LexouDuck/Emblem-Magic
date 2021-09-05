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
            Owner = owner;

            InitializeComponent();
        }

        void Core_Find(string text, bool forward)
        {
            if (text == null || text == "")
            {
                UI.ShowMessage("There is no input text to find."); return;
            }
            string current = Owner.Core_GetText(Owner.CurrentIndex);

            Find_ProgressBar.Maximum = MAXIMUM;

            for (int i = forward ?
                    (int)(Owner.CurrentSelection.Start + Owner.CurrentSelection.Length) :
                    (int)(Owner.CurrentSelection.Start - 1);
                forward ?
                    (i < current.Length) :
                    (i >= 0);
                i += forward ? 1 : -1)
            {
                if (Find_Current_Button.Checked)
                    Find_ProgressBar.Value = i;

                if (current[i] == text[0])
                {
                    bool match = true;
                    for (int j = 1; j < text.Length; j++)
                    {
                        if (current[i + j] == text[j]) continue;
                        else { match = false; break; }
                    }
                    if (match)
                    {
                        Find_ProgressBar.Value = MAXIMUM;
                        Owner.CurrentSelection = new Magic.Range(i, i + text.Length);
                        return;
                    }
                }
            }
            Find_ProgressBar.Value = 0;

            if (Find_Complete_Button.Checked)
            {
                Find_ProgressBar.Maximum = MAXIMUM;

                for (UInt16 entry = forward ?
                        (UInt16)(Owner.CurrentIndex + 1):
                        (UInt16)(Owner.CurrentIndex - 1);
                    forward ? 
                        (entry < MAXIMUM) :
                        (entry >= 0);
                    entry += (UInt16)(forward ? 1 : -1))
                {
                    Find_ProgressBar.Value = entry;
                    try
                    {
                        current = Owner.Core_GetText(entry);

                        for (int i = forward ? 0 : current.Length - 1;
                            forward ?
                                (i < current.Length) :
                                (i >= 0);
                            i += forward ? 1 : -1)
                        {
                            if (current[i] == text[0])
                            {
                                bool match = true;
                                for (int j = 1; j < text.Length; j++)
                                {
                                    if (current[i + j] == text[j]) continue;
                                    else { match = false; break; }
                                }
                                if (match)
                                {
                                    Find_ProgressBar.Value = MAXIMUM;
                                    Owner.Core_SetEntry((UInt16)entry);
                                    Owner.CurrentSelection = new Magic.Range(i, i + text.Length);
                                    return;
                                }
                            }
                        }
                    }
                    catch { continue; }
                }
                Find_ProgressBar.Value = 0;
            }
        }

        private void Find_Prev_Button_Click(Object sender, EventArgs e)
        {
            Core_Find(Find_TextBox.Text, false);
        }
        private void Find_Next_Button_Click(Object sender, EventArgs e)
        {
            Core_Find(Find_TextBox.Text, true);
        }
    }
}
