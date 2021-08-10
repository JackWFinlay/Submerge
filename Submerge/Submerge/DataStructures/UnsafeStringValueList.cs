using System;
using System.Buffers;

namespace Submerge.DataStructures
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder
    /// </summary>
    public struct UnsafeStringValueList
    {
        private char[] _array;
        private int _curr;
        private int _length;
        private const int _minimumCapacity = 1024;

        public UnsafeStringValueList(int initialCapacity)
        {
            if (initialCapacity < _minimumCapacity)
            {
                initialCapacity = _minimumCapacity;
            }

            _array = ArrayPool<char>.Shared.Rent(initialCapacity);
            
            _length = initialCapacity;
            _curr = 0;
        }

        public void Add(ReadOnlySpan<char> value)
        {
            var valueLength = value.Length;

            if (_curr + valueLength > _length)
            {
                Grow(valueLength);
            }
            
            value.CopyTo(_array.AsSpan(_curr, valueLength));

            _curr += valueLength;
        }

        public override unsafe string ToString()
        {
            fixed (char* a = _array)
            {
                var s = new string(a, 0, _curr);

                if (_array != null)
                {
                    ArrayPool<char>.Shared.Return(_array);
                }
                
                this = default;

                return s;
            }
        }

        private void Grow(int additionLength)
        {
            var requiredLength = _length;
            if (_curr + additionLength > requiredLength)
            {
                requiredLength = (_curr + additionLength) * 2;
            }
        
            var newSize = Math.Max(requiredLength, (_length * 2 ));

            var array = ArrayPool<char>.Shared.Rent(newSize);
            Buffer.BlockCopy(_array, 0, array, 0, _curr);


            if (_array != null)
            {
                ArrayPool<char>.Shared.Return(_array);
            }
            
            _array = array;
            _length = newSize;
        }
    }
}