using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System;
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

		delegate bool TestDelegate( int test );

		// Types.
		readonly Type _int = typeof( int );
		readonly Type _short = typeof( short );
		readonly Type _string = typeof( string );
		readonly Type _object = typeof( object );
		readonly Type _simple = typeof( Simple );
		readonly Type _comparable = typeof( IComparable );

		/// <summary>
		///   List which groups the type from which to which to convert,
		///   together with the expected outcome for an implicit and explicit conversion. (success/failure)
		/// </summary>
		class CanConvert : TupleList<Type, Type, bool, bool>
		{
			public void Add( Type from, Type to, bool expectedResult )
			{
				// By default, assume explicit conversions within the same hierarchy are always possible.
				Add( from, to, expectedResult, true );
			}

			public void Test()
			{
				foreach ( var test in this )
				{
					// Implicit test.
					Assert.AreEqual( test.Item3, test.Item1.CanConvertTo( test.Item2 ) );

					// Test whether explicit casts to to type in same hierarchy is possible.
					Assert.AreEqual( test.Item4, test.Item1.CanConvertTo( test.Item2, CastType.SameHierarchy ) );
				}
			}
		}

		#endregion // Common test members


		[TestMethod]
		public void CanConvertToTest()
		{
			new CanConvert
			{
				// Non generic.
				{ _int, _int, true },		// No change.
				{ _string, _string, true },
				{ _object, _object, true },
				{ _simple, _simple, true },
				{ _int, _object, true },	// object <-> value
				{ _object, _int, false },
				{ _string, _object, true },	// string <-> object
				{ _object, _string, false },
				{ _simple, _object, true },	// object <-> object
				{ _object, _simple, false },
				{ _int, _short, false },	// value <-> value (by .NET rules, not C#)
				{ _short, _int, false },

				// Interface.
				{ _comparable, _comparable, true },	// No change.
				{ _int, _comparable, true },		// value <-> interface
				{ _comparable, _int, false },
				{ _comparable, _object, true },		// object <-> interface
				{ _object, _comparable, false }
			}.Test();

			// Interface variant type parameters.
			Func<Type, Type, Type> makeGeneric = ( g, t ) => g.MakeGenericType( t );
			VarianceCheck( typeof( ICovariantOne<> ), makeGeneric, true );
			VarianceCheck( typeof( IContravariantOne<> ), makeGeneric, false );

			// Delegate variant type parameter.
			VarianceCheck( typeof( Func<> ), makeGeneric, true );
			VarianceCheck( typeof( Action<> ), makeGeneric, false );

			// Multiple variant type parameters.
			Type simpleObject = typeof( Func<Simple, object> );
			Type objectSimple = typeof( Func<object, Simple> );
			Type simpleSimple = typeof( Func<Simple, Simple> );
			Type objectObject = typeof( Func<object, object> );
			Assert.IsTrue( simpleObject.CanConvertTo( simpleObject ) );
			Assert.IsFalse( simpleObject.CanConvertTo( objectSimple ) );
			Assert.IsTrue( objectSimple.CanConvertTo( simpleObject ) );
			Assert.IsFalse( simpleObject.CanConvertTo( simpleSimple ) );
			Assert.IsTrue( objectSimple.CanConvertTo( objectObject ) );

			// TODO: Multiple inheritance for interfaces.

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
		///   For interfaces, only considers single implementing interface.
		/// </summary>
		/// <param name = "genericType">The generic type to check.</param>
		/// <param name = "makeGeneric">Function which can convert generic type into a specific type.</param>
		/// <param name="covariant">true when to check for covariance, false for contravariance.</param>
		void VarianceCheck( Type genericType, Func<Type, Type, Type> makeGeneric, bool covariant )
		{
			Type genericSimple = makeGeneric( genericType, _simple );
			Type genericObject = makeGeneric( genericType, _object );
			Type genericValue = makeGeneric( genericType, _int );

			bool isDelegate = genericType.IsDelegate();

			new CanConvert
			{
				// No change.
				{ genericObject, genericObject, true },		
		
				// generic type <-> object
				{ genericObject, _object, true },
				{ _object, genericObject, false },

				// No variance for value type parameters.
				// Converting from a generic type with a value parameter to one with a reference type parameters is only possible
				// when it is an interface type, and a certain type implements both interfaces. (e.g. ICovariance<int> -> ICovariance<object>)
				{ genericValue, genericObject, false, false },
				{ genericObject, genericValue, false, false },

				// Covariance/contraviariance between reference type parameters.
				// Only generic interface types can explicitly convert in the 'opposite' direction of their variance. Delegates can't!				
				{ genericSimple, genericObject,
					covariant,
					covariant ? true : !isDelegate },
				{ genericObject, genericSimple,
					!covariant,
					!covariant ? true : !isDelegate }
			}.Test();
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

		[TestMethod]
		public void IsDelegateTest()
		{
			Assert.IsTrue( typeof( Func<int> ).IsDelegate() );
			Assert.IsTrue( typeof( Action ).IsDelegate() );
			Assert.IsTrue( typeof( TestDelegate ).IsDelegate() );

			Assert.IsFalse( typeof( TypeExtensionsTest ).IsDelegate() );
		}
	}
}