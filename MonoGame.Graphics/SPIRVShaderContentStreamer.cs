using System.IO;
using MonoGame.Content;

namespace MonoGame.Graphics
{
    public class SPIRVShaderContentStreamer : IShaderContentStreamer
    {
        private readonly IContentStreamer mLoader;
        public SPIRVShaderContentStreamer(IContentStreamer loader)
        {
            mLoader = loader;
        }

        public Stream Load(AssetIdentifier assetId)
        {
            return mLoader.LoadContent(assetId, new[] {".spv"});
        }
    }
}
