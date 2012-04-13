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
	///   Properties which can be attached to a <see cref = "FrameworkElement" /> to detect mouse behavior.
	/// </summary>
	/// <author>Steven Jeuris</author>
	public static class MouseBehavior
	{
		/// <summary>
		///   The maximum distance the mouse is allowed to travel for a click to be valid.
		/// </summary>
		const double MaxClickDragDistance = 20;

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
			public Vector Displacement
			{
				get { return Mouse.Position.Relative - StartPosition.Relative; }
			}
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
			LeftClickDragCommand,
			RightClickDragCommand,
			DragCapturesMouse
		}


		static readonly DependencyPropertyFactory<Properties> DependencyProperties = new DependencyPropertyFactory<Properties>();

		public static DependencyProperty LeftMouseUpCommand = DependencyProperties[ Properties.LeftMouseUpCommand ];
		public static DependencyProperty RightMouseUpCommand = DependencyProperties[ Properties.RightMouseUpCommand ];
		public static DependencyProperty LeftMouseDownCommand = DependencyProperties[ Properties.LeftMouseDownCommand ];
		public static DependencyProperty RightMouseDownCommand = DependencyProperties[ Properties.RightMouseDownCommand ];
		public static DependencyProperty LefClickCommand = DependencyProperties[ Properties.LeftClickCommand ];
		public static DependencyProperty RightClickCommand = DependencyProperties[ Properties.RightClickCommand ];
		public static DependencyProperty MovedCommandProperty = DependencyProperties[ Properties.MovedCommand ];
		public static DependencyProperty LeftClickDragCommand = DependencyProperties[ Properties.LeftClickDragCommand ];
		public static DependencyProperty RightClickDragCommand = DependencyProperties[ Properties.RightClickDragCommand ];
		public static DependencyProperty DragCapturesMouse = DependencyProperties[ Properties.DragCapturesMouse ];

		static readonly Dictionary<object, ClickDragInfo> LeftClickInfo = new Dictionary<object,ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> RightClickInfo = new Dictionary<object, ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> LeftClickDragInfo = new Dictionary<object, ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> RightClickDragInfo = new Dictionary<object, ClickDragInfo>();

		static Point _previousPosition;
		static double _distanceDragged;


		#region LeftMouseUp Command

		[DependencyProperty( Properties.LeftMouseUpCommand )]
		public static ICommand GetLeftMouseUpCommand( FrameworkElement target )
		{
			return (ICommand)DependencyProperties.GetValue( target, Properties.LeftMouseUpCommand );
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
			return (ICommand)DependencyProperties.GetValue( target, Properties.RightMouseUpCommand );
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
			return (ICommand)DependencyProperties.GetValue( target, Properties.LeftMouseDownCommand );
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
			return (ICommand)DependencyProperties.GetValue( target, Properties.RightMouseDownCommand );
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
			return (ICommand)DependencyProperties.GetValue( target, Properties.LeftClickCommand );
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
			return (ICommand)DependencyProperties.GetValue( target, Properties.RightClickCommand );
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
			return (ICommand)DependencyProperties.GetValue( target, Properties.MovedCommand );
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


		#region LeftClickDrag Command

		[DependencyProperty( Properties.LeftClickDragCommand )]
		public static ICommand GetLeftClickDragCommand( FrameworkElement target )
		{
			return (ICommand)DependencyProperties.GetValue( target, Properties.LeftClickDragCommand );
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
			return (ICommand)DependencyProperties.GetValue( target, Properties.RightClickDragCommand );
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


		#region DragCapturesMouse

		[DependencyProperty( Properties.DragCapturesMouse, DefaultValue = true )]
		public static bool GetDragCapturesMouse( FrameworkElement target )
		{
			return (bool)DependencyProperties.GetValue( target, Properties.DragCapturesMouse );
		}

		[DependencyProperty( Properties.DragCapturesMouse )]
		public static void SetDragCapturesMouse( FrameworkElement target, bool value )
		{
			DependencyProperties.SetValue( target, Properties.DragCapturesMouse, value );
		}

		#endregion	//DragCapturesMouse


		static void HookMouseMoved( IInputElement element )
		{
			// Make sure to only hook event once.
			element.MouseMove -= OnMouseMoved;
			element.MouseMove += OnMouseMoved;
		}

		static void HookMouseLeft( IInputElement element )
		{
			// Make sure to only hook event once.
			element.MouseLeftButtonDown -= OnMouseButtonDown;
			element.MouseLeftButtonDown += OnMouseButtonDown;
			element.MouseLeftButtonUp -= OnMouseButtonUp;
			element.MouseLeftButtonUp += OnMouseButtonUp;
		}

		static void HookMouseRight( IInputElement element )
		{
			// Make sure to only hook event once.
			element.MouseRightButtonDown -= OnMouseButtonDown;
			element.MouseRightButtonDown += OnMouseButtonDown;
			element.MouseRightButtonUp -= OnMouseButtonUp;
			element.MouseRightButtonUp += OnMouseButtonUp;
		}

		static void OnMouseMoved( object sender, MouseEventArgs e )
		{
			var element = (FrameworkElement)sender;

			MouseState mouseState = GetMouseState( e, element );

			// Trigger MovedCommand.
			ICommand movedCommand = GetMovedCommand( element );
			movedCommand.SafeExecute( mouseState );

			// Trigger click drag commands.
			Action<ClickDragInfo, ICommand> executeCommand = ( info, command ) =>
			{
				if ( info.State != ClickDragState.Stop )
				{
					info.Mouse = mouseState;
					info.State = ClickDragState.Moving;
					command.SafeExecute( info );
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

			Point newPosition = mouseState.Position.Relative;
			_distanceDragged += _previousPosition.DistanceTo( newPosition );
			_previousPosition = newPosition;

			e.Handled = true;
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

		static void OnMouseButtonDown( object sender, MouseButtonEventArgs e )
		{			
			var element = (FrameworkElement)sender;

			MouseState mouseState = GetMouseState( e, element );

			// Trigger down commands.
			ICommand leftDownCommand = GetLeftMouseDownCommand( element );
			if ( e.ChangedButton == MouseButton.Left )
			{
				leftDownCommand.SafeExecute( mouseState );
			}
			ICommand rightDownCommand = GetRightMouseDownCommand( element );
			if ( e.ChangedButton == MouseButton.Right )
			{
				rightDownCommand.SafeExecute( mouseState );
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
			_previousPosition = mouseState.Position.Relative;
			_distanceDragged = 0;

			// Trigger click drag commands.
			ICommand clickDragCommand = e.ChangedButton == MouseButton.Left
				? GetLeftClickDragCommand( element )
				: GetRightClickDragCommand( element );
			Dictionary<object, ClickDragInfo> dragInfo = e.ChangedButton == MouseButton.Left
				? LeftClickDragInfo
				: RightClickDragInfo;
			if ( clickDragCommand != null )
			{
				if ( GetDragCapturesMouse( element ) )
				{
					// TODO: What to do if capturing the mouse fails?
					element.CaptureMouse();
				}

				var info = new ClickDragInfo
				{
					Mouse = mouseState,
					State = ClickDragState.Start,
					StartPosition = mouseState.Position
				};
				clickDragCommand.SafeExecute( info );

				if ( !dragInfo.ContainsKey( sender ) )
				{
					dragInfo.Add( sender, info );
				}
			}

			e.Handled = true;
		}

		static void OnMouseButtonUp( object sender, MouseButtonEventArgs e )
		{
			var element = (FrameworkElement)sender;

			MouseState mouseState = GetMouseState( e, element );

			// Trigger up commands.
			ICommand leftUpCommand = GetLeftMouseUpCommand( element );
			if ( e.ChangedButton == MouseButton.Left )
			{
				leftUpCommand.SafeExecute( mouseState );
			}
			ICommand rightUpCommand = GetRightMouseUpCommand( element );
			if ( e.ChangedButton == MouseButton.Right )
			{
				rightUpCommand.SafeExecute( mouseState );
			}

			// Trigger click commands.
			if ( _distanceDragged < MaxClickDragDistance )
			{
				ClickDragInfo clickInfo;
				ICommand leftClickCommand = GetLeftClickCommand( element );
				if ( e.ChangedButton == MouseButton.Left && LeftClickInfo.TryGetValue( sender, out clickInfo ) )
				{
					leftClickCommand.SafeExecute( mouseState );
				}
				ICommand rightClickCommand = GetRightClickCommand( element );
				if ( e.ChangedButton == MouseButton.Right && RightClickInfo.TryGetValue( sender, out clickInfo ) )
				{
					rightClickCommand.SafeExecute( mouseState );
				}
			}
			LeftClickInfo.Clear();
			RightClickInfo.Clear();

			// Trigger click drag commands.
			ICommand clickDragCommand = e.ChangedButton == MouseButton.Left
				? GetLeftClickDragCommand( element )
				: GetRightClickDragCommand( element );
			Dictionary<object, ClickDragInfo> dragInfos = e.ChangedButton == MouseButton.Left
				? LeftClickDragInfo
				: RightClickDragInfo;
			ClickDragInfo dragInfo;
			if ( dragInfos.TryGetValue( sender, out dragInfo ) )
			{
				element.ReleaseMouseCapture();

				dragInfo.Mouse = mouseState;
				dragInfo.State = ClickDragState.Stop;
				clickDragCommand.SafeExecute( dragInfo );

				dragInfos.Remove( sender );
			}

			e.Handled = true;
		}

		static void SafeExecute( this ICommand command, object parameter )
		{
			if ( command != null && command.CanExecute( null ) )
			{
				command.Execute( parameter );
			}
		}
	}
}