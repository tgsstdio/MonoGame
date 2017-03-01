using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGame.Content
{
    public class AssetNotFoundException : Exception
    {      
        public AssetNotFoundException(AssetIdentifier assetId)
            : base("Asset not found : " + assetId.ToString())
        {
            AssetId = assetId;
        }

        public AssetIdentifier AssetId { get; private set; }
    }
}
