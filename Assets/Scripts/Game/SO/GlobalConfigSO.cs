using UnityEngine;

[CreateAssetMenu(fileName = "New Global Config", menuName = "Config/Global Config")]
public class GlobalConfigSO : ScriptableObject
{
    public DamageModInfo[] DamageModifiers => _damageModifiers;
    [SerializeField] private DamageModInfo[] _damageModifiers;
}
