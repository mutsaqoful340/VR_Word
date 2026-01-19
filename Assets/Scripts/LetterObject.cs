using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LetterObject : MonoBehaviour
{
    [Header("Letter")]
    public string letter;

    [Header("Audio")]
    public AudioSource suaraHuruf;

    private XRGrabInteractable grab;
    private Rigidbody rb;

    private Vector3 startPos;
    private Quaternion startRot;

    private bool alreadyPlaced = false;
    private bool isCollidingSlot = false;
    private LetterSlot currentSlot = null;

    // 🔊 anti spam suara
    private bool sudahDiputar = false;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;

        // 🔊 EVENT AUDIO (TAMBAHAN)
        grab.selectEntered.AddListener(OnGrabSound);
        grab.selectExited.AddListener(OnReleaseSound);
    }

    private void OnDestroy()
    {
        if (grab)
        {
            grab.selectEntered.RemoveListener(OnGrabSound);
            grab.selectExited.RemoveListener(OnReleaseSound);
        }
    }

    // ===============================
    // 🔊 AUDIO SAAT DI-GRAB
    void OnGrabSound(SelectEnterEventArgs args)
    {
        if (suaraHuruf != null && !sudahDiputar)
        {
            suaraHuruf.Stop();
            suaraHuruf.Play();
            sudahDiputar = true;
        }
    }

    void OnReleaseSound(SelectExitEventArgs args)
    {
        sudahDiputar = false;
    }
    // ===============================

    public void OnHoverLetter_Enter()
    {
        if (alreadyPlaced || isCollidingSlot)
        {
            // fitur lama (tidak diubah)
        }
        else return;
    }

    public void OnHoverLetter_Exit()
    {
        // fitur lama (tidak diubah)
    }

    public void OnGrabLetter_Enter()
    {
        if (alreadyPlaced || isCollidingSlot)
        {
            UnlockObject();
        }
        else return;
    }

    public void OnGrabLetter_Exit()
    {
        if (!alreadyPlaced && isCollidingSlot)
        {
            LockObject();
        }
        else return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyPlaced) return;

        LetterSlot slot = other.GetComponent<LetterSlot>();
        if (slot == null) return;
        if (slot.isFilled) return;

        isCollidingSlot = true;

        if (grab)
            grab.enabled = false;

        Debug.Log($"Placing letter '{letter}' into slot");

        bool placed = slot.PlaceLetter(letter, gameObject);

        if (placed)
        {
            alreadyPlaced = true;
            currentSlot = slot;

            Invoke(nameof(ReEnableGrab), 0.05f);

            if (rb)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            WorldPuzzle manager = FindObjectOfType<WorldPuzzle>();
            if (manager != null)
                manager.TrySolve();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!alreadyPlaced)
        {
            LetterSlot slot = other.GetComponent<LetterSlot>();
            if (slot != null)
            {
                if (!slot.isFilled || slot.currentLetter == letter)
                {
                    slot.ClearSlot();
                }
                isCollidingSlot = false;
            }
        }
    }

    private void ReEnableGrab()
    {
        if (grab && alreadyPlaced)
        {
            grab.enabled = true;
        }
    }

    private void LockObject()
    {
        if (grab)
        {
            grab.enabled = false;
            Invoke(nameof(ReEnableGrab), 0.1f);
        }

        if (rb)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void UnlockObject()
    {
        Debug.Log($"UnlockObject called for letter '{letter}' - alreadyPlaced={alreadyPlaced}");

        if (grab)
            grab.enabled = true;

        if (alreadyPlaced && currentSlot != null)
        {
            Debug.Log($"Clearing slot {currentSlot.slotIndex} from letter '{letter}'");

            currentSlot.ClearSlot();
            currentSlot = null;
            alreadyPlaced = false;
            isCollidingSlot = false;

            transform.SetParent(null);
        }

        // FISIKA LAMA TIDAK DIUBAH
        // rb.isKinematic = false;
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
