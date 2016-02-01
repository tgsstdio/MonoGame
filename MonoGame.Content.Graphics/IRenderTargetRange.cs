using System.Collections.Generic;
using System;

namespace MonoGame.Shaders
{
	public interface IRenderTargetRange : IDisposable
	{
		IRenderTarget Default { get; }
		IList<IRenderTarget> Targets { get;}
		void Add(IRenderTarget target);
		void Clear();
	}
}

