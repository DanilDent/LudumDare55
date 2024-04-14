using UnityEngine;

[CreateAssetMenu(fileName = "MergeReciptConfigSO", menuName = "Recipt Config")]
public class MergeReciptConfigSO : ScriptableObject
{
    [field: SerializeField] public RecipeInfo RecipeInfo;
    [field: SerializeField] public int Cost;
}