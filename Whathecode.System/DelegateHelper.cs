﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Whathecode.System.Linq;


namespace Whathecode.System
{
    /// <summary>
    ///   A generic helper class to do common <see cref = "Delegate">Delegate</see> operations.
    ///   TODO: Add extra contracts to reenforce correct usage.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public static class DelegateHelper
    {
        /// <summary>
        ///   A struct which holds the expressions for the arguments when creating delegates.
        /// </summary>
        struct DelegateArgumentExpressions
        {
            public IEnumerable<ParameterExpression> OriginalArguments;
            public IEnumerable<Expression> ConvertedArguments;
        }


        /// <summary>
        ///   The name of the Invoke method of a Delegate.
        /// </summary>
        const string InvokeMethod = "Invoke";

        /// <summary>
        ///   Get method info for a specified delegate type.
        /// </summary>
        /// <param name = "delegateType">The delegate type to get info for.</param>
        /// <returns>The method info for the given delegate type.</returns>
        public static MethodInfo MethodInfoFromDelegateType( Type delegateType )
        {
            Contract.Requires<ArgumentException>( delegateType.IsSubclassOf( typeof( MulticastDelegate ) ), "Given type should be a delegate." );

            return delegateType.GetMethod( InvokeMethod );
        }

        /// <summary>
        ///   Creates a delegate of a specified type that represents the specified static or instance method,
        ///   with the specified first argument. Conversions are done when possible.
        /// </summary>
        /// <typeparam name = "TDelegate">The type for the delegate.</typeparam>
        /// <param name = "instance">The object to which the delegate is bound, or null to treat method as static.</param>
        /// <param name = "method">The MethodInfo describing the static or instance method the delegate is to represent.</param>
        public static TDelegate CreateCompatibleDelegate<TDelegate>( object instance, MethodInfo method )
        {
            MethodInfo delegateInfo = MethodInfoFromDelegateType( typeof( TDelegate ) );

            // Create delegate original and converted arguments.
            var delegateTypes = delegateInfo.GetParameters().Select( d => d.ParameterType );
            var methodTypes = method.GetParameters().Select( m => m.ParameterType );
            var delegateExpressions = CreateDelegateExpressions( delegateTypes, methodTypes );

            // Create method call.
            Expression methodCall = Expression.Call(
                instance == null ? null : Expression.Constant( instance ),
                method,
                delegateExpressions.ConvertedArguments );

            // Convert return type when necessary.
            Expression convertedMethodCall = delegateInfo.ReturnType == method.ReturnType
                                                 ? methodCall
                                                 : Expression.Convert( methodCall, delegateInfo.ReturnType );

            return Expression.Lambda<TDelegate>(
                convertedMethodCall,
                delegateExpressions.OriginalArguments
                ).Compile();
        }

        /// <summary>
        ///   Creates a delegate of a specified type that represents a method which can be executed on an instance passed as parameter.
        ///   Conversions are done when possible.
        /// </summary>
        /// <typeparam name="TDelegate">
        ///   The type for the delegate. This delegate should have at least one type parameter denoting the type of the instance
        ///   which will be passed.
        ///   E.g. Action&lt;ExampleObject, object&gt;,
        ///        where ExampleObject denotes the instance type and object denotes the desired type of the first parameter of the method.
        /// </typeparam>
        /// <param name="method">>The MethodInfo describing the method of the instance type.</param>
        public static TDelegate CreateDynamicInstanceDelegate<TDelegate>( MethodInfo method )
        {
            MethodInfo delegateInfo = MethodInfoFromDelegateType( typeof( TDelegate ) );
            var delegateParameters = delegateInfo.GetParameters();

            // Convert instance type when necessary.
            Type delegateInstanceType = delegateParameters.Select( p => p.ParameterType ).First();
            Type methodInstanceType = method.DeclaringType;
            ParameterExpression instance = Expression.Parameter( delegateInstanceType );
            Expression convertedInstance = delegateInstanceType == methodInstanceType
                                               ? (Expression)instance
                                               : Expression.Convert( instance, delegateInstanceType );

            // Create delegate original and converted arguments.
            var delegateTypes = delegateParameters.Select( d => d.ParameterType ).Skip( 1 );
            var methodTypes = method.GetParameters().Select( m => m.ParameterType );
            var delegateExpressions = CreateDelegateExpressions( delegateTypes, methodTypes );

            // Create method call.
            Expression methodCall = Expression.Call(
                convertedInstance,
                method,
                delegateExpressions.ConvertedArguments );

            // Convert return type when necessary.
            Expression convertedMethodCall = delegateInfo.ReturnType == method.ReturnType
                                                 ? methodCall
                                                 : Expression.Convert( methodCall, delegateInfo.ReturnType );

            return Expression.Lambda<TDelegate>(
                convertedMethodCall,
                new[] { instance }.Concat( delegateExpressions.OriginalArguments )
                ).Compile();
        }

        /// <summary>
        ///   Creates the expressions for the delegate parameters and their conversions
        ///   to the corresponding required types for the method parameters.
        /// </summary>
        /// <param name = "delegateTypes">The types of the delegate parameters.</param>
        /// <param name = "methodTypes">The required types of the method parameters.</param>
        /// <returns>An object containing the delegate expressions.</returns>
        static DelegateArgumentExpressions CreateDelegateExpressions(
            IEnumerable<Type> delegateTypes,
            IEnumerable<Type> methodTypes )
        {
            var delegateArguments = delegateTypes.Select( Expression.Parameter ).ToArray(); // ToArray prevents deferred execution.   

            // Convert the arguments from the delegate argument type to the method argument type when necessary.
            var convertedArguments = delegateArguments.Zip(
                methodTypes, delegateTypes,
                ( delegateArgument, methodType, delegateType ) => methodType != delegateType
                                                                      ? (Expression)Expression.Convert( delegateArgument, methodType )
                                                                      : delegateArgument );

            return new DelegateArgumentExpressions
            {
                OriginalArguments = delegateArguments,
                ConvertedArguments = convertedArguments
            };
        }
    }
}