using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action<int, int> TimeChanged;

    private int _seconds;
    private int _minutes;
    private bool _isPaused;

    private void OnDisable()
    {
        StopCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while (_isPaused == false)
        {
            yield return new WaitForSeconds(1);
            _seconds++;
            if (_seconds == 60)
            {
                _minutes++;
                _seconds = 0;
            }
            TimeChanged?.Invoke(_seconds, _minutes);
        }
    }

    public void Pause()
    {
        _isPaused = true;
        StopCoroutine(StartTimer());
    }
    public void Unpause()
    {
        _isPaused = false;
        StartCoroutine(StartTimer());
    }
}
