﻿using System;
using System.Runtime.InteropServices;
using MonoGame.Graphics;

namespace DrawItemStructSize
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			Console.WriteLine ("Size of DrawItem :" +  Marshal.SizeOf(typeof(DrawItem)) );
			Console.WriteLine ("Size of DepthStencilState :" +  Marshal.SizeOf(typeof(StencilState)) );
			Console.WriteLine ("Size of BlendState :" +  Marshal.SizeOf(typeof(BlendState)) );
			Console.WriteLine ("Size of RasterizerState :" +  Marshal.SizeOf(typeof(RasterizerState)) );
		}
	}
}