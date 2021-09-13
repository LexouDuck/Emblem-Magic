using GBA;

namespace Magic
{
    /// <summary>
    /// Represents a repointing of game data wherein the pointer is referenced/rewritten in several locations
    /// </summary>
    public class Repoint
    {
        public System.String AssetName { get; set; }
        public Pointer DefaultAddress { get; set; }
        public Pointer CurrentAddress { get; set; }
        public Pointer[] References { get; set; }

        /// <summary>
        /// Usual constructor
        /// </summary>
        public Repoint(System.String assetName, Pointer defaultAddress)
        {
            this.AssetName = assetName;

            this.DefaultAddress = defaultAddress;
            this.CurrentAddress = defaultAddress;

            this.UpdateReferences();
        }
        /// <summary>
        /// Called when loading an MHF file
        /// </summary>
        public Repoint(System.String assetName, System.UInt32 defaultAddress, System.UInt32 currentAddress)
        {
            this.AssetName = assetName;

            this.DefaultAddress = new Pointer(defaultAddress);
            this.CurrentAddress = new Pointer(currentAddress);

            this.UpdateReferences();
        }

        public void UpdateReferences()
        {
            this.References = Core.SearchData(this.CurrentAddress.ToBytes(), 4);
        }
    }
}
