// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Diagnostics;
#if WINRT
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
#endif
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


namespace Microsoft.Xna.Framework
{
    public class GameBackbone : IGameBackbone
    {
        private GameComponentCollection _components;
        //private GameServiceContainer _services;
        private IContentManager _content;
        internal GamePlatform Platform;

        private SortingFilteringCollection<IDrawable> _drawables =
            new SortingFilteringCollection<IDrawable>(
                d => d.Visible,
                (d, handler) => d.VisibleChanged += handler,
                (d, handler) => d.VisibleChanged -= handler,
                (d1 ,d2) => Comparer<int>.Default.Compare(d1.DrawOrder, d2.DrawOrder),
                (d, handler) => d.DrawOrderChanged += handler,
                (d, handler) => d.DrawOrderChanged -= handler);

        //private IGraphicsDeviceManager _graphicsDeviceManager;
      //  private IGraphicsDeviceService _graphicsDeviceService;

        private bool _initialized = false;
        private bool _isFixedTimeStep = true;

        private TimeSpan _targetElapsedTime = TimeSpan.FromTicks(166667); // 60fps
        private TimeSpan _inactiveSleepTime = TimeSpan.FromSeconds(0.02);

        private TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds(500);


    //    private bool _suppressDraw;
        
		private IContentTypeReaderManager ContentTypeReaderManager;
		private IPlatformActivator mPlatformActivator;
		private IPresentationParameters mPresentationParameters;
		private IDrawSuppressor mSuppressor;
		private ISoundEffectInstancePool mPool;
		public GameBackbone(
			Game vg,
			GamePlatform platform,
			IContentManager content,
			IContentTypeReaderManager ctrm,
			IPlatformActivator activator,
			IPresentationParameters presentation,
			IDrawSuppressor suppression,
			ISoundEffectInstancePool pool)
        {
			mSuppressor = suppression;
			mPlatformActivator = activator;
			mPresentationParameters = presentation;
			mPool = pool;
			_instance = vg;
			ContentTypeReaderManager = ctrm;

            LaunchParameters = new LaunchParameters();
            //_services = new GameServiceContainer();
            _components = new GameComponentCollection();
           // _content = new ContentManager(_services);
			_content = content;

			SetupPlatform (platform);
        }

        ~GameBackbone()
        {
            Dispose(false);
        }

		[System.Diagnostics.Conditional("DEBUG")]
		internal void Log(string Message)
		{
			if (Platform != null) Platform.Log(Message);
		}

        #region IDisposable Implementation

        private bool _isDisposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Raise(Disposed, EventArgs.Empty);
        }

		protected void SetupPlatform (GamePlatform platform)
		{
			//Platform = GamePlatform.Create(this);
			Platform = platform;

			mPlatformActivator.AddActivatedHandler (OnActivated);
			mPlatformActivator.AddDeactivatedHandler (OnDeactivated);

			//Platform.Activated += OnActivated;
			//Platform.Deactivated += OnDeactivated;
			// _services.AddService(typeof(GamePlatform), Platform);

			#if WINDOWS_STOREAPP && !WINDOWS_PHONE81
			Platform.ViewStateChanged += Platform_ApplicationViewChanged;
			#endif
		}

		protected void ReleasePlatform ()
		{
			if (Platform != null)
			{
				mPlatformActivator.RemoveActivatedHandler (OnActivated);
				mPlatformActivator.RemoveDeactivatedHandler (OnDeactivated);
				//Platform.Activated -= OnActivated;
				//Platform.Deactivated -= OnDeactivated;
				// _services.RemoveService(typeof(GamePlatform));
				#if WINDOWS_STOREAPP && !WINDOWS_PHONE81
				                        Platform.ViewStateChanged -= Platform_ApplicationViewChanged;
#endif
				//Platform.Dispose ();
				Platform = null;
			}
		}

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Dispose loaded game components
                    for (int i = 0; i < _components.Count; i++)
                    {
                        var disposable = _components[i] as IDisposable;
                        if (disposable != null)
                            disposable.Dispose();
                    }
                    _components = null;

                    if (_content != null)
                    {
                        _content.Dispose();
                        _content = null;
                    }

					// RELY ON USER TO DO IT
                    //_graphicsDeviceManager.Dispose();

                    ReleasePlatform ();

                    ContentTypeReaderManager.ClearTypeCreators();

                    SoundEffect.PlatformShutdown();
                }
#if ANDROID
                Activity = null;
#endif
                _isDisposed = true;
                _instance = null;
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private void AssertNotDisposed()
        {
            if (_isDisposed)
            {
                string name = GetType().Name;
                throw new ObjectDisposedException(
                    name, string.Format("The {0} object was used after being Disposed.", name));
            }
        }

        #endregion IDisposable Implementation

        #region Properties

#if ANDROID
        [CLSCompliant(false)]
        public static AndroidGameActivity Activity { get; internal set; }
#endif
		private Game _instance = null;
		internal Game Instance { get { return _instance; } }

        public LaunchParameters LaunchParameters { get; private set; }

        public GameComponentCollection Components
        {
            get { return _components; }
        }

        public TimeSpan InactiveSleepTime
        {
            get { return _inactiveSleepTime; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("The time must be positive.", default(Exception));

                _inactiveSleepTime = value;
            }
        }

        /// <summary>
        /// The maximum amount of time we will frameskip over and only perform Update calls with no Draw calls.
        /// MonoGame extension.
        /// </summary>
        public TimeSpan MaxElapsedTime
        {
            get { return _maxElapsedTime; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("The time must be positive.", default(Exception));
                if (value < _targetElapsedTime)
                    throw new ArgumentOutOfRangeException("The time must be at least TargetElapsedTime", default(Exception));

                _maxElapsedTime = value;
            }
        }

        public bool IsActive
        {
			get { return mPlatformActivator.IsActive; }
        }

        public bool IsMouseVisible
        {
            get { return Platform.IsMouseVisible; }
            set { Platform.IsMouseVisible = value; }
        }

        public TimeSpan TargetElapsedTime
        {
            get { return _targetElapsedTime; }
            set
            {
                // Give GamePlatform implementations an opportunity to override
                // the new value.
                value = Platform.TargetElapsedTimeChanging(value);

                if (value <= TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(
                        "The time must be positive and non-zero.", default(Exception));

                if (value != _targetElapsedTime)
                {
                    _targetElapsedTime = value;
                    Platform.TargetElapsedTimeChanged();
                }
            }
        }

        public bool IsFixedTimeStep
        {
            get { return _isFixedTimeStep; }
            set { _isFixedTimeStep = value; }
        }

//        public GameServiceContainer Services {
//            get { return _services; }
//        }

        public IContentManager Content
        {
            get { return _content; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _content = value;
            }
        }

//        public IGraphicsDevice GraphicsDevice
//        {
//            get
//            {
//                if (_graphicsDeviceService == null)
//                {
////                    _graphicsDeviceService = (IGraphicsDeviceService)
////                        Services.GetService(typeof(IGraphicsDeviceService));
//
//                    if (_graphicsDeviceService == null)
//                        throw new InvalidOperationException("No Graphics Device Service");
//                }
//                return _graphicsDeviceService.GraphicsDevice;
//            }
//        }

        [CLSCompliant(false)]
        public GameWindow Window
        {
            get { return Platform.Window; }
        }

        #endregion Properties

        #region Internal Properties

        // FIXME: Internal members should be eliminated.
        // Currently Game.Initialized is used by the Mac game window class to
        // determine whether to raise DeviceResetting and DeviceReset on
        // GraphicsDeviceManager.
        internal bool Initialized
        {
            get { return _initialized; }
        }

        #endregion Internal Properties

        #region Events

        public event EventHandler<EventArgs> Activated;
        public event EventHandler<EventArgs> Deactivated;
        public event EventHandler<EventArgs> Disposed;
        public event EventHandler<EventArgs> Exiting;

#if WINDOWS_STOREAPP && !WINDOWS_PHONE81
        [CLSCompliant(false)]
        public event EventHandler<ViewStateChangedEventArgs> ApplicationViewChanged;
#endif

#if WINRT
        [CLSCompliant(false)]
        public ApplicationExecutionState PreviousExecutionState { get; internal set; }
#endif

        #endregion

        #region Public Methods

#if IOS || WINDOWS_STOREAPP && !WINDOWS_PHONE81
        [Obsolete("This platform's policy does not allow programmatically closing.", true)]
#endif
        public void Exit()
        {
			// TODO : refactor this
			// ALSO THIS CODE IS CONSIDER Obsolete on other platforms
            Platform.Exit();
         //   _suppressDraw = true;
			mSuppressor.SuppressDraw = true;
        }

        public void ResetElapsedTime()
        {
            Platform.ResetElapsedTime();
            _gameTimer.Reset();
            _gameTimer.Start();
            _accumulatedElapsedTime = TimeSpan.Zero;
            _gameTime.ElapsedGameTime = TimeSpan.Zero;
            _previousTicks = 0L;
        }

        public void SuppressDraw()
        {
     //       _suppressDraw = true;
			mSuppressor.SuppressDraw = true;
        }
        
        public void RunOneFrame()
        {
            if (Platform == null)
                return;

            if (!Platform.BeforeRun())
                return;

            if (!_initialized) {
                DoInitialize ();
                _gameTimer = Stopwatch.StartNew();
                _initialized = true;
            }

            Instance.BeginRun();            

            //Not quite right..
            Tick ();

            Instance.EndRun ();

        }

        public void Run()
        {
            Run(Platform.DefaultRunBehavior);
        }

        public void Run(GameRunBehavior runBehavior)
        {
            AssertNotDisposed();
            if (!Platform.BeforeRun())
            {
                Instance.BeginRun();
                _gameTimer = Stopwatch.StartNew();
                return;
            }

            if (!_initialized) {
                DoInitialize ();
                _initialized = true;
            }

            Instance.BeginRun();
            _gameTimer = Stopwatch.StartNew();
            switch (runBehavior)
            {
            case GameRunBehavior.Asynchronous:
                Platform.AsyncRunLoopEnded += Platform_AsyncRunLoopEnded;
                Platform.StartRunLoop();
                break;
            case GameRunBehavior.Synchronous:
				Platform.RunLoop(this.Tick);
                Instance.EndRun();
				DoExiting();
                break;
            default:
                throw new ArgumentException(string.Format(
                    "Handling for the run behavior {0} is not implemented.", runBehavior));
            }
        }

        private TimeSpan _accumulatedElapsedTime;
        private readonly GameTime _gameTime = new GameTime();
        private Stopwatch _gameTimer;
        private long _previousTicks = 0;
        private int _updateFrameLag;

        public void Tick()
        {
            // NOTE: This code is very sensitive and can break very badly
            // with even what looks like a safe change.  Be sure to test 
            // any change fully in both the fixed and variable timestep 
            // modes across multiple devices and platforms.

        RetryTick:

            // Advance the accumulated elapsed time.
            var currentTicks = _gameTimer.Elapsed.Ticks;
            _accumulatedElapsedTime += TimeSpan.FromTicks(currentTicks - _previousTicks);
            _previousTicks = currentTicks;

            // If we're in the fixed timestep mode and not enough time has elapsed
            // to perform an update we sleep off the the remaining time to save battery
            // life and/or release CPU time to other threads and processes.
            if (IsFixedTimeStep && _accumulatedElapsedTime < TargetElapsedTime)
            {
                var sleepTime = (int)(TargetElapsedTime - _accumulatedElapsedTime).TotalMilliseconds;

                // NOTE: While sleep can be inaccurate in general it is 
                // accurate enough for frame limiting purposes if some
                // fluctuation is an acceptable result.
#if WINRT
                Task.Delay(sleepTime).Wait();
#else
                System.Threading.Thread.Sleep(sleepTime);
#endif
                goto RetryTick;
            }

            // Do not allow any update to take longer than our maximum.
            if (_accumulatedElapsedTime > _maxElapsedTime)
                _accumulatedElapsedTime = _maxElapsedTime;

            if (IsFixedTimeStep)
            {
                _gameTime.ElapsedGameTime = TargetElapsedTime;
                var stepCount = 0;

                // Perform as many full fixed length time steps as we can.
                while (_accumulatedElapsedTime >= TargetElapsedTime)
                {
                    _gameTime.TotalGameTime += TargetElapsedTime;
                    _accumulatedElapsedTime -= TargetElapsedTime;
                    ++stepCount;

                    DoUpdate(_gameTime);
                }

                //Every update after the first accumulates lag
                _updateFrameLag += Math.Max(0, stepCount - 1);

                //If we think we are running slowly, wait until the lag clears before resetting it
                if (_gameTime.IsRunningSlowly)
                {
                    if (_updateFrameLag == 0)
                        _gameTime.IsRunningSlowly = false;
                }
                else if (_updateFrameLag >= 5)
                {
                    //If we lag more than 5 frames, start thinking we are running slowly
                    _gameTime.IsRunningSlowly = true;
                }

                //Every time we just do one update and one draw, then we are not running slowly, so decrease the lag
                if (stepCount == 1 && _updateFrameLag > 0)
                    _updateFrameLag--;

                // Draw needs to know the total elapsed time
                // that occured for the fixed length updates.
                _gameTime.ElapsedGameTime = TimeSpan.FromTicks(TargetElapsedTime.Ticks * stepCount);
            }
            else
            {
                // Perform a single variable length update.
                _gameTime.ElapsedGameTime = _accumulatedElapsedTime;
                _gameTime.TotalGameTime += _accumulatedElapsedTime;
                _accumulatedElapsedTime = TimeSpan.Zero;

                DoUpdate(_gameTime);
            }

            // Draw unless the update suppressed it.
			if (mSuppressor.SuppressDraw)
			{
				//_suppressDraw = false;
				mSuppressor.SuppressDraw = false;
			}
            else
            {
                DoDraw(_gameTime);
            }
        }

        #endregion

        #region Protected Methods

//        protected virtual bool BeginDraw() { return true; }
//        protected virtual void EndDraw()
//        {
//            Platform.Present();
//        }
//
//        protected virtual void BeginRun() { }
//        protected virtual void EndRun() { }
//
//        protected virtual void LoadContent() { }
//        protected virtual void UnloadContent() { }

        protected virtual void Initialize()
        {
			Instance.Initialize ();
            // TODO: We shouldn't need to do this here.
            applyChanges();

            // According to the information given on MSDN (see link below), all
            // GameComponents in Components at the time Initialize() is called
            // are initialized.
            // http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.game.initialize.aspx
            // Initialize all existing components
            InitializeExistingComponents();

//            _graphicsDeviceService = (IGraphicsDeviceService)
//                Services.GetService(typeof(IGraphicsDeviceService));
//
//            // FIXME: If this test fails, is LoadContent ever called?  This
//            //        seems like a condition that warrants an exception more
//            //        than a silent failure.
//            if (_graphicsDeviceService != null &&
//                _graphicsDeviceService.GraphicsDevice != null)
//            {
                Instance.LoadContent();
        //    }
        }

        private static readonly Action<IDrawable, GameTime> DrawAction =
            (drawable, gameTime) => drawable.Draw(gameTime);

        protected virtual void Draw(GameTime gameTime)
        {

            _drawables.ForEachFilteredItem(DrawAction, gameTime);
        }

        protected virtual void OnExiting(object sender, EventArgs args)
        {
            Raise(Exiting, args);
        }
		
		protected virtual void OnActivated (object sender, EventArgs args)
		{
			AssertNotDisposed();
			Raise(Activated, args);
		}
		
		protected virtual void OnDeactivated (object sender, EventArgs args)
		{
			AssertNotDisposed();
			Raise(Deactivated, args);
		}

        #endregion Protected Methods

        #region Event Handlers

        private void Components_ComponentAdded(
            object sender, GameComponentCollectionEventArgs e)
        {
            // Since we only subscribe to ComponentAdded after the graphics
            // devices are set up, it is safe to just blindly call Initialize.
            e.GameComponent.Initialize();
            CategorizeComponent(e.GameComponent);
        }

        private void Components_ComponentRemoved(
            object sender, GameComponentCollectionEventArgs e)
        {
            DecategorizeComponent(e.GameComponent);
        }

        private void Platform_AsyncRunLoopEnded(object sender, EventArgs e)
        {
            AssertNotDisposed();

            var platform = (GamePlatform)sender;
            platform.AsyncRunLoopEnded -= Platform_AsyncRunLoopEnded;
			Instance.EndRun();
			DoExiting();
        }

#if WINDOWS_STOREAPP && !WINDOWS_PHONE81
        private void Platform_ApplicationViewChanged(object sender, ViewStateChangedEventArgs e)
        {
            AssertNotDisposed();
            Raise(ApplicationViewChanged, e);
        }
#endif

        #endregion Event Handlers

        #region Internal Methods

        // FIXME: We should work toward eliminating internal methods.  They
        //        break entirely the possibility that additional platforms could
        //        be added by third parties without changing MonoGame itself.

        internal void applyChanges()
        {
			Platform.BeginScreenDeviceChange(mPresentationParameters.IsFullScreen);

#if !(WINDOWS && DIRECTX)

			if (mPresentationParameters.IsFullScreen)
                Platform.EnterFullScreen();
            else
                Platform.ExitFullScreen();
#endif
            var viewport = new Viewport(0, 0,
				mPresentationParameters.BackBufferWidth,
				mPresentationParameters.BackBufferHeight);

           // TODO : GraphicsDevice.Viewport = viewport;
			Platform.EndScreenDeviceChange(string.Empty, viewport.Width, viewport.Height);
        }

        internal void DoUpdate(GameTime gameTime)
        {
            AssertNotDisposed();
            if (Platform.BeforeUpdate(gameTime))
            {
                // Once per frame, we need to check currently 
                // playing sounds to see if they've stopped,
                // and return them back to the pool if so.
				mPool.Update();

				Instance.Update(gameTime);

                //The TouchPanel needs to know the time for when touches arrive
                TouchPanelState.CurrentTimestamp = gameTime.TotalGameTime;
            }
        }

		internal void DoEndDraw()
		{
			Platform.Present();
			// NOT SURE ABOUT THE ORDER
			Instance.EndDraw ();
		}

        internal void DoDraw(GameTime gameTime)
        {
            AssertNotDisposed();
            // Draw and EndDraw should not be called if BeginDraw returns false.
            // http://stackoverflow.com/questions/4054936/manual-control-over-when-to-redraw-the-screen/4057180#4057180
            // http://stackoverflow.com/questions/4235439/xna-3-1-to-4-0-requires-constant-redraw-or-will-display-a-purple-screen
            if (Platform.BeforeDraw(gameTime) && Instance.BeginDraw())
            {
                Draw(gameTime);
                DoEndDraw();
            }
        }

        internal void DoInitialize()
        {
            AssertNotDisposed();
            Platform.BeforeInitialize();
            Initialize();

            // We need to do this after virtual Initialize(...) is called.
            // 1. Categorize components into IUpdateable and IDrawable lists.
            // 2. Subscribe to Added/Removed events to keep the categorized
            //    lists synced and to Initialize future components as they are
            //    added.            
            CategorizeComponents();
            _components.ComponentAdded += Components_ComponentAdded;
            _components.ComponentRemoved += Components_ComponentRemoved;
        }

		internal void DoExiting()
		{
			OnExiting(this, EventArgs.Empty);
			Instance.UnloadContent();
		}

        #endregion Internal Methods

//        internal IGraphicsDeviceManager graphicsDeviceManager
//        {
//            get
//            {
//                if (_graphicsDeviceManager == null)
//                {
////                    _graphicsDeviceManager = (IGraphicsDeviceManager)
////                        Services.GetService(typeof(IGraphicsDeviceManager));
//
//                    if (_graphicsDeviceManager == null)
//                        throw new InvalidOperationException ("No Graphics Device Manager");
//                }
//                return (IGraphicsDeviceManager)_graphicsDeviceManager;
//            }
//        }

        // NOTE: InitializeExistingComponents really should only be called once.
        //       Game.Initialize is the only method in a position to guarantee
        //       that no component will get a duplicate Initialize call.
        //       Further calls to Initialize occur immediately in response to
        //       Components.ComponentAdded.
        private void InitializeExistingComponents()
        {
            // TODO: Would be nice to get rid of this copy, but since it only
            //       happens once per game, it's fairly low priority.
            var copy = new IGameComponent[Components.Count];
            Components.CopyTo(copy, 0);
            foreach (var component in copy)
                component.Initialize();
        }

        private void CategorizeComponents()
        {
            DecategorizeComponents();
            for (int i = 0; i < Components.Count; ++i)
                CategorizeComponent(Components[i]);
        }

        // FIXME: I am open to a better name for this method.  It does the
        //        opposite of CategorizeComponents.
        private void DecategorizeComponents()
        {
            Instance.Updateables.Clear();
            _drawables.Clear();
        }

        private void CategorizeComponent(IGameComponent component)
        {
            if (component is IUpdateable)
                Instance.Updateables.Add((IUpdateable)component);
            if (component is IDrawable)
                _drawables.Add((IDrawable)component);
        }

        // FIXME: I am open to a better name for this method.  It does the
        //        opposite of CategorizeComponent.
        private void DecategorizeComponent(IGameComponent component)
        {
            if (component is IUpdateable)
				Instance.Updateables.Remove((IUpdateable)component);
            if (component is IDrawable)
                _drawables.Remove((IDrawable)component);
        }

        private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e)
            where TEventArgs : EventArgs
        {
            if (handler != null)
                handler(this, e);
        }
    }
}
