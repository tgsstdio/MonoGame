﻿using System;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace MonoGame.Graphics.AZDO
{
	/// <summary>
	/// Shader storage buffer for static data
	/// </summary>
	public class FullShaderStorageBuffer<TData> : IConstantBuffer, IShaderStorageBuffer<TData>
		where TData : struct
	{
		public int BufferId { get; private set; }
		public int Location { get; private set; }
		public BufferUsageHint Hint { get; private set; }

		public FullShaderStorageBuffer(int location, BufferUsageHint hint)
		{
			// Buffer for the linked list.
			BufferId = GL.GenBuffer();

			Location = location;
			Hint = hint;
		}

		public ushort Index {
			get {
				throw new NotImplementedException ();
			}
		}

		public void SetCapacity (IntPtr bufferSize)
		{
			GL.BindBuffer (BufferTarget.ShaderStorageBuffer, BufferId);
			GL.BindBufferBase (BufferRangeTarget.ShaderStorageBuffer, Location, BufferId);
			GL.BufferData(BufferTarget.ShaderStorageBuffer, bufferSize, IntPtr.Zero, Hint);
			GL.BindBuffer (BufferTarget.ShaderStorageBuffer, 0);
		}

		public void SetData (TData[] blocks, IntPtr offset)
		{
			GL.BindBuffer (BufferTarget.ShaderStorageBuffer, BufferId);
			GL.BindBufferBase (BufferRangeTarget.ShaderStorageBuffer, Location, BufferId);
			var structSize = Marshal.SizeOf (typeof(TData));
			var bufferSize = (IntPtr)(blocks.Length * structSize);
			GL.BufferSubData<TData> (BufferTarget.ShaderStorageBuffer, offset, bufferSize, blocks);
			GL.BindBuffer (BufferTarget.ShaderStorageBuffer, 0);
		}

		public void Bind()
		{
			GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, Location, BufferId);
			GL.BindBuffer(BufferTarget.ShaderStorageBuffer, BufferId);
		}

		public void Unbind()
		{
			GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
		}

		~FullShaderStorageBuffer()
		{
			Dispose (false);
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		void ReleaseUnmanagedResources()
		{
			GL.DeleteBuffer (BufferId);
		}

		void ReleaseManagedResources()
		{
			
		}

		private bool mDisposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (mDisposed)
				return;

			ReleaseUnmanagedResources ();

			if (disposing)
			{
				ReleaseManagedResources ();
			}
			mDisposed = true;
		}
	}
}

