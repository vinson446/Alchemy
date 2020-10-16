using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

enum BattleState { Draw, PlayerTurn, Battle };

public class BattleManager : MonoBehaviour
{
    [Header("Battle State")]
    [SerializeField] BattleState _battleState;

    [Header("Frontend- Lists")]
    [SerializeField] List<GameObject> _deckList;
    [SerializeField] List<GameObject> _discardList;
    [SerializeField] List<GameObject> _playerHandList;

    [Header("Frontend- Positions")]
    [SerializeField] Transform _deckPosition;
    [SerializeField] Transform[] _playerHandPositions;
    [SerializeField] Transform _discardPosition;

    [Header("Backend")]
    [SerializeField] List<ElementCardData> _elementDeckConfig = new List<ElementCardData>();
    [SerializeField] List<SpellCardData> _spellDeckConfig = new List<SpellCardData>();

    [Header("Prefabs")]
    [SerializeField] GameObject _elementCard;
    [SerializeField] GameObject _spellCard;

    Deck<Card> _deck = new Deck<Card>();
    Deck<Card> _discard = new Deck<Card>();
    Deck<Card> _playerHand = new Deck<Card>();

    int setUpDeckIndex = 0;
    int selectedCardIndex = -1;

    private void Start()
    {
        SetupElementDeck();
        SetupSpellDeck();

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

    public void ChangeBattleState(int index)
    {
        _battleState = (BattleState)index;
    }

    void SetupPlayerHand()
    {
        for (int i = 0; i < 5; i++)
        {
            ElementCard card = new ElementCard(_elementDeckConfig[0]);
            _playerHand.Add(card, DeckPosition.Top);
        }
    }

    void ShuffleDeck()
    {
        // frontend
        // TODO- cool shuffle animation
        StartCoroutine(MoveCardsInDiscardToDeck());

        // backend
        _deck.Shuffle(_deckList);
    }

    IEnumerator MoveCardsInDiscardToDeck()
    {
        int count = _discard.Count;
        for (int i = 0; i < count; i++)
        {
            // frontend- move card objs in discard list to deck list 
            CardMovement cardMovement = _discardList[0].GetComponent<CardMovement>();
            cardMovement.transform.parent = _deckPosition;
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _deckPosition;
            }
            _deckList.Add(_discardList[0]);
            _discardList.Remove(_discardList[0]);

            // backend- add cards in discard to deck
            _deck.Add(_discard.GetCard(0));
            _discard.Remove(0);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void SetupElementDeck()
    {
        foreach (ElementCardData elementData in _elementDeckConfig)
        {
            ElementCard newElementCard = new ElementCard(elementData);

            // frontend- update card stats on each card visually
            ElementCardView newElementCardView = _deckList[setUpDeckIndex].GetComponent<ElementCardView>();
            if (newElementCardView != null)
            {
                newElementCardView.Display(newElementCard);
            }
            setUpDeckIndex += 1;

            // backend- add element card to deck list
            _deck.Add(newElementCard);
        }
    }

    private void SetupSpellDeck()
    {
        foreach (SpellCardData spellData in _spellDeckConfig)
        {
            SpellCard newSpellCard = new SpellCard(spellData);

            // frontend- update card stats on card visually;
            SpellCardView newSpellCardView = _deckList[setUpDeckIndex].GetComponent<SpellCardView>();
            if (newSpellCardView != null)
            {
                newSpellCardView.Display(newSpellCard);
            }
            setUpDeckIndex += 1;

            // backend- add spell card to decklist
            _deck.Add(newSpellCard);
        }
    }

    IEnumerator Draw()
    {
        ChangeBattleState(0);

        if (_deck.IsEmpty)
        {
            ShuffleDeck();

            yield break;
        }

        for (int j = 0; j < _playerHandPositions.Length; j++)
        {
            if (!_deck.IsEmpty && _playerHandPositions[j].transform.childCount == 0)
            {
                // frontend- get top card of deck, child it to a hand position, then move the card to the hand position
                GameObject cardDrawn = _deckList[_deck.LastIndex];
                _playerHandList[j] = cardDrawn;

                cardDrawn.transform.parent = _playerHandPositions[j].transform;

                CardMovement cardMovement = cardDrawn.GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.TargetTransform = _playerHandPositions[j].transform;
                }

                _deckList.Remove(cardDrawn);

                yield return new WaitForSeconds(0.2f);

                // backend
                Card newCard = _deck.Draw(DeckPosition.Top);
                _playerHand.LazyAdd(newCard, j);
            }
        }             
    }

    // SelectManager calls this function based on card slot picked
    public void SelectCard(int index)
    {
        selectedCardIndex = index;

        RemoveCard();
    }

    public void RemoveCard()
    {
        // front end- remove selected card
        GameObject selectedCard = _playerHandList[selectedCardIndex];
        selectedCard.transform.parent = _discardPosition;

        CardMovement cardMovement = selectedCard.GetComponent<CardMovement>();
        if (cardMovement != null)
        {
            cardMovement.TargetTransform = _discardPosition;
        }

        _playerHandList[selectedCardIndex] = null;
        _discardList.Add(selectedCard);
        Debug.Log(_discardList[_discardList.Count - 1].GetComponent<ElementCardView>().Name);

        // back end- remove selected cards
        Card card = _playerHand.GetCard(selectedCardIndex);
        _discard.Add(card, DeckPosition.Top);
        _playerHand.LazyRemove(selectedCardIndex);
    }
}
