using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class TrickManager : MonoBehaviour
{
    //[SerializeField] private GameObject cardDisplayPrefab;
    [SerializeField] private float cardAnimationDuration;
    [SerializeField] private AnimationCurve easingCurve;
    [SerializeField] private RectTransform centerTableTransform; //A basic rect transform in the main canvas indicating the center of the table
    private Player[] players;
    private Card.Suit triumphSuit;
    private Card.Suit trickSuit;

    void Start()
    {
        players = GameManager.Instance.players;
    }

    public void SetTriumphSuit(Card.Suit newSuit)
    {
        triumphSuit = newSuit;
    }

    public void SetTrickSuit(Card.Suit newSuit)
    {
        trickSuit = newSuit;
    }

    public Player CalculateTrickWinner()
    {
        List<Player> trickWinners = new List<Player>();
        //First look the cards that are triumph suit, those are possible winners
        for(int p = 0; p < players.Length; p++)
        {
            if(players[p].playedCard.GetCardSuit() == triumphSuit)
            {
                trickWinners.Add(players[p]);
            }
        }

        //If there is only 1, that's the winner
        if(trickWinners.Count == 1)
        {
            AddScoredCards(trickWinners[0], players);
            return trickWinners[0];
        }
        else
        {
            trickWinners.Clear();
            //if not lets check for the trickSuit
            for(int p = 0; p < players.Length; p++)
            {
                if(players[p].playedCard.GetCardSuit() == trickSuit)
                {
                    trickWinners.Add(players[p]);
                }
            }
            //If there is only 1, that's the winner
            if(trickWinners.Count == 1)
            {
                AddScoredCards(trickWinners[0], players);
                return trickWinners[0];
            }
            
            else
            {
                Player winner = players[0];
                foreach(Player player in players)
                {
                    //The card rank is bigger
                    if (winner.playedCard.GetCardRank() < player.playedCard.GetCardRank())
                        winner = player;
                }
                AddScoredCards(winner, players);
                return winner;
            }
        }
    }

    public void CardToCenter(CardDisplay cardDisplay)
    {
        RectTransform cardRect = cardDisplay.GetComponent<RectTransform>();
        cardRect.SetParent(centerTableTransform, true);
        
        StartCoroutine(AnimateCard(Vector2.zero, cardRect));
    }
    private void AddScoredCards(Player trickWinner, Player[] players)
    {
        foreach(Player player in players)
        {
            trickWinner.scoredCards.Add(player.playedCard);
        }
    }

    
    private void RandomTriumphSuit()
    {
        SetTriumphSuit((Card.Suit)Random.Range(0, 3));
    }

    /*private GameObject CreateCardDisplay(Card playedCard)
    {
        GameObject newCard = Instantiate(cardDisplayPrefab);
        CardDisplay newCardDisplay = newCard.GetComponent<CardDisplay>();
        newCardDisplay.SetCardData(playedCard, null);
        return newCard;
    }*/

    

    private IEnumerator AnimateCard(Vector2 targetPosition, RectTransform selectedCard)
    {
        Vector2 startPosition = selectedCard.anchoredPosition;
        float timeElapsed = 0f;

        while (timeElapsed < cardAnimationDuration)
        {
            timeElapsed += Time.deltaTime;
            float percentage = timeElapsed / cardAnimationDuration;
            
            // Evaluate the animation curve for smooth acceleration/deceleration
            float curveValue = easingCurve.Evaluate(percentage);
            
            selectedCard.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, curveValue);
            yield return null; // Wait for the next frame
        }

        selectedCard.anchoredPosition = targetPosition; // Snap to final position
    }
    
}
