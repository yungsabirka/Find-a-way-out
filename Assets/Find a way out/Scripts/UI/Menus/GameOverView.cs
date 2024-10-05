
using System.Collections;
using UnityEngine;

//Class of displaying the game over menu after player's death
public class GameOverView : MonoBehaviour, IInitializable
{
    private Timer _timer;
    private PlayerInputController _inputController;
    private PlayerHealth _playerHealth;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;
     
    public IEnumerator Initialize()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _inputController = FindObjectOfType<PlayerInputController>();
        _timer = FindObjectOfType<Timer>();

        _playerHealth.Died += EnableGameOverMenu;
        gameObject.SetActive(false);

        _isInitialized = true;
        yield return null;

    }

    private void OnDestroy()
    {
        if(_playerHealth != null)
            _playerHealth.Died -= EnableGameOverMenu;
    }

    private void EnableGameOverMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        _timer.Pause();
        _inputController.DisableInputSystem();
    }
}
