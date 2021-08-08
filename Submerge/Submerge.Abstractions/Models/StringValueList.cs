using System;
using System.Buffers;
using System.Linq;

namespace Submerge.Abstractions.Models
{
    /// <summary>
    /// Adapted from System.Collections.Generic.ValueListBuilder
    /// </summary>
    public struct StringValueList
    {
        private Memory<char> _memory;
        private char[] _array;

        public int Length { get; private set; }

        public void Add(ReadOnlySpan<char> value)
        {
            if (Length + value.Length > _memory.Length)
            {
                Grow(value.Length);
            }

            var slice = _memory.Slice(Length, value.Length).Span;
            value.CopyTo(slice);

            Length += value.Length;
        }
        
        public bool IsEmpty => Length == 0;

        public ReadOnlyMemory<char> AsMemory()
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

        private void Grow(int additionLength)
        {
            var length = Length;
            
            if (IsEmpty)
            {
                length = 1024;
            }

            var newArray = ArrayPool<char>.Shared.Rent(Math.Max((length + additionLength), (Length * 2)));

            _memory.Slice(0, Length).CopyTo(newArray);

            var arrayToReturn = _array;
            _array = newArray;
            _memory = _array;

            if (arrayToReturn != null)
            {
                ArrayPool<char>.Shared.Return(_array);
            }
        }

        private void Dispose()
        {
            var array = _array;
            this = default;
            if (array != null)
            {
                ArrayPool<char>.Shared.Return(array);
            }
        }
    }
}