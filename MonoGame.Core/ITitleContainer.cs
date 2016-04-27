// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System.IO;

namespace Microsoft.Xna.Framework
{
	public interface ITitleContainer
	{
		string Location { get; }
		Stream OpenStream(string name);
	}
}

