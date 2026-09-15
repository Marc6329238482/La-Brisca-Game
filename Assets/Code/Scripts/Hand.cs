using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public List<Card> cards;
    public int maxCardsAmount;
    [SerializeField] private RectTransform cardsDisplay;

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

    /// <summary>
    /// Adds a new card to the hand.
    /// </summary>
    public void DrawCard(Card newCard)
    {
        AddCard(newCard);
    }

    public void ClearHand()
    {
        cards.Clear();
    }

    public void AddDisplayCard(Card newDataCard)
    {
        CardDisplay newCardDisplay = new CardDisplay();
        newCardDisplay.cardData = newDataCard;
    }

    /// <summary>
    /// Adds a card.
    /// </summary>
    private bool AddCard(Card newCard)
    {
        if(cards.Count >= maxCardsAmount)
            return false;
        
        cards.Add(newCard);
        return true;
    }

    /// <summary>
    /// Tryes to remove a card from the hand and returns true if succesfully removed the card, false if not.
    /// </summary>
    private bool RemoveCard(Card selectedCard)
    {
        return cards.Remove(selectedCard);
    }
}
