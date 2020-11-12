using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamagePlayEffect",
                 menuName = "CardData/PlayEffects/LevelDown")]
public class LevelDown : CardPlayEffect
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
        ElementCardView cardView = _fightBattleState._enemyMonster.GetComponent<ElementCardView>();

        cardView.StatChange(cardView.Attack - cardView.Level * 500, cardView.Defense - cardView.Level * 500, cardView.Level);
    }
}
