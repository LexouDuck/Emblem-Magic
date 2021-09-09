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
            InitializeComponent();
        }

        /// <summary>
        /// Is called after the constructor.
        /// </summary>
        public void Core_OnOpen()
        {
            if (HexEditSource.SelectionLength > 0)
            {
                String text = "";
                Byte[] data = new Byte[HexEditSource.SelectionLength];

                for (Int32 i = 0; i < HexEditSource.SelectionLength; i++)
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
                FindHexBox.Value = new Byte[0];
            }
        }
        /// <summary>
        /// Is called when this find window is activated
        /// </summary>
        void Core_OnFocus(Object sender, EventArgs e)
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



        private void Find_FindText_Checked(Object sender, EventArgs e)
        {
            FindTextBox.Enabled = true;
            FindHexBox.Enabled = false;

            FindTextBox.Focus();
        }
        private void Find_FindHex_Checked(Object sender, EventArgs e)
        {
            FindHexBox.Enabled = true;
            FindTextBox.Enabled = false;

            FindHexBox.Focus();
        }
        private void Find_CancelButton_Click(Object sender, EventArgs e)
        {
            if (Finding)
                this.HexEditSource.AbortFind();
            else
                this.Close();
        }
        private void Find_FindOKButton_Click(Object sender, EventArgs e)
        {
            if ((FindTextRadioBox.Checked && FindTextBox.Text.Length == 0) ||
                (FindHexRadioBox.Checked && FindHexBox.Value.Length == 0))
            {
                UI.ShowMessage("The given search query is empty."); return;
            }
            try
            {
                Core_FindNext();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error during the find operation.", ex);
            }
        }
        private void Find_Timer_Tick(Object sender, EventArgs e)
        {
            if (FindingLabel.Text.Length == 13)
                FindingLabel.Text = "";

            FindingLabel.Text += ".";
        }
        private void Find_TimerPercent_Tick(Object sender, EventArgs e)
        {
            Int64 pos = this.HexEditSource.CurrentFindingPosition;
            Int64 length = HexEditSource.Value.Length;
            Double percent = (Double)pos / (Double)length * (Double)100;

            System.Globalization.NumberFormatInfo nfi =
                new System.Globalization.CultureInfo("en-US").NumberFormat;

            String text = percent.ToString("0.00", nfi) + " %";
            PercentLabel.Text = text;
        }
    }
}
