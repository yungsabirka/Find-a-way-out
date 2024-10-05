using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The class is responsible for spawning keys in the maze
public class KeysSpawner : MonoBehaviour, IInitializable
{
    [SerializeField] private List<GameObject> _keyPrefabs;
    [SerializeField] private MazeGenerator _generator;
    [SerializeField] private int _minSpawnCell;
    [SerializeField] private int _oneTypeKeyAmount;

    private bool _isInitialized;

    public int KeysAmount => _oneTypeKeyAmount * _keyPrefabs.Count;
    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        var generator = FindObjectOfType<MazeGenerator>();

        SpawnKeys(generator.MazeGrid);
        _isInitialized = true;
        yield return null;
    }

    private void SpawnKeys(MazeCell[,] mazeGrid)
    {
        var keysGridPositions = new List<(int, int)>();

        foreach (var key in _keyPrefabs)
            for (int i = 0; i < _oneTypeKeyAmount; i++)
            {
                int x, y;
                do
                {
                    x = Random.Range(_minSpawnCell, mazeGrid.GetLength(0));
                    y = Random.Range(_minSpawnCell, mazeGrid.GetLength(1));
                } 
                while (keysGridPositions.Contains((x, y)));

                keysGridPositions.Add((x, y));

                var position = mazeGrid[x, y].transform.position;

                Instantiate(key, new Vector3(position.x, 0.3f, position.z), Quaternion.identity);
            }
    }
}
