using System;


namespace Whathecode.Interop
{
	public static class Constants
	{
		public const int MaximumPathLength = 260;


		/// <summary>
		///   Set of standard access rights that correspond to operations common to most types of securable objects.
		/// </summary>
		[Flags]
		public enum StandardAccessRights : uint
		{
			/// <summary>
			///   The right to delete the object.
			/// </summary>
			Delete = 0x00010000,
			/// <summary>
			///   The right to read the information in the object's security descriptor, not including the information in the system access control list (SACL).
			/// </summary>
			ReadControl = 0x00020000,
			/// <summary>
			///   The right to use the object for synchronization. This enables a thread to wait until the object is in the signaled state.
			///   Some object types do not support this access right.
			/// </summary>
			Synchronize = 0x00100000,
			/// <summary>
			///   The right to modify the discretionary access control list (DACL) in the object's security descriptor.
			/// </summary>
			// ReSharper disable once InconsistentNaming
			WriteDACL = 0x00040000,
			/// <summary>
			///   The right to change the owner in the object's security descriptor.
			/// </summary>
			WriteOwner = 0x00080000,
			All = Delete | ReadControl | WriteDACL | WriteOwner | Synchronize,
			Execute = ReadControl,
			Read = ReadControl,
			Required = Delete | ReadControl | WriteDACL | WriteOwner,
			Write = ReadControl
		}
	}
}
