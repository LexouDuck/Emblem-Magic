namespace EmblemMagic.Components
{
    /// <summary>
    /// Any class implementing this interface can be drawn onto an ImageBox
    /// </summary>
    public interface IDisplayable
    {
        /// <summary>
        /// This indexer allows for quick access to pixel data in GBA.Color format.
        /// </summary>
        GBA.Color this[int x, int y] { get; }

        /// <summary>
        /// The Width of this displayable item
        /// </summary>
        int Width { get; }
        /// <summary>
        /// The Height of this displayable item
        /// </summary>
        int Height { get; }
    }
}
