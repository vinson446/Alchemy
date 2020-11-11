using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;

public class EnemyTurnBattleState : BattleState
{
    public static event Action EnemyTurnBegan;
    public static event Action EnemyTurnEnded;

    [Header("Battle Stuff")]
    [SerializeField] float _pauseDuration = 1.5f;

    [Header("References")]
    [SerializeField] List<GameObject> _deckList;
    [SerializeField] ElementCardData[] _deckConfig1;
    [SerializeField] ElementCardData[] _deckConfig2;
    [SerializeField] ElementCardData[] _deckConfig3;
    [SerializeField] Transform _initalPos;
    [SerializeField] Transform _battlePosStandby;
    [SerializeField] TextMeshProUGUI _enemyTurnText;
    Deck<Card> _deck = new Deck<Card>();

    [Header("Animation Settings")]
    [SerializeField] float _normFactor;
    [SerializeField] float _shrinkFactor;
    [SerializeField] float _growthFactor;
    [SerializeField] float _duration;

    int _monsterIndex;
    int _prevMonsterIndex;
    GameObject _selectedMonster;
    public GameObject SelectedMonster => _selectedMonster;

    GameManager gameManager;

    public override void Enter()
    {
        EnemyTurnBegan?.Invoke();

        StartCoroutine(EnemyThinkingRoutine(_pauseDuration));
    }

    public override void Exit()
    {

    }

    IEnumerator EnemyThinkingRoutine(float pauseDuration)
    {
        StartCoroutine(ShowEnemyTurnText());
        PickAMonsterFromDeck();
        SummonMonster();

        EnemyTurnEnded?.Invoke();

        yield return new WaitForSeconds(pauseDuration);

        StateMachine.ChangeState<PlayerDrawBattleState>();
    }

    IEnumerator ShowEnemyTurnText()
    {
        _enemyTurnText.text = "Enemy Turn";
        _enemyTurnText.gameObject.SetActive(true);
        _enemyTurnText.DOFade(1, 0.25f);

        yield return new WaitForSeconds(1);

        _enemyTurnText.DOFade(0, 0.25f);

        yield return new WaitForSeconds(0.25f);

        _enemyTurnText.gameObject.SetActive(false);
    }

    // called by setup battle state
    public void CreateEnemyDeck()
    {
        gameManager = FindObjectOfType<GameManager>();

        for (int i = 0; i < _deckConfig1.Length; i++)
        {
            if (gameManager.CurrentLevel >= 6)
            {
                ElementCard elementCard = new ElementCard(_deckConfig3[i]);
                _deck.Add(elementCard);

                ElementCardView cardView = _deckList[i].GetComponent<ElementCardView>();
                if (cardView != null)
                {
                    cardView.EnemyBattleUpgrade();
                    cardView.Display(elementCard);
                }
            }
            else if (gameManager.CurrentLevel >= 3)
            {
                ElementCard elementCard = new ElementCard(_deckConfig2[i]);
                _deck.Add(elementCard);

                ElementCardView cardView = _deckList[i].GetComponent<ElementCardView>();
                if (cardView != null)
                {
                    cardView.EnemyBattleUpgrade();
                    cardView.Display(elementCard);
                }
            }
            else
            {
                ElementCard elementCard = new ElementCard(_deckConfig1[i]);
                _deck.Add(elementCard);

                ElementCardView cardView = _deckList[i].GetComponent<ElementCardView>();
                if (cardView != null)
                {
                    cardView.EnemyBattleUpgrade();
                    cardView.Display(elementCard);
                }
            }
        }
    }

    // called by setup battle state
    public void ShuffleEnemyDeck()
    {
        _deck.Shuffle(_deckList);
    }

    void PickAMonsterFromDeck()
    {
        _prevMonsterIndex = _monsterIndex;

        while (_monsterIndex == _prevMonsterIndex)
        {
            _monsterIndex = UnityEngine.Random.Range(0, _deckList.Count);
        }

        _selectedMonster = _deckList[_monsterIndex];
    }

    void SummonMonster()
    {
        CardMovement cardMovement = _selectedMonster.GetComponent<CardMovement>();
        if (cardMovement != null)
        {
            cardMovement.TargetTransform = _battlePosStandby;
        }
        cardMovement.gameObject.SetActive(true);

        // cardMovement.gameObject.transform.DOScale(_growthFactor, _duration);
    }

    public void ReturnMonsterFromDeck()
    {
        CardMovement cardMovement = _selectedMonster.GetComponent<CardMovement>();
        if (cardMovement != null)
        {
            cardMovement.TargetTransform = _initalPos;
        }
        cardMovement.gameObject.SetActive(true);
    }
}
