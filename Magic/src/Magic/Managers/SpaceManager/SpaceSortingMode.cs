namespace Magic
{
    /// <summary>
    /// Describes methods of sorting the space ranges in the marked Space output box
    /// </summary>
    public enum SpaceSortingMode
    {
        /// <summary>
        /// Sort marked space by offset in the ROM.
        /// </summary>
        Offset,
        /// <summary>
        /// Sort marked space by length of space.
        /// </summary>
        Length,
        /// <summary>
        /// Sort marked space ranges by marking type.
        /// </summary>
        Marked
    }
}
