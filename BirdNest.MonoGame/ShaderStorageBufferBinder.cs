using System;
using System.Collections.Generic;

namespace BirdNest.MonoGame
{
	/// <summary>
	/// For converting application info classes to shader data struct when passing data to shader storage buffer.
	/// </summary>
	public class ShaderStorageBufferBinder<TInfo, TData>
		where TInfo : class
		where TData : struct
	{
		public Func<TInfo, TData> GenerateData {get; private set;}
		public IList<TInfo> Sources { get; private set; }
		public ShaderStorageBuffer<TData> Buffer { get; private set; }

		public ShaderStorageBufferBinder (IList<TInfo> sources, ShaderStorageBuffer<TData> buffer, Func<TInfo, TData> generator)
		{
			this.Sources = sources;
			this.Buffer = buffer;
			this.GenerateData = generator;
		}

		public void ConvertSources(IntPtr offset)
		{
			var entries = new List<TData> ();

			foreach (var info in Sources)
			{
				entries.Add (GenerateData (info));
			}

			Buffer.SetData (entries.ToArray (), offset);
		}
	}
}

