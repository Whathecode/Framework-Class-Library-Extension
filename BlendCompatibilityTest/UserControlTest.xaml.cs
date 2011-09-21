using System;
using System.Windows;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace BlendCompatibilityTest
{
	/// <summary>
	///   Interaction logic for UserControlTest.xaml
	/// </summary>
	public partial class UserControlTest
	{
		[Flags]
		public enum Properties
		{
			FactoryProperty
		}


		public static DependencyProperty OrdinaryPropertyProperty = DependencyProperty.Register(
			"OrdinaryProperty",
			typeof( string ),
			typeof( UserControlTest ),
			new PropertyMetadata( "Ordinary property works!" ) );

		public string OrdinaryProperty
		{
			get { return (string)GetValue( OrdinaryPropertyProperty ); }
			set { SetValue( OrdinaryPropertyProperty, value ); }
		}


		static readonly DependencyPropertyFactory<Properties> Factory = new DependencyPropertyFactory<Properties>( false );

		[DependencyProperty( Properties.FactoryProperty, DefaultValue = "Factory property works!" )]
		public string FactoryProperty
		{
			get { return (string)Factory.GetValue( this, Properties.FactoryProperty ); }
			set { Factory.SetValue( this, Properties.FactoryProperty, value ); }
		}


		public UserControlTest()
		{
			InitializeComponent();
		}
	}
}