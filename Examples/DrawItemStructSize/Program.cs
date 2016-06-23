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
			Console.WriteLine ("Size of GLQueueDrawItem :" +  Marshal.SizeOf(typeof(GLQueueDrawItem)) );
			Console.WriteLine ("Size of GLCmdPipelineItem :" +  Marshal.SizeOf(typeof(GLCmdBufferPipelineItem)) );
			Console.WriteLine ("Size of GLQueueStencilState :" +  Marshal.SizeOf(typeof(GLQueueStencilState)) );
			Console.WriteLine ("Size of GLQueueBlendState :" +  Marshal.SizeOf(typeof(GLQueueBlendState)) );
			Console.WriteLine ("Size of GLQueueDepthState :" +  Marshal.SizeOf(typeof(GLQueueDepthState)) );
			Console.WriteLine ("Size of GLQueueRasterizerState :" +  Marshal.SizeOf(typeof(GLQueueRasterizerState)) );
		}
	}
}
