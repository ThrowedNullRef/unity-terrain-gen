using UnityEngine;

public sealed class Chunk
{
    public Chunk(Vector3 worldPosition, BlockType[,,] blocks)
    {
        WorldPosition = worldPosition;
        Blocks = blocks;
        ChunkSize = blocks.GetLength(0);
        ChunkHeight = blocks.GetLength(1);
    }

    public Vector3 WorldPosition { get; }

    public BlockType[,,] Blocks { get; }

    public int ChunkSize { get; }

    public int ChunkHeight { get; }

    public BlockType GetBlockType(Vector3Int relativePos)
    {
        return GetBlockType(relativePos.x, relativePos.y, relativePos.z);
    }

    public bool IsPositionInRange(int x, int y, int z) =>
        x >= 0 && y >= 0 && z >= 0 &&
        x < ChunkSize && z < ChunkSize && y < ChunkHeight;

    public BlockType GetBlockType(int x, int y, int z) => 
        !IsPositionInRange(x, y, z) ? BlockType.Nothing : Blocks[x, y, z];

    public BlockType GetBlockTypeOfNeighbour(Vector3Int relativePos, Face face) =>
        GetBlockTypeOfNeighbour(relativePos.x, relativePos.y, relativePos.z, face);
    
    public BlockType GetBlockTypeOfNeighbour(int x, int y, int z, Face face)
    {
        var direction = face.GetDirectionVector();
        return GetBlockType(direction.x + x, direction.y + y, direction.z + z);
    }
}