using System;

namespace MonoGame.Graphics.AZDO
{
	public interface IShaderStorageBuffer<TData> 
		where TData : struct
	{
		void SetData (TData[] blocks, IntPtr offset);				
	}
}

