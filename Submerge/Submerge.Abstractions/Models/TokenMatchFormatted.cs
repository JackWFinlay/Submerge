using System;
using Submerge.Abstractions.Interfaces;

namespace Submerge.Abstractions.Models;

public readonly struct TokenMatchFormatted : ITokenMatch
{
    public ReadOnlyMemory<char> Token { get; init; }
    public ReadOnlyMemory<char> Format { get; init; }
    public int Index { get; init; }
}