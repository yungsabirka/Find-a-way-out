using System.Collections;
using UnityEngine;

//The class is responsible for animating the movement of the keys
public class KeyMover : MonoBehaviour
{
    [SerializeField] private Transform _rotationCenter;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minHeight;
    [SerializeField] private float _verticalySpeed;

    private bool _up;

    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    private void OnDisable()
    {
        StopCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if (transform.position.y > _maxHeight)
                _up = false;
            if (transform.position.y < _minHeight)
                _up = true;

            var velocity = _up ? _verticalySpeed : -_verticalySpeed;

            transform.Translate(new Vector3(0.0f, velocity * Time.deltaTime, 0.0f));
            transform.RotateAround(_rotationCenter.transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }
}
