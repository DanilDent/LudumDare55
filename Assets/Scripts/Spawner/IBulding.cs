using System;
using UnityEngine;

public interface IBulding
{
    public event Action<IBulding> Dead;
    public event Action<IBulding> Clicked;
    public event Action<GameObject> EntitySpawned;

    public Membership Membership { get; }
    public Vector3 Waypoint { get; }
    public bool IsSelecteble();
    public void MoveEntitiesToNewWaypoint(Vector3 waypoint);
    public void MoveEntitiesToNewTarget(IBulding target);
    public IBulding CurrentTarget { get; set; }
    public Transform CurrentTargetTransform { get; set; }
    public Transform GetTransform();
}