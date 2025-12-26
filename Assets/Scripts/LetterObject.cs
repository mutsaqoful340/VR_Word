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
    private bool isCollidingSlot = false;
    private LetterSlot currentSlot = null; // Track which slot this letter is in

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void OnHoverLetter_Enter()
    {
        if (alreadyPlaced || isCollidingSlot)
        {
            
        }
        else return;
    }

    public void OnHoverLetter_Exit()
    {

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
        if (slot) isCollidingSlot = true;

        // Temporarily disable grab to prevent scale issues during parenting
        if (grab)
            grab.enabled = false;

        Debug.Log($"Placing letter '{letter}' into slot");
        bool placed = slot.PlaceLetter(letter, gameObject);

        if (placed)
        {
            alreadyPlaced = true;
            currentSlot = slot; // Remember which slot we're in
            // Re-enable grab after a frame to allow unlock functionality
            Invoke(nameof(ReEnableGrab), 0.05f);

            // Lock physics
            if (rb)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Check puzzle after each placement
            WordPuzzleManager manager = FindObjectOfType<WordPuzzleManager>();
            if (manager != null)
            {
                manager.TrySolve(); // Will only solve if all slots filled AND word is correct
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Only clear slot and reset collision flag if letter is NOT already placed
        if (!alreadyPlaced)
        {
            LetterSlot slot = other.GetComponent<LetterSlot>();
            if (slot != null)
            {
                // Only clear if this was the slot we were actually interacting with
                // Don't clear other slots we might be passing through
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
            Invoke(nameof(ReEnableGrab), 0.1f); // Re-enable after 0.1 seconds
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

        // Reset placement state
        if (alreadyPlaced && currentSlot != null)
        {
            Debug.Log($"Clearing slot {currentSlot.slotIndex} from letter '{letter}'");
            currentSlot.ClearSlot();
            currentSlot = null;
            alreadyPlaced = false;
            isCollidingSlot = false; // Reset collision flag
            
            // Unparent from slot
            transform.SetParent(null);
        }

        // Re-enable physics
        // if (rb)
        //     rb.isKinematic = false;
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
