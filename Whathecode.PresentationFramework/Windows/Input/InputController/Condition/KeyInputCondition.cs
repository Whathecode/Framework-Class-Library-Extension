using System;
using System.Windows;
using System.Windows.Input;


namespace Whathecode.System.Windows.Input.InputController.Condition
{
	/// <summary>
	///   A class which can check whether a certain key is in a certain key state. ( Down, Up, Pressed, Released )
	/// </summary>
	/// <author>Steven Jeuris <email>mailto:steven@aimproductions.be</email></author>
	public class KeyInputCondition : AbstractCondition
	{
		public enum KeyState
		{
			/// <summary>
			///   The key is down. (being pressed)
			/// </summary>
			Pressed,
			/// <summary>
			///   The key is not down. (Not being touched.)
			/// </summary>
			Released,
			/// <summary>
			///   The key was up and is just pressed.
			/// </summary>
			Down,
			/// <summary>
			///   The key was down and is just released.
			/// </summary>
			Up
		}


		readonly Func<bool> _isKeyDown;
		readonly KeyState _neededKeyState;


		/// <summary>
		///   Create an input condition which checks whether a certain key is in a certain key state.
		/// </summary>
		/// <param name = "isKeyDown">Method to check whether a key is down.</param>
		/// <param name = "keyState">The key state on which the condition should validate to true.</param>
		public KeyInputCondition( Func<bool> isKeyDown, KeyState keyState )
		{
			_isKeyDown = isKeyDown;
			_neededKeyState = keyState;
		}

		/// <summary>
		///   Create an input condition which checks whether a certain key is in a certain key state.
		///   HACK: For now the dispatcher of the application is used because an exception is thrown otherwise.
		/// </summary>
		/// <param name = "key">The key you want to check.</param>
		/// <param name = "keyState">The key state on which the condition should validate to true.</param>
		public KeyInputCondition( Key key, KeyState keyState )
			: this(
				() =>
				{
					bool down = false;
					Application app = Application.Current;
					if ( app != null )
					{
						down = app.Dispatcher.CheckAccess()
							? Keyboard.IsKeyDown( key )
							: (bool)app.Dispatcher.Invoke( new Func<bool>( () => Keyboard.IsKeyDown( key ) ) );
					}

					return down;
				},
				keyState
				) {}

		/// <summary>
		///   Create an input condition which checks whether a certain modifier key is in a certain key state.
		/// </summary>
		/// <param name = "modifierKey">The modifier key you want to check.</param>
		/// <param name = "keyState">The key state on which the condition should validate to true.</param>
		public KeyInputCondition( ModifierKeys modifierKey, KeyState keyState )
			: this(
				() => (Keyboard.Modifiers & modifierKey) == modifierKey,
				keyState
				) {}

		public KeyInputCondition( MouseButton mouseButton, KeyState keyState )
			: this(
				() => IsMouseKeyDown( mouseButton ),
				keyState
				) {}


		/// <summary>
		///   Returns whether a certain mouse button is currently pressed or not.
		/// </summary>
		/// <param name = "mouseButton">The button to check.</param>
		/// <returns>True if button is pressed, false otherwise.</returns>
		static bool IsMouseKeyDown( MouseButton mouseButton )
		{
			switch ( mouseButton )
			{
				case MouseButton.Left:
					return Mouse.LeftButton == MouseButtonState.Pressed;
				case MouseButton.Middle:
					return Mouse.MiddleButton == MouseButtonState.Pressed;
				case MouseButton.Right:
					return Mouse.RightButton == MouseButtonState.Pressed;
				case MouseButton.XButton1:
					return Mouse.XButton1 == MouseButtonState.Pressed;
				case MouseButton.XButton2:
					return Mouse.XButton2 == MouseButtonState.Pressed;
			}

			return false;
		}


		#region AbstractInputCondition Members

		bool _keyIsDown;
		bool _wasPressed;
		bool _wasReleased;

		public override void Update()
		{
			bool prevIsDown = _keyIsDown;
			bool nowIsDown = _isKeyDown();

			// HACK: Resharper believes nowIsDown is always true.
			// ReSharper disable ConditionIsAlwaysTrueOrFalse
			if ( prevIsDown == nowIsDown )
			{
				_wasPressed = false;
				_wasReleased = false;
			}
			else if ( !prevIsDown && nowIsDown )
			{
				_wasPressed = true;
			}
			else if ( prevIsDown && !nowIsDown )
			{
				_wasReleased = true;
			}
			// ReSharper restore ConditionIsAlwaysTrueOrFalse

			_keyIsDown = nowIsDown;
		}

		protected override bool InputValidates()
		{
			switch ( _neededKeyState )
			{
				case KeyState.Pressed:
					return _keyIsDown;
				case KeyState.Released:
					return !_keyIsDown;
				case KeyState.Down:
				{
					bool curWasPressed = _wasPressed;

					// when state validates reset it so it's not read more than once
					if ( _wasPressed )
					{
						_wasPressed = false;
					}

					return curWasPressed;
				}
				case KeyState.Up:
				{
					bool curWasReleased = _wasReleased;

					// when state validates reset it so it's not read more than once
					if ( _wasReleased )
					{
						_wasReleased = false;
					}

					return curWasReleased;
				}
				default:
					return false;
			}
		}

		#endregion  // AbstractInputCondition Members
	}
}