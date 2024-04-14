using UnityEngine;

[CreateAssetMenu(fileName = "MergerConfigSO", menuName = "Merge Config")]
public class MergerConfigSO : BaseSpawnConfigSO
{
    [field: SerializeField] public MergeReciptConfigSO MergeReciptConfigSO {  get; private set; }
}