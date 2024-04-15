using Misc;
using System.Linq;
using UnityEngine;

public class GlobalConfigHolder : MonoSingleton<GlobalConfigHolder>
{
    [SerializeField] private GlobalConfigSO _globalConfig;
    [SerializeField] private Transform _projectilesContainer;

    public Transform ProjectilesContainer => _projectilesContainer;
    [field: SerializeField] public Transform EnemyEntitiesContainer { get; private set; }
    [field: SerializeField] public Transform PlayerEntitiesContainer { get; private set; }

    public float GetDamageModifier(UnitTypeSO attacker, UnitTypeSO defender)
    {
        return _globalConfig.DamageModifiers.FirstOrDefault(_ => _.Attacker == attacker && _.Defender == defender)?.Modifier ?? 1f;
    }

    public Transform GetTeamUnitsContaienr(TeamEnum team)
    {
        return team == TeamEnum.Player ? PlayerEntitiesContainer : EnemyEntitiesContainer;
    }
}
