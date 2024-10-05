using System;
using UnityEngine;

//A class that is responsible for the amount of health and damage the player takes
public class PlayerHealth : MonoBehaviour
{
    public event Action Died;
    public event Action Damaged;

    [SerializeField] private float _health;

    private bool _isDied;

    public bool IsDied => _isDied;
    public float Health => _health;

    public void TakeDamage(float damage)
    {
        _health -= damage;

        Damaged?.Invoke();
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    private void Die()
    {
        Died?.Invoke();
    }
}