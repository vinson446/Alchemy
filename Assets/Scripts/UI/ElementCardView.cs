﻿using System.Collections;
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
    [SerializeField] TextMeshProUGUI _attackText;
    public int Attack;
    [SerializeField] TextMeshProUGUI _defenseText;
    public int Defense;

    [SerializeField] Image _elementSprite;
    [SerializeField] Image _elementSpriteBackground;
    [SerializeField] Image _elementCardBackground;

    public void Display(ElementCard card)
    {
        Name = card.Name;

        _nameTextUI.text = card.Name;
        _levelTextUI.text = card.Level.ToString();
        _attackText.text = card.Attack.ToString();
        Attack = card.Attack;
        _defenseText.text = card.Defense.ToString();
        Defense = card.Defense;

        _elementSprite.sprite = card.Sprite;
        _elementSpriteBackground.sprite = card.SpriteBackground;
        _elementCardBackground.sprite = card.CardBackground;
    }
}
