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
        ShowAllCardsInDeck();

        SetupPlayerHand();

        ShuffleDeck();
    }

    // assemble battle deck with game manager's deck to use in battle
    void SetupBattleDeck()
    {
        for (int i = 0; i < _gameManager.Deck.Count; i++)
        {
            // front end
            _battleManager.DeckList[i].name = _gameManager.Deck.GetCard(i).Name;

            // back end
            _battleManager.BattleDeck.Add(_gameManager.Deck.GetCard(i));
        }
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

    // set up player hand deck for back end use
    void SetupPlayerHand()
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
        StartCoroutine(MoveCardsInDiscardToDeck());

        // back end
        _battleManager.BattleDeck.Shuffle(_battleManager.DeckList);
    }

    IEnumerator MoveCardsInDiscardToDeck()
    {
        int count = _battleManager.Discard.Count;
        for (int i = 0; i < count; i++)
        {
            // front end- add cards in discard to deck
            CardMovement cardMovement = _battleManager.DiscardList[0].GetComponent<CardMovement>();
            cardMovement.transform.parent = _battleManager.DeckPos;
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.DeckPos;
            }
            _battleManager.DeckList.Add(_battleManager.DiscardList[0]);
            _battleManager.DiscardList.Remove(_battleManager.DiscardList[0]);

            // back end- add cards in discard to deck
            _battleManager.BattleDeck.Add(_battleManager.Discard.GetCard(0));
            _battleManager.Discard.Remove(0);

            yield return new WaitForSeconds(0.2f);
        }
    }
}
