using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallData : MonoBehaviour
{
    private GameObject ballObject;
    private Vector2 velocity;
    private bool isMoving;
    private float radius;
    private float speed;
    private bool isGather;


    public GameObject BallObject { get => ballObject; set => ballObject = value; }
    public Vector2 Velocity { get => velocity; set => velocity = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public float Radius { get => radius; set => radius = value; }
    public float Speed { get => speed; set => speed = value; }
    public bool IsGather { get => isGather; set => isGather = value; }
}
