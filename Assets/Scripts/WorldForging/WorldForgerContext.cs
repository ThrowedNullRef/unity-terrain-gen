using System.Collections.Generic;

public sealed class WorldForgerContext
{
    public WorldForgerContext(int amountOfChunks)
    {
        Chunks = new List<Chunk>(amountOfChunks);
    }
    
    public List<Chunk> Chunks { get; }
}