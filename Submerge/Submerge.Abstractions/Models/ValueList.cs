using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Submerge.Abstractions.Models
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder
    /// </summary>
    public struct ValueList<T> where T : struct
    {
        public int Length { get; private set; }
        
        private Memory<T> _memory;
        private T[] _array;
        private const int _minimumCapacity = 8;

        public ref T this[int index] => ref _memory.Span[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (Length + 1 > _memory.Length)
            {
                Grow();
            }

            _memory.Span[Length] = item;
            Length++;
        }

        public void Dispose()
        {
            if (_array != null)
            {
                ArrayPool<T>.Shared.Return(_array);
            }
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

            if (_array != null)
            {
                ArrayPool<T>.Shared.Return(_array);
            }
            
            _memory = _array = array;
        }
    }
}