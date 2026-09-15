using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Card> scoredCards;
    public Card playedCard;
    public int playerNumber;
    private Hand hand;

    void Awake()
    {
        hand = GetComponent<Hand>();
    }

    /*public void PlayRandomCard()
    {
        if(hand.cards.Count == 0)
            Debug.LogError("Hand from player " + this.name + " is empty!");
        else
        {
            int random = Random.Range(0, hand.cards.Count - 1);
            PlayCard(hand.displayedCards.GetChild(random));
        }
    }*/

    public void PlayCard(CardDisplay newPlayedCard)
    {
        playedCard = newPlayedCard.GetCardData();
        hand.PlayCard(playedCard);
        //print("Card played is: " + playedCard);
    }

    public void DrawCard(Card newCard)
    {
        hand.DrawCard(newCard, this);
    }

    public void ClearHand()
    {
        hand.ClearHand();
    }

    public void ClearPlayedCard()
    {
        playedCard = null;
    }

    public int CalculateScore()
    {
        int score = 0;
        foreach(Card card in scoredCards)
        {
            score += card.CalculateCardValue();
        }
        return score;
    }

}
