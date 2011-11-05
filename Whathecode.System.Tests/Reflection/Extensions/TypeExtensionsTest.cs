using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Collections.Generic;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.Tests.System.Reflection.Extensions
{
	[TestClass]
	public class TypeExtensionsTest
	{
		#region Common test members

		class Simple {}

		interface IOne<T> {}
		class One<T> : IOne<T> {}
		class ExtendingOne<T> : One<T> {}		
		
		interface ICovariantOne<out T> {}
		interface IContravariantOne<in T> {}
		class CovariantOne<T> : ICovariantOne<T> {}
		class ContravariantOne<T> : IContravariantOne<T> {}

		// Types.
		readonly Type _int = typeof( int );
		readonly Type _short = typeof( short );
		readonly Type _string = typeof( string );
		readonly Type _object = typeof( object );
		readonly Type _simple = typeof( Simple );

		/// <summary>
		///   List which groups the type from which to which to convert, together with the expected outcome (success/failure).
		/// </summary>
		class CanConvert : TupleList<Type, Type, bool>
		{
			public void Test()
			{
				foreach ( var test in this )
				{
					Assert.AreEqual( test.Item3, test.Item1.CanConvertTo( test.Item2 ) );
				}
			}
		}

		#endregion // Common test members


		[TestMethod]
		public void CanConvertToImplicitTest()
		{
			Type comparable = typeof( IComparable );

			new CanConvert
			{
				// Non generic.
				{ _int, _int, true }, // No change.
				{ _string, _string, true },
				{ _object, _object, true },
				{ _simple, _simple, true },
				{ _int, _object, true }, // object <-> value
				{ _object, _int, false },
				{ _string, _object, true }, // string <-> object
				{ _object, _string, false },
				{ _simple, _object, true }, // object <-> object
				{ _object, _simple, false },
				{ _int, _short, false }, // value <-> value (by .NET rules, not C#)
				{ _short, _int, false },

				// Interface.
				{ comparable, comparable, true }, // No change.
				{ _int, comparable, true }, // value <-> interface
				{ comparable, _int, false },
				{ comparable, _object, true }, // object <-> interface
				{ _object, comparable, false }
			}.Test();

			// Interface variant type parameters.
			Func<Type, Type, Type> makeGeneric = ( g, t ) => g.MakeGenericType( t );
			VarianceCheck( typeof( ICovariantOne<> ), makeGeneric, true );
			VarianceCheck( typeof( IContravariantOne<> ), makeGeneric, false );

			// Delegate variant type parameters.
			VarianceCheck( typeof( Func<> ), makeGeneric, true );
			VarianceCheck( typeof( Action<> ), makeGeneric, false );

			Type simpleObject = typeof( Func<Simple, object> );
			Type objectSimple = typeof( Func<object, Simple> );
			Type simpleSimple = typeof( Func<Simple, Simple> );
			Type objectObject = typeof( Func<object, object> );
			Assert.IsTrue( simpleObject.CanConvertTo( simpleObject ) );
			Assert.IsFalse( simpleObject.CanConvertTo( objectSimple ) );
			Assert.IsTrue( objectSimple.CanConvertTo( simpleObject ) );
			Assert.IsFalse( simpleObject.CanConvertTo( simpleSimple ) );
			Assert.IsTrue( objectSimple.CanConvertTo( objectObject ) );

			// Recursive variant type parameters.
			Func<Type, Type, Type> makeInnerGeneric = ( g, t ) 
				=> g.GetGenericTypeDefinition( g.GetGenericArguments()[ 0 ].GetGenericTypeDefinition( t ) );
			VarianceCheck( typeof( Func<Func<object>> ), makeInnerGeneric, true );
			VarianceCheck( typeof( Action<Action<object>> ), makeInnerGeneric, false );
			VarianceCheck( typeof( ICovariantOne<ICovariantOne<object>> ), makeInnerGeneric, true );
			VarianceCheck( typeof( IContravariantOne<IContravariantOne<object>> ), makeInnerGeneric, false );

			// Mixed recursive covariant/contravariant type parameters.
			VarianceCheck( typeof( Func<Action<object>> ), makeInnerGeneric, false );
			VarianceCheck( typeof( Action<Func<object>> ), makeInnerGeneric, true );
		}

		/// <summary>
		///   Checks the variance rules for generic types.
		/// </summary>
		/// <param name = "genericType">The generic type to check.</param>
		/// <param name = "makeGeneric">Function which can convert generic type into a specific type.</param>
		/// <param name="covariant">true when to check for covariance, false for contravariance.</param>
		void VarianceCheck( Type genericType, Func<Type, Type, Type> makeGeneric, bool covariant )
		{
			Type genericSimple = makeGeneric( genericType, _simple );
			Type genericObject = makeGeneric( genericType, _object );
			Type genericValue = makeGeneric( genericType, _int );

			// General variance checks.
			new CanConvert
			{
				// No change.
				{ genericObject, genericObject, true },		
		
				// generic type <-> object
				{ genericObject, _object, true },
				{ _object, genericObject, false },

				// no variance between object <-> value
				{ genericValue, genericObject, false },
				{ genericObject, genericValue, false }
			}.Test();

			// Specific covariance/contravariance checks.
			Assert.AreEqual( covariant, genericSimple.CanConvertTo( genericObject ) );
			Assert.AreEqual( !covariant, genericObject.CanConvertTo( genericSimple ) );
		}

		[TestMethod]
		public void GetMatchingGenericTypeTest()
		{
			Type baseType = typeof( One<int> );
			Type interfaceType = typeof( IOne<int> );
			Type incompleteInterfaceType = typeof( IOne<> );
			Type incompleteBaseType = typeof( One<> );
			Type extendingType = typeof( ExtendingOne<int> );
			
			// Base types.
			Assert.AreEqual( baseType, baseType.GetMatchingGenericType( baseType ) );
			Assert.AreEqual( baseType, extendingType.GetMatchingGenericType( baseType ) );
			Assert.AreEqual( baseType, extendingType.GetMatchingGenericType( incompleteBaseType ) );

			// Interfaces.
			Assert.AreEqual( interfaceType, interfaceType.GetMatchingGenericType( interfaceType ) );
			Assert.AreEqual( interfaceType, baseType.GetMatchingGenericType( interfaceType ) );
			Assert.AreEqual( interfaceType, baseType.GetMatchingGenericType( incompleteInterfaceType ) );
		}
	}
}