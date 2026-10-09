
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Chunk
{
    public Vector2Int chunkCoords;

    private System.Random rng;

    public int chunkSize = 3;

    public Chunk(Vector2Int chunkCoords, int seed)
    {
        this.chunkCoords = chunkCoords;

        this.rng = new System.Random(seed); // Seeded here

        populateEdges();
        handleWalls();
    }


    public List<Edge> vEdges = new List<Edge>();
    public List<Edge> hEdges = new List<Edge>();

    public List<Edge> allEdges = new List<Edge>();

    public List<Edge> processedEdges = new List<Edge>();


    public void populateEdges()
    {
        int chunkX = chunkCoords.x * chunkSize;
        int chunkY = chunkCoords.y * chunkSize;

        // 1. Horizontal Edges (X changes, Y stays constant)
        // Run y for all grid lines, x for cell widths
        for (int y = chunkY; y <= chunkY + chunkSize; y++)
        {
            for (int x = chunkX; x < chunkX + chunkSize; x++) // < instead of <=
            {
                Vector2Int startPoint = new Vector2Int(x, y);
                Vector2Int endPoint = new Vector2Int(x + 1, y);

                hEdges.Add(new Edge(startPoint, endPoint));
            }
        }

        // 2. Vertical Edges (Y changes, X stays constant)
        // Run x for all grid lines, y for cell heights
        for (int x = chunkX; x <= chunkX + chunkSize; x++)
        {
            for (int y = chunkY; y < chunkY + chunkSize; y++) // < instead of <=
            {
                // Corrected: (x, y) and (x, y + 1)
                Vector2Int startPoint = new Vector2Int(x, y);
                Vector2Int endPoint = new Vector2Int(x, y + 1);

                vEdges.Add(new Edge(startPoint, endPoint));
            }
        }

        allEdges = hEdges.Concat(vEdges).ToList();
    }

    public void handleWalls()
    {
        clearInnerSquare();
        clearOutterSquare();
        clearConnectingEdges();
    }

    public void clearConnectingEdges()
    {
        List<Edge> connectingEdges = allEdges.Except(processedEdges).ToList();
        List<Edge> candidatePool = new List<Edge>(connectingEdges);

        foreach (Edge e in connectingEdges)
        {
            e.isWall = false;
        }

        while (candidatePool.Count > 0)
        {
            // 1. Pick and process chosen edge
            int randomIndex = rng.Next(0, candidatePool.Count);
            Edge chosenEdge = candidatePool[randomIndex];
            chosenEdge.isWall = true; // Updates globally

            // 2. Remove chosen edge from candidatePool
            RemoveFast(candidatePool, randomIndex);

            // 3. Find and remove invalid neighbors
            FindAndRemoveParallelNeighbor(chosenEdge, candidatePool);
            FindAndRemoveCrossingNeighbor(chosenEdge, candidatePool);
        }

        void FindAndRemoveParallelNeighbor(Edge chosenEdge, List<Edge> pool)
        {
            int matchIndex = -1;

            for (int i = 0; i < pool.Count; i++)
            {
                Edge e = pool[i];

                if (chosenEdge.IsHorizontal && e.IsHorizontal)
                {
                    if (chosenEdge.startPoint.x == e.startPoint.x || chosenEdge.startPoint.x == e.endPoint.x)
                    {
                        matchIndex = i;
                        break; // Found matching index, exit loop
                    }
                }
                else if (chosenEdge.IsVertical && e.IsVertical)
                {
                    if (chosenEdge.startPoint.y == e.startPoint.y || chosenEdge.startPoint.y == e.endPoint.y)
                    {
                        matchIndex = i;
                        break;
                    }
                }
            }

            if (matchIndex != -1)
            {
                RemoveFast(pool, matchIndex);
            }
        }

        void FindAndRemoveCrossingNeighbor(Edge chosenEdge, List<Edge> pool)
        {
            int matchIndex = -1;

            for (int i = 0; i < pool.Count; i++)
            {
                Edge e = pool[i];

                if (chosenEdge.startPoint == e.startPoint || chosenEdge.startPoint == e.endPoint ||
                    chosenEdge.endPoint == e.startPoint || chosenEdge.endPoint == e.endPoint)
                {
                    matchIndex = i;
                    break;
                }
            }

            if (matchIndex != -1)
            {
                RemoveFast(pool, matchIndex);
            }
        }

        void RemoveFast(List<Edge> list, int index)
        {
            int lastIndex = list.Count - 1;
            list[index] = list[lastIndex];
            list.RemoveAt(lastIndex);
        }
    }

    public void clearOutterSquare()
    {
        int chunkX = chunkCoords.x * chunkSize;
        int chunkY = chunkCoords.y * chunkSize;

        // 1. Extract the 4 outer sides using coordinates
        List<Edge> bottomSide = hEdges.FindAll(e => e.startPoint.y == chunkY);
        List<Edge> topSide = hEdges.FindAll(e => e.startPoint.y == chunkY + chunkSize);
        List<Edge> leftSide = vEdges.FindAll(e => e.startPoint.x == chunkX);
        List<Edge> rightSide = vEdges.FindAll(e => e.startPoint.x == chunkX + chunkSize);

        processedEdges.AddRange(bottomSide.Union(topSide).Union(leftSide).Union(rightSide).Except(processedEdges));

        // 2. Clear 1 random wall for each side
        clearSide(bottomSide);
        clearSide(topSide);
        clearSide(leftSide);
        clearSide(rightSide);

        void clearSide(List<Edge> sideEdges)
        {
            if (sideEdges.Count == 0) return;

            // If a side already has an open door/pathway, don't force another open
            foreach (Edge e in sideEdges)
            {
                if (!e.isWall) return;
            }

            // Pick a random edge from the side list using its actual Count
            int randomIndex = rng.Next(0, sideEdges.Count);
            sideEdges[randomIndex].isWall = false;
        }
    }

    public void clearInnerSquare()
    {
        int chunkX = chunkCoords.x * chunkSize;
        int chunkY = chunkCoords.y * chunkSize;

        Vector2Int innerSquareCoords = new Vector2Int(chunkX + chunkSize / 2, chunkY + chunkSize / 2);

        Vector2Int firstPoint = innerSquareCoords;

        Vector2Int secondPoint = new Vector2Int(innerSquareCoords.x + 1, innerSquareCoords.y);

        clearEdge(firstPoint, secondPoint);

        secondPoint = new Vector2Int(innerSquareCoords.x, innerSquareCoords.y + 1);
        clearEdge(firstPoint, secondPoint);

        firstPoint = new Vector2Int(innerSquareCoords.x + 1, innerSquareCoords.y + 1);
        secondPoint = new Vector2Int(innerSquareCoords.x - 1, innerSquareCoords.y);
        clearEdge(firstPoint, secondPoint);

        secondPoint = new Vector2Int(innerSquareCoords.x, innerSquareCoords.y - 1);
        clearEdge(firstPoint, secondPoint);

        void clearEdge(Vector2Int firstPoint, Vector2Int secondPoint)
        {
            Edge foundEdge = allEdges.Find(e =>
                (e.startPoint == firstPoint && e.endPoint == secondPoint) ||
                (e.startPoint == secondPoint && e.endPoint == firstPoint)
                );

            if(foundEdge != null)
            {
                if (!processedEdges.Contains(foundEdge))
                {
                    processedEdges.Add(foundEdge);
                }
                foundEdge.isWall = false;
            }
            else
            {
                Debug.Log("Edge not found!");
            }
        }

    }

    public void inspectList(List<Edge> edgeList)
    {

        int edgeCount = 0;
        foreach (Edge edge in edgeList)
        {
            Debug.Log("Edge from " + edge.startPoint + " to " + edge.endPoint);
            edgeCount++;
        }

        Debug.Log("Total Edges count: " + edgeCount);
    }


}
