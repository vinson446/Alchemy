using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCard : Card
{
    public int Attack { get; private set; }
    public int Defense { get; private set; }

    // parallel arrays
    public ElementCardData[] FusionCombinations { get; private set; }
    public ElementCardData[] FusionMonsters { get; private set; }

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

        FusionCombinations = Data.FusionCombinations;
        FusionMonsters = Data.FusionMonsters;

        PlayEffect = Data.PlayEffect;
    }
    
    public override void Play()
    {
        /*
        ITargetable target = TargetController.CurrentTarget;

        Debug.Log("Playing " + Name + " on target.");
        PlayEffect.Activate(target);
        */
    }

    public void UpgradeCard()
    {
        Attack *= 2;
        Defense *= 2;

        Level++;
    }

    public void RevertUpgrade()
    {
        Attack /= 2;
        Defense /= 2;

        Level--;
    }
}
