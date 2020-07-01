using EmblemMagic.Components;
using EmblemMagic.Editors;
using EmblemMagic.Properties;
using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic
{
    public static class Prompt
    {
        /// <summary>
        /// Shows a window with a highlightable result label
        /// </summary>
        public static void ShowResult(string text, string caption, string result)
        {
            MessageBox.Show(result, caption, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Show a simple text input prompt, returns empty string if cancelled
        /// </summary>
        public static string ShowTextDialog(string text, string caption)
        {
            Form dialog = new Form()
            {
                Width = 500,
                Height = 150,
                Text = caption,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label label = new Label()
            {
                Left = 50,
                Top = 20,
                Text = text
            };
            TextBox input = new TextBox()
            {
                Left = 50,
                Top = 50,
                Width = 400
            };
            Button confirm = new Button()
            {
                Text = "Ok",
                Left = 350,
                Width = 100,
                Top = 70,
                DialogResult = DialogResult.OK
            };
            confirm.Click += (sender, e) => { dialog.Close(); };
            dialog.Controls.Add(label);
            dialog.Controls.Add(input);
            dialog.Controls.Add(confirm);
            dialog.AcceptButton = confirm;

            return dialog.ShowDialog() == DialogResult.OK ? input.Text : "";
        }
        /// <summary>
        /// Show a simple number input prompt, returns 0 if cancelled
        /// </summary>
        public static int ShowNumberDialog(string text, string caption, bool hex, int min, int max)
        {
            Label label = new Label()
            {
                Text = text,
                Left = 25,
                Top = 15,
                AutoSize = false,
                Size = new Size(250, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Form dialog = new Form()
            {
                Width = 300,
                Height = 100 + label.Height,
                Text = caption,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };
            NumericUpDown input = new NumericUpDown()
            {
                Left = 80,
                Top = 25 + label.Height,
                Width = 80,
                Minimum = min,
                Maximum = max,
                Hexadecimal = hex
            };
            Button confirm = new Button()
            {
                Text = "OK",
                Left = dialog.Width - 120,
                Top = 25 + label.Height,
                Width = 80,
                DialogResult = DialogResult.OK
            };
            confirm.Click += (sender, e) => { dialog.Close(); };
            dialog.Controls.Add(input);
            dialog.Controls.Add(confirm);
            dialog.Controls.Add(label);
            dialog.AcceptButton = confirm;

            return dialog.ShowDialog() == DialogResult.OK ? (int)input.Value : 0;
        }
        /// <summary>
        /// Shows a simple number input prompt, but for a pointer
        /// </summary>
        public static Pointer ShowPointerDialog(string text, string caption)
        {
            return new Pointer((uint)ShowNumberDialog(text, caption, true, 0, (int)Core.CurrentROMSize));
        }
        /// <summary>
        /// Shows a simple number input prompt, but for a pointer
        /// </summary>
        public static Pointer ShowPointerArrayBoxDialog(string text, string caption)
        {
            Label label = new Label()
            {
                Text = text,
                Left = 25,
                Top = 15,
                AutoSize = false,
                Size = new Size(250, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Form dialog = new Form()
            {
                Width = 300,
                Height = 100 + label.Height,
                Text = caption,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen
            };
            Button confirm = new Button()
            {
                Text = "OK",
                Left = dialog.Width - 100,
                Top = 25 + label.Height,
                Width = 60,
                DialogResult = DialogResult.OK
            };
            confirm.Click += (sender, e) => { dialog.Close(); };
            PointerArrayBox input = new PointerArrayBox()
            {
                Left = 20,
                Top = 25 + label.Height,
                Width = 160,
                Maximum = new GBA.Pointer(Core.CurrentROMSize),
            };
            input.Load("Chapter Unit Pointers.txt");
            dialog.Controls.Add(input);
            dialog.Controls.Add(confirm);
            dialog.Controls.Add(label);
            dialog.AcceptButton = confirm;

            return (dialog.ShowDialog() == DialogResult.OK ? input.Value : new GBA.Pointer());
        }

        /// <summary>
        /// Shows a multiple-entry repoint prompt
        /// </summary>
        /// <param name="sender">The Editor that should write the repoints</param>
        /// <param name="caption">The title of the prompt window</param>
        /// <param name="text">The text displayed in the repoint prompt window</param>
        /// <param name="writePrefix">The common prefix the write descriptions should have</param>
        /// <param name="repoints">the actual repoints, tuple is: Name, CurrentAddress, DataLength</param>
        /// <param name="addresses">the addresses at which to write the new pointers, if null does find+replace</param>
        /// <returns>'true' if the operation was cancelled</returns>
        public static bool ShowRepointDialog(Editor sender,
            string caption, string text, string writePrefix,
            Tuple<string, Pointer, int>[] repoints, Pointer[] addresses = null)
        {
            if (Settings.Default.WriteToFreeSpace)
            {
                int length = 0;
                for (int i = 0; i < repoints.Length; i++)
                {
                    length += repoints[i].Item3;
                }
                Pointer[] pointers = new Pointer[repoints.Length];
                pointers[0] = Core.GetFreeSpace(length);
                length = repoints[0].Item3;
                for (int i = 1; i < repoints.Length; i++)
                {
                    pointers[i] = pointers[0] + length;
                    length += repoints[i].Item3;
                }
                if (addresses == null)
                {
                    for (int i = 0; i < repoints.Length; i++)
                    {
                        Core.Repoint(sender,
                            repoints[i].Item2,
                            pointers[i],
                            writePrefix + repoints[i].Item1 + " repoint");
                    }
                }
                else
                {
                    for (int i = 0; i < repoints.Length; i++)
                    {
                        Core.WritePointer(sender,
                            addresses[i],
                            pointers[i],
                            writePrefix + repoints[i].Item1 + " repoint");
                    }
                }
            }
            else if (Settings.Default.PromptRepoints)
            {
                FormRepoint Dialog = new FormRepoint(caption, text, repoints);

                if (Dialog.ShowDialog() == DialogResult.OK)
                {
                    if (addresses == null)
                    {
                        for (int i = 0; i < Dialog.Boxes.Length; i++)
                        {
                            if (Dialog.Boxes[i].Value != repoints[i].Item2)
                            {
                                Core.Repoint(sender,
                                    repoints[i].Item2,
                                    Dialog.Boxes[i].Value,
                                    writePrefix + repoints[i].Item1 + " repoint");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Dialog.Boxes.Length; i++)
                        {
                            if (Dialog.Boxes[i].Value != Core.ReadPointer(addresses[i]))
                            {
                                Core.WritePointer(sender,
                                    addresses[i],
                                    Dialog.Boxes[i].Value,
                                    writePrefix + repoints[i].Item1 + " repoint");
                            }
                        }
                    }
                    return false;
                }
                else return true;
            }
            return false;
        }
        /// <summary>
        /// Shows a prompt asking to create a FEH Repoint tracker and returns that
        /// </summary>
        public static Repoint ShowRepointCreateDialog(string assetName = "")
        {
            CreateRepointDialog dialog = new CreateRepointDialog();

            if (assetName.Length != 0) dialog.AssetName = assetName;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Repoint repoint = new Repoint(dialog.AssetName, dialog.DefaultAddress);

                if (dialog.RepointAddress != new Pointer() && dialog.RepointAddress != dialog.DefaultAddress)
                {
                    repoint.CurrentAddress = dialog.RepointAddress;
                }
                return repoint;
            }
            else return null;
        }



        public static DialogResult SaveFEHChanges()
        {
            return MessageBox.Show(
            "The hack has been modified, would you like to save the FEH file ?",

            "Save changes to FE Hack ?",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Warning);
        }
        public static DialogResult SaveROMChanges()
        {
            return MessageBox.Show(
            "This ROM has unsaved changes, would you like to save the .gba file ?",

            "Save changes to ROM ?",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Warning);
        }
        
        public static DialogResult ChangeSeveralWrites()
        {
            return MessageBox.Show(
            "There are several writes selected.\n" + 
            "Are you sure you want to change all of them to have the same data ?",

            "Apply changes to several writes ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        public static DialogResult DeleteSeveralWrites()
        {
            return MessageBox.Show(
            "There are several writes selected.\n" +
            "Are you sure you want to delete all of them ?",

            "Delete several writes ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }

        public static DialogResult AskForFEHForHackedROM()
        {
            return MessageBox.Show(
            "The ROM loaded is a hacked ROM.\n" +
            "Click 'Yes' to browse for an FEH corresponding to this ROM if you have one.\n" +
            "Otherwise, the program will generate FEH hack information for this ROM.",

            "Open FEH for Hacked ROM ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        public static void ResolveUnreferencedPointers(List<Repoint> unreferenced)
        {
            Tuple<string, Pointer, int>[] pointers = new Tuple<string, Pointer, int>[unreferenced.Count];
            string[] pointer_names = new string[unreferenced.Count];
            for (int i = 0; i < unreferenced.Count; i++)
            {
                pointers[i] = Tuple.Create(unreferenced[i].AssetName, new Pointer(), (int)unreferenced[i].DefaultAddress);
                pointer_names[i] = unreferenced[i].AssetName;
            }
            FormRepoint Dialog = new FormRepoint("Unreferenced pointers to resolve",
            (unreferenced.Count == 1 ? "An important pointer " : ("A total of " + unreferenced.Count + " important pointers ")) +
            " have 0 references in this ROM - " + (unreferenced.Count == 1 ? "it" : "they") + " must have been repointed." +
            "\n"+"If you know the new value" + (unreferenced.Count == 1 ? " for this pointer" : "s for these pointers") + ", you can enter that in the numerical input boxes below, and click 'Repoint'." +
            "\n"+"Otherwise, you can click 'Cancel' to allow Emblem Magic to find the new pointer value" + (unreferenced.Count == 1 ? "s" : "")+ " on its own." +
            "\n"+"If there are any numerical boxes left empty when clicking 'Repoint', Emblem Magic will find the value for those on its own.",
            pointers,
            500);
            DialogResult answer = Dialog.ShowDialog();
            Pointer address = new GBA.Pointer();
            for (int i = 0; i < unreferenced.Count; i++)
            {
                if (answer == DialogResult.Cancel || Dialog.Boxes[i].Value == 0)
                {
                    try
                    {
                        DataManager rom = new DataManager();
                        rom.OpenFile(Core.Path_CleanROM);
                        address = Core.ReadPointer(rom.Find(unreferenced[i].DefaultAddress.ToBytes(), 4));
                    }
                    catch (Exception ex)
                    {
                        Program.ShowError("Error while searching for the " + unreferenced[i].AssetName + " pointer.", ex);
                    }
                }
                else if (answer == DialogResult.Yes)
                {
                    address = Dialog.Boxes[i].Value;
                }
                unreferenced[i].CurrentAddress = address;
                unreferenced[i].UpdateReferences();
            }
        }

        public static DialogResult ChangeROMSize()
        {
            return MessageBox.Show(
            "This action will change the file size of the ROM.\n" +
            "Are you sure you want to proceed ?",
            "Change ROM size ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        
        public static DialogResult ApplyEditorChanges(string editor)
        {
            return MessageBox.Show(
            "There have been changes made in the " + editor + ".\r\n Would you like to keep them applied ?",

            "Apply changes from this Editor ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        
        public static DialogResult ApplyWritesToROM(int writeTotal, int writesMissing)
        {
            return MessageBox.Show(
            "The current ROM file and the FEH file have some differences.\n" +
            "Of the " + writeTotal + " writes in this FEH file,\n" +
            writesMissing + " writes are not applied to the loaded ROM file.\n" +
            "Would you like to apply them ? (Clicking 'No' will delete these writes)",

            "Apply writes from file to current ROM ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        public static DialogResult DifferentGameTypes()
        {
            return MessageBox.Show(
            "The FEH file and the loaded ROM are of different games/versions.\n" +
            "Are you sure you want to proceed ?",

            "ROM and FEH aren't the same game",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        
        public static DialogResult WriteConflict(List<WriteConflict> conflict)
        {
            string message;

            if (conflict == null || conflict.Count == 0) return DialogResult.Yes;
            else if (conflict.Count == 1)
            {
                message = "A conflicting write was found at address " + conflict[0].Write.Address + "\n\n" +
                    "It was done by the " + conflict[0].Write.Author + " at the following time: " + conflict[0].Write.Time + '\n' +
                    "And it has the following description:\n" + conflict[0].Write.Phrase + "\n\n" +
                    "Would you like to overwrite it ?";
            }
            else
            {
                message = "Several existing writes will be affected by this action:\n\n";
                for (int i = 0; i < conflict.Count; i++)
                {
                    message += "by the " + conflict[i].Write.Author + " at address " + conflict[0].Write.Address;
                    message += "\ndescription: " + conflict[0].Write.Phrase + "\n\n";
                }
                message += "Are you sure you wish to proceed ?";
            }

            return MessageBox.Show(message, "Overwrite changes to ROM ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        public static DialogResult OverWriteRestoreRemainder()
        {
            return MessageBox.Show(
            "The pre-existing write was longer than the new one.\n" +
            "Would you like to restore the remaining portion of data from the loaded ROM file ?",

            "Restore ROM data after overwrite ?",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Warning);
        }
        
        public static DialogResult CreateMarkingType()
        {
            return MessageBox.Show(
            "The given space marking type does not exist.\n" +
            "Would you like to create it ?",

            "Create new Marking Type ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        public static DialogResult RemoveMarkingType()
        {
            return MessageBox.Show(
            "Deleting this Marking type will also delete any ranges of space marked as such.\n" +
            "Would you still like to proceed ?",

            "Remove marking type and its ranges ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
        
        public static DialogResult SaveHexChanges()
        {
            return MessageBox.Show(
            "Would you like to save the changes made to the file ?",

            "Save changes ?",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Warning);
        }

        public static DialogResult UpdateArrayFile()
        {
            if (Settings.Default.UseCustomPathArrays)
                return DialogResult.Yes;

            return MessageBox.Show(
            "You are about to edit an array from the 'Arrays' folder. These text files are meant to represent vanilla game data.\r\n" +
            "You may instead want to make a copy of this 'Arrays' folder and set the 'Use Custom Array Path' in the Options window.\r\n" +
            "In that case, click 'No'. Otherwise, if you wish to proceed, click 'Yes'.",

            "Edit Array File ?",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);
        }
    }
}
