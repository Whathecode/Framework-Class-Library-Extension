using System;
using System.Collections.ObjectModel;


namespace Whathecode.System.Windows.Controls
{
	/// <summary>
	///   A collection of label collections which can be assigned to a <see cref="AxesPanel{TX,TXSize,TY,TYSize}" />.
	/// </summary>
	/// <typeparam name="TX">The type of the values along the x-axis.</typeparam>
	/// <typeparam name="TXSize">The type of the size between values along the x-axis.</typeparam>
	/// <typeparam name="TY">The type of the values along the y-axis.</typeparam>
	/// <typeparam name="TYSize">The type of the size between values along the y-axis.</typeparam>
	public class AxesLabelFactories<TX, TXSize, TY, TYSize> : ObservableCollection<AbstractAxesLabelCollection<TX, TXSize, TY, TYSize>>
		where TX : IComparable<TX>
		where TXSize : IComparable<TXSize>
		where TY : IComparable<TY>
		where TYSize : IComparable<TYSize>
	{
	}
}
