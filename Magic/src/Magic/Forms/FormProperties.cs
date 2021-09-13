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
            this.App = app;

            this.InitializeComponent();

            this.LoadProperties();
        }

        void LoadProperties()
        {
            this.NameTextBox.Text        = this.App.MHF.HackName;
            this.AuthorTextBox.Text      = this.App.MHF.HackAuthor;
            this.DescriptionTextBox.Text = this.App.MHF.HackDescription;
        }

        private void ApplyButton_Click(Object sender, EventArgs e)
        {
            this.App.MHF.HackName        = this.NameTextBox.Text;
            this.App.MHF.HackAuthor      = this.AuthorTextBox.Text;
            this.App.MHF.HackDescription = this.DescriptionTextBox.Text;

            this.DialogResult = DialogResult.Yes;
        }
        private void CancelButton_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
