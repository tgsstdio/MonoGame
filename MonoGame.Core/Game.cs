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

		public SortingFilteringCollection<IDrawable> Drawables =
			new SortingFilteringCollection<IDrawable>(
				d => d.Visible,
				(d, handler) => d.VisibleChanged += handler,
				(d, handler) => d.VisibleChanged -= handler,
				(d1 ,d2) => Comparer<int>.Default.Compare(d1.DrawOrder, d2.DrawOrder),
				(d, handler) => d.DrawOrderChanged += handler,
				(d, handler) => d.DrawOrderChanged -= handler);

		private static readonly Action<IDrawable, GameTime> DrawAction =
			(drawable, gameTime) => drawable.Draw(gameTime);

		public virtual void Draw(GameTime gameTime)
		{

			Drawables.ForEachFilteredItem(DrawAction, gameTime);
		}

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

