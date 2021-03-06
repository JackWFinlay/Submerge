using System;

namespace Submerge.Abstractions.Models;

public ref struct TokenMatchSet
{
    public ReadOnlyMemory<char> Input { get; init; }
    public ValueList<TokenMatch> TokenMatches { get; init; }
}