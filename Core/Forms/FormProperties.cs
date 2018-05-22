using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmblemMagic
{
    public partial class FormProperties : Form
    {
        public FormProperties()
        {
            InitializeComponent();

            LoadProperties();
        }

        void LoadProperties()
        {
            NameTextBox.Text        = Program.Core.FEH.HackName;
            AuthorTextBox.Text      = Program.Core.FEH.HackAuthor;
            DescriptionTextBox.Text = Program.Core.FEH.HackDescription;
        }

        private void ApplyButton_Click(Object sender, EventArgs e)
        {
            Program.Core.FEH.HackName        = NameTextBox.Text;
            Program.Core.FEH.HackAuthor      = AuthorTextBox.Text;
            Program.Core.FEH.HackDescription = DescriptionTextBox.Text;

            this.DialogResult = DialogResult.Yes;
        }
        private void CancelButton_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
