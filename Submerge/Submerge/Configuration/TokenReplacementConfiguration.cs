using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Submerge.Abstractions.Interfaces;
using Submerge.Comparison;

namespace Submerge.Configuration
{
    public class TokenReplacementConfiguration : ITokenReplacementConfiguration
    {
        public ReadOnlyMemory<char> TokenStart { get; set; }
        public ReadOnlyMemory<char> TokenEnd { get; set; }
        public IDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>> SubstitutionMap { get; init; } =
            new Dictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>(new ReadOnlyMemoryComparer());
        
        public void UpdateOrAddMapping(string token, string substitution)
        {
            UpdateOrAddMapping(token.AsMemory(), substitution.AsMemory());
        }

        public void UpdateOrAddFromObject(object item)
        {
            var properties = item.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(prop =>
                    new KeyValuePair<ReadOnlyMemory<char>, 
                        ReadOnlyMemory<char>>(prop.Name.ToLowerInvariant().AsMemory(),
                                              prop.GetValue(item).ToString().AsMemory()));

            foreach (var (key, value) in properties)
            {
                UpdateOrAddMapping(key, value);
            }
        }

        internal void UpdateOrAddMapping(ReadOnlyMemory<char> token, ReadOnlyMemory<char> substitution)
        {
            if (SubstitutionMap.ContainsKey(token))
            {
                SubstitutionMap[token] = substitution;
                return;
            }
            
            SubstitutionMap.Add(token, substitution);
        }
    }
}