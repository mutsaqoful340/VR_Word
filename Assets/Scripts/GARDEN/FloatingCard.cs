using UnityEngine;

public class FloatingCard : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatAmplitude = 0.05f;  // cuma sedikit naik-turun
    public float floatFrequency = 1f;     // kecepatan wajar
    public bool enableFloating = false;   // aktifkan floating setelah unlock

    private Vector3 startLocalPos;

    private void Start()
    {
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (!enableFloating) return;

        Vector3 pos = startLocalPos;
        pos.y += Mathf.Sin(Time.time * floatFrequency * 2 * Mathf.PI) * floatAmplitude;
        transform.localPosition = pos;
    }

    public void EnableFloating()
    {
        enableFloating = true;
    }

    public void DisableFloating()
    {
        enableFloating = false;
        transform.localPosition = startLocalPos;
    }
}
