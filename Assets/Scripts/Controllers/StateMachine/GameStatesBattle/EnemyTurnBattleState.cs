using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class EnemyTurnBattleState : BattleState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [SerializeField] float _pauseDuration = 1.5f;

    [Header("References")]
    [SerializeField] List<GameObject> _deckList;
    [SerializeField] ElementCardData[] _deckConfig;
    [SerializeField] Transform _battlePos;
    Deck<Card> _deck = new Deck<Card>();

    [Header("Animation Settings")]
    [SerializeField] float _normFactor;
    [SerializeField] float _shrinkFactor;
    [SerializeField] float _growthFactor;
    [SerializeField] float _duration;

    int _monsterIndex;
    GameObject _selectedMonster;
    public GameObject SelectedMonster => _selectedMonster;

    public override void Enter()
    {
        Debug.Log("Enemy Turn: Enter");

        EnemyTurnBegan?.Invoke();

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    public override void Exit()
    {
        Debug.Log("Enemy Turn: Exit");
    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        PickAMonsterFromDeck();

        yield return new WaitForSeconds(pauseDuration);

        SummonMonster();

        EnemyTurnEnded?.Invoke();

        yield return new WaitForSeconds(pauseDuration);

        StateMachine.ChangeState<FightBattleState>();
    }

    // called by setup battle state
    public void CreateEnemyDeck()
    {
        for (int i = 0; i < _deckConfig.Length; i++)
        {
            ElementCard elementCard = new ElementCard(_deckConfig[i]);
            _deck.Add(elementCard);

            ElementCardView cardView = _deckList[i].GetComponent<ElementCardView>();
            if (cardView != null)
                cardView.Display(elementCard);
        }
    }

    // called by setup battle state
    public void ShuffleEnemyDeck()
    {
        _deck.Shuffle(_deckList);
    }

    void PickAMonsterFromDeck()
    {
        _monsterIndex = UnityEngine.Random.Range(0, _deckList.Count);
        _selectedMonster = _deckList[_monsterIndex];

        print(_deck.GetCard(_monsterIndex).Name);
    }

    void SummonMonster()
    {
        CardMovement cardMovement = _selectedMonster.GetComponent<CardMovement>();
        if (cardMovement != null)
        {
            cardMovement.TargetTransform = _battlePos;
        }

        cardMovement.gameObject.transform.DOScale(_growthFactor, _duration);
    }
}
