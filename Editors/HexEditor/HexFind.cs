using EmblemMagic.Components;
using EmblemMagic;
using System;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    /// <summary>
    /// Summary description for FormFind.
    /// </summary>
    public partial class HexFind : Form
    {
        public HexBox HexEditSource { get; set; }

        bool Finding;

        public HexFind()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Is called after the constructor.
        /// </summary>
        public void Core_OnOpen()
        {
            if (HexEditSource.SelectionLength > 0)
            {
                string text = "";
                byte[] data = new byte[HexEditSource.SelectionLength];

                for (int i = 0; i < HexEditSource.SelectionLength; i++)
                {
                    data[i] = HexEditSource.ByteProvider.ReadByte(HexEditSource.SelectionStart + i);
                    text += HexEditSource.ByteCharConverter.ToChar(data[i]);
                }

                FindTextBox.Text = text;
                FindHexBox.Value = data;
            }
            else
            {
                FindTextBox.Text = "";
                FindHexBox.Value = new byte[0];
            }
        }
        /// <summary>
        /// Is called when this find window is activated
        /// </summary>
        void Core_OnFocus(object sender, EventArgs e)
        {
            if (FindTextRadioBox.Checked)
                FindTextBox.Focus();
            else
                FindHexBox.Focus();
        }
        /// <summary>
        /// Finds the next entry as fits the current search query
        /// </summary>
        public void Core_FindNext()
        {
            if (!FindOkButton.Enabled) return;
            Update_ToFindingState();
            
            Int64 result;
            if (FindTextRadioBox.Checked)
            {
                result = HexEditSource.Find(FindTextBox.Text, MatchCaseCheckBox.Checked);
            }
            else
            {
                result = HexEditSource.Find(FindHexBox.Value);
            }

            Update_ToNormalState();

            Application.DoEvents();

            if (result == -1) // -1 = no match
            {
                Program.ShowMessage("End of file was reached.");
            }
            else if (result == -2) // -2 = find was aborted
            {
                return;
            }
            else // something was found
            {
                this.Close();

                Application.DoEvents();

                if (!HexEditSource.Focused)
                    HexEditSource.Focus();
            }
        }



        private void Update_ToNormalState()
        {
            Finding = false;

            FindTimer.Stop();
            FindPercent.Stop();

            FindingLabel.Text = "";
            PercentLabel.Text = "";

            MatchCaseCheckBox.Enabled = true;
            FindTextBox.Enabled = true;
            FindTextRadioBox.Enabled = true;
            FindHexBox.Enabled = true;
            FindHexRadioBox.Enabled = true;
            FindOkButton.Enabled = true;
        }
        private void Update_ToFindingState()
        {
            Finding = true;

            FindTimer.Start();
            FindPercent.Start();

            MatchCaseCheckBox.Enabled = false;
            FindTextBox.Enabled = false;
            FindTextRadioBox.Enabled = false;
            FindHexBox.Enabled = false;
            FindHexRadioBox.Enabled = false;
            FindOkButton.Enabled = false;
        }



        private void Find_FindText_Checked(object sender, EventArgs e)
        {
            FindTextBox.Enabled = true;
            FindHexBox.Enabled = false;

            FindTextBox.Focus();
        }
        private void Find_FindHex_Checked(object sender, EventArgs e)
        {
            FindHexBox.Enabled = true;
            FindTextBox.Enabled = false;

            FindHexBox.Focus();
        }
        private void Find_CancelButton_Click(object sender, EventArgs e)
        {
            if (Finding)
                this.HexEditSource.AbortFind();
            else
                this.Close();
        }
        private void Find_FindOKButton_Click(object sender, EventArgs e)
        {
            if ((FindTextRadioBox.Checked && FindTextBox.Text.Length == 0) ||
                (FindHexRadioBox.Checked && FindHexBox.Value.Length == 0))
            {
                Program.ShowMessage("The given search query is empty."); return;
            }
            try
            {
                Core_FindNext();
            }
            catch (Exception ex)
            {
                Program.ShowError("There has been an error during the find operation.", ex);
            }
        }
        private void Find_Timer_Tick(object sender, EventArgs e)
        {
            if (FindingLabel.Text.Length == 13)
                FindingLabel.Text = "";

            FindingLabel.Text += ".";
        }
        private void Find_TimerPercent_Tick(object sender, EventArgs e)
        {
            long pos = this.HexEditSource.CurrentFindingPosition;
            long length = HexEditSource.Value.Length;
            double percent = (double)pos / (double)length * (double)100;

            System.Globalization.NumberFormatInfo nfi =
                new System.Globalization.CultureInfo("en-US").NumberFormat;

            string text = percent.ToString("0.00", nfi) + " %";
            PercentLabel.Text = text;
        }
    }
}
