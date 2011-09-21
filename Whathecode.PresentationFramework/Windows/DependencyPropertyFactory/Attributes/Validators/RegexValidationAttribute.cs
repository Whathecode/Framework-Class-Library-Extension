using Whathecode.System.ComponentModel.Validation;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes.Validators
{
	/// <summary>
	///   A validator which can validate strings based on a specified regex pattern.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class RegexValidationAttribute : ValidationHandlerAttribute
	{
		/// <summary>
		///   Create a new regex validator which can validate strings based on a specified regex pattern.
		/// </summary>
		/// <param name = "regexPattern">The regular expression pattern to match.</param>
		public RegexValidationAttribute( string regexPattern )
			: base( typeof( RegexValidation ), regexPattern ) {}
	}
}