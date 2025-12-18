using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LetterObject : MonoBehaviour
{
    public string letter;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        LetterSlot slot = other.GetComponent<LetterSlot>();
        if (slot == null) return;
        if (slot.isFilled) return;

        slot.PlaceLetter(letter, gameObject); // ✅ FIX DI SINI
        LockObject();
    }

    private void LockObject()
    {
        if (grab)
            grab.enabled = false; // 🔥 kunci XR Grab

        if (rb)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;
    }
}
