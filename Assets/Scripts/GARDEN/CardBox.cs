using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardBox : MonoBehaviour
{
    [Header("Animator")]
    public Animator boxAnimator;

    [Header("Card Inside Box")]
    public GameObject card;

    [Header("Card Floating")]
    public float floatAmplitude = 0.2f;  // tinggi naik turun
    public float floatFrequency = 1f;    // kecepatan naik turun

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
            // naik turun sinusoidal
            Vector3 pos = cardStartLocalPos;
            pos.y += Mathf.Sin(Time.time * floatFrequency * 2 * Mathf.PI) * floatAmplitude;
            card.transform.localPosition = pos;
        }
    }

    public void Unlock()
    {
        if (isUnlocked) return;
        isUnlocked = true;

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
