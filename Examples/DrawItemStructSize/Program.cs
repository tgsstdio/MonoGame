using System;
using System.Runtime.InteropServices;
using Magnesium.OpenGL;

namespace DrawItemStructSize
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Size of GLCmdBufferDrawItem :" +  Marshal.SizeOf(typeof(GLCmdBufferDrawItem)) );
			//Console.WriteLine ("Size of GLQueueDrawItem :" +  Marshal.SizeOf(typeof(GLQueueDrawItem)) );
			Console.WriteLine ("Size of GLCmdBufferPipelineItem :" +  Marshal.SizeOf(typeof(GLCmdBufferPipelineItem)) );
			Console.WriteLine ("Size of GLQueueRendererStencilState :" +  Marshal.SizeOf(typeof(GLQueueRendererStencilState)) );
			Console.WriteLine ("Size of GLGraphicsPipelineBlendColorAttachmentState :" +  Marshal.SizeOf(typeof(GLGraphicsPipelineBlendColorAttachmentState)) );
			Console.WriteLine ("Size of GLGraphicsPipelineDepthState :" +  Marshal.SizeOf(typeof(GLGraphicsPipelineDepthState)) );
			Console.WriteLine ("Size of GLQueueRasterizerState :" +  Marshal.SizeOf(typeof(GLQueueRendererRasterizerState)) );

			Console.WriteLine ("Size of GLQueueRendererStencilState :" +  Marshal.SizeOf(typeof(GLQueueRendererStencilState)) );
		}
	}
}
