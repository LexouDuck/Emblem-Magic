using Magic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Editors
{
    public partial class SpaceEditor : Editor
    {
        SpaceSortingMode Sorting;

        public SpaceEditor()
        {
            this.InitializeComponent();

            this.Space_LengthBox.Maximum = Core.CurrentROMSize;

            this.Output_SpaceBar.MouseLeave += new EventHandler(this.Output_SpaceBar_MouseLeave);
            this.Output_SpaceBar.MouseMove += new MouseEventHandler(this.Output_SpaceBar_MouseMove);

            this.Update_MarkTypesList();
        }

        public override void Core_OnOpen()
        {
            this.Core_Update();
        }
        public override void Core_Update()
        {
            try
            {
                this.Update_MarkTypesList();
                this.Update_MarksPanel();
                this.Update_OutputTextBox();
                this.Update_OutputSpaceBar();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to update the ROM Space Marking Editor.", ex);
            }
        }

        

        void Update_MarksPanel()
        {
            Mark current = this.App.MHF.Marks.MarkingTypes[this.Marks_ListBox.SelectedIndex];
            if (current == null)
            {
                this.Marks_NameTextBox.Text = "";
                this.Marks_ColorBox.Color = Color.White;
                this.Marks_LayerNumBox.Value = 0;
            }
            else
            {
                this.Marks_NameTextBox.Text = current.Name;
                this.Marks_ColorBox.Color = current.Color;
                this.Marks_LayerNumBox.Value = current.Layer;
            }
        }
        void Update_MarkTypesList()
        {
            this.Space_MarkAsComboBox.DataSource = this.App.MHF.Marks.GetStringList(true);
            this.Marks_ListBox.DataSource = this.App.MHF.Marks.GetStringList(false);
        }
        void Update_OutputTextBox()
        {
            List<Space> ranges = new List<Space>(this.App.MHF.Space.MarkedRanges);

            switch (this.Sorting)
            {
                case SpaceSortingMode.Marked: ranges.Sort(delegate (Space first, Space second)
                {
                    return first.Marked.Name.CompareTo(second.Marked.Name);
                }); break;
                case SpaceSortingMode.Offset: ranges.Sort(delegate (Space first, Space second)
                {
                    return (first.Address - second.Address);
                }); break;
                case SpaceSortingMode.Length: ranges.Sort(delegate (Space first, Space second)
                {
                    return (first.Length - second.Length) ;
                }); break;
            }


            String result = "";
            String length;
            foreach (Space range in ranges)
            {
                length = "0x" + Util.UInt32ToHex((UInt32)range.Length);
                result += range.Marked.Name +
                    " | " + range.Address + " - " + range.EndByte +  " | " +
                    "Length: " + length.PadLeft(10) + "\r\n";
            }
            this.Output_TextBox.Text = result;
        }
        void Update_OutputSpaceBar()
        {
            this.Output_SpaceBar.Load(this.App.ROM.FileSize, this.App.MHF.Space.MarkedRanges);
        }



        void Space_EndOffLabel_Click(Object sender, EventArgs e)
        {
            this.Space_EndByteBox.Enabled = true;
            this.Space_EndByteBox.Focus();
            this.Space_LengthBox.Enabled = false;
            this.Space_LengthBox.ResetText();
        }
        void Space_LengthLabel_Click(Object sender, EventArgs e)
        {
            this.Space_LengthBox.Enabled = true;
            this.Space_LengthBox.Focus();
            this.Space_EndByteBox.Enabled = false;
            this.Space_EndByteBox.ResetText();
        }
        void Space_OKButton_Click(Object sender, EventArgs e)
        {
            String mark = this.Space_MarkAsComboBox.SelectedItem.ToString();
            GBA.Pointer address = this.Space_AddressBox.Value;
            GBA.Pointer endbyte = this.Space_EndByteBox.Value;
            Int32 length = (Int32)this.Space_LengthBox.Value;

            if ((this.Space_EndByteLabel.Checked && (endbyte <= address)) || (this.Space_LengthLabel.Checked && length <= 0))
            {
                UI.ShowMessage("Marked space can't be of null or negative length.");
                this.Space_AddressBox.ResetText();
                this.Space_EndByteBox.ResetText();
                this.Space_LengthBox.ResetText();
                return;
            }

            if (mark == "(unmark)")
            {
                if (this.Space_EndByteLabel.Checked)
                    this.App.MHF.Space.UnmarkSpace(address, endbyte);
                else
                    this.App.MHF.Space.UnmarkSpace(address, address + length);
            }
            else
            {
                if (this.Space_EndByteLabel.Checked)
                    this.App.MHF.Space.MarkSpace(mark, address, endbyte);
                else
                    this.App.MHF.Space.MarkSpace(mark, address, address + length);
            }
            this.Space_AddressBox.ResetText();
            this.Space_EndByteBox.ResetText();
            this.Space_LengthBox.ResetText();

            this.Space_AddressBox.Focus();

            this.Core_Update();
        }



        void Marks_ListBox_Click(Object sender, EventArgs e)
        {
            this.Update_MarksPanel();
        }
        void Marks_NameTextChanged(Object sender, EventArgs e)
        {
            Mark current = this.App.MHF.Marks.MarkingTypes[this.Marks_ListBox.SelectedIndex];

            current.Name = this.Marks_NameTextBox.Text;

            this.Core_Update();
        }
        void Marks_ColorButton_Click(Object sender, EventArgs e)
        {
            ColorDialog colorWindow = new ColorDialog();
            colorWindow.FullOpen = true;

            if (colorWindow.ShowDialog(this) == DialogResult.OK)
            {
                Mark current = this.App.MHF.Marks.MarkingTypes[this.Marks_ListBox.SelectedIndex];

                current.Color = colorWindow.Color;

                this.Core_Update();
            }
        }
        void Marks_LayerValueChanged(Object sender, EventArgs e)
        {
            Mark current = this.App.MHF.Marks.MarkingTypes[this.Marks_ListBox.SelectedIndex];

            current.Layer = (Int32)this.Marks_LayerNumBox.Value;

            this.Core_Update();
        }
        void Marks_CreateMarkButton_Click(Object sender, EventArgs e)
        {
            this.App.MHF.Marks.Add("NEW ", 0, Color.Black);

            this.Core_Update();
        }
        void Marks_DeleteMarkButton_Click(Object sender, EventArgs e)
        {
            Mark current = this.App.MHF.Marks.MarkingTypes[this.Marks_ListBox.SelectedIndex];

            this.App.MHF.Marks.Remove(current);

            this.Core_Update();
        }



        void Output_SpaceBar_MouseMove(Object sender, MouseEventArgs e)
        {
            Double ratio = (Single)this.Output_SpaceBar.Total / (Single)this.Output_SpaceBar.Width;
            UInt32 current = (UInt32)(ratio * e.Location.X);

            this.Output_CurrentAddressStatusLabel.Text = Util.AddressToString(current, 8);

            foreach (var range in this.Output_SpaceBar.Ranges)
            {
                if (e.Location.X >= (range.Item2.Start - 1) && e.Location.X <= (range.Item2.End + 1))
                {
                    this.Output_SpaceAddressStatusLabel.Text = "Start Offset: " + range.Item1.Address;
                    this.Output_SpaceEndByteStatusLabel.Text = "End Offset: " +   range.Item1.EndByte;
                    this.Output_SpaceLengthStatusLabel.Text = "Length: " + range.Item1.Length.ToString();
                    this.Output_SpaceMarkedStatusLabel.Text = "Marked: " + range.Item1.Marked.Name;
                    return;
                }
            }
            this.Output_SpaceAddressStatusLabel.Text = "Start Offset: ";
            this.Output_SpaceEndByteStatusLabel.Text = "End Offset: ";
            this.Output_SpaceLengthStatusLabel.Text = "Length: ";
            this.Output_SpaceMarkedStatusLabel.Text = "Marked: ";
        }
        void Output_SpaceBar_MouseLeave(Object sender, EventArgs e)
        {
            this.Output_CurrentAddressStatusLabel.Text = "";
            this.Output_SpaceAddressStatusLabel.Text = "Start Offset: ";
            this.Output_SpaceEndByteStatusLabel.Text = "End Offset: ";
            this.Output_SpaceLengthStatusLabel.Text = "Length: ";
            this.Output_SpaceMarkedStatusLabel.Text = "Marked: ";
        }

        void Output_SortOffsetButton_Click(Object sender, EventArgs e)
        {
            this.Sorting = SpaceSortingMode.Offset;
            this.Update_OutputTextBox();
        }
        void Output_SortLengthButton_Click(Object sender, EventArgs e)
        {
            this.Sorting = SpaceSortingMode.Length;
            this.Update_OutputTextBox();
        }
        void Output_SortByNameButton_Click(Object sender, EventArgs e)
        {
            this.Sorting = SpaceSortingMode.Marked;
            this.Update_OutputTextBox();
        }
    }
}
