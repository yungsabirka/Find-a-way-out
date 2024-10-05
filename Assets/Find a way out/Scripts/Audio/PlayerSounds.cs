using System.Collections;
using UnityEngine;

//The class is responsible for the sounds the player makes
//during actions: walking, jumping, taking damage
[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour, IInitializable
{
    [SerializeField] private AudioClip[] _walkSounds;
    [SerializeField] private AudioClip[] _runningSounds;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _hurtedSound;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private float _timeBetweenSounds;

    private AudioSource _audioSource;
    private PlayerMover _playerMover;
    private PlayerHealth _playerHealth;
    private PauseHandler _pauseHandler;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _audioSource = GetComponent<AudioSource>();
        _playerMover = GetComponentInParent<PlayerMover>();
        _playerHealth = GetComponentInParent<PlayerHealth>();
        _pauseHandler = FindObjectOfType<PauseHandler>();

        StartCoroutine(PlayWalkSound());
        StartCoroutine(PlayRunningSound());

        _playerMover.Jumped += PlayJumpSound;
        _playerHealth.Damaged += PlayHurtedSound;
        _isInitialized = true;
        yield return null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (_playerMover != null)
            _playerMover.Jumped -= PlayJumpSound;

        if (_playerHealth != null)
            _playerHealth.Damaged -= PlayHurtedSound;
    }

    private IEnumerator PlayWalkSound()
    {
        while (true)
        {
            if (_playerMover.IsWalking && _groundChecker.IsGrounded && _pauseHandler.IsPaused == false)
            {
                _audioSource.PlayOneShot(_walkSounds[Random.Range(0, _walkSounds.Length)]);
                yield return new WaitForSeconds(_timeBetweenSounds);
            }
            yield return null;
        }
    }

    private IEnumerator PlayRunningSound()
    {
        while (true)
        {
            if (_playerMover.IsRunning && _groundChecker.IsGrounded && _pauseHandler.IsPaused == false)
            {
                _audioSource.PlayOneShot(_runningSounds[Random.Range(0, _runningSounds.Length)]);
                yield return new WaitForSeconds(_timeBetweenSounds / 2);
            }
            yield return null;
        }
    }

    private void PlayJumpSound()
    {
        if (_pauseHandler.IsPaused == false)
            _audioSource.PlayOneShot(_jumpSound);
    }

    private void PlayHurtedSound()
    {
        if (_pauseHandler.IsPaused == false)
            _audioSource.PlayOneShot(_hurtedSound);
    }
}