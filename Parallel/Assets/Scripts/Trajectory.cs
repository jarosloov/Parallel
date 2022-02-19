using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Trajectory : MonoBehaviour
{

    [SerializeField] private List<Transform> points;
    [SerializeField] [Range(0,5)] private float distanse;
    [SerializeField] bool right;
    [SerializeField] bool twoPoint;
    [SerializeField] bool justParallel;
    [SerializeField] [Range(1,10)] private int numberParallel;

    private void OnDrawGizmos()
    {
        NPO.DrawPolyline(NPO.ConvertToVector2(points));
       
        if (points.Count == 2)
        {
            List<Vector2> polyline = new List<Vector2>();
            
            NPO.DrawPolyline(polyline);
            for (var i = 1; i < numberParallel; i++)
            {
                polyline = NPO.GetParallelPolylineTwoPoint(i == 0 ? NPO.ConvertToVector2(points) : polyline, distanse, right);
                NPO.DrawPolyline(polyline);
            }
        }
        else
        {
            List<Vector2> polyline = new List<Vector2>();
            for (var i = 0; i < numberParallel; i++)
            {
                polyline = NPO.GetParallelPolyline2(i == 0 ? NPO.ConvertToVector2(points) : polyline, distanse, right);
                NPO.DrawPolylineGreen(polyline);
            }
        }
        
    }
}