using System;

namespace WebKit
{
    public interface IWebKitBrowserHost
    {
        int Height { get; }
        int Width { get; }
        IntPtr Handle { get; }
        event EventHandler Load;
        event EventHandler Resize;
        event EventHandler GotFocus;
        bool InvokeRequired { get; }
        object Invoke(Delegate @delegate);
        bool InDesignMode { get; }
    }
}