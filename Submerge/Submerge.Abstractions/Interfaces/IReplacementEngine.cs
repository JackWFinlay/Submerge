using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Submerge.DataStructures;

namespace Submerge.Abstractions.Interfaces
{
    public interface IReplacementEngine
    {
        Task<ReadOnlyMemory<ReadOnlyMemory<char>>> ReplaceAsync(ReadOnlyMemory<char> raw, out int length);

        IEnumerable<ReplaceResult> ReplaceWithMatchIndexList(ReadOnlyMemory<char> raw, 
            IEnumerable<IDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>> substitutionMaps);
    }
}