using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class FloatingEffect : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatHeight = 0.05f;

    private Vector3 startPos;
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
    }

    void Start()
    {
        startPos = transform.localPosition;

        // 🔹 STOP floating saat di-grab
        grab.selectEntered.AddListener(OnGrab);
    }

    void Update()
    {
        if (grab.isSelected) return;

        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPos + new Vector3(0, yOffset, 0);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // pastikan posisi rapi saat diambil
        transform.localPosition = startPos;
    }
}
