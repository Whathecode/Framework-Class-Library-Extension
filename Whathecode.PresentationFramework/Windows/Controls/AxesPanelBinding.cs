using System;


namespace Whathecode.System.Windows.Controls
{
	[Flags]
	public enum AxesPanelBinding
	{
		MaximaX = 1,
		MaximaY = 1 << 1,
		MinimumSizeX = 1 << 2,
		MaximumSizeX = 1 << 3,
		MinimumSizeY = 1 << 4,
		MaximumSizeY = 1 << 5,
		VisibleIntervalX = 1 << 6,
		VisibleIntervalY = 1 << 7,
		LabelFactories = 1 << 8
	}
}
