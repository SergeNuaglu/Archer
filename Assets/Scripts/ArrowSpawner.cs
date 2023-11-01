using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private Arrow _arrowPrefab;
    [SerializeField] private Transform _arm;
    [SerializeField] private SightRotator _sightRotator;
    [SerializeField] private LounchForceCalculator _lounchForceCalculator;

    private void OnEnable()
    {
        _sightRotator.RotateEnded += OnSightRotateEnded;
    }

    private void OnDisable()
    {
        _sightRotator.RotateEnded -= OnSightRotateEnded;
    }

    private void OnSightRotateEnded()
    {
        Arrow arrow = Instantiate(_arrowPrefab, _arm.position, _arm.transform.rotation);
        Vector2 sightPosition = _sightRotator.transform.position;
        arrow.Shoot(sightPosition, _lounchForceCalculator);
    }
}
