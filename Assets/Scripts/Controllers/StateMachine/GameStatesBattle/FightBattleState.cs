using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class FightBattleState : BattleState
{
    [Header("Battle Stuff")]
    [SerializeField] GameObject _damageTextObj;

    [Header("Positioning Settings")]
    [SerializeField] Transform _playerBattlePos;
    [SerializeField] Transform _playerMoveOutPos;
    [SerializeField] Transform _enemyBattlePos;
    [SerializeField] Transform _enemyMoveOutPos;
    [SerializeField] Transform _battleCollidePos;

    [SerializeField] Transform _spawnPlayerDmgObj;
    [SerializeField] Transform _spawnEnemyDmgObj;

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

    }

    void SetVariablesForMonsters()
    {
        _playerMonster = _playerTurnBattleState.SelectedMonster;
        _enemyMonster = _enemyTurnBattleState.SelectedMonster;
    }

    IEnumerator AnimateBattleCoroutine()
    {
        // let tweening move the cards
        CardMovement playerCardMovement = _playerMonster.GetComponent<CardMovement>();
        playerCardMovement.enabled = false;
        CardMovement enemyCardMovement = _enemyMonster.GetComponent<CardMovement>();
        enemyCardMovement.enabled = false;

        // move to battle ready pos
        _playerMonster.transform.DOMoveX(_playerBattlePos.transform.position.x, _duration);
        _enemyMonster.transform.DOMoveX(_enemyBattlePos.transform.position.x, _duration);

        yield return new WaitForSeconds(0.75f);

        // move out
        _playerMonster.transform.DOMoveX(_playerMoveOutPos.transform.position.x, _durationMoveOut * 2);
        _enemyMonster.transform.DOMoveX(_enemyMoveOutPos.transform.position.x, _durationMoveOut * 2);

        yield return new WaitForSeconds(0.5f + _durationMoveOut * 2);

        // COLLIDE
        _playerMonster.transform.DOMoveX(_battleCollidePos.position.x, _durationMoveIn);
        _enemyMonster.transform.DOMoveX(_battleCollidePos.position.x, _durationMoveIn);

        yield return new WaitForSeconds(_durationMoveIn);

        // move back to battle ready pos
        _playerMonster.transform.DOMoveX(_playerBattlePos.position.x, _durationMoveOut);
        _enemyMonster.transform.DOMoveX(_enemyBattlePos.position.x, _durationMoveOut);

        yield return new WaitForSeconds(0.5f + _duration * 2);

        BattleCalculations();

        playerCardMovement.enabled = true;
        enemyCardMovement.enabled = true;
    }

    void BattleCalculations()
    {
        ElementCardView playerView = _playerMonster.GetComponent<ElementCardView>();
        ElementCardView enemyView = _enemyMonster.GetComponent<ElementCardView>();

        int playerAttack = playerView.Attack;
        int enemyDefense = enemyView.Defense;
        int damageToEnemy = playerAttack - enemyDefense;

        int enemyAttack = enemyView.Attack;
        int playerDefense = playerView.Defense;
        int damageToPlayer = enemyAttack - playerDefense;

        _battleManager.UpdateBothHP(damageToPlayer, damageToEnemy);
        ShowDamagePopup(damageToPlayer, damageToEnemy);

        ResetEverythingForNextTurn();
    }

    void ShowDamagePopup(int pDamage, int eDamage)
    {
        GameObject playerDamageObj = Instantiate(_damageTextObj, _spawnPlayerDmgObj.position, Quaternion.identity);
        playerDamageObj.GetComponent<DamagePopup>().Setup(pDamage);
        playerDamageObj.transform.parent = _spawnPlayerDmgObj;

        GameObject enemyDamageObj = Instantiate(_damageTextObj, _spawnEnemyDmgObj.position, Quaternion.identity);
        enemyDamageObj.GetComponent<DamagePopup>().Setup(eDamage);
        enemyDamageObj.transform.parent = _spawnEnemyDmgObj;
    }

    void ResetEverythingForNextTurn()
    {
        // reset player stuff
        _playerMonster.SetActive(false);
        _playerTurnBattleState.RemoveSelectedCards();

        // reset enemy stuff
        _enemyMonster.SetActive(false);
        _enemyTurnBattleState.ReturnMonsterFromDeck();

        StateMachine.ChangeState<EnemyTurnBattleState>();
    }
}
