using System;
using System.IO;
using MonoGame.Content;

namespace HelloMagnesium
{
    class SPIRVShaderContentStreamer : IShaderContentStreamer
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
