using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamagePlayEffect",
                 menuName = "CardData/PlayEffects/Forge")]
public class Forge : CardPlayEffect
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

        // use higher stat out of atk and def / 2 to heal player
        ElementCardView cardView = _fightBattleState._playerMonster.GetComponent<ElementCardView>();
        int defBoost = cardView.Defense * 2;

        cardView.StatChange(0, defBoost, cardView.Level);

        // deal half of player monster's attack as direct damage
        pDamage = _battleManager.PlayerHP / 2;

        bool continueGame = _battleManager.UpdateBothHP(-pDamage, 0);
        _fightBattleState.ShowDamagePopup(-pDamage, 0);
    }
}
