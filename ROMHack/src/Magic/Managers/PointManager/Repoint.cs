using GBA;

namespace Magic
{
    /// <summary>
    /// Represents a repointing of game data wherein the pointer is referenced/rewritten in several locations
    /// </summary>
    public class Repoint
    {
        public string AssetName { get; set; }
        public Pointer DefaultAddress { get; set; }
        public Pointer CurrentAddress { get; set; }
        public Pointer[] References { get; set; }

        /// <summary>
        /// Usual constructor
        /// </summary>
        public Repoint(string assetName, Pointer defaultAddress)
        {
            AssetName = assetName;

            DefaultAddress = defaultAddress;
            CurrentAddress = defaultAddress;

            UpdateReferences();
        }
        /// <summary>
        /// Called when loading an FEH file
        /// </summary>
        public Repoint(string assetName, uint defaultAddress, uint currentAddress)
        {
            AssetName = assetName;

            DefaultAddress = new Pointer(defaultAddress);
            CurrentAddress = new Pointer(currentAddress);

            UpdateReferences();
        }

        public void UpdateReferences()
        {
            References = Core.SearchData(CurrentAddress.ToBytes(), 4);
        }
    }
}
