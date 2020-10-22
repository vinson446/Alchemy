using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTurnCardGameState : CardGameState
{
    [SerializeField] TextMeshProUGUI _playerTurnTextUI = null;
    int _playerTurnCount = 0;

    public override void Enter()
    {
        Debug.Log("Player Turn: Entering");

        _playerTurnCount++;
        Debug.Log("Player Turn: " + _playerTurnCount);

        StateMachine.Input.PressedConfirm += OnPressedConfirm;
    }

    public override void Exit()
    {
        Debug.Log("Player Turn: Exiting");

        StateMachine.Input.PressedConfirm += OnPressedConfirm;
    }

    void OnPressedConfirm()
    {
        StateMachine.ChangeState<EnemyTurnCardGameState>();
    }
}
