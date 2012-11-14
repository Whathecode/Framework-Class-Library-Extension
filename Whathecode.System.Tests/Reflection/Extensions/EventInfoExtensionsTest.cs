using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.Tests.System.Reflection.Extensions
{
	[TestClass]
	public class EventInfoExtensionsTest
	{
		class StaticEvents
		{
			#pragma warning disable 67
			public event Action NonStatic;
			static public event Action Static;			

			static event Action CustomEvent;
			public event Action Custom
			{
				add { CustomEvent += value; }
				remove { CustomEvent -= value; }
			}
			static public event Action CustomStatic
			{
				add { CustomEvent += value; }
				remove { CustomEvent -= value; }
			}
			#pragma warning restore 67
		}

		[TestMethod]
		public void IsStaticTest()
		{
			Assert.IsFalse( typeof( StaticEvents ).GetEvent( "NonStatic" ).IsStatic() );
			Assert.IsTrue( typeof( StaticEvents ).GetEvent( "Static" ).IsStatic() );
			Assert.IsFalse( typeof( StaticEvents ).GetEvent( "Custom" ).IsStatic() );
			Assert.IsTrue( typeof( StaticEvents ).GetEvent( "CustomStatic" ).IsStatic() );
		}
	}
}
