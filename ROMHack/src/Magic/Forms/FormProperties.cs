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
            NameTextBox.Text        = App.FEH.HackName;
            AuthorTextBox.Text      = App.FEH.HackAuthor;
            DescriptionTextBox.Text = App.FEH.HackDescription;
        }

        private void ApplyButton_Click(Object sender, EventArgs e)
        {
            App.FEH.HackName        = NameTextBox.Text;
            App.FEH.HackAuthor      = AuthorTextBox.Text;
            App.FEH.HackDescription = DescriptionTextBox.Text;

            this.DialogResult = DialogResult.Yes;
        }
        private void CancelButton_Click(Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
