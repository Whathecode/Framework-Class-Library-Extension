using System;
using System.Diagnostics.Contracts;
using Whathecode.System.ComponentModel.Validation;
using Whathecode.System.Reflection.Emit;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Validators
{
    /// <summary>
    ///   When applied to a property in combination with <see cref="DependencyPropertyAttribute" />
    ///   specifies how the property should be validated.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [AttributeUsage( AttributeTargets.Property )]
    public class ValidationHandlerAttribute : AbstractGenericAttribute
    {
        public readonly AbstractValidation<object> GenericValidation;


        /// <summary>
        ///   Create a new attribute which hooks a validation handler to a dependency property.
        /// </summary>
        /// <param name="dynamicType">
        ///   The type of the validation handler.
        ///   Should extend from <see cref="AbstractValidation{T}" />.</param>
        /// <param name="constructorArguments">The arguments to pass to the constructor of the validation handler.</param>
        public ValidationHandlerAttribute( Type dynamicType, params object[] constructorArguments )
            : base( dynamicType, constructorArguments )
        {
            Contract.Requires( dynamicType.IsOfGenericType( typeof( AbstractValidation<> ) ) );

            GenericValidation = EmitHelper.CreateCompatibleGenericWrapper<AbstractValidation<object>>( DynamicInstance );
        }
    }
}