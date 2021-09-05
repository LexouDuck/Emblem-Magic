using System;
using System.Windows.Forms;
using Magic;

namespace EmblemMagic
{
    internal static class Program
    {
        public static App Core;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Core = new App("Emblem Magic");
            Application.Run(Core);
        }
    }
}
