using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;

    [Header("Door Opened Events")]
    public UnityEvent onDoorOpened;

    [Header("Door Sound")]
    public AudioSource doorAudioSource; // drag AudioSource yang ada clip nya

    private bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;

        if (other.CompareTag("Key"))
        {
            // Animasi buka pintu
            doorAnimator.SetTrigger("Open");
            isOpen = true;

            // Mainkan suara pintu
            if (doorAudioSource != null)
            {
                doorAudioSource.Play();
            }

            Debug.Log("Pintu terbuka pakai kunci");

            // Panggil semua fungsi yang di-assign di Inspector
            onDoorOpened?.Invoke();

            // Hancurkan key
            Destroy(other.gameObject);
        }
    }
}
