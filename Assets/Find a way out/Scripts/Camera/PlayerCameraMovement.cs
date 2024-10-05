using System.Collections;
using UnityEngine;

//The class is responsible for moving the camera with the mouse
public class PlayerCameraMovement : MonoBehaviour, IInitializable
{
    private const float MouseDeadZone = 0.3f;

    [SerializeField] private float _bottomClamp = -60f;
    [SerializeField] private float _topClamp = 80f;
    [SerializeField] private float _sensitivityX;
    [SerializeField] private float _sensitivityY;

    private PauseHandler _pauseHandler;
    private PlayerInputController _playerInputSystem;
    private float _cameraTargetYaw;
    private float _cameraTargetPitch;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _playerInputSystem = FindObjectOfType<PlayerInputController>();
        _pauseHandler = FindObjectOfType<PauseHandler>();
        _isInitialized = true;
        yield return null;
    }

    private void LateUpdate()
    {
        if (_isInitialized == false || _pauseHandler.IsPaused)
            return;

        RotateCamera();
    }

    private void RotateCamera()
    {
        var direction = _playerInputSystem.InputActions.KeyboardAndMouse.Look.ReadValue<Vector2>();
        if (direction.sqrMagnitude <= MouseDeadZone)
            return;

        _cameraTargetYaw += direction.x * _sensitivityX;
        _cameraTargetPitch -= direction.y * _sensitivityY;

        _cameraTargetYaw = ClampAngle(_cameraTargetYaw, -360, 360);
        _cameraTargetPitch = ClampAngle(_cameraTargetPitch, _bottomClamp, _topClamp);

        transform.rotation = Quaternion.Euler(_cameraTargetPitch, _cameraTargetYaw, 0f);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle > 360f) angle -= 360f;
        if (angle < -360f) angle += 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
