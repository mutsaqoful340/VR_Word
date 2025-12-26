using UnityEngine;

public class SideMoveLoop : MonoBehaviour
{
    public float moveDistance = 0.3f; // jarak kiri-kanan
    public float speed = 1f;          // kecepatan gerak

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * moveDistance;
        transform.position = startPos + new Vector3(offset, 0, 0);
    }
}
