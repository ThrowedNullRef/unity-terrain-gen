using UnityEngine;

public static class Vector3IntExtensions
{
    public static Vector3Int TranslateToNeighbourChunkPosition(this Vector3Int position, Face face)
    {
        var directionVector = face.GetDirectionVector();

        var neighbourX = position.x;
        var neighbourY = position.y;
        var neighbourZ = position.z;

        neighbourX = directionVector.x switch
        {
            1 => 0,
            -1 => World.CHUNK_SIZE - 1,
            _ => neighbourX
        };

        neighbourY = directionVector.y switch
        {
            1 => 0,
            -1 => World.CHUNK_SIZE - 1,
            _ => neighbourY
        };

        neighbourZ = directionVector.z switch
        {
            1 => 0,
            -1 => World.CHUNK_SIZE - 1,
            _ => neighbourZ
        };

        return new Vector3Int(neighbourX, neighbourY, neighbourZ);
    }
}