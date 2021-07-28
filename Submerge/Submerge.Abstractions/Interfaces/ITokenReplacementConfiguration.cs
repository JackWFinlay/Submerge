using System;
using System.Collections.Generic;

namespace Submerge.Abstractions.Interfaces
{
    public interface ITokenReplacementConfiguration
    {
        ReadOnlyMemory<char> TokenStart { get; set; }
        public ReadOnlyMemory<char> TokenEnd { get; set; }
        public IDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> SubstitutionMap { get; init; }
        
        void UpdateOrAddMapping(string token, string substitution);

        void UpdateOrAddFromObject(object item);
    }
}