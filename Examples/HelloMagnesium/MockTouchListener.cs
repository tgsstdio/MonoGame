using Microsoft.Xna.Framework.Input.Touch;

namespace HelloMagnesium
{
	public class MockTouchListener : ITouchListener
	{
		#region ITouchListener implementation

		public void ReleaseAllTouches ()
		{
			throw new System.NotImplementedException ();
		}
		public GestureSample GetGestumeSample ()
		{
			throw new System.NotImplementedException ();
		}
		public TouchPanelState GetPanelState ()
		{
			throw new System.NotImplementedException ();
		}

		public void AddEvent (int id, TouchLocationState state, Microsoft.Xna.Framework.Vector2 position)
		{
			throw new System.NotImplementedException ();
		}

		public void AddEvent (int id, TouchLocationState state, Microsoft.Xna.Framework.Vector2 position, bool isMouse)
		{
			throw new System.NotImplementedException ();
		}
		public ITouchPanelCapabilities GetCapabilities ()
		{
			throw new System.NotImplementedException ();
		}
		public TouchCollection GetState ()
		{
			throw new System.NotImplementedException ();
		}
		public bool EnableMouseGestures {
			get;
			set;
		}
		public bool IsGestureAvailable {
			get;
			set;
		}
		public bool EnableMouseTouchPoint {
			get;
			set;
		}
		public GestureType EnabledGestures {
			get;
			set;
		}
		public Microsoft.Xna.Framework.DisplayOrientation DisplayOrientation {
			get;
			set;
		}
		public int DisplayWidth {
			get;
			set;
		}
		public int DisplayHeight {
			get;
			set;
		}
		public System.IntPtr WindowHandle {
			get;
			set;
		}
		#endregion
		
	}
}
