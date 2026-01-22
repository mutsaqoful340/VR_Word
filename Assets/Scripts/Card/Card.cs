using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Card : MonoBehaviour
{
    public float DisappearDelay = 2f;
    public float ColliderDisableDuration = 0.5f;
    private CardSpawner spawner;
    private int currentSpawnPointIndex = -1; // Track which spawn point this card occupies
    private Collider cardCollider;
    private XRGrabInteractable cardXR;
    
    private void Awake()
    {
        cardCollider = GetComponent<Collider>();
        cardXR = GetComponent<XRGrabInteractable>();
        
        // Auto-discover spawner if not already set
        if (spawner == null && CardSpawner.Instance != null)
        {
            SetSpawner(CardSpawner.Instance);
            Debug.Log("Card '" + gameObject.name + "' found CardSpawner with " + CardSpawner.Instance.TPPoints.Length + " teleport points");
        }
    }
    
    public void SetSpawner(CardSpawner cardSpawner)
    {
        spawner = cardSpawner;
    }
    
    public void On_PickUpCard()
    {
        // Free the spawn point when card is picked up
        if (spawner != null && currentSpawnPointIndex >= 0)
        {
            spawner.FreeSpawnPoint(currentSpawnPointIndex);
            currentSpawnPointIndex = -1;
        }
    }

    public void On_CardActivated()
    {
        // Find an empty spawn point and teleport the card there
        Debug.Log("Card Activated: " + gameObject.name);
        
        if (spawner == null)
        {
            Debug.LogError("Spawner is null! Make sure to call RegisterCard() on the CardSpawner when spawning this card.");
            return;
        }
        
        if (spawner.TPPoints == null || spawner.TPPoints.Length == 0)
        {
            Debug.LogError("TPPoints array is null or empty!");
            return;
        }
        
        // Free the current spawn point if occupied
        if (currentSpawnPointIndex >= 0)
        {
            spawner.FreeSpawnPoint(currentSpawnPointIndex);
            Debug.Log("Freed spawn point: " + currentSpawnPointIndex);
        }
        
        // Find an empty spawn point
        int emptySpawnPointIndex = spawner.FindEmptySpawnPoint();
        Debug.Log("Found empty spawn point index: " + emptySpawnPointIndex);
        
        if (emptySpawnPointIndex >= 0 && emptySpawnPointIndex < spawner.TPPoints.Length)
        {
            Debug.Log("Teleporting to TP point " + emptySpawnPointIndex + " at position: " + spawner.TPPoints[emptySpawnPointIndex].transform.position);
            
            // Teleport the card to the TPPoint
            this.transform.SetParent(spawner.TPPoints[emptySpawnPointIndex].transform);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            
            // Mark this spawn point as occupied
            spawner.OccupySpawnPoint(emptySpawnPointIndex);
            currentSpawnPointIndex = emptySpawnPointIndex;
            
            Debug.Log("Card successfully teleported to spawn point " + emptySpawnPointIndex);
            
            // Disable collider temporarily to avoid XRGrab bug
            StartCoroutine(DisableColliderTemporarily());
        }
        else
        {
            Debug.LogWarning("No empty spawn point available for card: " + gameObject.name);
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
