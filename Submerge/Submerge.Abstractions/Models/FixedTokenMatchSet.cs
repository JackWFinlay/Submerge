using System;

namespace Submerge.Abstractions.Models;

public struct FixedTokenMatchSet
{
    public ReadOnlyMemory<char> Input { get; init; }
    public ValueList<FixedTokenMatch> TokenMatches { get; init; }
}