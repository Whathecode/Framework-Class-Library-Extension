using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Whathecode.System.Linq;


namespace Whathecode.System.Reflection.Extensions
{
	public static partial class Extensions
	{
		/// <summary>
		///   Returns a <see cref = "Type" /> object based on the <see cref = "Type" /> object
		///   with its type parameters replaced with the given type arguments.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <param name = "typeArguments">
		///   An array of types to be substituted for the type parameters of the given <see cref = "Type" />.
		/// </param>
		/// <returns>
		///   A <see cref = "Type" /> object that represents the constructed type formed by substituting
		///   the elements of <paramref name = "typeArguments" /> for the type parameters of the current type definition.
		/// </returns>
		public static Type GetGenericTypeDefinition( this Type source, params Type[] typeArguments )
		{
			Contract.Requires( source.IsGenericType );

			return source
				.GetGenericTypeDefinition()
				.MakeGenericType( typeArguments );
		}

		/// <summary>
		///   Searches for the specified method whose parameters match the specified argument types,
		///   using the specified binding constraints.
		/// </summary>
		/// <remarks>
		///   This is a simple overload which is missing in the original library.
		/// </remarks>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "name">The string containing the name of the public method to get.</param>
		/// <param name = "flags">
		///   A bitmask comprised of one or more <see cref = "BindingFlags" /> that specify how the search is conducted.
		///   -or-
		///   Zero, to return null.
		/// </param>
		/// <param name = "types">
		///   An array of <see cref = "Type" /> objects representing the number, order, and type of the parameters for the method to get.
		///   -or-
		///   An empty array of <see cref = "Type" /> objects (as provided by the EmptyTypes field) to get a method that takes no parameters.
		/// </param>
		/// <returns>
		///   An object representing the method that matches the requirements
		///   and whose parameters match the specified argument types, if found; otherwise, null.
		/// </returns>
		public static MethodInfo GetMethod( this Type source, string name, BindingFlags flags, Type[] types )
		{
			return source.GetMethod( name, flags, Type.DefaultBinder, types, null );
		}

		/// <summary>
		///   Determines whether a conversion from one type to another is possible.
		///   This uses .NET rules. E.g. short is not implicitly convertible to int, while this is possible in C#.
		///   TODO: Support constraints, custom implicit conversion operators? Unit tests for explicit converts.
		/// </summary>
		/// <param name = "fromType">The type to convert from.</param>
		/// <param name = "targetType">The type to convert to.</param>
		/// <param name = "castType">Specifies what types of casts should be considered.</param>
		/// <returns>true when a conversion to the target type is possible, false otherwise.</returns>
		public static bool CanConvertTo( this Type fromType, Type targetType, CastType castType = CastType.Implicit )
		{
			return CanConvertTo( fromType, targetType, castType, false );
		}

		static bool CanConvertTo( this Type fromType, Type targetType, CastType castType, bool switchVariance )
		{
			bool sameHierarchy = castType == CastType.SameHierarchy;

			Func<Type, Type, bool> covarianceCheck = sameHierarchy
				? (Func<Type, Type, bool>)IsInHierarchy
				: ( from, to ) => from == to || from.IsSubclassOf( to );
			Func<Type, Type, bool> contravarianceCheck = sameHierarchy
				? (Func<Type, Type, bool>)IsInHierarchy
				: ( from, to ) => from == to || to.IsSubclassOf( from );

			if ( switchVariance )
			{
				Variable.Swap( ref covarianceCheck, ref contravarianceCheck );
			}

			// Simple hierarchy check.
			if ( covarianceCheck( fromType, targetType ) )
			{
				return true;
			}

			// Interface check.
			if ( (targetType.IsInterface && fromType.ImplementsInterface( targetType ))
				|| (sameHierarchy && fromType.IsInterface && targetType.ImplementsInterface( fromType )) )
			{
				return true;
			}

			// Explicit value type conversions (including enums).
			if ( sameHierarchy && (fromType.IsValueType && targetType.IsValueType) )
			{
				return true;
			}

			// Recursively verify when it is a generic type.
			if ( targetType.IsGenericType )
			{
				Type genericDefinition = targetType.GetGenericTypeDefinition();
				Type sourceGeneric = fromType.GetMatchingGenericType( genericDefinition );

				// Delegates never support casting in the 'opposite' direction than their varience type parameters dictate.
				CastType cast = fromType.IsDelegate() ? CastType.Implicit : castType;

				if ( sourceGeneric != null ) // Same generic types.
				{
					// Check whether parameters correspond, taking into account variance rules.
					return sourceGeneric.GetGenericArguments().Zip(
						targetType.GetGenericArguments(), genericDefinition.GetGenericArguments(),
						( from, to, generic )
							=> !(from.IsValueType || to.IsValueType)	// Variance applies only to reference types.
								? generic.GenericParameterAttributes.HasFlag( GenericParameterAttributes.Covariant )
									? CanConvertTo( from, to, cast, false )
									: generic.GenericParameterAttributes.HasFlag( GenericParameterAttributes.Contravariant )
										? CanConvertTo( from, to, cast, true )
										: false
								: false )
						.All( match => match );
				}
			}

			return false;
		}

		/// <summary>
		///   Determines whether one type is in the same inheritance hierarchy than another.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "type">The type the check whether it is in the same inheritance hierarchy.</param>
		/// <returns>true when both types are in the same inheritance hierarchy, false otherwise.</returns>
		public static bool IsInHierarchy( this Type source, Type type )
		{
			return source == type || source.IsSubclassOf( type ) || type.IsSubclassOf( source );
		}

		/// <summary>
		///   Get the first found matching generic type.
		///   The type parameters of the generic type are optional.
		///   E.g. Dictionary&lt;,&gt; or Dictionary&lt;string,&gt;
		///   When full (generic) type is known (e.g. Dictionary&lt;string,string&gt;),
		///   the "is" operator is most likely more performant, but this function will still work correctly.
		/// </summary>
		/// <param name = "source">The source for this extension method.</param>
		/// <param name = "type">The type to check for.</param>
		/// <returns>The first found matching complete generic type, or null when no matching type found.</returns>
		public static Type GetMatchingGenericType( this Type source, Type type )
		{
			Type[] genericArguments = type.GetGenericArguments();
			Type rawType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

			// Used to compare type arguments and see whether they match.
			Func<Type[], bool> argumentsMatch
				= arguments => genericArguments
					.Zip( arguments, Tuple.Create )
					.All( t => t.Item1.IsGenericParameter // No type specified.
						|| t.Item1 == t.Item2 );

			Type matchingType = null;
			if ( type.IsInterface && !source.IsInterface )
			{
				// Traverse across all interfaces to find a matching interface.
				matchingType = (
					from t in source.GetInterfaces()
					let rawInterface = t.IsGenericType ? t.GetGenericTypeDefinition() : t
					where rawInterface == rawType && argumentsMatch( t.GetGenericArguments() )
					select t
					).FirstOrDefault();
			}
			else
			{
				// Traverse across the type, and all it's base types.
				Type baseType = source;
				while ( baseType != null && baseType != typeof( object ) )
				{
					Type rawCurrent = baseType.IsGenericType ? baseType.GetGenericTypeDefinition() : baseType;
					if ( rawType == rawCurrent )
					{
						// Same raw generic type, compare type arguments.
						if ( argumentsMatch( baseType.GetGenericArguments() ) )
						{
							matchingType = baseType;
							break;
						}
					}
					baseType = baseType.BaseType;
				}
			}

			return matchingType;
		}

		/// <summary>
		///   Is a certain type of a given generic type or not.
		///   Also works for raw generic types. E.g. Dictionary&lt;,&gt;.
		///   When full (generic) type is known (e.g. Dictionary&lt;string,string&gt;),
		///   the "is" operator is most likely more performant, but this function will still work correctly.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <param name = "type">The type to check for.</param>
		/// <returns>True when the type is of the given type, false otherwise.</returns>
		[Pure]
		public static bool IsOfGenericType( this Type source, Type type )
		{
			return GetMatchingGenericType( source, type ) != null;
		}

		/// <summary>
		///   Verify whether a given type is an enum with the <see cref = "FlagsAttribute" /> applied.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <returns>True when the given type is a flags enum, false otherwise.</returns>
		[Pure]
		public static bool IsFlagsEnum( this Type source )
		{
			return source.IsEnum && source.GetAttributes<FlagsAttribute>().Length != 0;
		}

		/// <summary>
		///   Verify whether a given type is a delegate.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <returns>True when the given type is a delegate, false otherwise.</returns>
		public static bool IsDelegate( this Type source )
		{
			return source.IsSubclassOf( typeof( Delegate ) );
		}

		/// <summary>
		///   Does a certain type implement a given interface or not.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <param name = "interfaceType">The interface type to check for.</param>
		/// <returns>True when the type implements the given interface, false otherwise.</returns>
		public static bool ImplementsInterface( this Type source, Type interfaceType )
		{
			Contract.Requires( interfaceType.IsInterface );

			return source.GetInterface( interfaceType.ToString() ) != null;
		}

		/// <summary>
		///   Create a default initialisation of the given object type.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <returns>The default initialisation for the given objectType.</returns>
		public static object CreateDefault( this Type source )
		{
			return (source.IsValueType ? Activator.CreateInstance( source ) : null);
		}

		/// <summary>
		///   Return all members of a specific type in a certain type.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <param name = "type">The type to search for.</param>
		/// <returns>A list of all object members with the specific type.</returns>
		public static IEnumerable<MemberInfo> GetMembers( this Type source, Type type )
		{
			return
				from m in source.GetMembers( ReflectionHelper.AllClassMembers )
				where m is FieldInfo || m is PropertyInfo || m is EventInfo
				where m.GetMemberType().IsOfGenericType( type )
				select m;
		}

		/// <summary>
		///   Returns all members which have a specified attribute annotated to them.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <param name = "memberTypes">The type of members to search in.</param>
		/// <param name = "inherit">Specifies whether to search this member's inheritance chain to find the attributes.</param>
		/// <param name = "bindingFlags">
		///   A bitmask comprised of one or more <see cref = "BindingFlags" /> that specify how the search is conducted.
		///   -or-
		///   Zero, to return null.
		/// </param>
		/// <typeparam name = "TAttribute">The type of the attributes to search for.</typeparam>
		/// <returns>A dictionary containing all members with their attached attributes.</returns>
		public static Dictionary<MemberInfo, TAttribute[]> GetAttributedMembers<TAttribute>(
			this Type source,
			MemberTypes memberTypes = MemberTypes.All,
			bool inherit = false,
			BindingFlags bindingFlags = ReflectionHelper.AllClassMembers )
			where TAttribute : Attribute
		{
			return (
				from member in source.GetMembers( bindingFlags )
				from attribute in (Attribute[])member.GetCustomAttributes( typeof( TAttribute ), inherit )
				where member.MemberType.HasFlag( memberTypes )
				group attribute by member
				).ToDictionary( g => g.Key, g => g.Cast<TAttribute>().ToArray() );
		}

		/// <summary>
		///   Searches for all methods defined in an interface and its inherited interfaces.
		/// </summary>
		/// <param name = "source">The source of this extension method.</param>
		/// <param name = "bindingFlags">
		///   A bitmask comprised of one or more <see cref = "BindingFlags" /> that specify how the search is conducted.
		///   -or-
		///   Zero, to return null.
		/// </param>
		/// <returns>A list of all found methods.</returns>
		public static IEnumerable<MethodInfo> GetFlattenedInterfaceMethods( this Type source, BindingFlags bindingFlags )
		{
			foreach ( var info in source.GetMethods( bindingFlags ) )
			{
				yield return info;
			}

			var flattened = source.GetInterfaces().SelectMany( interfaceType => GetFlattenedInterfaceMethods( interfaceType, bindingFlags ) );
			foreach ( var info in flattened )
			{
				yield return info;
			}
		}
	}
}