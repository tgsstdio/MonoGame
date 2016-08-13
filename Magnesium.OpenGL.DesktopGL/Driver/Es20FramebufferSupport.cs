namespace Magnesium.OpenGL
{
	public class Es20FramebufferSupport : IGLFramebufferSupport
	{
		#region IGraphicsCapabilitiesLookup implementation

		public bool SupportsFramebufferObjectARB ()
		{
			return true;
		}

		public bool SupportsFramebufferObjectEXT ()
		{
			return false;
		}

		#endregion
	}
}

