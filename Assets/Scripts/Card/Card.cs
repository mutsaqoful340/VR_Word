using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Card : MonoBehaviour
{
    public float DisappearDelay = 2f;
    public float ColliderDisableDuration = 0.5f;
    CardSpawner.Card cardData;
    private CardSpawner spawner;
    private int cardIndex;
    private Collider cardCollider;
    private XRGrabInteractable cardXR;
    
    private void Awake()
    {
        cardCollider = GetComponent<Collider>();
        cardXR = GetComponent<XRGrabInteractable>();
    }
    
    public void SetCardData(CardSpawner.Card data, CardSpawner cardSpawner, int index)
    {
        cardData = data;
        spawner = cardSpawner;
        cardIndex = index;
    }
    
    public void On_PickUpCard()
    {
        cardData.isCardPickedUp = true;
    }

    public void On_CardActivated()
    {
        // Parent the card to the matching SpawnPoint
        if (spawner != null && cardIndex >= 0 && cardIndex < spawner.SpawnPoints.Length)
        {
            this.transform.SetParent(spawner.SpawnPoints[cardIndex].transform);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            
            // Disable collider temporarily to avoid XRGrab bug
            StartCoroutine(DisableColliderTemporarily());
        }
    }

    private IEnumerator DisableColliderTemporarily()
    {
        if (cardCollider != null)
        {
            cardCollider.enabled = false;
            cardXR.enabled = false;
            yield return new WaitForSeconds(ColliderDisableDuration);
            cardCollider.enabled = true;
            cardXR.enabled = true;
        }
    }
}
