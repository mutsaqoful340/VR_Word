using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CardSpawner : MonoBehaviour
{
    public class Card
    {
        public GameObject cardObject;
        public bool isCardPickedUp = false;
    }

    public GameObject[] Cards;

    public GameObject[] SpawnPoints;

    public List<Card> cards = new List<Card>();

    private void Start()
    {
        InitializeCards();
    }

    private void InitializeCards()
    {
        cards.Clear();
        int count = Mathf.Min(Cards.Length, SpawnPoints.Length);
        
        for (int i = 0; i < count; i++)
        {
            Card newCard = new Card();
            newCard.cardObject = Cards[i];
            cards.Add(newCard);
            
            // Register the card with its index
            if (Cards[i] != null)
            {
                global::Card cardScript = Cards[i].GetComponent<global::Card>();
                if (cardScript != null)
                {
                    cardScript.SetCardData(newCard, this, i);
                }
            }
        }
    }

    public void SpawnCards()
    {
        for (int i = 0; i < cards.Count && i < SpawnPoints.Length; i++)
        {
            if (i < SpawnPoints.Length && cards[i].cardObject != null)
            {
                if (cards[i].isCardPickedUp)
                {
                    // Spawn the card at the corresponding spawn point
                    cards[i].cardObject.transform.position = SpawnPoints[i].transform.position;
                    cards[i].cardObject.transform.rotation = SpawnPoints[i].transform.rotation;
                    cards[i].cardObject.SetActive(true);
                    
                    // Reset the pickup status
                    cards[i].isCardPickedUp = false;
                }
            }
        }
    }

    public void RegisterCard(GameObject cardGameObject, int index)
    {
        if (index >= 0 && index < cards.Count)
        {
            cards[index].cardObject = cardGameObject;
            
            // Set the card's reference to this card data
            global::Card cardScript = cardGameObject.GetComponent<global::Card>();
            if (cardScript != null)
            {
                cardScript.SetCardData(cards[index], this, index);
            }
        }
    }
}
