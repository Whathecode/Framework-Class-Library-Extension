using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Whathecode.System.Arithmetic.Range;
using Whathecode.System.Extensions;
using Whathecode.System.Windows.DependencyPropertyFactory;
using Whathecode.System.Windows.DependencyPropertyFactory.Attributes;


namespace Whathecode.System.Xaml.Behaviors
{
	/// <summary>
	///   Properties which can be attached to a <see cref = "FrameworkElement">FrameworkElement</see> to detect mouse behavior.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class MouseBehavior
	{
		public struct MouseState
		{
			public MousePosition Position;

			public bool IsLeftButtonDown;
			public bool IsRightButtonDown;
		}


		public struct MousePosition
		{
			/// <summary>
			///   Specifies the relative mouse position to the element.
			/// </summary>
			public Point Relative;

			/// <summary>
			///   Percentage inside the element.
			/// </summary>
			public Point Percentage;
		}


		public struct ClickDragInfo
		{
			public MouseState Mouse;
			public ClickDragState State;
			public MousePosition StartPosition;
		}


		public enum ClickDragState
		{
			Start,
			Moving,
			Stop
		}


		[Flags]
		enum Properties
		{
			LeftMouseUpCommand,
			RightMouseUpCommand,
			LeftMouseDownCommand,
			RightMouseDownCommand,
			LeftClickCommand,
			RightClickCommand,
			MovedCommand,
			ClickDragContainer,
			LeftClickDragCommand,
			RightClickDragCommand
		}


		static readonly DependencyPropertyFactory<Properties> DependencyProperties = new DependencyPropertyFactory<Properties>();

		public static DependencyProperty LeftMouseUpCommand = DependencyProperties[ Properties.LeftMouseUpCommand ];
		public static DependencyProperty RightMouseUpCommand = DependencyProperties[ Properties.RightMouseUpCommand ];
		public static DependencyProperty LeftMouseDownCommand = DependencyProperties[ Properties.LeftMouseDownCommand ];
		public static DependencyProperty RightMouseDownCommand = DependencyProperties[ Properties.RightMouseDownCommand ];
		public static DependencyProperty LefClickCommand = DependencyProperties[ Properties.LeftClickCommand ];
		public static DependencyProperty RightClickCommand = DependencyProperties[ Properties.RightClickCommand ];
		public static DependencyProperty MovedCommandProperty = DependencyProperties[ Properties.MovedCommand ];
		public static DependencyProperty ClickDragContainer = DependencyProperties[ Properties.ClickDragContainer ];
		public static DependencyProperty LeftClickDragCommand = DependencyProperties[ Properties.LeftClickDragCommand ];
		public static DependencyProperty RightClickDragCommand = DependencyProperties[ Properties.RightClickDragCommand ];

		static readonly Dictionary<object, ClickDragInfo> LeftClickInfo = new Dictionary<object,ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> RightClickInfo = new Dictionary<object, ClickDragInfo>();
		static readonly Dictionary<object, FrameworkElement> ClickDragContainers = new Dictionary<object, FrameworkElement>(); 
		static readonly Dictionary<object, ClickDragInfo> LeftClickDragInfo = new Dictionary<object, ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> RightClickDragInfo = new Dictionary<object, ClickDragInfo>();


		#region LeftMouseUp Command

		[DependencyProperty( Properties.LeftMouseUpCommand )]
		public static ICommand GetLeftMouseUpCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.LeftMouseUpCommand ) as ICommand;
		}

		[DependencyProperty( Properties.LeftMouseUpCommand )]
		public static void SetLeftMouseUpCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.LeftMouseUpCommand, value );
		}

		[DependencyPropertyChanged( Properties.LeftMouseUpCommand )]
		static void OnLeftMouseUpCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseLeft( element );
			}
		}

		#endregion // LeftMouseUp Command


		#region RightMouseUp Command

		[DependencyProperty( Properties.RightMouseUpCommand )]
		public static ICommand GetRightMouseUpCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.RightMouseUpCommand ) as ICommand;
		}

		[DependencyProperty( Properties.RightMouseUpCommand )]
		public static void SetRightMouseUpCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.RightMouseUpCommand, value );
		}

		[DependencyPropertyChanged( Properties.RightMouseUpCommand )]
		static void OnRightMouseUpCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseRight( element );
			}
		}

		#endregion // RightMouseUp Command


		#region LeftMouseDown Command

		[DependencyProperty( Properties.LeftMouseDownCommand )]
		public static ICommand GetLeftMouseDownCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.LeftMouseDownCommand ) as ICommand;
		}

		[DependencyProperty( Properties.LeftMouseDownCommand )]
		public static void SetLeftMouseDownCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.LeftMouseDownCommand, value );
		}

		[DependencyPropertyChanged( Properties.LeftMouseDownCommand )]
		static void OnLeftMouseDownCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseLeft( element );
			}
		}

		#endregion // LeftMouseDown Command


		#region RightMouseUp Command

		[DependencyProperty( Properties.RightMouseDownCommand )]
		public static ICommand GetRightMouseDownCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.RightMouseDownCommand ) as ICommand;
		}

		[DependencyProperty( Properties.RightMouseDownCommand )]
		public static void SetRightMouseDownCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.RightMouseDownCommand, value );
		}

		[DependencyPropertyChanged( Properties.RightMouseDownCommand )]
		static void OnRightMouseDownCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseRight( element );
			}
		}

		#endregion // RightMouseDown Command


		#region LeftClick Command

		[DependencyProperty( Properties.LeftClickCommand )]
		public static ICommand GetLeftClickCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.LeftClickCommand ) as ICommand;
		}

		[DependencyProperty( Properties.LeftClickCommand )]
		public static void SetLeftClickCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.LeftClickCommand, value );
		}

		[DependencyPropertyChanged( Properties.LeftClickCommand )]
		static void OnLeftClickCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseLeft( element );
			}
		}

		#endregion // LeftClick Command


		#region RightClick Command

		[DependencyProperty( Properties.RightClickCommand )]
		public static ICommand GetRightClickCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.RightClickCommand ) as ICommand;
		}

		[DependencyProperty( Properties.RightClickCommand )]
		public static void SetRightClickCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.RightClickCommand, value );
		}

		[DependencyPropertyChanged( Properties.RightClickCommand )]
		static void OnRightClickCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseRight( element );
			}
		}

		#endregion // RightClick Command


		#region Moved Command

		[DependencyProperty( Properties.MovedCommand )]
		public static ICommand GetMovedCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.MovedCommand ) as ICommand;
		}

		[DependencyProperty( Properties.MovedCommand )]
		public static void SetMovedCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.MovedCommand, value );
		}

		[DependencyPropertyChanged( Properties.MovedCommand )]
		static void OnMovedCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseMoved( element );
			}
		}

		#endregion  // Moved Command


		#region ClickDragContainer

		[DependencyProperty( Properties.ClickDragContainer )]
		public static FrameworkElement GetClickDragContainer( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.ClickDragContainer ) as FrameworkElement;
		}

		[DependencyProperty( Properties.ClickDragContainer )]
		public static void SetClickDragContainer( FrameworkElement target, FrameworkElement value )
		{
			DependencyProperties.SetValue( target, Properties.ClickDragContainer, value );
		}

		[DependencyPropertyChanged( Properties.ClickDragContainer )]
		static void OnClickDragContainerChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as FrameworkElement;
			var container = e.NewValue as FrameworkElement;
			ClickDragContainers.Update( d, container );

			if ( element == null || container == null )
			{
				return;
			}			

			// Rehook current hooked events to the new container.
			// TODO: For this to work the logic for dragging should be separated from the other behaviors.
			/*if ( DependencyProperties.GetValue( element, Properties.LeftClickDragCommand ) != null )
			{
				HookMouseLeft( element, container );
			}
			if ( DependencyProperties.GetValue( element, Properties.RightClickDragCommand ) != null )
			{
				HookMouseRight( element, container );
			}*/
		}

		#endregion	// ClickDragContainer


		#region LeftClickDrag Command

		[DependencyProperty( Properties.LeftClickDragCommand )]
		public static ICommand GetLeftClickDragCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.LeftClickDragCommand ) as ICommand;
		}

		[DependencyProperty( Properties.LeftClickDragCommand )]
		public static void SetLeftClickDragCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.LeftClickDragCommand, value );
		}

		[DependencyPropertyChanged( Properties.LeftClickDragCommand )]
		static void OnLeftClickDragCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseMoved( element );
				HookMouseLeft( element );
			}
		}

		#endregion  // LeftClickDrag Command


		#region RightClickDrag Command

		[DependencyProperty( Properties.RightClickDragCommand )]
		public static ICommand GetRightClickDragCommand( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.RightClickDragCommand ) as ICommand;
		}

		[DependencyProperty( Properties.RightClickDragCommand )]
		public static void SetRightClickDragCommand( FrameworkElement target, ICommand value )
		{
			DependencyProperties.SetValue( target, Properties.RightClickDragCommand, value );
		}

		[DependencyPropertyChanged( Properties.RightClickDragCommand )]
		static void OnRightClickDragCommandChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			var element = d as IInputElement;

			if ( element != null )
			{
				HookMouseMoved( element );
				HookMouseRight( element );
			}
		}

		#endregion  // RightClickDrag Command


		static void HookMouseMoved( IInputElement element )
		{
			// Make sure to only hook event once.
			element.MouseMove -= OnMouseMoved;
			element.MouseMove += OnMouseMoved;
		}

		static void HookMouseLeft( IInputElement unhook, IInputElement hook = null )
		{
			if ( hook == null )
			{
				hook = unhook;
			}

			// Make sure to only hook event once.
			unhook.MouseLeftButtonDown -= MouseButtonDown;
			hook.MouseLeftButtonDown += MouseButtonDown;
			unhook.MouseLeftButtonUp -= MouseButtonUp;			
			hook.MouseLeftButtonUp += MouseButtonUp;
		}

		static void HookMouseRight( IInputElement unhook, IInputElement hook = null )
		{
			if ( hook == null )
			{
				hook = unhook;
			}

			// Make sure to only hook event once.
			unhook.MouseRightButtonDown -= MouseButtonDown;
			hook.MouseRightButtonDown += MouseButtonDown;
			unhook.MouseRightButtonUp -= MouseButtonUp;
			hook.MouseRightButtonUp += MouseButtonUp;
		}

		static void OnMouseMoved( object sender, MouseEventArgs e )
		{
			var element = (FrameworkElement)sender;

			MouseState mouseState = GetMouseState( e, element );

			// Trigger MovedCommand.
			ICommand movedCommand = GetMovedCommand( element );
			if ( movedCommand != null )
			{
				movedCommand.Execute( mouseState );
			}

			// Trigger click drag commands.
			Action<ClickDragInfo, ICommand> executeCommand = ( info, command ) =>
			{
				if ( command != null && info.State != ClickDragState.Stop )
				{
					info.Mouse = mouseState;
					info.State = ClickDragState.Moving;
					command.Execute( info );
				}
			};
			if ( mouseState.IsLeftButtonDown )
			{
				LeftClickDragInfo.TryUseValue( sender, info => executeCommand( info, GetLeftClickDragCommand( element ) ) );
			}
			if ( mouseState.IsRightButtonDown )
			{
				RightClickDragInfo.TryUseValue( sender, info => executeCommand( info, GetRightClickDragCommand( element ) ) );
			}
		}

		static MouseState GetMouseState( MouseEventArgs e, FrameworkElement element )
		{
			Point position = e.GetPosition( element );
			Point percentage = new Point(
				new Interval<double>( 0, element.ActualWidth ).GetPercentageFor( position.X ),
				new Interval<double>( 0, element.ActualHeight ).GetPercentageFor( position.Y )
				);
			return new MouseState
			{
				Position = new MousePosition
				{
					Relative = position,
					Percentage = percentage
				},
				IsLeftButtonDown = e.LeftButton == MouseButtonState.Pressed,
				IsRightButtonDown = e.RightButton == MouseButtonState.Pressed
			};
		}

		static void MouseButtonDown( object sender, MouseButtonEventArgs e )
		{
			var element = (FrameworkElement)sender;

			MouseState mouseState = GetMouseState( e, element );

			// Trigger down commands.
			ICommand leftDownCommand = GetLeftMouseDownCommand( element );
			if ( leftDownCommand != null && e.ChangedButton == MouseButton.Left )
			{
				leftDownCommand.Execute( mouseState );
			}
			ICommand rightDownCommand = GetRightMouseDownCommand( element );
			if ( rightDownCommand != null && e.ChangedButton == MouseButton.Right )
			{
				rightDownCommand.Execute( mouseState );
			}

			// Track click commands.
			ICommand leftClickCommand = e.ChangedButton == MouseButton.Left
				? GetLeftClickCommand( element )
				: GetRightClickCommand( element );
			Dictionary<object, ClickDragInfo> clickInfo = e.ChangedButton == MouseButton.Left
				? LeftClickInfo
				: RightClickInfo;
			if ( leftClickCommand != null && e.ChangedButton == MouseButton.Left )
			{
				var info = new ClickDragInfo
				{
					Mouse = mouseState,
					State = ClickDragState.Start,
					StartPosition = mouseState.Position
				};
				
				if ( !clickInfo.ContainsKey( sender ) )
				{
					clickInfo.Add( sender, info );
				}
			}

			// Trigger click drag commands.
			ICommand clickDragCommand = e.ChangedButton == MouseButton.Left
				? GetLeftClickDragCommand( element )
				: GetRightClickDragCommand( element );
			Dictionary<object, ClickDragInfo> dragInfo = e.ChangedButton == MouseButton.Left
				? LeftClickDragInfo
				: RightClickDragInfo;
			if ( clickDragCommand != null )
			{
				var info = new ClickDragInfo
				{
					Mouse = mouseState,
					State = ClickDragState.Start,
					StartPosition = mouseState.Position
				};
				clickDragCommand.Execute( info );

				if ( !dragInfo.ContainsKey( sender ) )
				{
					dragInfo.Add( sender, info );
				}
			}
		}

		static void MouseButtonUp( object sender, MouseButtonEventArgs e )
		{
			var element = (FrameworkElement)sender;

			MouseState mouseState = GetMouseState( e, element );

			// Trigger up commands.
			ICommand leftUpCommand = GetLeftMouseUpCommand( element );
			if ( leftUpCommand != null && e.ChangedButton == MouseButton.Left )
			{
				leftUpCommand.Execute( mouseState );
			}
			ICommand rightUpCommand = GetRightMouseUpCommand( element );
			if ( rightUpCommand != null && e.ChangedButton == MouseButton.Right )
			{
				rightUpCommand.Execute( mouseState );
			}

			// Trigger click commands.
			ClickDragInfo clickInfo;
			ICommand leftClickCommand = GetLeftClickCommand( element );
			if ( leftClickCommand != null && e.ChangedButton == MouseButton.Left && LeftClickInfo.TryGetValue( sender, out clickInfo ) )
			{
				leftClickCommand.Execute( mouseState );				
			}
			LeftClickInfo.Clear();
			ICommand rightClickCommand = GetRightClickCommand( element );
			if ( rightClickCommand != null && e.ChangedButton == MouseButton.Right && RightClickInfo.TryGetValue( sender, out clickInfo ) )
			{
				rightClickCommand.Execute( mouseState );				
			}
			RightClickInfo.Clear();

			// Trigger click drag commands.
			ICommand leftClickDragCommand = GetLeftClickDragCommand( element );
			ClickDragInfo leftInfo;
			if ( leftClickDragCommand != null && LeftClickDragInfo.TryGetValue( sender, out leftInfo ) )
			{
				if ( mouseState.IsLeftButtonDown )
				{
					leftInfo.Mouse = mouseState;
					leftInfo.State = ClickDragState.Stop;
					leftClickDragCommand.Execute( leftInfo );
				}

				LeftClickDragInfo.Remove( sender );
			}
			ICommand rightClickDragCommand = GetRightClickDragCommand( element );
			ClickDragInfo rightInfo;
			if ( rightClickDragCommand != null && RightClickDragInfo.TryGetValue( sender, out rightInfo ) )
			{
				if ( mouseState.IsRightButtonDown )
				{
					rightInfo.Mouse = mouseState;
					rightInfo.State = ClickDragState.Stop;
					rightClickDragCommand.Execute( rightInfo );
				}

				RightClickDragInfo.Remove( sender );
			}
		}
	}
}