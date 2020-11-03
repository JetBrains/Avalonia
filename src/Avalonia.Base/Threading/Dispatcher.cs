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
}