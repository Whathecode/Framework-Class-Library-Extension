using System;


namespace Whathecode.System
{
	/// <summary>
	///   A helper class to do common <see cref="Environment" /> operations.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class EnvironmentHelper
	{
		/// <summary>
		///   Determine whether system is running Windows Vista or later operating systems.
		///   A lot of new features got introduced in Vista which might need to be checked for.
		/// </summary>
		public static bool VistaOrHigher
		{
			get { return Environment.OSVersion.Version.Major >= 6; }
		}
	}
}
