using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Submerge.Abstractions.Interfaces;
using Submerge.Configuration;
using Submerge.ReplacementEngines;

namespace Submerge
{
    public class SubmergeTokenReplacer
    {
        private readonly IReplacementEngine _replacementEngine;
        
        public SubmergeTokenReplacer(ITokenReplacementConfiguration config)
        {
            _replacementEngine = new TokenReplacementEngine(config);
        }
        
        public async Task<string> ReplaceAsync(ReadOnlyMemory<char> input) => await EscapeAsync(input);

        public async Task<string> ReplaceAsync(string input) => await EscapeAsync(input);

        public IEnumerable<string> ReplaceAsync(string input,
            IEnumerable<IDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>> substitutionMaps) =>
                 ReplaceAsync(input.AsMemory(), substitutionMaps);

        public IEnumerable<string> ReplaceAsync(ReadOnlyMemory<char> input,
            IEnumerable<IDictionary<ReadOnlyMemory<char>, ReadOnlyMemory<char>>> substitutionMaps)
        {
            var results = _replacementEngine.ReplaceWithMatchIndexList(input, substitutionMaps);

            foreach (var result in results)
            {
                var escapedMemory = result.Memory;
                var length = result.Length;
                
                if (result.Memory.IsEmpty)
                {
                    escapedMemory = new ReadOnlyMemory<ReadOnlyMemory<char>>(new[] { input });
                    length = input.Length;
                }

                var escaped = CreateString(escapedMemory, length);

                yield return escaped;
            }
        }

        private async Task<string> EscapeAsync(string text)
        {
            var escaped = await ReplaceAsync(text.AsMemory());
            
            return escaped;
        }
        
        private async Task<string> EscapeAsync(ReadOnlyMemory<char> input)
        {
            var escapedMemory = await _replacementEngine.ReplaceAsync(input, out var length);

            if (escapedMemory.IsEmpty)
            {
                escapedMemory = new ReadOnlyMemory<ReadOnlyMemory<char>>(new[] { input });
                length = input.Length;
            }

            var escaped = CreateString(escapedMemory, length);

            return escaped;
        }

        private string CreateString(ReadOnlyMemory<ReadOnlyMemory<char>> input, int length)
        {
#if NETSTANDARD2_0
            var stringBuilder = new StringBuilder();
            var span = input.Span;
            
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < span[i].Length; i++)
                {
                    stringBuilder.Append(span[i].Span[j]);
                }
            }
            var escaped = stringBuilder.ToString();
#else
                var escaped = string.Create(length, input,
                (destination, state) =>
                {
                    int prevLength = 0;
                    for (var i = 0; i < state.Length; i++)
                    {
                        state.Span[i].Span.CopyTo(destination.Slice(prevLength));
                        prevLength += state.Span[i].Length;
                    }
                });
#endif
            return escaped;
        }
    }
}