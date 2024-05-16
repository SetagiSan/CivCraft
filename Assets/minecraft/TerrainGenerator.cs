using UnityEngine;
public static class TerrainGenerator
{
    public static BlockType[,,] GenerateTerrain(float x0Offset,float y0Offset)
    {
        var result = new BlockType[ChankRenderer.ChunkWeight, ChankRenderer.ChunkHeight, ChankRenderer.ChunkWeight];
        for (int x = 0; x < ChankRenderer.ChunkWeight; x++)
        {
            for(int z = 0; z < ChankRenderer.ChunkWeight; z++)
            {
                float height = Mathf.PerlinNoise(x: (x + x0Offset) * .2f, y: (z+y0Offset) * .2f) * 5 + 10;

                for(int y = 0; y < height; y++)
                {
                    result[x,y,z] = BlockType.Grass;
                }
            }
        }
        return result;
    }
   
}
