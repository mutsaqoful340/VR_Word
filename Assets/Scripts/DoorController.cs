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

            // 🔥 HANCURKAN KUNCI
            Destroy(other.gameObject);
        }
    }
}
