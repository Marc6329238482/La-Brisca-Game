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
    private int currentPlayer;
    private bool isNewTrickPlay = true;

    //-----CORE FUNCTIONS------
    void Start()
    {
        players = GameManager.Instance.players;
        //Select random player to start
    }

    //-----PUBLIC FUNCTIONS------
    public void CardPlayed(CardDisplay cardDisplay)
    {
        if(CheckPlayerTurn(cardDisplay.GetPlayerOwner())) //If the card owner is the corresponding player
        {
            CardToCenter(cardDisplay); //Put the card played on the center of the table
            cardDisplay.GetPlayerOwner().PlayCard(cardDisplay);
            if(isNewTrickPlay)
            {
                if(players[currentPlayer].playedCard != null) 
                    SetTrickSuit(players[currentPlayer].playedCard.GetCardSuit()); //In new tricks we set the new trick suit with the first played card
            }
            SelectNextPlayer();
            if(CheckAllPlayersHavePlayed())
            {
                Player winner = CalculateTrickWinner();
                print("The trick winner is: " + winner);
                ClearTableVisuals();
                isNewTrickPlay = true;
                EventManager.TrickEnded();
            }
        }
    }

    public void ResetTrick()
    {
        currentPlayer = Random.Range(0, players.Length);
        isNewTrickPlay = true;
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

    public void ClearTableVisuals()
    {
        // Recorremos todos los hijos del contenedor del centro de la mesa
        // Se hace en un bucle inverso o guardando referencias, porque cambiar de padre ROMPE el bucle foreach
        CardDisplay[] cardsOnTable = centerTableTransform.GetComponentsInChildren<CardDisplay>();
        
        foreach(CardDisplay card in cardsOnTable)
        {
            GameManager.Instance.ReturnCardToPool(card);
        }
    }

    //-----PRIVATE FUNCTIONS------
    private void AddScoredCards(Player trickWinner, Player[] players)
    {
        foreach(Player player in players)
        {
            trickWinner.scoredCards.Add(player.playedCard);
        }
    }

    private bool CheckPlayerTurn(Player owner)
    {
        if(players[currentPlayer] != owner)
        {
            Debug.LogWarning("Wait for your turn!");
            return false;
        }
        else
            return true;
    }

    private void SelectNextPlayer()
    {
        if(currentPlayer >= players.Length - 1)
            currentPlayer = 0;
        else
            currentPlayer++;
    }

    private void CardToCenter(CardDisplay cardDisplay)
    {
        RectTransform cardRect = cardDisplay.GetComponent<RectTransform>();
        cardRect.SetParent(centerTableTransform, true);
        
        //StartCoroutine(AnimateCard(cardRect));
        
    }

    

    private bool CheckAllPlayersHavePlayed()
    {
        foreach(Player player in players)
        {
            if(player.playedCard == null)
                return false;
        }
        //print("All players have played");
        return true;
    }

    private IEnumerator AnimateCard(RectTransform selectedCard)
    {
        Vector2 targetPosition = centerTableTransform.position;
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
        //selectedCard.SetParent(centerTableTransform, true);
    }
    
}
