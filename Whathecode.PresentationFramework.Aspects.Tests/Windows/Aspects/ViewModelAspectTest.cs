using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.ComponentModel.NotifyPropertyFactory.Attributes;
using Whathecode.System.Windows.Aspects.ViewModel;
using Whathecode.System.Windows.Input.CommandFactory.Attributes;
using Whathecode.System.Windows.Aspects;


namespace Whathecode.Tests.System.Windows.Aspects
{
    [TestClass]
    public class ViewModelAspectTest
    {
        #region Common test members

        [ViewModel( typeof( Properties ), typeof( Commands ) )]
        class DuckViewModel
        {
            enum Properties
            {
                Color,
                CanQuack
            }

            enum Commands
            {
                Quack
            }


            [NotifyProperty( Properties.Color )]
            public string Color { get; set; }

            [NotifyProperty( Properties.CanQuack )]
            public bool CanQuack { get; set; }

            [CommandExecute( Commands.Quack )]
            public void Quack()
            {                
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
    }
}
