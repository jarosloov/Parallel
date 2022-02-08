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
    //public static float GetAngle(Vector2 a, Vector2 b)
    //{
    //    var ch = a.x * b.x + a.y + b.y; // числитель
    //    var zn = Mathf.Sqrt(a.x * a.x + a.y * a.y) + Mathf.Sqrt(b.x * b.x + b.y * b.y); // знаминатель 
    //    return Mathf.Acos(ch / zn) * 180 / Mathf.PI;
    //}

    public static float GetAngle(Vector2 a, Vector2 b)
    {
        float angle = Mathf.Atan2(b.y - a.y, b.x - a.x) * 180 / Mathf.PI;
        if (angle < 0)
            angle += 360;
        return angle;
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
            rightLeftSide ? AngleAround360Degrees((angle + 90) *  Mathf.PI / 180) : AngleAround360Degrees((angle - 90) * Mathf.PI / 180),
            distance);
    }

    public static Vector2 GetPointCrossLines(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        // https://habr.com/ru/post/523440/ 
        float n; // 
        if(p2.y - p1.y != 0)
        {
            var q = (p2.x - p1.x) / (p1.y - p2.y);
            var sn = (p3.x - p4.x) + (p3.y - p4.y) * q;
            //if(sn != 0 ) // выдаст что они не пересекаются 
            var fn = (p3.x - p1.x) + (p3.y - p1.y) * q;
            n = fn / sn;
        }
        else
        {
            //if ((p3.y - p4.y) != 0) // выдаст что они не пересекаются
            n = (p3.y - p1.y) / (p3.y - p4.y);
        }
        return new Vector2(p3.x + (p4.x - p3.x) * n, p3.y + (p4.y - p3.y) * n);
    }


    public static void DrawLineTwoPoint(Vector2 point1, Vector2 point2)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(point1, point2);
    }

    public static Vector2 GetLineParallelTwoLine(Vector2 point0, Vector2 point1, Vector2 point2, Vector2 point3, 
        float distance, bool rightLeftSide, float angle)
    {
        

        return point0;
    }


    // не работает 
    public static Vector2 GetPoitnCrossLineAndCircle2(Vector2 pointCrossLines, Vector2 centerCircle, float distance)
    {
        // http://dmtsoft.ru/bn/377/as/oneaticleshablon 

        var k = (centerCircle.y - pointCrossLines.y) / (centerCircle.x - pointCrossLines.x);
        var b = centerCircle.y - k * centerCircle.x;

        var d = (2 * k * b - 2 * centerCircle.x - 2 * centerCircle.y * k) * 
            (2 * k * b - 2 * centerCircle.x - 2 * centerCircle.y * k) -(4 + 4 * k * k) * 
            (b * b - distance * distance + centerCircle.x * centerCircle.x + centerCircle.y * centerCircle.y - 2 * centerCircle.y * b);

        //если он равен 0, уравнение не имеет решения
        //если он меньше 0, прямая и окружность не пересекаются

        float x1 = ((-(2 * k * b - 2 * centerCircle.x - 2 * centerCircle.y * k) - Mathf.Sqrt(d)) / (2 + 2 * k * k));
        float x2 = ((-(2 * k * b - 2 * centerCircle.x - 2 * centerCircle.y * k) + Mathf.Sqrt(d)) / (2 + 2 * k * k));

        float y1 = k * x1 + b;
        float y2 = k * x2 + b;

        // дописать выбор точки, котороя ближе к Vector2 centerCircle

        return new Vector2(x2, y2);
    }

    // не работает как надо  
    public static Vector2 GetPoitnCrossLineAndCircle(Vector2 pointCrossLines, Vector2 centerCircle, float distance)
    {
        // distance == Radius
        // https://www.cyberforum.ru/cpp-beginners/thread1456151.html

        var dx = pointCrossLines.x - centerCircle.x;
        var dy = pointCrossLines.y - centerCircle.y;
        var l = Mathf.Sqrt(dx * dx + dy * dy);
        dx /= l;
        dy /= l;

        return new Vector2(pointCrossLines.x + dx * distance, pointCrossLines.y + dy * distance);
    }

    public static List<Vector2> GetParallelPolyline(List<Vector2> startCoordinates, float distance, bool rightLeftSide)
    {
        var parallelPolyline = new List<Vector2>();
        var angle = GetAngle(startCoordinates[0], startCoordinates[1]); //Vector2.SignedAngle
        //for (var i = 0; i < startCoordinates.Count; i = i + 2)
        //{
        //    angle = Vector2.SignedAngle(startCoordinates[1], startCoordinates[2]);
        //    parallelPolyline.Add(GetLineParallelTwoLine(startCoordinates[0], startCoordinates[1], startCoordinates[0],
        //        startCoordinates[1], distance, rightLeftSide, angle));
        //}
        return parallelPolyline;
    }

    public static List<Vector2> GetParallelPolylineTwoPoint(List<Vector2> startCoordinates, float distance, bool rightLeftSide)
    {
        var parallelPolyline = new List<Vector2>();
        var angle = GetAngle(startCoordinates[0], startCoordinates[1]); 
        parallelPolyline.Add(GetLineParallelCoord(startCoordinates[0], distance, rightLeftSide, angle));
        parallelPolyline.Add(GetLineParallelCoord(startCoordinates[1], distance, rightLeftSide, angle));
        return parallelPolyline;
    }
}
