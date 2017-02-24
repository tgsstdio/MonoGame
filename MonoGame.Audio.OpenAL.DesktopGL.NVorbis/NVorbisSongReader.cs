using Microsoft.Xna.Framework.Media;
using MonoGame.Content;

namespace MonoGame.Content.Audio.OpenAL.NVorbis
{
    public class NVorbisSongReader : ISongReader
    {
        private readonly IContentStreamer mContentStreamer;
        private IMediaPlayer mMediaPlayer;
        public NVorbisSongReader(IContentStreamer contentStreamer, IMediaPlayer player)
        {
            mMediaPlayer = player;
            mContentStreamer = contentStreamer;
        }

        public ISong Load(AssetIdentifier asset)
        {
            // TODO : maybe
            return new NVorbisSong(mContentStreamer, ".wma", asset, mMediaPlayer);
        }
    }
}
