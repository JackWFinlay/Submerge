using System;

namespace Submerge.Abstractions.Models
{
    public struct TokenReplacementSet : IDisposable
    {
        private ValueList<ReadOnlyMemory<char>> _valueList;

        public ReadOnlySpan<char> GetSubstitution(int index)
        {
            return _valueList[index].Span;
        }

        public TokenReplacementSet AddReplacement(string replacement)
            => AddReplacement(replacement.AsMemory());

        public TokenReplacementSet AddReplacement(ReadOnlyMemory<char> replacement)
        {
            _valueList.Add(replacement);
            return this;
        }

        public void Dispose()
        {
            _valueList.Dispose();
        }
    }
}