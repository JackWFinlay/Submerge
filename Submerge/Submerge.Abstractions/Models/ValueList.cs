using System;
using System.Buffers;
using System.Linq;

namespace Submerge.Abstractions.Models
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder
    /// </summary>
    public struct ValueList<T>
    {
        private Memory<T> _memory;
        private T[] _array;

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
            var mem = _memory;
            Dispose();
            return mem;
        }

        public override string ToString()
        {
            var s = _memory.Slice(0, Length).ToString();
            Dispose();
            return s;
        }

        private void Grow()
        {
            var length = Length;
            
            if (IsEmpty)
            {
                length = 4;
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
            var array = _array;
            this = default;
            if (array != null)
            {
                ArrayPool<T>.Shared.Return(array);
            }
        }
    }
}