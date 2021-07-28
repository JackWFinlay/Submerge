using System;

namespace Submerge.DataStructures
{
    internal struct TokenMatch
    {
        public ReadOnlyMemory<char> Token { get; init; }
        public int Index { get; init; }
    }
}