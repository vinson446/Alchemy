using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckTester : MonoBehaviour
{
    // Element Deck
    [SerializeField] List<ElementCardData> _elementDeckConfig = new List<ElementCardData>();
    [SerializeField] ElementCardView _elementCardView = null;

    // Spell Deck
    [SerializeField] List<SpellCardData> _spellDeckConfig = new List<SpellCardData>();
    [SerializeField] SpellCardView _spellCardView = null;

    Deck<Card> _deck = new Deck<Card>();
    Deck<Card> _discard = new Deck<Card>();
    Deck<Card> _playerHand = new Deck<Card>();

    private void Start()
    {
        ShuffleDeck();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Draw();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            PrintPlayerHand();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayTopCard();
        }
    }

    private void SetupElementDeck()
    {
        foreach(ElementCardData elementData in _elementDeckConfig)
        {
            ElementCard newElementCard = new ElementCard(elementData);
            _elementCardView.Display(newElementCard);

            _deck.Add(newElementCard);
        }
    }

    private void SetupSpellDeck()
    {
        foreach (SpellCardData spellData in _spellDeckConfig)
        {
            SpellCard newSpellCard = new SpellCard(spellData);
            _spellCardView.Display(newSpellCard);

            _deck.Add(newSpellCard);
        }
    }

    private void Draw()
    {
        // draw card and add it to player hand if we have cards in deck
        if (!_deck.IsEmpty)
        {
            Card newCard = _deck.Draw(DeckPosition.Top);
            Debug.Log(newCard.Name);

            _playerHand.Add(newCard, DeckPosition.Top);
        }
        // reshuffle deck
        else
        {
            ShuffleDeck();
        }
    }

    void PrintPlayerHand()
    {
        for (int i = 0; i < _playerHand.Count; i++)
        {
            Debug.Log("Player Hand Card: " + _playerHand.GetCard(i).Name);
        }
    }

    void PlayTopCard()
    {
        Card targetCard = _playerHand.TopItem;

        targetCard.Play();

        // TODO- consider expanding Remove to accept a deck position
        _playerHand.Remove(_playerHand.LastIndex);

        _discard.Add(targetCard);
    }

    void ShuffleDeck()
    {
        SetupElementDeck();
        SetupSpellDeck();

        _deck.Shuffle();
    }
}
