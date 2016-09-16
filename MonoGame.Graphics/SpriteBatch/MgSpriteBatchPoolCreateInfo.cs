using Magnesium;
using System;

namespace MonoGame.Graphics
{
    public class MgSpriteBatchPoolCreateInfo
    {
        public uint DescriptorSetCount { get; set; }
        public IMgDescriptorSetLayout SetLayout { get; set; }
    }
}
