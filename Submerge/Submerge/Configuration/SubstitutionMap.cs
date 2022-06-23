using System;
using System.Collections.Generic;
using Submerge.Abstractions.Interfaces;
using Submerge.Comparison;

namespace Submerge.Configuration;

public class SubstitutionMap : Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>, ISubstitutionMap
{
    public SubstitutionMap() : base(new ReadOnlyMemoryComparer())
    {
    }

    public ISubstitutionMap UpdateOrAddMapping(string token, string substitution)
    {
        UpdateOrAddMapping(token.AsMemory(), substitution.AsMemory());
        return this;
    }
        
    private void UpdateOrAddMapping(ReadOnlyMemory<char> token, ReadOnlyMemory<char> substitution)
    {
        if (ContainsKey(token))
        {
            this[token] = substitution;
            return;
        }
            
        Add(token, substitution);
    }
}