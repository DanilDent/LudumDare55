// Note: taken from here: https://arongranberg.com/astar/docs/astaraics.html

using Game;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(LocalAvoidance))]
public class MovementComp : MonoBehaviour
{
    [SerializeField] private UnitSO _unitSO;
    [SerializeField] private LocalAvoidance _localAvoidance;

    public void Construct(UnitSO unitSO)
    {
        _unitSO = unitSO;
        _localAvoidance = GetComponent<LocalAvoidance>();
        _localAvoidance.Construct();
    }

    public Transform TargetPosition => _targetPosition;
    [SerializeField] private Transform _targetPosition;

    private Seeker seeker;

    public Path path;

    public float speed => _unitSO.Speed;
    public float decelerationModifier = 1f;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    public bool reachedEndOfPath;
    public bool canMove = true;

    public Vector2 NextMoveDirection2D => new Vector2(_dir.x, _dir.y);
    private Vector3 _dir;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
    }

    public void SetTarget(Transform target)
    {
        _targetPosition = target;
        lastRepath = float.NegativeInfinity;
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            p.Release(this);
        }
    }

    public void Update()
    {
        if (_targetPosition == null)
        {
            return;
        }

        if (!canMove)
        {
            return;
        }

        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            seeker.StartPath(transform.position, _targetPosition.position, OnPathComplete);
        }

        if (path == null)
        {
            return;
        }

        reachedEndOfPath = false;
        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) / decelerationModifier : 1f;

        _dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = _dir * speed * speedFactor;
        transform.position += velocity * Time.deltaTime;
    }
}