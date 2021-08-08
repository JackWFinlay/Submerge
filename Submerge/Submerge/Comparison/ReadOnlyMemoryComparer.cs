using System;
using System.Collections.Generic;

namespace Submerge.Comparison
{
    public class ReadOnlyMemoryComparer : IEqualityComparer<ReadOnlyMemory<char>>
    {
        public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            for (var i = 0; i < x.Length; i++)
            {
                if (x.Span[i] != y.Span[i])
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(ReadOnlyMemory<char> obj)
        {
            var hashCode = new HashCode();
            var span = obj.Span;
            
            for (var i = 0; i < obj.Length; i++)
            {
                hashCode.Add(span[i]);
            }

            return hashCode.ToHashCode();
        }
    }
}