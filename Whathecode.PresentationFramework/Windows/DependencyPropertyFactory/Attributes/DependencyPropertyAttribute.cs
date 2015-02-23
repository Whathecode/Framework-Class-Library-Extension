using System;
using System.ComponentModel;


namespace Whathecode.System.Windows.DependencyPropertyFactory.Attributes
{
	/// <summary>
	///   When applied to the property of a type,
	///   specifies how the dependency property should be created by the <see cref = "DependencyPropertyFactory" />.
	/// </summary>
	/// <author>Steven Jeuris</author>
	[AttributeUsage( AttributeTargets.Property | AttributeTargets.Method )]
	public class DependencyPropertyAttribute : AbstractDependencyPropertyAttribute
	{
		readonly bool _readOnly;
		readonly bool _readOnlySet;

		/// <summary>
		///   The name to use for the dependency property.
		///   By default this is the name of the property to which the attribute is applied.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///   The default value for the dependency property.
		///   Should be of the same type as the property, or be convertable from a string using a <see cref="TypeConverter" />.
		/// </summary>
		public object DefaultValue { get; set; }

		/// <summary>
		///   The type of a <see cref="IDefaultValueProvider{TProperty}" /> which can provide default values for passed types.
		/// </summary>
		public Type DefaultValueProvider { get; set; }

		/// <summary>
		///   When set to true, the first time the value is set,
		///   a changed callback is triggered, whether it's the default value or not.
		/// </summary>
		public bool ForceChangedCallbackFirstTime { get; set; }

		/// <summary>
		///   Gets or sets a value that indicates whether a dependency property potentially affects the arrange pass during layout engine operations.
		///   true if the dependency property on which this metadata exists potentially affects the arrange pass; otherwise, false. The default is false.
		/// </summary>
		public bool AffectsArrange { get; set; }

		/// <summary>
		///   Gets or sets a value that indicates whether a dependency property potentially affects the measure pass during layout engine operations.
		///   true if the dependency property on which this metadata exists potentially affects the measure pass; otherwise, false. The default is false.
		/// </summary>
		public bool AffectsMeasure { get; set; }

		/// <summary>
		///   Gets or sets a value that indicates whether a dependency property potentially affects the arrange pass of its parent element's layout during layout engine operations.
		///   true if the dependency property on which this metadata exists potentially affects the arrange pass specifically on its parent element; otherwise, false. The default is false.
		/// </summary>
		public bool AffectsParentArrange { get; set; }

		/// <summary>
		///   Gets or sets a value that indicates whether a dependency property potentially affects the measure pass of its parent element's layout during layout engine operations.
		///   true if the dependency property on which this metadata exists potentially affects the measure pass specifically on its parent element; otherwise, false.The default is false. 
		/// </summary>
		public bool AffectsParentMeasure { get; set; }

		/// <summary>
		///   Gets or sets a value that indicates whether a dependency property potentially affects the general layout in some way that does not specifically influence arrangement or measurement, but would require a redraw.
		///   true if the dependency property on which this metadata exists affects rendering; otherwise, false. The default is false.
		/// </summary>
		public bool AffectsRender { get; set; }

		/// <summary>
		///   Gets or sets a value that indicates whether sub-properties of the dependency property do not affect the rendering of the containing object.
		///   true if changes to sub-property values do not affect rendering if changed; otherwise, false. The default is false.
		/// </summary>
		public bool SubPropertiesDoNotAffectRender { get; set; }


		/// <summary>
		///   Create a new attribute which gives information about the dependency property to create for a given property.
		/// </summary>
		/// <param name = "id">The ID of the dependency property.</param>
		public DependencyPropertyAttribute( object id )
			: base( id )
		{
			_readOnlySet = false;
		}

		public DependencyPropertyAttribute( object id, bool readOnly )
			: base( id )
		{
			_readOnly = readOnly;
			_readOnlySet = true;
		}


		/// <summary>
		///   Should the dependency property be a readonly dependency property or not.
		/// </summary>
		/// <returns></returns>
		public bool IsReadOnly()
		{
			return _readOnly;
		}

		/// <summary>
		///   Has the readonly value been set?
		/// </summary>
		/// <returns></returns>
		public bool IsReadOnlySet()
		{
			return _readOnlySet;
		}
	}
}