using System;
using System.Collections.Generic;

using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Nintenlord.Event_Assembler.UserInterface
{
    public partial class EnumChooser : UserControl
    {
        Point startingPoint = new Point(4, 0);
        RadioButton[] buttons;
        Type enumType;

        public EnumChooser()
        {
            InitializeComponent();
        }

        public void SetEnumEnabled(object enumTo, bool enabled)
        {
            string name = Enum.GetName(enumType, enumTo);
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Name.Equals(name))
                {
                    buttons[i].Enabled = enabled;
                }
            }
        }
        
        public void SetEnumType(Type enumType)
        {
            this.AutoSize = true;
            this.enumType = enumType;

            this.SuspendLayout();
            Point position = startingPoint;
            string[] names = Enum.GetNames(enumType);

            int maxWidth = 0;
            buttons = new RadioButton[names.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = new RadioButton();
                buttons[i].AutoSize = true;
                buttons[i].Location = position;
                buttons[i].Name = names[i];
                buttons[i].Text = names[i];
                buttons[i].UseVisualStyleBackColor = true;
                maxWidth = Math.Max(maxWidth, buttons[i].Width);
                position.Y += buttons[i].Height;
                this.Controls.Add(buttons[i]);
            }

            buttons[0].Checked = true;
            this.ResumeLayout();
            this.AutoSize = false;
        }

        public void SetChosenEnum(object enumToChoose)
        {
            string name = Enum.GetName(enumType, enumToChoose);
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Name.Equals(name))
                {
                    buttons[i].Checked = true;
                }
            }
        }

        public object GetChosenEnum()
        {
            string chosenName = null;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].Checked)
                {
                    chosenName = buttons[i].Name;
                } 
            }
            return Enum.Parse(enumType, chosenName);
        }

    }
}
