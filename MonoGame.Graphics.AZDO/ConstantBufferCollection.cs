// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace MonoGame.Graphics.AZDO
{
	public sealed class ConstantBufferCollection : IConstantBufferCollection
    {
        private readonly IConstantBuffer[] _buffers;

		public ushort Active { get; private set; }

        public ConstantBufferCollection(int maxBuffers)
        {
			_buffers = new IConstantBuffer[maxBuffers];
            Active = 0;
        }

		#region IConstantBufferCollection implementation

		public IConstantBuffer[] Filter (MonoGame.Graphics.Mesh mesh, EffectPass pass, int options)
		{
			throw new System.NotImplementedException ();
		}

		public void Add (IConstantBuffer b)
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		public IConstantBuffer this[ushort index]
        {
            get { return _buffers[index]; }
            set
            {
                if (_buffers[index] == value)
                    return;

                if (value != null)
                {
                    _buffers[index] = value;
					Active |= (ushort)(1 << index);
                }
                else
                {
                    _buffers[index] = null;
					Active &= (ushort)(~1 << index);
                }
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _buffers.Length; i++)
                _buffers[i] = null;

            Active = 0;
        }

#if DIRECTX
        internal void SetConstantBuffers(GraphicsDevice device)
#elif WEB
        internal void SetConstantBuffers(GraphicsDevice device, int shaderProgram)
#elif OPENGL
        internal void SetConstantBuffers(GraphicsDevice device, ShaderProgram shaderProgram)
#else
		// NEVER IN USE
		internal void SetConstantBuffers ()
#endif
        {
            // If there are no constant buffers then skip it.
            if (Active == 0)
                return;

            var valid = Active;

            for (var i = 0; i < _buffers.Length; i++)
            {
                var buffer = _buffers[i];
                if (buffer != null)
                {
					buffer.Bind();

					#if DIRECTX
					buffer.PlatformApply(device, _stage, i);
					#elif OPENGL || WEB
					buffer.PlatformApply(device, shaderProgram);
					#endif
                }

                // Early out if this is the last one.
				valid &= (ushort) ~(1 << i);
                if (valid == 0)
                    return;
            }
        }

    }
}
