using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Type", menuName = "Config/Unit Type")]
public class UnitTypeSO : ScriptableObject
{
    public string Name;
    public AttackTypeEnum AttackType;
}
