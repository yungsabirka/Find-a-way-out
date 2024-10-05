using System.Collections;
using TMPro;
using UnityEngine;

//Class to display the number of collected keys and their maximum number of keys
[RequireComponent(typeof(TextMeshProUGUI))]
public class KeysView : MonoBehaviour, IInitializable
{
    private KeysPicker _picker;
    private TextMeshProUGUI _keysAmount;
    private int _maxKeysAmount;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        var spawner = FindObjectOfType<KeysSpawner>();
        _picker = FindObjectOfType<KeysPicker>();
        _keysAmount = GetComponent<TextMeshProUGUI>();
        _maxKeysAmount = spawner.KeysAmount;

        _picker.Picked += SetKeysAmount;
        SetKeysAmount(0);

        _isInitialized = true;
        yield return null;
    }

    private void OnDestroy()
    {
        _picker.Picked -= SetKeysAmount;
    }

    private void SetKeysAmount(int keysAmount)
        => _keysAmount.text = $"{keysAmount} / {_maxKeysAmount}";  
}
