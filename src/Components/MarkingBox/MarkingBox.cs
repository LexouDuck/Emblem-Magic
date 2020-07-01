using EmblemMagic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Components
{
    class MarkingBox : ComboBox
    {
        public MarkingBox()
        {
            this.SuspendLayout();
            
            this.Name = "MarkingBox";
            this.Size = new Size(60, 20);
            this.Click += new EventHandler(UpdateMarkList);

            this.ResumeLayout(false);

            UpdateMarkList(this, null);
        }

        void UpdateMarkList(object sender, EventArgs e)
        {
            try
            {
                DataSource = Program.Core.FEH.Marks.GetStringList(true);
            }
            catch
            {
                BindingList <string> emptylist = new BindingList<string>();
                emptylist.Add("(unmark)");
                DataSource = emptylist;
            }
        }
    }
}
