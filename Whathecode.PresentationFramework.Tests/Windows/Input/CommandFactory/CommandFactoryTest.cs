using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.Windows.Input.CommandFactory;
using Whathecode.System.Windows.Input.CommandFactory.Attributes;
using Whathecode.Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Whathecode.Tests.System.Windows.Input.CommandFactory
{
    [TestClass]
    public class CommandFactoryTest
    {
        #region Common test members

        class ViewModel
        {
            public enum Command
            {
                NoParameter,
                Parameter
            }


            readonly CommandFactory<Command> _commands;
            public Dictionary<Command, ICommand> Commands
            {
                get;
                private set;
            }


            public ViewModel()
            {
                _commands = new CommandFactory<Command>( this );
                Commands = _commands.Commands;
            }


            public bool NoParametersCalled { get; private set; }
            [CommandExecute( Command.NoParameter )]
            public void NoParameters()
            {
                NoParametersCalled = true;
            }

            public int SetParameter { get; private set; }
            [CommandExecute( Command.Parameter )]
            public void Parameter( int parameter )
            {
                SetParameter = parameter;
            }
        }


        ViewModel _model;


        [TestInitialize]
        public void Initialize()
        {
            _model = new ViewModel();
        }

        #endregion // Common test members


        [TestMethod]
        public void TriggerCommandTest()
        {
            // Command without parameter.
            ICommand noParameters = _model.Commands[ ViewModel.Command.NoParameter ];
            noParameters.Execute( null );
            Assert.IsTrue( _model.NoParametersCalled );

            // Command with parameter.
            ICommand parameter = _model.Commands[ ViewModel.Command.Parameter ];
            parameter.Execute( 42 );
            Assert.AreEqual( 42, _model.SetParameter );
            AssertHelper.ThrowsException<InvalidOperationException>( () => parameter.Execute( null ) ); 
        }
    }
}
