using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UICardsPile : MonoBehaviour
{
    public float height = 50f; // В пикселях
    public float width = 100f; // В пикселях
    [Range(0f, 90f)] public float maxCardAngle = 5f;
    public float yPerCard = -2f;

    public float moveDuration = 0.5f;
    public RectTransform cardHolderPrefab;

    readonly List<GameObject> cards = new();
    readonly List<RectTransform> cardsHolders = new();

    public List<GameObject> Cards => new List<GameObject>(cards);
    public event Action<int> OnCountChanged;

    private bool updatePositions;
    readonly List<GameObject> forceSetPosition = new();

    public void Add(GameObject card, bool moveAnimation = true) => Add(card, -1, moveAnimation);

    public void Add(GameObject card, int index, bool moveAnimation = true)
    {
        var cardHolder = GetCardHolder();

        if (index == -1)
        {
            cards.Add(card);
            cardsHolders.Add(cardHolder);
        }
        else
        {
            cards.Insert(index, card);
            cardsHolders.Insert(index, cardHolder);
        }

        updatePositions = true;

        if (!moveAnimation)
            forceSetPosition.Add(card);

        OnCountChanged?.Invoke(cards.Count);
    }

    public void Remove(GameObject card)
    {
        if (!cards.Contains(card))
            return;

        var index = cards.IndexOf(card);
        var cardHolder = cardsHolders[index];
        cardsHolders.RemoveAt(index);
        Destroy(cardHolder.gameObject);

        cards.RemoveAt(index);
        card.transform.DOKill();
        card.transform.SetParent(null);
        updatePositions = true;

        OnCountChanged?.Invoke(cards.Count);
    }

    public void RemoveAt(int index) => Remove(cards[index]);

    public void RemoveAll()
    {
        while (cards.Count > 0)
            Remove(cards[0]);
    }

    private RectTransform GetCardHolder()
    {
        var cardHolder = Instantiate(cardHolderPrefab, transform as RectTransform, false);
        return cardHolder;
    }

    private void UpdatePositions()
    {
        var radius = Mathf.Abs(height) < 0.001f
            ? width * width / 0.001f * Mathf.Sign(height)
            : height / 2f + width * width / (8f * height);

        var angle = 2f * Mathf.Asin(0.5f * width / radius) * Mathf.Rad2Deg;
        angle = Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), maxCardAngle * (cards.Count - 1));
        var cardAngle = cards.Count == 1 ? 0f : angle / (cards.Count - 1f);

        for (var i = 0; i < cards.Count; i++)
        {
            var cardRect = cards[i].GetComponent<RectTransform>();
            cardRect.SetParent(transform, true);

            var position = new Vector2(0f, radius);
            position = Quaternion.Euler(0f, 0f, angle / 2f - cardAngle * i) * position;
            position.y += height - radius;
            position += i * new Vector2(0f, yPerCard);

            cardsHolders[i].anchoredPosition = position;
            cardsHolders[i].localEulerAngles = new Vector3(0f, 0f, angle / 2f - cardAngle * i);

            cardRect.SetParent(cardsHolders[i], true);

            if (!forceSetPosition.Contains(cards[i]))
            {
                cardRect.DOKill();
                cardRect.DOLocalMove(Vector3.zero, moveDuration);
                cardRect.DOLocalRotate(Vector3.zero, moveDuration);
                cardRect.DOScale(Vector3.one, moveDuration);
            }
            else
            {
                forceSetPosition.Remove(cards[i]);
                cardRect.localPosition = Vector3.zero;
                cardRect.localRotation = Quaternion.identity;
                cardRect.localScale = Vector3.one;
            }
        }
    }

    private void LateUpdate()
    {
        if (!updatePositions) 
            return;
        updatePositions = false;
        UpdatePositions();
    }

    private void OnValidate() => updatePositions = true;
}
