using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupBattleState : BattleState
{
    [SerializeField] BattleManager _battleManager;
    GameManager _gameManager;

    bool _activated = false;

    public override void Enter()
    {
        Debug.Log("Setup: Entering");
        _activated = false;

        // CANT change state while still in Enter()/Exit() transition
        // DONT put ChangeState<> here
        _gameManager = FindObjectOfType<GameManager>();

        SetupBattle();

        StateMachine.Input.PressedGoToMenu += OnPressedMenu;
    }

    // happens as a transition to player turn battle state
    public override void Tick()
    {
        if (_activated == false)
        {
            _activated = true;
            StateMachine.ChangeState<PlayerTurnBattleState>();
        }
    }

    public override void Exit()
    {
        Debug.Log("Setup: Exiting");
        _activated = false;
    }

    public void OnPressedMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void SetupBattle()
    {
        SetupBattleDeck();

        LazySetupPlayerHand();

        ShuffleDeck();
        ShowAllCardsInDeck();
    }

    // assemble battle deck with game manager's deck to use in battle
    void SetupBattleDeck()
    {
        for (int i = 0; i < _gameManager.Deck.Count; i++)
        {
            // back end
            _battleManager.BattleDeck.Add(_gameManager.Deck.GetCard(i));
        }
    }

    // set up player hand deck for back end use
    void LazySetupPlayerHand()
    {
        for (int i = 0; i < 5; i++)
        {
            ElementCard card = new ElementCard(_gameManager.DeckConfig[0]);
            _battleManager.PlayerHand.Add(card, DeckPosition.Top);
        }
    }

    void ShuffleDeck()
    {
        // front end
        // TODO- cool shuffle animation

        // back end
        _battleManager.BattleDeck.Shuffle(_battleManager.DeckList);
    }

    // display cards visually in battle deck
    void ShowAllCardsInDeck()
    {
        for (int i = 0; i < _gameManager.Deck.Count; i++)
        {
            ElementCardView c = _battleManager.DeckList[i].GetComponent<ElementCardView>();
            ElementCard newCard = (ElementCard)_battleManager.BattleDeck.GetCard(i);
            c.Display(newCard);
        }
    }
}
