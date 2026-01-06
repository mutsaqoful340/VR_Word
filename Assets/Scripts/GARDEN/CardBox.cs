using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardBox : MonoBehaviour
{
    [Header("Animator")]
    public Animator boxAnimator;

    [Header("Card Inside Box")]
    public GameObject card;

    private bool isUnlocked = false;

    private void Start()
    {
        // box terkunci
        Collider boxCol = GetComponent<Collider>();
        if (boxCol != null)
            boxCol.enabled = false;

        // card sudah ada, tapi TIDAK bisa diambil
        if (card != null)
        {
            card.SetActive(true); // jangan pernah false

            Collider cardCol = card.GetComponent<Collider>();
            if (cardCol != null)
                cardCol.enabled = false;

            XRGrabInteractable grab = card.GetComponent<XRGrabInteractable>();
            if (grab != null)
                grab.enabled = false;
        }
    }

    public void Unlock()
    {
        Debug.Log("CARD BOX UNLOCK DIPANGGIL");

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
        Collider cardCol = card.GetComponent<Collider>();
        if (cardCol != null)
            cardCol.enabled = true;

        XRGrabInteractable grab = card.GetComponent<XRGrabInteractable>();
        if (grab != null)
            grab.enabled = true;
    }
}
