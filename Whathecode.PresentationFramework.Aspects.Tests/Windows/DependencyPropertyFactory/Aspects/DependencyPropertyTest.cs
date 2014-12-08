using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.DependencyPropertyFactory;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory.Aspects
{
	// HACK: This hides the warning for the imported type conflict, caused by BaseDependencyPropertyFactoryTest being added as a link.
	//       The base source file needs to be added as link in order for the unit tests to run due to limitations of the framework.
	#pragma warning disable 0436

	/// <summary>
	///   Unit tests for <see cref = "DependencyPropertyFactory{T}">DependencyPropertyFactory</see> simplified with an aspect by PostSharp.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[TestClass]
	public class DependencyPropertyTest : BaseDependencyPropertyTest<TestControl> { }

	#pragma warning restore 0436
}