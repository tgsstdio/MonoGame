using System;

namespace MonoGame.Graphics.Vk
{
	public class PhysicalDeviceProperties
	{
		public UInt32 ApiVersion { get; set; }
		public UInt32 DriverVersion { get; set; }
		public UInt32 VendorID { get; set; }
		public UInt32 DeviceID { get; set; }
		public PhysicalDeviceType DeviceType { get; set; }
		public String DeviceName { get; set; }
		public Guid PipelineCacheUUID { get; set; }
		public PhysicalDeviceLimits Limits { get; set; }
		public PhysicalDeviceSparseProperties SparseProperties { get; set; }
	}
}

