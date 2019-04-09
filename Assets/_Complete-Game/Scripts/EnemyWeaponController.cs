using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    public float delay;
    public float fireRate;
    public GameObject shot;
    public Transform shotSpawn;

    private void Start()
    {
        InvokeRepeating(nameof(Fire), delay, fireRate);
    }

    private void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        GetComponent<AudioSource>().Play();
    }
}