using System.Collections;
using System.Collections.Generic;

namespace Avalonia.Utilities
{
    /// <summary>
    /// Implements a simple set which is safe to modify during enumeration.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// Implements a collection which, when written to while enumerating, performs a copy of the collection
    /// items. 
    /// </remarks>
    internal class SafeEnumerableSet<T> : IEnumerable<T>
    {
        private HashSet<T> _list = new();
        private int _generation;
        private int _enumCount = 0;

        public int Count => _list.Count;
        internal HashSet<T> Inner => _list;

        public void Add(T item) => GetList().Add(item);
        public bool Remove(T item) => GetList().Remove(item);

        public Enumerator GetEnumerator() => new(this, _list);
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        private HashSet<T> GetList()
        {
            if (_enumCount > 0)
            {
                _list = new(_list);
                ++_generation;
                _enumCount = 0;
            }

            return _list;
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly SafeEnumerableSet<T> _owner;
            private readonly int _generation;
            private IEnumerator<T> _enumerator;

            internal Enumerator(SafeEnumerableSet<T> owner, HashSet<T> list)
            {
                _owner = owner;
                _generation = owner._generation;
                _enumerator = list.GetEnumerator();
                ++_owner._enumCount;
            }

            public void Dispose()
            {
                if (_owner._generation == _generation)
                    --_owner._enumCount;
                _enumerator.Dispose();
            }

            public bool MoveNext() => _enumerator.MoveNext();

            public T Current => _enumerator.Current;
            object? IEnumerator.Current => _enumerator.Current;

            void IEnumerator.Reset() => _enumerator.Reset();
        }
    }
}
