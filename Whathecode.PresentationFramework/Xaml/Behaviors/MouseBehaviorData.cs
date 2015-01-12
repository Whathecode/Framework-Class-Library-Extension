
namespace Whathecode.System.Xaml.Behaviors
{
	public class MouseBehaviorData
	{
		public object MouseBehaviorParameter { get; private set; }
		public MouseBehavior.MouseState MouseState { get; private set; }

		public MouseBehaviorData( object mouseBehaviorParameter, MouseBehavior.MouseState mouseState )
		{
			MouseBehaviorParameter = mouseBehaviorParameter;
			MouseState = mouseState;
		}
	}
}
