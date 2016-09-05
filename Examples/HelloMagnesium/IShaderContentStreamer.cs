using MonoGame.Content;
using System.IO;

namespace HelloMagnesium
{
    public interface IShaderContentStreamer
    {
        Stream Load(AssetIdentifier assetId);
    }
}