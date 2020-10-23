using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _enemyThinkingTextUI = null;

    private void OnEnable()
    {
        EnemyTurnBattleState.EnemyTurnBegan += OnEnemyTurnBegan;
        EnemyTurnBattleState.EnemyTurnEnded += OnEnemyTurnEnded;
    }

    private void OnDisable()
    {
        EnemyTurnBattleState.EnemyTurnBegan -= OnEnemyTurnBegan;
        EnemyTurnBattleState.EnemyTurnEnded -= OnEnemyTurnEnded;
    }

    private void Start()
    {

    }

    void OnEnemyTurnBegan()
    {

    }

    void OnEnemyTurnEnded()
    {

    }
}
