
namespace MonoGame.Graphics
{
	public class StateGroup
	{		
		public uint? InstanceId { get; set;}
		public DrawState? State { get;set; }
		public byte? SlotIndex { get; set; }
		public byte? TargetIndex {get;set;}

		public byte? EffectIndex { get; set; }
		public System.Int32? Pass {get;set;}

		//public byte? ProgramIndex { get; set;}
		public ushort? ShaderOptions { get; set; }
		public byte? UniformsIndex { get; set;}
		public uint? MarkerIndex { get; set; }
		public uint? ResourceIndex { get; set; }

		public uint? MeshOffset { get; set; }
		public ushort? BufferMask { get; set; }

		public DrawCommand? Command {get;set;}
		public RasterizerState? RasterizerValues {get;set;}
		public StencilState? StencilValues {get;set;}
		public BlendState? BlendValues { get; set;}
		public DepthState? DepthValues { get; set;}

		public DrawItemBitFlags? Flags {get;set;}
	}
}

