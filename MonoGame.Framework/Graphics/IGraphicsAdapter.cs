// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#if MONOMAC
using MonoMac.AppKit;
using MonoMac.Foundation;

#elif IOS
using UIKit;

#elif ANDROID
using Android.Views;
using Android.Runtime;
#endif

namespace Microsoft.Xna.Framework.Graphics
{
	public interface IGraphicsAdapter
	{
	}

}
