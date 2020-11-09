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

    ElementCardData elementCardData;
    public ElementCardData ElementCardData { get => elementCardData; set => elementCardData = value; }

    public ElementCard(ElementCardData Data)
    {
        // Card variables
        Name = Data.Name;
        Level = Data.Level;
        Description = Data.Description;
        Attack = Data.Attack;
        Defense = Data.Defense;

        MonsterSprite = Data.MonsterSprite;
        MonsterColor = Data.MonsterColor;
        SpriteBackground = Data.SpriteBackground;
        MonsterBackgroundColor = Data.MonsterBackgroundColor;

        CardBackground = Data.CardBackground[0];

        FusionCombinations = Data.FusionCombinations;
        FusionMonsters = Data.FusionMonsters;

        CardEffect = Data.CardEffect;

        elementCardData = Data;
    }

    public override void Play()
    {
        CardEffect.Activate();
    }

    public void UpgradeCard()
    {
        Attack += Level * 500;
        Defense += Level * 500;

        Level++;

        if (Level >= 6)
        {
            CardBackground = elementCardData.CardBackground[2];
        }
        else if (Level >= 3)
        {
            CardBackground = elementCardData.CardBackground[1];
        }
        else
        {
            CardBackground = elementCardData.CardBackground[0];
        }
    }

    public void RevertUpgrade()
    {
        Attack /= 2;
        Defense /= 2;

        Level--;

        if (Level >= 6)
        {
            CardBackground = elementCardData.CardBackground[2];
        }
        else if (Level >= 3)
        {
            CardBackground = elementCardData.CardBackground[1];
        }
        else
        {
            CardBackground = elementCardData.CardBackground[0];
        }
    }

    public void SetFusionMonsterStats(int attack1, int attack2, int defense1, int defense2, int level1, int level2)
    {
        Attack = attack1 + attack2;
        Defense = defense1 + defense2;

        Level = level1 + level2;
    }
}
