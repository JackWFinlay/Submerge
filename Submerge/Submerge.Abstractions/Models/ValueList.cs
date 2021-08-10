using System;
using System.Buffers;

namespace Submerge.Abstractions.Models
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder
    /// </summary>
    public class ValueList<T>
    {
        private Memory<T> _memory;
        private T[] _array;
        private const int _minimumCapacity = 16;

        public ValueList()
        {
            _array = ArrayPool<T>.Shared.Rent(_minimumCapacity);
            _memory = _array;
            Length = 0;
        }

        public ValueList(int initialCapacity)
        {
            if (initialCapacity < _minimumCapacity)
            {
                initialCapacity = _minimumCapacity;
            }

            _array = ArrayPool<T>.Shared.Rent(initialCapacity);
            _memory = _array;
            Length = 0;
        }

        public int Length { get; private set; }

        public ref T this[int index] => ref _memory.Span[index];

        public void Add(T item)
        {
            if (Length + 1 > _memory.Length)
            {
                Grow();
            }

            _memory.Span[Length] = item;
            Length++;
        }

        public bool IsEmpty => Length == 0;

        public ReadOnlyMemory<T> AsMemory()
        {
            var mem = _memory.Slice(0, Length);
            Dispose();
            return mem;
        }

        private void Grow()
        {
            var length = Length;
            
            if (_memory.Length < _minimumCapacity)
            {
                length = _minimumCapacity;
            }

            var array = ArrayPool<T>.Shared.Rent((length * 2));

            _memory.Slice(0, Length).CopyTo(array);

            var arrayToReturn = _array;
            _array = array;
            _memory = _array;

            if (arrayToReturn != null)
            {
                ArrayPool<T>.Shared.Return(_array);
            }
        }

        private void Dispose()
        {
            if (_array != null)
            {
                ArrayPool<T>.Shared.Return(_array);
            }
        }
    }
}