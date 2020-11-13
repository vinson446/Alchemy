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

    [SerializeField] int _levelsUnlocked = 0;
    public int LevelsUnlocked { get => _levelsUnlocked; set => _levelsUnlocked = value; }

    [SerializeField] int _currentLevel = 0;
    public int CurrentLevel { get => _currentLevel; set => _currentLevel = value; }

    [SerializeField] int _gold = 100;
    public int Gold { get => _gold; set => _gold = value; }

    public bool finishedTutorial = false;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (_deck.IsEmpty)
            CreateDeck();
    }

    public void CreateDeck()
    {
        foreach (ElementCardData elementData in _deckConfig)
        {
            ElementCard newElementCard = new ElementCard(elementData);

            _deck.Add(newElementCard);
        }
    }

    public void UnlockLevel()
    {
        if (_currentLevel == _levelsUnlocked)
        {
            _levelsUnlocked++;
        }
    }

    public void SelectLevel(int level)
    {
        _currentLevel = level;
    }

    public void IncrementGold(int gold)
    {
        _gold += gold;
    }

    public void SaveGame()
    {
        Save.SaveGame(this);
    }

    public void LoadGame()
    {
        SaveData data = Save.LoadGame();

        if (data != null)
        {
            print("load game");
            _levelsUnlocked = data.levelsUnlocked;
            _gold = data.gold;
            // _deck = data.deck;

            if (_deck.IsEmpty)
                CreateDeck();

            for (int i = 0; i < 30; i++)
            {
                ((ElementCard)_deck.GetCard(i)).SetLevel(data.level[i]);
                ((ElementCard)_deck.GetCard(i)).SetAttack(data.attack[i]);
                ((ElementCard)_deck.GetCard(i)).SetDefense(data.defense[i]);
            }

            MenuManager menuManager = FindObjectOfType<MenuManager>();
            menuManager.PlayGame();
        }
        else
        {
            print("null");
            MenuManager menuManager = FindObjectOfType<MenuManager>();
            menuManager.PlayGame();
        }
    }
}
