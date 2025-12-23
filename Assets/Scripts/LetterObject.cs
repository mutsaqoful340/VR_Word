using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LetterObject : MonoBehaviour
{
    public string letter;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    private Vector3 startPos;
    private Quaternion startRot;

    private bool alreadyPlaced = false;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyPlaced) return;

        LetterSlot slot = other.GetComponent<LetterSlot>();
        if (slot == null) return;
        if (slot.isFilled) return;

        bool correct = slot.PlaceLetter(letter, gameObject);

        if (correct)
        {
            alreadyPlaced = true;
            LockObject();   // ✅ kunci huruf

            // 🔥 TAMBAHAN PENTING
            WordPuzzleManager manager = FindObjectOfType<WordPuzzleManager>();
            if (manager != null)
            {
                manager.TrySolve(); // 👉 INI yang buka pintu & spawn key
            }
        }
        else
        {
            ReturnToStart(); // ❌ salah → balik
        }
    }

    private void LockObject()
    {
        if (grab)
            grab.enabled = false;

        if (rb)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Collider col = GetComponent<Collider>();
        if (col) col.enabled = false;
    }

    private void ReturnToStart()
    {
        transform.SetParent(null);

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPos;
        transform.rotation = startRot;

        rb.isKinematic = false;
    }
}
