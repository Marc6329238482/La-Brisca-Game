using System;

public static class EventManager
{
    public static event Action<CardDisplay> OnCardButtonClicked;

    public static void CardButtonClicked(CardDisplay cardDisplay)
    {
        OnCardButtonClicked?.Invoke(cardDisplay);
    }
}
