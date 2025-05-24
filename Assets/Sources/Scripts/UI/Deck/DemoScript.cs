using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public int handSize;
    public int deckSize;

    public UICardsPile hand;
    public UICardsPile deck;

    public GameObject cardPrefab;

    private void Start()
    {
        for (var i = 0; i < handSize; i++)
            hand.Add(Instantiate(cardPrefab), false);
        
        for (var i = 0; i < deckSize; i++)
            deck.Add(Instantiate(cardPrefab), false);
    }

    public void SpawnCard()
    {
        if (deck.Cards.Count == 0)
            return;

        var card = deck.Cards[^1];
        deck.Remove(card);
        hand.Add(card, 0);
    }

    public void RemoveCard()
    {
        if (hand.Cards.Count == 0)
            return;
        
        var card = hand.Cards[^1];
        hand.Remove(card);
        deck.Add(card);
    }
}
