using Misc;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfoHolder : MonoSingleton<LevelInfoHolder>
{
    [field: SerializeField] public List<WaypointsData> Waypoints;
}
