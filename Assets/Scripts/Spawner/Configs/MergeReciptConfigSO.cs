using UnityEngine;

[CreateAssetMenu(fileName = "MergeReciptConfigSO", menuName = "Recipt Config")]
public class MergeReciptConfigSO : ScriptableObject
{
    [field: SerializeField] public EntityType EntityTypeMergeFrom;
    [field: SerializeField] public int EntitysCountToMerge;
}