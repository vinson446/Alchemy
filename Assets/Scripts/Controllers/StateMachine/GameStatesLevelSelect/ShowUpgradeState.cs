using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class ShowUpgradeState : LevelSelectState
{
    [SerializeField] int _upgradeMultiplier;

    [Header("Panels")]
    [SerializeField] GameObject _deckPanel;
    [SerializeField] GameObject _upgradePanel;

    [Header("Hookups")]
    [SerializeField] int _cardInDeckIndex;
    public int CardInDeckIndex { get => _cardInDeckIndex; set => _cardInDeckIndex = value; }
    [SerializeField] ElementCardView _selectedCardObj;
    public ElementCardView SelectedCardObj { get => _selectedCardObj; set => _selectedCardObj = value; }
    [SerializeField] ElementCardView _upgradedCardObj;
    public ElementCardView UpgradedCardObj { get => _upgradedCardObj; set => _upgradedCardObj = value; }

    [SerializeField] TextMeshProUGUI _levelText;
    [SerializeField] TextMeshProUGUI _costText;
    int _cost;
    [SerializeField] TextMeshProUGUI _goldText;
    int _gold;

    [SerializeField] GameObject noMoneyPopup;
    [SerializeField] Transform noMoneyPos;

    ShowDeckState _showDeckState;
    GameManager _gameManager;

    SoundEffects soundEffects;

    private void Start()
    {

    }

    public override void Enter()
    {
        soundEffects = FindObjectOfType<SoundEffects>();
        soundEffects.PlayClickSound();

        _showDeckState = GetComponent<ShowDeckState>();

        _gameManager = FindObjectOfType<GameManager>();

        _deckPanel.SetActive(true);
        _upgradePanel.SetActive(true);

        ShowSelectedCard();
        ShowUpgradedCard();
        ShowGoldStuff();

        StateMachine.Input.PressedViewDeck += OnPressedDeck;
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {
        StateMachine.Input.PressedViewDeck -= OnPressedDeck;

        _upgradePanel.SetActive(false);
    }

    void ShowSelectedCard()
    {
        ElementCard card = (ElementCard)_gameManager.Deck.GetCard(_cardInDeckIndex);
        _selectedCardObj.Display(card);
    }

    void ShowUpgradedCard()
    {
        ElementCard card = (ElementCard)_gameManager.Deck.GetCard(_cardInDeckIndex);
        card.UpgradeCard();

        _upgradedCardObj.Display(card);

        card.RevertUpgrade();
    }
    
    void ShowGoldStuff()
    {
        _cost = int.Parse(_levelText.text) * _upgradeMultiplier;
        _costText.text = "Cost: " + _cost.ToString(); ;

        _gold = _gameManager.Gold;
        _goldText.text = _gold.ToString();
    }

    void OnPressedUpgrade()
    {
        if (_gold >= _cost)
        {
            // decrement gold
            _gameManager.IncrementGold(-_cost);

            // upgrade card
            ElementCard card = (ElementCard)_gameManager.Deck.GetCard(_cardInDeckIndex);
            card.UpgradeCard();
            
            _gameManager.Deck.SetCard(card, _cardInDeckIndex);

            soundEffects.PlayUpgradeSound();

            GameManager._instance.SaveGame();

            OnPressedDeck();
        }
        else
        {
            soundEffects.PlayNoMoneySound();

            GameObject noMoneyText = Instantiate(noMoneyPopup, noMoneyPos.position, Quaternion.identity);
            noMoneyText.transform.SetParent(_deckPanel.transform);
            noMoneyText.GetComponent<DamagePopup>().SetupMessage("Not enough gold", 3);

            OnPressedDeck();
        }
    }

    void OnPressedDeck()
    {
        StateMachine.ChangeState<ShowDeckState>();
    }
}
