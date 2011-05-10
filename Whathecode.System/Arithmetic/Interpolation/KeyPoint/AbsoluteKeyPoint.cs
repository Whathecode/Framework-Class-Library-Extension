namespace Whathecode.System.Arithmetic.Interpolation.KeyPoint
{
    /// <summary>
    ///   A helper struct for AbsoluteKeyPointCollection which allows linking key points to values which
    ///   are used by AbsoluteKeyPointCollection.
    /// </summary>
    /// <typeparam name = "TKey">The type used for the values to which the added key points are linked.</typeparam>
    /// <typeparam name = "TValue">The type of the key points.</typeparam>
    /// <author>Steven Jeuris</author>
    public struct AbsoluteKeyPoint<TKey, TValue> where TKey : new()
    {
        /// <summary>
        ///   A reference key point which can be used to initialize the
        ///   <see cref="System.Arithmetic.Interpolation.KeyPoint.AbsoluteKeyPointCollection{TValue,TMath}">AbsoluteKeyPointCollection</see>.
        /// </summary>
        public static AbsoluteKeyPoint<TKey, TValue> ReferenceKeyPoint
        {
            get
            {
                return new AbsoluteKeyPoint<TKey, TValue>( new TKey(), default( TValue ) );
            }
        }

        /// <summary>
        ///   The value to which the key point is linked.
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        ///   The actual key point.
        /// </summary>
        public TValue KeyPoint { get; private set; }


        /// <summary>
        ///   Create a new key point linked to a value.
        /// </summary>
        /// <param name="key">The value to which the key point is linked.</param>
        /// <param name="keyPoint">The actual key point.</param>
        public AbsoluteKeyPoint( TKey key, TValue keyPoint )
            : this()
        {
            Key = key;
            KeyPoint = keyPoint;
        }
    }
}