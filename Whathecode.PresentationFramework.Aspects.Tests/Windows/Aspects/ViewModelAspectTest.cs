using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.ComponentModel.NotifyPropertyFactory.Attributes;
using Whathecode.System.Reflection.Extensions;
using Whathecode.System.Windows.Aspects.ViewModel;
using Whathecode.System.Windows.Input.CommandFactory;
using Whathecode.System.Windows.Input.CommandFactory.Attributes;


namespace Whathecode.Tests.System.Windows.Aspects
{
	[ViewModel( typeof( Properties ), typeof( Commands ) )]
	public class DuckViewModel
	{
		public enum Properties
		{
			Color,
			CanQuack
		}


		public enum Commands
		{
			Quack
		}


		[NotifyProperty( Properties.Color )]
		public string Color { get; set; }

		[NotifyProperty( Properties.CanQuack )]
		public bool CanQuack { get; set; }

		public bool QuackCalled { get; private set; }

		[CommandExecute( Commands.Quack )]
		public void Quack()
		{
			QuackCalled = true;
		}
	}


	[TestClass]
	public class ViewModelAspectTest
	{
		#region Common test members

		DuckViewModel _viewModel;


		[TestInitialize]
		public void Initialize()
		{
			_viewModel = new DuckViewModel();
		}

		#endregion // Common test members


		[TestMethod]
		public void GetterSetterTest()
		{
			Assert.AreEqual( null, _viewModel.Color );
			_viewModel.Color = "yellow";
			Assert.AreEqual( "yellow", _viewModel.Color );
		}

		[TestMethod]
		public void TriggerCommandTest()
		{
			// Find command.
			MemberInfo commandFactory = _viewModel.GetType().GetMembers( typeof( CommandFactory<> ) ).First();
			var factory = (CommandFactory<DuckViewModel.Commands>)_viewModel.GetValue( commandFactory );
			IDictionary dictionary = factory.Commands;

			// Execute command.
			var quackCommand = dictionary[ DuckViewModel.Commands.Quack ] as ICommand;
			Assert.IsNotNull( quackCommand );
			quackCommand.Execute( null );
			Assert.IsTrue( _viewModel.QuackCalled );
		}
	}
}