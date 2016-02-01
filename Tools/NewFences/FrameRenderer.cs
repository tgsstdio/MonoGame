using System;

namespace NewFences
{
	public class FrameRenderer
	{
		public RenderFrame[] Frames { get; set; }

		private readonly IFenceIndexVariable mIndexer;
		public FrameRenderer (IFenceIndexVariable indexer)
		{
			mIndexer = indexer;
		}

		public void Render()
		{
			if (mIndexer != null)
			{
				int index = mIndexer.Index;
				if (Frames != null)
				{
					foreach (var f in Frames)
					{
						if (f.Passes != null)
						{
							foreach (var pass in f.Passes)
							{	
								if (pass.Requirements != null)
								{
									foreach (var r in pass.Requirements)
									{						
										r.WaitForGPU (index);
									}
								}

								if (pass.Groups != null)
								{
									foreach (var group in pass.Groups)
									{
										group.RenderAll (index);
										// only lock after final render 
										if (pass.Id == group.Buffer.Fence.LastPass)
										{
											group.Buffer.Fence.Lock (index);
										}
									}
								}
								if (pass.Fence != null)
									pass.Fence.Lock (index);
							}

							if (f.Fence != null)
							{
								f.Fence.Lock (index);
								//f.Fence.WaitForGPU (Index);
								// IF SWAP HERE
							}
						}
					}
				}
				mIndexer.Increment ();
			}
		}
	}
}

