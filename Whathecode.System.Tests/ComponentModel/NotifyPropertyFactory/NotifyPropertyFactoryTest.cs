using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Whathecode.System.ComponentModel.NotifyPropertyFactory;
using Whathecode.System.ComponentModel.NotifyPropertyFactory.Attributes;


namespace Whathecode.Tests.System.ComponentModel.NotifyPropertyFactory
{
	[TestClass]
	public class NotifyPropertyFactoryTest
	{
		#region Common test members

		class Test : INotifyPropertyChanged
		{
			enum Properties
			{
				Normal
			}


			readonly NotifyPropertyFactory<Properties> _propertyFactory;
			public event PropertyChangedEventHandler PropertyChanged;


			[NotifyProperty( Properties.Normal )]
			public int Normal
			{
				get { return (int)_propertyFactory.GetValue( Properties.Normal ); }
				set { _propertyFactory.SetValue( Properties.Normal, value ); }
			}


			public Test()
			{
				_propertyFactory = new NotifyPropertyFactory<Properties>( this, () => PropertyChanged );
			}
		}


		Test _model;
		string _changedPropertyName;


		[TestInitialize]
		public void Initialize()
		{
			_model = new Test();
			_model.PropertyChanged += PropertyChanged;
			_changedPropertyName = null;
		}

		void PropertyChanged( object sender, PropertyChangedEventArgs e )
		{
			_changedPropertyName = e.PropertyName;
		}

		#endregion // Common test members


		[TestMethod]
		public void GetterSetterTest()
		{
			Assert.AreEqual( 0, _model.Normal );
			_model.Normal = 10;
			Assert.AreEqual( 10, _model.Normal );
		}

		[TestMethod]
		public void NotifyTest()
		{
			_model.Normal = 10;
			Assert.AreEqual( "Normal", _changedPropertyName );
		}

		[TestMethod]
		public void NotifyUnchangedTest()
		{
			_model.Normal = 0; // Since 0 is the default value, this shouldn't change the value.
			Assert.AreEqual( null, _changedPropertyName );
		}
	}
}