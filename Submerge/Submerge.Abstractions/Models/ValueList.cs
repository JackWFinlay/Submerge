using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Submerge.Abstractions.Models;

/// <summary>
/// Adapted from System.Collections.Generic.ValueListBuilder
/// </summary>
public struct ValueList<T> where T : struct
{
    public int Length { get; private set; }
        
    private Memory<T> _memory;
    private T[] _array;
    private const int _minimumCapacity = 8;
    private const int _maxCapacity = int.MaxValue;
    
    public ref T this[int index] => ref _memory.Span[index];
    
    public ValueList()
    {
        _memory = _array = ArrayPool<T>.Shared.Rent(_minimumCapacity);
        Length = 0;
    }
    
    public ValueList(int initialCapacity)
    {
        var capacityToRent = GetInitialLength(initialCapacity);
        _memory = _array = ArrayPool<T>.Shared.Rent(capacityToRent);
        Length = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        if (Length + 1 > _memory.Length)
        {
            Grow(1);
        }

        _memory.Span[Length] = item;
        Length++;
    }

    public void Add(T[] items)
    {
        var itemsLength = items.Length;
        
        if (Length + itemsLength > _memory.Length)
        {
            Grow(itemsLength);
        }
        
        items.AsSpan().CopyTo(_array);

        Length += itemsLength;
    }
    
    public void Dispose()
    {
        if (_array != null)
        {
            ArrayPool<T>.Shared.Return(_array);
        }
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
        var size = Math.Max((Length + additionLength), Math.Min((_array.Length * 2), _maxCapacity));
        size = Math.Clamp(size, _minimumCapacity, _maxCapacity);
        return size;
    }

    private void Grow(int additionLength)
    {
        var newSize = GetNewArraySize(additionLength);
            
        var newArray = ArrayPool<T>.Shared.Rent(newSize);

        _memory.Slice(0, Length).CopyTo(newArray);

        if (_array != null)
        {
            ArrayPool<T>.Shared.Return(_array);
        }
            
        _memory = _array = newArray;
    }
}