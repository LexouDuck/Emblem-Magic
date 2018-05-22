using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EmblemMagic.Components
{
    class ControlPainting
    {

        private const int WM_SETREDRAW = 11;

        public static void SuspendPainting(Control control)
        {
            NativeMethods.SendMessage(control.Handle, WM_SETREDRAW, (IntPtr)0, (IntPtr)0);
        }

        public static void ResumePainting(Control control)
        {
            NativeMethods.SendMessage(control.Handle, WM_SETREDRAW, (IntPtr)1, (IntPtr)0);
            control.Refresh();
        }
    }
}
