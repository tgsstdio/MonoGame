using Magnesium;

namespace MgSpriteBatchReplacement
{
    internal class MgBatchedDrawCall
    {
        public MgBatchedDrawCall()
        {
        }

        public IMgPipeline Pipeline { get; internal set; }
        public uint PipelineIndex { get; internal set; }
    }
}