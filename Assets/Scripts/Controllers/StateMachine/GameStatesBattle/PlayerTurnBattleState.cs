﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class PlayerTurnBattleState : BattleState
{
    [SerializeField] BattleManager _battleManager;

    [SerializeField] GraphicRaycaster _raycaster;
    PointerEventData _pointerEventData;
    [SerializeField] EventSystem _eventSystem;

    [SerializeField] GameObject tutorialPanel;

    [Header("Fusion Card Settings")]
    [SerializeField] ElementCardView _fusionCardView;
    [SerializeField] GameObject _combatButtonObj;
    [SerializeField] GameObject _cancelButtonObj1;
    [SerializeField] GameObject _fuseButtonObj;
    [SerializeField] GameObject _cancelButtonObj2;
    [SerializeField] float _durationMoveOut;
    [SerializeField] float _durationMoveIn;

    [Header("Battle Settings")]
    [SerializeField] Transform _playerBattlestandbyPos;

    [Header("Animation Settings")]
    [SerializeField] float _normFactor;
    [SerializeField] float _shrinkFactor;
    [SerializeField] float _growthFactor;
    [SerializeField] float _duration;

    [Header("Visual Settings")]
    [SerializeField] TextMeshProUGUI _playerTurnText;

    bool _selectingFirstCard = true;
    bool _selectingSecondCard = false;
    bool _finishedPlayerAction = false;
    public int _selectedCardIndex1 = -2;
    public int _selectedCardIndex2 = -2;
    int[] _fusionCombinationIndexes = new int[5];
    int _fusionCombinationIndex;

    GameObject _selectedMonster;
    public GameObject SelectedMonster => _selectedMonster;

    GameManager _gameManager;
    AudioSource _audioSource;
    [SerializeField] AudioClip moveCardSFX;
    [SerializeField] float moveCardPitch;
    [SerializeField] float moveCardVolume;

    SoundEffects soundEffects;

    private void Start()
    {

    }

    public override void Enter()
    {
        soundEffects = FindObjectOfType<SoundEffects>();
        _gameManager = FindObjectOfType<GameManager>();
        _audioSource = GetComponentInParent<AudioSource>();

        StartCoroutine(ShowPlayerTurnText());
    }

    public override void Tick()
    {
        if ((_selectedCardIndex1 == -1 || _selectedCardIndex2 == -1) && _gameManager.finishedTutorial)
        {
            CheckForSelection();
        }
    }

    public override void Exit()
    {

    }

    IEnumerator ShowPlayerTurnText()
    {
        if (!_gameManager.finishedTutorial)
        {
            ShowTutorialPanel();
        }

        while (tutorialPanel.activeInHierarchy)
            yield return null;

        _playerTurnText.text = "Player Turn";
        _playerTurnText.gameObject.SetActive(true);
        _playerTurnText.DOFade(1, 0.25f);

        soundEffects.PlayPlayerTurnSound();

        yield return new WaitForSeconds(1f);

        _playerTurnText.DOFade(0, 0.25f);

        yield return new WaitForSeconds(0.25f);

        _playerTurnText.gameObject.SetActive(false);
    }

    void ShowTutorialPanel()
    {
        tutorialPanel.SetActive(true);
    }

    public void TurnOffTutorialPanel()
    {
        _gameManager.finishedTutorial = true;
        tutorialPanel.SetActive(false);
    }

    void CheckForSelection()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // set up new Pointer Event
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();

            // raycast using the graphics raycaster and mouse click position
            _raycaster.Raycast(_pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                Slot slot = result.gameObject.GetComponentInParent<Slot>();

                // if player selects a card
                if (slot != null)
                {
                    int index = int.Parse(slot.name);
                    SelectCardCoroutine(index, _selectingFirstCard);

                    soundEffects.PlayArrowSound();
                }
                // let combat button be clicked if it has been
                else if (result.gameObject.GetComponentInParent<Button>() != null)
                {

                }
                // if player did not select a card, reset player hand positioning
                else
                {
                    ResetBothEnds();
                }
                break;
            }
        }
    }

    void StartBattleState()
    {
        StateMachine.ChangeState<FightBattleState>();
    }

    public void ResetBothEnds()
    {
        ResetFrontEnd();
        ResetBackEnd();
    }

    void ResetFrontEnd()
    {
        for (int i = 0; i < _battleManager.PlayerHandPositions.Length; i++)
        {
            // reset all cards if player hasn't finalized his selection
            if (!_finishedPlayerAction)
            {
                if (_battleManager.PlayerHandList[i] != null)
                {
                    // reset positioning and scale
                    CardMovement cardMovement = _battleManager.PlayerHandList[i].GetComponent<CardMovement>();
                    if (cardMovement != null)
                    {
                        cardMovement.TargetTransform = _battleManager.PlayerHandPositions[i];
                        cardMovement.transform.SetParent(_battleManager.PlayerHandPositions[i]);
                    }

                    _battleManager.PlayerShrinkPositions[i].rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    _battleManager.PlayerHandList[i].transform.DOScale(_normFactor, _duration);

                    // reset glow
                    GameObject glowImageObj = _battleManager.PlayerHandList[i];
                    Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                    glowImage.enabled = false;
                }
            }
            // else reset all remaining cards in player's hand 
            else
            {
                if (i != _selectedCardIndex1 && i != _selectedCardIndex2)
                {
                    // reset positioning and scale
                    CardMovement cardMovement = _battleManager.PlayerHandList[i].GetComponent<CardMovement>();
                    cardMovement.TargetTransform = _battleManager.PlayerHandPositions[i];
                    cardMovement.transform.SetParent(_battleManager.PlayerHandPositions[i]);

                    _battleManager.PlayerShrinkPositions[i].rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    _battleManager.PlayerHandList[i].transform.DOScale(_normFactor, _duration);

                    // reset glow
                    GameObject glowImageObj = _battleManager.PlayerHandList[i];
                    Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                    glowImage.enabled = false;
                }
            }
        }

        // turn off fusion monster button and cancel button
        _fuseButtonObj.SetActive(false);
        _cancelButtonObj2.SetActive(false);
        _combatButtonObj.SetActive(false);
        _cancelButtonObj1.SetActive(false);
    }

    void ResetBackEnd()
    {
        if (_selectedCardIndex1 >= 0)
        {
            FadeFusionCardForFusionCombination(false);
            ShowFusionMonster(false);
        }

        // reset backend variables
        _selectingFirstCard = true;
        _selectingSecondCard = false;
        _selectedCardIndex1 = -1;
        _selectedCardIndex2 = -1;

        _finishedPlayerAction = false;
    }

    // SelectManager calls this function based on card slot picked
    void SelectCardCoroutine(int index, bool first)
    {
        if (first)
            _selectedCardIndex1 = index;
        else
            _selectedCardIndex2 = index;

        // shrink cards in playing hand
        ShrinkPlayerHandCards();

        // move selected cards to playing field
        MoveSelectedPlayerHandCardToField();

        // show fusion combination
        if (_selectingSecondCard)
        {
            _selectingSecondCard = false;

            ShowFusionCombinationButton();
        }
        // check for fusion combinations
        else if (_selectingFirstCard)
        {
            _selectingFirstCard = false;
            _selectingSecondCard = true;

            CheckForFusionCombinations();
            ShowFightButton();
        }

        // RemoveCard();
    }

    void ShrinkPlayerHandCards()
    {
        // if player selected first card, shrink the other cards in the hand
        if (_selectingFirstCard)
        {
            for (int i = 0; i < _battleManager.PlayerShrinkPositions.Length; i++)
            {
                if (_battleManager.PlayerHandList[i] != null)
                {
                    CardMovement cardMovement = _battleManager.PlayerHandList[i].GetComponent<CardMovement>();
                    if (cardMovement != null)
                    {
                        cardMovement.TargetTransform = _battleManager.PlayerShrinkPositions[i];
                        cardMovement.transform.SetParent(_battleManager.PlayerShrinkPositions[i]);
                    }

                    _battleManager.PlayerHandList[i].transform.DOScale(_shrinkFactor, _duration);
                }
            }
        }
    }

    void MoveSelectedPlayerHandCardToField()
    {
        // if player selected first card, move it to playing field slot 1
        if (_selectingFirstCard)
        {
            CardMovement cardMovement = _battleManager.PlayerHandList[_selectedCardIndex1].GetComponent<CardMovement>();
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.PlayerPlayingCardPositions[0];
                cardMovement.transform.SetParent(_battleManager.PlayerPlayingCardPositions[0]);
            }

            _battleManager.PlayerHandList[_selectedCardIndex1].transform.DOScale(_growthFactor, _duration);
        }
        // if player selected first card, move it to playing field slot 2
        else if (_selectingSecondCard)
        {
            CardMovement cardMovement = _battleManager.PlayerHandList[_selectedCardIndex2].GetComponent<CardMovement>();
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.PlayerPlayingCardPositions[1];
                cardMovement.transform.SetParent(_battleManager.PlayerPlayingCardPositions[1]);
            }

            _battleManager.PlayerHandList[_selectedCardIndex2].transform.DOScale(_growthFactor, _duration);

            // turn off all glow images
            for (int i = 0; i < _battleManager.PlayerHandList.Count; i++)
            {
                GameObject glowImageObj = _battleManager.PlayerHandList[i];
                Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                glowImage.enabled = false;
            }

            FadeFusionCardForFusionCombination(true);
        }
    }

    void FadeFusionCardForFusionCombination(bool up)
    {
        if (up)
        {
            /*
            GameObject card1 = _battleManager.PlayerHandList[_selectedCardIndex1];
            Image[] card1Images = card1.GetComponentsInChildren<Image>();

            foreach (Image image in card1Images)
            {
                image.DOFade(0.5f, 0.2f);
            }

            GameObject card2 = _battleManager.PlayerHandList[_selectedCardIndex2];
            Image[] card2Images = card2.GetComponentsInChildren<Image>();

            foreach (Image image in card2Images)
            {
                image.DOFade(0.5f, 0.2f);
            }
            */

            GameObject fusionCard = _fusionCardView.gameObject;
            Image[] cardImages = fusionCard.GetComponentsInChildren<Image>();
            // TextMeshProUGUI[] texts = fusionCard.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (Image image in cardImages)
            {
                image.CrossFadeAlpha(0.5f, 0.2f, true);
            }

            /*
            foreach (TextMeshProUGUI text in texts)
            {
                text.CrossFadeAlpha(0.33f, 0.2f, true);
            }
            */
        }
        else
        {
            /*
            if (_selectedCardIndex1 != -1)
            {
                GameObject card1 = _battleManager.PlayerHandList[_selectedCardIndex1];
                Image[] card1Images = card1.GetComponentsInChildren<Image>();

                foreach (Image image in card1Images)
                {
                    image.DOFade(1f, 0.2f);
                }
            }

            if (_selectedCardIndex2 != -1)
            {
                GameObject card2 = _battleManager.PlayerHandList[_selectedCardIndex2];
                Image[] card2Images = card2.GetComponentsInChildren<Image>();

                foreach (Image image in card2Images)
                {
                    image.DOFade(1f, 0.2f);
                }
            }
            */
            GameObject fusionCard = _fusionCardView.gameObject;
            Image[] cardImages = fusionCard.GetComponentsInChildren<Image>();
            // TextMeshProUGUI[] texts = fusionCard.GetComponentsInChildren<TextMeshProUGUI>();

            foreach (Image image in cardImages)
            {
                image.CrossFadeAlpha(1f, 0f, true);
            }

            /*
            foreach (TextMeshProUGUI text in texts)
            {
                text.CrossFadeAlpha(1, 0, true);
            }
            */
        }
    }

    void CheckForFusionCombinations()
    {
        ElementCard currentCard = (ElementCard)_battleManager.PlayerHand.GetCard(_selectedCardIndex1);

        for (int i = 0; i < _battleManager.PlayerHandList.Count; i++)
        {
            if (i != _selectedCardIndex1)
            {
                ElementCard elementCard = (ElementCard)_battleManager.PlayerHand.GetCard(i);
                bool isACombination = false;

                // turn on glow backgrounds of all cards in player hand that can fuse together with the currently selected card
                for (int j = 0; j < elementCard.FusionCombinations.Length; j++)
                {
                    if (currentCard.Name == elementCard.FusionCombinations[j].Name)
                    {
                        GameObject glowImageObj = _battleManager.PlayerHandList[i];
                        Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                        glowImage.enabled = true;

                        isACombination = true;

                        _fusionCombinationIndexes[i] = j;
                        _fusionCombinationIndex = i;

                        break;
                    }
                }

                // if the card cannot be fused with currently selected card, flip it
                if (!isACombination)
                {
                    CardMovement cardMovement = _battleManager.PlayerHandList[i].GetComponent<CardMovement>();
                    if (cardMovement != null)
                    {
                        Quaternion flippedOver = _battleManager.PlayerHandPositions[i].transform.rotation;
                        flippedOver = Quaternion.Euler(new Vector3(flippedOver.x, 180, flippedOver.z));
                        cardMovement.TargetTransform.rotation = flippedOver;
                    }
                }
            }
        }
    }

    void ShowFightButton()
    {
        _combatButtonObj.SetActive(true);
        _cancelButtonObj1.SetActive(true);
    }

    void ShowFusionCombinationButton()
    {
        _combatButtonObj.SetActive(false);
        _cancelButtonObj1.SetActive(false);
        _fuseButtonObj.SetActive(true);
        _cancelButtonObj2.SetActive(true);

       ShowFusionMonster(true);
    }

    public void StartFusionCoroutine()
    {
        _finishedPlayerAction = true;

        _fuseButtonObj.SetActive(false);
        _cancelButtonObj2.SetActive(false);

        StartCoroutine(FusionCoroutine());
    }

    IEnumerator FusionCoroutine()
    {
        // animating fusion
        Sequence fusionSequence1 = DOTween.Sequence();
        Sequence fusionSequence2 = DOTween.Sequence();

        // move out
        fusionSequence1.Append(_battleManager.PlayerPlayingCardPositions[0].transform.DOLocalMoveX
            (_battleManager.PlayerPlayingCardPositions[2].transform.localPosition.x, _durationMoveOut));
        fusionSequence2.Append(_battleManager.PlayerPlayingCardPositions[1].transform.DOLocalMoveX
            (_battleManager.PlayerPlayingCardPositions[3].transform.localPosition.x, _durationMoveOut));

        // move in
        fusionSequence1.Append(_battleManager.PlayerPlayingCardPositions[0].transform.DOLocalMoveX
            (_battleManager.PlayerPlayingCardPositions[6].transform.localPosition.x, _durationMoveIn));
        fusionSequence2.Append(_battleManager.PlayerPlayingCardPositions[1].transform.DOLocalMoveX
            (_battleManager.PlayerPlayingCardPositions[6].transform.localPosition.x, _durationMoveIn));

        fusionSequence1.Play();
        fusionSequence2.Play();

        yield return new WaitForSeconds(_durationMoveOut + _durationMoveIn);

        FadeFusionCardForFusionCombination(false);

        yield return new WaitForSeconds(0.25f);

        // hide selected cards
        _battleManager.PlayerHandList[_selectedCardIndex1].SetActive(false);
        _battleManager.PlayerHandList[_selectedCardIndex2].SetActive(false);

        // reset positioning
        _battleManager.PlayerPlayingCardPositions[0].transform.position = _battleManager.PlayerPlayingCardPositions[4].transform.position;
        _battleManager.PlayerPlayingCardPositions[1].transform.position = _battleManager.PlayerPlayingCardPositions[5].transform.position;

        StartMoveCardToBattlePosCoroutine();
    }

    void ShowFusionMonster(bool show)
    {
        if (show)
        {
            ElementCard firstCard = (ElementCard)_battleManager.PlayerHand.GetCard(_selectedCardIndex1);
            ElementCard currentCard = (ElementCard)_battleManager.PlayerHand.GetCard(_selectedCardIndex2);
            ElementCard fusionCard = new ElementCard(currentCard.FusionMonsters[_fusionCombinationIndexes[_selectedCardIndex2]]);

            fusionCard.SetFusionMonsterStats(firstCard.Attack, currentCard.Attack, firstCard.Defense, currentCard.Defense, firstCard.Level, currentCard.Level);

            _fusionCardView.Display(fusionCard);
            _fusionCardView.gameObject.SetActive(true);
        }
        else
        {
            _fusionCardView.gameObject.SetActive(false);
        }
    }

    public void StartMoveCardToBattlePosCoroutine()
    {
        _combatButtonObj.SetActive(false);
        _cancelButtonObj1.SetActive(false);

        // stop first card from being sent back to hand if player presses cancel during battle transition
        if (_selectedCardIndex2 == -1)
            _selectedCardIndex2 = -2;

        StartCoroutine(MoveCardToBattlePosCoroutine());
    }

    IEnumerator MoveCardToBattlePosCoroutine()
    {
        // send first card to battle
        if (_selectedCardIndex2 < 0)
        {
            CardMovement cardMovement = _battleManager.PlayerHandList[_selectedCardIndex1].GetComponent<CardMovement>();
            cardMovement.TargetTransform = _playerBattlestandbyPos;   
            cardMovement.gameObject.transform.DOScale(_growthFactor, _duration);

            // make other cards disappear
            for (int i = 0; i < _battleManager.PlayerHandList.Count; i++)
            {
                if (i != _selectedCardIndex1)
                {
                    _battleManager.PlayerHandList[i].gameObject.SetActive(false);
                }
            }

            _selectedMonster = _battleManager.PlayerHandList[_selectedCardIndex1];
            _selectedMonster.tag = "Monster";
        }
        // send fusion card to battle
        else
        {
            CardMovement cardMovement = _fusionCardView.GetComponent<CardMovement>();
            cardMovement.TargetTransform = _playerBattlestandbyPos;

            // make other cards disappear
            for (int i = 0; i < _battleManager.PlayerHandList.Count; i++)
            {
                _battleManager.PlayerHandList[i].gameObject.SetActive(false);
            }

            _selectedMonster = _fusionCardView.gameObject;
            _selectedMonster.tag = "FusionMonster";
        }

        yield return new WaitForSeconds(0.75f);

        StartBattleState();
    }

     public void RemoveSelectedCards()
    {
        if (_selectedCardIndex1 != -1)
        {
            // front end- remove selected card from player hand to discard
            GameObject selectedCard = _battleManager.PlayerHandList[_selectedCardIndex1];
            selectedCard.transform.SetParent(_battleManager.DiscardPosition);
            selectedCard.transform.DOScale(_normFactor, _duration);
            selectedCard.SetActive(true);

            CardMovement cardMovement1 = selectedCard.GetComponent<CardMovement>();
            if (cardMovement1 != null)
            {
                cardMovement1.TargetTransform = _battleManager.DiscardPosition;
            }

            _battleManager.PlayerHandList[_selectedCardIndex1] = null;
            _battleManager.DiscardList.Add(selectedCard);

            // back end- remove selected card from player hand to discard
            Card card = _battleManager.PlayerHand.GetCard(_selectedCardIndex1);
            _battleManager.Discard.Add(card, DeckPosition.Top);
            _battleManager.PlayerHand.LazyRemove(_selectedCardIndex1);
        }

        if (_selectedCardIndex2 == -2)
            _selectedCardIndex2 = -1;

        if (_selectedCardIndex2 != -1)
        {
            // front end- remove selected card from player hand to discard
            GameObject selectedCard = _battleManager.PlayerHandList[_selectedCardIndex2];
            selectedCard.transform.SetParent(_battleManager.DiscardPosition);
            selectedCard.transform.DOScale(_normFactor, _duration);
            selectedCard.SetActive(true);

            CardMovement cardMovement2 = selectedCard.GetComponent<CardMovement>();
            if (cardMovement2 != null)
            {
                cardMovement2.TargetTransform = _battleManager.DiscardPosition;
            }

            _battleManager.PlayerHandList[_selectedCardIndex2] = null;
            _battleManager.DiscardList.Add(selectedCard);

            // back end- remove selected card from player hand to discard
            Card card = _battleManager.PlayerHand.GetCard(_selectedCardIndex2);
            _battleManager.Discard.Add(card, DeckPosition.Top);
            _battleManager.PlayerHand.LazyRemove(_selectedCardIndex2);
        }

        ResetShrinkAndPositoning();
    }

    void ResetShrinkAndPositoning()
    {
        for (int i = 0; i < _battleManager.PlayerShrinkPositions.Length; i++)
        {
            if (_battleManager.PlayerHandList[i] != null)
            {
                CardMovement cardMovement = _battleManager.PlayerHandList[i].GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.TargetTransform = _battleManager.PlayerHandPositions[i];
                    cardMovement.transform.SetParent(_battleManager.PlayerHandPositions[i]);
                    cardMovement.gameObject.SetActive(true);

                    GameObject glowImageObj = cardMovement.gameObject;
                    Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                    glowImage.enabled = false;
                }

                _battleManager.PlayerHandList[i].transform.DOScale(_normFactor, _duration);
            }
        }
    }

    public void ResetFusionMonsterPositioning()
    {
        _fusionCardView.gameObject.transform.localPosition = Vector3.zero;
        _fusionCardView.gameObject.GetComponent<CardMovement>().TargetTransform = null;
    }
}
