using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private PauseMenuView _pauseMenu;

    public void ContinueGame()
    {
        if (_pauseMenu == null)
            return;

        _pauseMenu.PauseHandler.SetPause(!_pauseMenu.PauseHandler.IsPaused);
        _pauseMenu.ToggleVisibility();
    }
}

