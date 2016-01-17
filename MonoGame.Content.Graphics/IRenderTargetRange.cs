using System.Collections.Generic;
using System;

namespace BirdNest.MonoGame.Graphics
{
	public interface IRenderTargetRange : IDisposable
	{
		IRenderTarget Default { get; }
		IList<IRenderTarget> Targets { get;}
		void Add(IRenderTarget target);
		void Clear();
	}
}

