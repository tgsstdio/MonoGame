using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework
{
	public abstract class Game
	{
		public SortingFilteringCollection<IUpdateable> Updateables =
			new SortingFilteringCollection<IUpdateable>(
				u => u.Enabled,
				(u, handler) => u.EnabledChanged += handler,
				(u, handler) => u.EnabledChanged -= handler,
				(u1, u2) => Comparer<int>.Default.Compare(u1.UpdateOrder, u2.UpdateOrder),
				(u, handler) => u.UpdateOrderChanged += handler,
				(u, handler) => u.UpdateOrderChanged -= handler);

		private static readonly Action<IUpdateable, GameTime> UpdateAction =
			(updateable, gameTime) => updateable.Update(gameTime);

		public virtual void Update(GameTime gameTime)
		{
			Updateables.ForEachFilteredItem(UpdateAction, gameTime);
		}

		public virtual bool BeginDraw() { return true; }
		public virtual void EndDraw() { }

		public virtual void BeginRun() { }
		public virtual void EndRun() { }

		public virtual void LoadContent() { }
		public virtual void UnloadContent() { }

		public virtual void Initialize()
		{

		}
	}
}

