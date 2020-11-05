using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewElementCard", 
                 menuName = "CardData/ElementCard")]
public class ElementCardData : ScriptableObject
{
    [SerializeField] string _name = "...";
    public string Name => _name;

    [SerializeField] int _level = 1;
    public int Level => _level;

    [SerializeField] int _attack = 0;
    public int Attack => _attack;

    [SerializeField] int _defense = 0;
    public int Defense => _defense;

    [SerializeField] string _description = "";
    public string Description => _description;

    [SerializeField] Sprite _sprite = null;
    public Sprite Sprite => _sprite;

    [SerializeField] Sprite _spriteBackground = null;
    public Sprite SpriteBackground => _spriteBackground;

    [SerializeField] Sprite _cardBackground = null;
    public Sprite CardBackground => _cardBackground;

    [SerializeField] ElementCardData[] _fusionCombinations = null;
    public ElementCardData[] FusionCombinations => _fusionCombinations;

    [SerializeField] ElementCardData[] _fusionMonsters = null;
    public ElementCardData[] FusionMonsters => _fusionMonsters;

    [SerializeField] CardPlayEffect _cardEffect;
    public CardPlayEffect CardEffect => _cardEffect;
}
