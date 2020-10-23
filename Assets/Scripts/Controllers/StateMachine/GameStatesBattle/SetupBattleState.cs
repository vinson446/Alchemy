using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupBattleState : BattleState
{
    [SerializeField] int _startingCardNumber = 30;
    [SerializeField] int _numberOfPlayers = 2;

    bool _activated = false;

    public override void Enter()
    {
        Debug.Log("Setup: Entering");

        // CANT change state while still in Enter()/Exit() transition
        // DONT put ChangeState<> here

        _activated = false;

        StateMachine.Input.PressedGoToMenu += OnPressedMenu;
    }

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
}
