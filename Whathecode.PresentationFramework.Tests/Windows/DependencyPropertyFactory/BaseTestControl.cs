using System.Diagnostics;
using System.Windows;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory
{
	public class BaseTestControl : UIElement
	{
		public static bool IsValidateCallbackCalled { get; private set; }

		public bool IsCoerceCallbackCalled { get; private set; }

		public bool IsChangedCallbackCalled { get; private set; }

		public bool AllCallbacksCalled
		{
			get { return IsCoerceCallbackCalled && IsChangedCallbackCalled && IsValidateCallbackCalled; }
		}


		protected static bool ValidateCallbackCalled( object o )
		{
			string oString = o == null ? "null" : o.ToString();
			Debug.WriteLine( "Validate callback called. Object to validate: " + oString );

			IsValidateCallbackCalled = true;

			return true;
		}

		protected static object CoerceCallbackCalled( DependencyObject d, object o )
		{
			Debug.WriteLine( "Coerce callback called." );

			BaseTestControl control = d as BaseTestControl;
			if ( control != null )
			{
				control.IsCoerceCallbackCalled = true;
			}

			return o;
		}

		protected static void ChangedCallbackCalled( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			Debug.WriteLine( "Changed callback called." );

			BaseTestControl control = d as BaseTestControl;
			if ( control != null )
			{
				control.IsChangedCallbackCalled = true;
			}
		}

		public void ResetCallbackFlags()
		{
			IsValidateCallbackCalled = false;
			IsCoerceCallbackCalled = false;
			IsChangedCallbackCalled = false;
		}
	}
}