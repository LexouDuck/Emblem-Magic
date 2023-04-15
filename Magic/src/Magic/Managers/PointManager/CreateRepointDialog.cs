using GBA;
using System.Windows.Forms;

namespace Magic
{
    public partial class CreateRepointDialog : Form
    {
        public CreateRepointDialog()
        {
            this.InitializeComponent();

            this.Create_Button.DialogResult = DialogResult.OK;
            this.Cancel_Button.DialogResult = DialogResult.Cancel;
        }

        public System.String AssetName
        {
            get
            {
                return this.AssetName_TextBox.Text;
            }
            set
            {
                this.AssetName_TextBox.Text = value;
            }
        }
        public Pointer DefaultAddress
        {
            get
            {
                return this.Default_PointerBox.Value;
            }
        }
        public Pointer RepointAddress
        {
            get
            {
                return this.Repoint_PointerBox.Value;
            }
        }
    }
}
