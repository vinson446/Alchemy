using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamagePlayEffect",
                 menuName = "CardData/PlayEffects/DirectDamage")]
public class DirectDamage : CardPlayEffect
{
    int eDamage;

    BattleManager _battleManager;
    FightBattleState _fightBattleState;

    public override void Activate()
    {
        _fightBattleState = FindObjectOfType<FightBattleState>();
        _battleManager = FindObjectOfType<BattleManager>();

        // deal half of player monster's attack as direct damage
        eDamage = _fightBattleState._playerMonster.GetComponent<ElementCardView>().Attack / 2;

        bool continueGame = _battleManager.UpdateBothHP(0, -eDamage);
        _fightBattleState.ShowDamagePopup(0, -eDamage);
    }
}
