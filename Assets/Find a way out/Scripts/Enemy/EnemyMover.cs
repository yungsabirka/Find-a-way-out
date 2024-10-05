using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//The class is responsible for the movement of an enemy
//that is either patrolling an area or stalking the player
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMover : MonoBehaviour, IInitializable
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxChaseTime;
    [SerializeField] private float _viewRadius;
    [SerializeField] private int _mazeWalkingRange;

    private NavMeshAgent _agent;
    private MazeCell[,] _mazeGrid;
    private Transform _target;
    private float _lastChaseTime;
    private Vector3 _patrolDestination;
    private bool _isChasing;
    private bool _isHasPatrolDestination;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        var mazeGenerator = FindObjectOfType<MazeGenerator>();
        _target = FindObjectOfType<PlayerMover>().transform;
        _agent = GetComponent<NavMeshAgent>();

        _mazeGrid = mazeGenerator.MazeGrid;
        _isInitialized = true;
        yield return null;
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;

        CheckTarget();
        Move();
    }

    private void Move()
    {
        if (_isChasing)
            ChaseTarget();
        else
            Patrol();
    }

    private void Patrol()
    {
        if (_isHasPatrolDestination)
        {
            var distance = Vector3.Distance(transform.position, _patrolDestination);
            if (distance < 0.5f)
                _isHasPatrolDestination = false;
            return;
        }

        int x = (int)transform.position.x;
        int z = (int)transform.position.z;
        int mazeX = x / 2 + x % 2;
        int mazeZ = z / 2 + z % 2;

        int randomMazeX = RandomMaze(mazeX, _mazeGrid.GetLength(0));
        int randomMazeZ = RandomMaze(mazeZ, _mazeGrid.GetLength(1));

        _patrolDestination = _mazeGrid[randomMazeX, randomMazeZ].transform.position;
        _isHasPatrolDestination = true;
        _agent.SetDestination(_patrolDestination);
    }

    private int RandomMaze(int currentMazePosition, int mazeLenght)
        =>  Random.Range(
            Mathf.Max(0, currentMazePosition - _mazeWalkingRange),
            Mathf.Min(currentMazePosition + _mazeWalkingRange, mazeLenght));

    private void ChaseTarget()
    {
        _agent.SetDestination(_target.transform.position);
        _isHasPatrolDestination = false;
    }

    private void CheckTarget()
    {
        var isTargetInSphere = Physics.CheckSphere(transform.position, _viewRadius, _targetLayer);

        if (isTargetInSphere)
        {
            _lastChaseTime = Time.time;
            _isChasing = true;
        }
        else
        {
            if (Time.time - _lastChaseTime > _maxChaseTime)
                _isChasing = false;
        }

    }
}
