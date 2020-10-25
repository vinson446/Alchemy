﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class PlayerTurnBattleState : BattleState
{
    [SerializeField] BattleManager _battleManager;

    // normal raycasts do not work on UI elements, they require a special kind
    [SerializeField] GraphicRaycaster _raycaster;
    PointerEventData _pointerEventData;
    [SerializeField] EventSystem _eventSystem;

    [Header("Selection Settings")]
    [SerializeField] float _shrinkFactor;
    [SerializeField] float _growthFactor;
    [SerializeField] float _duration;

    [Header("Fusion Card Settings")]
    [SerializeField] ElementCardView _fusionCardView;
    [SerializeField] GameObject _fuseButtonObj;
    [SerializeField] float _xMovePos;
    [SerializeField] float _durationMoveOut;
    [SerializeField] float _durationMoveIn;

    public bool _selectingFirstCard = true;
    public bool _selectingSecondCard = false;
    public bool _finishedPlayerAction = false;
    public int _selectedCardIndex1 = -2;
    public int _selectedCardIndex2 = -2;
    int[] _fusionCombinationIndexes = new int[5];
    int _fusionCombinationIndex;

    public override void Enter()
    {
        Debug.Log("Player Turn: Entering");

        Draw();
    }

    public override void Tick()
    {
        if (_selectedCardIndex1 == -1 || _selectedCardIndex2 == -1)
            CheckForSelection();
    }

    public override void Exit()
    {
        Debug.Log("Player Turn: Exiting");
    }

    void StartEnemyTurnBattleState()
    {
        StateMachine.ChangeState<EnemyTurnBattleState>();
    }

    void Draw()
    {
        StartCoroutine(DrawCoroutine());
    }

    IEnumerator DrawCoroutine()
    {
        for (int i = 0; i < _battleManager.PlayerHandPositions.Length; i++)
        {
            if (_battleManager.BattleDeck.IsEmpty)
            {
                ShuffleDeck();

                yield return new WaitForSeconds(2f);
            }

            if (!_battleManager.BattleDeck.IsEmpty && _battleManager.PlayerHandPositions[i].transform.childCount == 0)
            {
                // front end- get top card of deck, then move card to an empty hand slot
                GameObject cardDrawn = _battleManager.DeckList[_battleManager.BattleDeck.LastIndex];
                _battleManager.PlayerHandList[i] = cardDrawn;

                cardDrawn.transform.parent = _battleManager.PlayerHandPositions[i].transform;

                CardMovement cardMovement = cardDrawn.GetComponent<CardMovement>();
                if (cardMovement != null)
                {
                    cardMovement.TargetTransform = _battleManager.PlayerHandPositions[i].transform;
                }

                _battleManager.DeckList.Remove(cardDrawn);

                yield return new WaitForSeconds(0.2f);

                // back end- draw from battle deck to player hand
                Card newCard = _battleManager.BattleDeck.Draw(DeckPosition.Top);
                _battleManager.PlayerHand.LazyAdd(newCard, i);
            }
        }

        ResetBackEnd();
    }

    IEnumerator MoveCardsInDiscardToDeck()
    {
        int count = _battleManager.Discard.Count;
        for (int i = 0; i < count; i++)
        {
            // front end- add cards in discard to deck
            CardMovement cardMovement = _battleManager.DiscardList[0].GetComponent<CardMovement>();
            cardMovement.transform.parent = _battleManager.DeckPos;
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.DeckPos;
            }
            _battleManager.DeckList.Add(_battleManager.DiscardList[0]);
            _battleManager.DiscardList.Remove(_battleManager.DiscardList[0]);

            // back end- add cards in discard to deck
            _battleManager.BattleDeck.Add(_battleManager.Discard.GetCard(0));
            _battleManager.Discard.Remove(0);

            yield return new WaitForSeconds(0.2f);
        }

        // back end
        _battleManager.BattleDeck.Shuffle(_battleManager.DeckList);
        ShowAllCardsInDeck();
    }

    void ShuffleDeck()
    {
        // front end
        // TODO- cool shuffle animation
        StartCoroutine(MoveCardsInDiscardToDeck());
    }

    // display cards visually in battle deck
    void ShowAllCardsInDeck()
    {
        for (int i = 0; i < _battleManager.BattleDeck.Count; i++)
        {
            ElementCardView c = _battleManager.DeckList[i].GetComponent<ElementCardView>();
            ElementCard newCard = (ElementCard)_battleManager.BattleDeck.GetCard(i);
            c.Display(newCard);
        }
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
                    SelectCard(index, _selectingFirstCard);
                }
                // if player did not select a card, reset player hand positioning
                else
                {
                    ResetFrontEnd();
                    ResetBackEnd();

                    break;
                }
                break;
            }
        }
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
                        cardMovement.transform.parent = _battleManager.PlayerHandPositions[i];
                    }

                    _battleManager.PlayerShrinkPositions[i].rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    _battleManager.PlayerHandList[i].transform.DOScale(1f, 0.2f);

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
                    cardMovement.transform.parent = _battleManager.PlayerHandPositions[i];

                    _battleManager.PlayerShrinkPositions[i].rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    _battleManager.PlayerHandList[i].transform.DOScale(1f, 0.2f);

                    // reset glow
                    GameObject glowImageObj = _battleManager.PlayerHandList[i];
                    Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                    glowImage.enabled = false;
                }
            }
        }

        // turn off fusion monster button
        _fuseButtonObj.SetActive(false);
    }

    void ResetBackEnd()
    {
        // reset backend variables
        _selectingFirstCard = true;
        _selectingSecondCard = false;
        _selectedCardIndex1 = -1;
        _selectedCardIndex2 = -1;

        _finishedPlayerAction = false;
    }

    // SelectManager calls this function based on card slot picked
    void SelectCard(int index, bool first)
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
                        cardMovement.transform.parent = _battleManager.PlayerShrinkPositions[i];
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
                cardMovement.transform.parent = _battleManager.PlayerPlayingCardPositions[0];
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
                cardMovement.transform.parent = _battleManager.PlayerPlayingCardPositions[1];
            }

            _battleManager.PlayerHandList[_selectedCardIndex2].transform.DOScale(_growthFactor, _duration);

            // turn off all glow images
            for (int i = 0; i < _battleManager.PlayerHandList.Count; i++)
            {
                GameObject glowImageObj = _battleManager.PlayerHandList[i];
                Image glowImage = glowImageObj.transform.GetChild(0).gameObject.GetComponent<Image>();
                glowImage.enabled = false;
            }
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

    void ShowFusionCombinationButton()
    {
        _fuseButtonObj.SetActive(true);
    }

    public void StartFusionCoroutine()
    {
        _finishedPlayerAction = true;
        StartCoroutine(FusionCoroutine());
    }

    IEnumerator FusionCoroutine()
    {
        // animating fusion
        Sequence fusionSequence1 = DOTween.Sequence();
        Sequence fusionSequence2 = DOTween.Sequence();
        fusionSequence1.Append(_battleManager.PlayerPlayingCardPositions[0].transform.DOLocalMoveX
            (_battleManager.PlayerPlayingCardPositions[2].transform.localPosition.x, _durationMoveOut));
        fusionSequence2.Append(_battleManager.PlayerPlayingCardPositions[1].transform.DOLocalMoveX
            (_battleManager.PlayerPlayingCardPositions[3].transform.localPosition.x, _durationMoveOut));

        fusionSequence1.Append(_battleManager.PlayerPlayingCardPositions[0].transform.DOLocalMoveX(0, _durationMoveIn));
        fusionSequence2.Append(_battleManager.PlayerPlayingCardPositions[1].transform.DOLocalMoveX(0, _durationMoveIn));

        fusionSequence1.Play();
        fusionSequence2.Play();

        yield return new WaitForSeconds(_durationMoveOut + _durationMoveIn);

        // show fusion monster
        _battleManager.PlayerHandList[_selectedCardIndex1].SetActive(false);
        _battleManager.PlayerHandList[_selectedCardIndex2].SetActive(false);

        ElementCard currentCard = (ElementCard)_battleManager.PlayerHand.GetCard(_selectedCardIndex2);
        ElementCard fusionCard = new ElementCard(currentCard.FusionMonsters[_fusionCombinationIndexes[_selectedCardIndex2]]);

        _fusionCardView.Display(fusionCard);
        _fusionCardView.gameObject.SetActive(true);

        _battleManager.PlayerPlayingCardPositions[0].transform.position = _battleManager.PlayerPlayingCardPositions[4].transform.position;
        _battleManager.PlayerPlayingCardPositions[1].transform.position = _battleManager.PlayerPlayingCardPositions[5].transform.position;

        yield return new WaitForSeconds(1.5f);

        // clean up => transition to enemy turn
        _fusionCardView.gameObject.SetActive(false);

        ResetFrontEnd();
        RemoveSelectedCards();

        StartEnemyTurnBattleState();
    }

    void RemoveSelectedCards()
    {
        if (_selectedCardIndex1 != -1)
        {
            // front end- remove selected card from player hand to discard
            GameObject selectedCard = _battleManager.PlayerHandList[_selectedCardIndex1];
            selectedCard.transform.parent = _battleManager.DiscardPosition;
            selectedCard.transform.DOScale(1f, 0f);
            selectedCard.SetActive(true);

            CardMovement cardMovement = selectedCard.GetComponent<CardMovement>();
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.DiscardPosition;
            }

            _battleManager.PlayerHandList[_selectedCardIndex1] = null;
            _battleManager.DiscardList.Add(selectedCard);

            // back end- remove selected card from player hand to discard
            Card card = _battleManager.PlayerHand.GetCard(_selectedCardIndex1);
            _battleManager.Discard.Add(card, DeckPosition.Top);
            _battleManager.PlayerHand.LazyRemove(_selectedCardIndex1);
        }

        if (_selectedCardIndex2 != -1)
        {
            // front end- remove selected card from player hand to discard
            GameObject selectedCard = _battleManager.PlayerHandList[_selectedCardIndex2];
            selectedCard.transform.parent = _battleManager.DiscardPosition;
            selectedCard.transform.DOScale(1f, 0f);
            selectedCard.SetActive(true);

            CardMovement cardMovement = selectedCard.GetComponent<CardMovement>();
            if (cardMovement != null)
            {
                cardMovement.TargetTransform = _battleManager.DiscardPosition;
            }

            _battleManager.PlayerHandList[_selectedCardIndex2] = null;
            _battleManager.DiscardList.Add(selectedCard);

            // back end- remove selected card from player hand to discard
            Card card = _battleManager.PlayerHand.GetCard(_selectedCardIndex2);
            _battleManager.Discard.Add(card, DeckPosition.Top);
            _battleManager.PlayerHand.LazyRemove(_selectedCardIndex2);
        }
    }
}
