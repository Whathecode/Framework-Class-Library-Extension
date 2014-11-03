using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using TriAxis.RunSharp;
using Whathecode.System.Linq;
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
			return (T)CreateGenericInterfaceWrapper( typeof( T ), o );
		}

		/// <summary>
		///   Create a wrapper class for a generic interface with more general type parameters than the wrapped interface.
		///   Downcasts to the correct more specific type are generated where necessary.
		///   This of course breaks type safety, and only calls to the class with the correct orginal types will work.
		///   Incorrect calls will throw <see cref = "InvalidCastException" />.
		/// </summary>
		/// <remarks>
		///   This is useful during reflection, when you don't want to know about specific types, but you can guarantee
		///   that a certain call will always be done with objects of the correct type.
		///   TODO: This non-generic method is only needed since RunSharp can't call generic methods, needed to generate wrappers recursively.
		///   TODO: Possibly Castle DynamicProxy could replace this if it allows creating 'non-matching' proxies and thus support the downcasting.
		/// </remarks>
		/// <param name = "typeToCreate">The less-specific generic type of the wrapper which will be generated.</param>
		/// <param name = "o">The object to wrap, which should implement the desired interface, with arbitrary type parameters.</param>
		/// <returns>An instance of the specified type which wraps the given object.</returns>
		public static object CreateGenericInterfaceWrapper( Type typeToCreate, object o )
		{
			Contract.Requires( o.GetType().IsOfGenericType( typeToCreate.GetGenericTypeDefinition() ) );
			Contract.Requires( typeToCreate.IsInterface );

			Type typeToCreateGeneric = typeToCreate.GetGenericTypeDefinition();
			Type innerType = o.GetType();
			Type innerMatchingType = innerType.GetMatchingGenericType( typeToCreateGeneric );

			// Implement passed type and redirect all public calls to inner instance.
			var assembly = new AssemblyGen( "Whathecode.System.RunSharp" );
			TypeGen type = assembly.Public.Class( "Wrapped" + typeToCreate.Name, typeof( object ), typeToCreate );
			{
				const string inner = "inner";

				FieldGen innerInstance = type.Private.Field( innerType, "_innerInstance" );
				FieldGen returnCached = type.Private.Field( typeof( Dictionary<int, object> ), "_returnCached" );
				FieldGen returnWrappers = type.Private.Field( typeof( Dictionary<int, object> ), "_returnWrappers" );

				// Create constructor which takes the wrapped instance as an argument.
				ConstructorGen constructor = type.Public.Constructor();
				{
					constructor.Parameter( innerType, inner );

					CodeGen code = constructor.GetCode();
					{
						code.Assign( innerInstance, code.Arg( inner ) );
						code.Assign( returnCached, Exp.New( typeof( Dictionary<int, object> ) ) );
						code.Assign( returnWrappers, Exp.New( typeof( Dictionary<int, object> ) ) );
					}
				}

				// Create methods.
				int methodCount = 0;
				MethodInfo[] innerMethods = innerMatchingType.GetFlattenedInterfaceMethods( ReflectionHelper.FlattenedInstanceMembers ).ToArray();
				MethodInfo[] toCreateMethods = typeToCreate.GetFlattenedInterfaceMethods( ReflectionHelper.FlattenedInstanceMembers ).ToArray();
				MethodInfo[] genericMethods = typeToCreateGeneric.GetFlattenedInterfaceMethods( ReflectionHelper.FlattenedInstanceMembers ).ToArray();
				foreach ( var method in innerMethods
					.Zip( toCreateMethods, genericMethods,
						( matching, toCreate, generic ) => new
						{
							Id = methodCount++,
							Matching = matching,
							ToCreate = toCreate,
							Generic = generic
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
						ParameterInfo[] toCreateParameters = toCreate.GetParameters();
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
							Operand[] castArgs = { };
							if ( args.Length > 0 )
							{
								Type[] parameterTypes = method.Matching.GetParameters().Select( p => p.ParameterType ).ToArray();
								// TODO: When searching for generic methods, GetMethod returns null.
								MethodInfo methodToCall
									= innerType.GetMethod( toCreate.Name, ReflectionHelper.FlattenedInstanceMembers, parameterTypes );
								castArgs = methodToCall.GetParameters()
									.Select( ( p, index ) => args[ index ].Cast( typeof( object ) ).Cast( p.ParameterType ) ).ToArray();
							}

							// Call inner instance and return value when needed.
							if ( toCreate.ReturnType != typeof( void ) )
							{
								Operand result = innerInstance.Invoke( toCreate.Name, castArgs );

								// Wrappers will recursively need to be created for generic return types.
								Type genericReturnType = method.Generic.ReturnType;
								if ( genericReturnType.IsGenericType && genericReturnType.ContainsGenericParameters && genericReturnType.IsInterface )
								{
									// Check whether a new result is returned.
									Operand innerCached = code.Local( typeof( object ) );
									code.If( returnCached.Invoke( "TryGetValue", method.Id, innerCached.Ref() ) );
									{
										code.If( (innerCached == result).LogicalNot() );
										{
											code.Invoke( returnWrappers, "Remove", method.Id );
											code.Invoke( returnCached, "Remove", method.Id );
											code.Invoke( returnCached, "Add", method.Id, result );
										}
										code.End();
									}
									code.Else();
									{
										code.Invoke( returnCached, "Add", method.Id, result );
									}
									code.End();

									// Check whether a wrapper needs to be generated.
									Operand wrappedCached = code.Local( typeof( object ) );
									code.If( returnWrappers.Invoke( "TryGetValue", method.Id, wrappedCached.Ref() ).LogicalNot() );
									{
										Operand proxied = Static.Invoke( typeof( Proxy ), "CreateGenericInterfaceWrapper", toCreate.ReturnType, result );
										code.Assign( wrappedCached, proxied );
										code.Invoke( returnWrappers, "Add", method.Id, wrappedCached );
									}
									code.End();
									code.Return( wrappedCached.Cast( toCreate.ReturnType ) );
								}
								else
								{
									// A simple cast will work.
									// TODO: Throw proper exception when this is known to fail. E.g. generic type which is not an interface?
									code.Return( result.Cast( toCreate.ReturnType ) );
								}
							}
							else
							{
								code.Invoke( innerInstance, toCreate.Name, castArgs );
							}
						}
					}
				}
			}
			Type wrapperType = type.GetCompletedType( true );

			return Activator.CreateInstance( wrapperType, new[] { o } );
		}
	}
}