using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEffect12 : MonoBehaviour
{
    public float amplitude = 0.05f;
    public float frequency = 1f;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos +
            Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
    }
}
