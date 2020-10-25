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
    public List<GameObject> DeckList => _deckList;

    [SerializeField] List<GameObject> _discardList;
    public List<GameObject> DiscardList => _discardList;

    [SerializeField] List<GameObject> _playerHandList;
    public List<GameObject> PlayerHandList => _playerHandList;

    [Header("Frontend- Positions")]
    [SerializeField] Transform _deckPosition;
    public Transform DeckPos => _deckPosition;

    [SerializeField] Transform[] _playerHandPositions;
    public Transform[] PlayerHandPositions => _playerHandPositions;

    [SerializeField] Transform[] _playerPlayingCardPositions;
    public Transform[] PlayerPlayingCardPositions => _playerPlayingCardPositions;

    [SerializeField] Transform[] _playerShrinkPositions;
    public Transform[] PlayerShrinkPositions => _playerShrinkPositions;

    [SerializeField] Transform _discardPosition;
    public Transform DiscardPosition => _discardPosition;

    // backend
    Deck<Card> _battleDeck = new Deck<Card>();
    public Deck<Card> BattleDeck => _battleDeck;

    Deck<Card> _discard = new Deck<Card>();
    public Deck<Card> Discard => _discard;

    Deck<Card> _playerHand = new Deck<Card>();
    public Deck<Card> PlayerHand => _playerHand;
}
