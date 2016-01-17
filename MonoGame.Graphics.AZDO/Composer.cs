using System;
using System.Collections.Generic;

namespace MonoGame.Graphics.AZDO
{
	public class Composer
	{
		private readonly IRenderSlotCache mSlots;
		private readonly IDrawItemCompiler mCompiler;
		private readonly IRenderList mDestination;
		public Composer (IDrawItemCompiler compiler, IRenderSlotCache slots, IRenderList destination)
		{
			mSlots = slots;
			mCompiler = compiler;
			mDestination = destination;
		}

		static void InsertNewGroup (IDictionary<uint, Stack<StateGroup>> lookup, StateGroup group, StateGroup defaultGroup)
		{
			Stack<StateGroup> found;
			if (group.InstanceId.HasValue)
			{
				if (lookup.TryGetValue (group.InstanceId.Value, out found))
				{
					found.Push (group);
				}
				else
				{
					found = new Stack<StateGroup> ();
					found.Push (defaultGroup);
					found.Push (group);
					lookup.Add (group.InstanceId.Value, found);
				}
			}
		}

		public void Register(ObjectModelSceneNode node, StateGroup defaultGroup)
		{
			var lookup = new Dictionary<uint, Stack<StateGroup>> ();

			foreach (var group in node.Model.DefaultStates)
			{
				InsertNewGroup (lookup, group, defaultGroup);
			}

			foreach (var group in node.Overrides)
			{
				InsertNewGroup (lookup, group, defaultGroup);
			}

			// add draw items to the draw item buffer

			var offsets = new List<DrawItemOffset> ();
			foreach (var v in lookup.Values)
			{
				var items = mCompiler.Compile (v.ToArray ());

				DrawItemOffset output;
				if (!mDestination.Push (items, out output))
				{
					throw new InvalidOperationException ();
				}
			}
			node.Offsets = offsets.ToArray ();
		}
	}
}

