using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementCardView : MonoBehaviour
{
    [SerializeField] string _name;
    public string Name { get => _name; set => _name = value; }

    [SerializeField] TextMeshProUGUI _nameTextUI;
    [SerializeField] TextMeshProUGUI _levelTextUI;
    public int Level;
    int _levelBoost;
    [SerializeField] TextMeshProUGUI _attackText;
    public int Attack;
    int _attackBoost;
    [SerializeField] TextMeshProUGUI _defenseText;
    public int Defense;
    int _defenseBoost;

    [SerializeField] Image _elementSprite;
    [SerializeField] Image _elementSpriteBackground;
    [SerializeField] Image _elementCardBackground;

    ElementCard _elementCard;

    public void Display(ElementCard card)
    {
        Name = card.Name;
        _nameTextUI.text = card.Name;

        Level = card.Level + _levelBoost;
        _levelTextUI.text = Level.ToString();

        Attack = card.Attack + _attackBoost;
        _attackText.text = Attack.ToString();

        Defense = card.Defense + _defenseBoost;
        _defenseText.text = Defense.ToString();

        _elementSprite.sprite = card.Sprite;
        _elementSpriteBackground.sprite = card.SpriteBackground;
        _elementCardBackground.sprite = card.CardBackground;

        _elementCard = card;
    }

    public void EnemyBattleUpgrade()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();

        _attackBoost = (gameManager.LevelsUnlocked - 1) * 1000;
        _defenseBoost = (gameManager.LevelsUnlocked - 1) * 1000;

        _levelBoost = (gameManager.LevelsUnlocked - 1);
    }

    public void StatChange(int atkChange, int defChange, int lvlChange)
    {
        Attack = atkChange;
        Defense = defChange;
        Level = lvlChange;

        _attackText.text = Attack.ToString();
        _defenseText.text = Defense.ToString();
        _levelTextUI.text = Level.ToString();
    }

    public void PlayCardEffect()
    {
        _elementCard.Play();
    }
}
