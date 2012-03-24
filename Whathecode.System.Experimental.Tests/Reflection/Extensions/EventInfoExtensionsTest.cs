using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Experimental.Reflection.Extensions;


namespace Whathecode.Tests.System.Experimental.Reflection.Extensions
{
	[TestClass]
	public class EventInfoExtensionsTest
	{
		class EventClass
		{
			#pragma warning disable 67
			public event Action Public;
			event Action Private;
			public event EventHandler EventHandler;
			#pragma warning restore 67

			Action _custom;
			public event Action Custom
			{
				add { _custom = (Action)Delegate.Combine( _custom, value ); }
				remove { _custom = (Action)Delegate.Remove( _custom, value ); }
			}
		}


		[TestMethod]
		public void GetFieldInfoTest()
		{
			// The following tests only succeed when the compiler uses the same for the delegate backing field for compiler generated events.
			Type type = typeof( EventClass );
			EventInfo publicEvent = type.GetEvent( "Public" );
			Assert.IsNotNull( publicEvent.GetFieldInfo() );
			EventInfo privateEvent = type.GetEvent( "Private", BindingFlags.NonPublic | BindingFlags.Instance );
			Assert.IsNotNull( privateEvent.GetFieldInfo() );
			EventInfo eventHandlerEvent = type.GetEvent( "EventHandler" );
			Assert.IsNotNull( eventHandlerEvent.GetFieldInfo() );

			// An event with custom accessors can (and with the C# compiler has to) use a differently named backing field.
			EventInfo customEvent = type.GetEvent( "Custom" );
			MethodInfo regularAddMethod = publicEvent.GetAddMethod();
			MethodInfo customAddMethod = customEvent.GetAddMethod();
			Assert.IsNull( customEvent.GetFieldInfo() );
		}
	}
}
