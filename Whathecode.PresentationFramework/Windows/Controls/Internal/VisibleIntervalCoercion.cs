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

		object _context;
		Func<object> _getMaxima;
		Func<object> _getMinimumSize;
		Func<object, Interval<double>> _convertToInternalInterval;
		Func<Interval<double>, object> _convertToInterval;
		Func<object, double> _convertToInternalSize; 
		
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
			if ( context != _context )
			{
				_context = context;
				Type type = _context.GetType();

				// Allow accessing properties.
				MethodInfo getMaxima = type.GetProperty( "Maxima" + _axisName ).GetGetMethod();
				_getMaxima = DelegateHelper.CreateDelegate<Func<object>>( getMaxima, _context );
				MethodInfo getMinimumSize = type.GetProperty( "MinimumSize" + _axisName ).GetGetMethod();
				_getMinimumSize = DelegateHelper.CreateDelegate<Func<object>>( getMinimumSize, _context, DelegateHelper.CreateOptions.Downcasting );

				// Allow accessing protected conversion functions.
				MethodInfo convertToInternalInterval = type.GetMethod( _convertToInternalIntervalMethod, System.Reflection.ReflectionHelper.InstanceMembers );
				_convertToInternalInterval =
					DelegateHelper.CreateDelegate<Func<object, Interval<double>>>( convertToInternalInterval, _context, DelegateHelper.CreateOptions.Downcasting );
				MethodInfo convertToInterval = type.GetMethod( _convertToIntervalMethod, System.Reflection.ReflectionHelper.InstanceMembers );
				_convertToInterval =
					DelegateHelper.CreateDelegate<Func<Interval<double>, object>>( convertToInterval, _context, DelegateHelper.CreateOptions.Downcasting );
				MethodInfo convertToInternalSize = type.GetMethod( _convertToInternalSizeMethod, System.Reflection.ReflectionHelper.InstanceMembers );
				_convertToInternalSize =
					DelegateHelper.CreateDelegate<Func<object, double>>( convertToInternalSize, _context, DelegateHelper.CreateOptions.Downcasting );
			}

			// Limit size of the desired interval.
			Interval<double> setInterval = _convertToInternalInterval( value );
			object bleh = _getMinimumSize();
			double minimumSize = _convertToInternalSize( bleh );
			double tooSmallRatio = minimumSize / setInterval.Size;
			if ( tooSmallRatio > 1 )
			{
				setInterval = setInterval.Scale( tooSmallRatio );
			}

			// Limit how far the time line goes.
			Interval<double> limitInterval = _convertToInternalInterval( _getMaxima() );
			Interval<double> limited = setInterval.Clamp( limitInterval );
			return _convertToInterval( limited );
		}
	}
}
