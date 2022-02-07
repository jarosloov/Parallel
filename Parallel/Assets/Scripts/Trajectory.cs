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

	private void OnDrawGizmos()
	{
		var vectors = NPO.ConvertPointsToVector2(NPO.ConvertToVector2(points));
		NPO.DrawPolyline(NPO.GetParallelPolyline(NPO.ConvertToVector2(points), distanse, right));
		




		// отрисовка
		NPO.DrawPolyline(NPO.ConvertToVector2(points));

	}
}
