// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Graphics
{
	public interface IGraphicsAdapter
	{
		DisplayMode CurrentDisplayMode {
			get;
		}

		DisplayModeCollection SupportedDisplayModes { get; }
		bool IsWideScreen {	get; }
	}
}
