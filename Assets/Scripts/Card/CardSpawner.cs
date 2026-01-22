using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CardSpawner : MonoBehaviour
{
    public static CardSpawner Instance { get; private set; }
    
    public class Card
    {
        public GameObject cardObject;
        public bool isCardPickedUp = false;
    }

    [Header("Manual Card Assignment (Optional)")]
    [Tooltip("Assign cards here if not using prefab spawning")]
    public GameObject[] ManuallyAssignedCards;

    public GameObject[] TPPoints;

    public List<Card> cards = new List<Card>();
    
    private bool[] spawnPointOccupied;

    private void Awake()
    {
        // Set singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple CardSpawner instances detected. Using the first one.");
        }
    }

    private void Start()
    {
        InitializeCards();
    }

    private void InitializeCards()
    {
        cards.Clear();
        spawnPointOccupied = new bool[TPPoints.Length];
        
        // Register manually assigned cards if they exist
        if (ManuallyAssignedCards != null && ManuallyAssignedCards.Length > 0)
        {
            foreach (GameObject cardObj in ManuallyAssignedCards)
            {
                if (cardObj != null)
                {
                    RegisterCard(cardObj);
                }
            }
        }
    }

    public void SpawnCards()
    {
        for (int i = 0; i < cards.Count && i < TPPoints.Length; i++)
        {
            if (i < TPPoints.Length && cards[i].cardObject != null)
            {
                if (cards[i].isCardPickedUp)
                {
                    // Spawn the card at the corresponding spawn point
                    cards[i].cardObject.transform.position = TPPoints[i].transform.position;
                    cards[i].cardObject.transform.rotation = TPPoints[i].transform.rotation;
                    cards[i].cardObject.SetActive(true);
                    
                    // Reset the pickup status
                    cards[i].isCardPickedUp = false;
                }
            }
        }
    }

    public void RegisterCard(GameObject cardGameObject)
    {
        // Register spawner reference with the card
        global::Card cardScript = cardGameObject.GetComponent<global::Card>();
        if (cardScript != null)
        {
            cardScript.SetSpawner(this);
        }
    }
    
    // Find the first available empty spawn point
    public int FindEmptySpawnPoint()
    {
        for (int i = 0; i < spawnPointOccupied.Length; i++)
        {
            if (!spawnPointOccupied[i])
            {
                return i;
            }
        }
        return -1; // No empty spawn point found
    }
    
    // Mark a spawn point as occupied
    public void OccupySpawnPoint(int index)
    {
        if (index >= 0 && index < spawnPointOccupied.Length)
        {
            spawnPointOccupied[index] = true;
        }
    }
    
    // Mark a spawn point as free
    public void FreeSpawnPoint(int index)
    {
        if (index >= 0 && index < spawnPointOccupied.Length)
        {
            spawnPointOccupied[index] = false;
        }
    }
}
