using System;

namespace Submerge.Abstractions.Models
{
    public class TokenReplacementSet
    {
        private readonly ValueList<ReadOnlyMemory<char>> _valueList = new ValueList<ReadOnlyMemory<char>>();

        public ReadOnlySpan<char> GetSubstitution(int index)
        {
            return _valueList[index].Span;
        }

        public TokenReplacementSet AddReplacement(string replacement)
        {
            _valueList.Add(replacement.AsMemory());
            return this;
        }
    }
}