using System;
using Avalonia.Native.Interop;

namespace Avalonia.Native
{
    public class PredicateCallback : CallbackBase, IAvnPredicateCallback
    {
        private Func<bool> _predicate;

        public PredicateCallback(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        int IAvnPredicateCallback.Evaluate()
        {
            return Platform.PlatformExceptionHandler.Catch(() =>
            {
            return _predicate().AsComBool();
            });
        }
    }
}
