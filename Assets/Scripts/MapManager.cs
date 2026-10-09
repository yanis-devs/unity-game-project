using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerT;
    [SerializeField] private MapRenderer mapRenderer;

    [Header("World Settings")]
    [SerializeField] private int worldSeed = 0;
    [SerializeField] private int renderRadius = 3;
    [SerializeField] private int chunkSize = 10;

    

    public Dictionary<Vector2Int, Chunk> activeChunks = new();

    private void Awake()
    {
        // TODO: Move worldSeed assignment to GameManager/SaveManager when implemented
        if (worldSeed == 0)
        {
            worldSeed = Random.Range(int.MinValue, int.MaxValue);
        }
    }

    // TODO: Add Initialize(int seed) method here for external setup later

    public void ManageMap()
    {

        if (playerT == null)
        {
            Debug.LogError("MapManager: PlayerT (Transform) is NULL!");
            return;
        }

        Vector2Int currentChunkCoords = GetCurrentChunk();
        PopulateActiveChunks(currentChunkCoords);
        CleanActiveChunks(currentChunkCoords);
    }

    public Vector2Int GetCurrentChunk()
    {
        int currentChunkX = Mathf.FloorToInt(playerT.position.x / chunkSize);
        int currentChunkY = Mathf.FloorToInt(playerT.position.y / chunkSize);

        return new Vector2Int(currentChunkX, currentChunkY);
    }

    private void PopulateActiveChunks(Vector2Int currentChunkCoords)
    {
        for (int x = currentChunkCoords.x - renderRadius; x <= currentChunkCoords.x + renderRadius; x++)
        {
            for (int y = currentChunkCoords.y - renderRadius; y <= currentChunkCoords.y + renderRadius; y++)
            {
                Vector2Int targetCoords = new Vector2Int(x, y);

                if (!activeChunks.ContainsKey(targetCoords))
                {
                    activeChunks.Add(targetCoords, new Chunk(targetCoords, GetChunkSeed(targetCoords)));

                    // TODO: Tell MapRenderer to instantiate GameObjects for this chunk
                    mapRenderer.RenderChunk(activeChunks[targetCoords]);
                }
            }
        }
    }

    private int GetChunkSeed(Vector2Int chunkCoord)
    {
        int hash = worldSeed;
        hash = hash ^ (chunkCoord.x * 73856093);
        hash = hash ^ (chunkCoord.y * 19349663);
        return hash;
    }

    private void CleanActiveChunks(Vector2Int currentChunkCoords)
    {
        List<Vector2Int> keysToRemove = new();

        foreach (Vector2Int key in activeChunks.Keys)
        {
            Vector2Int distance = key - currentChunkCoords;

            if (Mathf.Abs(distance.x) > renderRadius || Mathf.Abs(distance.y) > renderRadius)
            {
                keysToRemove.Add(key);
            }
        }

        foreach (Vector2Int key in keysToRemove)
        {
            // TODO: Tell MapRenderer to destroy or pool GameObjects for this chunk
            mapRenderer.UnrenderChunk(key);

            activeChunks.Remove(key);
        }
    }
}