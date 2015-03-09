using System;
using System.Reflection;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Coercion;


namespace Whathecode.System.Windows.Controls.Internal
{
	/// <summary>
	///   Coerces the current visible interval based on set minima and maxima of the <see cref="AxesPanel{TX, TXSize, TY, TYSize}" />.
	/// </summary>
	public class VisibleIntervalCoercion : IControlCoercion<AxesPanelBinding, object>
	{
		readonly string _axisName;
		readonly AxesPanelBinding _dependentProperties;
		readonly string _convertToInternalIntervalMethod = "ConvertToInternalInterval";
		readonly string _convertToIntervalMethod = "ConvertToInterval";
		readonly string _convertToInternalSizeMethod;

		// Open instance delegates are used intentionally, since holding on to the context within this converter causes memory leaks.
		Func<object, object> _getMaxima;
		Func<object, object> _getMinimumSize;
		Func<object, object, Interval<double>> _convertToInternalInterval;
		Func<object, Interval<double>, object> _convertToInterval;
		Func<object, object, double> _convertToInternalSize; 
		
		public AxesPanelBinding DependentProperties
		{
			get { return _dependentProperties; }
		}


		public VisibleIntervalCoercion( Axis axis )
		{
			_axisName = axis == Axis.X ? "X" : "Y";
			_dependentProperties = axis == Axis.X
				? AxesPanelBinding.MaximaX | AxesPanelBinding.MinimumSizeX
				: AxesPanelBinding.MaximaY | AxesPanelBinding.MinimumSizeY;
			_convertToInternalIntervalMethod += _axisName;
			_convertToIntervalMethod += _axisName;
			_convertToInternalSizeMethod = "ConvertFrom" + _axisName + "SizeValue";
		}


		public object Coerce( object context, object value )
		{
			if ( value == null )
			{
				return null;
			}

			// Initialize functions to access the panel.
			if ( _getMaxima == null )
			{
				Type type = context.GetType();

				// Allow accessing properties.
				MethodInfo getMaxima = type.GetProperty( "Maxima" + _axisName ).GetGetMethod();
				_getMaxima = DelegateHelper.CreateOpenInstanceDelegate<Func<object, object>>( getMaxima, DelegateHelper.CreateOptions.Downcasting );
				MethodInfo getMinimumSize = type.GetProperty( "MinimumSize" + _axisName ).GetGetMethod();
				_getMinimumSize = DelegateHelper.CreateOpenInstanceDelegate<Func<object, object>>( getMinimumSize, DelegateHelper.CreateOptions.Downcasting );

				// Allow accessing protected conversion functions.
				MethodInfo convertToInternalInterval = type.GetMethod( _convertToInternalIntervalMethod, Reflection.ReflectionHelper.InstanceMembers );
				_convertToInternalInterval =
					DelegateHelper.CreateOpenInstanceDelegate<Func<object, object, Interval<double>>>( convertToInternalInterval, DelegateHelper.CreateOptions.Downcasting );
				MethodInfo convertToInterval = type.GetMethod( _convertToIntervalMethod, Reflection.ReflectionHelper.InstanceMembers );
				_convertToInterval =
					DelegateHelper.CreateOpenInstanceDelegate<Func<object, Interval<double>, object>>( convertToInterval, DelegateHelper.CreateOptions.Downcasting );
				MethodInfo convertToInternalSize = type.GetMethod( _convertToInternalSizeMethod, Reflection.ReflectionHelper.InstanceMembers );
				_convertToInternalSize =
					DelegateHelper.CreateOpenInstanceDelegate<Func<object, object, double>>( convertToInternalSize, DelegateHelper.CreateOptions.Downcasting );
			}

			// Limit size of the desired interval.
			Interval<double> setInterval = _convertToInternalInterval( context, value );
			object min = _getMinimumSize( context );
			double minimumSize = _convertToInternalSize( context, min );
			double tooSmallRatio = minimumSize / setInterval.Size;
			if ( tooSmallRatio > 1 )
			{
				setInterval = setInterval.Scale( tooSmallRatio );
			}

			// Limit how far the time line goes.
			Interval<double> limitInterval = _convertToInternalInterval( context, _getMaxima( context ) );
			Interval<double> limited = setInterval.Clamp( limitInterval );
			return _convertToInterval( context, limited );
		}
	}
}
