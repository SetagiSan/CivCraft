using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(requiredComponent: typeof(MeshFilter), requiredComponent2: typeof(MeshRenderer))]
public class ChankRenderer : MonoBehaviour
{
    public const int ChunkWeight = 10;
    public const int ChunkHeight = 128;
    public const float BlockScale = .5f;

    public ChunkData ChunkData;
    public GameWorld ParentWorld;

    private Mesh chunkMesh;

    private List<Vector3> verticies = new List<Vector3>();
    private List<int> triangles = new List<int>();

    private void AddLastVerticiesSquare()
    {
        triangles.Add(item: verticies.Count - 4);
        triangles.Add(item: verticies.Count - 3);
        triangles.Add(item: verticies.Count - 2);

        triangles.Add(item: verticies.Count - 3);
        triangles.Add(item: verticies.Count - 1);
        triangles.Add(item: verticies.Count - 2);
    }
    public void SpawnBlock(Vector3Int blockPosition)
    {
        ChunkData.Blocks[blockPosition.x, blockPosition.y, blockPosition.z] = BlockType.Grass;
        RegenerateMesh();
    }

    public void DestroyBlock(Vector3Int blockPosition)
    {
        ChunkData.Blocks[blockPosition.x, blockPosition.y, blockPosition.z] = BlockType.Air;
        RegenerateMesh();
    }

    private void GenerateBlock(int x,int y,int z)
    {
        var blockPosition = new Vector3Int(x, y, z);
        if (GetBlockAtPosition(blockPosition)==0) return;

        if (GetBlockAtPosition(blockPosition + Vector3Int.right) == 0) GenerateRightSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.left) == 0) GenerateLeftSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.forward) == 0) GenerateFrontSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.back) == 0) GenerateBackSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.up) == 0) GenerateTopSide(blockPosition);
        if (GetBlockAtPosition(blockPosition + Vector3Int.down) == 0) GenerateBottomSide(blockPosition);
    }

    private BlockType GetBlockAtPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < ChunkWeight &&
            blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < ChunkWeight)
            return ChunkData.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        else
        {
            if (blockPosition.y < 0 || blockPosition.y >= ChunkWeight) return BlockType.Air;

            Vector2Int adjacentChunkPosition = ChunkData.ChunkPosition;
            if (blockPosition.x < 0)
            {
                adjacentChunkPosition.x --;
                blockPosition.x += ChunkWeight;
            }
            else if (blockPosition.x >= ChunkWeight)
            {
                adjacentChunkPosition.x++;
                blockPosition.x -= ChunkWeight;
            }

            if (blockPosition.z < 0)
            {
                adjacentChunkPosition.y--;
                blockPosition.z += ChunkWeight;
            }
            else if (blockPosition.z >= ChunkWeight)
            {
                adjacentChunkPosition.y++;
                blockPosition.z -= ChunkWeight;
            }
            if (ParentWorld.ChankDatas.TryGetValue(adjacentChunkPosition, out ChunkData adjacentChunk)){
                return adjacentChunk.Blocks[blockPosition.x, blockPosition.y,blockPosition.z];
            }
            else return BlockType.Air; 
        }
    }

    private void GenerateRightSide(Vector3Int blockPosition)
    {
        verticies.Add(item: (new Vector3(x: 1, y: 0, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 1, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 0, z: 1) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 1, z: 1) + (Vector3)blockPosition)*BlockScale);
        AddLastVerticiesSquare();
    }
    private void GenerateLeftSide(Vector3Int blockPosition)
    {

        verticies.Add(item: (new Vector3(x: 0, y: 0, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 0, y: 0, z: 1) + (Vector3)blockPosition) * BlockScale);
        verticies.Add(item: (new Vector3(x: 0, y: 1, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 0, y: 1, z: 1) + (Vector3)blockPosition)*BlockScale);
        AddLastVerticiesSquare();
    }
    private void GenerateFrontSide(Vector3Int blockPosition)
    {
        verticies.Add(item: (new Vector3(x: 0, y: 0, z: 1) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 0, z: 1) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 0, y: 1, z: 1) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 1, z: 1) + (Vector3)blockPosition)*BlockScale);
        AddLastVerticiesSquare();
    }
     private void GenerateBackSide(Vector3Int blockPosition)
    {
        verticies.Add(item: (new Vector3(x: 0, y: 0, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 0, y: 1, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 0, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 1, z: 0) + (Vector3)blockPosition)*BlockScale);
        AddLastVerticiesSquare();
    }
      private void GenerateTopSide(Vector3Int blockPosition)
    {
        verticies.Add(item: (new Vector3(x: 0, y: 1, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 0, y: 1, z: 1) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 1, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 1, z: 1) + (Vector3)blockPosition)*BlockScale);
        AddLastVerticiesSquare();
    }
      private void GenerateBottomSide(Vector3Int blockPosition)
    {
        verticies.Add(item: (new Vector3(x: 0, y: 0, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 0, z: 0) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 0, y: 0, z: 1) + (Vector3)blockPosition)*BlockScale);
        verticies.Add(item: (new Vector3(x: 1, y: 0, z: 1) + (Vector3)blockPosition)*BlockScale);
        AddLastVerticiesSquare();
    }

    private void RegenerateMesh()
    {
        verticies.Clear();
        triangles.Clear();
        for (int y = 0; y < ChunkHeight; y++)
        {
            for (int x = 0; x < ChunkWeight; x++)
            {
                for (int z = 0; z < ChunkWeight; z++)
                {
                    GenerateBlock(x, y, z);
                }
            }
        }
        chunkMesh.triangles = new int[0];
        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize();

        chunkMesh.RecalculateNormals();
        chunkMesh.RecalculateBounds();

        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    void Start()
    {
        chunkMesh = new Mesh();

        for (int y = 0;y < ChunkHeight; y++)
        {
            for (int x = 0;x< ChunkWeight; x++)
            {
                for(int z = 0;z< ChunkWeight; z++)
                {
                    GenerateBlock(x,y,z);
                }
            }
        }


        chunkMesh.vertices = verticies.ToArray();
        chunkMesh.triangles = triangles.ToArray();

        chunkMesh.Optimize();

        chunkMesh.RecalculateNormals();
        chunkMesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        GetComponent<MeshCollider>().sharedMesh = chunkMesh;

    }
}
