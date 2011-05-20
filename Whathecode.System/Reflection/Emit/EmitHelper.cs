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

            // Extend from passed type and redirect all public calls to inner instance.
            AssemblyGen assembly = new AssemblyGen( "Whathecode.System.RunSharp" );
            TypeGen type = assembly.Public.Class( typeToCreate.Name, typeToCreate );
            {
                const string inner = "inner";

                FieldGen innerInstance = type.Private.Field( innerType, "_innerInstance" );

                // Create constructors.
                foreach ( var toCreateConstructor in typeToCreate.GetConstructors( ReflectionHelper.AllInstanceMembers ) )
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

                // Create methods.
                foreach ( var methods in innerMatchingType.GetMethods( ReflectionHelper.AllInstanceMembers )
                    .Zip( typeToCreate.GetMethods( ReflectionHelper.AllInstanceMembers ),
                          ( matching, toCreate ) => new
                          {
                              Matching = matching,
                              ToCreate = toCreate                        
                          } )
                    .Where( z => z.Matching.IsPublic || z.Matching.IsFamily ) )
                {
                    MethodGen method = type.Public.Override.Method( methods.ToCreate.ReturnType, methods.ToCreate.Name );
                    {
                        ParameterInfo[] toCreateParameters = methods.ToCreate.GetParameters();
                        var parameters = toCreateParameters
                            .Select( p =>
                            {
                                var info = method.BeginParameter( p.ParameterType, p.Name );
                                info.End();
                                return info;
                            } ).ToArray();

                        CodeGen code = method.GetCode();
                        {
                            // Cast arguments to the type of the inner instance.
                            Operand[] args = parameters.Select( p => code.Arg( p.Name ) ).ToArray();
                            Operand[] castArgs = new Operand[] { };
                            if ( args.Length > 0 )
                            {
                                Type[] parameterTypes = methods.Matching.GetParameters().Select( p => p.ParameterType ).ToArray();
                                MethodInfo methodToCall 
                                    = innerType.GetMethod( methods.ToCreate.Name, ReflectionHelper.AllInstanceMembers, parameterTypes );
                                castArgs = methodToCall.GetParameters()
                                    .Select( ( p, index ) => args[ index ].Cast( typeof( object ) ).Cast( p.ParameterType ) ).ToArray();
                            }

                            // Call inner instance and return value when needed.
                            Operand result = innerInstance.Cast( innerType ).Invoke( methods.ToCreate.Name, castArgs );
                            if ( methods.ToCreate.ReturnType != typeof( void ) )
                            {
                                code.Return( result );
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