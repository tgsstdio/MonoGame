using System;
using System.Runtime.Remoting.Messaging;

namespace Microsoft.Xna.Framework.Storage
{
	public class WinRTStorageDevice : IStorageDevice
	{
		public WinRTStorageDevice ()
		{
		}

		#region IStorageDevice implementation

		#if WINRT
		// Dirty trick to avoid the need to get the delegate from the IAsyncResult (can't be done in WinRT)
		static Delegate showDelegate;
		static Delegate containerDelegate;
		#endif

		private IAsyncResult OpenContainer (string displayName, AsyncCallback callback, object state)
		{

			#if !WINDOWS_PHONE81 && !ANDROID && !IOS && !NETFX_CORE && !WINDOWS_PHONE
			try
			{
				OpenContainerAsynchronous AsynchronousOpen = new OpenContainerAsynchronous(Open);
			#if WINRT
			containerDelegate = AsynchronousOpen;
			#endif
				return AsynchronousOpen.BeginInvoke(displayName, callback, state);
			}
			finally
			{
			}
			#else
			var tcs = new TaskCompletionSource<StorageContainer>(state);
			var task = Task.Run<StorageContainer>(() => Open(displayName));
			task.ContinueWith(t =>
			{
			// Copy the task result into the returned task.
			if (t.IsFaulted)
			tcs.TrySetException(t.Exception.InnerExceptions);
			else if (t.IsCanceled)
			tcs.TrySetCanceled();
			else
			tcs.TrySetResult(t.Result);

			// Invoke the user callback if necessary.
			if (callback != null)
			callback(tcs.Task);
			});
			return tcs.Task;
			#endif
		}

		public IAsyncResult BeginOpenContainer (string displayName, AsyncCallback callback, object state)
		{
			return OpenContainer(displayName, callback, state);
		}

		public IAsyncResult BeginShowSelector (AsyncCallback callback, object state)
		{
			throw new NotImplementedException ();
		}

		public IAsyncResult BeginShowSelector (PlayerIndex player, AsyncCallback callback, object state)
		{
			throw new NotImplementedException ();
		}

		public IAsyncResult BeginShowSelector (int sizeInBytes, int directoryCount, AsyncCallback callback, object state)
		{
			throw new NotImplementedException ();
		}

		public IAsyncResult BeginShowSelector (PlayerIndex player, int sizeInBytes, int directoryCount, AsyncCallback callback, object state)
		{
			throw new NotImplementedException ();
		}

		public IStorageContainer EndOpenContainer (IAsyncResult result)
		{
			throw new NotImplementedException ();
		}

		public IStorageDevice EndShowSelector (IAsyncResult result)
		{
			#if !WINDOWS_PHONE81 && !ANDROID && !IOS && !NETFX_CORE && !WINDOWS_PHONE
			if (!result.IsCompleted)
			{
				// Wait for the WaitHandle to become signaled.
				try
				{
					result.AsyncWaitHandle.WaitOne();
				}
				finally
				{
			#if !WINRT
					result.AsyncWaitHandle.Close();
			#endif
				}
			}
			#if WINRT
			var del = showDelegate;
			showDelegate = null;
			#else
			// Retrieve the delegate.
			AsyncResult asyncResult = (AsyncResult)result;

			var del = asyncResult.AsyncDelegate;
			#endif

			if (del is ShowSelectorAsynchronousShow)
				return (del as ShowSelectorAsynchronousShow).EndInvoke(result);
			else if (del is ShowSelectorAsynchronousShowNoPlayer)
				return (del as ShowSelectorAsynchronousShowNoPlayer).EndInvoke(result);
			else
				throw new ArgumentException("result");
			#else
			try
			{
			return ((Task<StorageDevice>)result).Result;
			}
			catch (AggregateException ex)
			{
			throw;
			}
			#endif
		}

		public string StorageRoot {
			get {
				#if WINRT
				return ApplicationData.Current.LocalFolder.Path; 
				#else
				throw new NotImplementedException ();
				//return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				#endif
			}
		}

		/// <summary>
		/// Returns the amount of free space.
		/// </summary>
		public long FreeSpace { 
			get { 
				// I do not know if the DriveInfo is is implemented on Mac or not
				// thus the try catch
				try {
					#if WINRT
					return long.MaxValue;
					#else
					throw new NotSupportedException();
					//return new DriveInfo(GetDevicePath).AvailableFreeSpace;
					#endif
				}
				catch (Exception) {
					StorageDeviceHelper.Path = StorageRoot;
					return StorageDeviceHelper.FreeSpace;
				}
			} 
		}

		/// <summary>
		/// Returns true if device is connected, false otherwise.
		/// </summary>
		public bool IsConnected { 
			get { 
				// I do not know if the DriveInfo is is implemented on Mac or not
				// thus the try catch
				try {
					#if WINRT
					return true;
					#else
					throw new NotSupportedException();
					//return new DriveInfo(GetDevicePath).IsReady;
					#endif
				}
				catch (Exception) {
					return true;
				}
			} 
		}

		/// <summary>
		/// Returns the total size of device.
		/// </summary>
		public long TotalSpace { 
			get { 

				// I do not know if the DriveInfo is is implemented on Mac or not
				// thus the try catch
				try {
					#if WINRT
					return long.MaxValue;
					#else

					// Not sure if this should be TotalSize or TotalFreeSize
					throw new NotSupportedException();
					//return new DriveInfo(GetDevicePath).TotalSize;
					#endif
				}
				catch (Exception) {
					StorageDeviceHelper.Path = StorageRoot;
					return StorageDeviceHelper.TotalSpace;
				}

			} 
		}

		#endregion
	}
}

