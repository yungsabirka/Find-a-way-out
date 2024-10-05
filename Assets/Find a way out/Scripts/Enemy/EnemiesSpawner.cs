using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The class is responsible for spawning enemies, and initializing them 
public class EnemiesSpawner : MonoBehaviour, IInitializable
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _enemiesCount;

    private List<GameObject> _enemies;
    private MazeCell[,] _mazeGrid;
    private bool _isInitialized;
    private bool _isSpawned;

    public bool IsInitialized => _isInitialized;
    public IReadOnlyCollection<GameObject> Enemies => _enemies;

    public IEnumerator Initialize()
    {
        var mazeGenerator = FindObjectOfType<MazeGenerator>();
        _mazeGrid = mazeGenerator.MazeGrid;
        _enemies = new List<GameObject>();

        StartCoroutine(SpawnEnemies());
        yield return new WaitUntil(() => _isSpawned);

        StartCoroutine(InitializeEnemies());
        _isInitialized = true;
        yield return null;
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < _enemiesCount; i++)
        {
            var randomX = Random.Range(_mazeGrid.GetLength(0) / 2, _mazeGrid.GetLength(0));
            var randomZ = Random.Range(_mazeGrid.GetLength(0) / 2, _mazeGrid.GetLength(1));

            var enemy = Instantiate(_enemyPrefab,
                _mazeGrid[randomX, randomZ].transform.position, Quaternion.identity);

            _enemies.Add(enemy);
            yield return null;
        }
        _isSpawned = true;
    }

    private IEnumerator InitializeEnemies()
    {
        foreach (var enemy in _enemies)
        {
            var enemyMover = enemy.GetComponent<EnemyMover>();
            StartCoroutine(enemyMover.Initialize());
            yield return new WaitUntil(() => enemyMover.IsInitialized);
        }
    }
}
