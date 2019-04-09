using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    ///     the audio source used to play 'weapon' audio clip
    /// </summary>
    private AudioSource _audioSource;

    /// <summary>
    ///     the minimal timestamp in millisecond for next fire
    /// </summary>
    private float _nextFire;

    private Rigidbody _rigidbody;
    public Boundary boundary;
    public float fireRate;

    /// <summary>
    ///     the bullet object
    /// </summary>
    public GameObject shot;

    public Transform shotSpawn;
    public float speed;
    public float tilt;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!Input.GetButton("Fire1") || !(Time.time > _nextFire)) return;

        _nextFire = Time.time + fireRate;
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        _audioSource.Play();
    }

    private void FixedUpdate()
    {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        _rigidbody.velocity = movement * speed;

        _rigidbody.position = new Vector3
        (
            Mathf.Clamp(_rigidbody.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(_rigidbody.position.z, boundary.zMin, boundary.zMax)
        );

        _rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, _rigidbody.velocity.x * -tilt);
    }
}