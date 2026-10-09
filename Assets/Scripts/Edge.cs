using UnityEngine;

public class Edge
{
    public Vector2Int startPoint;
    public Vector2Int endPoint;
    public bool isWall;

    public bool IsHorizontal => Mathf.Approximately(startPoint.y, endPoint.y) && !Mathf.Approximately(startPoint.x, endPoint.x);
    public bool IsVertical => Mathf.Approximately(startPoint.x, endPoint.x) && !Mathf.Approximately(startPoint.y, endPoint.y);
    public Edge(Vector2Int startPoint, Vector2Int endPoint, bool isWall = true)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.isWall = isWall;
    }
}
