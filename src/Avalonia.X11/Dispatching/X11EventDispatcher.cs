using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using Avalonia.X11.Screens;
using Avalonia.Platform;
using static Avalonia.X11.XLib;
namespace Avalonia.X11;

public class X11EventArgs : CancelEventArgs
{
    public IntPtr XEvent { get; }

    public X11EventArgs(IntPtr xEvent) => XEvent = xEvent;
}

public static class X11Tools
{
    public static IntPtr Display => (AvaloniaLocator.Current.GetService<IWindowingPlatform>() as AvaloniaX11Platform)?.Display ?? IntPtr.Zero;
    public static IntPtr DeferredDisplay => (AvaloniaLocator.Current.GetService<IWindowingPlatform>() as AvaloniaX11Platform)?.DeferredDisplay ?? IntPtr.Zero;
    public static int? XIOpcode => AvaloniaLocator.Current.GetService<IWindowingPlatform>() is  AvaloniaX11Platform p && p.XI2 != null ? p.Info.XInputOpcode : null;

    public static (int x, int y) GetCursorPos() => XLib.GetCursorPos((AvaloniaLocator.Current.GetService<IWindowingPlatform>() as AvaloniaX11Platform)?.Info!);

    public static event EventHandler<X11EventArgs>? XEvent;

    internal static bool OnXEvent(object sender, IntPtr xEvent)
    {
        var args = new X11EventArgs(xEvent);
        XEvent?.Invoke(sender, args);
        return !args.Cancel;
    }

    public static void RefreshScreenInfo()
    {
        if (AvaloniaLocator.Current.GetService<IWindowingPlatform>() is not AvaloniaX11Platform platform)
            return;

        var screens = new X11Screens(platform);
        typeof(AvaloniaX11Platform).GetProperty(nameof(AvaloniaX11Platform.X11Screens))?.SetValue(platform, screens);
        typeof(AvaloniaX11Platform).GetProperty(nameof(AvaloniaX11Platform.Screens))?.SetValue(platform, screens);
    }
}

internal class X11EventDispatcher
{
    private readonly AvaloniaX11Platform _platform;
    private readonly IntPtr _display;

    public delegate void EventHandler(ref XEvent xev);
    public int Fd { get; }
    private readonly Dictionary<IntPtr, EventHandler> _eventHandlers;

    public X11EventDispatcher(AvaloniaX11Platform platform)
    {
        _platform = platform;
        _display = platform.Display;
        _eventHandlers = platform.Windows;
        Fd = XLib.XConnectionNumber(_display);
    }

    public bool IsPending => XPending(_display) != 0;
    
    public unsafe void DispatchX11Events(CancellationToken cancellationToken)
    {
        while (IsPending)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
                
            XNextEvent(_display, out var xev);
            if(XFilterEvent(ref xev, IntPtr.Zero))
                continue;

            if (xev.type == XEventName.GenericEvent)
                XGetEventData(_display, &xev.GenericEventCookie);
            try
            {
                if (!X11Tools.OnXEvent(this, (IntPtr)(&xev)))
                    return;
                
                if (xev.type == XEventName.GenericEvent)
                {
                    if (_platform.XI2 != null && _platform.Info.XInputOpcode ==
                        xev.GenericEventCookie.extension)
                    {
                        _platform.XI2.OnEvent((XIEvent*)xev.GenericEventCookie.data);
                    }
                }
                else if (_eventHandlers.TryGetValue(xev.AnyEvent.window, out var handler))
                    handler(ref xev);
            }
            finally
            {
                if (xev.type == XEventName.GenericEvent && xev.GenericEventCookie.data != null)
                    XFreeEventData(_display, &xev.GenericEventCookie);
            }
        }
        Flush();
    }

    public void Flush() => XFlush(_display);
}
