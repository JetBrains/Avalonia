using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform;
using JetBrains.Annotations;

namespace Avalonia.Threading
{
    /// <summary>
    /// Provides services for managing work items on a thread.
    /// </summary>
    /// <remarks>
    /// In Avalonia, there is usually only a single <see cref="Dispatcher"/> in the application -
    /// the one for the UI thread, retrieved via the <see cref="UIThread"/> property.
    /// </remarks>
    public class DispatcherImpl : IDispatcherImpl
    {
        private readonly JobRunner _jobRunner;
        private IPlatformThreadingInterface? _platform;

        public static DispatcherImpl UIThread { get; } =
            new DispatcherImpl(AvaloniaLocator.Current.GetService<IPlatformThreadingInterface>());

        public DispatcherImpl(IPlatformThreadingInterface? platform)
        {
            _platform = platform;
            _jobRunner = new JobRunner(platform);

            if (_platform != null)
            {
                _platform.Signaled += _jobRunner.RunJobs;
            }
        }

        /// <summary>
        /// Checks that the current thread is the UI thread.
        /// </summary>
        public bool CheckAccess() => _platform?.CurrentThreadIsLoopThread ?? true;

        /// <summary>
        /// Checks that the current thread is the UI thread and throws if not.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The current thread is not the UI thread.
        /// </exception>
        public void VerifyAccess()
        {
            if (!CheckAccess())
                throw new InvalidOperationException("Call from invalid thread");
        }

        /// <summary>
        /// Runs the dispatcher's main loop.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token used to exit the main loop.
        /// </param>
        public void MainLoop(CancellationToken cancellationToken)
        {
            var platform = AvaloniaLocator.Current.GetService<IPlatformThreadingInterface>();

            if (platform is null)
                throw new InvalidOperationException("Unable to locate IPlatformThreadingInterface");

            cancellationToken.Register(() => platform.Signal(DispatcherPriority.Send));
            platform.RunLoop(cancellationToken);
        }

        /// <summary>
        /// Runs continuations pushed on the loop.
        /// </summary>
        public void RunJobs()
        {
            _jobRunner.RunJobs(null);
        }

        /// <summary>
        /// Use this method to ensure that more prioritized tasks are executed
        /// </summary>
        /// <param name="minimumPriority"></param>
        public void RunJobs(DispatcherPriority minimumPriority) => _jobRunner.RunJobs(minimumPriority);

        /// <inheritdoc/>
        public Task InvokeAsync(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));
            return _jobRunner.InvokeAsync(action, priority);
        }
        
        /// <inheritdoc/>
        public Task<TResult> InvokeAsync<TResult>(Func<TResult> function, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            _ = function ?? throw new ArgumentNullException(nameof(function));
            return _jobRunner.InvokeAsync(function, priority);
        }

        /// <inheritdoc/>
        public Task InvokeAsync(Func<Task> function, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            _ = function ?? throw new ArgumentNullException(nameof(function));
            return _jobRunner.InvokeAsync(function, priority).Unwrap();
        }

        /// <inheritdoc/>
        public Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> function, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            _ = function ?? throw new ArgumentNullException(nameof(function));
            return _jobRunner.InvokeAsync(function, priority).Unwrap();
        }

        /// <inheritdoc/>
        public void Post(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            _ = action ?? throw new ArgumentNullException(nameof(action));
            _jobRunner.Post(action, priority);
        }

        /// <summary>
        /// This is needed for platform backends that don't have internal priority system (e. g. win32)
        /// To ensure that there are no jobs with higher priority
        /// </summary>
        /// <param name="currentPriority"></param>
        internal void EnsurePriority(DispatcherPriority currentPriority)
        {
            if (currentPriority == DispatcherPriority.MaxValue)
                return;
            currentPriority += 1;
            _jobRunner.RunJobs(currentPriority);
        }

        /// <summary>
        /// Allows unit tests to change the platform threading interface.
        /// </summary>
        internal void UpdateServices()
        {
            if (_platform != null)
            {
                _platform.Signaled -= _jobRunner.RunJobs;
            }

            _platform = AvaloniaLocator.Current.GetService<IPlatformThreadingInterface>();
            _jobRunner.UpdateServices();

            if (_platform != null)
            {
                _platform.Signaled += _jobRunner.RunJobs;
            }
        }

        void IDispatcherImpl.EnsurePriority(DispatcherPriority currentPriority) => EnsurePriority(currentPriority);

        void IDispatcherImpl.UpdateServices() => UpdateServices();
    }
    
    /// <summary>
    /// Provides services for managing work items on a thread.
    /// </summary>
    /// <remarks>
    /// In Avalonia, there is usually only a single <see cref="Dispatcher"/> in the application -
    /// the one for the UI thread, retrieved via the <see cref="UIThread"/> property.
    /// </remarks>
    public class Dispatcher : IDispatcher
    {
        private IDispatcherImpl _dispatcherImpl;

        public static Dispatcher UIThread { get; } =
            new Dispatcher(AvaloniaLocator.Current.GetService<IDispatcherImpl>());

        private Dispatcher([NotNull] IDispatcherImpl dispatcherImpl)
        {
            _dispatcherImpl = dispatcherImpl ?? throw new ArgumentNullException(nameof(dispatcherImpl));
        }

        public Dispatcher(IPlatformThreadingInterface platform)
        {
            _dispatcherImpl = new DispatcherImpl(platform);
        }

        public bool CheckAccess()
        {
            return _dispatcherImpl.CheckAccess();
        }

        public void VerifyAccess()
        {
            _dispatcherImpl.VerifyAccess();
        }

        public void Post(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            _dispatcherImpl.Post(action, priority);
        }

        public Task InvokeAsync(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return _dispatcherImpl.InvokeAsync(action, priority);
        }

        public Task<TResult> InvokeAsync<TResult>(Func<TResult> function, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return _dispatcherImpl.InvokeAsync(function, priority);
        }

        public Task InvokeAsync(Func<Task> function, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return _dispatcherImpl.InvokeAsync(function, priority);
        }

        public Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> function, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return _dispatcherImpl.InvokeAsync(function, priority);
        }

        public void MainLoop(CancellationToken cancellationToken)
        {
            _dispatcherImpl.MainLoop(cancellationToken);
        }

        public void RunJobs()
        {
            _dispatcherImpl.RunJobs();
        }

        public void RunJobs(DispatcherPriority minimumPriority)
        {
            _dispatcherImpl.RunJobs(minimumPriority);
        }
        
        public void EnsurePriority(DispatcherPriority currentPriority)
        {
            _dispatcherImpl.EnsurePriority(currentPriority);
        }

        public void UpdateServices()
        {
            _dispatcherImpl.UpdateServices();
        }
    }
    
    public interface IDispatcherImpl : IDispatcher
    {
        /// <summary>
        /// Runs the dispatcher's main loop.
        /// </summary>
        /// <param name="cancellationToken">
        /// A cancellation token used to exit the main loop.
        /// </param>
        void MainLoop(CancellationToken cancellationToken);

        /// <summary>
        /// Runs continuations pushed on the loop.
        /// </summary>
        void RunJobs();

        /// <summary>
        /// Use this method to ensure that more prioritized tasks are executed
        /// </summary>
        /// <param name="minimumPriority"></param>
        void RunJobs(DispatcherPriority minimumPriority);

        /// <summary>
        /// This is needed for platform backends that don't have internal priority system (e. g. win32)
        /// To ensure that there are no jobs with higher priority
        /// </summary>
        /// <param name="currentPriority"></param>
        void EnsurePriority(DispatcherPriority currentPriority);

        /// <summary>
        /// Allows unit tests to change the platform threading interface.
        /// </summary>
        void UpdateServices();
    }
}
