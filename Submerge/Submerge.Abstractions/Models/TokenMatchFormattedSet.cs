namespace Submerge.Abstractions.Models;

public ref struct TokenMatchFormattedSet
{
    public ValueList<TokenMatch> TokenMatches { get; init; }
}