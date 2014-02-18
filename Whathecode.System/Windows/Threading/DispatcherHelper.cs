using System;
using System.Windows.Threading;


namespace Whathecode.System.Windows.Threading
{
	/// <summary>
	///   A helper class for dispatcher operations.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class DispatcherHelper
	{
		public static void SafeDispatch( Dispatcher dispatcher, Action action )
		{
			if ( dispatcher.CheckAccess() )
			{
				action();
			}
			else
			{
				dispatcher.Invoke( action );
			}
		}

		public static void SafeDispatch<TParameter>( Dispatcher dispatcher, Action<TParameter> action, TParameter argument )
		{
			if ( dispatcher.CheckAccess() )
			{
				action( argument );
			}
			else
			{
				dispatcher.Invoke( action, argument );
			}
		}

		public static TResult SafeDispatch<TResult>( Dispatcher dispatcher, Func<TResult> action )
		{
			return dispatcher.CheckAccess()
				? action()
				: dispatcher.Invoke( action );
		}
	}
}