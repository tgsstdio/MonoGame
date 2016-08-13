namespace Magnesium.OpenGL
{
	public class FullDescriptorPoolEntrypoint : IGLDescriptorPoolEntrypoint
	{
		#region IMgDescriptorPoolFactory implementation

		readonly IGLImageDescriptorEntrypoint mImgDescriptor;

		public FullDescriptorPoolEntrypoint (IGLImageDescriptorEntrypoint entrypoint)
		{
			mImgDescriptor = entrypoint;
		}

		public IGLDescriptorPool CreatePool (MgDescriptorPoolCreateInfo createInfo)
		{
			return new GLDescriptorPool (createInfo.MaxSets != 0 ? (int) createInfo.MaxSets : 100, mImgDescriptor);
		}

		#endregion
	}
}

