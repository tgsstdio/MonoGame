using Microsoft.Xna.Framework.Media;

namespace MonoGame.Content.Audio
{
    public interface ISongReader
    {
        ISong Load(AssetIdentifier asset);
    }
}