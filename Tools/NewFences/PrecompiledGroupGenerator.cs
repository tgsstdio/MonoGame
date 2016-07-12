using System;
using Magnesium;
using System.Collections.Generic;

namespace NewFences
{
	public class PrecompiledGroupGenerator
	{
		public static PrecompiledCommandGroup Generate
		(
			CommandGroupCreateInfo[] groups
		)
		{			
			if (groups == null)
			{
				throw new ArgumentNullException ("groups");
			}

			var submitInfos = new List<SubmitInfoGraphNode> ();
			for (var i = 0; i < groups.Length; ++i)
			{
				var groupInfo = groups [i];

				groupInfo.BuildAction (groupInfo.CommandBuffer, groupInfo.Framebuffer);

				var submit = new SubmitInfoGraphNode {
					IsVisible = true,
					Submit = new MgSubmitInfo
					{						
						WaitSemaphores = groupInfo.Waits,
						CommandBuffers = new []{groupInfo.CommandBuffer},
						SignalSemaphores = groupInfo.Signals,
					},
					Fence = groupInfo.Fence,
				};
				submitInfos.Add (submit);
			}

			var output = new PrecompiledCommandGroup (
			       submitInfos.ToArray ()
            );

			return output;
		}	
	}
}

