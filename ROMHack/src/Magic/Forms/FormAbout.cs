using System;
using System.Windows.Forms;

namespace Magic
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();

            label1.Text = "(version 0.7)"
                 + "\n\n" + "The Fire Emblem all-in-one editing tool"
                 + "\n\n" + "by (Lexou Duck)"
                 + '\n' + "Thanks to documentation and code written by:"
                 + '\n' + "(Hextator), (Nintenlord), (Zahlman), (CrazyColors)";

            label2.Text = "Fire Emblem " + '\u00A9' + " is property of Nintendo."
                 + '\n' + "Emblem Magic is in no way affiliated with Nintendo or Intelligent Systems." + '\n'
                 + '\n' + "This software is free and open source, following the GNU General Public License.";
        }
    }
}
