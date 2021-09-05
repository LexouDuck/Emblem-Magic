using System;
using System.Collections.Generic;

using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nintenlord.Event_Assembler.UserInterface
{
    public partial class EnumChooserForm : Form
    {
        public string Description
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public void SetEnumEnabled(object enumTo, bool enabled)
        {
            this.enumChooser1.SetEnumEnabled(enumTo, enabled);
        }

        public void SetEnumType(Type enumType)
        {
            this.enumChooser1.SetEnumType(enumType);
        }

        public void SetChosenEnum(object enumToChoose)
        {
            this.enumChooser1.SetChosenEnum(enumToChoose);
        }

        public object GetChosenEnum()
        {
            return this.enumChooser1.GetChosenEnum();
        }

        public EnumChooserForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
