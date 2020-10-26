using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FightBattleState : BattleState
{
    [Header("Positioning Settings")]
    [SerializeField] Transform _battleCollidePos;

    [Header("Animation Settings")]
    [SerializeField] float _duration;
    [SerializeField] float _durationMoveOut;
    [SerializeField] float _durationMoveIn;

    PlayerTurnBattleState _playerTurnBattleState;
    GameObject _playerMonster;
    EnemyTurnBattleState _enemyTurnBattleState;
    GameObject _enemyMonster;

    BattleManager _battleManager;

    int _damageDoneToPlayer;
    int _damageDoneToEnemy;

    public override void Enter()
    {
        print("Start Battle");

        _playerTurnBattleState = GetComponent<PlayerTurnBattleState>();
        _enemyTurnBattleState = GetComponent<EnemyTurnBattleState>();

        _battleManager = FindObjectOfType<BattleManager>();

        SetVariablesForMonsters();

        StartCoroutine(AnimateBattleCoroutine());
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {
        print("End Battle");
    }

    void SetVariablesForMonsters()
    {
        _playerMonster = _playerTurnBattleState.SelectedMonster;
        _enemyMonster = _enemyTurnBattleState.SelectedMonster;
    }

    IEnumerator AnimateBattleCoroutine()
    {
        // animating fusion
        Sequence fusionSequence1 = DOTween.Sequence();
        Sequence fusionSequence2 = DOTween.Sequence();
        fusionSequence1.Append(_playerMonster.transform.DOMoveX
            (_battleManager.PlayerPlayingCardPositions[2].transform.position.x, _durationMoveOut));
        fusionSequence2.Append(_enemyMonster.transform.DOMoveX
            (_battleManager.PlayerPlayingCardPositions[3].transform.position.x, _durationMoveOut));

        fusionSequence1.Append(_playerMonster.transform.DOMoveX(_battleCollidePos.position.x, _durationMoveIn));
        fusionSequence2.Append(_enemyMonster.transform.DOMoveX(_battleCollidePos.position.x, _durationMoveIn));

        fusionSequence1.Play();
        fusionSequence2.Play();

        yield return new WaitForSeconds(_duration);

        BattleCalculations();
    }

    void BattleCalculations()
    {

    }

    void ApplyDamageForPlayer()
    {

    }

    void ApplyDamageForEnemy()
    {

    }
}
