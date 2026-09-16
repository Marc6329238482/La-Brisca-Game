using System;

public static class EventManager
{
    public static event Action<CardDisplay> OnCardButtonClicked;
    public static event Action OnTrickEnded;

    public static void CardButtonClicked(CardDisplay cardDisplay)
    {
        OnCardButtonClicked?.Invoke(cardDisplay);
    }

    public static void TrickEnded()
    {
        OnTrickEnded?.Invoke();
    }
}
