using System;
using System.Runtime.CompilerServices;

namespace Submerge.Extensions;

public static class MemoryExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfTokenStart(this ReadOnlyMemory<char> span, ReadOnlyMemory<char> token)
    {
        for (var i = 0; i < span.Length; i++)
        {
            for (var j = 0; j < token.Length; j++)
            {
                if (span.Span[i + j] != token.Span[j])
                {
                    break;
                }

                // Continue if not last item in token.
                if (j != (token.Length - 1))
                {
                    continue;
                }

                return i;
            }
        }

        return -1;
    }
}