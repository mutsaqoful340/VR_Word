using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;
    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;

        if (other.CompareTag("Key"))
        {
            doorAnimator.SetTrigger("Open");
            isOpen = true;
            Debug.Log("Pintu terbuka pakai kunci");
        }
    }

    void TryOpenDoor()
    {
        if (isOpen) return;

        if (KeyObject.hasKey)
        {
            doorAnimator.SetTrigger("Open");
            isOpen = true;
            Debug.Log("Pintu terbuka");
        }
        else
        {
            Debug.Log("Pintu terkunci, butuh kunci");
        }
    }
}
