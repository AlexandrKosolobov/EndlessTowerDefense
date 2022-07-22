using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] private WaterflowShortWaySearcher _defaultWaterflowShortWaySearcher;
    [SerializeField] private float _speed;
    [SerializeField] private bool _canMove;
    [SerializeField] private Queue<Vector3> _waypoints;
    [SerializeField] private Vector3 _curWaypoint;
    private bool _waypointsInitialized = false;

    private void Awake()
    {
        StartCoroutine(TryGetWaypoints());
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!_waypointsInitialized) return;
        if (_canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, _curWaypoint, _speed * Time.fixedDeltaTime);
        }
        else return;
        if (Vector3.Distance(transform.position, _curWaypoint) < 0.01f)
        {
            if (!_waypoints.TryDequeue(out _curWaypoint))
            {
                OnLastWaypointReached();
            }
        }
    }

    public IEnumerator TryGetWaypoints()
    {
        Vector3[] waypoints;
        do
        {
            waypoints = InitializeWaypoints();
            yield return null;
        }
        while (waypoints == null || waypoints.Length == 0);
        _waypoints = new Queue<Vector3>(waypoints);
        _waypointsInitialized = true;
        _curWaypoint = _waypoints.Dequeue();
    }

    /// <summary>
    /// Use the same parameter as used in <see cref="DisapplySpeedModifier"/>
    /// </summary>
    /// <param name="mod">speed = speed * mod. Must be > 0</param>
    public void ApplySpeedModifier(float mod)
    {
        if (mod <= 0)
        {
            EndMoving();
            return;
        }
        _speed *= mod;
    }

    /// <summary>
    /// Use the same parameter as used in <see cref="ApplySpeedModifier"/>
    /// </summary>
    /// <param name="mod">speed = speed / mod. Must be > 0</param>
    public void DisapplySpeedModifier(float mod)
    {
        if (mod <= 0)
        {
            StartMoving();
            return;
        }
        _speed /= mod;
    }

    /// <summary>
    /// Pause unit movement
    /// </summary>
    public void EndMoving()
    {
        _canMove = false;
    }

    /// <summary>
    /// Unpause unit movement
    /// </summary>
    public void StartMoving()
    {
        _canMove = true;
    }

    /// <summary>
    /// Choose your method of waypoint creation
    /// </summary>
    /// <returns>Positions in correct oreder from spawner to home</returns>
    protected virtual Vector3[] InitializeWaypoints()
    {
        _defaultWaterflowShortWaySearcher = FindObjectOfType<WaterflowShortWaySearcher>();
        return _defaultWaterflowShortWaySearcher.Waypoints.ToArray();
    }

    /// <summary>
    /// When unit reach the last point of path (reach home)
    /// </summary>
    protected virtual void OnLastWaypointReached()
    {
        Destroy(gameObject);
    }
}
