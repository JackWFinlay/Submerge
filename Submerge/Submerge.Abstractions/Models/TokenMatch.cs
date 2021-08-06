using System;

namespace Submerge.Abstractions.Models
{
    public struct TokenMatch
    {
        public ReadOnlyMemory<char> Token { get; init; }
        public int Index { get; init; }
    }
}