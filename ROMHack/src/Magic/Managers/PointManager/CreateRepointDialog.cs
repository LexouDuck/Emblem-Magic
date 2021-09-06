using GBA;
using System.Windows.Forms;

namespace Magic
{
    public partial class CreateRepointDialog : Form
    {
        public CreateRepointDialog()
        {
            InitializeComponent();

            Create_Button.DialogResult = DialogResult.OK;
            Cancel_Button.DialogResult = DialogResult.Cancel;
        }

        public System.String AssetName
        {
            get
            {
                return AssetName_TextBox.Text;
            }
            set
            {
                AssetName_TextBox.Text = value;
            }
        }
        public Pointer DefaultAddress
        {
            get
            {
                return Default_PointerBox.Value;
            }
        }
        public Pointer RepointAddress
        {
            get
            {
                return Repoint_PointerBox.Value;
            }
        }
    }
}
