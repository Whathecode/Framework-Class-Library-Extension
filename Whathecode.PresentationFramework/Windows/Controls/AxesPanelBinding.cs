using System;


namespace Whathecode.System.Windows.Controls
{
	[Flags]
	public enum AxesPanelBinding
	{
		MaximaX = 1,
		MaximaY = 1 << 1,
		MinimumSizeX = 1 << 2,
		MinimumSizeY = 1 << 3,
		VisibleIntervalX = 1 << 4,
		VisibleIntervalY = 1 << 5,
		LabelFactories = 1 << 6
	}
}
