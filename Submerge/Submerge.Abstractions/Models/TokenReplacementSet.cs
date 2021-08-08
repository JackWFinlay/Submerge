using System;

namespace Submerge.Abstractions.Models
{
    public struct TokenReplacementSet
    {
        private ValueList<ReadOnlyMemory<char>> _valueList;

        public bool IsEmpty(int index)
        {
            return _valueList[index].IsEmpty;
        }

        public ReadOnlySpan<char> GetSubstitution(int index)
        {
            if (index > _valueList.Length)
            {
                throw new IndexOutOfRangeException();
            }

            return _valueList[index].Span;
        }

        public TokenReplacementSet AddReplacement(string replacement)
        {
            _valueList.Add(replacement.AsMemory());
            return this;
        }
    }
}