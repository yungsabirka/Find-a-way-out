using System;
using System.Collections;
using UnityEngine;

//The class is responsible for exiting the labyrinth,
//which becomes available once all the keys have been collected
[RequireComponent(typeof(BoxCollider))]
public class ExitWall : MonoBehaviour, IInitializable
{
    public event Action Won;

    private const int PlayerLayer = 8;

    private KeysPicker _picker;
    private Material _material;
    private BoxCollider _collider;

    private bool _isOpen;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _picker = FindObjectOfType<KeysPicker>();
        _collider = GetComponent<BoxCollider>();
        _material = GetComponentInChildren<Renderer>().material;
        _material.color = Color.black;

        _picker.AllPicked += Open;

        _isInitialized = true;
        yield return null;
    }
    private void OnDestroy()
    {
        if (_picker != null)
            _picker.AllPicked -= Open;
    }

    private void Open()
    {
        _isOpen = true;
        _material.color = Color.green;
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isOpen == false || other.gameObject.layer != PlayerLayer)
            return;

        Won?.Invoke();
    }
}
