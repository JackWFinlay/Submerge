using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Submerge.DataStructures
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder,
    /// and System.Runtime.CompilerServices.DefaultInterpolatedStringHandler
    /// </summary>
    public ref struct StringValueList
    {
        private Span<char> _span;
        private char[] _array;
        private int _pos;
        private const int _minimumCapacity = 1024;
        private const int _maxCapacity = 1_000_000_000;

        public StringValueList(int initialCapacity)
        {
            var capacityToRent = GetInitialLength(initialCapacity);
            _array = ArrayPool<char>.Shared.Rent(capacityToRent);
            _span = _array;
            _pos = 0;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(ReadOnlySpan<char> value)
        {
            if (value.Length + _pos > _span.Length)
            {
                Grow(value.Length);
            }

            value.CopyTo(_span.Slice(_pos, value.Length));
            
            _pos += value.Length;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            var s = _span.Slice(0, _pos).ToString();
            Dispose(); 
            return s;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetInitialLength(int initialLength)
        {
            var size = Math.Min(initialLength, _maxCapacity);
            size = Math.Clamp(size, _minimumCapacity, _maxCapacity);
            return size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetNewArraySize(int additionLength)
        {
            var size = Math.Max((_pos + additionLength), Math.Min((_array.Length * 2), _maxCapacity));
            size = Math.Clamp(size, _minimumCapacity, _maxCapacity);
            return size;
        }
        
        private void Grow(int additionLength)
        {
            var newSize = GetNewArraySize(additionLength);
            
            var newArray = ArrayPool<char>.Shared.Rent(newSize);

            _span.Slice(0, _pos).CopyTo(newArray);

            if (_array != null)
            {
                ArrayPool<char>.Shared.Return(_array);
            }
            
            _span = _array = newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Dispose()
        {
            var array = _array;
            this = default; // Reset this instance, we are done with it, any new adds will be to new StringValueList.
            if (array != null)
            {
                ArrayPool<char>.Shared.Return(array);
            }
        }
    }
}