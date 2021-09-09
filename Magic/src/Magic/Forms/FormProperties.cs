using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magic
{
    public partial class FormProperties : Form
    {
        IApp App;

        public FormProperties(IApp app)
        {
            App = app;

            InitializeComponent();

            LoadProperties();
        }

        void LoadProperties()
        {
            NameTextBox.Text        = App.MHF.HackName;
            AuthorTextBox.Text      = App.MHF.HackAuthor;
            DescriptionTextBox.Text = App.MHF.HackDescription;
        }

        private void ApplyButton_Click(Object sender, EventArgs e)
        {
            App.MHF.HackName        = NameTextBox.Text;
            App.MHF.HackAuthor      = AuthorTextBox.Text;
            App.MHF.HackDescription = DescriptionTextBox.Text;

            this.DialogResult = DialogResult.Yes;
        }
        private void CancelButton_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
