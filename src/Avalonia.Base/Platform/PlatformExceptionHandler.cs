using System;

namespace Avalonia.Platform;

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
            if (!ShouldSuppress(action, e))
                throw;
            return false;
        }
    }

    public static bool ShouldSuppress(object? sender, Exception e)
    {
        var args = new PlatformExceptionEventArgs(e);
        Exception?.Invoke(sender, args);
        return args.Handled;
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
