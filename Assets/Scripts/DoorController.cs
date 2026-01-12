using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;
    
    [Header("Door Opened Events")]
    public UnityEvent onDoorOpened;

    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;

        if (other.CompareTag("Key"))
        {
            doorAnimator.SetTrigger("Open");
            isOpen = true;

            Debug.Log("Pintu terbuka pakai kunci");

            // Invoke all functions assigned in Inspector
            onDoorOpened?.Invoke();

            Destroy(other.gameObject);
        }
    }
}
