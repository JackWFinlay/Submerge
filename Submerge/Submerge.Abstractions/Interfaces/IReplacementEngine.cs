using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Submerge.Abstractions.Models;

namespace Submerge.Abstractions.Interfaces
{
    public interface IReplacementEngine
    {
        ReplaceResult Replace(ReadOnlyMemory<char> raw);

        IEnumerable<ReplaceResult> Replace(ReadOnlyMemory<char> raw, IList<ISubstitutionMap> substitutionMaps);

        ReplaceResult Replace(TokenMatchSet matchSet, ISubstitutionMap substitutionMap);

        TokenMatchSet GetTokenMatchSet(ReadOnlyMemory<char> input);
    }
}