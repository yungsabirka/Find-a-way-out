using System.Collections;
using UnityEngine;

//Winning menu display class
public class WinView : MonoBehaviour, IInitializable
{
    private PauseHandler _pauseHandler;
    private ExitWall _exitWall;
    private PlayerInputController _inputController;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _inputController = FindObjectOfType<PlayerInputController>();
        _pauseHandler = FindObjectOfType<PauseHandler>();
        _exitWall = FindObjectOfType<ExitWall>();

        _exitWall.Won += EnableWinMenu;
        gameObject.SetActive(false);

        _isInitialized = true;
        yield return null;
    }

    private void OnDestroy()
    {
        if (_exitWall != null)
            _exitWall.Won -= EnableWinMenu;
    }

    private void EnableWinMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        _pauseHandler.SetPause(true);
        _inputController.DisableInputSystem();
    }
}
