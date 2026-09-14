using System.Collections.Generic;
using UnityEngine;

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

    public void PlayRandomCard()
    {
        if(hand.cards.Count == 0)
            Debug.LogError("Hand from player " + this.name + " is empty!");
        else
        {
            int random = Random.Range(0, hand.cards.Count - 1);
            playedCard = hand.cards[random];
            print("Card played is: " + playedCard);
            hand.PlayCard(playedCard);
    
        }
    }

    public void DrawCard(Card newCard)
    {
        hand.DrawCard(newCard);
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
