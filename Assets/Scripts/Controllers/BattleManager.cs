using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject _elementCard;
    // [SerializeField] GameObject _spellCard;

    [Header("Frontend- Lists")]
    [SerializeField] List<GameObject> _deckList;
    [SerializeField] List<GameObject> _discardList;
    [SerializeField] List<GameObject> _playerHandList;

    [Header("Frontend- Positions")]
    [SerializeField] Transform _deckPosition;
    [SerializeField] Transform[] _playerHandPositions;
    [SerializeField] Transform _discardPosition;

    // backend
    Deck<Card> _battleDeck = new Deck<Card>();
    Deck<Card> _discard = new Deck<Card>();
    Deck<Card> _playerHand = new Deck<Card>();

    int _setUpDeckIndex = 0;
    int _selectedCardIndex = -1;

    GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        SetupBattleDeck();
        ShowAllCardsInDeck();

        SetupPlayerHand();

        ShuffleDeck();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Draw());
        }
    }

    // assemble battle deck with game manager's deck to use in battle
    void SetupBattleDeck()
    {
        for (int i = 0; i < _gameManager.Deck.Count; i++)
        {
            _battleDeck.Add(_gameManager.Deck.GetCard(i));
        }
    }

    // display cards visually in battle deck
    void ShowAllCardsInDeck()
    {
        for (int i = 0; i < _deckList.Count; i++)
        {
            ElementCardView c = _deckList[i].GetComponent<ElementCardView>();
            ElementCard newCard = (ElementCard)_battleDeck.GetCard(i);
            c.Display(newCard);
        }
    }

    // set up player hand deck for back end use
    void SetupPlayerHand()
    {
        for (int i = 0; i < 5; i++)
        {
            ElementCard card = new ElementCard(_gameManager.DeckConfig[0]);
            _playerHand.Add(card, DeckPosition.Top);
        }
    }

    void ShuffleDeck()
    {
        // front end
        // TODO- cool shuffle animation
        StartCoroutine(MoveCardsInDiscardToDeck());

        // back end
        _battleDeck.Shuffle(_deckList);
    }

    IEnumerator MoveCardsInDiscardToDeck()
    {
        int count = _discard.Count;
        for (int i = 0; i < count; i++)
        {
            // front end- add cards in discard to deck
            CardMovement cardMovement = _discardList[0].GetComponent<CardMovement>();
            cardMovement.transform.parent = _deckPosition;
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _deckPosition;
            }
            _deckList.Add(_discardList[0]);
            _discardList.Remove(_discardList[0]);

            // back end- add cards in discard to deck
            _battleDeck.Add(_discard.GetCard(0));
            _discard.Remove(0);

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Draw()
    {
        if (_battleDeck.IsEmpty)
        {
            ShuffleDeck();

            yield break;
        }

        for (int i = 0; i < _playerHandPositions.Length; i++)
        {
            if (!_battleDeck.IsEmpty && _playerHandPositions[i].transform.childCount == 0)
            {
                // front end- get top card of deck, then move card to an empty hand slot
                GameObject cardDrawn = _deckList[_battleDeck.LastIndex];
                _playerHandList[i] = cardDrawn;

                cardDrawn.transform.parent = _playerHandPositions[i].transform;

                CardMovement cardMovement = cardDrawn.GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.TargetTransform = _playerHandPositions[i].transform;
                }

                _deckList.Remove(cardDrawn);

                yield return new WaitForSeconds(0.2f);

                // back end- draw from battle deck to player hand
                Card newCard = _battleDeck.Draw(DeckPosition.Top);
                _playerHand.LazyAdd(newCard, i);
            }
        }             
    }

    // SelectManager calls this function based on card slot picked
    public void SelectCard(int index)
    {
        _selectedCardIndex = index;

        RemoveCard();
    }

    public void RemoveCard()
    {
        // front end- remove selected card from player hand to discard
        GameObject selectedCard = _playerHandList[_selectedCardIndex];
        selectedCard.transform.parent = _discardPosition;

        CardMovement cardMovement = selectedCard.GetComponent<CardMovement>();
        if (cardMovement != null)
        {
            cardMovement.TargetTransform = _discardPosition;
        }

        _playerHandList[_selectedCardIndex] = null;
        _discardList.Add(selectedCard);

        // back end- remove selected card from player hand to discard
        Card card = _playerHand.GetCard(_selectedCardIndex);
        _discard.Add(card, DeckPosition.Top);
        _playerHand.LazyRemove(_selectedCardIndex);
    }
}
