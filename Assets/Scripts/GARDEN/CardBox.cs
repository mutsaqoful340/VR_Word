using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardBox : MonoBehaviour
{
    [Header("Animator")]
    public Animator boxAnimator;

    [Header("Card Inside Box")]
    public GameObject card;

    [Header("Card Floating")]
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 1f;

    [Header("Audio")]
    public AudioClip unlockSFX;
    public float audioVolume = 1f;

    private bool isUnlocked = false;
    private Vector3 cardStartLocalPos;

    private void Start()
    {
        if (card != null)
        {
            cardStartLocalPos = card.transform.localPosition;

            // box terkunci
            Collider boxCol = GetComponent<Collider>();
            if (boxCol != null)
                boxCol.enabled = false;

            // card tidak bisa diambil
            Collider cardCol = card.GetComponent<Collider>();
            if (cardCol != null)
                cardCol.enabled = false;

            XRGrabInteractable grab = card.GetComponent<XRGrabInteractable>();
            if (grab != null)
                grab.enabled = false;
        }
    }

    private void Update()
    {
        if (isUnlocked && card != null)
        {
            Vector3 pos = cardStartLocalPos;
            pos.y += Mathf.Sin(Time.time * floatFrequency * 2 * Mathf.PI) * floatAmplitude;
            card.transform.localPosition = pos;
        }
    }

    public void Unlock()
    {
        if (isUnlocked) return;
        isUnlocked = true;

        // 🔊 PLAY SOUND
        if (unlockSFX != null)
        {
            AudioSource.PlayClipAtPoint(
                unlockSFX,
                transform.position,
                audioVolume
            );
        }

        Collider boxCol = GetComponent<Collider>();
        if (boxCol != null)
            boxCol.enabled = true;

        if (boxAnimator != null)
            boxAnimator.SetTrigger("Unlock");

        EnableCard();
    }

    void EnableCard()
    {
        if (card == null) return;

        Collider cardCol = card.GetComponent<Collider>();
        if (cardCol != null)
            cardCol.enabled = true;

        XRGrabInteractable grab = card.GetComponent<XRGrabInteractable>();
        if (grab != null)
            grab.enabled = true;
    }
}
