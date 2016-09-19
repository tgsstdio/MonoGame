using System.IO;
using MonoGame.Content;

namespace MonoGame.Graphics
{
    public class GLSLShaderContentStreamer : IShaderContentStreamer
    {
        private IContentStreamer mLoader;
        public GLSLShaderContentStreamer(IContentStreamer loader)
        {
            mLoader = loader;
        }

        public Stream Load(AssetIdentifier assetId)
        {
            return mLoader.LoadContent(assetId, new[] { ".vert", ".frag" });
        }
    }
}
