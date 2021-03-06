using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Submerge.Abstractions.Models;

// Adapted from System.Collections.Generic.ValueListBuilder,
// and System.Runtime.CompilerServices.DefaultInterpolatedStringHandler
/// <summary>
/// A fast and efficient store for the values that make up a new string created by the TokenReplacementEngine.
/// </summary>
public ref struct StringValueList
{
    // A span to give us some fast access to the memory underneath _array.
    private Span<char> _span;
        
    // Backing array for the string.
    private char[] _array;
        
    // Tracks the current length of the string withing the _span.
    private int _pos;
        
    // We are assuming here that we want a reasonably sized result string,
    // so reduce Grow operations by making the default large. TODO: Tune as necessary.
    private const int _minimumCapacity = 256;

    private const int _defaultHoleSize = 16; 
        
    // A bit lower than the theoretical limit of 2^30ish as there's no clear information on what the *real* limit is.
    private const int _maxCapacity = 1_000_000_000;

    /// <summary>
    /// Initialise a <see cref="StringValueList"/> with the specified capacity.
    /// </summary>
    public StringValueList(int initialCapacity)
    {
        var capacityToRent = GetInitialLength(initialCapacity);
        _array = ArrayPool<char>.Shared.Rent(capacityToRent);
        _span = _array;
        _pos = 0;
    }
    
    /// <summary>
    /// Initialise a <see cref="StringValueList"/> with the specified capacity.
    /// </summary>
    public StringValueList(int rawLength, int numberOfSubstitutions)
    {
        var initialCapacityEstimate = rawLength + (numberOfSubstitutions * _defaultHoleSize);
        var capacityToRent = GetInitialLength(initialCapacityEstimate);
        _array = ArrayPool<char>.Shared.Rent(capacityToRent);
        _span = _array;
        _pos = 0;
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(ReadOnlySpan<char> value)
    {
        // Where theres only a single character, don't engage a CopyTo, just write it.
        if (value.Length == 1)
        {
            if (_pos < _span.Length)
            {
                _span[_pos] = value[0];
                _pos++;
            }
            else
            {
                Grow(1);
                _span[_pos] = value[0];
                _pos++;
            }
            
            return;
        }

        if (value.Length + _pos > _span.Length)
        {
            GrowThenCopy(value);
            return;
        }

        AddDirect(value);
    }
    
    private void GrowThenCopy(ReadOnlySpan<char> value)
    {
        Grow(value.Length);
        value.CopyTo(_span.Slice(_pos, value.Length));

        _pos += value.Length;
    }

    private void AddDirect(ReadOnlySpan<char> value)
    {
        if (value.TryCopyTo(_span.Slice(_pos)))
        {
            _pos += value.Length;
        }
        else
        {
            GrowThenCopy(value);
        }
    }

    public override string ToString()
    {
        var s = new string(_span.Slice(0, _pos));
        Dispose();
        return s;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetInitialLength(int initialLength)
    {
        var size = Math.Min(initialLength, _maxCapacity);
        size = Math.Clamp(size, _minimumCapacity, _maxCapacity);
        return size;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetNewArraySize(int additionLength)
    {
        var size = Math.Max((_pos + additionLength), Math.Min((_array.Length * 2), _maxCapacity));
        size = Math.Clamp(size, _minimumCapacity, _maxCapacity);
        return size;
    }
        
    private void Grow(int additionLength)
    {
        var newSize = GetNewArraySize(additionLength);
            
        var newArray = ArrayPool<char>.Shared.Rent(newSize);

        _span.Slice(0, _pos).CopyTo(newArray);

        if (_array != null)
        {
            ArrayPool<char>.Shared.Return(_array);
        }
            
        _span = _array = newArray;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Dispose()
    {
        var array = _array;
        this = default; // Reset this instance, we are done with it, any new adds will be to a new StringValueList.
        if (array != null)
        {
            ArrayPool<char>.Shared.Return(array);
        }
    }
}