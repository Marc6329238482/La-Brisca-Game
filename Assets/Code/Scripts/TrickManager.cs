using System.Collections.Generic;
using UnityEngine;

public class TrickManager : MonoBehaviour
{
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
            if(players[p].playedCard.cardSuit == triumphSuit)
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
                if(players[p].playedCard.cardSuit == trickSuit)
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
                    if (winner.playedCard.cardRank < player.playedCard.cardRank)
                        winner = player;
                }
                AddScoredCards(winner, players);
                return winner;
            }
        }
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
    
}
