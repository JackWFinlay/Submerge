using System;

namespace Submerge.Abstractions.Models
{
    public readonly struct TokenMatch
    {
        public ReadOnlyMemory<char> Token { get; init; }
        public ReadOnlyMemory<char> Replacement { get; init; }
        public int Index { get; init; }
    }
}