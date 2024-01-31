using System;
using Avalonia.Native.Interop;

namespace Avalonia.Native
{
    internal partial class PredicateCallback : NativeCallbackBase, IAvnPredicateCallback
    {
        private Func<bool> _predicate;

        public PredicateCallback(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        int /*IAvnPredicateCallback.*/Evaluate()
        {
            return _predicate().AsComBool();
        }
    }
}
