using System;
using UnityEngine;

[Serializable]
public class WaypointsData
{
    [field: SerializeField] public EntitiesInBuldingWaypointController Sender { get; private set; }
    [field: SerializeField] public EntitiesInBuldingWaypointController Target { get; private set; }
}
