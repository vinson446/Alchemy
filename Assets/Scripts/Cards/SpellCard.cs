using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : Card
{
    public int AttackBuff { get; private set; }
    public int DefenseBuff { get; private set; }

    public SpellCard(SpellCardData Data)
    {
        // Card variables
        Name = Data.Name;
        Level = Data.Level;
        Description = Data.Description;
        AttackBuff = Data.AttackBuff;
        DefenseBuff = Data.DefenseBuff;

        MonsterSprite = Data.Sprite;
        SpriteBackground = Data.SpriteBackground;

        CardEffect = Data.PlayEffect;
    }

    public override void Play()
    {
        /*
        ITargetable target = TargetController.CurrentTarget;

        Debug.Log("Playing " + Name + " on target.");
        PlayEffect.Activate(target);
        */
    }
}
