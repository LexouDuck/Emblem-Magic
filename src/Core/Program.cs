using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace EmblemMagic
{
    internal static class Program
    {
        public const string SoftwareName = "Emblem Magic";
        public static Suite Core;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Core = new Suite();
            Application.Run(Core);
        }



        public static void ShowMessage(string text)
        {
            MessageBox.Show(text,
                SoftwareName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        public static void ShowWarning(string text)
        {
            MessageBox.Show(text,
                SoftwareName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        public static DialogResult ShowError(string text)
        {
            DialogResult result = MessageBox.Show(
                "Error: " + text,
                SoftwareName + " - Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return result;
        }
        public static DialogResult ShowError(Exception ex)
        {
            return ShowError(GetMethodName(ex) + " : " + ex.Message);
        }
        public static DialogResult ShowError(string text, Exception ex)
        {
            return ShowError(text + "\n\n" + GetMethodName(ex) + " :\n" + ex.Message);
        }
        public static void ShowDebug(string text)
        {
            Form debug = new Form()
            {
                Width = 300, Height = 200,
            };
            TextBox textbox = new TextBox()
            {
                Multiline = true,
                Location = new System.Drawing.Point(12, 8),
                Size = new System.Drawing.Size(260, 120),
                Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left),
                Font = new System.Drawing.Font("Consolas", 8),
                Text = text
            };
            debug.Controls.Add(textbox);
            debug.ShowDialog();
        }

        public static DialogResult ShowQuestion(string text)
        {
            DialogResult result = MessageBox.Show(text, SoftwareName,
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);
            return result;
        }



        private static string GetMethodName(Exception ex)
        {
            return ex.TargetSite.DeclaringType.FullName + "." + ex.TargetSite.Name;
        }
    }
}
