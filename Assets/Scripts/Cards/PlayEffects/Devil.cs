using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamagePlayEffect",
                 menuName = "CardData/PlayEffects/Devil")]
public class Devil : CardPlayEffect
{
    int pDamage;
    // update UI
    BattleManager _battleManager;
    // get stats
    FightBattleState _fightBattleState;

    public override void Activate()
    {
        _fightBattleState = FindObjectOfType<FightBattleState>();
        _battleManager = FindObjectOfType<BattleManager>();

        pDamage = _battleManager.PlayerHP / 2;

        ElementCardView cardView = _fightBattleState._playerMonster.GetComponent<ElementCardView>();
        int atkBoost = cardView.Attack + pDamage;

        cardView.StatChange(atkBoost, cardView.Defense, cardView.Level);

        bool continueGame = _battleManager.UpdateBothHP(-pDamage, 0);
        _fightBattleState.ShowDamagePopup(-pDamage, 0);
    }
}
