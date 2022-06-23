using Submerge.Abstractions.Interfaces;

namespace Submerge.Abstractions.Models;

public struct FixedTokenMatch
{
    public int Index { get; init; }
    public int TokenLength { get; init; }
}