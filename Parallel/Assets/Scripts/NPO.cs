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
    
    public static Vector2 ConvertPointToVector(Vector2 point1, Vector2 point2)
    {
       
        return new Vector2(point2.x - point1.x, point2.y - point1.y);
    }
    
    public static void DrawPolyline(List<Vector2> points)
    {
        for (var i = 1; i < points.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(points[i-1], points[i]);
        }
    }

    public static void DrawPolylineGreen(List<Vector2> points)
    {
        for (var i = 1; i < points.Count; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(points[i - 1], points[i]);
        }
    }


    public static float GetAngleVectors(Vector2 a, Vector2 b)
    {
        var ch = a.x * b.x + a.y + b.y; // числитель
        var zn = Mathf.Sqrt(a.x * a.x + a.y * a.y) + Mathf.Sqrt(b.x * b.x + b.y * b.y); // знаминатель 
        return Mathf.Acos(ch / zn) * 180 / Mathf.PI;
    }

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
        if (rightLeftSide)
            return GetCoordinate(point1, (angle + 90) * Mathf.PI / 180, distance);
        else
            return GetCoordinate(point1, (angle - 90) * Mathf.PI / 180, distance);
    }

    public static Vector2 GetPointCrossLines(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        // https://habr.com/ru/post/523440/ 
        float n; // 
        if((p2.y - p1.y) != 0)
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
    

    // работает хорошо
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

        // дописать выбор точки, котороя ближе к Vector2 centerCircle (дописано) 

        if (GetLengthVector(new Vector2(x1, y1), pointCrossLines) < GetLengthVector(new Vector2(x2, y2), pointCrossLines))
            return new Vector2(x1, y1);
        else
            return new Vector2(x2, y2);
    }

    public static float GetLengthVector(Vector2 point, Vector2 centerCircle)
    {
        return Mathf.Abs(Mathf.Sqrt((point.x - centerCircle.x) * (point.x - centerCircle.x) + 
        (point.y - centerCircle.y) * (point.y - centerCircle.y)));
    }

    private static List<Vector2> CheckingRendering(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3,
         Vector2 crossLinesPoint, Vector2 pointOnCircle, float angleVector, bool rightLeftSide)
    {
        if (rightLeftSide)
        {
            if (angleVector <= 1 && angleVector >= -1)
            {
                return new List<Vector2> { p1 };  // когда почти прямая 
            }
            else if (angleVector > 0.5 && angleVector <= 30)
            {
                return new List<Vector2> { pointOnCircle }; // точка на оружности
            }
            else if (angleVector > 30 && angleVector < 180)
            {
                return new List<Vector2> { crossLinesPoint }; // точка на пересечении 
            }
            else // if (angleVector < -0.5 && angleVector >= -180)
            {
                return new List<Vector2> { p1, pointOnCircle, p2 }; // три точки на оружности 
            }
        }
        else
        {
            if (angleVector <= 1 && angleVector >= -1)
            {
                return new List<Vector2> { p1 };  // когда почти прямая 
            }

            else if (angleVector < -1 && angleVector >= -30)
            {
                return new List<Vector2> { pointOnCircle }; // точка на оружности
            }
            else if (angleVector < -30 && angleVector > -180)
            {
                return new List<Vector2> { crossLinesPoint }; // точка на пересечении 
            }
            else // if (angleVector > 1 && angleVector <= 180)
            {
                return new List<Vector2> { p1, pointOnCircle, p2 }; // три точки на оружности 
            }
        }
    }

    public static List<Vector2> GetParallelPolyline2(List<Vector2> startCoordinates, float distance, bool rightLeftSide)
    {
        
        var parallelPolyline = new List<Vector2>();
        for(var i = 2; i < startCoordinates.Count; i++)
        {
            var angle = GetAngle(startCoordinates[i-2], startCoordinates[i-1]);
            var p0 = GetLineParallelCoord(startCoordinates[i - 2], distance, rightLeftSide, angle);
            var p1 = GetLineParallelCoord(startCoordinates[i-1], distance, rightLeftSide, angle);
            angle = GetAngle(startCoordinates[i - 1], startCoordinates[i]);
            var p2 = GetLineParallelCoord(startCoordinates[i - 1], distance, rightLeftSide, angle);
            var p3 = GetLineParallelCoord(startCoordinates[i], distance, rightLeftSide, angle);
            
            var crossLinesPoint = GetPointCrossLines(p0, p1, p2, p3);
            var pointOnCircle = GetPoitnCrossLineAndCircle2(crossLinesPoint, startCoordinates[i-1], distance);
            
            // отрисовка круга для наглядности 
            //Gizmos.color = Color.gray;
            //Gizmos.DrawWireSphere(startCoordinates[i - 1], distance);
            
            // угол между векторами
            var angleVector = Vector2.SignedAngle(ConvertPointToVector(p0, p1), ConvertPointToVector(p2, p3));

            if (i == 2)
                parallelPolyline.Add(p0);
                
            
            parallelPolyline.AddRange(CheckingRendering(p0, p1, p2, p3, crossLinesPoint, pointOnCircle, angleVector, rightLeftSide));

            if (i == startCoordinates.Count - 1)
                parallelPolyline.Add(p3);

               
        }
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