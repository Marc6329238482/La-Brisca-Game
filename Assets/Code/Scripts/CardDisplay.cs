using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private Card cardData;
    [SerializeField] private Image image;

    public void SetCardData(Card newCardData)
    {
        cardData = newCardData;
        image.sprite = cardData.GetCardSprite();
        
    }

}
