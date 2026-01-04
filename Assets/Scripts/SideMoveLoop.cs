using UnityEngine;

public class SideMoveLoop : MonoBehaviour
{
    public float moveDistance = 0.3f; // jarak kiri-kanan
    public float speed = 1f;          // kecepatan gerak

    private Vector3 startPos;
    private bool isStopped = false;   // status berhenti

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isStopped) return; // kalau kena peluru, STOP

        float offset = Mathf.Sin(Time.time * speed) * moveDistance;
        transform.position = startPos + new Vector3(offset, 0, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        // pastikan peluru punya tag "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            isStopped = true;
        }
    }
}
