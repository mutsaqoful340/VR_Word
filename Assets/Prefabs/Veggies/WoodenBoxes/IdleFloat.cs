using UnityEngine;

public class IdleFloat : MonoBehaviour
{
    public float amplitude = 0.04f; // jarak naik-turun
    public float speed = 0.6f;      // kecepatan gerak

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = startPos + Vector3.up * offsetY;
    }
}
