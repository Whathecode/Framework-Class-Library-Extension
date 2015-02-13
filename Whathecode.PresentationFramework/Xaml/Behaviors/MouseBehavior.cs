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

		public struct MouseCommandArgs
		{
			public MouseState MouseState;
			public object Parameter;
		}

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

		public struct MouseDragCommandArgs
		{
			public ClickDragInfo DragInfo;
			public object Parameter;
		}

		public struct ClickDragInfo
		{
			public FrameworkElement Sender;
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
		public enum Properties
		{
			LeftMouseUpCommand = 1,
			LeftMouseUpCommandParameter,
			RightMouseUpCommand,
			RightMouseUpCommandParameter,
			LeftMouseDownCommand,
			LeftMouseDownCommandParameter,
			RightMouseDownCommand,
			RightMouseDownCommandParameter,
			LeftClickCommand,
			LeftClickCommandParameter,
			RightClickCommand,
			RightClickCommandParameter,
			MovedCommand,
			MovedCommandParameter,
			LeftClickDragCommand,
			LeftClickDragCommandParameter,
			RightClickDragCommand,
			RightClickDragCommandParameter,
			DragCapturesMouse
		}


		static readonly DependencyPropertyFactory<Properties> DependencyProperties = new DependencyPropertyFactory<Properties>();

		public static DependencyProperty LeftMouseUpCommandProperty = DependencyProperties[ Properties.LeftMouseUpCommand ];
		public static DependencyProperty LeftMouseUpCommandParameterProperty = DependencyProperties[ Properties.LeftMouseUpCommandParameter ];
		public static DependencyProperty RightMouseUpCommandProperty = DependencyProperties[ Properties.RightMouseUpCommand ];
		public static DependencyProperty RightMouseUpCommandParameterProperty = DependencyProperties[ Properties.RightMouseUpCommandParameter ];
		public static DependencyProperty LeftMouseDownCommandProperty = DependencyProperties[ Properties.LeftMouseDownCommand ];
		public static DependencyProperty LeftMouseDownCommandParameterProperty = DependencyProperties[ Properties.LeftMouseDownCommandParameter ];
		public static DependencyProperty RightMouseDownCommandProperty = DependencyProperties[ Properties.RightMouseDownCommand ];
		public static DependencyProperty RightMouseDownCommandParameterProperty = DependencyProperties[ Properties.RightMouseDownCommandParameter ];
		public static DependencyProperty LefClickCommandProperty = DependencyProperties[ Properties.LeftClickCommand ];
		public static DependencyProperty LeftClickCommandParameterProperty = DependencyProperties[ Properties.LeftClickCommandParameter ];
		public static DependencyProperty RightClickCommandProperty = DependencyProperties[ Properties.RightClickCommand ];
		public static DependencyProperty RightClickCommandParameterProperty = DependencyProperties[ Properties.RightClickCommandParameter ];
		public static DependencyProperty MovedCommandProperty = DependencyProperties[ Properties.MovedCommand ];
		public static DependencyProperty MovedCommandParameterProperty = DependencyProperties[ Properties.MovedCommandParameter ];
		public static DependencyProperty LeftClickDragCommandProperty = DependencyProperties[ Properties.LeftClickDragCommand ];
		public static DependencyProperty LeftClickDragCommandParameterProperty = DependencyProperties[ Properties.LeftClickDragCommandParameter ];
		public static DependencyProperty RightClickDragCommandProperty = DependencyProperties[ Properties.RightClickDragCommand ];
		public static DependencyProperty RightClickDragCommandParameterProperty = DependencyProperties[ Properties.RightClickDragCommandParameter ];
		public static DependencyProperty DragCapturesMouseProperty = DependencyProperties[ Properties.DragCapturesMouse ];

		static readonly Dictionary<object, ClickDragInfo> LeftClickInfo = new Dictionary<object, ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> RightClickInfo = new Dictionary<object, ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> LeftClickDragInfo = new Dictionary<object, ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> RightClickDragInfo = new Dictionary<object, ClickDragInfo>();

		static Point _previousPosition;
		static double _distanceDragged;


		// ReSharper disable UnusedMember.Local
		// ReSharper disable UnusedParameter.Local

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

		[DependencyProperty( Properties.LeftMouseUpCommandParameter )]
		public static object GetLeftMouseUpCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.LeftMouseUpCommandParameter );
		}

		[DependencyProperty( Properties.LeftMouseUpCommandParameter )]
		public static void SetLeftMouseUpCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.LeftMouseUpCommandParameter, value );
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

		[DependencyProperty( Properties.RightMouseUpCommandParameter )]
		public static object GetRightMouseUpCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.RightMouseUpCommandParameter );
		}

		[DependencyProperty( Properties.RightMouseUpCommandParameter )]
		public static void SetRightMouseUpCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.RightMouseUpCommandParameter, value );
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

		[DependencyProperty( Properties.LeftMouseDownCommandParameter )]
		public static object GetLeftMouseDownCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.LeftMouseDownCommandParameter );
		}

		[DependencyProperty( Properties.LeftMouseDownCommandParameter )]
		public static void SetLeftMouseDownCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.LeftMouseDownCommandParameter, value );
		}

		#endregion // LeftMouseDown Command


		#region RightMouseDown Command

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

		[DependencyProperty( Properties.RightMouseDownCommandParameter )]
		public static object GetRightMouseDownCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.RightMouseDownCommandParameter );
		}

		[DependencyProperty( Properties.RightMouseDownCommandParameter )]
		public static void SetRightMouseDownCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.RightMouseDownCommandParameter, value );
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

		[DependencyProperty( Properties.LeftClickCommandParameter )]
		public static object GetLeftClickCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.LeftClickCommandParameter );
		}

		[DependencyProperty( Properties.LeftClickCommandParameter )]
		public static void SetLeftClickCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.LeftClickCommandParameter, value );
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

		[DependencyProperty( Properties.RightClickCommandParameter )]
		public static object GetRightClickCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.RightClickCommandParameter );
		}

		[DependencyProperty( Properties.RightClickCommandParameter )]
		public static void SetRightClickCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.RightClickCommandParameter, value );
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

		[DependencyProperty( Properties.MovedCommandParameter )]
		public static object GetMovedCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.MovedCommandParameter );
		}

		[DependencyProperty( Properties.MovedCommandParameter )]
		public static void SetMovedCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.MovedCommandParameter, value );
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

		[DependencyProperty( Properties.LeftClickDragCommandParameter )]
		public static object GetLeftClickDragCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.LeftClickDragCommandParameter );
		}

		[DependencyProperty( Properties.LeftClickDragCommandParameter )]
		public static void SetLeftClickDragCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.LeftClickDragCommandParameter, value );
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

		[DependencyProperty( Properties.RightClickDragCommandParameter )]
		public static object GetRightClickDragCommandParameter( FrameworkElement target )
		{
			return DependencyProperties.GetValue( target, Properties.RightClickDragCommandParameter );
		}

		[DependencyProperty( Properties.RightClickDragCommandParameter )]
		public static void SetRightClickDragCommandParameter( FrameworkElement target, object value )
		{
			DependencyProperties.SetValue( target, Properties.RightClickDragCommandParameter, value );
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

		// ReSharper restore UnusedParameter.Local
		// ReSharper restore UnusedMember.Local


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
			movedCommand.SafeExecute( mouseState, GetMovedCommandParameter( element ) );

			// Trigger click drag commands.
			Action<ClickDragInfo, object, ICommand> executeCommand = ( info, parameter, command ) =>
			{
				if ( info.State != ClickDragState.Stop )
				{
					info.Mouse = mouseState;
					info.State = ClickDragState.Moving;
					command.SafeExecute( info, parameter );
				}
			};
			if ( mouseState.IsLeftButtonDown )
			{
				LeftClickDragInfo.TryUseValue( sender, info => executeCommand( info, GetLeftClickDragCommandParameter( element ), GetLeftClickDragCommand( element ) ) );
			}
			if ( mouseState.IsRightButtonDown )
			{
				RightClickDragInfo.TryUseValue( sender, info => executeCommand( info, GetRightClickDragCommandParameter( element ), GetRightClickDragCommand( element ) ) );
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
			bool isLeftButton = e.ChangedButton == MouseButton.Left;

			// Trigger down commands.
			ICommand leftDownCommand = GetLeftMouseDownCommand( element );
			if ( isLeftButton )
			{
				leftDownCommand.SafeExecute( mouseState, GetLeftMouseDownCommandParameter( element ) );
			}
			ICommand rightDownCommand = GetRightMouseDownCommand( element );
			if ( e.ChangedButton == MouseButton.Right )
			{
				rightDownCommand.SafeExecute( mouseState, GetRightMouseDownCommandParameter( element ) );
			}

			// Track click commands.
			ICommand clickCommand = isLeftButton ? GetLeftClickCommand( element ) : GetRightClickCommand( element );
			Dictionary<object, ClickDragInfo> clickInfo = isLeftButton ? LeftClickInfo : RightClickInfo;
			if ( clickCommand != null )
			{
				var info = new ClickDragInfo
				{
					Sender = element,
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
			ICommand clickDragCommand = isLeftButton ? GetLeftClickDragCommand( element ) : GetRightClickDragCommand( element );
			object clickDragParameter = isLeftButton ? GetLeftClickDragCommandParameter( element ) : GetRightClickDragCommandParameter( element );
			Dictionary<object, ClickDragInfo> dragInfo = isLeftButton ? LeftClickDragInfo : RightClickDragInfo;
			if ( clickDragCommand != null )
			{
				if ( GetDragCapturesMouse( element ) )
				{
					// TODO: What to do if capturing the mouse fails?
					element.CaptureMouse();
				}

				var info = new ClickDragInfo
				{
					Sender = element,
					Mouse = mouseState,
					State = ClickDragState.Start,
					StartPosition = mouseState.Position
				};
				clickDragCommand.SafeExecute( info, clickDragParameter );

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
			bool isLeftButton = e.ChangedButton == MouseButton.Left;

			// Trigger up commands.
			ICommand leftUpCommand = GetLeftMouseUpCommand( element );
			if ( isLeftButton )
			{
				leftUpCommand.SafeExecute( mouseState, GetLeftMouseUpCommandParameter( element ) );
			}
			ICommand rightUpCommand = GetRightMouseUpCommand( element );
			if ( e.ChangedButton == MouseButton.Right )
			{
				rightUpCommand.SafeExecute( mouseState, GetRightMouseUpCommandParameter( element ) );
			}

			// Trigger click commands.
			if ( _distanceDragged < MaxClickDragDistance )
			{
				ClickDragInfo clickInfo;
				ICommand leftClickCommand = GetLeftClickCommand( element );
				if ( isLeftButton && LeftClickInfo.TryGetValue( sender, out clickInfo ) )
				{
					leftClickCommand.SafeExecute( mouseState, GetLeftClickCommandParameter( element ) );
				}
				ICommand rightClickCommand = GetRightClickCommand( element );
				if ( e.ChangedButton == MouseButton.Right && RightClickInfo.TryGetValue( sender, out clickInfo ) )
				{
					rightClickCommand.SafeExecute( mouseState, GetRightClickCommandParameter( element ) );
				}
			}
			LeftClickInfo.Clear();
			RightClickInfo.Clear();

			// Trigger click drag commands.
			ICommand clickDragCommand = isLeftButton ? GetLeftClickDragCommand( element ) : GetRightClickDragCommand( element );
			object clickDragParameter = isLeftButton ? GetLeftClickDragCommandParameter( element ) : GetRightClickDragCommandParameter( element );
			Dictionary<object, ClickDragInfo> dragInfos = isLeftButton ? LeftClickDragInfo : RightClickDragInfo;
			ClickDragInfo dragInfo;
			if ( dragInfos.TryGetValue( sender, out dragInfo ) )
			{
				element.ReleaseMouseCapture();

				dragInfo.Mouse = mouseState;
				dragInfo.State = ClickDragState.Stop;
				clickDragCommand.SafeExecute( dragInfo, clickDragParameter );

				dragInfos.Remove( sender );
			}

			e.Handled = true;
		}

		static void SafeExecute( this ICommand command, MouseState state, object parameter )
		{
			var args = new MouseCommandArgs { MouseState = state, Parameter = parameter };
			if ( command != null && command.CanExecute( args ) )
			{
				command.Execute( args );
			}
		}

		static void SafeExecute( this ICommand dragCommand, ClickDragInfo dragInfo, object parameter )
		{
			var args = new MouseDragCommandArgs { DragInfo = dragInfo, Parameter = parameter };
			if ( dragCommand != null && dragCommand.CanExecute( args ) )
			{
				dragCommand.Execute( args );
			}
		}
	}
}