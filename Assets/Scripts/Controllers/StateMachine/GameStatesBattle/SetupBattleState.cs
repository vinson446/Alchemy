using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetupBattleState : BattleState
{
    [SerializeField] BattleManager _battleManager;
    GameManager _gameManager;

    [SerializeField] Image backgroundImage;
    [SerializeField] Sprite[] battleSceneryImages;

    bool _activated = false;

    EnemyTurnBattleState _enemyTurnBattleState;

    public override void Enter()
    {
        _activated = false;

        // CANT change state while still in Enter()/Exit() transition
        // DONT put ChangeState<> here
        _gameManager = FindObjectOfType<GameManager>();

        _enemyTurnBattleState = GetComponent<EnemyTurnBattleState>();
        _enemyTurnBattleState.CreateEnemyDeck();
        _enemyTurnBattleState.ShuffleEnemyDeck();

        SetupBattle();
        SetupBattleScenery();

        StateMachine.Input.PressedGoToLevelSelect += OnPressedLevelSelect;
    }

    // happens as a transition to player turn battle state
    public override void Tick()
    {
        if (_activated == false)
        {
            _activated = true;
            StateMachine.ChangeState<EnemyTurnBattleState>();
        }
    }

    public override void Exit()
    {
        _activated = false;
    }

    public void OnPressedLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
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

    void SetupBattleScenery()
    {
        if (_gameManager.CurrentLevel >= 6)
        {
            backgroundImage.sprite = battleSceneryImages[2];
        }
        else if (_gameManager.CurrentLevel >= 3)
        {
            backgroundImage.sprite = battleSceneryImages[1];
        }
        else if (_gameManager.CurrentLevel >= 0)
        {
            backgroundImage.sprite = battleSceneryImages[0];
        }
    }
}
