using UnityEngine;

public class LounchForceCalculator : MonoBehaviour
{
    [SerializeField] private float _maxForceDistance = 5.0f;
    [SerializeField] private float _minForceDistance = 1.0f;
    [SerializeField] private float _maxLounchForce = 10.0f;
    [SerializeField] private float _minLounchForce = 5.0f;

    public float Calculate(Vector2 sightPosition)
    {
        Vector3 currentPointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(sightPosition, currentPointerPosition);
        float lounchForce = Mathf.Lerp(_minLounchForce, _maxLounchForce, Mathf.InverseLerp(_minForceDistance, _maxForceDistance, distance));
        return lounchForce;
    }
}
