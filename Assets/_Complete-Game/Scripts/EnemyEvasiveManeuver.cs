using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class EnemyEvasiveManeuver : MonoBehaviour
{
    private float _currentSpeed;
    private Rigidbody _rigidbody;
    private float _targetManeuver;
    public Boundary boundary;
    public float dodge;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public float smoothing;
    public Vector2 startWait;
    public float tilt;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _currentSpeed = _rigidbody.velocity.z;
        StartCoroutine(Evade());
    }

    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    private IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        while (true)
        {
            _targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            _targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverWait.x, maneuverWait.y));
        }
    }

    private void FixedUpdate()
    {
        var newManeuver = Mathf.MoveTowards(_rigidbody.velocity.x, _targetManeuver,
            smoothing * Time.deltaTime);
        _rigidbody.velocity = new Vector3(newManeuver, 0.0f, _currentSpeed);
        _rigidbody.position = new Vector3
        (
            Mathf.Clamp(_rigidbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(_rigidbody.position.z, boundary.zMin, boundary.zMax)
        );

        _rigidbody.rotation = Quaternion.Euler(0, 0, _rigidbody.velocity.x * -tilt);
    }
}