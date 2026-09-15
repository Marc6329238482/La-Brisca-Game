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

    void Awake()
    {
        cards = new List<Card>();
    }

    public Card PlayCard(Card selectedCard)
    {
        print("Played card is: " + selectedCard.GetCardRank() + " of " + selectedCard.GetCardSuit());
        if(RemoveCard(selectedCard))
            return selectedCard;
        else
            return null;
    }

    public void DrawCard(Card newCard, Player owner)
    {
        AddCard(newCard, owner);
    }

    ///--------------------Adding cards--------------------

    /// <summary>
    /// Adds a card.
    /// </summary>
    private bool AddCard(Card newCard, Player owner)
    {
        if(cards.Count >= maxCardsAmount)
            return false;
        
        cards.Add(newCard);
        AddDisplayCard(newCard, owner);
        return true;
    }

    private void AddDisplayCard(Card newCardData, Player owner)
    {
        GameObject newCard = Instantiate(cardDisplayPrefab);
        newCard.GetComponent<RectTransform>().SetParent(displayedCards);
        CardDisplay newCardDisplay = newCard.GetComponent<CardDisplay>();
        newCardDisplay.SetCardData(newCardData, owner);
        
    }

    ///--------------------Removing cards----------------

    /// <summary>
    /// Tryes to remove a card from the hand and returns true if succesfully removed the card, false if not.
    /// </summary>
    private bool RemoveCard(Card selectedCard)
    {
        return cards.Remove(selectedCard);
    }

    public void ClearHand()
    {
        cards.Clear();
        RemoveDisplayedCards();
    }

    private void RemoveDisplayedCards()
    {
        for(int c = 0; c < displayedCards.childCount; c++)
        {
            Destroy(displayedCards.GetChild(c).gameObject);
        }
    }
}
