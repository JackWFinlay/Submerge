using System;
using System.Collections.Generic;
using Submerge.Abstractions.Models;

namespace Submerge.Abstractions.Interfaces;

public interface IReplacementEngine
{
    string Replace(ReadOnlyMemory<char> raw);

    IEnumerable<string> Replace(ReadOnlyMemory<char> raw, IList<ISubstitutionMap> substitutionMaps);

    string Replace(TokenMatchSet matchSet, ISubstitutionMap substitutionMap);

    string Replace(in FixedTokenMatchSet matchSet, in TokenReplacementSet tokenReplacementSet);

    TokenMatchSet GetTokenMatchSet(ReadOnlyMemory<char> input);
        
    FixedTokenMatchSet GetFixedTokenMatchSet(ReadOnlyMemory<char> input);

}