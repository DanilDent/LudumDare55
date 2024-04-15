using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class AttackLineRenderer : MonoBehaviour
{
    [SerializeField] private Seeker _seeker;
    [SerializeField] private Transform _container;

    [SerializeField] private Sprite _lineSpriteGreen;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _targetPos;

    [SerializeField] private Color _playerColor;
    [SerializeField] private Color _enemyColor;
    [SerializeField] private Color _mergerColor;

    private Pathfinding.Path path;
    private Color _drawColor;

    private List<GameObject> _pathsRenders = new List<GameObject>();

    private void Start()
    {
        var selectedBuildingShower = SelectedBuldingsShower.Instance;
        selectedBuildingShower.SelectionChanged += HandleSelectionChange;
    }

    private void OnDestroy()
    {
        SelectedBuldingsShower.Instance.SelectionChanged -= HandleSelectionChange;
    }

    private void HandleSelectionChange(Transform from, Transform to)
    {
        ClearLineRenders();

        if (from == null || to == null)
        {
            return;
        }

        IBuilding fromBuilding = from.GetComponent<IBuilding>();
        IBuilding toBuilding = to.GetComponent<IBuilding>();

        if (toBuilding is Merger)
        {
            _drawColor = _mergerColor;
        }
        else if (fromBuilding.Team == TeamEnum.Player)
        {
            _drawColor = _playerColor;
        }
        else
        {
            _drawColor = _enemyColor;
        }



        _seeker.StartPath(from.transform.position, to.transform.position, OnPathComplete);
    }

    private void ClearLineRenders()
    {
        foreach (var path in _pathsRenders)
        {
            path.gameObject.SetActive(false);
            Destroy(path.gameObject, 0.1f);
        }
        _pathsRenders.Clear();
    }

    private void DrawLine(List<Vector3> vectorPath)
    {
        var newPath = new GameObject("Path");
        newPath.transform.SetParent(_container);

        for (int i = 0; i < vectorPath.Count; i++)
        {
            if (i == 0)
                continue;

            var vector = vectorPath[i];

            var line = new GameObject("Line");
            line.transform.SetParent(newPath.transform);
            var sr = line.AddComponent<SpriteRenderer>();
            sr.sprite = _lineSpriteGreen;

            line.gameObject.transform.position = vector;
            sr.sortingOrder = 5;
            sr.transform.localScale = Vector3.one * .04f;
            sr.color = _drawColor;

            var dir = vectorPath[i] - vectorPath[i - 1];
            sr.transform.up = dir.normalized;
        }

        _pathsRenders.Add(newPath);
    }

    public void OnPathComplete(Pathfinding.Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            DrawLine(p.vectorPath);
        }
        else
        {
            p.Release(this);
        }
    }
}
