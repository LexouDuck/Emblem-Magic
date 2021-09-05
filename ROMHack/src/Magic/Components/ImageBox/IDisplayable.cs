namespace Magic.Components
{
    /// <summary>
    /// Any class implementing this interface can be drawn onto an ImageBox
    /// </summary>
    public interface IDisplayable
    {
        /// <summary>
        /// This function allows for quick access to pixel data in GBA.Color format.
        /// </summary>
        GBA.Color GetColor(int x, int y);

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
