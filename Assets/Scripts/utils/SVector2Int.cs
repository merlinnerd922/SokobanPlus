using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * A serializable version of a SVector2Int.
 */
[Serializable]
public class SVector2Int
{
    protected bool Equals(SVector2Int other)
    {
        return x == other.x && y == other.y;
    }
    
    public static SVector2Int operator + (SVector2Int thisVec, SVector2Int other)
    {
        return new(thisVec.x + other.x, thisVec.y + other.y);
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

    public int x;
    public int y;
    public static SVector2Int left = new(-1, 0);

    public SVector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static SVector2Int right = new(1, 0);
    public static SVector2Int up = new(0, 1);
    public static SVector2Int down = new(0, -1);
}