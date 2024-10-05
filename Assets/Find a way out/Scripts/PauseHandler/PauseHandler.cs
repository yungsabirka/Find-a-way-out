using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//The class responsible for pausing the game
public class PauseHandler : MonoBehaviour, IInitializable
{
    public event Action Paused;
    public event Action Unpaused;

    private PlayerInputController _playerInputController;
    private bool _isPaused;
    private bool _isInitialized;

    public bool IsPaused => _isPaused;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _playerInputController = FindObjectOfType<PlayerInputController>();
        _isInitialized = true;
        _playerInputController.InputActions.KeyboardAndMouse.Pause.performed += InputPause;
        SetPause(false);
        yield return null;
    }

    private void OnDestroy()
    {
        if (_playerInputController != null)
            _playerInputController.InputActions.KeyboardAndMouse.Pause.performed -= InputPause;
    }

    private void InputPause(InputAction.CallbackContext context)
    {
        SetPause(!_isPaused);
    }

    public void SetPause(bool isPaused)
    {
        _isPaused = isPaused;
        if (_isPaused)
            Paused?.Invoke();
        else
            Unpaused?.Invoke();

        Time.timeScale = _isPaused ? 0f : 1f;
    }
}