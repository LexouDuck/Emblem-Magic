using Magic.Components;
using Magic;
using System;
using System.Windows.Forms;

namespace Magic.Editors
{
    /// <summary>
    /// Summary description for FormFind.
    /// </summary>
    public partial class HexFind : Form
    {
        public HexBox HexEditSource { get; set; }

        Boolean Finding;

        public HexFind()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Is called after the constructor.
        /// </summary>
        public void Core_OnOpen()
        {
            if (this.HexEditSource.SelectionLength > 0)
            {
                String text = "";
                Byte[] data = new Byte[this.HexEditSource.SelectionLength];

                for (Int32 i = 0; i < this.HexEditSource.SelectionLength; i++)
                {
                    data[i] = this.HexEditSource.ByteProvider.ReadByte(this.HexEditSource.SelectionStart + i);
                    text += this.HexEditSource.ByteCharConverter.ToChar(data[i]);
                }

                this.FindTextBox.Text = text;
                this.FindHexBox.Value = data;
            }
            else
            {
                this.FindTextBox.Text = "";
                this.FindHexBox.Value = new Byte[0];
            }
        }
        /// <summary>
        /// Is called when this find window is activated
        /// </summary>
        void Core_OnFocus(Object sender, EventArgs e)
        {
            if (this.FindTextRadioBox.Checked)
                this.FindTextBox.Focus();
            else
                this.FindHexBox.Focus();
        }
        /// <summary>
        /// Finds the next entry as fits the current search query
        /// </summary>
        public void Core_FindNext()
        {
            if (!this.FindOkButton.Enabled) return;
            this.Update_ToFindingState();
            
            Int64 result;
            if (this.FindTextRadioBox.Checked)
            {
                result = this.HexEditSource.Find(this.FindTextBox.Text, this.MatchCaseCheckBox.Checked);
            }
            else
            {
                result = this.HexEditSource.Find(this.FindHexBox.Value);
            }

            this.Update_ToNormalState();

            Application.DoEvents();

            if (result == -1) // -1 = no match
            {
                UI.ShowMessage("End of file was reached.");
            }
            else if (result == -2) // -2 = find was aborted
            {
                return;
            }
            else // something was found
            {
                this.Close();

                Application.DoEvents();

                if (!this.HexEditSource.Focused)
                    this.HexEditSource.Focus();
            }
        }



        private void Update_ToNormalState()
        {
            this.Finding = false;

            this.FindTimer.Stop();
            this.FindPercent.Stop();

            this.FindingLabel.Text = "";
            this.PercentLabel.Text = "";

            this.MatchCaseCheckBox.Enabled = true;
            this.FindTextBox.Enabled = true;
            this.FindTextRadioBox.Enabled = true;
            this.FindHexBox.Enabled = true;
            this.FindHexRadioBox.Enabled = true;
            this.FindOkButton.Enabled = true;
        }
        private void Update_ToFindingState()
        {
            this.Finding = true;

            this.FindTimer.Start();
            this.FindPercent.Start();

            this.MatchCaseCheckBox.Enabled = false;
            this.FindTextBox.Enabled = false;
            this.FindTextRadioBox.Enabled = false;
            this.FindHexBox.Enabled = false;
            this.FindHexRadioBox.Enabled = false;
            this.FindOkButton.Enabled = false;
        }



        private void Find_FindText_Checked(Object sender, EventArgs e)
        {
            this.FindTextBox.Enabled = true;
            this.FindHexBox.Enabled = false;

            this.FindTextBox.Focus();
        }
        private void Find_FindHex_Checked(Object sender, EventArgs e)
        {
            this.FindHexBox.Enabled = true;
            this.FindTextBox.Enabled = false;

            this.FindHexBox.Focus();
        }
        private void Find_CancelButton_Click(Object sender, EventArgs e)
        {
            if (this.Finding)
                this.HexEditSource.AbortFind();
            else
                this.Close();
        }
        private void Find_FindOKButton_Click(Object sender, EventArgs e)
        {
            if ((this.FindTextRadioBox.Checked && this.FindTextBox.Text.Length == 0) ||
                (this.FindHexRadioBox.Checked && this.FindHexBox.Value.Length == 0))
            {
                UI.ShowMessage("The given search query is empty."); return;
            }
            try
            {
                this.Core_FindNext();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error during the find operation.", ex);
            }
        }
        private void Find_Timer_Tick(Object sender, EventArgs e)
        {
            if (this.FindingLabel.Text.Length == 13)
                this.FindingLabel.Text = "";

            this.FindingLabel.Text += ".";
        }
        private void Find_TimerPercent_Tick(Object sender, EventArgs e)
        {
            Int64 pos = this.HexEditSource.CurrentFindingPosition;
            Int64 length = this.HexEditSource.Value.Length;
            Double percent = (Double)pos / (Double)length * (Double)100;

            System.Globalization.NumberFormatInfo nfi =
                new System.Globalization.CultureInfo("en-US").NumberFormat;

            String text = percent.ToString("0.00", nfi) + " %";
            this.PercentLabel.Text = text;
        }
    }
}
