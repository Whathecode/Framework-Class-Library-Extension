using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.DependencyPropertyFactory;


namespace Whathecode.Tests.System.Windows.DependencyPropertyFactory.Aspects
{
    /// <summary>
    ///   Unit tests for <see cref = "DependencyPropertyFactory{T}">DependencyPropertyFactory</see> simplified with an aspect by PostSharp.
    /// </summary>
    /// <author>Steven Jeuris</author>
    [TestClass]
    public class DependencyPropertyFactoryTest : BaseDependencyPropertyFactoryTest<TestControl>
    {
    }
}