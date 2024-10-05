using System;
using System.Collections;
using UnityEngine;

//The class is responsible for picking up keys during the game
public class KeysPicker : MonoBehaviour, IInitializable
{
    public event Action<AudioClip> Picking;
    public event Action<int> Picked;
    public event Action AllPicked;

    private const int KeyLayer = 7;

    [SerializeField] private AudioClip _pickUpSound;

    private int _pickedKeys = 0;
    private int _keysAmount;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        var keysSpawner = FindObjectOfType<KeysSpawner>();
        _keysAmount = keysSpawner.KeysAmount;
        _isInitialized = true;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != KeyLayer)
            return;

        _pickedKeys++;
        Destroy(other.gameObject);

        if (_pickUpSound != null)
            Picking?.Invoke(_pickUpSound);

        Picked?.Invoke(_pickedKeys);

        if (_pickedKeys == _keysAmount)
            AllPicked?.Invoke();
    }
}
