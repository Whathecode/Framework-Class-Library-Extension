using System;
using System.Runtime.InteropServices;


namespace Whathecode.Interop
{
	/// <summary>
	///   Contains User32.dll definitions relating to list-view controls.
	/// </summary>
	public static partial class User32
	{
		#region Types

		const int FirstListViewMessage = 0x1000;

		/// <summary>
		///   The different messages which can be sent to list-view controls.
		///   For an overview of return values, and required lParam and wParam see:
		///   https://msdn.microsoft.com/en-us/library/windows/desktop/ff485961(v=vs.85).aspx
		/// </summary>
		/// <remarks>
		///   ANSI formatted messages have been removed from this enumeration, and all messages expect Unicode where appropriate.
		/// </remarks>
		public enum ListViewMessage : uint
		{
			/// <summary>
			///   Gets the background color of a list-view control.
			/// </summary>
			GetBackgroundColor = FirstListViewMessage,
			/// <summary>
			///   Sets the background color of a list-view control.
			/// </summary>
			SetBackgroundColor = FirstListViewMessage + 1,
			/// <summary>
			///   Retrieves the handle to an image list used for drawing list-view items.
			/// </summary>
			GetImageList = FirstListViewMessage + 2,
			/// <summary>
			///   Assigns an image list to a list-view control.
			/// </summary>
			SetImageList = FirstListViewMessage + 3,
			/// <summary>
			///   Retrieves the number of items in a list-view control.
			///   wParam and lParam must be zero.
			/// </summary>
			GetItemCount = FirstListViewMessage + 4,
			/// <summary>
			///   Retrieves some or all of a list-view item's attributes, using Unicode for text.
			/// </summary>
			GetItem = FirstListViewMessage + 75,
			/// <summary>
			///   Sets some or all of a list-view item's attributes, expecting Unicode for text.
			/// </summary>
			SetItem = FirstListViewMessage + 76,
			/// <summary>
			///   Inserts a new item in a list-view control, expecting Unicode for text.
			/// </summary>
			InsertItem = FirstListViewMessage + 77,
			/// <summary>
			///   Removes an item from a list-view control.
			/// </summary>
			DeleteItem = FirstListViewMessage + 8,
			/// <summary>
			///   Removes all items from a list-view control.
			/// </summary>
			DeleteAlItems = FirstListViewMessage + 9,
			/// <summary>
			///   Gets the callback mask for a list-view control.
			/// </summary>
			GetCallbackMask = FirstListViewMessage + 10,
			/// <summary>
			///   Changes the callback mask for a list-view control.
			/// </summary>
			SetCallbackMast = FirstListViewMessage + 11,
			/// <summary>
			///   This message searches for a list-view item that has the specified properties and bears the specified relationship to a specified item.
			/// </summary>
			GetNextItem = FirstListViewMessage + 12,
			/// <summary>
			///   This message searches for a list-view item with the specified characteristics. String within the find structure should be Unicode.
			/// </summary>
			FindItem = FirstListViewMessage + 83,
			/// <summary>
			///   This message retrieves the bounding rectangle for all or part of an item in the current view.
			/// </summary>
			GetItemRectangle = FirstListViewMessage + 14,
			/// <summary>
			///   This message moves an item to a specified position in a list-view control, which must be in icon or small icon view.
			/// </summary>
			SetItemPosition = FirstListViewMessage + 15,
			/// <summary>
			///   This message retrieves the position of a list-view item.
			///   wParam: Index of the list-view item.
			///   lParam: Pointer to a <see cref="Point" /> structure that receives the position of the item's upper-left corner, in view coordinates.
			///   returns: Returns TRUE if successful, or FALSE otherwise.
			/// </summary>
			GetItemPosition = FirstListViewMessage + 16,
			/// <summary>
			///   This message determines the width of a specified Unicode string, using the specified list-view control's current font.
			/// </summary>
			GetStringWidth = FirstListViewMessage + 87,
			/// <summary>
			///   This message determines which list-view item, if any, is at a specified position.
			/// </summary>
			HitTest = FirstListViewMessage + 18,
			/// <summary>
			///   This message ensures that a list-view item is entirely or at least partially visible, scrolling the list-view control if necessary.
			/// </summary>
			EnsureVisible = FirstListViewMessage + 19,
			/// <summary>
			///   This message scrolls the content of a list-view control.
			/// </summary>
			Scroll = FirstListViewMessage + 20,
			/// <summary>
			///   This message forces a list-view control to redraw a range of items.
			/// </summary>
			RedrawItems = FirstListViewMessage + 21,
			/// <summary>
			///   This message arranges items in icon view.
			/// </summary>
			Arrange = FirstListViewMessage + 22,
			/// <summary>
			///   This message begins in-place editing of the specified list-view item's text.
			/// </summary>
			EditLabel = FirstListViewMessage + 118,
			/// <summary>
			///   This message retrieves the handle of the edit control being used to edit a list-view item's text.
			/// </summary>
			GetEditControl = FirstListViewMessage + 24,
			/// <summary>
			///   This message retrieves the attributes of a list-view control column. Column header text is retrieved as Unicode.
			/// </summary>
			GetColumnAttributes = FirstListViewMessage + 95,
			/// <summary>
			///   This message sets the attributes of a list-view column. Column header text should be Unicode.
			/// </summary>
			SetColumnAttributes = FirstListViewMessage + 96,
			/// <summary>
			///   This message inserts a new column in a list-view control. Column header text should be Unicode.
			/// </summary>
			InsertColumn = FirstListViewMessage + 97,
			/// <summary>
			///   This message removes a column from a list-view control.
			/// </summary>
			DeleteColumn = FirstListViewMessage + 28,
			/// <summary>
			///   This message retrieves the width of a column in report or list view.
			/// </summary>
			GetColumnWidth = FirstListViewMessage + 29,
			/// <summary>
			///   This message changes the width of a column in report or list view.
			/// </summary>
			SetColumnWidth = FirstListViewMessage + 30,
			/// <summary>
			///   Gets the handle to the header control used by the list-view control.
			/// </summary>
			GetHeaderControl = FirstListViewMessage + 31,
			/// <summary>
			///   This message creates a drag image list for the specified item.
			/// </summary>
			CreateDragImage = FirstListViewMessage + 33,
			/// <summary>
			///   This message retrieves the bounding rectangle of all items in the list-view control. The list view must be in icon or small icon view.
			/// </summary>
			GetViewRectangle = FirstListViewMessage + 34,
			/// <summary>
			///   This message retrieves the text color of a list-view control.
			/// </summary>
			GetTextColor = FirstListViewMessage + 35,
			/// <summary>
			///   This message sets the text color of a list-view control.
			/// </summary>
			SetTextColor = FirstListViewMessage + 36,
			/// <summary>
			///   This message retrieves the text background color of a list-view control.
			/// </summary>
			GetTextBackgroundColor = FirstListViewMessage + 37,
			/// <summary>
			///   This message sets the background color of text in a list-view control.
			/// </summary>
			SetTextBackgroundColor = FirstListViewMessage + 38,
			/// <summary>
			///   This message retrieves the index of the topmost visible item when in list or report view.
			/// </summary>
			GetTopmostVisibleIndex = FirstListViewMessage + 39,
			/// <summary>
			///   This message calculates the number of items that can fit vertically in the visible area of a list-view control when in list or report view.
			///   Only fully visible items are counted.
			/// </summary>
			GetCounterPage = FirstListViewMessage + 40,
			/// <summary>
			///   This message retrieves the current view origin for a list-view control.
			/// </summary>
			GetViewOrigin = FirstListViewMessage + 41,
			/// <summary>
			///   This message updates a list-view item. If the list-view control has the LVS_AUTOARRANGE style, this causes the list-view control to be arranged.
			/// </summary>
			Update = FirstListViewMessage + 42,
			/// <summary>
			///   This message changes the state of an item in a list-view control.
			/// </summary>
			SetItemState = FirstListViewMessage + 43,
			/// <summary>
			///   This message retrieves the state of a list-view item.
			/// </summary>
			GetItemState = FirstListViewMessage + 44,
			/// <summary>
			///   This message retrieves the text in Unicode of a list-view item or subitem.
			/// </summary>
			GetItemText = FirstListViewMessage + 115,
			/// <summary>
			///   This message changes the text of a list-view item or subitem, expecting Unicode.
			/// </summary>
			SetItemText = FirstListViewMessage + 116,
			/// <summary>
			///   This message prepares a list-view control for adding a large number of items.
			///   In a regular list-view control, it causes the control to allocate memory for the specified number of items.
			///   In a virtual list-view control, available in Windows CE 2.0 and later, it sets the number of virtual items in the control.
			/// </summary>
			SetItemCount = FirstListViewMessage + 47,
			/// <summary>
			///   This message sorts list-view items using an application-defined comparison function. The index of each item changes to reflect the new sequence.
			/// </summary>
			SortItems = FirstListViewMessage + 48,
			/// <summary>
			///   Moves an item to a specified position in a list-view control (must be in icon or small icon view).
			///   This message differs from the <see cref="SetItemPosition" /> message in that it uses 32-bit coordinates.
			///   wParam: Index of the list-view item for which to set the position.
			///   lParam: Pointer to a <see cref="Point" /> structure that contains the new position of the item, in list-view coordinates.
			///   returns: No return value.
			/// </summary>
			SetItemPosition32 = FirstListViewMessage + 49,
			/// <summary>
			///   Determines the number of selected items in a list-view control.
			/// </summary>
			GetSelectedCount = FirstListViewMessage + 50,
			/// <summary>
			///   Determines the spacing between items in a list-view control.
			/// </summary>
			GetItemsSpacing = FirstListViewMessage + 51,
			/// <summary>
			///   Retrieves the incremental search string of a list-view control, in Unicode.
			/// </summary>
			GetIncrementalSearchString = FirstListViewMessage + 117,
			/// <summary>
			///   Sets the spacing between icons in list-view controls that have the LVS_ICON style.
			/// </summary>
			SetIconSpacing = FirstListViewMessage + 53,
			/// <summary>
			///   Sets extended styles in list-view controls. 
			/// </summary>
			SetExtendedListViewStyle = FirstListViewMessage + 54,
			/// <summary>
			///   Gets the extended styles that are currently in use for a given list-view control.
			/// </summary>
			GetExtendedListViewStyle = FirstListViewMessage + 55,
			/// <summary>
			///   Retrieves information about the bounding rectangle for a subitem in a list-view control. 
			///   This message is intended to be used only with list-view controls that use the LVS_REPORT style.
			/// </summary>
			GetSubItemRectangle = FirstListViewMessage + 56,
			/// <summary>
			///   Determines which list-view item or subitem is at a given position.
			/// </summary>
			SubItemHitTest = FirstListViewMessage + 57,
			/// <summary>
			///   Sets the left-to-right order of columns in a list-view control.
			/// </summary>
			SetColumnOrder = FirstListViewMessage + 58,
			/// <summary>
			///   Gets the current left-to-right order of columns in a list-view control. 
			/// </summary>
			GetColumnOrder = FirstListViewMessage + 59,
			/// <summary>
			///   Sets the hot item for a list-view control.
			/// </summary>
			SetHotItem = FirstListViewMessage + 60,
			/// <summary>
			///   Retrieves the index of the hot item.
			/// </summary>
			GetHotItem = FirstListViewMessage + 61,
			/// <summary>
			///   Sets the HCURSOR value that the list-view control uses when the pointer is over an item while hot tracking is enabled.
			///   To check whether hot tracking is enabled, call SystemParametersInfo.
			/// </summary>
			SetHotCursor = FirstListViewMessage + 62,
			/// <summary>
			///   Retrieves the HCURSOR value used when the pointer is over an item while hot tracking is enabled.
			/// </summary>
			GetHotCursor = FirstListViewMessage + 63,
			/// <summary>
			///   Calculates the approximate width and height required to display a given number of items.
			/// </summary>
			ApproximateViewRectangle = FirstListViewMessage + 64,
			/// <summary>
			///   Sets the working areas within a list-view control.
			/// </summary>
			SetWorkAreas = FirstListViewMessage + 65,
			/// <summary>
			///   Retrieves the working areas from a list-view control.
			/// </summary>
			GetWorkAreas = FirstListViewMessage + 70,
			/// <summary>
			///   Retrieves the number of working areas in a list-view control.
			/// </summary>
			GetNumberOfWorkAreas = FirstListViewMessage + 73,
			/// <summary>
			///   Retrieves the selection mark from a list-view control.
			/// </summary>
			GetSelectionMark = FirstListViewMessage + 66,
			/// <summary>
			///   Sets the selection mark in a list-view control.
			/// </summary>
			SetSelectionMark = FirstListViewMessage + 67,
			/// <summary>
			///   Sets the amount of time which the mouse cursor must hover over an item before it is selected.
			/// </summary>
			SetHoverSelectionTime = FirstListViewMessage + 71,
			/// <summary>
			///   Retrieves the amount of time that the mouse cursor must hover over an item before it is selected.
			/// </summary>
			GetHoverSelectionTime = FirstListViewMessage + 72,
			/// <summary>
			///   Sets the tooltip control that the list-view control will use to display tooltips.
			/// </summary>
			SetTooltipControl = FirstListViewMessage + 74,
			/// <summary>
			///   Retrieves the tooltip control that the list-view control uses to display tooltips.
			/// </summary>
			GetTooltipControl = FirstListViewMessage + 78,
			/// <summary>
			///   Uses an application-defined comparison function to sort the items of a list-view control. The index of each item changes to reflect the new sequence.
			///   This message is similar to <see cref="SortItems" />, except for the type of information passed to the comparison function.
			///   With <see cref="SortItemsIndex" />, lParam1 is the current index of the first item, and lParam2 is the current index of the second item.
			/// </summary>
			SortItemsIndex = FirstListViewMessage + 81,
			/// <summary>
			///   Sets the background image in a list-view control, expecting Unicode for the path.
			/// </summary>
			SetBackgroundImage = FirstListViewMessage + 138,
			/// <summary>
			///   Gets the background image in a list-view control, using Unicode to represent the path.
			/// </summary>
			GetBackgroundImage = FirstListViewMessage + 139,
			/// <summary>
			///   Sets the index of the selected column.
			/// </summary>
			SetSelectedColumn = FirstListViewMessage + 140,
			/// <summary>
			///   Sets the view of a list-view control.
			/// </summary>
			SetView = FirstListViewMessage + 142,
			/// <summary>
			///   Retrieves the current view of a list-view control.
			/// </summary>
			GetView = FirstListViewMessage + 143,
			/// <summary>
			///   Inserts a group into a list-view control.
			/// </summary>
			InsertGroup = FirstListViewMessage + 145,
			/// <summary>
			///   Sets group information.
			/// </summary>
			SetGroupInfo = FirstListViewMessage + 147,
			/// <summary>
			///   Gets group information.
			/// </summary>
			GetGroupInfo = FirstListViewMessage + 149,
			/// <summary>
			///   Removes a group from a list-view control.
			/// </summary>
			RemoveGroup = FirstListViewMessage + 150,
			/// <summary>
			///   Gets the number of groups.
			/// </summary>
			GetGroupCount = FirstListViewMessage + 152,
			/// <summary>
			///   Gets information on a specified group.
			/// </summary>
			GetGroupInfoByIndex = FirstListViewMessage + 153,
			/// <summary>
			///   Gets the rectangle for a specified group.
			/// </summary>
			GetGroupRectangle = FirstListViewMessage + 98,
			/// <summary>
			///   Sets information about the display of groups.
			/// </summary>
			SetGroupMetrics = FirstListViewMessage + 155,
			/// <summary>
			///   Gets information about the display of groups.
			/// </summary>
			GetGroupMetrics = FirstListViewMessage + 156,
			/// <summary>
			///   Enables or disables whether the items in a list-view control display as a group.
			/// </summary>
			EnableGroupView = FirstListViewMessage + 157,
			/// <summary>
			///   Uses an application-defined comparison function to sort groups by ID within a list-view control.
			/// </summary>
			SortGroups = FirstListViewMessage + 158,
			/// <summary>
			///   Inserts a group into an ordered list of groups.
			/// </summary>
			InsertGroupsSorted = FirstListViewMessage + 159,
			/// <summary>
			///   Removes all groups from a list-view control.
			/// </summary>
			RemoveAllGroups = FirstListViewMessage + 160,
			/// <summary>
			///   Determines whether the list-view control has a specified group.
			/// </summary>
			HasGroup = FirstListViewMessage + 161,
			/// <summary>
			///   Gets the state for a specified group.
			/// </summary>
			GetGroupState = FirstListViewMessage + 92,
			/// <summary>
			///   Gets the group that has the focus.
			/// </summary>
			GetFocusedGroup = FirstListViewMessage + 93,
			/// <summary>
			///   Sets information that a list-view control uses in tile view.
			/// </summary>
			SetTileViewInfo = FirstListViewMessage + 162,
			/// <summary>
			///   Retrieves information about a list-view control in tile view.
			/// </summary>
			GetTileViewInfo = FirstListViewMessage + 163,
			/// <summary>
			///   Sets information for an existing tile of a list-view control.
			/// </summary>
			SetTileInfo = FirstListViewMessage + 164,
			/// <summary>
			///   Retrieves information about a tile in a list-view control.
			/// </summary>
			GetTileInfo = FirstListViewMessage + 165,
			/// <summary>
			///   Sets the insertion point to the defined position.
			/// </summary>
			SetInsertMarkPosition = FirstListViewMessage + 166,
			/// <summary>
			///   Retrieves the position of the insertion point.
			/// </summary>
			GetInsertMarkPosition = FirstListViewMessage + 167,
			/// <summary>
			///   Retrieves the insertion point closest to a specified point.
			/// </summary>
			InsertMarkHitTest = FirstListViewMessage + 168,
			/// <summary>
			///   Retrieves the rectangle that bounds the insertion point.
			/// </summary>
			GetInsertMarkRectangle = FirstListViewMessage + 169,
			/// <summary>
			///   Sets the color of the insertion point.
			/// </summary>
			SetInsertMarkColor = FirstListViewMessage + 170,
			/// <summary>
			///   Retrieves the color of the insertion point.
			/// </summary>
			GetInsertMarkColor = FirstListViewMessage + 171,
			/// <summary>
			///   Retrieves an integer that specifies the selected column.
			/// </summary>
			GetSelectedColumn = FirstListViewMessage + 174,
			/// <summary>
			///   Checks whether the list-view control has group view enabled.
			/// </summary>
			IsGroupViewEnabled = FirstListViewMessage + 175,
			/// <summary>
			///   Retrieves the color of the border of a list-view control if the LVS_EX_BORDERSELECT extended window style is set.
			/// </summary>
			GetOutlineColor = FirstListViewMessage + 176,
			/// <summary>
			///   Sets the color of the border of a list-view control if the LVS_EX_BORDERSELECT extended window style is set.
			/// </summary>
			SetOutlineColor = FirstListViewMessage + 177,
			/// <summary>
			///   Cancels an item text editing operation.
			/// </summary>
			CancelEditLabel = FirstListViewMessage + 179,
			/// <summary>
			///   Maps the index of an item to a unique ID.
			/// </summary>
			MapIndexToId = FirstListViewMessage + 180,
			/// <summary>
			///   Maps the ID of an item to an index.
			/// </summary>
			MapIdToIndex = FirstListViewMessage + 181,
			/// <summary>
			///   Indicates if an item in the list-view control is visible.
			/// </summary>
			IsItemVisible = FirstListViewMessage + 182,
			/// <summary>
			///   Gets the text meant for display when the list-view control appears empty.
			/// </summary>
			GetEmptyText = FirstListViewMessage + 204,
			/// <summary>
			///   Retrieves the coordinates of the footer for a list-view control.
			/// </summary>
			GetFooterRectangle = FirstListViewMessage + 205,
			/// <summary>
			///   Gets information about the footer of a list-view control.
			/// </summary>
			GetFooterInfo = FirstListViewMessage + 206,
			/// <summary>
			///   Gets the coordinates of a footer for a specified item in a list-view control.
			/// </summary>
			GetFooterItemRectangle = FirstListViewMessage + 207,
			/// <summary>
			///   Gets information on a footer item in a list-view control.
			/// </summary>
			GetFooterItem = FirstListViewMessage + 208,
			/// <summary>
			///   Retrieves the bounding rectangle for all or part of a subitem in the current view of a list-view control.
			/// </summary>
			GetItemIndexRectangle = FirstListViewMessage + 209,
			/// <summary>
			///   Sets the state of a list-view item.
			/// </summary>
			SetItemIndexState = FirstListViewMessage + 210,
			/// <summary>
			///   Retrieves the index of an item in a specified list-view control that matches the specified properties and relationship to another item.
			/// </summary>
			GetNextItemIndex = FirstListViewMessage + 211
		}

		#endregion
	}
}
