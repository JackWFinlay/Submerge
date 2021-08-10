using System;
using System.Buffers;

namespace Submerge.DataStructures
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder
    /// </summary>
    public ref struct StringValueList
    {
        private Span<char> _span;
        private char[] _array;
        private int _length;
        private const int _minimumCapacity = 1024;

        public StringValueList(int initialCapacity)
        {
            if (initialCapacity < _minimumCapacity)
            {
                initialCapacity = _minimumCapacity;
            }

            _array = ArrayPool<char>.Shared.Rent(initialCapacity);
            _span = _array;
            _length = 0;
        }

        public void Add(ReadOnlySpan<char> value)
        {
            if (_length + value.Length > _span.Length)
            {
                Grow(value.Length);
            }

            var slice = _span.Slice(_length, value.Length);
            value.CopyTo(slice);

            _length += value.Length;
        }
        
        public override string ToString()
        {
            var s = _span.Slice(0, _length).ToString();
            Dispose(); 
            return s;
        }

        private void Grow(int additionLength)
        {
            var requiredLength = _length;
            if (_length + additionLength > requiredLength)
            {
                requiredLength = (_length + additionLength) * 2;
            }
        
            var newSize = Math.Max(requiredLength, (_length * 2 ));

            var array = ArrayPool<char>.Shared.Rent(newSize);
            //Buffer.BlockCopy(_array, 0, array, 0, _length);
            _span.Slice(0, _length).CopyTo(array);

            if (_array != null)
            {
                ArrayPool<char>.Shared.Return(_array);
            }
            
            _array = array;
            _span = _array;
        }

        private void Dispose()
        {
            if (_array != null)
            {
                ArrayPool<char>.Shared.Return(_array);
            }
        }
    }
}