using System.Collections;
using UnityEngine;

//A class controller that uses a new input system for player use
public class PlayerInputController : MonoBehaviour, IInitializable
{
    private PlayerInputAction _inputAction;
    private bool _isInitialized;
    private bool _isInputEnabled;

    public PlayerInputAction InputActions => _inputAction;
    public bool IsInitialized => _isInitialized;
    public bool IsInputEnabled => _isInputEnabled;

    public IEnumerator Initialize()
    {
        DontDestroyOnLoad(this);

        _inputAction = new PlayerInputAction();
        Cursor.lockState = CursorLockMode.Locked;

        _isInitialized = true;
        yield return null;
    }

    public void EnableInputSystem()
    {
        _inputAction.Enable();
        _isInputEnabled = true;
    }

    public void DisableInputSystem()
    {
        _inputAction.Disable();
        _isInputEnabled = false;
    }
}
