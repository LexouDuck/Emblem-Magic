﻿using GBA;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic.Components
{
    public class PointerBox : NumericUpDown
    {
        [DesignerSerializationVisibility
        (DesignerSerializationVisibility.Hidden)]
        public new Pointer Value
        {
            get
            {
                return new Pointer((uint)base.Value);
            }
            set
            {
                base.Value = value.Address;
            }
        }

        public PointerBox() : base()
        {
            SuspendLayout();

            Minimum = 0;
            Maximum = DataManager.ROM_MAX_SIZE;
            Name = "PointerBox";
            Size = new Size(70, 20);
            Hexadecimal = true;
            ValueChanged += new EventHandler(UpdateMaximum);

            ResumeLayout(false);
        }

        void UpdateMaximum(object sender, EventArgs e)
        {
            try
            {
                Maximum = Core.CurrentROMSize;
            }
            catch
            {
                Maximum = 0;
            }
        }
    }
}
