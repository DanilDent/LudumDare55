using UnityEngine;

public class CombatSystemTest : MonoBehaviour
{
    [SerializeField] private Transform _playerCity;
    [SerializeField] private Transform _enemyCity;

    [SerializeField] private UnitSO _playerUnitSO;
    [SerializeField] private UnitSO _enemyUnitSO;

    [SerializeField] private Transform _enemyUnitsContainer;
    [SerializeField] private Transform _playerUnitsContainer;

    private UnitFactory _unitFactory;

    private void Start()
    {
        _unitFactory = UnitFactory.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos2D = new Vector2(pos.x, pos.y);
            var instance = _unitFactory.Create(_playerUnitsContainer, pos2D, TeamEnum.Player, _playerUnitSO);
            instance.AddTarget(_enemyCity);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos2D = new Vector2(pos.x, pos.y);
            var instance = _unitFactory.Create(_enemyUnitsContainer, pos2D, TeamEnum.Enemy, _enemyUnitSO);
            instance.AddTarget(_playerCity);
        }
    }
}
