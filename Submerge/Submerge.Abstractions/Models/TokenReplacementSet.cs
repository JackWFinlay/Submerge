using System;
using System.Runtime.CompilerServices;

namespace Submerge.Abstractions.Models;

public struct TokenReplacementSet
{
    private ValueList<ReadOnlyMemory<char>> _valueList;

    public ReadOnlyMemory<char> this[int index] => _valueList[index];
    
    public TokenReplacementSet()
    {
        _valueList = new ValueList<ReadOnlyMemory<char>>();
    }

    public TokenReplacementSet(string[] items)
    {
        _valueList = new ValueList<ReadOnlyMemory<char>>(items.Length);
        AddReplacements(items);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<char> GetSubstitution(int index)
    {
        return _valueList[index].Span;
    }
    
    public TokenReplacementSet AddReplacements(string[] items)
    {
        var memArray = new ReadOnlyMemory<char>[items.Length];
        for (var i = 0; i < items.Length; i++)
        {
            memArray[i] = items[i].AsMemory();
        }

        return AddReplacements(memArray);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TokenReplacementSet AddReplacements(ReadOnlyMemory<char>[] items)
    {
        _valueList.Add(items);
        return this;
    }

    public TokenReplacementSet AddReplacement(string replacement)
        => AddReplacement(replacement.AsMemory());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TokenReplacementSet AddReplacement(ReadOnlyMemory<char> replacement)
    {
        _valueList.Add(replacement);
        return this;
    }

    public void Dispose()
    {
        _valueList.Dispose();
    }
}