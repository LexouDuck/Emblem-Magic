﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EmblemMagic
{
    public class MarksManager
    {
        /// <summary>
        /// The list that types of space marking types exist for this hack.
        /// </summary>
        public List<Mark> MarkingTypes { get; set; }



        public MarksManager()
        {
            MarkingTypes = new List<Mark>();

            MarkingTypes.Add(Mark.FREE);
            MarkingTypes.Add(Mark.USED);
        }



        /// <summary>
        /// Returns the marking type that holds the given string.
        /// </summary>
        public Mark Get(string markname)
        {
            foreach (var mark in MarkingTypes)
            {
                if (mark.Name.Equals(markname))
                {
                    return mark;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a new Mark to the list from the given parameters.
        /// </summary>
        /// <param name="name">the 4-char string of this marking type</param>
        /// <param name="layer">the layer on which this marking type goes (so as to conflict with other types)</param>
        /// <param name="color">the color associated with this marking type</param>
        /// <returns>The Mark that was just added.</returns>
        public Mark Add(string name, int layer, Color color)
        {
            Mark mark = new Mark(name, layer, color);
            MarkingTypes.Add(mark);

            Program.Core.FEH.Changed = true;

            return mark;
        }

        /// <summary>
        /// Removes the given marking type from the list.
        /// </summary>
        /// <param name="mark"></param>
        public void Remove(Mark mark)
        {
            if (Prompt.RemoveMarkingType() == DialogResult.Yes)
            {
                MarkingTypes.Remove(mark);

                Program.Core.FEH.Space.RemoveAllMarkedAs(mark);

                Program.Core.FEH.Changed = true;
            }
        }



        /// <summary>
        /// Returns a list of strings of the marking types - used to populate ComboBoxes mainly.
        /// </summary>
        /// <param name="includeUnmark">Whether or not "(unmark)" should be included as the first item</param>
        public BindingList<string> GetStringList(bool includeUnmark)
        {
            BindingList<string> result = new BindingList<string>();
            if (includeUnmark)
            {
                result.Add("(unmark)");
            }
            foreach (var mark in MarkingTypes)
            {
                result.Add(mark.Name);
            }
            return result;
        }
    }
}
