using Misc;
using System.Linq;
using UnityEngine;

public class GlobalConfigHolder : MonoSingleton<GlobalConfigHolder>
{
    [SerializeField] private GlobalConfigSO _globalConfig;
    [SerializeField] private Transform _projectilesContainer;

    public Transform ProjectilesContainer => _projectilesContainer;

    public float GetDamageModifier(UnitTypeSO attacker, UnitTypeSO defender)
    {
        return _globalConfig.DamageModifiers.FirstOrDefault(_ => _.Attacker == attacker && _.Defender == defender)?.Modifier ?? 1f;
    }
}
