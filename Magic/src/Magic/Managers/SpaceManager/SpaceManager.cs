using GBA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Magic
{
    /// <summary>
    /// This manager keeps in store and updates ranges of marked space in the ROM, be it free, used, or anything
    /// </summary>
    public class SpaceManager
    {
        /// <summary>
        /// the first int is the index in MarksList of what this space is marked as, then its offset and endoffset
        /// </summary>
        public List<Space> MarkedRanges { get; set; }



        public IApp App;
        public SpaceManager(IApp app)
        {
            this.App = app;
            this.MarkedRanges = new List<Space>();
        }
        public void Load(Range[] free_space)
        {
            for (Int32 i = 0; i < free_space.Length; i++)
            {
                this.MarkSpace("FREE",
                    new Pointer(free_space[i].Start),
                    new Pointer(free_space[i].End));
            }
        }

        
        
        /// <summary>
        /// Marks the given range of space in the ROM, fuses with other ranges or overwrites them appropriately
        /// </summary>
        public void MarkSpace(String markname, Pointer address, Pointer endbyte)
        {
            Mark mark = this.App.MHF.Marks.Get(markname);
            if (mark == null)
            {
                if (Prompt.CreateMarkingType() == DialogResult.Yes)
                {
                    mark = this.App.MHF.Marks.Add(markname, 0, System.Drawing.Color.Aquamarine);
                }
                else return;
            }

            Int32 i = 0;
            while (i < this.MarkedRanges.Count && address > this.MarkedRanges[i].Address)
            { i++; }

            this.MarkedRanges.Insert(i, new Space(mark, address, endbyte));

            this.ListCleanup(i);

            this.App.MHF.Changed = true;
        }
        /// <summary>
        /// Unmarks the given range of space in the ROM, shortens or deletes existing ranges appropriately.
        /// </summary>
        public void UnmarkSpace(Pointer address, Pointer endbyte)
        {
            Int32 i = 0;
            while (i < this.MarkedRanges.Count && address > this.MarkedRanges[i].Address)
            { i++; }

            if (this.MarkedRanges.Count == 0)
            {
                return;
            }
            else
            {
                Boolean check = true;
                Mark mark = this.MarkedRanges[i].Marked;

                if (address < this.MarkedRanges[i].EndByte)
                {   // if there's an overlap with preceding range
                    if (endbyte < this.MarkedRanges[i].EndByte)
                    {   // if the range to remove is within a bigger one
                        Int32 first = this.MarkedRanges[i].Address;
                        Int32 last = this.MarkedRanges[i].EndByte;
                        this.MarkedRanges.RemoveAt(i);
                        this.MarkedRanges.Insert(i, new Space(mark, endbyte, last));
                        this.MarkedRanges.Insert(i, new Space(mark, first, address));
                        check = false;
                    }   // split the preceding range in 2
                    else
                    {   // so it's an overlap
                        Int32 first = this.MarkedRanges[i].Address;
                        this.MarkedRanges.RemoveAt(i);
                        this.MarkedRanges.Insert(i, new Space(mark, first, address));

                        i++;
                        if (i < this.MarkedRanges.Count)
                            mark = this.MarkedRanges[i].Marked;
                    }   // remove the end of the preceding range
                }
                else
                {
                    i++;
                    if (i < this.MarkedRanges.Count)
                        mark = this.MarkedRanges[i].Marked;
                }

                while (check && i < this.MarkedRanges.Count)
                {
                    if (endbyte <= this.MarkedRanges[i].Address)
                    {   // if the following ranges are too far
                        check = false;
                    }
                    else
                    {   // so there are ranges to modify
                        if (endbyte < this.MarkedRanges[i].EndByte)
                        {   // if the following range has a remaining portion after the removal
                            Int32 last = this.MarkedRanges[i].EndByte;
                            this.MarkedRanges.RemoveAt(i);
                            this.MarkedRanges.Insert(i, new Space(mark, endbyte, last));
                            check = false;
                        }
                        else
                        {   // so delete it completely and increment the loop
                            this.MarkedRanges.RemoveAt(i);

                            i++;
                            mark = this.MarkedRanges[i].Marked;
                        }
                    }
                }
            }

            this.App.MHF.Changed = true;
        }

        /// <summary>
        /// Returns true if the given range is within a space marked 'markname'
        /// </summary>
        public Boolean IsMarked(String markname, Pointer address, Int32 length = 1)
        {
            Mark mark = this.App.MHF.Marks.Get(markname);

            List<Range> ranges = this.GetAllMarkedAs(markname);

            foreach (Space marked in this.MarkedRanges)
            {
                if (marked.Contains(address))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// returns all the ranges of marked space with the given mark to them
        /// </summary>
        public List<Range> GetAllMarkedAs(String markname)
        {
            Mark mark = this.App.MHF.Marks.Get(markname);
            List<Range> list = new List<Range>();

            foreach (Space space in this.MarkedRanges)
            {
                if (space.Marked == mark)
                {
                    list.Add(new Range(space.Address, space.EndByte));
                }
            }
            return list;
        }
        /// <summary>
        /// Removes all the ranges of marked space bearing the given mark.
        /// </summary>
        public void RemoveAllMarkedAs(Mark marked)
        {
            foreach (Space space in this.MarkedRanges)
            {
                if (space.Marked == marked)
                {
                    this.MarkedRanges.Remove(space);
                }
            }
        }

        /// <summary>
        /// Returns a pointer of an area of marked space in the loaded rom that is at least 'length'
        /// </summary>
        public Pointer GetPointer(String markname, Int32 length)
        {
            foreach (Space space in this.MarkedRanges)
            {
                if (space.Marked.Name == markname && space.Length >= length)
                {
                    return space.Address;
                }
            }
            return new Pointer();
        }

        /// <summary>
        /// Merges intersecting ranges and whatnot, checking space at the previous and following indices.
        /// </summary>
        private void ListCleanup(Int32 index)
        {
            Boolean prev_check = false;
            Int32 prev_index = index - 1;
            Mark prev_marked = null;
            Int32 prev_offset = 0;
            Int32 prev_endoff = 0;
            if (prev_index >= 0 && prev_index < this.MarkedRanges.Count)
            {
                prev_check = true;
                prev_marked = this.MarkedRanges[prev_index].Marked;
                prev_offset = this.MarkedRanges[prev_index].Address;
                prev_endoff = this.MarkedRanges[prev_index].EndByte;
            }
            Boolean this_check = false;
            Int32 this_index = index;
            Mark this_marked = null;
            Int32 this_offset = 0;
            Int32 this_endoff = 0;
            if (this_index >= 0 && this_index < this.MarkedRanges.Count)
            {
                this_check = true;
                this_marked = this.MarkedRanges[this_index].Marked;
                this_offset = this.MarkedRanges[this_index].Address;
                this_endoff = this.MarkedRanges[this_index].EndByte;
            }
            Boolean next_check = false;
            Int32 next_index = index + 1;
            Mark next_marked = null;
            Int32 next_offset = 0;
            Int32 next_endoff = 0;
            if (next_index >= 0 && next_index < this.MarkedRanges.Count)
            {
                next_check = true;
                next_marked = this.MarkedRanges[next_index].Marked;
                next_offset = this.MarkedRanges[next_index].Address;
                next_endoff = this.MarkedRanges[next_index].EndByte;
            }

            if (this_check)
            {
                if (prev_check)
                {
                    if (prev_endoff >= this_offset)
                    {   //The ranges intersect
                        if (prev_endoff >= this_endoff)
                        {   // if one is within the other
                            if (this_marked == prev_marked)
                            {   // if they can be merged, just use the larger one
                                this.MarkedRanges.RemoveAt(this_index);
                                next_check = false;
                            }
                            else// so they're not mergeable
                            {   // split the larger one into 2
                                this.MarkedRanges.RemoveAt(prev_index);
                                this.MarkedRanges.Insert(prev_index, new Space(prev_marked, prev_offset, this_offset));
                                this.MarkedRanges.Insert(next_index, new Space(prev_marked, this_endoff, prev_endoff));
                            }
                        }
                        else
                        {   // so it's an overlap
                            if (this_marked == prev_marked)
                            {   // if they have the same marking, merge the two
                                this.MarkedRanges.RemoveAt(this_index);
                                this.MarkedRanges.RemoveAt(prev_index);
                                this.MarkedRanges.Insert(prev_index, new Space(this_marked, prev_offset, this_endoff));
                                this_offset = prev_offset;
                                this_index = index - 1;
                                next_index = index;
                            }
                            else
                            {   // otherwise, reduce the old space to make way for the new one
                                this.MarkedRanges.RemoveAt(prev_index);
                                this.MarkedRanges.Insert(prev_index, new Space(prev_marked, prev_offset, this_offset));
                            }
                        }
                    }
                }

                while (next_check)
                {
                    if (this_endoff >= next_offset)
                    {   //The ranges intersect
                        if (this_endoff >= next_endoff)
                        {   // if this new space contains the other
                            this.MarkedRanges.RemoveAt(next_index);
                            if (next_index == this.MarkedRanges.Count)
                            {
                                next_check = false;
                            }
                            else
                            {
                                next_marked = this.MarkedRanges[next_index].Marked;
                                next_offset = this.MarkedRanges[next_index].Address;
                                next_endoff = this.MarkedRanges[next_index].EndByte;
                            }
                        }   // and it will loop over
                        else
                        {   // so it's an overlap
                            next_check = false;

                            if (this_marked == next_marked)
                            {   // if they have the same marking, merge the two
                                this.MarkedRanges.RemoveAt(next_index);
                                this.MarkedRanges.RemoveAt(this_index);
                                this.MarkedRanges.Insert(this_index, new Space(this_marked, this_offset, next_endoff));
                            }
                            else
                            {   // otherwise, reduce the old space to make way for the new one
                                this.MarkedRanges.RemoveAt(next_index);
                                this.MarkedRanges.Insert(next_index, new Space(next_marked, this_endoff, next_endoff));
                            }
                        }
                    }
                    else
                    {
                        next_check = false;
                    }
                }
            }

            for (Int32 i = 0; i < this.MarkedRanges.Count; i++)
            {
                if (this.MarkedRanges[i].Length <= 0) this.MarkedRanges.RemoveAt(i);
            }
        }
    }
}