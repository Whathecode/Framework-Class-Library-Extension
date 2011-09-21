using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using TriAxis.RunSharp;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Reflection.Emit
{
	/// <summary>
	///   A class which allows generating proxy classes.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class Proxy
	{
		/// <summary>
		///   Create a wrapper class for a generic interface with more general type parameters than the wrapped interface.
		///   Downcasts to the correct more specific type are generated where necessary.
		///   This of course breaks type safety, and only calls to the class with the correct orginal types will work.
		///   Incorrect calls will throw <see cref = "InvalidCastException" />.
		/// </summary>
		/// <remarks>
		///   This is useful during reflection, when you don't want to know about specific types, but you can guarantee
		///   that a certain call will always be done with objects of the correct type.
		/// </remarks>
		/// <typeparam name = "T">The less-specific generic type of the wrapper which will be generated.</typeparam>
		/// <param name = "o">The object to wrap, which should implement the desired interface, with arbitrary type parameters.</param>
		/// <returns>An instance of the specified type which wraps the given object.</returns>
		public static T CreateGenericInterfaceWrapper<T>( object o )
		{
			Contract.Requires( o.GetType().IsOfGenericType( typeof( T ).GetGenericTypeDefinition() ) );
			Contract.Requires( typeof( T ).IsInterface );

			Type typeToCreate = typeof( T );
			Type typeToCreateGeneric = typeToCreate.GetGenericTypeDefinition();
			Type innerType = o.GetType();
			Type innerMatchingType = innerType.GetMatchingGenericType( typeToCreateGeneric );

			// Implement passed type and redirect all public calls to inner instance.
			AssemblyGen assembly = new AssemblyGen( "Whathecode.System.RunSharp" );
			TypeGen type = assembly.Public.Class( "Wrapped" + typeToCreate.Name, typeof( object ), typeToCreate );
			{
				const string inner = "inner";

				FieldGen innerInstance = type.Private.Field( innerType, "_innerInstance" );

				// Create constructor which takes the wrapped instance as an argument.
				ConstructorGen constructor = type.Public.Constructor();
				{
					constructor.Parameter( innerType, inner );

					CodeGen code = constructor.GetCode();
					{
						code.Assign( innerInstance, code.Arg( inner ) );
					}
				}

				// Create methods.
				MethodInfo[] innerMethods = innerMatchingType.GetFlattenedInterfaceMethods( ReflectionHelper.AllInstanceMembers ).ToArray();
				MethodInfo[] toCreateMethods = typeToCreate.GetFlattenedInterfaceMethods( ReflectionHelper.AllInstanceMembers ).ToArray();
				foreach ( var method in innerMethods
					.Zip( toCreateMethods,
						( matching, toCreate ) => new
						{
							Matching = matching,
							ToCreate = toCreate
						} )
					.Where( z => z.Matching.IsPublic || z.Matching.IsFamily ) )
				{
					// TODO: Not quite certain why override is required for extended interfaces (DeclaringType != typeTocreate),
					//       but this seems to work.
					MethodInfo toCreate = method.ToCreate;
					MethodGen methodGen = toCreate.DeclaringType == typeToCreate
						? type.MethodImplementation( typeToCreate, toCreate.ReturnType, toCreate.Name )
						: type.Public.Override.Method( toCreate.ReturnType, toCreate.Name );
					{
						ParameterInfo[] toCreateParameters = method.ToCreate.GetParameters();
						var parameters = toCreateParameters
							.Select( p =>
							{
								var info = methodGen.BeginParameter( p.ParameterType, p.Name );
								info.End();
								return info;
							} ).ToArray();

						CodeGen code = methodGen.GetCode();
						{
							// Cast arguments to the type of the inner instance.
							Operand[] args = parameters.Select( p => code.Arg( p.Name ) ).ToArray();
							Operand[] castArgs = new Operand[] { };
							if ( args.Length > 0 )
							{
								Type[] parameterTypes = method.Matching.GetParameters().Select( p => p.ParameterType ).ToArray();
								MethodInfo methodToCall
									= innerType.GetMethod( method.ToCreate.Name, ReflectionHelper.AllInstanceMembers, parameterTypes );
								castArgs = methodToCall.GetParameters()
									.Select( ( p, index ) => args[ index ].Cast( typeof( object ) ).Cast( p.ParameterType ) ).ToArray();
							}

							// Call inner instance and return value when needed.                            
							if ( method.ToCreate.ReturnType != typeof( void ) )
							{
								Operand result = innerInstance.Invoke( method.ToCreate.Name, castArgs );
								code.Return( result.Cast( method.ToCreate.ReturnType ) );
							}
							else
							{
								code.Invoke( innerInstance, method.ToCreate.Name, castArgs );
							}
						}
					}
				}
			}
			Type wrapperType = type.GetCompletedType( true );

			return (T)Activator.CreateInstance( wrapperType, new[] { o } );
		}
	}
}