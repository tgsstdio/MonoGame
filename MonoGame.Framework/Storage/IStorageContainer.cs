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
using System.IO;
using System;


#endregion License

namespace Microsoft.Xna.Framework.Storage
{
	public enum StorageFileMode
	{
		CreateNew = 1,
		Create = 2,
		Open = 3,
		OpenOrCreate = 4,
		Truncate = 5,
		Append = 6,
	}

	public enum StorageFileAccess
	{
		Read = 1,
		Write = 2,
		ReadWrite = 3,
	}

	public enum StorageFileShare
	{
		None = 0,
		Read = 1,
		Write = 2,
		ReadWrite = 3,
		Delete = 4,
		Inheritable = 0x10,
	}

	//	Implementation on Windows
	//
	//	User storage is in the My Documents folder of the user who is currently logged in, in the SavedGames folder.
	//	A subfolder is created for each game according to the titleName passed to the BeginOpenContainer method.
	//	When no PlayerIndex is specified, content is saved in the AllPlayers folder. When a PlayerIndex is specified,
	//	the content is saved in the Player1, Player2, Player3, or Player4 folder, depending on which PlayerIndex
	//	was passed to BeginShowSelector.

	public interface IStorageContainer : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Microsoft.Xna.Framework.Storage.StorageContainer"/> class.
		/// </summary>
		/// <param name='name'> name.</param>
		/// <param name='playerIndex'>The player index of the player to save the data.</param>
		void Initialize(string name, PlayerIndex? playerIndex);

		/// <summary>
		/// Returns display name of the title.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Gets a bool value indicating whether the instance has been disposed.
		/// </summary>
		bool IsDisposed { get; }

		/// <summary>
		/// Creates a new directory in the storage-container.
		/// </summary>
		/// <param name="directory">Relative path of the directory to be created.</param>
		void CreateDirectory (string directory);

		/// <summary>
		/// Creates a file in the storage-container.
		/// </summary>
		/// <param name="file">Relative path of the file to be created.</param>
		/// <returns>Returns <see cref="Stream"/> for the created file.</returns>
		Stream CreateFile (string file);

		/// <summary>
		/// Deletes specified directory for the storage-container.
		/// </summary>
		/// <param name="directory">The relative path of the directory to be deleted.</param>
		void DeleteDirectory (string directory);

		/// <summary>
		/// Deletes a file from the storage-container.
		/// </summary>
		/// <param name="file">The relative path of the file to be deleted.</param>
		void DeleteFile (string file);

		/// <summary>
		/// Returns true if specified path exists in the storage-container, false otherwise.
		/// </summary>
		/// <param name="directory">The relative path of directory to query for.</param>
		/// <returns>True if queried directory exists, false otherwise.</returns>
		bool DirectoryExists (string directory);

		/// <summary>
		/// Returns true if the specified file exists in the storage-container, false otherwise.
		/// </summary>
		/// <param name="file">The relative path of file to query for.</param>
		/// <returns>True if queried file exists, false otherwise.</returns>
		bool FileExists (string file);

		/// <summary>
		/// Returns list of directory names in the storage-container.
		/// </summary>
		/// <returns>List of directory names.</returns>
		string[] GetDirectoryNames ();

		/// <summary>
		/// Returns list of file names in the storage-container.
		/// </summary>
		/// <returns>List of file names.</returns>
		string[] GetFileNames ();

		/// <summary>
		/// Returns list of file names with given search pattern.
		/// </summary>
		/// <param name="searchPattern">A search pattern that supports single-character ("?") and multicharacter ("*") wildcards.</param>
		/// <returns>List of matched file names.</returns>
		string[] GetFileNames (string searchPattern);

		/// <summary>
		/// Opens a file contained in storage-container.
		/// </summary>
		/// <param name="file">Relative path of the file.</param>
		/// <param name="fileMode"><see cref="FileMode"/> that specifies how the file is opened.</param>
		/// <returns><see cref="Stream"/> object for the opened file.</returns>
		Stream OpenFile (string file, StorageFileMode fileMode);

		/// <summary>
		/// Opens a file contained in storage-container.
		/// </summary>
		/// <param name="file">Relative path of the file.</param>
		/// <param name="fileMode"><see cref="FileMode"/> that specifies how the file is opened.</param>
		/// <param name="fileAccess"><see cref="FileAccess"/> that specifies access mode.</param>
		/// <returns><see cref="Stream"/> object for the opened file.</returns>
		Stream OpenFile (string file, StorageFileMode fileMode, StorageFileAccess fileAccess);

		/// <summary>
		/// Opens a file contained in storage-container.
		/// </summary>
		/// <param name="file">Relative path of the file.</param>
		/// <param name="fileMode"><see cref="FileMode"/> that specifies how the file is opened.</param>
		/// <param name="fileAccess"><see cref="FileAccess"/> that specifies access mode.</param>
		/// <param name="fileShare">A bitwise combination of <see cref="FileShare"/> enumeration values that specifies access modes for other stream objects.</param>
		/// <returns><see cref="Stream"/> object for the opened file.</returns>
		Stream OpenFile (string file, StorageFileMode fileMode, StorageFileAccess fileAccess, StorageFileShare fileShare);
	}
}
