using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowDeckState : LevelSelectState
{
    [SerializeField] GameObject[] _allPanels;
    [SerializeField] GameObject _deckPanel;
    [SerializeField] ElementCardView[] cards;

    GameManager _gameManager;
    DeckSelectManager _deckSelectManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();    
    }

    public override void Enter()
    {
        _deckPanel.SetActive(true);

        ShowAllCardStats();

        _deckSelectManager = FindObjectOfType<DeckSelectManager>();
        _deckSelectManager.enabled = true;

        StateMachine.Input.PressedGoToLevelSelect += OnPressedLevelSelect;
        StateMachine.Input.PressedGoToMenu += OnPressedMenu;

        StateMachine.Input.PressedViewUpgrade += OnPressedViewUpgrade;
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {
        StateMachine.Input.PressedGoToLevelSelect -= OnPressedLevelSelect;
        StateMachine.Input.PressedGoToMenu -= OnPressedMenu;

        StateMachine.Input.PressedViewUpgrade -= OnPressedViewUpgrade;
    }

    void OnPressedLevelSelect()
    {
        _deckPanel.SetActive(false);

        _deckSelectManager.enabled = false;

        StateMachine.ChangeState<ShowLevelSelectState>();
    }

    void OnPressedViewUpgrade()
    {
        StateMachine.ChangeState<ShowUpgradeState>();
    }

    void OnPressedMenu()
    {
        _deckPanel.SetActive(false);

        _deckSelectManager.enabled = false;

        SceneManager.LoadScene("Menu");
    }

    void ShowAllCardStats()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            ElementCard newCard = (ElementCard)_gameManager.Deck.GetCard(i);
            cards[i].Display(newCard);
        }
    }
}
