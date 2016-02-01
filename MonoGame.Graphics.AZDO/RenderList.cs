using System;
using System.Collections.Generic;

namespace MonoGame.Graphics.AZDO
{
	public class RenderList : IRenderList
	{
		#region IRenderList implementation

		public DrawItemOffset[] Push (DrawItemCompilerOutput[] items)
		{
			var result = new List<DrawItemOffset>();

			foreach (var i in items)
			{			
				var dest = i.Variant.Destination;
				// TODO : possibly derive destination for variant instead
				if (dest != null)
				{
					var collection = dest.Items;
					var output = new DrawItemOffset{ RenderPassID = dest.InstanceID,  Offset = collection.Count };
					collection.Add (i.Item);
					result.Add (output);
				}
				else
				{
					throw new InvalidOperationException ("Variant expected here");
				}
			}
			return result.ToArray();
		}

		#endregion
	}
}

