using System;
using UnityEngine;

public static class FaceExtensions
{
    public static Vector3Int GetDirectionVector(this Face face)
    {
        return face switch
        {
            Face.Top => Vector3Int.up,
            Face.Bottom => Vector3Int.down,
            Face.Right => Vector3Int.right,
            Face.Left => Vector3Int.left,
            Face.Front => Vector3Int.back,
            Face.Back => Vector3Int.forward,
            _ => throw new Exception("Invalid input direction")
        };
    }
}