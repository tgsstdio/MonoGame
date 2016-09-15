using MonoGame.Content;
using System.IO;

namespace MonoGame.Graphics
{
    public interface IShaderContentStreamer
    {
        Stream Load(AssetIdentifier assetId);
    }
}