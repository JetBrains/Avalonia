using System;
using Avalonia.Platform;

namespace Avalonia.Win32;
using static Interop.UnmanagedMethods;

internal static class SafeWndProc
{
    public static WndProc WndProc(WndProc wndProc) =>
        (hWnd, msg, wParam, lParam) =>
        {
            var result = IntPtr.Zero;
            return PlatformExceptionHandler.Catch(() =>
            {
                result = wndProc(hWnd, msg, wParam, lParam);
            }) ? result : DefWindowProc(hWnd, msg, wParam, lParam);
        };
}
