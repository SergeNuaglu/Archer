using System.Collections;
using UnityEngine;

public class ArrowPathRenderer : MonoBehaviour
{
    [SerializeField] private GameObject _pointPrefab;
    [SerializeField] private Transform _pointContainer;
    [SerializeField] private Transform _bow;
    [SerializeField] private Transform _string;
    [SerializeField] private SightRotator _sightRotator;
    [SerializeField] private LounchForceCalculator _launchForceCalculator;
    [SerializeField] private int _numberOfPoints;
    [SerializeField] private float _maxScaleFactor = 1;
    [SerializeField] private float _minScaleFactor = 0.5f;

    private GameObject[] _points;
    private Vector2 _direction;
    private bool _canRender;

    private void OnEnable()
    {
        _sightRotator.RotateStarted += OnSightRotateStarted;
        _sightRotator.RotateEnded += OnSightRotateEnded;
    }

    private void Start()
    {
        _points = new GameObject[_numberOfPoints];

        for (int i = 0; i < _numberOfPoints; i++)
        {
            _points[i] = Instantiate(_pointPrefab, _pointContainer);
        }
    }

    private void OnDisable()
    {
        _sightRotator.RotateStarted -= OnSightRotateStarted;
        _sightRotator.RotateEnded -= OnSightRotateEnded;
    }

    private Vector2 GetPointPosition(float time, float force)
    {
        Vector2 pointPosition = (Vector2)_bow.transform.position + (_direction.normalized * force * time) + 0.5f * Physics2D.gravity * (time * time);
        return pointPosition;
    }

    private void OnSightRotateStarted()
    {
        _canRender = true;
        StartCoroutine(Render());
    }

    private void OnSightRotateEnded()
    {
        _canRender = false;
    }

    private IEnumerator Render()
    {
        const float GapFactor = 0.005f;

        float ratio;
        float scaleFactor;
        float force;

        while (_canRender)
        {
            _direction = (Vector2)(_bow.position - _string.position);
            transform.right = _direction;

            for (int i = 1; i < _points.Length; i++)
            {
                ratio = (float)i / (_points.Length - 1);
                scaleFactor = Mathf.Lerp(_maxScaleFactor, _minScaleFactor, ratio);
                _points[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);
                force = _launchForceCalculator.Calculate(_sightRotator.transform.position);
                _points[i].transform.position = GetPointPosition(i * GapFactor * force, force);
            }

            yield return null;
        }

        for (int i = 1; i < _points.Length; i++)
        {
            _points[i].transform.position = _pointContainer.position;
        }
    }
}
