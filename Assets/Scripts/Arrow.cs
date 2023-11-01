using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speedX;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _angleInDegrees;

    private Rigidbody2D _rigidbody;
    private bool _isFlight;

    public void Shoot(Vector2 sightPosition, LounchForceCalculator lounchForceCalculator)
    {
        const float ForceFactor = 50f;

        float force = lounchForceCalculator.Calculate(sightPosition);
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(transform.right * force * ForceFactor);
        _isFlight = true;
        StartCoroutine(RotateArrow());
    }

    private IEnumerator RotateArrow()
    {
        while (_isFlight)
        {
            Vector3 velocity = _rigidbody.velocity;

            if (velocity != Vector3.zero)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Ground>(out Ground _))
        {
            _rigidbody.bodyType = RigidbodyType2D.Static;
            _isFlight = false;
        }
    }
}
