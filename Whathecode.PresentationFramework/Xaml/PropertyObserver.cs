using System.Windows;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.System.Xaml
{
    /// <summary>
    ///   Defines an observer of a specific dependency property.
    ///   Should be contained within an instance of <see cref="PropertyObservers">PropertyObservers</see>.
    /// </summary>
    /// <author>Steven Jeuris</author>
    public class PropertyObserver : FrameworkElement
    {
        private enum Properties
        {
            Path,
            Observer
        }


        static readonly DependencyPropertyFactory<Properties> PropertyFactory = new DependencyPropertyFactory<Properties>();

        public static readonly DependencyProperty PathProperty = PropertyFactory[ Properties.Path ];
        public static readonly DependencyProperty ObserverProperty = PropertyFactory[ Properties.Observer ];


        [DependencyProperty( Properties.Path )]
        public PropertyPath Path
        {
            get { return PropertyFactory.GetValue( this, Properties.Path ) as PropertyPath; }
            set { PropertyFactory.SetValue( this, Properties.Path, value ); }
        }

        [DependencyProperty( Properties.Observer )]
        public object Observer
        {
            get { return PropertyFactory.GetValue( this, Properties.Observer ); }
            set { PropertyFactory.SetValue( this, Properties.Observer, value ); }
        }
    }
}
