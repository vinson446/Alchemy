using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject _elementCard;
    // [SerializeField] GameObject _spellCard;

    [Header("Life Points")]
    [SerializeField] int _playerHP;
    public int PlayerHP => _playerHP;
    [SerializeField] int _enemyHP;
    public int EnemyHP => _enemyHP;
    [SerializeField] TextMeshProUGUI _playerHPText;
    [SerializeField] TextMeshProUGUI _enemyHPText;

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

    GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        SetupEnemyHP();
        UpdateBothHP(0, 0);    
    }

    public void SetupEnemyHP()
    {
        _enemyHP = _playerHP + _gameManager.LevelsUnlocked * 500;
        _enemyHPText.text = _enemyHP.ToString();
    }

    public void UpdateBothHP(int p, int e)
    {
        if (p > 0)
            _playerHP -= p;
        if (e > 0)
            _enemyHP -= e;

        _playerHPText.text = _playerHP.ToString();
        _enemyHPText.text = _enemyHP.ToString();

        // win battle
        if (_enemyHP <= 0)
        {
            WinBattle();
        }
        // lose battle
        else if (_playerHP <= 0)
        {
            LoseBattle();
        }
    }

    void WinBattle()
    {

    }

    void LoseBattle()
    {

    }
}
