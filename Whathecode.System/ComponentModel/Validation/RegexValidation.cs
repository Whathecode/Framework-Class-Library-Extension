using System.Text.RegularExpressions;


namespace Whathecode.System.ComponentModel.Validation
{
	/// <summary>
	///   Validator using regex to validate strings.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public class RegexValidation : AbstractValidation<string>
	{
		readonly string _pattern;


		/// <summary>
		///   Create a new regex validator which can validate strings based on a specified regex pattern.
		/// </summary>
		/// <param name = "regexPattern">The regular expression pattern to match.</param>
		public RegexValidation( string regexPattern )
		{
			_pattern = regexPattern;
		}


		public override bool IsValid( string value )
		{
			return Regex.IsMatch( value, _pattern );
		}
	}
}