using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCardView : MonoBehaviour
{
    [SerializeField] string _name;
    public string Name { get => _name; set => _name = value; }

    [SerializeField] TextMeshProUGUI _nameTextUI;
    [SerializeField] TextMeshProUGUI _levelTextUI;
    [SerializeField] TextMeshProUGUI _attackText;
    [SerializeField] TextMeshProUGUI _defenseText;

    [SerializeField] Image _spellSprite;
    [SerializeField] Image _spellSpriteBackground;
    [SerializeField] Image _spellCardBackground;

    public void Display(SpellCard spellCard)
    {
        Name = spellCard.Name;

        _nameTextUI.text = spellCard.Name;
        _levelTextUI.text = spellCard.Level.ToString();
        _attackText.text = spellCard.AttackBuff.ToString();
        _defenseText.text = spellCard.DefenseBuff.ToString();

        _spellSprite.sprite = spellCard.Sprite;
        _spellSpriteBackground.sprite = spellCard.SpriteBackground;
        _spellCardBackground.sprite = spellCard.CardBackground;
    }
}
