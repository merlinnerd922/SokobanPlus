using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * A serializable version of a Vector2Int.
 */
[Serializable]
public class SVector2Int
{
    protected bool Equals(SVector2Int other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == this.GetType() && Equals((SVector2Int) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public readonly int x;
    public readonly int y;

    public SVector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Vector2Int(SVector2Int vec)
    {
        return new(vec.x, vec.y);
    }
    public static implicit operator SVector2Int(Vector2Int vec)
    {
        return new(vec.x, vec.y);
    }
}