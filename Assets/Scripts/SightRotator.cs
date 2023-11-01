using System;
using System.Collections;
using UnityEngine;

public class SightRotator : MonoBehaviour
{
    [SerializeField] private float _leftMaxRotation = 90f;
    [SerializeField] private float _rightMaxRotation = 60f;
    [SerializeField] private float _rotationSpeed = 5f;

    private Quaternion _initialRotation;
    private float _initialY;

    public event Action RotateStarted;
    public event Action RotateEnded;

    private void Start()
    {
        _initialRotation = transform.rotation;
    }

    public void OnMouseDown()
    {
        RotateStarted?.Invoke();
    }

    private void OnMouseDrag()
    {
        float deltaY = Input.mousePosition.y - _initialY;
        float rotationZ = deltaY * _rotationSpeed;

        transform.Rotate(Vector3.forward, rotationZ);
        float currentRotation = transform.localEulerAngles.z;

        if (currentRotation > 180f)
        {
            currentRotation -= 360f;
        }

        float clampedRotation = Mathf.Clamp(currentRotation, _leftMaxRotation, _rightMaxRotation);
        transform.localEulerAngles = new Vector3(0, 0, clampedRotation);
        _initialY = Input.mousePosition.y;
    }

    public void OnMouseUp()
    {
        RotateEnded?.Invoke();
        StartCoroutine(RotateSmoothly());
    }

    private IEnumerator RotateSmoothly()
    {
        const float Duration = 1f;

        float elapsedTime = 0;

        while (elapsedTime < Duration)
        {
            float interpolationRatio = elapsedTime / Duration;
            transform.rotation = Quaternion.Slerp(transform.rotation, _initialRotation, interpolationRatio);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
