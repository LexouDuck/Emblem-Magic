namespace Magic
{
    /// <summary>
    /// Describes an action done by the user onto the loaded ROM
    /// </summary>
    public enum UserAction
    {
        /// <summary>
        /// A write that the user decided to cancel upon seeing that it was in conflict with a previous one.
        /// </summary>
        Cancel,
        /// <summary>
        /// A write done by the user onto the loaded ROM
        /// </summary>
        Write,
        /// <summary>
        /// An overwrite - where the FEH Hack Manager recorded a write and a new write is to replace it.
        /// </summary>
        Overwrite,
        /// <summary>
        /// A deletion of a write - that is, a restoration of data.
        /// </summary>
        Restore
    }
}