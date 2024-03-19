using UnityEngine;

public sealed class WorldRenderer : MonoBehaviour
{
    private readonly World _world = new ();
    
    public GameObject chunkPrefab;

    private void Start()
    {
        _world.Generate();

        foreach (var chunk in _world.ChunksByPositions.Values)
        {
            var chunkObject = Instantiate(chunkPrefab, chunk.WorldPosition, Quaternion.identity);
            var chunkRenderer = chunkObject.GetComponent<ChunkRenderer>();
            chunkRenderer.Initialize(chunk, _world);
            chunkRenderer.Rerender();
        }
    }
}