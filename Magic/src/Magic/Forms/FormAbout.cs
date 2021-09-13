using System;
using System.Windows.Forms;

namespace Magic
{
    public partial class FormAbout : Form
    {
        public FormAbout(String name,
            String text_about,
            String text_legal,
            System.Drawing.Icon icon_small,
            System.Drawing.Image icon_large)
        {
            this.InitializeComponent();

            this.Icon = icon_small;
            this.TitleLabel.Image = icon_large;
            this.TitleLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TitleLabel.Text = name;
            this.label1.Text = text_about;
            this.label2.Text = text_legal;
        }
    }
}
