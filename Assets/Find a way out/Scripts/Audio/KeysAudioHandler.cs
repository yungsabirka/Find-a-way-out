using System.Collections;
using UnityEngine;

//The class is responsible for the sound of picking up keys
[RequireComponent(typeof(AudioSource))]
public class KeysAudioHandler : MonoBehaviour, IInitializable
{
    private KeysPicker _keysPicker;
    private AudioSource _audioSource;
    private bool _isInitialized;

    public bool IsInitialized => _isInitialized;

    public IEnumerator Initialize()
    {
        _keysPicker = GetComponentInParent<KeysPicker>();
        _audioSource = GetComponent<AudioSource>();
        _keysPicker.Picking += PlayOneShot;
        _isInitialized = true;
        yield return null;
    }

    private void OnDestroy()
    {
        if (_keysPicker != null)
            _keysPicker.Picking -= PlayOneShot;
    }

    private void PlayOneShot(AudioClip clip) => _audioSource.PlayOneShot(clip);
}
