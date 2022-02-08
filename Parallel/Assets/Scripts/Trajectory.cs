using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class Trajectory : MonoBehaviour
{

	[SerializeField] private List<Transform> points;
	[SerializeField] [Range(0,5)] private float distanse;
	[SerializeField] bool right;
	[SerializeField] bool twoPoint;
	[SerializeField] [Range(1,10)] private int numberParallel;

	private void OnDrawGizmos()
	{
		

        if (twoPoint)
        {
			// отрисовка линий соед. сточки 
			NPO.DrawPolyline(NPO.ConvertToVector2(points));


			List<Vector2> polyline = new List<Vector2>();
            polyline = NPO.GetParallelPolylineTwoPoint(NPO.ConvertToVector2(points), distanse, right);
            NPO.DrawPolyline(polyline);
            for (var i = 1; i < numberParallel; i++)
            {
                polyline = NPO.GetParallelPolylineTwoPoint(polyline, distanse, right);
                NPO.DrawPolyline(polyline);
            }
            print(NPO.GetAngle(points[0].position, points[1].position));
        }
        else
        {
			Gizmos.DrawLine(points[0].position, points[1].position);
			Gizmos.DrawLine(points[2].position, points[3].position);


			Gizmos.color = Color.red;
			Gizmos.DrawLine(points[1].position, NPO.GetPointCrossLines(points[0].position, points[1].position, points[2].position, points[3].position));
			Gizmos.DrawLine(points[2].position, NPO.GetPointCrossLines(points[0].position, points[1].position, points[2].position, points[3].position));
			Gizmos.color = Color.blue;
			var dis = Mathf.Sqrt((points[1].position.x - points[4].position.x) * (points[1].position.x - points[4].position.x)
				+ (points[1].position.y - points[4].position.y) * (points[1].position.y - points[4].position.y));
			Gizmos.DrawWireSphere(points[4].position, dis);

			Gizmos.color = Color.green;
			Gizmos.DrawLine(points[4].position, NPO.GetPointCrossLines(points[0].position, points[1].position, points[2].position, points[3].position));

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(new Vector2(0, 0), NPO.GetPoitnCrossLineAndCircle2(NPO.GetPointCrossLines(points[0].position, points[1].position, points[2].position, points[3].position), points[4].position, dis));
		}

        //var vectors = NPO.ConvertPointsToVector2(NPO.ConvertToVector2(points));
        //NPO.DrawPolyline(NPO.GetParallelPolyline(NPO.ConvertToVector2(points), distanse, right));







    }
}
