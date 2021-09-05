using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Nintenlord.Forms.Utility;

namespace Nintenlord.Event_Assembler.UserInterface
{
    public static class FormHelpers
    {
        public static void ClingToScreenEndges(Form form, int distance)
        {
            if (form.WindowState == FormWindowState.Normal)
            {
                Rectangle area = SystemInformation.WorkingArea;
                Rectangle formArea = form.DesktopBounds;
                RectangleExtensions.ClingInside2(ref area, ref formArea, distance);
                form.DesktopLocation = formArea.Location;
            }
        }

        public static void ClingToParentEdges(Form form, int distance)
        {
            if (form.Parent != null && form.Parent is Form)
            {
                Rectangle area = (form.Parent as Form).DesktopBounds;
                Rectangle formArea = form.DesktopBounds;
                RectangleExtensions.Cling(ref area, ref formArea, distance);
            }
        }
    }
}
