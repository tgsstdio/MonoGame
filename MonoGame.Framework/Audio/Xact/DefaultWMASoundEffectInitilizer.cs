using System;
using System.IO;
using MonoGame.Core;

namespace Microsoft.Xna.Framework.Audio
{
	public class DefaultWMASoundEffectInitilizer : IWMASoundEffectInitializer
	{
		private readonly ISoundEffectImplementationFactory mFactory;
		public DefaultWMASoundEffectInitilizer (ISoundEffectImplementationFactory factory)
		{
			mFactory = factory;
		}

		#region IWMASoundEffectInitializer implementation

		public SoundEffect LoadEffect (byte[] audiodata, bool isWma, bool isM4a)
		{
			//hack - NSSound can't play non-wav from data, we have to give a filename
			string filename = Path.GetTempFileName();
			if (isWma) {
				filename = filename.Replace(".tmp", ".wma");
			} else if (isM4a) {
				filename = filename.Replace(".tmp", ".m4a");
			}
			using (var audioFile = File.Create(filename))
			{
				audioFile.Write(audiodata, 0, audiodata.Length);
				audioFile.Seek(0, SeekOrigin.Begin);

				var sfx = new SoundEffect (mFactory.Create (), audioFile);
				return sfx;
			}
		}

		#endregion
	}
}

