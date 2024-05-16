using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public Dictionary<Vector2Int, ChunkData> ChankDatas = new Dictionary<Vector2Int, ChunkData>();
    public ChankRenderer ChunkPrefab;
    public int ChunkCount = 10;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        for (int x = 0;x < ChunkCount; x++)
        {
            for(int y = 0;y < ChunkCount; y++)
            {
                float xPos = x * ChankRenderer.ChunkWeight * ChankRenderer.BlockScale;
                float zPos = y * ChankRenderer.ChunkWeight * ChankRenderer.BlockScale;
                
                ChunkData chunkData=new ChunkData();
                chunkData.ChunkPosition = new Vector2Int(x, y);
                chunkData.Blocks = TerrainGenerator.GenerateTerrain(xPos,zPos);
                ChankDatas.Add(new Vector2Int(x, y), chunkData);

                var chunk = Instantiate(ChunkPrefab, position: new Vector3(xPos, y: 0, zPos),Quaternion.identity,transform);
                chunk.ChunkData = chunkData;
                chunk.ParentWorld = this;
                chunkData.Renderer = chunk;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            bool isDestroying = Input.GetMouseButtonDown(0);

            Ray ray = mainCamera.ViewportPointToRay(pos: new Vector3(x: 0.5f, y:0.5f));

            if (Physics.Raycast(ray,out var hitInfo))
            {
                Vector3 blockCenter;
                if(isDestroying)
                {
                blockCenter = hitInfo.point - hitInfo.normal * ChankRenderer.BlockScale / 2;
                }
                else blockCenter = hitInfo.point + hitInfo.normal * ChankRenderer.BlockScale / 2;
                Vector3Int blockWorldPos = Vector3Int.FloorToInt(blockCenter / ChankRenderer.BlockScale);
                Vector2Int chunkPos = GetChunkContainingBlock(blockWorldPos);
                if (ChankDatas.TryGetValue(chunkPos,out ChunkData chunkData))
                {
                    Vector3Int chunkOrigin = new Vector3Int(chunkPos.x, y:0,z:chunkPos.y)*ChankRenderer.ChunkWeight;
                    if(isDestroying)
                    { 
                    chunkData.Renderer.DestroyBlock(blockWorldPos - chunkOrigin);
                    }
                    else chunkData.Renderer.SpawnBlock(blockWorldPos - chunkOrigin);
                }
            }
        }
    }
    public Vector2Int GetChunkContainingBlock(Vector3Int blockWorldPos)
    {
        return new Vector2Int(x:blockWorldPos.x/ ChankRenderer.ChunkWeight,y:blockWorldPos.z/ ChankRenderer.ChunkWeight);
    }
}
