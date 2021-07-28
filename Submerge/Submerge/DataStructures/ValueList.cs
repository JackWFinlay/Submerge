using System;
using System.Buffers;

namespace Submerge.DataStructures
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder
    /// </summary>
    internal ref struct ValueList<T> where T : struct
    {
        private Span<T> _span;
        private T[] _array;

        public ValueList(Span<T> span)
        {
            _span = span;
            _array = null;
            Length = 0;
        }

        public int Length { get; private set; }

        public ref T this[int index] => ref _span[index];

        public void Add(T item)
        {
            var length = Length;
            if (length >= _span.Length)
            {
                Grow();
            }

            _span[length] = item;
            Length = (length + 1);
        }

        public void AddRange(ReadOnlySpan<T> span)
        {
            foreach (var item in span)
            {
                Add(item);
            }
        }

        public bool IsEmpty => Length == 0;

        public ReadOnlySpan<T> AsSpan()
        {
            return _span.Slice(0, Length);
        }

        public ReadOnlyMemory<T> AsMemory()
        {
            return new ReadOnlyMemory<T>(_array, 0, Length);
        }

        private void Grow()
        {
            var length = _span.Length;
            
            if (_span.IsEmpty)
            {
                length = 1;
            }

            var array = ArrayPool<T>.Shared.Rent(length * 2);

            _span.TryCopyTo(array);

            var arrayToReturn = _array;
            _array = array;
            _span = _array;

            if (arrayToReturn != null)
            {
                ArrayPool<T>.Shared.Return(_array);
            }
        }
    }
}