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


		enum Properties
		{
			MovedCommand,
			LeftClickDragCommand,
			RightClickDragCommand
		}


		static readonly DependencyPropertyFactory<Properties> DependencyProperties = new DependencyPropertyFactory<Properties>();

		public static DependencyProperty MovedCommandProperty = DependencyProperties[ Properties.MovedCommand ];
		public static DependencyProperty LeftClickDragCommand = DependencyProperties[ Properties.LeftClickDragCommand ];
		public static DependencyProperty RightClickDragCommand = DependencyProperties[ Properties.RightClickDragCommand ];

		static readonly Dictionary<object, ClickDragInfo> LeftClickDragInfo = new Dictionary<object, ClickDragInfo>();
		static readonly Dictionary<object, ClickDragInfo> RightClickDragInfo = new Dictionary<object, ClickDragInfo>();


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
			IInputElement element = d as IInputElement;

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
			IInputElement element = d as IInputElement;

			if ( element != null )
			{
				HookMouseMoved( element );
				element.MouseLeftButtonDown += MouseButtonDown;
				element.MouseLeftButtonUp += MouseButtonUp;
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
			IInputElement element = d as IInputElement;

			if ( element != null )
			{
				HookMouseMoved( element );
				element.MouseRightButtonDown += MouseButtonDown;
				element.MouseRightButtonUp += MouseButtonUp;
			}
		}

		#endregion  // RightClickDrag Command


		static void HookMouseMoved( IInputElement element )
		{
			// Make sure to only hook event once.
			element.MouseMove -= OnMouseMoved;
			element.MouseMove += OnMouseMoved;
		}

		static void OnMouseMoved( object sender, MouseEventArgs e )
		{
			FrameworkElement element = (FrameworkElement)sender;

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
			FrameworkElement element = (FrameworkElement)sender;

			MouseState mouseState = GetMouseState( e, element );

			// Trigger click drag commands.
			ICommand clickDragCommand = e.ChangedButton == MouseButton.Left
				? GetLeftClickDragCommand( element )
				: GetRightClickDragCommand( element );
			Dictionary<object, ClickDragInfo> dragInfo = e.ChangedButton == MouseButton.Left
				? LeftClickDragInfo
				: RightClickDragInfo;
			if ( clickDragCommand != null )
			{
				ClickDragInfo info = new ClickDragInfo
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
			FrameworkElement element = (FrameworkElement)sender;

			MouseState mouseState = GetMouseState( e, element );

			// Trigger click drag commands
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