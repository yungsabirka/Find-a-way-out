using System;
using System.Collections;
using UnityEngine;

//A class responsible for moving the player with the CharacterController,
//choosing to walk or run, as well as jumping with gravity 
[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour, IInitializable
{
    public event Action Jumped;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _speedChangeRate;
    [SerializeField, Range(0f, 0.3f)] private float _smoothTime = 0.12f;
    [SerializeField] private Transform _camera;

    private CharacterController _characterController;
    private PlayerInputController _inputController;
    private GroundChecker _groundChecker;

    private float _targetRotation;

    private float _speed;
    private float _currentSpeed = 0.0f;
    private readonly float _speedOffset = 0.05f;

    private float _verticalVelocity;

    private float _fallTimeoutDelta;
    private float _jumpTimeoutDelta;

    private readonly float _fallTimeout = 0.35f;
    private readonly float _jumpTimeout = 0.5f;

    private bool _isInitialized;
    private bool _isWalking;
    private bool _isRunning;

    public bool IsInitialized => _isInitialized;
    public bool IsWalking => _isWalking;
    public bool IsRunning => _isRunning;

    public IEnumerator Initialize()
    {
        _inputController = FindObjectOfType<PlayerInputController>();
        _characterController = GetComponent<CharacterController>();
        _groundChecker = GetComponentInChildren<GroundChecker>();

        _isInitialized = true;
        yield return null;
    }

    private void Update()
    {
        if (_isInitialized == false || _inputController.IsInputEnabled == false)
            return;

        Move();
        Jump();
        Fall();
    }

    private void CalculateSpeed()
    {
        _isRunning = _inputController.InputActions.KeyboardAndMouse.Sprint.IsPressed();
        _isWalking = !_isRunning;

        float targetSpeed = _isRunning ? _sprintSpeed : _walkSpeed;
        var direction = _inputController.InputActions.KeyboardAndMouse.Movement.ReadValue<Vector2>();

        if (direction == Vector2.zero)
        {
            targetSpeed = 0.0f;
            _isRunning = false;
            _isWalking = false;
        }

        if (_currentSpeed < targetSpeed - _speedOffset || _currentSpeed > targetSpeed + _speedOffset)
        {
            _speed = Mathf.Lerp(_currentSpeed, targetSpeed, _speedChangeRate * Time.deltaTime);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
            _speed = targetSpeed;
    }
    private void Move()
    {
        CalculateSpeed();
        var direction = _inputController.InputActions.KeyboardAndMouse.Movement.ReadValue<Vector2>();

        if (direction != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg
                + _camera.transform.eulerAngles.y;
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        _characterController.Move((_speed * targetDirection.normalized + 
            new Vector3(0f, _verticalVelocity, 0f)) * Time.deltaTime);

        _currentSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;
    }

    private void Jump()
    {
        if (_groundChecker.IsGrounded == false)
            return;

        _fallTimeoutDelta = _fallTimeout;
        _verticalVelocity = 0.0f;

        var isJumpPressed = _inputController.InputActions.KeyboardAndMouse.Jump.IsPressed();

        if (isJumpPressed && _jumpTimeoutDelta <= 0.0f)
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * (-Physics.gravity.y));
            Jumped?.Invoke();
        }

        if (_jumpTimeoutDelta >= 0.0f)
            _jumpTimeoutDelta -= Time.deltaTime;
    }

    private void Fall()
    {
        if (_groundChecker.IsGrounded)
            return;

        _jumpTimeoutDelta = _jumpTimeout;

        if (_fallTimeoutDelta >= 0.0f)
            _fallTimeoutDelta -= Time.deltaTime;

        _verticalVelocity -= Time.deltaTime * (-Physics.gravity.y);
    }
}
