using Magic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Magic.Components
{
    class MarkingBox : ComboBox
    {
        MarksManager Marks;

        public MarkingBox(MarksManager marks)
        {
            Marks = marks;

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
                DataSource = Marks.GetStringList(true);
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
