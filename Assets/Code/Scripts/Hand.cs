using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public List<Card> cards;
    public int maxCardsAmount;
    [SerializeField] private GameObject cardDisplayPrefab;
    [SerializeField] private RectTransform displayedCards;
    private int cardCount = 1;

    void Awake()
    {
        cards = new List<Card>();
    }

    public Card PlayCard(Card selectedCard)
    {
        if(RemoveCard(selectedCard))
            return selectedCard;
        else
            return null;
    }

    public void DrawCard(Card newCard)
    {
        AddCard(newCard);
    }

    ///--------------------Adding cards--------------------

    /// <summary>
    /// Adds a card.
    /// </summary>
    private bool AddCard(Card newCard)
    {
        if(cards.Count >= maxCardsAmount)
            return false;
        newCard.cardHandNumber = cardCount;
        cardCount++;
        cards.Add(newCard);
        AddDisplayCard(newCard);
        return true;
    }

    private void AddDisplayCard(Card newCardData)
    {
        GameObject newCard = Instantiate(cardDisplayPrefab);
        newCard.GetComponent<RectTransform>().SetParent(displayedCards);
        CardDisplay newCardDisplay = newCard.GetComponent<CardDisplay>();
        newCardDisplay.SetCardData(newCardData);
        
    }

    ///--------------------Removing cards----------------

    /// <summary>
    /// Tryes to remove a card from the hand and returns true if succesfully removed the card, false if not.
    /// </summary>
    private bool RemoveCard(Card selectedCard)
    {
        if(cards.Remove(selectedCard))
        {
            Destroy(displayedCards.GetChild(cardCount).gameObject);
            cardCount--;
            return true;
        }
        return false;
        
    }

    public void ClearHand()
    {
        cards.Clear();
        RemoveDisplayedCards();
        cardCount = 1;
    }

    private void RemoveDisplayedCards()
    {
        for(int c = 0; c < displayedCards.childCount; c++)
        {
            Destroy(displayedCards.GetChild(c).gameObject);
        }
    }
}
