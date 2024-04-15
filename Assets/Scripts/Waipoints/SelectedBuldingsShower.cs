using Misc;
using System;
using UnityEngine;

[DefaultExecutionOrder(10)]
public sealed class SelectedBuldingsShower : MonoSingleton<SelectedBuldingsShower>
{
    public Action<Transform, Transform> SelectionChanged;

    private BuildingsHolder _waypointsHolder;
    private BackClickDetector _backClickDetector;
    private IBuilding _selected;
    private IBuilding _selectedTarget;

    private bool _firstBuldingSelected;

    private void Start()
    {
        _waypointsHolder = BuildingsHolder.Instance;
        _backClickDetector = BackClickDetector.Instance;

        _backClickDetector.OnBackClick += OnBackClick;
        _waypointsHolder.OnBuildingClick += OnBuldingClicked;
    }

    protected override void OnDestroy()
    {
        _backClickDetector.OnBackClick -= OnBackClick;
        _waypointsHolder.OnBuildingClick -= OnBuldingClicked;
    }

    private void OnBuldingClicked(IBuilding bulding)
    {
        _waypointsHolder.DisableAllSelecetedSpriteOnBuldings();

        if (bulding.Team == TeamEnum.Player && bulding.IsEnoughResourcesToSpawn == false)
        {
            SelectionChanged?.Invoke(_selected?.GetTransform(), _selectedTarget?.GetTransform());
            return;
        }

        if (_firstBuldingSelected)
        {
            _firstBuldingSelected = false;

            if (bulding.IsSelecteble() == false || _selected == bulding || _selected.Team == TeamEnum.Enemy)
            {
                SelectionChanged?.Invoke(_selected?.GetTransform(), _selectedTarget?.GetTransform());
                return;
            }

            _selectedTarget = bulding;
        }
        else
        {
            _selected = bulding;
            _selectedTarget = bulding.CurrentTarget;
            _firstBuldingSelected = true;
        }

        if (_selected.SelectedSprite != null)
        {
            _selected.SelectedSprite.color = Color.grey;
            _selected.SelectedSprite.gameObject.SetActive(true);
        }

        if (_selectedTarget?.SelectedSprite != null)
        {
            _selectedTarget.SelectedSprite.color = Color.green;
            _selectedTarget.SelectedSprite.gameObject.SetActive(true);
        }

        SelectionChanged?.Invoke(_selected?.GetTransform(), _selectedTarget?.GetTransform());
    }

    private void OnBackClick()
    {
        _waypointsHolder.DisableAllSelecetedSpriteOnBuldings();
        _selected = null;
        _selectedTarget = null;
        _firstBuldingSelected = false;

        SelectionChanged?.Invoke(null, null);
    }
}