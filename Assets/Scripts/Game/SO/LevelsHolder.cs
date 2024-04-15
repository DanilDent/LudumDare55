using UnityEngine;

[CreateAssetMenu(fileName = "New Levels Config", menuName = "Config/Levels Config")]
public class LevelsHolder : ScriptableObject
{
    public GameObject[] Levels => _levels;

    [SerializeField] private GameObject[] _levels;
}
