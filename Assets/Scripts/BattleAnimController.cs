using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BattleAnimController : MonoBehaviour
{
    [SerializeField] private GameObject _battleAnim;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private float _animDuration;

    private List<CombatAIComp> _unitsCombatComp = new();
    private List<Animator> _activeAnimators = new();

    private BuildingsHolder _buildingsHolder;

    private void Start()
    {
        _buildingsHolder = BuildingsHolder.Instance;

        foreach (var building in _buildingsHolder.Buildings)
        {
            building.EntitySpawned += OnUnitSpawned;
        }
    }

    private void OnDestroy()
    {
        foreach (var building in _buildingsHolder.Buildings)
        {
            if (building != null)
            {
                building.EntitySpawned += OnUnitSpawned;
            }
        }

        foreach (var unit in _unitsCombatComp)
        {
            if (unit != null && unit.isActiveAndEnabled)
            {
                unit.AttackStrategyComp.Attacked -= OnAttack;
            }
        }

        StopAllCoroutines();
    }

    private void OnUnitSpawned(GameObject gameObject)
    {
        var unit = gameObject.GetComponent<UnitComp>();

        if (unit.UnitSO.UnitType.AttackType == AttackTypeEnum.Melee)
        {
            unit.CombatAIComp.AttackStrategyComp.Attacked += OnAttack;
            _unitsCombatComp.Add(unit.CombatAIComp);
        }
    }

    private void OnAttack(UnitComp unit)
    {
        var pos = unit.transform.position;

        foreach (var activeAnim in _activeAnimators)
        {
            if (activeAnim.isActiveAndEnabled && Vector3.Distance(activeAnim.gameObject.transform.position, pos) < _spawnRadius)
            {
                return;
            }
        }

        var anim = Instantiate(_battleAnim, pos, Quaternion.identity);
        anim.gameObject.SetActive(false);
        _activeAnimators.Add(anim.GetComponent<Animator>());
        StartCoroutine(AnimCoroutine(anim.gameObject, unit.CombatAIComp));
    }

    private IEnumerator AnimCoroutine(GameObject gameObject, CombatAIComp combatAIComp)
    {
        float currentTime = 0;
        gameObject.SetActive(true);

        while (currentTime < _animDuration)
        {
            if (combatAIComp.IsInAttackingState)
            {
                currentTime = 0;
            }

            currentTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}