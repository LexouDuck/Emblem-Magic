using EmblemMagic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Editors
{
    public partial class SpaceEditor : Editor
    {
        SpaceSortingMode Sorting;

        public SpaceEditor() : base()
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
                Program.ShowError("There has been an error while trying to update the ROM Space Marking Editor.", ex);
            }
        }

        

        void Update_MarksPanel()
        {
            Mark current = Program.Core.FEH.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];
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
            Space_MarkAsComboBox.DataSource = Program.Core.FEH.Marks.GetStringList(true);
            Marks_ListBox.DataSource = Program.Core.FEH.Marks.GetStringList(false);
        }
        void Update_OutputTextBox()
        {
            List<Space> ranges = new List<Space>(Program.Core.FEH.Space.MarkedRanges);

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


            string result = "";
            string length;
            foreach (Space range in ranges)
            {
                length = "0x" + Util.UInt32ToHex((uint)range.Length);
                result += range.Marked.Name +
                    " | " + range.Address + " - " + range.EndByte +  " | " +
                    "Length: " + length.PadLeft(10) + "\r\n";
            }
            Output_TextBox.Text = result;
        }
        void Update_OutputSpaceBar()
        {
            Output_SpaceBar.Load(Program.Core.ROM.FileSize, Program.Core.FEH.Space.MarkedRanges);
        }



        void Space_EndOffLabel_Click(object sender, EventArgs e)
        {
            Space_EndByteBox.Enabled = true;
            Space_EndByteBox.Focus();
            Space_LengthBox.Enabled = false;
            Space_LengthBox.ResetText();
        }
        void Space_LengthLabel_Click(object sender, EventArgs e)
        {
            Space_LengthBox.Enabled = true;
            Space_LengthBox.Focus();
            Space_EndByteBox.Enabled = false;
            Space_EndByteBox.ResetText();
        }
        void Space_OKButton_Click(object sender, EventArgs e)
        {
            string mark = Space_MarkAsComboBox.SelectedItem.ToString();
            GBA.Pointer address = Space_AddressBox.Value;
            GBA.Pointer endbyte = Space_EndByteBox.Value;
            int length = (int)Space_LengthBox.Value;

            if ((Space_EndByteLabel.Checked && (endbyte <= address)) || (Space_LengthLabel.Checked && length <= 0))
            {
                Program.ShowMessage("Marked space can't be of null or negative length.");
                Space_AddressBox.ResetText();
                Space_EndByteBox.ResetText();
                Space_LengthBox.ResetText();
                return;
            }

            if (mark == "(unmark)")
            {
                if (Space_EndByteLabel.Checked)
                    Program.Core.FEH.Space.UnmarkSpace(address, endbyte);
                else
                    Program.Core.FEH.Space.UnmarkSpace(address, address + length);
            }
            else
            {
                if (Space_EndByteLabel.Checked)
                    Program.Core.FEH.Space.MarkSpace(mark, address, endbyte);
                else
                    Program.Core.FEH.Space.MarkSpace(mark, address, address + length);
            }
            Space_AddressBox.ResetText();
            Space_EndByteBox.ResetText();
            Space_LengthBox.ResetText();

            Space_AddressBox.Focus();

            Core_Update();
        }



        void Marks_ListBox_Click(object sender, EventArgs e)
        {
            Update_MarksPanel();
        }
        void Marks_NameTextChanged(object sender, EventArgs e)
        {
            Mark current = Program.Core.FEH.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];

            current.Name = Marks_NameTextBox.Text;

            Core_Update();
        }
        void Marks_ColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorWindow = new ColorDialog();
            colorWindow.FullOpen = true;

            if (colorWindow.ShowDialog(this) == DialogResult.OK)
            {
                Mark current = Program.Core.FEH.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];

                current.Color = colorWindow.Color;

                Core_Update();
            }
        }
        void Marks_LayerValueChanged(object sender, EventArgs e)
        {
            Mark current = Program.Core.FEH.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];

            current.Layer = (int)Marks_LayerNumBox.Value;

            Core_Update();
        }
        void Marks_CreateMarkButton_Click(object sender, EventArgs e)
        {
            Program.Core.FEH.Marks.Add("NEW ", 0, Color.Black);

            Core_Update();
        }
        void Marks_DeleteMarkButton_Click(object sender, EventArgs e)
        {
            Mark current = Program.Core.FEH.Marks.MarkingTypes[Marks_ListBox.SelectedIndex];

            Program.Core.FEH.Marks.Remove(current);

            Core_Update();
        }



        void Output_SpaceBar_MouseMove(object sender, MouseEventArgs e)
        {
            double ratio = (float)Output_SpaceBar.Total / (float)Output_SpaceBar.Width;
            uint current = (uint)(ratio * e.Location.X);

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
        void Output_SpaceBar_MouseLeave(object sender, EventArgs e)
        {
            Output_CurrentAddressStatusLabel.Text = "";
            Output_SpaceAddressStatusLabel.Text = "Start Offset: ";
            Output_SpaceEndByteStatusLabel.Text = "End Offset: ";
            Output_SpaceLengthStatusLabel.Text = "Length: ";
            Output_SpaceMarkedStatusLabel.Text = "Marked: ";
        }

        void Output_SortOffsetButton_Click(object sender, EventArgs e)
        {
            Sorting = SpaceSortingMode.Offset;
            Update_OutputTextBox();
        }
        void Output_SortLengthButton_Click(object sender, EventArgs e)
        {
            Sorting = SpaceSortingMode.Length;
            Update_OutputTextBox();
        }
        void Output_SortByNameButton_Click(object sender, EventArgs e)
        {
            Sorting = SpaceSortingMode.Marked;
            Update_OutputTextBox();
        }
    }
}
