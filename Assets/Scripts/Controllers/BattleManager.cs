using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("References")]
    [SerializeField] GameObject winLosePanel;
    [SerializeField] TextMeshProUGUI winLoseText;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] int goldReward;

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
        _enemyHP = 1500 + (_gameManager.LevelsUnlocked) * 500;
        _enemyHPText.text = _enemyHP.ToString();
    }

    public bool UpdateBothHP(int p, int e)
    {
        _playerHP += p;
        _enemyHP += e;

        _playerHPText.text = _playerHP.ToString();
        _enemyHPText.text = _enemyHP.ToString();

        // lose battle
        if (_playerHP <= 0)
        {
            LoseBattle();
            BattleSM stateMachine = FindObjectOfType<BattleSM>();
            stateMachine.ChangeState<ExitBattleState>();
            return false;
        }
        // win battle
        else if (_enemyHP <= 0)
        {
            WinBattle();
            BattleSM stateMachine = FindObjectOfType<BattleSM>();
            stateMachine.ChangeState<ExitBattleState>();
            return false;
        }

        return true;
    }

    void WinBattle()
    {
        winLosePanel.SetActive(true);
        winLoseText.text = "Victory!";

        goldReward = 100;
        rewardText.text = "+" + goldReward.ToString() + " Gold";

        _gameManager.IncrementGold(goldReward);
        _gameManager.UnlockLevel();
    }

    void LoseBattle()
    {
        winLosePanel.SetActive(true);
        winLoseText.text = "Defeat...";

        goldReward = 0;
        rewardText.text = "";
    }

    public void ReturnToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
