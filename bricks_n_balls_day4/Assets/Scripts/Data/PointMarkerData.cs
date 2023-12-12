using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMarkerData
{
    private GameObject pointMarkerObject;
    private float radius;

    public GameObject PointMarkerObject { get => pointMarkerObject; set => pointMarkerObject = value; }
    public float Radius { get => radius; set => radius = value; }
}
