using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public int levelsUnlocked;
    public int gold;
    public int[] level = new int[30];
    public int[] attack = new int[30];
    public int[] defense = new int[30];
    // public Card[] deck = new Card[30];

    public SaveData(GameManager gm)
    {
        levelsUnlocked = gm.LevelsUnlocked;
        gold = gm.Gold;
        // deck = gm.Deck;
        
        for (int i = 0; i < gm.Deck.Count; i++)
        {
            level[i] = ((ElementCard)gm.Deck.GetCard(i)).Level;
            attack[i] = ((ElementCard)gm.Deck.GetCard(i)).Attack;
            defense[i] = ((ElementCard)gm.Deck.GetCard(i)).Defense;
        }
    }
}
