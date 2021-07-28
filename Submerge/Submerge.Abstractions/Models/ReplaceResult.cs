using System;

namespace Submerge.DataStructures
{
    public struct ReplaceResult
    {
        public ReadOnlyMemory<ReadOnlyMemory<char>> Memory { get; init; }
        public int Length { get; init; }
    }
}