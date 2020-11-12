using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElementCardView : MonoBehaviour
{
    public bool enemyCard;
    [SerializeField] string _name;
    public string Name { get => _name; set => _name = value; }

    [SerializeField] TextMeshProUGUI _nameTextUI;
    [SerializeField] TextMeshProUGUI _levelTextUI;
    public int Level;
    int _levelBoost;
    int _originalLevel;
    [SerializeField] TextMeshProUGUI _attackText;
    public int Attack;
    int _originalAttack;
    int _attackBoost;
    [SerializeField] TextMeshProUGUI _defenseText;
    public int Defense;
    int _originalDefense;
    int _defenseBoost;

    [SerializeField] TextMeshProUGUI _descriptionText;

    [SerializeField] Image _elementSprite;
    [SerializeField] Image _elementSpriteBackground;

    [SerializeField] Image _cardBackground;
    [SerializeField] Sprite[] cardBordersForEnemies;

    ElementCard _elementCard;

    bool start = false;

    private void Awake()
    {

    }

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

        if (!start)
        {
            _originalLevel = Level;
            _originalAttack = Attack;
            _originalDefense = Defense;
            start = true;
        }

        _descriptionText.text = card.Description;

        _elementSprite.sprite = card.MonsterSprite;
        _elementSprite.color = card.MonsterColor;
        _elementSpriteBackground.sprite = card.SpriteBackground;
        _elementSpriteBackground.color = card.MonsterBackgroundColor;

        if (!enemyCard)
         _cardBackground.sprite = card.CardBackground;

        _elementCard = card;
    }

    public void EnemyBattleUpgrade()
    {
        enemyCard = true;

        _attackBoost = (GameManager._instance.CurrentLevel) * 1000;
        _defenseBoost = (GameManager._instance.CurrentLevel) * 1000;

        _levelBoost = (GameManager._instance.CurrentLevel);

        // set card background color
        if (GameManager._instance.CurrentLevel >= 6)
        {
            _cardBackground.sprite = cardBordersForEnemies[2];
        }
        else if (GameManager._instance.CurrentLevel >= 3)
        {
            _cardBackground.sprite = cardBordersForEnemies[1];
        }
        else
        {
            _cardBackground.sprite = cardBordersForEnemies[0];
        }
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

    public void RevertStatChange()
    {
        Attack = _originalAttack;
        Defense = _originalDefense;
        Level = _originalLevel;

        _attackText.text = Attack.ToString();
        _defenseText.text = Defense.ToString();
        _levelTextUI.text = Level.ToString();
    }

    public void PlayCardEffect()
    {
        _elementCard.Play();
    }
}
