using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        SetupElementDeck();
        SetupSpellDeck();

        ShuffleDeck();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Draw());
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Card")
                {
                    // front end
                    GameObject cardObj = hit.transform.gameObject;
                    int index = int.Parse(cardObj.transform.parent.name);

                    _playerHandList.RemoveAt(index);
                    

                    // backend
                    _playerHand.Remove(index);
                }
            }
        }
    }

    public void ChangeBattleState(int index)
    {
        _battleState = (BattleState)index;
    }

    void ShuffleDeck()
    {
        // frontend
        // TODO- cool shuffle animation
        MoveCardsInDiscardToDeck();

        _deck.Shuffle();
    }

    void MoveCardsInDiscardToDeck()
    {
        if (!_discard.IsEmpty)
        {
            for (int i = 0; i < _discard.Count; i++)
            {
                // frontend- move any card objs in discard list position to deck list position
                CardMovement cardMovement = _discardList[i].GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.TargetTransform = _deckPosition;
                }

                // backend- remove card from discard 
                _discard.Remove(i);
            }
        }
    }

    private void SetupElementDeck()
    {
        foreach (ElementCardData elementData in _elementDeckConfig)
        {
            // frontend- create element card obj at deck position
            GameObject newElementCardObj = Instantiate(_elementCard, _deckPosition.position, Quaternion.Euler(new Vector3(0, -180, 0)));
            newElementCardObj.transform.parent = _deckPosition;
            _deckList.Add(newElementCardObj);

            // backend- create element card and add to deck list
            ElementCard newElementCard = new ElementCard(elementData);
            _deck.Add(newElementCard);
        }
    }

    private void SetupSpellDeck()
    {
        foreach (SpellCardData spellData in _spellDeckConfig)
        {
            // frontend- create spell card obj at deck position
            GameObject newSpellCardObj = Instantiate(_spellCard, _deckPosition.position, Quaternion.Euler(new Vector3(0, -180, 0)));
            newSpellCardObj.transform.parent = _deckPosition;
            _deckList.Add(newSpellCardObj);

            // backend- create spell card and add to deck list
            SpellCard newSpellCard = new SpellCard(spellData);
            _deck.Add(newSpellCard);
        }
    }

    IEnumerator Draw()
    {
        ChangeBattleState(0);

        if (!_deck.IsEmpty)
        { 
            for (int j = 0; j < _playerHandPositions.Length; j++)
            {
                if (_playerHandPositions[j].transform.childCount == 0)
                {
                    // frontend
                    GameObject cardDrawn = _deckList[_deck.LastIndex];
                    _playerHandList.Add(cardDrawn);

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
                    _playerHand.Add(newCard, DeckPosition.Top);
                }
            }          
        }
        // reshuffle deck
        else
        {
            ShuffleDeck();
        }
    }
}
