using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetupBattleState : BattleState
{
    [SerializeField] BattleManager _battleManager;

    [SerializeField] Image backgroundImage;
    [SerializeField] Sprite[] battleSceneryImages;
    [SerializeField] Image[] uiImages;
    [SerializeField] Color[] uiColors;

    bool _activated = false;

    EnemyTurnBattleState _enemyTurnBattleState;

    BGM bgm;
    [SerializeField] AudioClip[] bgms;
    AudioClip clip;

    public override void Enter()
    {
        _activated = false;
        bgm = FindObjectOfType<BGM>();

        // CANT change state while still in Enter()/Exit() transition
        // DONT put ChangeState<> here

        _enemyTurnBattleState = GetComponent<EnemyTurnBattleState>();
        _enemyTurnBattleState.CreateEnemyDeck();
        _enemyTurnBattleState.ShuffleEnemyDeck();

        SetupBattle();
        SetupBGM();
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
        for (int i = 0; i < GameManager._instance.Deck.Count; i++)
        {
            // back end
            _battleManager.BattleDeck.Add(GameManager._instance.Deck.GetCard(i));
        }
    }

    // set up player hand deck for back end use
    void LazySetupPlayerHand()
    {
        for (int i = 0; i < 5; i++)
        {
            ElementCard card = new ElementCard(GameManager._instance.DeckConfig[0]);
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
        for (int i = 0; i < GameManager._instance.Deck.Count; i++)
        {
            ElementCardView c = _battleManager.DeckList[i].GetComponent<ElementCardView>();
            ElementCard newCard = (ElementCard)_battleManager.BattleDeck.GetCard(i);
            c.Display(newCard);
        }
    }

    void SetupBattleScenery()
    {
        if (GameManager._instance.CurrentLevel >= 6)
        {
            backgroundImage.sprite = battleSceneryImages[2];
            uiImages[0].color = uiColors[2];
            uiImages[1].color = uiColors[2];
        }
        else if (GameManager._instance.CurrentLevel >= 3)
        {
            backgroundImage.sprite = battleSceneryImages[1];
            uiImages[0].color = uiColors[1];
            uiImages[1].color = uiColors[1];
        }
        else if (GameManager._instance.CurrentLevel >= 0)
        {
            backgroundImage.sprite = battleSceneryImages[0];
            uiImages[0].color = uiColors[0];
            uiImages[1].color = uiColors[0];
        }
    }

    void SetupBGM()
    {
        if (GameManager._instance.CurrentLevel >= 6)
        {
            clip = bgms[2];
        }
        else if (GameManager._instance.CurrentLevel >= 3)
        {
            clip = bgms[1];
        }
        else 
        {
            clip = bgms[0];
        }

        bgm.StartFadeIn(clip);
    }
}
