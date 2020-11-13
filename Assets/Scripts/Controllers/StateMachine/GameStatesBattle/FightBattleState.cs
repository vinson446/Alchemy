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
    [SerializeField] Transform _spawnPlayerMessageObj;
    [SerializeField] Transform _spawnEnemyDmgObj;
    [SerializeField] Transform _spawnEnemyMessageObj;

    [Header("Animation Settings")]
    [SerializeField] float _duration;
    [SerializeField] float _durationMoveOut;
    [SerializeField] float _durationMoveIn;

    [Header("VFX")]
    [SerializeField] ParticleSystem collideVFX;

    PlayerTurnBattleState _playerTurnBattleState;
    public GameObject _playerMonster { get; private set; }
    EnemyTurnBattleState _enemyTurnBattleState;
    public GameObject _enemyMonster { get; private set; }

    BattleManager _battleManager;

    int _damageDoneToPlayer;
    int _damageDoneToEnemy;

    SoundEffects soundEffects;

    public override void Enter()
    {
        soundEffects = FindObjectOfType<SoundEffects>();

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
        bool continueBattle = true;

        // let tweening move the cards
        CardMovement playerCardMovement = _playerMonster.GetComponent<CardMovement>();
        playerCardMovement.enabled = false;
        CardMovement enemyCardMovement = _enemyMonster.GetComponent<CardMovement>();
        enemyCardMovement.enabled = false;

        // move to battle ready pos
        _playerMonster.transform.DOMoveX(_playerBattlePos.transform.position.x, _duration);
        _enemyMonster.transform.DOMoveX(_enemyBattlePos.transform.position.x, _duration);

        yield return new WaitForSeconds(_duration);

        if (_playerMonster.tag == "FusionMonster")
        {
            // activate player play effect
            continueBattle = ActivatePlayerPlayEffect();
            yield return new WaitForSeconds(1f);
        }
        if (continueBattle)
        {
            // move out
            _playerMonster.transform.DOMoveX(_playerMoveOutPos.transform.position.x, _durationMoveIn * 1.5f);
            _enemyMonster.transform.DOMoveX(_enemyMoveOutPos.transform.position.x, _durationMoveIn * 1.5f);

            yield return new WaitForSeconds(_durationMoveIn * 1.5f);

            // COLLIDE
            _playerMonster.transform.DOMoveX(_battleCollidePos.position.x, _durationMoveIn);
            _enemyMonster.transform.DOMoveX(_battleCollidePos.position.x, _durationMoveIn);

            yield return new WaitForSeconds(_durationMoveIn);

            collideVFX.Play();
            soundEffects.PlayCollideSound();

            // move back to battle ready pos
            _playerMonster.transform.DOMoveX(_playerBattlePos.position.x, _durationMoveIn);
            _enemyMonster.transform.DOMoveX(_enemyBattlePos.position.x, _durationMoveIn);

            yield return new WaitForSeconds(_durationMoveIn * 2);

            BattleCalculations();

            playerCardMovement.enabled = true;
            enemyCardMovement.enabled = true;
        }
        else
        {
            playerCardMovement.enabled = true;
            enemyCardMovement.enabled = true;

            ResetEverythingForNextTurn();
        }
    }

    void BattleCalculations()
    {
        ElementCardView playerView = _playerMonster.GetComponent<ElementCardView>();
        ElementCardView enemyView = _enemyMonster.GetComponent<ElementCardView>();

        int playerAttack = playerView.Attack;
        int enemyDefense = enemyView.Defense;
        int damageToEnemy = playerAttack - enemyDefense;
        if (damageToEnemy < 0)
            damageToEnemy = 0;

        int enemyAttack = enemyView.Attack;
        int playerDefense = playerView.Defense;
        int damageToPlayer = enemyAttack - playerDefense;
        if (damageToPlayer < 0)
            damageToPlayer = 0;

        bool continueGame = _battleManager.UpdateBothHP(-damageToPlayer, -damageToEnemy);
        ShowDamagePopup(-damageToPlayer, -damageToEnemy);

        if (continueGame)
            ResetEverythingForNextTurn();
    }

    public void ShowDamagePopup(int pDamage, int eDamage)
    {
        GameObject playerDamageObj = Instantiate(_damageTextObj, _spawnPlayerDmgObj.position, Quaternion.identity);
        playerDamageObj.GetComponent<DamagePopup>().SetupDamage(pDamage, 1f);
        playerDamageObj.transform.SetParent(_spawnPlayerDmgObj);
        if (pDamage < 0)
        {
            soundEffects.PlayPlayerTakeDamageSound();
        }
        else if (pDamage > 0)
        {
            soundEffects.PlayPlayerHealSound();
        }

        GameObject enemyDamageObj = Instantiate(_damageTextObj, _spawnEnemyDmgObj.position, Quaternion.identity);
        enemyDamageObj.GetComponent<DamagePopup>().SetupDamage(eDamage, 1f);
        enemyDamageObj.transform.SetParent(_spawnEnemyDmgObj);
        if (eDamage < 0)
        {
            soundEffects.PlayEnemyTakeDamageSound();
        }

    }

    bool ActivatePlayerPlayEffect()
    {
        GameObject playerDamageObj = Instantiate(_damageTextObj, _spawnPlayerMessageObj.position, Quaternion.identity);

        ElementCardView cardView = _playerMonster.GetComponent<ElementCardView>();
        //playerDamageObj.GetComponent<DamagePopup>().SetupMessage("Activate " + cardView.Name + "\nPlay Effect", 1f);
        playerDamageObj.transform.SetParent(_spawnPlayerMessageObj);

        cardView.PlayCardEffect();

        if (cardView.Name == "Ghost")
            return false;
        else
            return true;
    }

    /*
    void ActivateEnemyPlayEffect()
    {
        GameObject enemyDamageObj = Instantiate(_damageTextObj, _spawnEnemyMessageObj.position, Quaternion.identity);
        enemyDamageObj.GetComponent<DamagePopup>().SetupMessage("Activate Play Effect", 1f);
        enemyDamageObj.transform.parent = _spawnEnemyDmgObj;
    }
    */

    void ResetEverythingForNextTurn()
    {
        // reset player stuff
        _playerMonster.SetActive(false);
        _playerTurnBattleState.RemoveSelectedCards();

        // reset enemy stuff
        _enemyMonster.SetActive(false);
        _enemyTurnBattleState.ReturnMonsterFromDeck();
        _enemyMonster.GetComponent<ElementCardView>().RevertStatChange();

        StateMachine.ChangeState<EnemyTurnBattleState>();
    }
}
