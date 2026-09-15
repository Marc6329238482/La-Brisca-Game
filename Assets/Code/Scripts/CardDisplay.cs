using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Sprite sprite;

    void Start()
    {
        sprite = cardData.GetCardSprite();
    }

}
