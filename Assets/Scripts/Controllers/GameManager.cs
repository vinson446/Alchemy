using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [SerializeField] List<ElementCardData> _deckConfig;
    public List<ElementCardData> DeckConfig => _deckConfig;

    // back end
    [SerializeField] Deck<Card> _deck = new Deck<Card>();
    public Deck<Card> Deck { get => _deck; set => _deck = value; }

    [SerializeField] int _levelsUnlocked = 1;
    public int LevelsUnlocked => _levelsUnlocked;

    [SerializeField] int _currentLevel = 1;
    public int CurrentLevel => _currentLevel;

    [SerializeField] int _gold = 100;
    public int Gold { get => _gold; set => _gold = value; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        CreateDeck();
    }

    void CreateDeck()
    {
        foreach (ElementCardData elementData in _deckConfig)
        {
            ElementCard newElementCard = new ElementCard(elementData);

            _deck.Add(newElementCard);
        }
    }

    public void UnlockLevel()
    {
        _levelsUnlocked++;
    }

    public void SelectLevel(int level)
    {
        _currentLevel = level;
    }

    public void IncrementGold(int gold)
    {
        _gold += gold;
    }
}
