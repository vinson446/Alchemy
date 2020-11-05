using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamagePlayEffect",
                 menuName = "CardData/PlayEffects/Heal")]
public class Heal : CardPlayEffect
{
    int heal;

    // update UI
    BattleManager _battleManager;
    // get stats
    FightBattleState _fightBattleState;

    public override void Activate()
    {
        _fightBattleState = FindObjectOfType<FightBattleState>();
        _battleManager = FindObjectOfType<BattleManager>();

        // use higher stat out of atk and def / 2 to heal player
        ElementCardView cardView = _fightBattleState._playerMonster.GetComponent<ElementCardView>();
        int attack = cardView.Attack;
        int defense = cardView.Defense;
        if (attack >= defense)
        {
            heal = attack / 2;
            bool continueGame = _battleManager.UpdateBothHP(heal, 0);
            _fightBattleState.ShowDamagePopup(heal, 0);
        }
        else
        {
            heal = defense / 2;
            bool continueGame = _battleManager.UpdateBothHP(heal, 0);
            _fightBattleState.ShowDamagePopup(heal, 0);
        }
    }
}
