using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The class is responsible for the volume of the background music,
//which increases as the enemy approaches
[RequireComponent(typeof(AudioSource))]
public class EnemiesAudioHandler : MonoBehaviour, IInitializable
{
    [SerializeField] private float _minVolume;

    private IReadOnlyCollection<GameObject> _enemies;
    private AudioSource _audioSource;
    private PlayerMover _playerMover;
    private PauseHandler _pauseHandler;
    private bool _isInitialied;

    public bool IsInitialized => _isInitialied;

    public IEnumerator Initialize()
    {
        var enemiesSpawner = FindObjectOfType<EnemiesSpawner>();
        _enemies = enemiesSpawner.Enemies;
        _audioSource = GetComponent<AudioSource>();
        _playerMover = FindObjectOfType<PlayerMover>();
        _pauseHandler = FindObjectOfType<PauseHandler>();

        _pauseHandler.Paused += StopPlayingSound;
        _pauseHandler.Unpaused += ContinuePlayingSound;
        _isInitialied = true;
        yield return null;
    }

    private void OnDisable()
    {
        if (_pauseHandler == null)
            return;

        _pauseHandler.Paused -= StopPlayingSound;
        _pauseHandler.Unpaused -= ContinuePlayingSound;
    }

    private void Update()
    {
        if (_isInitialied == false)
            return;

        var minDistance = CalculateMinEnemyDistance();

        _audioSource.volume = 1 / (minDistance);

        if (_audioSource.volume < _minVolume)
            _audioSource.volume = _minVolume;
    }

    private float CalculateMinEnemyDistance()
    {
        float minDistance = float.MaxValue;
        foreach (var enemy in _enemies)
        {
            var distance = Vector3.Distance(enemy.transform.position, _playerMover.transform.position);
            if (distance < minDistance)
                minDistance = distance;
        }
        return minDistance;
    }

    private void StopPlayingSound() => _audioSource.Pause();

    private void ContinuePlayingSound() => _audioSource.UnPause();
}
