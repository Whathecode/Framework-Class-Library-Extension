using System.ComponentModel;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Xaml;


namespace Whathecode.System.Windows.DependencyPropertyFactory
{
	public partial class DependencyPropertyFactory<T>
	{
		/// <summary>
		///   Load XAML type converters for common types.
		/// </summary>
		static DependencyPropertyFactory()
		{
			TypeDescriptor.AddAttributes( typeof( Interval<,> ), new TypeConverterAttribute( typeof( IntervalTypeConverter ) ) );
		}
	}
}
