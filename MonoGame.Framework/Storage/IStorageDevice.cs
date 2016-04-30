#region License
/*
Microsoft Public License (Ms-PL)
MonoGame - Copyright © 2009 The MonoGame Team

All rights reserved.

This license governs use of the accompanying software. If you use the software, you accept this license. If you do not
accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under 
U.S. copyright law.

A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution 
notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including 
a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object 
code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees
or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent
permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular
purpose and non-infringement.
*/
using System;


#endregion License

namespace Microsoft.Xna.Framework.Storage
{
	// The delegate must have the same signature as the method
	// it will call asynchronously.
	public delegate IStorageDevice ShowSelectorAsynchronousShow (PlayerIndex player, int sizeInBytes, int directoryCount);
	// The MonoTouch AOT cannot deal with nullable types in a delegate (or
	// at least not the straightforward implementation), so we define two
	// delegate types.
	public delegate IStorageDevice ShowSelectorAsynchronousShowNoPlayer (int sizeInBytes, int directoryCount);

	// The delegate must have the same signature as the method
	// it will call asynchronously.
	public delegate IStorageContainer OpenContainerAsynchronous (string displayName);

	public interface IStorageDevice
	{
		string StorageRoot  { get; }

		/// <summary>
		/// Returns the amount of free space.
		/// </summary>
		long FreeSpace { 
			get;
		}

		/// <summary>
		/// Returns true if device is connected, false otherwise.
		/// </summary>
		bool IsConnected { 
			get;
		}

		/// <summary>
		/// Returns the total size of device.
		/// </summary>
		long TotalSpace { 
			get;
		}

		// Summary:
		//     Begins the process for opening a StorageContainer containing any files for
		//     the specified title.
		//
		// Parameters:
		//   displayName:
		//     A constant human-readable string that names the file.
		//
		//   callback:
		//     An AsyncCallback that represents the method called when the operation is
		//     complete.
		//
		//   state:
		//     A user-created object used to uniquely identify the request, or null.
		IAsyncResult BeginOpenContainer (string displayName, AsyncCallback callback, object state);

		//
		// Summary:
		//     Begins the process for displaying the storage device selector user interface,
		//     and for specifying a callback implemented when the player chooses a device.
		//     Reference page contains links to related code samples.
		//
		// Parameters:
		//   callback:
		//     An AsyncCallback that represents the method called when the player chooses
		//     a device.
		//
		//   state:
		//     A user-created object used to uniquely identify the request, or null.
		IAsyncResult BeginShowSelector (AsyncCallback callback, object state);

		//
		// Summary:
		//     Begins the process for displaying the storage device selector user interface;
		//     specifies the callback implemented when the player chooses a device. Reference
		//     page contains links to related code samples.
		//
		// Parameters:
		//   player:
		//     The PlayerIndex that represents the player who requested the save operation.
		//     On Windows, the only valid option is PlayerIndex.One.
		//
		//   callback:
		//     An AsyncCallback that represents the method called when the player chooses
		//     a device.
		//
		//   state:
		//     A user-created object used to uniquely identify the request, or null.
		IAsyncResult BeginShowSelector (PlayerIndex player, AsyncCallback callback, object state);

		//
		// Summary:
		//     Begins the process for displaying the storage device selector user interface,
		//     and for specifying the size of the data to be written to the storage device
		//     and the callback implemented when the player chooses a device. Reference
		//     page contains links to related code samples.
		//
		// Parameters:
		//   sizeInBytes:
		//     The size, in bytes, of data to write to the storage device.
		//
		//   directoryCount:
		//     The number of directories to write to the storage device.
		//
		//   callback:
		//     An AsyncCallback that represents the method called when the player chooses
		//     a device.
		//
		//   state:
		//     A user-created object used to uniquely identify the request, or null.
		IAsyncResult BeginShowSelector (int sizeInBytes, int directoryCount, AsyncCallback callback, object state);

		//
		// Summary:
		//     Begins the process for displaying the storage device selector user interface,
		//     for specifying the player who requested the save operation, for setting the
		//     size of data to be written to the storage device, and for naming the callback
		//     implemented when the player chooses a device. Reference page contains links
		//     to related code samples.
		//
		// Parameters:
		//   player:
		//     The PlayerIndex that represents the player who requested the save operation.
		//     On Windows, the only valid option is PlayerIndex.One.
		//
		//   sizeInBytes:
		//     The size, in bytes, of the data to write to the storage device.
		//
		//   directoryCount:
		//     The number of directories to write to the storage device.
		//
		//   callback:
		//     An AsyncCallback that represents the method called when the player chooses
		//     a device.
		//
		//   state:
		//     A user-created object used to uniquely identify the request, or null.
		IAsyncResult BeginShowSelector (PlayerIndex player, int sizeInBytes, int directoryCount, AsyncCallback callback, object state);

		//
		// Summary:
		//     Ends the process for opening a StorageContainer.
		//
		// Parameters:
		//   result:
		//     The IAsyncResult returned from BeginOpenContainer.
		IStorageContainer EndOpenContainer (IAsyncResult result);

		//
		// Summary:
		//     Ends the display of the storage selector user interface. Reference page contains
		//     links to related code samples.
		//
		// Parameters:
		//   result:
		//     The IAsyncResult returned from BeginShowSelector.
		IStorageDevice EndShowSelector (IAsyncResult result);
	}    
}
