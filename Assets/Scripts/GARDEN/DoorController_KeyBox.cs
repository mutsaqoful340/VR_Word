using UnityEngine;
using UnityEngine.Events;

public class DoorController_KeyBox : MonoBehaviour
{
    public Animator doorAnimator;
    public AudioSource doorAudioSource;

    public UnityEvent onDoorOpened;

    private bool isOpen = false;

    public void OpenDoorFromKeyBox()
    {
        if (isOpen) return;

        isOpen = true;

        if (doorAnimator != null)
            doorAnimator.SetTrigger("Open");

        if (doorAudioSource != null)
            doorAudioSource.Play();

        Debug.Log("Pintu terbuka dari KeyBox");

        onDoorOpened?.Invoke();
    }
}
