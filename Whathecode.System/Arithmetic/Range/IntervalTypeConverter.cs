using System.ComponentModel;
using Whathecode.System.ComponentModel;


namespace Whathecode.System.Arithmetic.Range
{
	/// <summary>
	///   Redirects the type converter to a custom one added to the <see cref="TypeDescriptor" />.
	/// </summary>
	class IntervalTypeConverter : RedirectTypeConverter
	{
		public IntervalTypeConverter()
			: base( typeof( Interval<,> ) )
		{
		}
	}
}
