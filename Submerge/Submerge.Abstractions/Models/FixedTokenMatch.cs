using Submerge.Abstractions.Interfaces;

namespace Submerge.Abstractions.Models
{
    public readonly struct FixedTokenMatch : ITokenMatch
    {
        public int Index { get; init; }
        public int TokenLength { get; init; }
    }
}