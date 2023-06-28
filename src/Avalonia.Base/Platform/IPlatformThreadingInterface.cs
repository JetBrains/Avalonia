using System;
using System.Threading;
using Avalonia.Metadata;
using Avalonia.Threading;

namespace Avalonia.Platform
{
    public class PlatformExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; }
        public bool Handled { get; set; }

        public PlatformExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    public static class PlatformExceptionHandler
    {
        public static event EventHandler<PlatformExceptionEventArgs>? Exception;

        public static bool Catch(Action? action)
        {
            try
            {
                action?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                var args = new PlatformExceptionEventArgs(e);
                Exception?.Invoke(action, args);
                if (!args.Handled)
                    throw;
                return false;
            }
        }

        public static T? Catch<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception e)
            {
                var args = new PlatformExceptionEventArgs(e);
                Exception?.Invoke(action, args);
                if (!args.Handled)
                    throw;
                return default;
            }
        }
    }

    /// <summary>
    /// Provides platform-specific services relating to threading.
    /// </summary>
    [PrivateApi]
    public interface IPlatformThreadingInterface
    {
        /// <summary>
        /// Starts a timer.
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="interval">The interval.</param>
        /// <param name="tick">The action to call on each tick.</param>
        /// <returns>An <see cref="IDisposable"/> used to stop the timer.</returns>
        IDisposable StartTimer(DispatcherPriority priority, TimeSpan interval, Action tick);

        void Signal(DispatcherPriority priority);

        bool CurrentThreadIsLoopThread { get; }

        event Action<DispatcherPriority?>? Signaled;
    }
}
