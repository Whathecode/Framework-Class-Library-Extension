using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System
{
	/// <summary>
	///   A generic helper class to do common <see cref = "Enum">Enum</see> operations.
	/// </summary>
	/// <typeparam name = "T">The type of the enum.</typeparam>
	/// <author>Steven Jeuris</author>
	public static class EnumHelper<T>
	{
		/// <summary>
		///   Converts the string representation of the name or numeric value of one or more enumerated constants (seperated by comma)
		///   to an equivalent enumerated object. A parameter specifies whether the operation is case-sensitive.
		/// </summary>
		/// <param name = "value">A string containing the name or value to convert.</param>
		/// <param name = "ignoreCase">True to ignore case; false to regard case.</param>
		public static T Parse( string value, bool ignoreCase = false )
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
			Contract.Requires( flags != null );

			// In case .NET 4.0 isn't an option:
			//return GetValues().Where( flag => BinaryOperator<T>.And( flag, flags ).Equals( flag ) );

			// ReSharper disable PossibleNullReferenceException
			return from Enum value in Enum.GetValues( typeof( T ) )
				   where (flags as Enum).HasFlag( value )
				   select (T)(object)value;
			// ReSharper restore PossibleNullReferenceException
		}
	}
}