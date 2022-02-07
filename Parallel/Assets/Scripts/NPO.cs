using System.Collections.Generic;
using UnityEngine;

public static class NPO
{
    public static List<Vector2> ConvertToVector2(List<Transform> points)
    {
        var vector = new List<Vector2>();
        for (int i = 0; i < points.Count; i++)
            vector.Add(new Vector2(points[i].position.x, points[i].position.y));
        return vector;
    }

    public static List<Vector2> ConvertPointsToVector2(List<Vector2> points)
    {
        var vector = new List<Vector2>();
        for (int i = 1; i < points.Count; i++)
        {
            var point1 = points[i-1];
            var point2 = points[i];
            vector.Add(new Vector2(point2.x - point1.x, point2.y - point1.y));
        }
        return vector;
    }

    public static void DrawPolyline(List<Vector2> points)
    {
        for (var i = 1; i < points.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(points[i-1], points[i]);
        }
    }

    // не работает как надо 
    public static float GetAngle(Vector2 a, Vector2 b)
    {
        var ch = a.x * b.x + a.y + b.y; // числитель
        var zn = Mathf.Sqrt(a.x * a.x + a.y * a.y) + Mathf.Sqrt(b.x * b.x + b.y * b.y); // знаминатель 
        return Mathf.Acos(ch / zn) * 180 / Mathf.PI;
    }
    

    public static float AngleAround360Degrees(float angle)
    {
        if (angle>360) 
            return angle-360;
        if (angle<0) 
            return angle+360;
        return angle;
    }
    

    public static Vector3 GetCoordinate(Vector3 point, float angle, float distance)
    {
        var coordinates = new Vector2
        {
            x = point.x + distance * Mathf.Cos(angle),
            y = point.y + distance * Mathf.Sin(angle),
        };
        return coordinates;
    }
    
    public static Vector3 GetLineParallelCoord(Vector3 point1, float distance, bool rightLeftSide, float angle)
    {
        return GetCoordinate(point1, 
            rightLeftSide ? AngleAround360Degrees(angle + 90) : AngleAround360Degrees(angle - 90),
            distance);
    }
        
    public static List<Vector2> GetParallelPolyline(List<Vector2> startCoordinates, float distance,
        bool rightLeftSide)
    {
        var parallelPolyline = new List<Vector2>();
        var angle = Vector2.Angle(startCoordinates[0], startCoordinates[1]);
        parallelPolyline.Add(GetLineParallelCoord(startCoordinates[0], distance, rightLeftSide, angle));
        parallelPolyline.Add(GetLineParallelCoord(startCoordinates[1], distance, rightLeftSide, angle));
        angle = Vector2.Angle(startCoordinates[1], startCoordinates[2]);
        parallelPolyline.Add(GetLineParallelCoord(startCoordinates[1], distance, rightLeftSide, angle));
        parallelPolyline.Add(GetLineParallelCoord(startCoordinates[2], distance, rightLeftSide, angle));
        return parallelPolyline;
    }
}
