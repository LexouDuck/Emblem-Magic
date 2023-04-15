using GBA;
using System.ComponentModel;
using System.Windows.Forms;

namespace Magic.Components
{
    public class PointerArrayBox : ArrayBox<Pointer>
    {
        /// <summary>
        /// Fast access to this ArrayBox's NumberBox value.
        /// </summary>
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        public override Pointer Value
        {
            get
            {
                return new Pointer((System.UInt32)this.EntryValueBox.Value);
            }
            set
            {
                this.EntryValueBox.Value = value.Address;
            }
        }



        public PointerArrayBox()
        {
            this.EntryValueBox = new PointerBox();
            this.EntryComboBox = new ComboBox();
            base.InitializeComponent();

            this.EntryValueBox.ValueChanged += this.UpdateEntryComboBox;
            this.EntryValueBox.Maximum = DataManager.ROM_MAX_SIZE;

            this.Name = "PointerArrayBox";
            this.Size = new System.Drawing.Size(128, 26);
            this.MinimumSize = new System.Drawing.Size(128, 26);
            this.EntryValueBox.Size = new System.Drawing.Size(70, 26);
            this.EntryComboBox.Location = new System.Drawing.Point(78, 2);
            this.EntryComboBox.Width = this.Width - this.EntryValueBox.Width - 10;
        }
    }
}
