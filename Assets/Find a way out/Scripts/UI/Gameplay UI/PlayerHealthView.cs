using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A class that displays the number of lives left in a player's possession
public class PlayerHealthView : MonoBehaviour, IInitializable
{
    [SerializeField] private GameObject _heartPrefab;
    [SerializeField] private GameObject _hitScreen;
    [SerializeField] private float _hitVisiblityTime;

    private List<GameObject> _hearts;
    private PlayerHealth _playerHealth;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _hearts = new List<GameObject>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _hitScreen.SetActive(false);

        for (int i = 0; i < _playerHealth.Health; i++)
        {
            var heart = Instantiate(_heartPrefab, transform);
            _hearts.Add(heart);
            yield return null;
        }

        _playerHealth.Damaged += TakeDamage;

        _isInitialized = true;
        yield return null;
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
            _playerHealth.Damaged -= TakeDamage;
    }

    private void TakeDamage()
    {
        if (_hearts.Count > 0)
        {
            var heart = _hearts[_hearts.Count - 1];
            _hearts.RemoveAt(_hearts.Count - 1);
            Destroy(heart);
            StartCoroutine(EnableHitScreen());
        }
    }

    private IEnumerator EnableHitScreen()
    {
        _hitScreen.SetActive(true);
        yield return new WaitForSeconds(_hitVisiblityTime);
        if (_playerHealth.Health > 0)
            _hitScreen.SetActive(false);
    }
}
