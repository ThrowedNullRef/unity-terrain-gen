using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public sealed class World
{
    public Dictionary<Vector3, Chunk> ChunksByPositions { get; } = new ();
    
    public void Generate()
    {
        BlockType[,,] blocks =
        {
            { { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt }, { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt }, { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt } },
            { { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt }, { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt }, { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt } },
            { { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt }, { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt }, { BlockType.Dirt, BlockType.Dirt, BlockType.Dirt } }
        };

        blocks[0, 0, 0] = BlockType.Air;
        blocks[0, 1, 0] = BlockType.Air;

        var pos = new Vector3(0, 0, 0);
        ChunksByPositions.Add(pos, new Chunk(pos, blocks));
    }

    public bool TryGetChunkNeighbour(Vector3 chunkPosition, Face face, out Chunk neighbourChunk)
    {
        var directionVector = face.GetDirectionVector();
        var neighbourPosition = chunkPosition + directionVector;
        return ChunksByPositions.TryGetValue(neighbourPosition, out neighbourChunk);
    }
}