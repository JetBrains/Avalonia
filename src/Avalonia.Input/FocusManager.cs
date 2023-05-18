using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Interactivity;
using Avalonia.Rendering;
using Avalonia.VisualTree;

namespace Avalonia.Input
{
    /// <summary>
    /// Manages focus for the application.
    /// </summary>
    public class FocusManager : IFocusManager
    {
        /// <summary>
        /// The focus scopes in which the focus is currently defined.
        /// </summary>
        private readonly ConditionalWeakTable<IFocusScope, IInputElement?> _focusScopes =
            new ConditionalWeakTable<IFocusScope, IInputElement?>();

        private readonly ConditionalWeakTable<IRenderRoot, HashSet<IInputElement>> _focusedControls =
            new ConditionalWeakTable<IRenderRoot, HashSet<IInputElement>>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusManager"/> class.
        /// </summary>
        static FocusManager()
        {
            InputElement.PointerPressedEvent.AddClassHandler(
                typeof(IInputElement),
                new EventHandler<RoutedEventArgs>(OnPreviewPointerPressed),
                RoutingStrategies.Tunnel);
        }

        /// <summary>
        /// Gets the instance of the <see cref="IFocusManager"/>.
        /// </summary>
        public static IFocusManager? Instance => AvaloniaLocator.Current.GetService<IFocusManager>();

        /// <summary>
        /// Gets the currently focused <see cref="IInputElement"/>.
        /// </summary>
        public IInputElement? Current => Scope != null && _focusScopes.TryGetValue(Scope, out var result) && result != null ? result : KeyboardDevice.Instance?.FocusedElement;

        /// <summary>
        /// Gets the current focus scope.
        /// </summary>
        public IFocusScope? Scope
        {
            get;
            private set;
        }

        public void ClearFocus(IInputElement control)
        {
            var scope = GetFocusScopeAncestors(control).FirstOrDefault();
            if (scope != null)
                SetFocusedElement(scope, null);
            else
                Focus(null);
        }

        /// <summary>
        /// Focuses a control.
        /// </summary>
        /// <param name="control">The control to focus.</param>
        /// <param name="method">The method by which focus was changed.</param>
        /// <param name="keyModifiers">Any key modifiers active at the time of focus.</param>
        public void Focus(
            IInputElement? control, 
            NavigationMethod method = NavigationMethod.Unspecified,
            KeyModifiers keyModifiers = KeyModifiers.None)
        {
            if (control != null)
            {
                var scope = GetFocusScopeAncestors(control)
                    .FirstOrDefault();

                if (scope != null)
                {
                    Scope = scope;
                    SetFocusedElement(scope, control, method, keyModifiers);
                }
            }
            else if (Current != null)
            {
                // If control is null, set focus to the topmost focus scope.
                foreach (var scope in GetFocusScopeAncestors(Current).Reverse().ToList())
                {
                    if (scope != Scope &&
                        _focusScopes.TryGetValue(scope, out var element) &&
                        element != null)
                    {
                        Focus(element, method);
                        return;
                    }
                }

                if (Scope is object)
                {
                    // Couldn't find a focus scope, clear focus.
                    SetFocusedElement(Scope, null);
                }
            }
        }

        public IInputElement? GetFocusedElement(IInputElement e)
        {
            if (e is IFocusScope scope)
            {
                _focusScopes.TryGetValue(scope, out var result);
                return result;
            }

            return null;
        }

        public IEnumerable<IInputElement> GetFocusedElements(IRenderRoot root) =>
            root != null && _focusedControls.TryGetValue(root, out var result) ? result : Enumerable.Empty<IInputElement>();
        
        /// <summary>
        /// Sets the currently focused element in the specified scope.
        /// </summary>
        /// <param name="scope">The focus scope.</param>
        /// <param name="element">The element to focus. May be null.</param>
        /// <param name="method">The method by which focus was changed.</param>
        /// <param name="keyModifiers">Any key modifiers active at the time of focus.</param>
        /// <remarks>
        /// If the specified scope is the current <see cref="Scope"/> then the keyboard focus
        /// will change.
        /// </remarks>
        public void SetFocusedElement(
            IFocusScope scope,
            IInputElement? element,
            NavigationMethod method = NavigationMethod.Unspecified,
            KeyModifiers keyModifiers = KeyModifiers.None)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            IRenderRoot? oldRoot = null;
            IRenderRoot? newRoot = null;
            
            if (_focusScopes.TryGetValue(scope, out var existingElement))
            {
                if (element != existingElement)
                {
                    _focusScopes.Remove(scope);
                    _focusScopes.Add(scope, element);

                    oldRoot = existingElement?.VisualRoot;
                    newRoot = element?.VisualRoot;
                }
            }
            else
            {
                _focusScopes.Add(scope, element);
                
                oldRoot = newRoot = element?.VisualRoot;
            }

            if (Scope == scope)
            {
                KeyboardDevice.Instance?.SetFocusedElement(element, method, keyModifiers);

                if (element != existingElement)
                {
                    if (existingElement?.VisualRoot != null && _focusedControls.TryGetValue(existingElement.VisualRoot, out var existingSet))
                    {
                        existingSet.Remove(existingElement);
                        if (!existingSet.Any())
                            _focusedControls.Remove(existingElement.VisualRoot);
                    }

                    existingElement?.RaiseEvent(new RoutedEventArgs { RoutedEvent = InputElement.LostFocusEvent, });

                    if (element?.VisualRoot != null)
                    {
                        if (!_focusedControls.TryGetValue(element.VisualRoot, out var newSet))
                        {
                            newSet = new HashSet<IInputElement>();
                            _focusedControls.Add(element.VisualRoot, newSet);
                        }

                        newSet.Add(element);
                    }

                    element?.RaiseEvent(new GotFocusEventArgs { RoutedEvent = InputElement.GotFocusEvent, NavigationMethod = method, KeyModifiers = keyModifiers, });

                    if (oldRoot != null)
                        UpdateFocusWithin(oldRoot);

                    if (newRoot != null && (oldRoot == null || oldRoot != newRoot))
                        UpdateFocusWithin(newRoot);
                }
            }
        }

        public void UpdateFocusWithin(IRenderRoot root)
        {
            HashSet<IVisual>? setFocusWithin = null;
            if (_focusedControls.TryGetValue(root, out var oldFocusedControls))
            {
                oldFocusedControls.RemoveWhere(x => !x.IsAttachedToVisualTree);
                if (!oldFocusedControls.Any())
                    _focusedControls.Remove(root);
                setFocusWithin = new HashSet<IVisual>(oldFocusedControls.SelectMany(x => x.GetSelfAndVisualAncestors()));
            }

            var allChildren = root.GetSelfAndVisualDescendants().OfType<InputElement>().ToArray();
            foreach (var child in allChildren)
                child.IsFocusWithin = setFocusWithin?.Contains(child) == true;
        }

        /// <summary>
        /// Notifies the focus manager of a change in focus scope.
        /// </summary>
        /// <param name="scope">The new focus scope.</param>
        public void SetFocusScope(IFocusScope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (!_focusScopes.TryGetValue(scope, out var e))
            {
                // TODO: Make this do something useful, i.e. select the first focusable
                // control, select a control that the user has specified to have default
                // focus etc.
                e = scope as IInputElement;
                _focusScopes.Add(scope, e);
            }

            Scope = scope;
            Focus(e);
        }

        public void RemoveFocusScope(IFocusScope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));
            
            if (_focusScopes.TryGetValue(scope, out var existingElement))
            {
                SetFocusedElement(scope, null);
                _focusScopes.Remove(scope);
            }

            if (Scope == scope)
            {
                Scope = null;
            }
            else if (existingElement?.VisualRoot != null)
            {
                UpdateFocusWithin(existingElement.VisualRoot);
            }
        }

        public static bool GetIsFocusScope(IInputElement e) => e is IFocusScope;

        /// <summary>
        /// Checks if the specified element can be focused.
        /// </summary>
        /// <param name="e">The element.</param>
        /// <returns>True if the element can be focused.</returns>
        private static bool CanFocus(IInputElement e) => e.Focusable && e.IsEffectivelyEnabled && e.IsVisible;

        /// <summary>
        /// Gets the focus scope ancestors of the specified control, traversing popups.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>The focus scopes.</returns>
        private static IEnumerable<IFocusScope> GetFocusScopeAncestors(IInputElement control)
        {
            IInputElement? c = control;

            while (c != null)
            {
                var scope = c as IFocusScope;

                if (scope != null && c.VisualRoot?.IsVisible == true)
                {
                    yield return scope;
                }

                c = c.GetVisualParent<IInputElement>() ??
                    ((c as IHostedVisualTreeRoot)?.Host as IInputElement);
            }
        }

        /// <summary>
        /// Global handler for pointer pressed events.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private static void OnPreviewPointerPressed(object? sender, RoutedEventArgs e)
        {
            if (sender is null)
                return;

            var ev = (PointerPressedEventArgs)e;
            var visual = (IVisual)sender;

            if (sender == e.Source && ev.GetCurrentPoint(visual).Properties.IsLeftButtonPressed)
            {
                IVisual? element = ev.Pointer?.Captured ?? e.Source as IInputElement;

                while (element != null)
                {
                    if (element is IInputElement inputElement && CanFocus(inputElement))
                    {
                        Instance?.Focus(inputElement, NavigationMethod.Pointer, ev.KeyModifiers);

                        break;
                    }
                    
                    element = element.VisualParent;
                }
            }
        }
    }
}
