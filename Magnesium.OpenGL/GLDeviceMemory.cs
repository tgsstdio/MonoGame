using System;
using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace Magnesium.OpenGL
{
	public class GLDeviceMemory : IMgDeviceMemory
	{
		private readonly bool mIsHostCached;
		public GLDeviceMemory (MgMemoryAllocateInfo pAllocateInfo)
		{	
			BufferType = (GLMemoryBufferType)pAllocateInfo.MemoryTypeIndex;
			mIsHostCached = (BufferType == GLMemoryBufferType.INDIRECT || BufferType== GLMemoryBufferType.IMAGE);

			if (pAllocateInfo.AllocationSize >= (ulong)int.MaxValue)
			{
				throw new InvalidCastException ("pAllocateInfo.AllocationSize");
			}

			BufferSize = (int) pAllocateInfo.AllocationSize;
		
			if (BufferType != GLMemoryBufferType.IMAGE)
				mBufferTarget = BufferType.GetBufferTarget ();

			if (mIsHostCached || pAllocateInfo.MemoryTypeIndex == (uint) GLMemoryBufferType.IMAGE)
			{
				Handle = Marshal.AllocHGlobal (BufferSize);
			} 
			else
			{
				if (mBufferTarget.HasValue)
				{
					{
						var error = GL.GetError ();
						Debug.WriteLineIf (error != ErrorCode.NoError, "GLDeviceMemory (PREVIOUS) : " + error);
					}

					var buffers = new int[1];
					// ARB_direct_state_access
					// Allows buffer objects to be initialised without binding them
					GL.CreateBuffers (1, buffers);

					{
						var error = GL.GetError ();
						Debug.WriteLineIf (error != ErrorCode.NoError, "GL.CreateBuffers : " + error);
					}
					BufferId = buffers[0];

					//GL.BindBuffer (mBufferTarget.Value, BufferId);
					BufferStorageFlags flags = BufferStorageFlags.MapWriteBit | BufferStorageFlags.MapPersistentBit | BufferStorageFlags.MapCoherentBit;
					GL.NamedBufferStorage (BufferId, BufferSize, IntPtr.Zero, flags);

					{
						var error = GL.GetError ();
						Debug.WriteLineIf (error != ErrorCode.NoError, "GL.NamedBufferStorage : " + error);
					}					 

//					BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;
//					Handle = GL.MapNamedBufferRange (buffers[0], (IntPtr)0, BufferSize, rangeFlags);
				}
			}
		}
		private BufferTarget? mBufferTarget;

		public readonly GLMemoryBufferType BufferType;
		public readonly int BufferSize;
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

				if (size >= (ulong)int.MaxValue)
				{
					throw new InvalidCastException ("size >= Int64.MaxValue");
				}

				var handleOffset = (IntPtr)((Int64)offset);
				var handleSize = (int) size;

				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "MapMemory (BEFORE)  : " + error);

				// TODO: flags translate 
				BufferAccessMask rangeFlags = BufferAccessMask.MapWriteBit | BufferAccessMask.MapPersistentBit | BufferAccessMask.MapCoherentBit;
				ppData = GL.MapNamedBufferRange (BufferId, IntPtr.Zero, handleSize, rangeFlags);

				error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "MapMemory (MapNamedBufferRange)  : " + error);

				//ppData = GL.Ext.MapNamedBufferRange(BufferId, handleOffset, handleSize, BufferAccessMask.MapWriteBit | BufferAccessMask.MapCoherentBit);


				mIsMapped = true;
				return Result.SUCCESS;
			}

		}

		public void UnmapMemory (IMgDevice device)
		{
			if (!mIsHostCached && mIsMapped)
			{	
				bool isValid = GL.Ext.UnmapNamedBuffer (BufferId);

				Debug.WriteLineIf (!isValid, "DeviceMemory is invalid");

				var error = GL.GetError ();
				Debug.WriteLineIf (error != ErrorCode.NoError, "UnmapNamedBuffer : " + error);
			}
//			else if (mIsHostCached && BufferType == GLMemoryBufferType.IMAGE)
//			{
//
//			}

			mIsMapped = false;
		}

		#endregion
	}
}

