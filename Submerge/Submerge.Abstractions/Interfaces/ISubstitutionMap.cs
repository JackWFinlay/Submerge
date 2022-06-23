using System;

namespace Submerge.Abstractions.Interfaces;

public interface ISubstitutionMap
{
    ISubstitutionMap UpdateOrAddMapping(string token, string substitution);
    bool TryGetValue(ReadOnlyMemory<char> token, out ReadOnlyMemory<char> value);
}