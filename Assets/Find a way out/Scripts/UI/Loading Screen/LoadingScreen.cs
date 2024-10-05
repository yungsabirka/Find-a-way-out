using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image _frontBar;

    public void FillBar(float fillAmount)
    {
        if (fillAmount <= _frontBar.fillAmount || fillAmount > 1.0f)
            return;

        while(_frontBar.fillAmount <= fillAmount * 0.95)
            _frontBar.fillAmount = Mathf.Lerp(_frontBar.fillAmount, fillAmount, 0.1f);        
    }
}
