using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public class GLDeviceMemory : IMgDeviceMemory
	{
		private readonly bool mIsHostCached;
		public GLDeviceMemory (MgMemoryAllocateInfo pAllocateInfo)
		{	
			mIsHostCached = (pAllocateInfo.MemoryTypeIndex == (int) GLMemoryBufferType.INDIRECT);

			if (pAllocateInfo.AllocationSize >= (ulong)Int64.MaxValue)
			{
				throw new InvalidCastException ("pAllocateInfo.AllocationSize");
			}

			BufferSize = (IntPtr) ((Int64) pAllocateInfo.AllocationSize);
			BufferType = (GLMemoryBufferType)pAllocateInfo.MemoryTypeIndex;
			mBufferTarget = BufferType.GetBufferTarget ();

			if (mIsHostCached)
			{
				Handle = Marshal.AllocHGlobal (BufferSize);
			} 
			else
			{
				BufferId = GL.GenBuffer ();
				GL.BindBuffer(mBufferTarget, BufferId);
				BufferStorageFlags flags = BufferStorageFlags.MapWriteBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapCoherentBit;
				GL.BufferStorage(mBufferTarget, BufferSize, IntPtr.Zero, flags);

				BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;
				Handle = GL.MapBufferRange(mBufferTarget, (IntPtr) 0, BufferSize, rangeFlags);
			}
		}
		private BufferTarget mBufferTarget;

		public readonly GLMemoryBufferType BufferType;
		public readonly IntPtr BufferSize;
		public readonly int BufferId;
		public readonly IntPtr Handle;

		#region IMgDeviceMemory implementation
		private bool mIsDisposed = false;
		public void FreeMemory (IMgDevice device, MgAllocationCallbacks allocator)
		{
			if (mIsDisposed)
				return;

			if (mIsHostCached)
			{	
				Marshal.FreeHGlobal (Handle);
			}
			else
			{
				GL.DeleteBuffer (BufferId);
			}

			mIsDisposed = true;
		}

		private bool mIsMapped = false;
		public Result MapMemory (IMgDevice device, ulong offset, ulong size, uint flags, out IntPtr ppData)
		{
			if (mIsHostCached)
			{	
				if (offset >= (ulong)Int32.MaxValue)
				{
					throw new InvalidCastException ("offset >= Int32.MaxValue");
				}
		
				var handleOffset = (Int32)offset;
				ppData = IntPtr.Add (Handle, handleOffset);
				mIsMapped = true;
				return Result.SUCCESS;
			} 
			else
			{
				if (offset >= (ulong)Int64.MaxValue)
				{
					throw new InvalidCastException ("offset >= Int64.MaxValue");
				}

				if (size >= (ulong)Int64.MaxValue)
				{
					throw new InvalidCastException ("size >= Int64.MaxValue");
				}

				var handleOffset = (IntPtr)((Int64)offset);
				var handleSize = (IntPtr)((Int64)size);

				// TODO: flags translate 
				ppData = GL.Ext.MapNamedBufferRange(BufferId, handleOffset, handleSize, 0);
				mIsMapped = true;
				return Result.SUCCESS;
			}

		}

		public void UnmapMemory (IMgDevice device)
		{
			if (!mIsHostCached && mIsMapped)
			{	
				GL.Ext.UnmapNamedBuffer (BufferId);
			}

			mIsMapped = false;
		}

		#endregion
	}
}

