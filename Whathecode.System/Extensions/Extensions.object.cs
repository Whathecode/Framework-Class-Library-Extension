namespace Whathecode.System.Extensions
{
    public partial class Extensions
    {
        /// <summary>
        ///   Determines whether the reference of the specified <see cref="object" /> equals that from this object
        ///   when it's a reference type, or whether the values equal when its a boxed value.
        /// </summary>
        /// <param name="source">The source for this extension method.</param>
        /// <param name="o">The object to compare with.</param>
        public static bool ReferenceOrBoxedValueEquals( this object source, object o )
        {
            return ((o != null) && o.GetType().IsValueType)
                       ? source.Equals( o )
                       : source == o;
        }
    }
}
