using System;
using Submerge.Abstractions.Interfaces;

namespace Submerge.Abstractions.Models
{
    public readonly struct TokenMatch : ITokenMatch
    {
        public ReadOnlyMemory<char> Token { get; init; }
        public int Index { get; init; }
    }
}