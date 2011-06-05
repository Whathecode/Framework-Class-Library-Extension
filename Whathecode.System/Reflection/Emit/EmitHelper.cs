using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using TriAxis.RunSharp;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Reflection.Emit
{
    /// <summary>
    ///   A helper class to do common Emit operations.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public static class EmitHelper
    {
        public static T CreateCompatibleGenericWrapper<T>( object o, object[] arguments = null )
        {
            Contract.Requires( o.GetType().IsOfGenericType( typeof( T ).GetGenericTypeDefinition() ) );

            Type typeToCreate = typeof( T );
            Type typeToCreateGeneric = typeToCreate.GetGenericTypeDefinition();
            Type innerType = o.GetType();
            Type innerMatchingType = innerType.GetMatchingGenericType( typeToCreateGeneric );
            bool isInterface = typeToCreate.IsInterface;

            // Extend from passed type and redirect all public calls to inner instance.
            AssemblyGen assembly = new AssemblyGen( "Whathecode.System.RunSharp" );
            TypeGen type = isInterface
                               ? assembly.Public.Class( typeToCreate.Name, typeof( object ), typeToCreate )
                               : assembly.Public.Class( typeToCreate.Name, typeToCreate );
            {
                const string inner = "inner";

                FieldGen innerInstance = type.Private.Field( innerType, "_innerInstance" );

                // Create constructors.
                var toCreateConstructors = typeToCreate.GetConstructors( ReflectionHelper.AllInstanceMembers );
                foreach ( var toCreateConstructor in toCreateConstructors )
                {
                    ConstructorGen constructor = type.Public.Constructor();
                    {
                        // Always add an extra first parameter, passing the instance, followed by the normal constructor arguments.                        
                        constructor.Parameter( innerType, inner );
                        ParameterInfo[] requiredParameters = toCreateConstructor.GetParameters();
                        var parameters = requiredParameters
                            .Select( p =>
                            {
                                var info = constructor.BeginParameter( p.ParameterType, p.Name );
                                info.End();
                                return info;
                            } ).ToArray();

                        CodeGen code = constructor.GetCode();
                        {
                            code.InvokeBase( parameters.Select( p => code.Arg( p.Name ) ).ToArray() );
                            code.Assign( innerInstance, code.Arg( inner ) );
                        }
                    }
                }

                // Enforce at least one constructor.
                if ( toCreateConstructors.Count() == 0 )
                {
                    ConstructorGen emptyConstructor = type.Public.Constructor();
                    {
                        // Always add an extra first parameter, passing the instance, followed by the normal constructor arguments.                        
                        emptyConstructor.Parameter( innerType, inner );

                        CodeGen code = emptyConstructor.GetCode();
                        {
                            code.Assign( innerInstance, code.Arg( inner ) );
                        }
                    }
                }

                // Create methods.
                MethodInfo[] innerMethods 
                    = isInterface ? innerMatchingType.GetFlattenedInterfaceMethods( ReflectionHelper.AllInstanceMembers ).ToArray()
                                  : innerMatchingType.GetMethods( ReflectionHelper.AllInstanceMembers );
                MethodInfo[] toCreateMethods 
                    = isInterface ? typeToCreate.GetFlattenedInterfaceMethods( ReflectionHelper.AllInstanceMembers ).ToArray()
                                  : typeToCreate.GetMethods( ReflectionHelper.AllInstanceMembers );
                foreach ( var method in innerMethods
                    .Zip( toCreateMethods,
                          ( matching, toCreate ) => new
                          {
                              Matching = matching,
                              ToCreate = toCreate
                          } )
                    .Where( z => z.Matching.IsPublic || z.Matching.IsFamily ) )
                {
                    MethodGen methodGen = type.Public.Override.Method( method.ToCreate.ReturnType, method.ToCreate.Name );
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
                            Operand result = innerInstance.Invoke( method.ToCreate.Name, castArgs );
                            if ( method.ToCreate.ReturnType != typeof( void ) )
                            {
                                code.Return( result.Cast( method.ToCreate.ReturnType ) );
                            }
                        }
                    }
                }
            }
            Type wrapperType = type.GetCompletedType( true );

            object[] constructorArguments = new[] { o };
            if ( arguments != null )
            {
                constructorArguments = constructorArguments.Concat( arguments ).ToArray();
            }
            return (T)Activator.CreateInstance( wrapperType, constructorArguments );
        }
    }
}