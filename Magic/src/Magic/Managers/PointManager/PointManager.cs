using System;
using System.Collections.Generic;
using GBA;

namespace Magic
{
    /// <summary>
    /// This class takes cares of how the main game assets are pointed/repointed
    /// since these arrays/assets can have several references in the ASM code at the beginning of the game data
    /// </summary>
    public class PointManager
    {
        public List<Repoint> Repoints { get; set; }

        public IApp App;
        public PointManager(IApp app)
        {
            this.App = app;
            this.Repoints = new List<Repoint>();
        }
        public void Load(Repoint[] pointers)
        {
            this.Repoints.Clear();

            for (Int32 i = 0; i < pointers.Length; i++)
            {
                this.Repoints.Add(pointers[i]);
            }
        }



        /// <summary>
        /// Adds a repoint tracker for a pointer in the ROM to the list
        /// </summary>
        /// <param name="repoint"></param>
        public void Add(Repoint repoint)
        {
            this.Repoints.Add(repoint);
        }
        /// <summary>
        /// Removes a repoint tracker from the list
        /// </summary>
        public void Remove(String assetName)
        {
            for (Int32 i = 0; i < this.Repoints.Count; i++)
            {
                if (assetName.Equals(this.Repoints[i].AssetName))
                    this.Repoints.RemoveAt(i);
            }
        }

        /// <summary>
        /// Returns the repoint tracker whose name matches the one given
        /// </summary>
        public Repoint Get(String assetName)
        {
            for (Int32 i = 0; i < this.Repoints.Count; i++)
            {
                if (assetName.Equals(this.Repoints[i].AssetName))
                    return this.Repoints[i];
            }
            return null;
        }
        /// <summary>
        /// Return the repoint tracker whose default or current address matches the one given
        /// </summary>
        public Repoint Get(Pointer address)
        {
            for (Int32 i = 0; i < this.Repoints.Count; i++)
            {
                if (address == this.Repoints[i].CurrentAddress)
                    return this.Repoints[i];
            }
            return null;
        }
    }
}
