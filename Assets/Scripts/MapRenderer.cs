using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject wallPrefab;

    Dictionary<Vector2Int, GameObject> renderedChunks = new();
    

    public void RenderChunk(Chunk chunk)
    {
        // 1. Guard against double rendering
        if (renderedChunks.ContainsKey(chunk.chunkCoords))
        {
            return;
        }

        // 2. Instantiate container GameObject
        GameObject chunkRoot = new GameObject($"Chunk_{chunk.chunkCoords.x}_{chunk.chunkCoords.y}");

        // 3. Set world position & parent
        Vector3 worldPos = new Vector3(chunk.chunkCoords.x * chunk.chunkSize, chunk.chunkCoords.y * chunk.chunkSize, 0f);
        chunkRoot.transform.position = worldPos;
        chunkRoot.transform.parent = this.transform;

        

        foreach (Edge e in chunk.allEdges)
        {
            if (!e.isWall) continue; // Skip non-walls early

            Debug.Log($"Edge from {e.startPoint} to {e.endPoint} | IsH: {e.IsHorizontal} | IsV: {e.IsVertical}");

            GameObject wallInstance = Instantiate(wallPrefab, chunkRoot.transform);

            if (e.IsHorizontal)
            {
                // 1. Calculate length
                float length = Mathf.Abs(e.startPoint.x - e.endPoint.x);

                // 2. Find the center point X so the sprite stretches evenly left and right
                float centerX = (e.startPoint.x + e.endPoint.x) / 2f;
                Vector3 wallCenter = new Vector3(centerX, e.startPoint.y, 0f);

                // 3. Apply position and scale
                wallInstance.transform.localPosition = wallCenter;
                wallInstance.transform.localScale = new Vector3(length, 0.3f, 1f);
            }
            else if (e.IsVertical)
            {
                // 1. Calculate length
                float length = Mathf.Abs(e.startPoint.y - e.endPoint.y);

                // 2. Find the center point Y
                float centerY = (e.startPoint.y + e.endPoint.y) / 2f;
                Vector3 wallCenter = new Vector3(e.startPoint.x, centerY, 0f);

                // 3. Apply position and scale
                wallInstance.transform.localPosition = wallCenter;
                wallInstance.transform.localScale = new Vector3(0.3f, length, 1f);
            }
        }

        // 5. Register the container GameObject in renderedChunks dictionary
        renderedChunks.Add(chunk.chunkCoords, chunkRoot);
    }

    public void UnrenderChunk(Vector2Int coord)
    {
        // 1. Check if the chunk visual actually exists
        if (renderedChunks.TryGetValue(coord, out GameObject chunkRoot))
        {
            // 2. Destroy the root container (and all its child walls automatically)
            Destroy(chunkRoot);

            // 3. Remove from renderedChunks tracking dictionary
            renderedChunks.Remove(coord);
        }
    }
}
