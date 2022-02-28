using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angle : MonoBehaviour
{
    [SerializeField] private Transform p0, p1, p2;

    private void OnDrawGizmos()
    {
        NPO.DrawPolyline(new List<Vector2>{p0.position, p1.position, p2.position});
        Debug.Log( "Vector2.SignedAngle: " + Vector2.SignedAngle(NPO.ConvertPointToVector(p0.position, p1.position), NPO.ConvertPointToVector(p1.position, p2.position)));
        Debug.Log("NPO.GetAngleVectors2: " + NPO.GetAngleVectors2(NPO.ConvertPointToVector(p0.position, p1.position), NPO.ConvertPointToVector(p1.position, p2.position)));
    }
}
