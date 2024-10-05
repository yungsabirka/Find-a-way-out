using UnityEngine;

//A class that checks if the object touches the floor
public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groudedRadius = 0.5f;

    private bool _isGrounded;

    public bool IsGrounded => _isGrounded;

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position, _groudedRadius, _groundLayer,
            QueryTriggerInteraction.Ignore);
    }
}