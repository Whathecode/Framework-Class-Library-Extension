namespace Whathecode.System.Arithmetic.Range
{
    /// <summary>
    ///   Option which specifies in which intervals the split point ends up.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public enum SplitOption
    {
        /// <summary>
        ///   The point at which is split is excluded from both intervals.
        /// </summary>
        None,
        /// <summary>
        ///   The point at which is split is included in both intervals.
        /// </summary>
        Both,
        /// <summary>
        ///   The point at which is split lies only within the left (smaller) part of the split interval.
        /// </summary>
        Left,
        /// <summary>
        ///   The point at which is split lies only within the right (larger) part of the split interval.
        /// </summary>
        Right
    }
}