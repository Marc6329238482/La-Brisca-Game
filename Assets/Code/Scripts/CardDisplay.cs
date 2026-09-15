using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private Card cardData;
    [SerializeField] private Image image;
    private Player playerOwner;

    public void SetCardData(Card newCardData, Player owner)
    {
        cardData = newCardData;
        image.sprite = cardData.GetCardSprite();
        playerOwner = owner;
    }

    public Card GetCardData()
    {
        return cardData;
    }

    public void onButtonClick()
    {
        EventManager.CardButtonClicked(this);
    }

    public Player GetPlayerOwner()
    {
        return playerOwner;
    }

}
