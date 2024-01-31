using System;
using Avalonia.Native.Interop;
using Avalonia.Platform;

namespace Avalonia.Native
{
    internal partial class AvaloniaNativeApplicationPlatform : IAvnApplicationEvents
    {
        void IAvnApplicationEvents.FilesOpened(IAvnStringArray urls) => PlatformExceptionHandler.Catch(() => FilesOpened(urls));
        int IAvnApplicationEvents.TryShutdown() => PlatformExceptionHandler.Catch(TryShutdown);
        void IAvnApplicationEvents.OnReopen() => PlatformExceptionHandler.Catch(OnReopen);
        void IAvnApplicationEvents.OnHide() => PlatformExceptionHandler.Catch(OnHide);
        void IAvnApplicationEvents.OnUnhide() => PlatformExceptionHandler.Catch(OnUnhide);
    }

    internal partial class AvaloniaNativeDragSource
    {
        private partial class DndCallback : IAvnDndResultCallback
        {
            void IAvnDndResultCallback.OnDragAndDropComplete(AvnDragDropEffects effecct) => PlatformExceptionHandler.Catch(() => OnDragAndDropComplete(effecct));
        }
    }

    // No need to catch because there is no user code executed
    // partial class AvaloniaNativePlatform
    // {
    //     partial class GCHandleDeallocator : IAvnGCHandleDeallocatorCallback
    //     {
    //         void IAvnGCHandleDeallocatorCallback.FreeGCHandle(IntPtr handle) => PlatformExceptionHandler.Catch(() => FreeGCHandle(handle));
    //     }
    // }

    internal partial class AvaloniaNativeTextInputMethod
    {
        private partial class AvnTextInputMethodClient : IAvnTextInputMethodClient
        {
            void IAvnTextInputMethodClient.SetPreeditText(string preeditText) => PlatformExceptionHandler.Catch(() => SetPreeditText(preeditText));
            void IAvnTextInputMethodClient.SelectInSurroundingText(int start, int length) => PlatformExceptionHandler.Catch(() => SelectInSurroundingText(start, length));
        }
    }

    internal partial class AvnAutomationPeer : IAvnAutomationPeer
    {
        IAvnAutomationNode IAvnAutomationPeer.Node => Node;
        void IAvnAutomationPeer.SetNode(IAvnAutomationNode node) => PlatformExceptionHandler.Catch(() => SetNode(node));
        IAvnString IAvnAutomationPeer.AcceleratorKey => AcceleratorKey;
        IAvnString IAvnAutomationPeer.AccessKey => AccessKey;
        AvnAutomationControlType IAvnAutomationPeer.AutomationControlType => AutomationControlType;
        IAvnString IAvnAutomationPeer.AutomationId => AutomationId;
        AvnRect IAvnAutomationPeer.BoundingRectangle => BoundingRectangle;
        IAvnAutomationPeerArray IAvnAutomationPeer.Children => Children;
        IAvnString IAvnAutomationPeer.ClassName => ClassName;
        IAvnAutomationPeer IAvnAutomationPeer.LabeledBy => LabeledBy;
        IAvnString IAvnAutomationPeer.Name => Name;
        IAvnAutomationPeer IAvnAutomationPeer.Parent => Parent;
        IAvnAutomationPeer IAvnAutomationPeer.VisualRoot => VisualRoot;
        int IAvnAutomationPeer.HasKeyboardFocus() => PlatformExceptionHandler.Catch(HasKeyboardFocus);
        int IAvnAutomationPeer.IsContentElement() => PlatformExceptionHandler.Catch(IsContentElement);
        int IAvnAutomationPeer.IsControlElement() => PlatformExceptionHandler.Catch(IsControlElement);
        int IAvnAutomationPeer.IsEnabled() => PlatformExceptionHandler.Catch(IsEnabled);
        int IAvnAutomationPeer.IsKeyboardFocusable() => PlatformExceptionHandler.Catch(IsKeyboardFocusable);
        void IAvnAutomationPeer.SetFocus() => PlatformExceptionHandler.Catch(SetFocus);
        int IAvnAutomationPeer.ShowContextMenu() => PlatformExceptionHandler.Catch(ShowContextMenu);
        IAvnAutomationPeer IAvnAutomationPeer.RootPeer => RootPeer;
        int IAvnAutomationPeer.IsRootProvider() => PlatformExceptionHandler.Catch(IsRootProvider);
        IAvnWindowBase IAvnAutomationPeer.RootProvider_GetWindow() => PlatformExceptionHandler.Catch(RootProvider_GetWindow);
        IAvnAutomationPeer IAvnAutomationPeer.RootProvider_GetFocus() => PlatformExceptionHandler.Catch(RootProvider_GetFocus);
        IAvnAutomationPeer IAvnAutomationPeer.RootProvider_GetPeerFromPoint(AvnPoint point) => PlatformExceptionHandler.Catch(() => RootProvider_GetPeerFromPoint(point));
        int IAvnAutomationPeer.IsEmbeddedRootProvider() => PlatformExceptionHandler.Catch(IsEmbeddedRootProvider);
        IAvnAutomationPeer IAvnAutomationPeer.EmbeddedRootProvider_GetFocus() => PlatformExceptionHandler.Catch(EmbeddedRootProvider_GetFocus);
        IAvnAutomationPeer IAvnAutomationPeer.EmbeddedRootProvider_GetPeerFromPoint(AvnPoint point) => PlatformExceptionHandler.Catch(() => EmbeddedRootProvider_GetPeerFromPoint(point));
        int IAvnAutomationPeer.IsExpandCollapseProvider() => PlatformExceptionHandler.Catch(IsExpandCollapseProvider);
        int IAvnAutomationPeer.ExpandCollapseProvider_GetIsExpanded() => PlatformExceptionHandler.Catch(ExpandCollapseProvider_GetIsExpanded);
        int IAvnAutomationPeer.ExpandCollapseProvider_GetShowsMenu() => PlatformExceptionHandler.Catch(ExpandCollapseProvider_GetShowsMenu);
        void IAvnAutomationPeer.ExpandCollapseProvider_Expand() => PlatformExceptionHandler.Catch(ExpandCollapseProvider_Expand);
        void IAvnAutomationPeer.ExpandCollapseProvider_Collapse() => PlatformExceptionHandler.Catch(ExpandCollapseProvider_Collapse);
        int IAvnAutomationPeer.IsInvokeProvider() => PlatformExceptionHandler.Catch(IsInvokeProvider);
        void IAvnAutomationPeer.InvokeProvider_Invoke() => PlatformExceptionHandler.Catch(InvokeProvider_Invoke);
        int IAvnAutomationPeer.IsRangeValueProvider() => PlatformExceptionHandler.Catch(IsRangeValueProvider);
        double IAvnAutomationPeer.RangeValueProvider_GetValue() => PlatformExceptionHandler.Catch(RangeValueProvider_GetValue);
        double IAvnAutomationPeer.RangeValueProvider_GetMinimum() => PlatformExceptionHandler.Catch(RangeValueProvider_GetMinimum);
        double IAvnAutomationPeer.RangeValueProvider_GetMaximum() => PlatformExceptionHandler.Catch(RangeValueProvider_GetMaximum);
        double IAvnAutomationPeer.RangeValueProvider_GetSmallChange() => PlatformExceptionHandler.Catch(RangeValueProvider_GetSmallChange);
        double IAvnAutomationPeer.RangeValueProvider_GetLargeChange() => PlatformExceptionHandler.Catch(RangeValueProvider_GetLargeChange);
        void IAvnAutomationPeer.RangeValueProvider_SetValue(double value) => PlatformExceptionHandler.Catch(() => RangeValueProvider_SetValue(value));
        int IAvnAutomationPeer.IsSelectionItemProvider() => PlatformExceptionHandler.Catch(IsSelectionItemProvider);
        int IAvnAutomationPeer.SelectionItemProvider_IsSelected() => PlatformExceptionHandler.Catch(SelectionItemProvider_IsSelected);
        int IAvnAutomationPeer.IsToggleProvider() => PlatformExceptionHandler.Catch(IsToggleProvider);
        int IAvnAutomationPeer.ToggleProvider_GetToggleState() => PlatformExceptionHandler.Catch(ToggleProvider_GetToggleState);
        void IAvnAutomationPeer.ToggleProvider_Toggle() => PlatformExceptionHandler.Catch(ToggleProvider_Toggle);
        int IAvnAutomationPeer.IsValueProvider() => PlatformExceptionHandler.Catch(IsValueProvider);
        IAvnString IAvnAutomationPeer.ValueProvider_GetValue() => PlatformExceptionHandler.Catch(ValueProvider_GetValue);
        void IAvnAutomationPeer.ValueProvider_SetValue(string value) => PlatformExceptionHandler.Catch(() => ValueProvider_SetValue(value));
    }

    internal partial class AvnAutomationPeerArray : IAvnAutomationPeerArray
    {
        uint IAvnAutomationPeerArray.Count => Count;
        IAvnAutomationPeer IAvnAutomationPeerArray.Get(uint index) => PlatformExceptionHandler.Catch(() => Get(index));
    }

    // No need to catch because these events already handled in DispatcherOperation
    // internal partial class AvnDispatcher : IAvnDispatcher
    // {
    //     void IAvnDispatcher.Post(IAvnActionCallback cb) => PlatformExceptionHandler.Catch(() => Post(cb));
    // } 
}

// namespace Avalonia.Native.Interop
// {
//      No need to catch because there is no user code executed
//     internal sealed partial class AvnString : IAvnString
//     {
//         string IAvnString.String => String;
//         byte[] IAvnString.Bytes => Bytes;
//         unsafe void* IAvnString.Pointer() => (void*)PlatformExceptionHandler.Catch(() => (IntPtr)Pointer());
//         int IAvnString.Length() => PlatformExceptionHandler.Catch(Length);
//     }
//
//      No need to catch because there is no user code executed
//     internal sealed partial class AvnStringArray : IAvnStringArray
//     {
//         string[] IAvnStringArray.ToStringArray() => PlatformExceptionHandler.Catch(ToStringArray);
//         uint IAvnStringArray.Count => Count;
//         IAvnString IAvnStringArray.Get(uint index) => PlatformExceptionHandler.Catch(() => Get(index));
//     }
// }

namespace Avalonia.Native
{
    // No need to catch because these events already handled in DispatcherOperation
    // internal partial class DispatcherImpl
    // {
    //     private partial class Events : IAvnPlatformThreadingInterfaceEvents
    //     {
    //         void IAvnPlatformThreadingInterfaceEvents.Signaled() => PlatformExceptionHandler.Catch(Signaled);
    //         void IAvnPlatformThreadingInterfaceEvents.Timer() => PlatformExceptionHandler.Catch(Timer);
    //         void IAvnPlatformThreadingInterfaceEvents.ReadyForBackgroundProcessing() => PlatformExceptionHandler.Catch(ReadyForBackgroundProcessing);
    //     }
    // }
}

namespace Avalonia.Native.Interop
{
    internal partial class MenuEvents : IAvnMenuEvents
    {
        void IAvnMenuEvents.NeedsUpdate() => PlatformExceptionHandler.Catch(NeedsUpdate);
        void IAvnMenuEvents.Opening() => PlatformExceptionHandler.Catch(Opening);
        void IAvnMenuEvents.Closed() => PlatformExceptionHandler.Catch(Closed);
    }
}

namespace Avalonia.Native
{
    internal partial class MenuActionCallback : IAvnActionCallback
    {
        void IAvnActionCallback.Run() => PlatformExceptionHandler.Catch(Run);
    }

    internal partial class NativePlatformSettings
    {
        private partial class ColorsChangeCallback : IAvnActionCallback
        {
            void IAvnActionCallback.Run() => PlatformExceptionHandler.Catch(Run);
        }
    }

    internal partial class PopupImpl
    {
        partial class PopupEvents : IAvnWindowBaseEvents, IAvnWindowEvents
        {
            void IAvnWindowBaseEvents.Paint() => PlatformExceptionHandler.Catch(Paint);
            void IAvnWindowBaseEvents.Closed() => PlatformExceptionHandler.Catch(Closed);
            void IAvnWindowBaseEvents.Activated() => PlatformExceptionHandler.Catch(Activated);
            void IAvnWindowBaseEvents.Deactivated() => PlatformExceptionHandler.Catch(Deactivated);
            unsafe void IAvnWindowBaseEvents.Resized(AvnSize* size, AvnPlatformResizeReason reason) => PlatformExceptionHandler.Catch(() => Resized(size, reason));
            void IAvnWindowBaseEvents.PositionChanged(AvnPoint position) => PlatformExceptionHandler.Catch(() => PositionChanged(position));
            void IAvnWindowBaseEvents.RawMouseEvent(AvnRawMouseEventType type, ulong timeStamp, AvnInputModifiers modifiers, AvnPoint point, AvnVector delta) => PlatformExceptionHandler.Catch(() => RawMouseEvent(type, timeStamp, modifiers, point, delta));
            int IAvnWindowBaseEvents.RawKeyEvent(AvnRawKeyEventType type, ulong timeStamp, AvnInputModifiers modifiers, AvnKey key, AvnPhysicalKey physicalKey, string keySymbol) => PlatformExceptionHandler.Catch(() => RawKeyEvent(type, timeStamp, modifiers, key, physicalKey, keySymbol));
            int IAvnWindowBaseEvents.RawTextInputEvent(ulong timeStamp, string text) => PlatformExceptionHandler.Catch(() => RawTextInputEvent(timeStamp, text));
            void IAvnWindowBaseEvents.ScalingChanged(double scaling) => PlatformExceptionHandler.Catch(() => ScalingChanged(scaling));
            void IAvnWindowBaseEvents.RunRenderPriorityJobs() => PlatformExceptionHandler.Catch(RunRenderPriorityJobs);
            void IAvnWindowBaseEvents.LostFocus() => PlatformExceptionHandler.Catch(LostFocus);
            AvnDragDropEffects IAvnWindowBaseEvents.DragEvent(AvnDragEventType type, AvnPoint position, AvnInputModifiers modifiers, AvnDragDropEffects effects, IAvnClipboard clipboard, IntPtr dataObjectHandle) => PlatformExceptionHandler.Catch(() => DragEvent(type, position, modifiers, effects, clipboard, dataObjectHandle));
            IAvnAutomationPeer IAvnWindowBaseEvents.AutomationPeer => AutomationPeer;
            int IAvnWindowEvents.Closing() => PlatformExceptionHandler.Catch(Closing);
            void IAvnWindowEvents.WindowStateChanged(AvnWindowState state) => PlatformExceptionHandler.Catch(() => WindowStateChanged(state));
            void IAvnWindowEvents.GotInputWhenDisabled() => PlatformExceptionHandler.Catch(GotInputWhenDisabled);
        }
    }

    internal partial class PredicateCallback : IAvnPredicateCallback
    {
        int IAvnPredicateCallback.Evaluate() => PlatformExceptionHandler.Catch(Evaluate);
    }

    internal partial class FilePickerFileTypesWrapper : IAvnFilePickerFileTypes
    {
        int IAvnFilePickerFileTypes.Count => Count;
        int IAvnFilePickerFileTypes.IsDefaultType(int index) => PlatformExceptionHandler.Catch(() => IsDefaultType(index));
        int IAvnFilePickerFileTypes.IsAnyType(int index) => PlatformExceptionHandler.Catch(() => IsAnyType(index));
        IAvnString IAvnFilePickerFileTypes.GetName(int index) => PlatformExceptionHandler.Catch(() => GetName(index));
        IAvnStringArray IAvnFilePickerFileTypes.GetPatterns(int index) => PlatformExceptionHandler.Catch(() => GetPatterns(index));
        IAvnStringArray IAvnFilePickerFileTypes.GetExtensions(int index) => PlatformExceptionHandler.Catch(() => GetExtensions(index));
        IAvnStringArray IAvnFilePickerFileTypes.GetMimeTypes(int index) => PlatformExceptionHandler.Catch(() => GetMimeTypes(index));
        IAvnStringArray IAvnFilePickerFileTypes.GetAppleUniformTypeIdentifiers(int index) => PlatformExceptionHandler.Catch(() => GetAppleUniformTypeIdentifiers(index));
    }

    internal partial class SystemDialogEvents : IAvnSystemDialogEvents
    {
        unsafe void IAvnSystemDialogEvents.OnCompleted(int numResults, void* ptrFirstResult) => PlatformExceptionHandler.Catch(() => OnCompleted(numResults, ptrFirstResult));
    }

    internal partial class WindowImpl
    {
        private partial class WindowEvents : IAvnWindowEvents
        {
            int IAvnWindowEvents.Closing() => PlatformExceptionHandler.Catch(Closing);
            void IAvnWindowEvents.WindowStateChanged(AvnWindowState state) => PlatformExceptionHandler.Catch(() => WindowStateChanged(state));
            void IAvnWindowEvents.GotInputWhenDisabled() => PlatformExceptionHandler.Catch(GotInputWhenDisabled);
        }
    }

    internal abstract partial class WindowBaseImpl
    {
        protected unsafe partial class WindowBaseEvents : IAvnWindowBaseEvents
        {
            void IAvnWindowBaseEvents.Paint() => PlatformExceptionHandler.Catch(Paint);
            void IAvnWindowBaseEvents.Closed() => PlatformExceptionHandler.Catch(Closed);
            void IAvnWindowBaseEvents.Activated() => PlatformExceptionHandler.Catch(Activated);
            void IAvnWindowBaseEvents.Deactivated() => PlatformExceptionHandler.Catch(Deactivated);
            void IAvnWindowBaseEvents.Resized(AvnSize* size, AvnPlatformResizeReason reason) => PlatformExceptionHandler.Catch(() => Resized(size, reason));
            void IAvnWindowBaseEvents.PositionChanged(AvnPoint position) => PlatformExceptionHandler.Catch(() => PositionChanged(position));
            void IAvnWindowBaseEvents.RawMouseEvent(AvnRawMouseEventType type, ulong timeStamp, AvnInputModifiers modifiers, AvnPoint point, AvnVector delta) => PlatformExceptionHandler.Catch(() => RawMouseEvent(type, timeStamp, modifiers, point, delta));
            int IAvnWindowBaseEvents.RawKeyEvent(AvnRawKeyEventType type, ulong timeStamp, AvnInputModifiers modifiers, AvnKey key, AvnPhysicalKey physicalKey, string keySymbol) => PlatformExceptionHandler.Catch(() => RawKeyEvent(type, timeStamp, modifiers, key, physicalKey, keySymbol));
            int IAvnWindowBaseEvents.RawTextInputEvent(ulong timeStamp, string text) => PlatformExceptionHandler.Catch(() => RawTextInputEvent(timeStamp, text));
            void IAvnWindowBaseEvents.ScalingChanged(double scaling) => PlatformExceptionHandler.Catch(() => ScalingChanged(scaling));
            void IAvnWindowBaseEvents.RunRenderPriorityJobs() => PlatformExceptionHandler.Catch(RunRenderPriorityJobs);
            void IAvnWindowBaseEvents.LostFocus() => PlatformExceptionHandler.Catch(LostFocus);
            AvnDragDropEffects IAvnWindowBaseEvents.DragEvent(AvnDragEventType type, AvnPoint position, AvnInputModifiers modifiers, AvnDragDropEffects effects, IAvnClipboard clipboard, IntPtr dataObjectHandle) => PlatformExceptionHandler.Catch(() => DragEvent(type, position, modifiers, effects, clipboard, dataObjectHandle));
            IAvnAutomationPeer IAvnWindowBaseEvents.AutomationPeer => AutomationPeer;
        }
    }
}
