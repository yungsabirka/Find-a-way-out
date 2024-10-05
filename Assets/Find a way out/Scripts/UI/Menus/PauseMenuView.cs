using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//The pause menu class that appears when the esc key is pressed
public class PauseMenuView : MonoBehaviour, IInitializable
{
    private PlayerInputController _playerInputController;
    private PauseHandler _pauseHandler;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;
    public PauseHandler PauseHandler => _pauseHandler;

    public IEnumerator Initialize()
    {
        _playerInputController = FindObjectOfType<PlayerInputController>();
        _pauseHandler = FindObjectOfType<PauseHandler>();

        _playerInputController.InputActions.KeyboardAndMouse.Pause.performed += InputToggleVisibility;
        gameObject.SetActive(false);
        _isInitialized = true;
        yield return null;
    }

    private void OnDestroy()
    {
        if (_playerInputController != null)
            _playerInputController.InputActions.KeyboardAndMouse.Pause.performed -= InputToggleVisibility;
    }

    private void InputToggleVisibility(InputAction.CallbackContext context) => ToggleVisibility();

    public void ToggleVisibility()
    {
        gameObject.SetActive(_pauseHandler.IsPaused);
        Cursor.lockState = _pauseHandler.IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
