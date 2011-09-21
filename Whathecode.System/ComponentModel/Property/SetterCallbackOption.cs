namespace Whathecode.System.ComponentModel.Property
{
	/// <summary>
	///   Option which can be passed to the setter of the Property class.
	/// </summary>
	public enum SetterCallbackOption
	{
		/// <summary>
		///   The PropertyChanged callback is called every time set is called, regardless of the old and new value.
		/// </summary>
		Always,
		/// <summary>
		///   The PropertyChanged callback is only called when the new value is different from the old value.
		/// </summary>
		OnNewValue,
		/// <summary>
		///   The PropertyChanged callback is only called when the new value is different from the old value,
		///   but also the first time the setter is called, regardless of the old and new value.
		/// </summary>
		OnNewValueAndFirst,
		/// <summary>
		///   The PropertyChanged callback is only called when the new value equals the old value.
		/// </summary>
		OnSameValue
	}
}