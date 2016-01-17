using System;

namespace MonoGame.Graphics.AZDO
{
	public class ObjectModelSceneNode
	{
		public ObjectModel Model {get;set;}
		public StateGroup[] Overrides {get;set;}
		public DrawItemOffset[] Offsets {get;set;}
	}
}

