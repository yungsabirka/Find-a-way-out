using System.Collections;
using UnityEngine;

//The class initializes other components that are
//initialized and fills the boot screen scale
public class GameplayEntryPoint : MonoBehaviour
{
    private bool _allInitialized;

    private IEnumerator Start()
    {
        var playerInputController = FindObjectOfType<PlayerInputController>();
        var initializableComponents = new Component[]
        {
            FindObjectOfType<MazeGenerator>(),
            FindObjectOfType<KeysSpawner>(),
            FindObjectOfType<KeysPicker>(),
            FindObjectOfType<KeysAudioHandler>(),
            playerInputController,
            FindObjectOfType<PlayerMover>(),
            FindObjectOfType<PlayerSounds>(),
            FindObjectOfType<PlayerCameraMovement>(),
            FindObjectOfType<TimerView>(),
            FindObjectOfType<KeysView>(),
            FindObjectOfType<PauseHandler>(),
            FindObjectOfType<PauseMenuView>(),
            FindObjectOfType<WinView>(),
            FindObjectOfType<GameOverView>(),
            FindObjectOfType<EnemiesSpawner>(),
            FindObjectOfType<EnemiesAudioHandler>(),
            FindObjectOfType<PlayerHealthView>(),
            FindObjectOfType<NavMeshBuilder>(),

        };

        StartCoroutine(InitializeComponent(initializableComponents));
        yield return new WaitUntil(() => _allInitialized);

        var timer = FindObjectOfType<Timer>();
        timer.Unpause();
        playerInputController.EnableInputSystem();
        gameObject.SetActive(false);
    }

    private IEnumerator InitializeComponent(Component[] components)
    {
        int counter = 0;
        var loadingScreen = FindObjectOfType<LoadingScreen>();

        foreach (var component in components)
        {
            if (component is IInitializable)
            {
                StartCoroutine((component as IInitializable).Initialize());
                yield return new WaitUntil(() => (component as IInitializable).IsInitialized);
                counter++;
                loadingScreen.FillBar((float)counter / components.Length);
            }
            else
                throw new System.Exception("All components must be IInitializable");
        }
        _allInitialized = true;
        loadingScreen.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
