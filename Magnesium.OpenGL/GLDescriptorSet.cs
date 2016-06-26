using System;
using OpenTK.Graphics.OpenGL;

namespace Magnesium.OpenGL
{
	public enum GLDescriptorBindingGroup
	{
		Image = 0,
		Buffer = 1
	}

	public class GLDescriptorBinding
	{
		public GLDescriptorBinding (int location, GLImageDescriptor image)
			: this(location)
		{
			Group = GLDescriptorBindingGroup.Image;
			ImageDesc = image;
		}

		public GLDescriptorBinding (int location, GLBufferDescriptor buffer)
			: this(location)
		{
			Group = GLDescriptorBindingGroup.Buffer;
			BufferDesc = buffer;
		}

		private GLDescriptorBinding(int location)
		{
			Location = location;
		}

		public void Destroy ()
		{
			if (Group == GLDescriptorBindingGroup.Image)
			{
				ImageDesc.Destroy ();
				ImageDesc = null;
			}
			else
			{
				BufferDesc.Destroy ();
				BufferDesc = null;
			}
		}

		public int Location { get; private set;}
		public GLDescriptorBindingGroup Group { get; private set; }
		public GLImageDescriptor ImageDesc  { get; private set;}
		public GLBufferDescriptor BufferDesc { get; private set;}
	}

	public class GLImageDescriptor
	{
		public ulong? SamplerHandle { get; set; }

		public void Replace (long handle)
		{
			Destroy ();
			SamplerHandle = (ulong)handle;
		}

		public void Destroy ()
		{
			if (SamplerHandle.HasValue)
			{
				GL.Arb.MakeTextureHandleNonResident (SamplerHandle.Value);
				SamplerHandle = null;
			}
		}
	}

	public class GLBufferDescriptor
	{
		public GLBufferDescriptor ()
		{
			BufferId = 0;
		}

		public int BufferId { get; set; }

		public void Destroy ()
		{
	
		}
	}

	public class GLDescriptorSet : MgDescriptorSet, IEquatable<GLDescriptorSet>
	{
		public int Key { get; private set; }
		public GLDescriptorSet (int key)
		{
			Key = key;
		}

		public GLDescriptorBinding[] Bindings { get; private set; }

		public void Populate(GLDescriptorSetLayout layout)
		{	
			// LET'S USE ARRAY INDEXING
			Bindings = new GLDescriptorBinding[layout.Uniforms.Count];
			int index = 0;
			foreach (var bind in layout.Uniforms)
			{
				if (bind.DescriptorType == MgDescriptorType.SAMPLER)
				{
					Bindings [index] = new GLDescriptorBinding (bind.Location,
						new GLImageDescriptor ());
				}
				else if (bind.DescriptorType == MgDescriptorType.STORAGE_BUFFER)
				{
					Bindings [index] = new GLDescriptorBinding (bind.Location,
						new GLBufferDescriptor ());
				}
				++index;
			}
		}

		public void Destroy ()
		{
			foreach (var image in Bindings)
			{
				image.Destroy ();
			}
			Bindings = null;
		}

		#region IEquatable implementation

		public bool Equals (GLDescriptorSet other)
		{
			return Key == other.Key;
		}

		#endregion

		public override int GetHashCode()
		{ 
			unchecked // Overflow is fine, just wrap
			{
				int hash = 17;
				// Suitable nullity checks etc, of course :)
				//hash = hash * 23 + Pool.GetHashCode();
				hash = hash * 23 + Key.GetHashCode();
				return hash;
			}
		} 

	}
}

