using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamagePlayEffect",
                 menuName = "CardData/PlayEffects/AtkToDef")]
public class AtkToDef : CardPlayEffect
{
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
        int defBoost = cardView.Attack;

        cardView.StatChange(0, cardView.Defense + defBoost, cardView.Level);
    }
}
