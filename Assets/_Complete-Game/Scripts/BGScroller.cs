using UnityEngine;

public class BGScroller : MonoBehaviour
{
    private Vector3 _startPosition;
    public float scrollSpeed;
    public float tileSizeZ;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        var newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        transform.position = _startPosition + Vector3.forward * newPosition;
    }
}