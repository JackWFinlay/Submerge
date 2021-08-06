using System;

namespace Submerge.Abstractions.Models
{
    public struct ReplaceResult
    {
        public ReadOnlyMemory<ReadOnlyMemory<char>> Memory { get; init; }
        public int Length { get; init; }
    }
}