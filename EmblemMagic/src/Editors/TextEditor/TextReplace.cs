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
            Owner = owner;
            
            InitializeComponent();
        }

        void Core_Replace(String text, String replace)
        {
            if (text == null || text == "")
            {
                UI.ShowMessage("There is no input text to find."); return;
            }

            String current = Owner.Core_GetText(Owner.CurrentIndex);

            if (text == current.Substring((Int32)Owner.CurrentSelection.Start, (Int32)Owner.CurrentSelection.Length))
            {
                Owner.Core_WriteText(
                    current.Substring(0, (Int32)Owner.CurrentSelection.Start) + replace +
                    current.Substring((Int32)Owner.CurrentSelection.End), Owner.CurrentIndex);
            }

            Find_ProgressBar.Maximum = current.Length;

            for (Int32 i = (Int32)(Owner.CurrentSelection.Start + Owner.CurrentSelection.Length); i < current.Length; i++)
            {
                if (Find_Current_Button.Checked)
                    Find_ProgressBar.Value = i;

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
                        Find_ProgressBar.Value = Find_ProgressBar.Maximum - 1;
                        Owner.CurrentSelection = new Magic.Range(i, i + text.Length);
                        return;
                    }
                }
            }
            Find_ProgressBar.Value = 0;

            if (Find_Complete_Button.Checked)
            {
                Find_ProgressBar.Maximum = MAXIMUM;

                for (UInt16 entry = (UInt16)(Owner.CurrentIndex + 1); entry < MAXIMUM; entry++)
                {
                    Find_ProgressBar.Value = entry;
                    try
                    {
                        current = Owner.Core_GetText(entry);

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
                                    Find_ProgressBar.Value = Find_ProgressBar.Maximum - 1;
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
        void Core_ReplaceAll(String text, String replace)
        {
            if (text == null || text == "")
            {
                UI.ShowMessage("There is no input text to find."); return;
            }

            String current = Owner.Core_GetText(Owner.CurrentIndex);

            Find_ProgressBar.Maximum = MAXIMUM;

            for (UInt16 entry = 0; entry < MAXIMUM; entry++)
            {
                Find_ProgressBar.Value = entry;
                try
                {
                    current = Owner.Core_GetText(entry);
                    
                    Owner.Core_WriteText(current.Replace(text, replace), Owner.CurrentIndex);
                }
                catch { continue; }
            }
            Find_ProgressBar.Value = 0;
        }

        private void ReplaceNext_Button_Click(Object sender, EventArgs e)
        {
            Core_Replace(Find_TextBox.Text, Replace_TextBox.Text);
        }
        private void ReplaceAll_Button_Click(Object sender, EventArgs e)
        {
            Core_ReplaceAll(Find_TextBox.Text, Replace_TextBox.Text);
        }
    }
}
