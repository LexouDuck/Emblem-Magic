using System;
using System.Drawing;

namespace EmblemMagic
{
    /// <summary>
    /// A class that respresents a space marking type - stores the string, color and layer associated.
    /// </summary>
    public class Mark
    {
        public static Mark FREE = new Mark("FREE", 0, SystemColors.Highlight);
        public static Mark USED = new Mark("USED", 0, Color.PaleVioletRed);



        /// <summary>
        /// The 4-character string for this marking type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The color associated with this marking type.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The layer on which this marking type is - types on the same layer conflict with each other.
        /// </summary>
        public int Layer { get; set; }



        /// <summary>
        /// Creates a marking type
        /// </summary>
        public Mark(string name, int layer, Color color)
        {
            Load(name, layer, color);
        }
        /// <summary>
        /// Constructor used for loading from file
        /// </summary>
        public Mark(string name, uint layer, uint color)
        {
            Load(name, (int)layer, Color.FromArgb((int)color));
        }
        /// <summary>
        /// Initialization function
        /// </summary>
        void Load(string name, int layer, Color color)
        {
            if (name.Length != 4) throw new Exception("Mark: given name has more than 4 characters.");
            if (layer < 0) throw new Exception("Mark: layer cannot be negative.");
            Name = name;
            Color = color;
            Layer = layer;
        }
    }
}
