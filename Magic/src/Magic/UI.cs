using System;
using System.Windows.Forms;
using Magic;
using Magic.Editors;
using Magic.Properties;
using GBA;

namespace Magic
{
    public static class UI
    {
        public static IApp App;



        private static String GetMethodName(Exception ex)
        {
            return ex.TargetSite.DeclaringType.FullName + "." + ex.TargetSite.Name;
        }

        //============================= Popup messages ============================

        /// <summary>
        /// Displays a simple modal popup message to the user
        /// </summary>
        /// <param name="text">The text to display in this message popup</param>
        public static void ShowMessage(String text)
        {
            MessageBox.Show(text,
                App.AppName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Displays a simple modal popup warning message to the user
        /// </summary>
        /// <param name="text">The text to display in this warning popup</param>
        public static void ShowWarning(String text)
        {
            MessageBox.Show(text,
                App.AppName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Displays a simple modal popup error message to the user
        /// </summary>
        /// <param name="text">The text to display in this error popup</param>
        /// <param name="ex">The exception holding the actual error message to display, if applicable</param>
        public static DialogResult ShowError(String text)
        {
            DialogResult result = MessageBox.Show(
                "Error: " + text,
                App.AppName + " - Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return result;
        }
        public static DialogResult ShowError(Exception ex)
        {
            return ShowError(GetMethodName(ex) + " : " + ex.Message);
        }
        public static DialogResult ShowError(String text, Exception ex)
        {
            return ShowError(text + "\n\n" + GetMethodName(ex) + " :\n" + ex.Message);
        }

        /// <summary>
        /// Displays a simple modal popup debug message to the user (in a copy/paste-able TextBox)
        /// </summary>
        /// <param name="text">The text to display in this debug message popup</param>
        public static void ShowDebug(String text)
        {
            Form debug = new Form()
            {
                Width = 300,
                Height = 200,
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

        /// <summary>
        /// Displays a question prompt modal popup to the user (with Yes/No buttons for the user to reply)
        /// </summary>
        /// <param name="text">The text to display in this debug message popup</param>
        public static DialogResult ShowQuestion(String text)
        {
            DialogResult result = MessageBox.Show(text,
                App.AppName,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            return result;
        }

        //============================= Editor Updating ============================

        /// <summary>
        /// Stops all editor windows from updating (until ResumeUpdate is called)
        /// </summary>
        public static void SuspendUpdate()
        {
            App.Suspend = true;
        }

        /// <summary>
        /// Allows editor windows to update automatically, as they normally do upon writing
        /// </summary>
        public static void ResumeUpdate()
        {
            App.Suspend = false;
        }

        /// <summary>
        /// Manually forces all editor windows to update
        /// </summary>
        public static void PerformUpdate()
        {
            App.Suspend = false;
            App.Core_Update();
        }

        //============================= Contextual Editors ============================

        /// <summary>
        /// Opens a PaletteEditor for the palette at 'address' (set paletteAmount as 0 if palette is compressed)
        /// </summary>
        public static void OpenPaletteEditor(
            Editor sender,
            String prefix,
            Pointer address,
            Int32 paletteAmount)
        {
            App.Core_OpenEditor(new PaletteEditor(
                sender,
                prefix,
                address,
                (Byte)paletteAmount));
        }

        /// <summary>
        /// Opens a TSA Editor for the TSA array at 'address'
        /// </summary>
        public static void OpenTSAEditor(
            Editor sender,
            String prefix,
            Pointer palette_address, Int32 palette_length,
            Pointer tileset_address, Int32 tileset_length,
            Pointer tsa_address,
            Int32 tsa_width, Int32 tsa_height,
            Boolean compressed,
            Boolean flipRows)
        {
            App.Core_OpenEditor(new TSAEditor(
                sender,
                prefix,
                palette_address, palette_length,
                tileset_address, tileset_length,
                tsa_address,
                tsa_width, tsa_height,
                compressed,
                flipRows));
        }

        /// <summary>
        /// Opens an OAM Editor for the OAM data at 'address'
        /// </summary>
        public static void OpenOAMEditor(
            Editor sender,
            String prefix,
            Pointer address,
            Int32 compressed,
            Int32 offsetX, Int32 offsetY,
            Palette palette,
            Tileset tileset)
        {
            App.Core_OpenEditor(new OAMEditor(
                sender,
                prefix,
                address,
                compressed,
                offsetX, offsetY,
                palette,
                tileset));
        }
    }
}
