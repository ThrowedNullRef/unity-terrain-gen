using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public sealed class World
{
    // ChunkSize * ChunkSize * ChunkHeight * 8 should never be more than 65536 as unity supports only this much vertexes for each mesh
    // public const int CHUNK_SIZE = 5;
    // public const int CHUNK_HEIGHT = 255;
    public const int CHUNK_SIZE = 8;
    public const int CHUNK_HEIGHT = 80;
    public const int WORLD_SIZE = 30;
    public const int GROUND_HEIGHT = 30;
    
    public Dictionary<Vector3, Chunk> ChunksByPositions { get; } = new ();
    
    public void Generate()
    {
        var chunks = CreateChunks(WORLD_SIZE);
        foreach (var chunk in chunks)
            ChunksByPositions.Add(chunk.WorldPosition, chunk);
    }
    
    public List<Chunk> CreateChunks(int mapSize)
    {
        var chunks = new List<Chunk>();
        for (var x = 0; x < mapSize; x++)
        {
            for (var z = 0; z < mapSize; z++)
            {
                var chunk = new Chunk(new Vector3(x * CHUNK_SIZE, 0f, z * CHUNK_SIZE), CHUNK_SIZE, CHUNK_HEIGHT);
                GenerateVoxels(chunk, 0.01f);
                chunks.Add(chunk);
            }
        }
        return chunks;
    }
    
    private void GenerateVoxels(Chunk chunk, float noiseScale)
    {
        for (var x = 0; x < CHUNK_SIZE; x++)
        {
            for (var z = 0; z < CHUNK_SIZE; z++)
            {
                var absoluteX = chunk.WorldPosition.x + x;
                var absoluteZ = chunk.WorldPosition.z + z;
                var noiseValue = Mathf.PerlinNoise(absoluteX * noiseScale, absoluteZ * noiseScale);
                var groundPosition = Mathf.RoundToInt(noiseValue * CHUNK_HEIGHT);
                
                for (var y = 0; y < CHUNK_HEIGHT; y++)
                {
                    var blockType = BlockType.Dirt;
                    if (y > groundPosition)
                    {
                        blockType = BlockType.Air;
                    }

                    chunk.Blocks[x, y, z] = blockType;
                }
            }
        }
    }

    public bool TryGetChunkNeighbour(Vector3 chunkPosition, Face face, out Chunk neighbourChunk)
    {
        var directionVector = face.GetDirectionVector();
        var neighbourPosition = chunkPosition + directionVector * World.CHUNK_SIZE;
        return ChunksByPositions.TryGetValue(neighbourPosition, out neighbourChunk);
    }
}