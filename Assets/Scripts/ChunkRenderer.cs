using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ChunkRenderer : MonoBehaviour
{
    private Chunk _chunk;
    private World _world;
    private Mesh _mesh;
    private MeshFilter _meshFilter;

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
    }

    public void Initialize(Chunk chunk, World world)
    {
        _chunk = chunk;
        _world = world;
    }

    public void Rerender()
    {
        if (_chunk is null || _world is null)
            throw new Exception("Not initialized");

        var mesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();

        var counter = 0;
        for (var x = 0; x < _chunk.ChunkSize; ++x)
        {
            for (var z = 0; z < _chunk.ChunkSize; ++z)
            {
                for (var y = 0; y < _chunk.ChunkHeight; ++y)
                {
                    var blockType = _chunk.Blocks[x, y, z];
                    if (blockType is BlockType.Air or BlockType.Nothing)
                        continue;
                    
                    var blockWorldPosition = new Vector3(x + _chunk.WorldPosition.x, y + _chunk.WorldPosition.y, z + _chunk.WorldPosition.z);
                    vertices.AddRange(new []
                    {
                        new Vector3(0, 0, 0) + blockWorldPosition,
                        new Vector3(1, 0, 0) + blockWorldPosition,
                        new Vector3(1, 1, 0) + blockWorldPosition,
                        new Vector3(0, 1, 0) + blockWorldPosition,
                        new Vector3(0, 1, 1) + blockWorldPosition,
                        new Vector3(1, 1, 1) + blockWorldPosition,
                        new Vector3(1, 0, 1) + blockWorldPosition,
                        new Vector3(0, 0, 1) + blockWorldPosition
                    });
        
                    var faces = new List<Face>();
                    foreach (var face in Faces.All)
                    {
                        var neighbourBlockType = _chunk.GetBlockTypeOfNeighbour(x, y, z, face);
                        if (neighbourBlockType == BlockType.Nothing && _world.TryGetChunkNeighbour(_chunk.WorldPosition, face, out var neighbourChunk))
                            neighbourBlockType = neighbourChunk.GetBlockType(x, y, z);

                        if (neighbourBlockType == BlockType.Dirt)
                            continue;
                        
                        faces.Add(face);
                    }
        
                    var nextTriangles = CalculateTriangles(faces).Select(triangle => triangle + (counter * 8)).ToList();
                    triangles.AddRange(nextTriangles);
                    ++counter;
                }
            }   
        }
        
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        _meshFilter.mesh = mesh;
    }

    private List<int> CalculateTriangles(List<Face> facesToRender)
    {
        var triangles = new List<int>(facesToRender.Count * 6);

        foreach (var face in facesToRender)
            switch (face)
            {
                case Face.Bottom:
                    triangles.AddRange(new List<int> { 0, 6, 7, 0, 1, 6 });
                    break;
                case Face.Left:
                    triangles.AddRange(new List<int> { 0, 7, 4, 0, 4, 3 });
                    break;
                case Face.Back:
                    triangles.AddRange(new List<int> { 5, 4, 7, 5, 7, 6 });
                    break;
                case Face.Right:
                    triangles.AddRange(new List<int> { 1, 2, 5, 1, 5, 6 });
                    break;
                case Face.Front:
                    triangles.AddRange(new List<int> { 0, 2, 1, 0, 3, 2 });
                    break;
                case Face.Top:
                    triangles.AddRange(new List<int> { 2, 3, 4, 2, 4, 5 });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        return triangles;
    }
}