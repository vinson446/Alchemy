using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowDeckState : LevelSelectState
{
    [SerializeField] GameObject[] _allPanels;
    [SerializeField] GameObject _deckPanel;

    GameManager _gameManager;
    DeckSelectManager _deckSelectManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();    
    }

    public override void Enter()
    {
        _deckPanel.SetActive(true);
        EnableDisableDeckPanelButtons(true);

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

    public void OnPressedLevelSelect()
    {
        _deckPanel.SetActive(false);

        _deckSelectManager.enabled = false;

        StateMachine.ChangeState<ShowLevelSelectState>();
    }

    public void OnPressedViewUpgrade()
    {
        EnableDisableDeckPanelButtons(false);

        StateMachine.ChangeState<ShowUpgradeState>();
    }

    public void OnPressedMenu()
    {
        _deckPanel.SetActive(false);

        _deckSelectManager.enabled = false;

        SceneManager.LoadScene("Menu");
    }

    void EnableDisableDeckPanelButtons(bool b)
    {
        Button[] buttonsInDeckPanel = _deckPanel.GetComponentsInChildren<Button>();
        foreach (Button button in buttonsInDeckPanel)
        {
            button.enabled = b;
        }
    }

    void ShowAllCardStats()
    {
        int index = 0;

        ElementCardView[] cards = _deckPanel.GetComponentsInChildren<ElementCardView>();
        foreach (ElementCardView c in cards)
        {
            ElementCard newCard = (ElementCard)_gameManager.Deck.GetCard(index);
            c.Display(newCard);

            index++;
        }
    }
}
