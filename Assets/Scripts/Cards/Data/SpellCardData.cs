using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpellCard", 
                 menuName = "CardData/SpellCard")]
public class SpellCardData : ScriptableObject
{
    [SerializeField] string _name = "...";
    public string Name => _name;

    [SerializeField] int _level = 1;
    public int Level => _level;

    [SerializeField] int _attackBuff = 0;
    public int AttackBuff => _attackBuff;

    [SerializeField] int _defenseBuff = 0;
    public int DefenseBuff => _defenseBuff;

    [SerializeField] string _description = "";
    public string Description => _description;

    [SerializeField] Sprite _sprite = null;
    public Sprite Sprite => _sprite;

    [SerializeField] Sprite _spriteBackground = null;
    public Sprite SpriteBackground => _spriteBackground;

    [SerializeField] Sprite _cardBackground = null;
    public Sprite CardBackground => _cardBackground;

    [SerializeField] CardPlayEffect _playEffect = null;
    public CardPlayEffect PlayEffect => _playEffect;
}
