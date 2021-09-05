using System;
using System.IO;
using System.Windows.Forms;
using Nintenlord.Event_Assembler.Core;
using Nintenlord.Event_Assembler.Core.Code.Language;
using Nintenlord.Utility;
using Core = Nintenlord.Event_Assembler.Core;
using Nintenlord.Event_Assembler.Core.IO.Logs;

namespace Nintenlord.Event_Assembler.UserInterface
{    
    public partial class MainForm : Form
    {
        String textFile, binaryFile;
        StringWriter lastMessages;
        
        public MainForm()
        {
            InitializeComponent();

            this.OffsetControl.Maximum = 0x20000000;
            this.LengthControl.Maximum = 0x20000000;
            this.MinimumSize = this.Size;
#if DEBUG
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
#endif
            this.Move += new EventHandler((x, y) => FormHelpers.ClingToScreenEndges(x as Form, 20));
            this.Load += new EventHandler(MainForm_Load);

            

            this.assemblyBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(
                (sender, args) =>
                {
                    lastMessages = new StringWriter();
                    var messageLog = new TextWriterMessageLog(lastMessages);
                    var tuple = args.Argument as Tuple<string, string, string>;
#if DEBUG
                    try
                    {
#endif
                        Core.Program.Assemble(tuple.Item1, tuple.Item2, tuple.Item3, messageLog);
#if DEBUG
                    }
                    catch (Exception e)
                    {
                        messageLog.AddError("Exception: " + e.ToString());
                    }
#endif
                    args.Result = messageLog;
                });
            this.assemblyBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(
                (sender, args) => PrintAll(args.Result as MessageLog));


            this.disassemblyBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(
                (sender, args) =>
                {
                    lastMessages = new StringWriter();
                    var messageLog = new TextWriterMessageLog(lastMessages);
                    var tuple = args.Argument as Tuple<string, string, string, int, int, DisassemblyMode, bool>;

                    Priority priority;
                    if (tuple.Item6 == DisassemblyMode.Block || tuple.Item6 == DisassemblyMode.ToEnd)
                    {
                        if (!GetPriority(out priority))
                        {
                            return;
                        }
                    }
                    else priority = Priority.none;

                    try
                    {
                        Core.Program.Disassemble(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item7,
                            tuple.Item6, tuple.Item4, priority, tuple.Item5, messageLog);
                    }
                    catch (Exception e)
                    {
                        messageLog.AddError("Exception: " + e.ToString());
                    }

                    args.Result = messageLog;
                });
            this.disassemblyBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(
                (sender, args) => PrintAll(args.Result as MessageLog));
        }

        public void PrintAll(MessageLog messageLog)
        {
            string message;
                messageLog.PrintAll();
            message = lastMessages.ToString();

            using (TextShower shower = new TextShower(message))
            {
                shower.Text = "";
                shower.ShowDialog();
            }
        }

#if DEBUG
        void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && (e.KeyCode == Keys.P))
            {
                lastMessages = new StringWriter();
                var messageLog = new TextWriterMessageLog(lastMessages);

                Core.Program.Preprocess(textFile, binaryFile, GetChosenGame(), messageLog);

                PrintAll(messageLog);
            }
        }
#endif

        void MainForm_Load(object sender, EventArgs e)
        {
            LoadCodes();
        }

        private void textFile_TextChanged(object sender, EventArgs e)
        {
            textFile = textFileTextBox.Text;
        }

        private void binaryFile_TextChanged(object sender, EventArgs e)
        {
            binaryFile = binaryFileTextBox.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Open text file...";
                openDialog.Filter = "Event files|*.event|Text files|*.txt|All files|*";
                if (textFile != null)
                {
                    openDialog.InitialDirectory = Path.GetDirectoryName(textFile);
                    openDialog.FileName = Path.GetFileName(textFile);
                }
                openDialog.Multiselect = false;
                openDialog.CheckFileExists = false;
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    textFile = openDialog.FileName;
                    textFileTextBox.Text = textFile;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Open ROM file...";
                openDialog.Filter = "GBA ROMs|*.gba|Binary files|*.bin|All files|*";
                if (binaryFile != null)
                {
                    openDialog.InitialDirectory = Path.GetDirectoryName(binaryFile);
                    openDialog.FileName = Path.GetFileName(binaryFile);
                }
                openDialog.Multiselect = false;
                openDialog.CheckFileExists = false;
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    binaryFile = openDialog.FileName;
                    binaryFileTextBox.Text = binaryFile;
                }
            }
        }


        private void assembleButton_Click(object sender, EventArgs e)
        {
            string game = GetChosenGame();
            if (textFile == null || binaryFile == null)
            {
                MessageBox.Show("Choose both files");
            }
            else if (!File.Exists(textFile))
            {
                MessageBox.Show("Text file doesn't exist.");
            }
            else if (!Nintenlord.Event_Assembler.Core.Program.CodesLoaded)
            {
                MessageBox.Show("Raws not loaded");
            }
            else
            {
#if DEBUG
                lastMessages = new StringWriter();
                var messageLog = new TextWriterMessageLog(lastMessages);
                Core.Program.Assemble(textFile, binaryFile, game, messageLog);
                PrintAll(messageLog);
#else
                assemblyBackgroundWorker.RunWorkerAsync(Tuple.Create(textFile, binaryFile, game));   
#endif            
            }
        }
        
        private void disassembleButton_Click(object sender, EventArgs e)
        {
            string game = GetChosenGame();
            DisassemblyMode mode = GetChosenMode();
            if (textFile == null || binaryFile == null)
            {
                MessageBox.Show("Choose both files");
            }
            else if (!File.Exists(binaryFile))
            {
                MessageBox.Show("ROM file doesn't exist.");
            }
            else if (!Nintenlord.Event_Assembler.Core.Program.CodesLoaded)
            {
                MessageBox.Show("Raws not loaded");
            }
            else
            {
#if DEBUG
                lastMessages = new StringWriter();
                var messageLog = new TextWriterMessageLog(lastMessages);

                Priority priority;
                if (mode == DisassemblyMode.Block || mode == DisassemblyMode.ToEnd)
                {
                    if (!GetPriority(out priority))
                    {
                        return;
                    }
                }
                else priority = Priority.none;
                
                Core.Program.Disassemble(binaryFile, textFile, game, this.checkBoxEnder.Checked,
                        mode, (int)OffsetControl.Value, priority, (int)LengthControl.Value, messageLog);

                PrintAll(messageLog);
#else
                disassemblyBackgroundWorker.RunWorkerAsync(Tuple.Create(binaryFile, textFile, game,
                        (int)OffsetControl.Value, (int)LengthControl.Value,
                        mode, this.checkBoxEnder.Checked)); 
#endif
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            LoadCodes();
        }


        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            LengthControl.Enabled = true;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            LengthControl.Enabled = false;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            LengthControl.Enabled = false;
        }
                
        private string GetChosenGame()
        {
            if (radioButton1.Checked)
            {
                return radioButton1.Text;
            }
            if (radioButton2.Checked)
            {
                return radioButton2.Text;
            }
            if (radioButton3.Checked)
            {
                return radioButton3.Text;
            }
            throw new Exception("Unknown game");
        }

        private DisassemblyMode GetChosenMode()
        {
            if (radioButton4.Checked)
            {
                return DisassemblyMode.Block;
            }
            if (radioButton5.Checked)
            {
                return DisassemblyMode.ToEnd;
            }
            if (radioButton6.Checked)
            {
                return DisassemblyMode.Structure;
            }
            throw new Exception("Unknown enum");
        }

        private void LoadCodes()
        {
            Core.Program.LoadCodes("Language raws", ".txt", true, false);

        }

        private bool GetPriority(out Priority priority)
        {
            Priority[] nonValidOptions = 
            {
                Priority.ASM,
                Priority.unknown
            };
            bool result;
            using (EnumChooserForm chooser = new EnumChooserForm())
            {
                chooser.SetEnumType(typeof(Priority));
                chooser.Text = "Choose an option";
                chooser.Description = "Just choose \"none\" if you don't know what to choose.";
                foreach (Priority item in nonValidOptions)
                {
                    chooser.SetEnumEnabled(item, false);
                }
                result = chooser.ShowDialog() == DialogResult.OK;
                priority = (Priority)chooser.GetChosenEnum();
            }
            ;
            return result;
        }
    }
}