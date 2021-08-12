using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Submerge.Abstractions.Models
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder
    /// </summary>
    public class ValueList<T>
    {
        public int Length { get; private set; }
        
        private Memory<T> _memory;
        private T[] _array;
        private const int _minimumCapacity = 16;
        private const int _maxCapacity = 1024 * 1024;

        public ValueList()
        {
            _array = ArrayPool<T>.Shared.Rent(_minimumCapacity);
            _memory = _array;
            Length = 0;
        }

        public ValueList(int initialCapacity)
        {
            _array = ArrayPool<T>.Shared.Rent(GetInitialLength(initialCapacity));
            _memory = _array;
            Length = 0;
        }

        public ref T this[int index] => ref _memory.Span[index];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (Length + 1 > _array.Length)
            {
                Grow();
            }

            _memory.Span[Length] = item;
            Length++;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetInitialLength(int initialLength)
        {
            var size = Math.Min(initialLength, _maxCapacity);
            size = Math.Clamp(size, _minimumCapacity, _maxCapacity);
            return size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Dispose()
        {
            if (_array != null)
            {
                ArrayPool<T>.Shared.Return(_array);
            }
        }
    }
}