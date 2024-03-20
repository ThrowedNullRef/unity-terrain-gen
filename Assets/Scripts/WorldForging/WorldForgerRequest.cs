using System.Collections.Generic;
using UnityEngine;

public sealed class WorldForgerRequest
{
    public WorldForgerRequest(List<Vector3> chunkPositions, World world)
    {
        ChunkPositions = chunkPositions;
        World = world;
    }
    
    public List<Vector3> ChunkPositions { get; }
    
    public World World { get; }
}