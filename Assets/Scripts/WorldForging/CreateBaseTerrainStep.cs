using UnityEngine;

public sealed class CreateBaseTerrainStep : IWorldForgerStep
{
    private readonly WorldForgerContext _context;
    private readonly WorldForgerRequest _request;

    public CreateBaseTerrainStep(WorldForgerContext context, WorldForgerRequest request)
    {
        _context = context;
        _request = request;
    }
    
    public void Forge()
    {
        foreach (var chunkPosition in _request.ChunkPositions)
        {
            var chunk = new Chunk(chunkPosition, _request.World.ChunkSize, _request.World.ChunkHeight);
            GenerateVoxels(chunk, 0.01f);
            _context.Chunks.Add(chunk);
        }
    }
    
    private void GenerateVoxels(Chunk chunk, float noiseScale)
    {
        for (var x = 0; x < _request.World.ChunkSize; x++)
        {
            for (var z = 0; z < _request.World.ChunkSize; z++)
            {
                var absoluteX = chunk.WorldPosition.x + x;
                var absoluteZ = chunk.WorldPosition.z + z;
                var noiseValue = Mathf.PerlinNoise(absoluteX * noiseScale, absoluteZ * noiseScale);
                var groundPosition = Mathf.RoundToInt(noiseValue * _request.World.ChunkHeight);
                
                for (var y = 0; y < _request.World.ChunkHeight; y++)
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
}