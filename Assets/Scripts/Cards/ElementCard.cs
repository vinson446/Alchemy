using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCard : Card
{
    public int Attack { get; private set; }
    public int Defense { get; private set; }

    public ElementCard(ElementCardData Data)
    {
        // Card variables
        Name = Data.Name;
        Level = Data.Level;
        Description = Data.Description;
        Attack = Data.Attack;
        Defense = Data.Defense;

        Sprite = Data.Sprite;
        SpriteBackground = Data.SpriteBackground;
        CardBackground = Data.CardBackground;

        PlayEffect = Data.PlayEffect;
    }
    
    public override void Play()
    {
        ITargetable target = TargetController.CurrentTarget;

        Debug.Log("Playing " + Name + " on target.");
        PlayEffect.Activate(target);
    }
}
