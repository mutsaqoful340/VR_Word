using UnityEngine;

public class SlowFloat : MonoBehaviour
{
    public float speed = 0.1f; // kecepatan naik (pelan!)

    void Update()
    {
        transform.localPosition += Vector3.up * speed * Time.deltaTime;
    }
}
