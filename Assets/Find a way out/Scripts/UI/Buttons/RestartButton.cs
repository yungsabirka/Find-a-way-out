using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    private const string GameplayScene = "Gameplay";

    public void Restart()
    {
        SceneManager.LoadScene(GameplayScene);
    }
}

