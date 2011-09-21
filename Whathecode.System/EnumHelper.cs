using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lambda.Generic.Arithmetic;
using Whathecode.System.Arithmetic;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System
{
	/// <summary>
	///   A generic helper class to do common <see cref = "Enum">Enum</see> operations.
	/// </summary>
	/// <typeparam name = "T">The type of the enum.</typeparam>
	/// <author>Steven Jeuris</author>
	public class EnumHelper<T>
	{
		/// <summary>
		///   Backing field for Calculator which does lazy loading since the calculator is only needed when doing flag operations.
		/// </summary>
		static readonly Lazy<IBinaryMath<T>> CalculatorLazy = new Lazy<IBinaryMath<T>>(
			() => (IBinaryMath<T>)CalculatorFactory.CreateIntegerCalculator<T>( CalculatorFactory.CheckedOption.Checked ) );

		/// <summary>
		///   Calculator which is required to be able to do flag operations.
		/// </summary>
		static IBinaryMath<T> Calculator
		{
			get { return CalculatorLazy.Value; }
		}


		/// <summary>
		///   Converts the string representation of the name or numeric value of one or more enumerated constants (seperated by comma)
		///   to an equivalent enumerated object. Case-sensitive operation.
		/// </summary>
		/// <param name = "value">A string containing the name or value to convert.</param>
		public static T Parse( string value )
		{
			return Parse( value, false );
		}

		/// <summary>
		///   Converts the string representation of the name or numeric value of one or more enumerated constants (seperated by comma)
		///   to an equivalent enumerated object. A parameter specifies whether the operations is case-sensitive.
		/// </summary>
		/// <param name = "value">A string containing the name or value to convert.</param>
		/// <param name = "ignoreCase">True to ignore case; false to regard case.</param>
		public static T Parse( string value, bool ignoreCase )
		{
			return (T)Enum.Parse( typeof( T ), value, ignoreCase );
		}

		/// <summary>
		///   Retrieves an enumerator of the constants in a specified enumeration.
		/// </summary>
		/// <returns>Enumerable which can be used to enumerate over all constants in the specified enumeration.</returns>
		public static IEnumerable<T> GetValues()
		{
			return from object value in Enum.GetValues( typeof( T ) )
				select (T)value;
		}

		/// <summary>
		///   Retrieves an enumerator of all the flagged values in a flags enum.
		/// </summary>
		/// <param name = "flags">The value specifying the flags.</param>
		/// <returns>Enumerable which can be used to enumerate over all flagged values of the passed enum value.</returns>
		public static IEnumerable<T> GetFlaggedValues( T flags )
		{
			Contract.Requires( typeof( T ).IsFlagsEnum() );

			return GetValues().Where( flag => Calculator.And( flag, flags ).Equals( flag ) );
		}
	}
}