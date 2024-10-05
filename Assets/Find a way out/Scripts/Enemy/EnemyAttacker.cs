using UnityEngine;

//The class is responsible for dealing damage
//to the player while in contact with the enemy
public class EnemyAttacker : MonoBehaviour
{
    private const int PlayerLayer = 8;

    [SerializeField] private float _damage;
    [SerializeField] private float _attackDelay;

    private float _lastAttackedTime;
    private bool _isAttacked;

    private void OnTriggerEnter(Collider other)
    {
        if (_isAttacked)
        {
            if (Time.time - _lastAttackedTime > _attackDelay)
                _isAttacked = false;
            else
                return;
        }

        if (other.gameObject.layer != PlayerLayer)
            return;

        var playerHealth = other.GetComponent<PlayerHealth>();
        Attack(playerHealth);
        _isAttacked = true;
        _lastAttackedTime = Time.time;
    }

    private void Attack(PlayerHealth playerHealth) => playerHealth.TakeDamage(_damage);
}

