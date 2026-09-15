using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance { get; private set; }
    [SerializeField] public Player[] players;
    [SerializeField] public Deck deck;
    [SerializeField] public TrickManager trickManager;
    [SerializeField] private int initialCardsAmount;
    private int currentPlayer;
    private bool isNewTrickPlay = true;

    void OnEnable()
    {
        EventManager.OnCardButtonClicked += PlayClickedCard;
    }

    void OnDisable()
    {
        EventManager.OnCardButtonClicked -= PlayClickedCard;
    }

    private void Awake()
    {
        // 1. Verificar si ya existe una instancia
        if (Instance != null && Instance != this)
        {
            // Si ya existe otra, destruir este duplicado
            Destroy(gameObject);
            return;
        }

        // 2. Asignar la instancia actual
        Instance = this;

        // 3. Hacer que persista entre cambios de escena
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PrepareGame();
    }

    public void PrepareGame()
    {
        //Reset deck
        deck.CreateDeck();
        //Reset players
        ResetPlayers();
        //Deal cards
        DealCards(initialCardsAmount);
        //Discover Triumph Suit and set it
        trickManager.SetTriumphSuit(deck.DiscoverTriumphSuit().GetCardSuit());
        //Select random player to start
        currentPlayer = Random.Range(0, players.Length);
        isNewTrickPlay = true;
        //print("Game prepared");
    }

    /*public void PlayTestTrick()
    {
        print("Player " + players[currentPlayer].playerNumber + " is playing");
        players[currentPlayer].PlayRandomCard();
        if(isNewTrickPlay)
            if(players[currentPlayer].playedCard != null) //Players have a card to play
                trickManager.SetTrickSuit(players[currentPlayer].playedCard.GetCardSuit());
            else //The game ends
            {
                GetWinner();
                PrepareGame();
            }
        SelectNextPlayer();
        if(CheckAllPlayersHavePlayed())
        {
            Player winner = trickManager.CalculateTrickWinner();
            print("The player winner is: " + winner);
            RemovePlayerHands();
            DealCards(1);
            isNewTrickPlay = true;
        }
    }*/

    private void DealCards(int cardsToDeal)
    {
        if(deck.IsDeckEmpty()) //We dont try to deal if the dekc is empty
            return;
        //print("Dealing cards...");
        for(int c = 0; c < cardsToDeal; c++)
        {
            //print("Card number: " + c);
            DealOneCard();
        }
    }

    private void DealOneCard()
    {
        if(deck.IsDeckEmpty()) //We dont try to deal if the dekc is empty
            return;
        foreach(Player player in players)
        {
            Card card = deck.RemoveCard();
            if(card)
                player.DrawCard(card);
        }
    }

    

    private void SelectNextPlayer()
    {
        if(currentPlayer >= players.Length - 1)
            currentPlayer = 0;
        else
            currentPlayer++;
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

    private void RemovePlayerHands()
    {
        //print("Removing played cards from players...");
        foreach(Player player in players)
        {
            player.ClearPlayedCard();
        }
    }

    private Player GetWinner()
    {
        Player winner = players[0];
        int winnerScore = players[0].CalculateScore();
        //print("Player " + winner.playerNumber + " scored " + winnerScore);
        for(int p = 1; p < players.Length; p++)
        {
            int otherScore = players[p].CalculateScore();
            if(winnerScore < otherScore)
            {
                winnerScore = otherScore;
                winner = players[p];
            }
            //print("Player " + players[p].playerNumber + " scored " + otherScore);
        }
        return winner;
    }

    private void ResetPlayers()
    {
        for(int p = 0; p < players.Length; p++)
        {
            players[p].ClearHand();
            players[p].playerNumber = p + 1;
            players[p].scoredCards.Clear();
        }
    }

    private void PlayClickedCard(CardDisplay cardDisplay)
    {
        //print("Card clicked is: " + cardDisplay.GetCardData().GetCardRank() + " of " + cardDisplay.GetCardData().GetCardSuit());
        if(CheckPlayerTurn(cardDisplay.GetPlayerOwner()))
        {
            cardDisplay.GetPlayerOwner().PlayCard(cardDisplay); //Card owner (a player) plays the card

            //Destroy(cardDisplay.gameObject); //Destroy the gameobject card after being played

            if(isNewTrickPlay)
                if(players[currentPlayer].playedCard != null) //Players have a card to play
                    trickManager.SetTrickSuit(players[currentPlayer].playedCard.GetCardSuit());
                else //The game ends
                {
                    GetWinner();
                    PrepareGame();
                }
            SelectNextPlayer();
            if(CheckAllPlayersHavePlayed())
            {
                Player winner = trickManager.CalculateTrickWinner();
                //print("The player winner is: " + winner);
                RemovePlayerHands();
                DealCards(1);
                isNewTrickPlay = true;
            }
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

    private void CardToCenter()
    {

    }

    /*private IEnumerator AnimateCard(Vector 2 targetPosition)
    {
        Vector2 startPosition = windowTransform.anchoredPosition;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float percentage = timeElapsed / duration;
            
            // Evaluate the animation curve for smooth acceleration/deceleration
            float curveValue = easingCurve.Evaluate(percentage);
            
            windowTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, curveValue);
            yield return null; // Wait for the next frame
        }

        windowTransform.anchoredPosition = targetPosition; // Snap to final position
    }*/
}
