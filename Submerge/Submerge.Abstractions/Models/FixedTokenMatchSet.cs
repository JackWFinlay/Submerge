using System;

namespace Submerge.Abstractions.Models
{
    public class FixedTokenMatchSet
    {
        public ReadOnlyMemory<char> Input { get; init; }
        public ValueList<FixedTokenMatch> TokenMatches { get; init; }
    }
}