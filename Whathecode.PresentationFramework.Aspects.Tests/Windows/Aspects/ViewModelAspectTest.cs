using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.ComponentModel.NotifyPropertyFactory.Attributes;
using Whathecode.System.Windows.Aspects.ViewModel;
using Whathecode.System.Windows.Input.CommandFactory;
using Whathecode.System.Windows.Input.CommandFactory.Attributes;
using Whathecode.System.Reflection.Extensions;


namespace Whathecode.Tests.System.Windows.Aspects
{
    [TestClass]
    public class ViewModelAspectTest
    {
        #region Common test members

        /// <summary>
        ///   The name of the property holding the dictionary with all the commands
        ///   in the CommandFactory.
        ///   TODO: Can this string literal somehow be circumvented by accessing the generic class directly?
        /// </summary>
        const string CommandsProperty = "Commands";

        [ViewModel( typeof( Properties ), typeof( Commands ) )]
        public class DuckViewModel
        {
            enum Properties
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
            object factory = _viewModel.GetValue( commandFactory );
            IDictionary dictionary = factory.GetPropertyValue( CommandsProperty ) as IDictionary;

            // Execute command.
            var quackCommand = dictionary[ DuckViewModel.Commands.Quack ] as ICommand;
            Assert.IsNotNull( quackCommand );
            quackCommand.Execute( null );
            Assert.IsTrue( _viewModel.QuackCalled );
        }
    }
}
