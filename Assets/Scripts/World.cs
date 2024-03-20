using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class World
{
    private readonly WorldForger _worldForger = new();
    
    // ChunkSize * ChunkSize * ChunkHeight * 8 should never be more than 65536 as unity supports only this much vertexes for each mesh
    // public const int CHUNK_SIZE = 5;
    // public const int CHUNK_HEIGHT = 255;
    public int ChunkSize = 8;
    public int ChunkHeight = 80;
    public int WorldSize = 30;
    
    public Dictionary<Vector3, Chunk> ChunksByPositions { get; } = new ();
    
    public void Generate()
    {
        var positions = new List<Vector3>(WorldSize * 2);
        for (var x = 0; x < WorldSize; ++x)
        for (var z = 0; z < WorldSize; ++z)
        {
            var position = new Vector3(x * ChunkSize, 0f, z * ChunkSize);
            positions.Add(position);
        }

        var forgeRequest = new WorldForgerRequest(positions, this);
        var context = _worldForger.Forge(forgeRequest);
        foreach (var chunk in context.Chunks)
        {
            ChunksByPositions.Add(chunk.WorldPosition, chunk);
        }
    }
    
    public bool TryGetChunkNeighbour(Vector3 chunkPosition, Face face, out Chunk neighbourChunk)
    {
        var directionVector = face.GetDirectionVector();
        var neighbourPosition = chunkPosition + directionVector * ChunkSize;
        return ChunksByPositions.TryGetValue(neighbourPosition, out neighbourChunk);
    }
}