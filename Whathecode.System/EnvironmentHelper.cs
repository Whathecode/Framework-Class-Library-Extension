using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Whathecode.Interop;
using Whathecode.System.Runtime.InteropServices;


namespace Whathecode.System
{
	/// <summary>
	///   A helper class to do common <see cref="Environment" /> operations.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class EnvironmentHelper
	{
		// TODO: This only lists old special folders with CSIDL's. Also support new folders from after and including Windows Vista.
		//       It also seems to be missing e.g. CSIDL_CONTROLS, CSIDL_FAVORITES, CSIDL_COMMON_FAVORITES, CSIDL_INTERNET, CSIDL_BITBUCKET (recycle bin), ...
		static readonly Dictionary<Environment.SpecialFolder, Guid> SpecialFolderGuids = new Dictionary<Environment.SpecialFolder, Guid>
		{
			{ Environment.SpecialFolder.AdminTools, new Guid( "724EF170-A42D-4FEF-9F26-B60E846FBA4F" ) },
			{ Environment.SpecialFolder.ApplicationData, new Guid( "3EB685DB-65F9-4CF6-A03A-E3EF65729F3D" ) },
			{ Environment.SpecialFolder.CDBurning, new Guid( "9E52AB10-F80D-49DF-ACB8-4330F5687855" ) },
			{ Environment.SpecialFolder.CommonAdminTools, new Guid( "D0384E7D-BAC3-4797-8F14-CBA229B392B5" ) },
			{ Environment.SpecialFolder.CommonApplicationData, new Guid( "62AB5D82-FDC1-4DC3-A9DD-070D1D495D97" ) },
			{ Environment.SpecialFolder.CommonDesktopDirectory, new Guid( "C4AA340D-F20F-4863-AFEF-F87EF2E6BA25" ) },
			{ Environment.SpecialFolder.CommonDocuments, new Guid( "ED4824AF-DCE4-45A8-81E2-FC7965083634" ) },
			{ Environment.SpecialFolder.CommonMusic, new Guid( "3214FAB5-9757-4298-BB61-92A9DEAA44FF" ) },
			{ Environment.SpecialFolder.CommonOemLinks, new Guid( "C1BAE2D0-10DF-4334-BEDD-7AA20B227A9D" ) },
			{ Environment.SpecialFolder.CommonPictures, new Guid( "B6EBFB86-6907-413C-9AF7-4FC2ABF07CC5" ) },
			{ Environment.SpecialFolder.CommonProgramFiles, new Guid( "F7F1ED05-9F6D-47A2-AAAE-29D317C6F066" ) },
			{ Environment.SpecialFolder.CommonProgramFilesX86, new Guid( "DE974D24-D9C6-4D3E-BF91-F4455120B917" ) },
			{ Environment.SpecialFolder.CommonPrograms, new Guid( "0139D44E-6AFE-49F2-8690-3DAFCAE6FFB8" ) },
			{ Environment.SpecialFolder.CommonStartMenu, new Guid( "A4115719-D62E-491D-AA7C-E74B8BE3B067" ) },
			{ Environment.SpecialFolder.CommonStartup, new Guid( "82A5EA35-D9CD-47C5-9629-E15D2F714E6E" ) },
			{ Environment.SpecialFolder.CommonTemplates, new Guid( "B94237E7-57AC-4347-9151-B08C6C32D1F7" ) },
			{ Environment.SpecialFolder.CommonVideos, new Guid( "2400183A-6185-49FB-A2D8-4A392A602BA3" ) },
			{ Environment.SpecialFolder.Cookies, new Guid( "2B0F765D-C0E9-4171-908E-08A611B84FF6" ) },
			{ Environment.SpecialFolder.Desktop, new Guid( "B4BFCC3A-DB2C-424C-B029-7FE99A87C641" ) },
			// TODO: Unsure about the difference between Desktop and DesktopDirectory.
			//{ Environment.SpecialFolder.DesktopDirectory, new Guid( "B4BFCC3A-DB2C-424C-B029-7FE99A87C641" ) },
			{ Environment.SpecialFolder.Favorites, new Guid( "1777F761-68AD-4D8A-87BD-30B759FA33DD" ) },
			{ Environment.SpecialFolder.Fonts, new Guid( "FD228CB7-AE11-4AE3-864C-16F3910AB8FE" ) },
			{ Environment.SpecialFolder.History, new Guid( "D9DC8A3B-B784-432E-A781-5A1130A75963" ) },
			{ Environment.SpecialFolder.InternetCache, new Guid( "352481E8-33BE-4251-BA85-6007CAEDCF9D" ) },
			{ Environment.SpecialFolder.LocalApplicationData, new Guid( "F1B32785-6FBA-4FCF-9D55-7B8E7F157091" ) },
			{ Environment.SpecialFolder.LocalizedResources, new Guid( "2A00375E-224C-49DE-B8D1-440DF7EF3DDC" ) },
			{ Environment.SpecialFolder.MyComputer, new Guid( "0AC0837C-BBF8-452A-850D-79D08E667CA7" ) },
			{ Environment.SpecialFolder.MyDocuments, new Guid( "FDD39AD0-238F-46AF-ADB4-6C85480369C7" ) },
			{ Environment.SpecialFolder.MyMusic, new Guid( "4BD8D571-6D19-48D3-BE97-422220080E43" ) },
			{ Environment.SpecialFolder.MyPictures, new Guid( "33E28130-4E1E-4676-835A-98395C3BC3BB" ) },
			{ Environment.SpecialFolder.MyVideos, new Guid( "18989B1D-99B5-455B-841C-AB7C74E4DDFC" ) },
			{ Environment.SpecialFolder.NetworkShortcuts, new Guid( "C5ABBF53-E17F-4121-8900-86626FC2C973" ) },
			// TODO: Unsure about the difference between Personal and MyDocuments.
			//{ Environment.SpecialFolder.Personal, new Guid( "FDD39AD0-238F-46AF-ADB4-6C85480369C7" ) },
			{ Environment.SpecialFolder.PrinterShortcuts, new Guid( "9274BD8D-CFD1-41C3-B35E-B13F55A758F4" ) },
			{ Environment.SpecialFolder.ProgramFiles, new Guid( "905e63b6-c1bf-494e-b29c-65b732d3d21a" ) },
			{ Environment.SpecialFolder.ProgramFilesX86, new Guid( "7C5A40EF-A0FB-4BFC-874A-C0F2E0B9FA8E" ) },
			{ Environment.SpecialFolder.Programs, new Guid( "A77F5D77-2E2B-44C3-A6A2-ABA601054A51" ) },
			{ Environment.SpecialFolder.Recent, new Guid( "AE50C081-EBD2-438A-8655-8A092E34987A" ) },
			{ Environment.SpecialFolder.Resources, new Guid( "8AD10C31-2ADB-4296-A8F7-E4701232C972" ) },
			{ Environment.SpecialFolder.SendTo, new Guid( "8983036C-27C0-404B-8F08-102D10DCFD74" ) },
			{ Environment.SpecialFolder.StartMenu, new Guid( "625B53C3-AB48-4EC1-BA1F-A1EF4146FC19" ) },
			{ Environment.SpecialFolder.Startup, new Guid( "B97D20BB-F46A-4C97-BA10-5E3608430854" ) },
			{ Environment.SpecialFolder.System, new Guid( "1AC14E77-02E7-4E5D-B744-2EB1AE5198B7" ) },
			{ Environment.SpecialFolder.SystemX86, new Guid( "D65231B0-B2F1-4857-A4CE-A8E7C6EA7D27" ) },
			{ Environment.SpecialFolder.Templates, new Guid( "A63293E8-664E-48DB-A079-DF759E0509F7" ) },
			{ Environment.SpecialFolder.UserProfile, new Guid( "5E6C858F-0E22-4760-9AFE-EA3317B67173" ) },
			{ Environment.SpecialFolder.Windows, new Guid( "F38BF404-1D43-42F2-9305-67DE0B28FC23" ) }

		};

		/// <summary>
		///   Determine whether system is running Windows Vista or later operating systems.
		///   A lot of new features got introduced in Vista which might need to be checked for.
		/// </summary>
		public static bool VistaOrHigher
		{
			get { return Environment.OSVersion.Version.Major >= 6; }
		}

		/// <summary>
		///   Sets the path of the system special folder that is identified by the specified enumeration.
		/// </summary>
		/// <param name="folder">An enumerated constant that identifies a system special folder.</param>
		/// <param name="path">The full path to the folder which should be used for the specified system special folder.</param>
		public static void SetFolderPath( Environment.SpecialFolder folder, string path )
		{
			Guid folderGuid = SafeGetSpecialFolderGuid( folder );

			// Verify valid path.
			if ( !Directory.Exists( path ) )
			{
				throw new ArgumentException( "Invalid path.", "path" );
			}

			int result = Shell32.SHSetKnownFolderPath( ref folderGuid, Shell32.KnownFolderRetrievalFlags.None, new SafeTokenHandle(), path );
			Marshal.ThrowExceptionForHR( result );
		}

		/// <summary>
		///   Gets the default path to the system special folder that is identified by the specified enumeration.
		/// </summary>
		/// <param name="folder">An enumerated constant that identifies a system special folder.</param>
		public static string GetDefaultPath( Environment.SpecialFolder folder )
		{
			Guid folderGuid = SafeGetSpecialFolderGuid( folder );

			IntPtr pathPointer;
			int result = Shell32.SHGetKnownFolderPath( ref folderGuid, Shell32.KnownFolderRetrievalFlags.DefaultPath, new SafeTokenHandle(), out pathPointer );
			string path = Marshal.PtrToStringUni( pathPointer );
			Marshal.FreeCoTaskMem( pathPointer );
			Marshal.ThrowExceptionForHR( result );

			// TODO: Which possible paths can be returned here?
			return path;
		}

		static Guid SafeGetSpecialFolderGuid( Environment.SpecialFolder folder )
		{
			Guid folderGuid;
			bool retrieved = SpecialFolderGuids.TryGetValue( folder, out folderGuid );
			if ( !retrieved )
			{
				string message = String.Format( "The special folder path \"{0}\" is currently not supported.", folder );
				throw new NotSupportedException( message );
			}

			return folderGuid;
		}
	}
}
