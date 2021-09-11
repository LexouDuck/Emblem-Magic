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
            InitializeComponent();
            
            Space_LengthBox.Maximum = Core.CurrentROMSize;

            Output_SpaceBar.MouseLeave += new EventHandler(Output_SpaceBar_MouseLeave);
            Output_SpaceBar.MouseMove += new MouseEventHandler(Output_SpaceBar_MouseMove);

            Update_MarkTypesList();
        }

        public override void Core_OnOpen()
        {
            Core_Update();
        }
        public override void Core_Update()
        {
            try
            {
                Update_MarkTypesList();
                Update_MarksPanel();
                Update_OutputTextBox();
                Update_OutputSpaceBar();
            }
            catch (Exception ex)
            {
                UI.ShowError("There has been an error while trying to update the ROM Space Marking Editor.", ex);
            }
        }

        

        void Update_MarksPanel()
        {
            Mark current = App.MHF.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];
            if (current == null)
            {
                Marks_NameTextBox.Text = "";
                Marks_ColorBox.Color = Color.White;
                Marks_LayerNumBox.Value = 0;
            }
            else
            {
                Marks_NameTextBox.Text = current.Name;
                Marks_ColorBox.Color = current.Color;
                Marks_LayerNumBox.Value = current.Layer;
            }
        }
        void Update_MarkTypesList()
        {
            Space_MarkAsComboBox.DataSource = App.MHF.Marks.GetStringList(true);
            Marks_ListBox.DataSource = App.MHF.Marks.GetStringList(false);
        }
        void Update_OutputTextBox()
        {
            List<Space> ranges = new List<Space>(App.MHF.Space.MarkedRanges);

            switch (Sorting)
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
            Output_TextBox.Text = result;
        }
        void Update_OutputSpaceBar()
        {
            Output_SpaceBar.Load(App.ROM.FileSize, App.MHF.Space.MarkedRanges);
        }



        void Space_EndOffLabel_Click(Object sender, EventArgs e)
        {
            Space_EndByteBox.Enabled = true;
            Space_EndByteBox.Focus();
            Space_LengthBox.Enabled = false;
            Space_LengthBox.ResetText();
        }
        void Space_LengthLabel_Click(Object sender, EventArgs e)
        {
            Space_LengthBox.Enabled = true;
            Space_LengthBox.Focus();
            Space_EndByteBox.Enabled = false;
            Space_EndByteBox.ResetText();
        }
        void Space_OKButton_Click(Object sender, EventArgs e)
        {
            String mark = Space_MarkAsComboBox.SelectedItem.ToString();
            GBA.Pointer address = Space_AddressBox.Value;
            GBA.Pointer endbyte = Space_EndByteBox.Value;
            Int32 length = (Int32)Space_LengthBox.Value;

            if ((Space_EndByteLabel.Checked && (endbyte <= address)) || (Space_LengthLabel.Checked && length <= 0))
            {
                UI.ShowMessage("Marked space can't be of null or negative length.");
                Space_AddressBox.ResetText();
                Space_EndByteBox.ResetText();
                Space_LengthBox.ResetText();
                return;
            }

            if (mark == "(unmark)")
            {
                if (Space_EndByteLabel.Checked)
                    App.MHF.Space.UnmarkSpace(address, endbyte);
                else
                    App.MHF.Space.UnmarkSpace(address, address + length);
            }
            else
            {
                if (Space_EndByteLabel.Checked)
                    App.MHF.Space.MarkSpace(mark, address, endbyte);
                else
                    App.MHF.Space.MarkSpace(mark, address, address + length);
            }
            Space_AddressBox.ResetText();
            Space_EndByteBox.ResetText();
            Space_LengthBox.ResetText();

            Space_AddressBox.Focus();

            Core_Update();
        }



        void Marks_ListBox_Click(Object sender, EventArgs e)
        {
            Update_MarksPanel();
        }
        void Marks_NameTextChanged(Object sender, EventArgs e)
        {
            Mark current = App.MHF.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];

            current.Name = Marks_NameTextBox.Text;

            Core_Update();
        }
        void Marks_ColorButton_Click(Object sender, EventArgs e)
        {
            ColorDialog colorWindow = new ColorDialog();
            colorWindow.FullOpen = true;

            if (colorWindow.ShowDialog(this) == DialogResult.OK)
            {
                Mark current = App.MHF.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];

                current.Color = colorWindow.Color;

                Core_Update();
            }
        }
        void Marks_LayerValueChanged(Object sender, EventArgs e)
        {
            Mark current = App.MHF.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];

            current.Layer = (Int32)Marks_LayerNumBox.Value;

            Core_Update();
        }
        void Marks_CreateMarkButton_Click(Object sender, EventArgs e)
        {
            App.MHF.Marks.Add("NEW ", 0, Color.Black);

            Core_Update();
        }
        void Marks_DeleteMarkButton_Click(Object sender, EventArgs e)
        {
            Mark current = App.MHF.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];

            App.MHF.Marks.Remove(current);

            Core_Update();
        }



        void Output_SpaceBar_MouseMove(Object sender, MouseEventArgs e)
        {
            Double ratio = (Single)Output_SpaceBar.Total / (Single)Output_SpaceBar.Width;
            UInt32 current = (UInt32)(ratio * e.Location.X);

            Output_CurrentAddressStatusLabel.Text = Util.AddressToString(current, 8);

            foreach (var range in Output_SpaceBar.Ranges)
            {
                if (e.Location.X >= (range.Item2.Start - 1) && e.Location.X <= (range.Item2.End + 1))
                {
                    Output_SpaceAddressStatusLabel.Text = "Start Offset: " + range.Item1.Address;
                    Output_SpaceEndByteStatusLabel.Text = "End Offset: " +   range.Item1.EndByte;
                    Output_SpaceLengthStatusLabel.Text = "Length: " + range.Item1.Length.ToString();
                    Output_SpaceMarkedStatusLabel.Text = "Marked: " + range.Item1.Marked.Name;
                    return;
                }
            }
            Output_SpaceAddressStatusLabel.Text = "Start Offset: ";
            Output_SpaceEndByteStatusLabel.Text = "End Offset: ";
            Output_SpaceLengthStatusLabel.Text = "Length: ";
            Output_SpaceMarkedStatusLabel.Text = "Marked: ";
        }
        void Output_SpaceBar_MouseLeave(Object sender, EventArgs e)
        {
            Output_CurrentAddressStatusLabel.Text = "";
            Output_SpaceAddressStatusLabel.Text = "Start Offset: ";
            Output_SpaceEndByteStatusLabel.Text = "End Offset: ";
            Output_SpaceLengthStatusLabel.Text = "Length: ";
            Output_SpaceMarkedStatusLabel.Text = "Marked: ";
        }

        void Output_SortOffsetButton_Click(Object sender, EventArgs e)
        {
            Sorting = SpaceSortingMode.Offset;
            Update_OutputTextBox();
        }
        void Output_SortLengthButton_Click(Object sender, EventArgs e)
        {
            Sorting = SpaceSortingMode.Length;
            Update_OutputTextBox();
        }
        void Output_SortByNameButton_Click(Object sender, EventArgs e)
        {
            Sorting = SpaceSortingMode.Marked;
            Update_OutputTextBox();
        }
    }
}
