using System.Collections;
using TMPro;
using UnityEngine;

//Timer display class
public class TimerView : MonoBehaviour, IInitializable
{
    private Timer _timer;
    private TextMeshProUGUI _timerView;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _timerView = GetComponent<TextMeshProUGUI>();
        _timer = FindObjectOfType<Timer>();
        _timer.TimeChanged += SetTimeView;
        _isInitialized = true;
        yield return null;
    }

    private void OnDestroy()
    {
        _timer.TimeChanged -= SetTimeView;
    }

    private void SetTimeView(int seconds, int minutes)
    {
        _timerView.text = $"{minutes}:{seconds}";
    }
}
