using UnityEngine;

public class DestroyByTimeToLive : MonoBehaviour
{
    public float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}